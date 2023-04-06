using System;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    PlayerMovement playerController;


    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] RawImage screen;

    [Header("Object detection")]
    [SerializeField] private float detectionRange = 1f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform playerCam;
    [SerializeField, Tooltip("Transform of game object where the object should be after interaction")] private Transform interactedObjectPos;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode toggleKey = KeyCode.Q;

    [SerializeField] Transform currentInteractedObject;
    private float inputTimer;

    private Ray ray;
    private RaycastHit hit;
    private float lastTimeInteractPressed = 0;

    private Pickable pickable;
    private IRayCastMessage rayCastMessage;
    private IInteractable interactable;

    bool isScreenOpen = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerMovement>();
        currentInteractedObject = null;
    }


    private void Update()
    {
        //if (!playerController.CanInteract)
        //{
        //    return;
        //}

        if (Input.GetKeyDown(toggleKey))
            ToggleScreen();

        if (currentInteractedObject != null)
        {
            if (interactedObjectPos == null)
                Debug.LogWarning("Interacted object position is not set up");
            
            currentInteractedObject.transform.position = interactedObjectPos.position - currentInteractedObject.GetChild(0).localPosition;

            if (Input.GetKeyDown(interactKey))
            {
                //Invoke(nameof(ResetInteractedObject),2f);
                ResetInteractedObject();
            }

            return;
        }

        SetRay();

        if (ObjectInRange() && currentInteractedObject == null)
        {
            if (!TryGetObjectData()) //Trying to set the refrences to use interfaces methods
            {
                Debug.LogWarning("Couldn't get the refernces. The required script might not be attached");
                return;

            }
            var message = rayCastMessage.OnPlayerViewedText();
            IRayCastMessage.OnPlayerViewed?.Invoke(message);
            Debug.Log("We can hold the object");

            if (Input.GetKeyDown(interactKey))
            {
                interactable.Interact();
                currentInteractedObject = pickable.transform.parent;
                Debug.Log($"Current holded object is: {currentInteractedObject.name}");
            }
        }
        else
        {
            IRayCastMessage.OnPlayerViewed?.Invoke("");
        }
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
            rayCastMessage = pickable;
            interactable = pickable;
            return true;
        }
        Debug.LogError("Object don't contain pickable scripts to reference");
        return false; 
        
        
    }

    [SerializeField] CameraInfluence cam;

    private void ToggleScreen() {
        isScreenOpen = !isScreenOpen;
        CameraManager.instance.activeCam = isScreenOpen ? cam : null;

        ResetInteractedObject();

        // Enables and disables the screen
        screen.enabled = isScreenOpen;
        playerController.lockInput = isScreenOpen;
    }

}
