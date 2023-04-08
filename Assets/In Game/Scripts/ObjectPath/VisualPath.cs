using UnityEngine;

public class VisualPath : MonoBehaviour {

    public LineRenderer rewindLine;
    public LineRenderer forwardLine;

    [SerializeField] Transform boxBox;

    int rewindIndex = 0;
    int forwardIndex = 0;

    bool isVisible = false;

    [SerializeField] int maxSkips = 5;
    int skipCount = 0;
    int skipCount1 = 0;

    #region Add New Position

    public void AddRewindPos() {
        skipCount++;
        if (skipCount < maxSkips)
            return;
        skipCount = 0;

        AddNewPos(rewindLine, ref rewindIndex);
    }

    public void AddRewindPos(Vector3 pos) {
        skipCount++;
        if (skipCount < maxSkips)
            return;
        skipCount = 0;

        AddNewPos(rewindLine, ref rewindIndex, pos);
    }

    public void AddForwardPos() {
        skipCount1++;
        if (skipCount1 < maxSkips)
            return;
        skipCount1 = 0;

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

    /// <summary>
    /// Adds pos in the line renderer
    /// </summary>
    /// <param name="line"></param>
    /// <param name="i"></param>
    /// <param name="pos"></param>
    void AddNewPos(LineRenderer line, ref int i, Vector3 pos) {
        line.positionCount = i + 1;
        line.SetPosition(i, pos);
        i++;
    }

    #endregion

    #region Remove Position

    public void RemoveRewindPos() {
        skipCount--;
        if (skipCount > 0)
            return;
        skipCount = maxSkips;

        RemovePos(rewindLine, ref rewindIndex);
    }

    public void RemoveForwardPos() {
        skipCount1--;
        if (skipCount1 > 0)
            return;
        skipCount1 = maxSkips;

        RemovePos(forwardLine, ref forwardIndex);
    }

    /// <summary>
    /// Removes position from that line renderer
    /// </summary>
    /// <param name="line"></param>
    /// <param name="i"></param>
    void RemovePos(LineRenderer line, ref int i) {
        if (line.positionCount == 1)
            return;

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