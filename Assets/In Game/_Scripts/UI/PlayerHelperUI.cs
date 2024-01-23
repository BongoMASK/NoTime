//using DG.Tweening;
using UnityEngine;

public class PlayerHelperUI : MonoBehaviour
{
    [Header("Booleans")]
    [SerializeField] bool flashTab;
    [SerializeField] bool flashSwitchCamera;
    [SerializeField] bool flashPause;
    [SerializeField] bool flashRewind;
    [SerializeField] bool flashForward;

    [SerializeField] CanvasGroup tabGroup;
    [SerializeField] CanvasGroup switchCameraGroup;
    [SerializeField] CanvasGroup pauseGroup;
    [SerializeField] CanvasGroup rewindGroup;
    [SerializeField] CanvasGroup forwardGroup;

    private void Start() {
        if (flashTab)
            Invoke(nameof(FlashTab), 60);
    }

    void FlashTab() {
        //tabGroup.DOFade(0, 0.5f).SetLoops(3);
    }

    void FlashSwitchCamera() {

    }

    void FlashPause() { }

    void FlashRewind() { }

    void FlashForward() { }
}
