using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    FPSController playerController;
    PlayerMovement playerMovement;

    [Header("References")]
    [SerializeField] RawImage screen;

    [Header("Object detection")]
    [SerializeField] private float detectionRange = 1f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform playerCam;
    [SerializeField, Tooltip("Transform of game object where the object should be after interaction")] private Transform interactedObjectPos;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

    [SerializeField] TMP_Text interactText;

    [SerializeField] Transform currentInteractedObject;
    public Transform CurrentInetractedObject { get => currentInteractedObject;  set => currentInteractedObject = value; }
    private float inputTimer;

    private Ray ray;
    private RaycastHit hit;
    private float lastTimeInteracted = 0;

    private Pickable pickable;
    private IRayCastMessage rayCastMessage;
    private IInteractable interactable;

    bool isScreenOpen = false;
    Collider colliderOnOnbject = null;


    //Read-only properties
    private bool IsCurrentlyInteracted { get => currentInteractedObject != null; }



    private void Awake()
    {
        playerController = GetComponent<FPSController>();
        playerMovement = GetComponent<PlayerMovement>();    
        currentInteractedObject = null;
    }

    private void Start()
    {
        lastTimeInteracted = 0;
    }


    private void Update()
    {

        if (!playerController.CanInteract)
        {
            return;
        }

        if (Input.GetKeyDown(toggleKey))
            ToggleScreen();

        if (IsCurrentlyInteracted)
        {
            HandleAlreadyInteracting();
            ShowInteractMessage("");
            return;
        }

        SetRay();

        if (ObjectInRange() && !IsCurrentlyInteracted)
        {
            if (!TryGetObjectData()) //Trying to set the refrences to use interfaces methods
            {
                //Debug.LogWarning("Couldn't get the refernces. The required script might not be attached");
                return;

            }
            string message = rayCastMessage.OnPlayerViewedText;
            IRayCastMessage.OnPlayerViewed?.Invoke(message);
            ShowInteractMessage(message);

            if (Input.GetKeyDown(interactKey))
            {
                
                HandleInteraction();
            }
        }
        else
        {
            IRayCastMessage.OnPlayerViewed?.Invoke("");
            ShowInteractMessage("");
        }
    }

    void ShowInteractMessage(string message) {
        interactText.text = message;
    }

    private void SetRay()
    {
        ray = new Ray(playerCam.position, playerCam.forward);
        Debug.DrawRay(playerCam.position, playerCam.forward * detectionRange);
    }

    private bool ObjectInRange()
    {
        return Physics.Raycast(ray, out hit, detectionRange, interactableLayer);
    }

    private void ResetInteractedObject()
    {
        currentInteractedObject = null;
    }

    private bool TryGetObjectData()
    {
        if(hit.transform.TryGetComponent(out pickable))
        {
            /*if (interactedObjectPos.GetComponent<Collider>() == null)
            {

                colliderOnOnbject = hit.transform.GetComponent<Collider>();

                Collider newCollider = interactedObjectPos.gameObject.AddComponent(colliderOnOnbject.GetType()) as Collider;

                newCollider.isTrigger = colliderOnOnbject.isTrigger;
            }*/

            rayCastMessage = pickable;
            interactable = pickable;
            return true;
        }
        //Debug.LogError("Object don't contain pickable scripts to reference");
        return false; 
        
        
    }

    private void HandleAlreadyInteracting()
    {
        if (interactedObjectPos == null) {

            //Debug.LogWarning("Interacted object position is not set up");
        }

        // Puts "object" in "hand" position by moving the "parent"
        Vector3 dist = currentInteractedObject.GetChild(0).position - interactedObjectPos.position;
        Vector3 targetDist = currentInteractedObject.transform.position - dist;
        currentInteractedObject.transform.position = Vector3.Lerp(currentInteractedObject.transform.position, targetDist, 0.4f);

        if (Input.GetKeyDown(interactKey))
        {
            ResetInteractedObject();
        }
    }

    private void HandleInteraction()
    {
        if (Time.time >= pickable.TimeToPick + lastTimeInteracted)
        {
            //Debug.Log("Interacting in time: " + pickable.TimeToPick);
            interactable.Interact(this);
            IRayCastMessage.OnPlayerViewed?.Invoke(pickable.OnInteractText);
            lastTimeInteracted = Time.time;
        }
    }

    private void ToggleScreen() {
        isScreenOpen = !isScreenOpen;
        CameraManager.instance.activeCam = isScreenOpen ? CameraManager.instance.FindActiveCam() : null;

        ResetInteractedObject();

        // Enables and disables the screen
        screen.enabled = isScreenOpen;
        playerController.lockInput = isScreenOpen;
        playerMovement.lockInput = isScreenOpen;
    }

}
