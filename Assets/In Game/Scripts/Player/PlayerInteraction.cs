using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    FPSController playerController;

    [Header("Object detection")]
    [SerializeField] private float detectionRange = 1f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform playerCam;
    [SerializeField, Tooltip("Transform of game object where the object should be after interaction")] private Transform interactedObjectPos;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private Pickable currentInteractedObject;
    private float inputTimer;

    private Ray ray;
    private RaycastHit hit;
    private float lastTimeInteractPressed = 0;

    private void Awake()
    {
        playerController = GetComponent<FPSController>();
        currentInteractedObject = null;
    }


    private void Update()
    {
        if (!playerController.CanInteract)
        {
            return;
        }

        if (currentInteractedObject != null)
        {
            if (interactedObjectPos == null)
                Debug.LogWarning("Interacted object position is not set up");
            currentInteractedObject.transform.position = interactedObjectPos.position;

            if (Input.GetKeyDown(interactKey))
            {
                Invoke(nameof(ResetInteractedObject),2f);
            }

            return;
        }

        ray = new Ray(playerCam.position, playerCam.forward);
        Debug.DrawRay(playerCam.position, playerCam.forward * detectionRange);

        if (ObjectInRange() && currentInteractedObject == null)
        {
            Debug.Log("We can hold the object");
            if (hit.transform.TryGetComponent<Pickable>(out Pickable pickableObject))
            {
                Debug.Log(pickableObject.OnPlayerViewed());
                if (Input.GetKey(interactKey))
                {
                    currentInteractedObject = pickableObject;
                    Debug.Log($"Current holded object is: {currentInteractedObject.name}");
                }




            }

        }


    }

    private bool ObjectInRange()
    {
        return Physics.Raycast(ray, out hit, detectionRange, interactableLayer);
    }

    private void ResetInteractedObject()
    {
        currentInteractedObject = null;
    }


}
