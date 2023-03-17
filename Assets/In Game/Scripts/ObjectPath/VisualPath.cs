using UnityEngine;

public class VisualPath : MonoBehaviour {

    [SerializeField] LineRenderer rewindLine;
    [SerializeField] LineRenderer forwardLine;

    [SerializeField] Transform boxBox;

    int rewindIndex = 0;
    int forwardIndex = 0;

    bool isVisible = false;

    #region Add New Position

    public void AddRewindPos() {
        AddNewPos(rewindLine, ref rewindIndex);
    }

    public void AddForwardPos() {
        AddNewPos(forwardLine, ref forwardIndex);
    }

    /// <summary>
    /// Adds new position in line renderer
    /// </summary>
    /// <param name="line"></param>
    void AddNewPos(LineRenderer line, ref int i) {
        line.positionCount = i + 1;
        line.SetPosition(i, boxBox.localPosition);
        i++;
    }

    #endregion

    #region Remove Position

    public void RemoveRewindPos() {
        RemovePos(rewindLine, ref rewindIndex);
    }

    public void RemoveForwardPos() {
        RemovePos(forwardLine, ref forwardIndex);
    }

    /// <summary>
    /// Removes position from that line renderer
    /// </summary>
    /// <param name="line"></param>
    /// <param name="i"></param>
    void RemovePos(LineRenderer line, ref int i) {
        i--;
        line.positionCount = i + 1;
    }

    #endregion

    #region Clear Line

    public void ClearForwardLine() {
        ClearLine(forwardLine, ref forwardIndex);
    }

    void ClearLine(LineRenderer line, ref int i) {
        i = 0;
        line.positionCount = i;
    }

    #endregion

    #region Visibility

    /// <summary>
    /// Makes the linerenderer visible or not visible
    /// </summary>
    public void MakeVisible(bool b) {
        isVisible = b;
        rewindLine.enabled = isVisible;
        forwardLine.enabled = isVisible;
    }

    /// <summary>
    /// Toggles line renderer visibilty
    /// </summary>
    public void MakeVisible() {
        isVisible = !isVisible;
        rewindLine.enabled = isVisible;
        forwardLine.enabled = isVisible;
    }

    #endregion 
}