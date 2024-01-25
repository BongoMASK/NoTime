using TMPro;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ButtonTheme", menuName = "ScriptableObjects/UI/ButtonTheme", order = 1)]
public class ButtonTheme : UIBaseTheme
{
    public ThemeVars[] buttonVars = new ThemeVars[6];
}

[System.Serializable]
public class ThemeVars {

    public ButtonType type;
    public Sprite buttonSprite;

    public UnityEvent<Transform> OnClickAnim;
    public UnityEvent<Transform> DefaultAnim;

    public TMP_FontAsset font;
}
