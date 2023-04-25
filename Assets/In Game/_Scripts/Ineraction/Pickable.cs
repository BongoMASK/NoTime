using UnityEngine;


public class Pickable : MonoBehaviour,IRayCastMessage, IInteractable
{
    [SerializeField] private string playerVeiwedText = "";
    [SerializeField] private string onInteractText = "";
    [SerializeField] private float timeToPick = 1f;

    public float TimeToPick { get => timeToPick; private set => timeToPick = value; }

    public string OnInteractText => onInteractText;
    public string OnPlayerViewedText => playerVeiwedText;

    public void Interact(PlayerInteraction interactor)
    {
        Debug.Log("Setting up the interaction references");
        interactor.CurrentInetractedObject = transform.parent;
    }
}
