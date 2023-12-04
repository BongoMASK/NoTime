using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    Transform player;
    [SerializeField] KeyCode restartKey = KeyCode.P;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
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
        }

        // Make player go to next level in case of any bugs
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void CheckForPos() {
        if (player.position.y < -40)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

