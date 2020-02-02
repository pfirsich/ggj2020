using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Tooltip("Image component displaying health left")]
    public Image healthBarImage;
    [Tooltip("The floating healthbar pivot transform")]
    public Transform healthBarPivot;
    [Tooltip("Whether the health bar is visible when at full health or not")]
    public bool hideFullHealthBar = true;

    private MachineComponent machine;

    void Start()
    {
        machine = transform.parent.GetComponent<MachineComponent>();
    }
    void Update()
    {
        // update health bar value
        healthBarImage.fillAmount = machine.health / 1.0f;

        // rotate health bar to face the camera/player
        //healthBarPivot.LookAt(Camera.main.transform.position);

        // hide health bar if needed

    }
}
