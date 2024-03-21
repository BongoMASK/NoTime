using DG.Tweening;
using UnityEngine;

public class InteractDoor : MonoBehaviour, IInteractable, IRayCastMessage {

    [SerializeField] private string closeDoorText = "Press [E] to Open Door";
    [SerializeField] private string openDoorText = "Press [E] to Close Door";
    [SerializeField] private string onInteractText = "";

    [SerializeField] private float timeToPick = 1f;
    [SerializeField] private float pickableDist = 4;

    public string OnInteractText => onInteractText;

    public string OnPlayerViewedText => isOpen ? openDoorText : closeDoorText;

    public float messageDistance => pickableDist;

    public void Interact(PlayerInteraction interactor) {
        InteractWithDoor();
    }

    public void OnPlayerViewEnter() {
    }

    public void OnPlayerViewExit() {
    }

    public void OnPlayerViewing() {
    }

    [SerializeField] Transform doorModel;

    public bool isOpen { get; private set; }
    
    bool isDoorMoving = false;

    [SerializeField] bool isOpenAtStart = false;
    [SerializeField] Vector3 openDoorRotation = new Vector3(0, -90, 0);
    Vector3 closeDoorRotation = Vector3.zero;


    void Start()
    {
        closeDoorRotation = transform.rotation.eulerAngles;

        if (isOpenAtStart)
            Invoke(nameof(OpenDoor), 1);
    }

    public void InteractWithDoor() {
        if (isDoorMoving)
            return;

        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
    }

    void OpenDoor() {
        isDoorMoving = true;
        transform.DORotate(openDoorRotation, 0.5f, RotateMode.Fast)
            .onComplete += () => isDoorMoving = false;
        isOpen = true;
    }

    void CloseDoor() {
        isDoorMoving = true;
        transform.DORotate(closeDoorRotation, 0.5f, RotateMode.Fast)
            .onComplete += () => isDoorMoving = false;
        isOpen = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if(isDoorMoving) {
            if(collision.transform.CompareTag("Player")) {
                return;
            }

            transform.DOPause();
            isDoorMoving = false;

            Debug.Log(collision.transform.name);
        }
    }
}
