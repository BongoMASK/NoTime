using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    [Header("Assignables")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Transform playerCam;
    [SerializeField] Transform interactedObjectPos;

    [Header("Vars")]
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] LayerMask whatIsInteractable;

    public Transform currentInteractedObject { get; set; }

    public bool isCurrentlyInteracting => currentInteractedObject != null;

    float timeSinceLastInteracted = 0;
    float minTimeBetweenInteraction = 0.5f;

    bool isObjectInRange = false;

    bool isScreenOpen => CameraManager.instance.activeCam != null;

    IRayCastMessage _currentRCM;
    IRayCastMessage currentRCM {
        get => _currentRCM;

        set {
            if(value == _currentRCM) 
                return;

            if (value == null) {
                _currentRCM.OnPlayerViewExit();
                ShowInteractMessage("");
                _currentRCM = value;
                return;
            }

            _currentRCM = value;
            OnRCMValueUpdate();
        }
    }

    private void Update() {
        if(isScreenOpen) {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(interactKey)) {
                CameraManager.instance.activeCam = null;
                OnCameraOpened(false);
            }
        }
        else {
            ShootRayCast();
            Grabbing();
        }
    }

    #region Object Detection With Raycast

    /// <summary>
    /// Shoots raycast to find an object that can be interacted with
    /// </summary>
    private void ShootRayCast() {
        if (isCurrentlyInteracting)
            return;

        if (Physics.Raycast(playerCam.position, playerCam.forward, out RaycastHit hit, Mathf.Infinity)) {
            HandleRayCastHit(hit);
        }
        else {
            isObjectInRange = false;
            ShowInteractMessage("");
        }
    }

    private void HandleRayCastHit(RaycastHit hit) {
        IInteractable interactable = TryGetObjectData(hit);

        if (interactable != null && Input.GetKeyDown(interactKey)) {
            interactable.Interact(this);
            timeSinceLastInteracted = Time.time;
        }
    }

    private IInteractable TryGetObjectData(RaycastHit hit) {
        if (hit.transform.TryGetComponent(out IRayCastMessage raycastMessage)) {

            float dist = Vector3.Distance(hit.point, playerCam.position);
            if (dist > raycastMessage.messageDistance) {
                isObjectInRange = false;
                currentRCM = null;
                return null;
            }

            isObjectInRange = true;
            currentRCM = raycastMessage;
            currentRCM.OnPlayerViewing();

            return hit.transform.GetComponent<IInteractable>();
        }
        else {
            currentRCM = null;
        }

        return null;
    }

    private void OnRCMValueUpdate() {
        ShowInteractMessage(currentRCM.OnPlayerViewedText);
        currentRCM.OnPlayerViewEnter();
    }

    private void ShowInteractMessage(string message) {
        UIManager.Instance.SetInteractText(message);
    }

    #endregion

    #region Grabbing Funcs

    private void Grabbing() {
        if (!isCurrentlyInteracting)
            return;

        Transform t = currentInteractedObject.transform;

        // Puts "object" in "hand" position by moving the "parent"
        Vector3 dist = t.GetChild(0).position - interactedObjectPos.position;
        Vector3 targetDist = t.position - dist;
        t.position = Vector3.Lerp(t.position, targetDist, 0.4f);

        // Release object
        if (Time.time - timeSinceLastInteracted > minTimeBetweenInteraction && Input.GetKeyDown(interactKey))
            ResetInteractedObject();
    }

    public void MakeCurrentInteractedObject(Transform pickable) {
        if (!isCurrentlyInteracting)
            currentInteractedObject = pickable;
    }

    /// <summary>
    /// Lets go of the object that was being interacted with
    /// </summary>
    public void ResetInteractedObject() {
        if (Time.time - timeSinceLastInteracted < minTimeBetweenInteraction)
            return;

        if (isCurrentlyInteracting)
            currentInteractedObject = null;
    }

    //private void ToggleScreen() {
    //    isScreenOpen = !isScreenOpen;
    //    CameraManager.instance.activeCam = isScreenOpen ? CameraManager.instance.FindActiveCam() : null;

    //    ResetInteractedObject();

    //    // Enables and disables the screen
    //    //screen.enabled = isScreenOpen;
    //    UIManager.Instance.EnableScreen(isScreenOpen);
    //    playerController.lockInput = isScreenOpen;
    //    playerMovement.lockInput = isScreenOpen;

    //    playerMovement.ResetInput();
    //}

    public void OnCameraOpened(bool isScreenOpen = true) {
        ResetInteractedObject();
        UIManager.Instance.EnableScreen(isScreenOpen);

        playerMovement.lockInput = isScreenOpen;
        playerMovement.ResetInput();
    }

    #endregion
}
