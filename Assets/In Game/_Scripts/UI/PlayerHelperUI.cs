using DG.Tweening;
using UnityEngine;

public class PlayerHelperUI : MonoBehaviour
{
    private bool _hasPressedTab = false;

    [Header("Helper Vars")]
    [SerializeField] float flashSpeed = 0.3f;
    
    public bool hasPressedTab {
        get => _hasPressedTab;
        set {
            _hasPressedTab = value;
            OnPressedTab();
        }
    }

    public bool hasSwitchedCamera = false;
    public bool hasPlay = false;
    public bool hasRewind = false;
    public bool hasForward = false;

    bool isCameraOpen => CameraManager.instance.activeCam != null;

    [Header("Booleans")]
    [SerializeField] bool flashTab;
    [SerializeField] bool flashSwitchCamera;
    [SerializeField] bool flashPlay;
    [SerializeField] bool flashRewind;
    [SerializeField] bool flashForward;

    [Header("Canvas Groups")]
    [SerializeField] CanvasGroup tabGroup;
    [SerializeField] CanvasGroup switchCameraGroup;
    [SerializeField] CanvasGroup playGroup;
    [SerializeField] CanvasGroup rewindGroup;
    [SerializeField] CanvasGroup forwardGroup;

    private void Start() {
        Invoke(nameof(FlashTab), 1);
    }

    void FlashUI(CanvasGroup cg, bool hasPressed, bool shouldBeDone, bool isCameraOpen, string funcName) {
        if (hasPressed || !shouldBeDone || !isCameraOpen)
            return;

        cg.DOFade(0, flashSpeed).SetLoops(6, LoopType.Yoyo).SetEase(Ease.InOutSine);
        Invoke(funcName, 10);
    }

    void FlashTab() {
        FlashUI(tabGroup, hasPressedTab, flashTab, !isCameraOpen, nameof(FlashTab));
    }

    void FlashSwitchCamera() {
        FlashUI(switchCameraGroup, hasSwitchedCamera, flashSwitchCamera, isCameraOpen, nameof(FlashSwitchCamera));
    }

    void FlashPlay() {
        FlashUI(playGroup, hasPlay, flashPlay, isCameraOpen, nameof(FlashPlay));
    }

    void FlashRewind() {
        FlashUI(rewindGroup, hasRewind, flashRewind, isCameraOpen, nameof(FlashRewind));
    }

    void FlashForward() {
        FlashUI(forwardGroup, hasForward, flashForward, isCameraOpen, nameof(FlashForward));
    }

    void OnPressedTab() {
        Invoke(nameof(FlashPlay), 3);
        Invoke(nameof(FlashForward), 3);
        Invoke(nameof(FlashRewind), 3);
        Invoke(nameof(FlashSwitchCamera), 3);
    }

    public void ResetBools() {
        hasPressedTab = false;
        hasSwitchedCamera = false;
        hasPlay = false;
        hasRewind = false;
        hasForward = false;
    }
}
