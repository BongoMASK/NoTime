using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // remove this later. Try not to use singletons
    public static UIManager Instance;

    public CameraUI cameraUI;

    [SerializeField] Transform playerHUD;
    [SerializeField] Transform cameraHUD;
    [SerializeField] Transform crosshairHUD;

    [SerializeField] RawImage screen;

    [SerializeField] TMP_Text interactText;

    private void Awake() {
        Instance = this;
    }

    public void SwitchToPlayerHUD() {
        cameraHUD.gameObject.SetActive(false);
        playerHUD.gameObject.SetActive(true);
    }

    public void SwitchToCameraHUD() {
        cameraHUD.gameObject.SetActive(true);
        playerHUD.gameObject.SetActive(false);
    }

    public void EnableScreen(bool enabled) {
        screen.enabled = enabled;

        if (enabled)
            SwitchToCameraHUD();
        else
            SwitchToPlayerHUD();
    }

    public void SetInteractText(string message) {
        interactText.text = message;
    }
}
