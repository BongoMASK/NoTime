public abstract class PuzzleComponent : PuzzleObject {

    /// <summary>
    /// Checks whether it should turn on or not
    /// </summary>
    public void CheckTrigger() {
        bool b = true;
        foreach (var x in puzzleObjList)
            b = b && x.isOn;

        // Done so that it does not continue calling isOn. Saves memory
        if (b != isOn)
            isOn = b;
    }

    public override void SwitchOn() {
        base.SwitchOn();
    }

    public override void SwitchOff() {
        base.SwitchOff();
    }
}