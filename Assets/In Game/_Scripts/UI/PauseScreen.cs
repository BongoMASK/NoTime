using UnityEngine;
using UnityEngine.EventSystems;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen instance;

    [SerializeField] MenuManager pauseMenuMananger;
    [SerializeField] GameObject pausePanel;

    PlayerMovement player;

    private static bool _isPaused;

    public static bool isPaused {
        get {
            return _isPaused;
        }
        set {
            _isPaused = value;
            instance.OnPausePressed();
        }
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        player = (PlayerMovement)FindFirstObjectByType(typeof(PlayerMovement));
        _isPaused = false;
    }

    private void OnPausePressed() {
        if(isPaused) {
            Pause();
        }
        else {
            Resume();
        }
    }

    private void Pause() {
        player.lockInput = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;
        pausePanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Resume() {
        player.lockInput = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
