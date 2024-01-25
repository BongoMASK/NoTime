using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ButtonType {
    H1 = 0,
    H2, 
    H3, 
    H4, 
    H5, 
    H6
}

public class ButtonStyle : MonoBehaviour
{
    public ButtonType type;
    int buttonType => (int)type;

    private Button button;

    public UnityEvent OnClickEvent;

    UnityEvent OnClickAnim = new UnityEvent();
    UnityEvent OnHoverAnim = new UnityEvent();
    UnityEvent DefaultAnim = new UnityEvent();

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        SetupButton();

        DefaultAnim?.Invoke();
    }

    public void ApplyButtonStyle(UIButtonHandler uIButtonHandler) {
        GetComponent<Image>().sprite = uIButtonHandler.currentButtonTheme.buttonVars[buttonType].buttonSprite;
        GetComponentInChildren<TMP_Text>().font = uIButtonHandler.currentButtonTheme.buttonVars[buttonType].font;
    }

    void SetupButton() {
        ButtonTheme buttonTheme = UIButtonHandler.instance.currentButtonTheme;

        OnClickAnim.AddListener(() => buttonTheme.buttonVars[buttonType].OnClickAnim?.Invoke(transform));
        DefaultAnim.AddListener(() => buttonTheme.buttonVars[buttonType].DefaultAnim?.Invoke(transform));

        button.onClick.AddListener(() => OnClickAnim?.Invoke());
        button.onClick.AddListener(() => OnClickEvent?.Invoke());
    }

}