using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MachineComponent : MonoBehaviour
{
    public struct AnimationParams
    {
        public float bounceFreq;
        public float bounceAmp;
        public float spinFreq;
        public int spinProb;
        public float swayAmp;

        public AnimationParams(float bounceFreq, float bounceAmp, float spinFreq, int spinProb, float swayAmp)
        {
            this.bounceFreq = bounceFreq;
            this.bounceAmp = bounceAmp;
            this.spinFreq = spinFreq;
            this.spinProb = spinProb;
            this.swayAmp = swayAmp;
        }
    };

    static AnimationParams[] animationParams = new AnimationParams[3]
    {
        new AnimationParams(4.0f, 0.1f, 90.0f, 100, 0.3f),
        new AnimationParams(8.0f, 0.3f, 90.0f, 80, 1.5f),
        new AnimationParams(8.0f, 0.5f, 120.0f, 40, 2.0f)
    };

    public float health = 1.0f;
    public List<ToolType> damageTypes = new List<ToolType>();
    public int brokenness = 0;
    public int animIndex = 0;

    private Quaternion startRot;
    private float timeOffset = 0;
    private float startY = 0;

    public AudioSource repairSound;
    public AudioSource breakSound;


    void Start()
    {
        timeOffset = UnityEngine.Random.Range(0.0f, 1.0f) * (2.0f * Mathf.PI);
        startY = transform.localPosition.y;
        startRot = transform.localRotation;

        var aSources = GetComponents<AudioSource>();
        //repairSound = aSources[0];
        //loadSound = aSources[1];
        //audio3 = aSources[2];
        repairSound = aSources[0];
        breakSound = aSources[1];
    }

    void Bounce(float freq, float amp, float swayAmp)
    {
        Vector3 pos = transform.localPosition;
        float t = 2.0f * Mathf.PI * freq * Time.time;
        pos.y = startY + Mathf.Abs(Mathf.Sin(t)) * amp;
        transform.localPosition = pos;
        transform.localRotation = startRot * Quaternion.Euler(Mathf.Sin(t) * swayAmp, 0, 0);
    }

    void SpinChildren(float freq, int prob)
    {
        foreach (Transform child in transform) {
            if (child.name.StartsWith("Spin_")) {
                if (UnityEngine.Random.Range(1, 100) < prob)
                    child.transform.localRotation *= Quaternion.Euler(freq * Time.deltaTime, 0, 0);
            }
        }
    }

    void rerollDamageTypes(int num)
    {
        damageTypes.Clear();
        for (int i = 0; i < num; ++i)
        {
            ToolType candidate;
            do
            {
                candidate = (ToolType)UnityEngine.Random.Range(0, 4);
            } while(damageTypes.Count > 0 && candidate == damageTypes[0]);
            damageTypes.Insert(0, candidate);
        }
    }

    public void gnaw(float amount)
    {
        int preBrokenness = getBrokenness();
        health = Mathf.Max(0.0f, health - amount);
        int newBrokenness = getBrokenness();
        if (newBrokenness != preBrokenness)
        {
            rerollDamageTypes(newBrokenness);
            //play broken
            breakSound.Play(0);
        }
    }

    public void repair(float amount)
    {
        int preBrokenness = getBrokenness();
        health = Mathf.Min(1.0f, health + amount);
        if (getBrokenness() != preBrokenness)
        {
            damageTypes.RemoveAt(0);
            repairSound.Play(0);
            //play repair
        }
    }

    public int getBrokenness()
    {
        if (health == 1.0f)
            return 0;
        else if (health >= 0.66666f) // should be < 1.0f!
            return 1;
        else if (health >= 0.33333f)
            return 2;
        else
            return 3;
    }

    void AnimateBrokenness(AnimationParams animParams)
    {
        Bounce(animParams.bounceFreq, animParams.bounceAmp, animParams.swayAmp);
        SpinChildren(animParams.spinFreq, animParams.spinProb);
    }

    // Update is called once per frame
    void Update()
    {
        brokenness = getBrokenness();
        animIndex = Math.Max(0, getBrokenness() - 1);
        AnimateBrokenness(animationParams[animIndex]);
    }
}
