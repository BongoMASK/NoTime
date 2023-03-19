using UnityEngine;

public class DoorTrigger : PuzzleObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Plate")
        {
            SwitchOn();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Plate")
        {
            SwitchOff();
        }
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
