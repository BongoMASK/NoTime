using UnityEngine;

public class CameraInteraction : MonoBehaviour, IRayCastMessage, IInteractable
{
    [Header("Assignables")]
    [SerializeField] CameraInfluence cameraInfluence;
    [SerializeField] Outline outline;

    [SerializeField] string interactText = "Press [E] to Access Camera";
    float maxInteractDist = Mathf.Infinity;

    public string OnPlayerViewedText => interactText;

    public float messageDistance => maxInteractDist;

    public string OnInteractText => interactText;

    public void Interact(PlayerInteraction interactor) {
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
}
