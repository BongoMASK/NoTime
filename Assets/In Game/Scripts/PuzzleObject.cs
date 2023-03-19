using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleObject : MonoBehaviour
{
    public bool isOn;
    public List<DoorTrigger> triggerList;
    public abstract void SwitchOn();
    public abstract void SwitchOff();

    public void CheckTrigger()
    {
        bool something = true;
        foreach (var x in triggerList)
        {
            something = something && x.isOn;
        }
        isOn = something;
    }
}
