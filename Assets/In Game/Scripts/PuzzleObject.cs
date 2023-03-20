using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleObject : MonoBehaviour
{
    private bool _isOn;

    /// <summary>
    /// Determines whether the object is switched on or off.
    /// SwitchOn() is called the frame it is turned on.
    /// SwitchOff() is called the frame it is turned off.
    /// </summary>
    public bool isOn {
        //get; set;
        get => _isOn;

        set {
            _isOn = value;

            if (_isOn)
                SwitchOn();
            else
                SwitchOff();
        }
    }

    public List<DoorTrigger> triggerList;

    /// <summary>
    /// Is called whenever isOn becomes true.
    /// Functions that override this must always call the base function.
    /// </summary>
    public virtual void SwitchOn() {
        _isOn = true;
    }

    /// <summary>
    /// Is called whenever isOn becomes false.
    /// Functions that override this must always call the base function.
    /// </summary>
    public virtual void SwitchOff() {
        _isOn = false;
    }

    /// <summary>
    /// Checks whether it should turn on or not
    /// </summary>
    public void CheckTrigger()
    {
        bool b = true;
        foreach (var x in triggerList)
            b = b && x.isOn;

        // Done so that it does not continue calling isOn. Saves memory
        if (b != isOn)
            isOn = b;
    }
}
