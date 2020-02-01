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
        new AnimationParams(4.0f, 0.15f, 90.0f, 100, 0.3f),
        new AnimationParams(8.0f, 0.3f, 90.0f, 80, 1.5f),
        new AnimationParams(8.0f, 0.4f, 120.0f, 40, 2.0f)
    };

    public float health = 1.0f;

    private float timeOffset = 0;
    private float startY = 0;

    void Start()
    {
        timeOffset = UnityEngine.Random.Range(0.0f, 1.0f) * (2.0f * Mathf.PI);
        startY = transform.localPosition.y;
    }

    void Bounce(float freq, float amp, float swayAmp)
    {
        Vector3 pos = transform.localPosition;
        float t = 2.0f * Mathf.PI * freq * Time.time;
        pos.y = startY + Mathf.Abs(Mathf.Sin(t)) * amp;
        transform.localPosition = pos;
        transform.localRotation *= Quaternion.Euler(Mathf.Sin(t) * swayAmp, 0, 0);
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

    public void gnaw(float amount)
    {
        health = Mathf.Max(0.0f, health - amount);
    }

    public void repair(float amount)
    {
        health = Mathf.Min(1.0f, health + amount);
    }

    int getBrokenness()
    {
        if (health <= 0.33333f)
            return 2;
        else if (health <= 0.66666f)
            return 1;
        else
            return 0;
    }

    void AnimateBrokenness(AnimationParams animParams)
    {
        Bounce(animParams.bounceFreq, animParams.bounceAmp, animParams.swayAmp);
        SpinChildren(animParams.spinFreq, animParams.spinProb);
    }

    // Update is called once per frame
    void Update()
    {
        AnimateBrokenness(animationParams[getBrokenness()]);
    }
}
