using UnityEngine;


public class Pickable : MonoBehaviour, IRayCastMessage, IInteractable 
{

    [SerializeField] private string playerVeiwedText = "";
    [SerializeField] private string onInteractText = "";
    [SerializeField] private float timeToPick = 1f;
    [SerializeField] private float pickableDist = 4;

    public float TimeToPick { get => timeToPick; private set => timeToPick = value; }

    public string OnInteractText => onInteractText;
    public string OnPlayerViewedText => playerVeiwedText;

    public float messageDistance => pickableDist;

    public void Interact(PlayerInteraction interactor) {
        Debug.Log("Setting up the interaction references");
        interactor.currentInteractedObject = transform.parent;
    }

    public void OnPlayerViewEnter() {
    }

    public void OnPlayerViewExit() {
    }

    public void OnPlayerViewing() {
    }
}