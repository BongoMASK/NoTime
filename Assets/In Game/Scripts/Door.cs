using UnityEngine;

public class Door : PuzzleObject
{
    [SerializeField] Animator anim;

    private void Update()
    {
        CheckTrigger();
    }

    public override void SwitchOn()
    {
        base.SwitchOn();
        anim.SetBool("_openDoor", true);
    }

    public override void SwitchOff()
    {   
        base.SwitchOff();
        anim.SetBool("_openDoor", false);
    }
}
