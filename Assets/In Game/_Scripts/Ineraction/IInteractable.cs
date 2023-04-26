using System;

public interface IInteractable 
{
    void Interact(PlayerInteraction interactor);
    string OnInteractText { get; }

    public static Action OnFocus;
    public static Action OnUnfocus;
}
