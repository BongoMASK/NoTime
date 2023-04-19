using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[CreateAssetMenu(menuName = "Flexible UI Data")]
public class FlexibleUIdata : ScriptableObject
{
    public Color buttonSprite;
    
    public SpriteState buttonSpriteState;

    public Color defaultColor;
    public Sprite defaultIcon;

    public Color confirmColor;
    public Sprite condirmIcon;

    public Color declineColor;
    public Sprite declineIcon;

    public Color warningColor;
    public Sprite warningIcon;

    public Color HighlightedButton;
   

    public static implicit operator string(FlexibleUIdata v)
    {
        throw new NotImplementedException();
    }
}
