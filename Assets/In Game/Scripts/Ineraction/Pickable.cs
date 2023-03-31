using UnityEngine;


public class Pickable : MonoBehaviour,IRayCastMessage, IInteractable
{
    [SerializeField] private string playerVeiwedText = "";
    [SerializeField] private float timeToPick = 1f;

    public float TimeToPick { get => timeToPick; private set => timeToPick = value; }

    public void Interact()
    {
        Debug.Log("Interacted with object");
    }

    public string OnPlayerViewedText()
    {
        return playerVeiwedText;
    }
}
