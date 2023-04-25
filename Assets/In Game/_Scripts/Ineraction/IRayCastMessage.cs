using System;

public interface IRayCastMessage 
{
    string OnPlayerViewedText { get; }

    public static Action<string> OnPlayerViewed;

}
