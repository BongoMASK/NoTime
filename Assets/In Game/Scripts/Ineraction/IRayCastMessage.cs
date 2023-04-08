using System;

public interface IRayCastMessage 
{
    string OnPlayerViewedText();

    public static Action<string> OnPlayerViewed;

}
