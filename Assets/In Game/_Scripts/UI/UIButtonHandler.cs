using UnityEngine;

public class UIButtonHandler : MonoBehaviour
{
    public static UIButtonHandler instance { get; private set; }

    [SerializeField] public ButtonTheme currentButtonTheme;

    private void Awake() {
        instance = this;
    }
}

