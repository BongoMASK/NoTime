using UnityEngine;

public class Door : PuzzleObject
{
    [SerializeField] Animator _anim;

    private void Update()
    {
        CheckTrigger();

        if (isOn)
        {
            openeDoor();
        }
        if (!isOn)
        {
            closeDoor();
        }
    }


    public void openeDoor()
    {
        _anim.SetBool("_openDoor", true);
        SwitchOn();
    }
    public void closeDoor()
    {
        _anim.SetBool("_openDoor", false);
        SwitchOff();
    }


    public override void SwitchOn()
    {
        isOn = true;
    }

    public override void SwitchOff()
    {
        isOn = false;
    }
}
