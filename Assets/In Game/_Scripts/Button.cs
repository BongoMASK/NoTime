public abstract class Button : PuzzleObject {

    /// <summary>
    /// Called to update the objects it is connected to check if their state should change
    /// </summary>
    public void UpdateConnectedObjects() {
        foreach (PuzzleObject item in puzzleObjList) {
            // Casting to PuzzleComponent
            PuzzleComponent p = (PuzzleComponent)item;
            p.CheckTrigger();
        }
    }

    public override void SwitchOn() {
        base.SwitchOn();
        UpdateConnectedObjects();
    }

    public override void SwitchOff() {
        base.SwitchOff();
        UpdateConnectedObjects();
    }
}