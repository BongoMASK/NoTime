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
        get => _isOn;

        set {
            _isOn = value;

            if (_isOn)
                SwitchOn();
            else
                SwitchOff();
        }
    }

    public List<PuzzleObject> puzzleObjList;

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
}
