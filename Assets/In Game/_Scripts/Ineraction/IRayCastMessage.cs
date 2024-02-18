using System;

public interface IRayCastMessage 
{
    string OnPlayerViewedText { get; }

    float messageDistance { get; }

    public static Action<string> OnPlayerViewed;

    public void OnPlayerViewEnter();

    public void OnPlayerViewing();

    public void OnPlayerViewExit();

}
