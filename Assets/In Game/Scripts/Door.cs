using UnityEngine;

public class Door : PuzzleComponent
{
    [SerializeField] Animator anim;

    [Tooltip("Keeps door open at Start")]
    [SerializeField] bool isDoorAlreadyOpen = false;

    private void Start() {
        anim.SetBool("_openDoor", isDoorAlreadyOpen);
    }

    public override void SwitchOn()
    {
        base.SwitchOn();
        anim.SetBool("_openDoor", !isDoorAlreadyOpen);
    }

    public override void SwitchOff()
    {   
        base.SwitchOff();
        anim.SetBool("_openDoor", isDoorAlreadyOpen);
    }
}
