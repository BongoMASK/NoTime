using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{

    [SerializeField] MenuManager menuManager;
    bool hasPressed = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (hasPressed)
                return;

            hasPressed = true;
            menuManager.DoTransitionAnimIn();
            Invoke(nameof(fjksdafdlsa), 1);
        }
    }

    void fjksdafdlsa() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
