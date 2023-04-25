public interface IInteractable 
{
    void Interact(PlayerInteraction interactor);
    string OnInteractText { get; }
}
