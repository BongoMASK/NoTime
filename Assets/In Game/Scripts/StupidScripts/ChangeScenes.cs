using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] KeyCode restartKey = KeyCode.P;

    private void Update() {
        CheckForPos();
        RestartLevel();
    }

    void RestartLevel() {
        if(Input.GetKey(KeyCode.LeftShift)) {
            if(Input.GetKeyDown(restartKey)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void CheckForPos() {
        if (player.position.y < -40)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

