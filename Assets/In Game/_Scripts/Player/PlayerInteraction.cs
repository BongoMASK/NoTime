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
            if (value == _currentRCM) {
                if (_currentRCM != null)
                    ShowInteractMessage(currentRCM.OnPlayerViewedText);

                return;
            }

            if (value == null) {
                if (_currentRCM != null)
                    _currentRCM.OnPlayerViewExit();

                ShowInteractMessage("");
                _currentRCM = value;
                return;
            }

            _currentRCM = value;
            currentRCM.OnPlayerViewEnter();
        }
    }

    private void Update() {
        if(isScreenOpen) {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(interactKey)) {
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
            ShowInteractMessage("");
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
        if (currentInteractedObject == null)
            return;

        if (Time.time - timeSinceLastInteracted < minTimeBetweenInteraction)
            return;

        // Check if it is safe to release object
        Collider col = currentInteractedObject.GetChild(0).transform.GetComponent<Collider>();

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 dir = col.transform.position - cameraPos;

        // check if object is still in view of the player
        if (Physics.Raycast(Camera.main.transform.position, dir, out RaycastHit hit)) {
            if (hit.collider != col)
                return;
        }

        Vector3 size = col.bounds.extents;
        Collider[] cols = Physics.OverlapBox(col.bounds.center, size);

        // if something is colliding against the interacted obj...
        if (cols.Length > 1) {
            // tries to offset it to get a good floor pos
            if (Physics.Raycast(col.bounds.center, Vector3.down, out RaycastHit hitInfo, col.bounds.extents.y)) {
                float pressurePlateOffset = 0;
                if (hitInfo.collider.CompareTag("PressurePlate")) {
                    pressurePlateOffset = 0.22f;
                }

                Vector3 newObjOrigin = new Vector3(hitInfo.point.x, hitInfo.point.y + col.bounds.extents.y, hitInfo.point.z);
                Transform t = currentInteractedObject;
                Vector3 dist = t.GetChild(0).position - newObjOrigin;
                Vector3 targetDist = t.position - dist;
                currentInteractedObject.position = targetDist;
            }
            else
                return;
        }

        // Tries to make the object touch the floor
        //if (Physics.Raycast(col.bounds.center, Vector3.down, out RaycastHit hitInfo, col.bounds.size.y * 2)) {
        //    float pressurePlateOffset = 0;
        //    if(hitInfo.collider.CompareTag("PressurePlate")) {
        //        pressurePlateOffset = 0.6f;
        //    }

        //    Vector3 newObjOrigin = new Vector3(hitInfo.point.x, hitInfo.point.y + col.bounds.extents.y - pressurePlateOffset, hitInfo.point.z);

        //    cols = Physics.OverlapBox(newObjOrigin, size / 2);
        //    if (cols.Length > 1) {
        //        return;
        //    }

        //    Transform t = currentInteractedObject;
        //    Vector3 dist = t.GetChild(0).position - newObjOrigin;
        //    Vector3 targetDist = t.position - dist;
        //    currentInteractedObject.position = targetDist;
        //}
        //else
        //    return;

        if (isCurrentlyInteracting)
            currentInteractedObject = null;
    }

    public void OnCameraOpened(bool isScreenOpen = true) {
        ResetInteractedObject();
        UIManager.Instance.EnableScreen(isScreenOpen);

        playerMovement.lockInput = isScreenOpen;
        playerMovement.ResetInput();
    }

    #endregion
}
