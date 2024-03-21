using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    Transform player;
    [SerializeField] KeyCode restartKey = KeyCode.P;
    [SerializeField] MenuManager menuManager;

    [SerializeField] bool enableCursor = false;

    [SerializeField] int lowestYVal = -40;
    [SerializeField] bool checkForPos = true;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if(enableCursor) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Update() {
        if(player == null)
            return;

        CheckForPos();
        RestartLevel();
    }

    void RestartLevel() {
        if(Input.GetKey(KeyCode.LeftShift)) {
            if(Input.GetKeyDown(restartKey)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
            if (Input.GetKeyDown(KeyCode.O)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }

            // Make player go to next level in case of any bugs
            if (Input.GetKeyDown(KeyCode.Return)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void ChangeLevelWithDelay(int sceneIndex) {
        StartCoroutine(Cor_ChangeLevelWithDelay(sceneIndex));
    }

    IEnumerator Cor_ChangeLevelWithDelay(int index, float time = 1) {
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(index);
        Time.timeScale = 1;
    }

    void CheckForPos() {
        if (!checkForPos)
            return;

        if (player.position.y < lowestYVal) {
            menuManager.DoTransitionAnimIn();
            ChangeLevelWithDelay(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Quit() {
        Application.Quit();
    }

    public void OpenURL(string url) {
        Application.OpenURL(url);
    }
}

