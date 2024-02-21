using UnityEngine;

public class CameraInteraction : PuzzleComponent, IRayCastMessage, IInteractable
{
    [Header("Assignables")]
    [SerializeField] CameraInfluence cameraInfluence;
    [SerializeField] Outline outline;

    [Header("Vars")]
    [SerializeField] string interactText = "Press [E] to Access Camera";
    [SerializeField] string cannotInteractText = "Cannot interact. Camera is Switched Off";

    [SerializeField] bool isAlreadyOn = true;

    float maxInteractDist = Mathf.Infinity;

    public string OnPlayerViewedText => isOn ? interactText : cannotInteractText;

    public float messageDistance => maxInteractDist;

    public string OnInteractText => interactText;

    private void Start() {
        isOn = isAlreadyOn;
    }

    public void Interact(PlayerInteraction interactor) {
        if(!isOn)
            return;

        CameraManager.instance.activeCam = cameraInfluence;
        interactor.OnCameraOpened();
    }

    public void OnPlayerViewEnter() {
        outline.enabled = true;
    }

    public void OnPlayerViewExit() {
        outline.enabled = false;
    }

    public void OnPlayerViewing() {
    }

    public override void SwitchOff() {
        base.SwitchOff();
    }

    public override void SwitchOn() {
        base.SwitchOn();
    }
}
