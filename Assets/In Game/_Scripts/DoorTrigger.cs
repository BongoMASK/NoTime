using UnityEngine;

public class DoorTrigger : PhysicalButton {
    private void OnTriggerEnter(Collider other) {
        if (other.transform.name == "Plate") {
            isOn = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.name == "Plate") {
            isOn = false;
        }
    }

    public override void SwitchOn()
    {
        base.SwitchOn();

        // TODO:
        // Change colour of button
        // Change button path visual
    }

    public override void SwitchOff()
    {
        base.SwitchOff();

        // TODO:
        // Change colour of button
        // Change button path visual
    }
}
