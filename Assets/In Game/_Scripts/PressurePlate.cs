using UnityEngine;

public class PressurePlate : MonoBehaviour {
    [SerializeField] float speed = 1f;
    [SerializeField] float pressPosY = 0.2f;

    Vector3 ogPos;

    bool canMoveBack = false;

    private void Start() {
        ogPos = transform.localPosition;
    }

    private void Update() {
        ControlPressurePlate();
    }

    /// <summary>
    /// Checks whether pressure plate can move up
    /// </summary>
    void ControlPressurePlate() {
        if (!canMoveBack)
            return;

        if (transform.localPosition.y < ogPos.y)
            transform.Translate(0, 0.1f * Time.deltaTime * speed, 0);

        else
            canMoveBack = false;
    }

    #region Collisions

    private void OnCollisionStay(Collision collision) {
        // While button is BEING pressed
        if (isValidObject(collision.gameObject)) {
            if (transform.localPosition.y > ogPos.y - pressPosY) {
                transform.Translate(0, -0.1f * Time.deltaTime * speed, 0);
            }

            canMoveBack = false;
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if(isValidObject(collision.gameObject)) {
            canMoveBack = false;
        }
    }
    private void OnCollisionExit(Collision collision) {
        // When button has been released
        if (isValidObject(collision.gameObject)) {
            canMoveBack = true;
        }
    }

    #endregion

    /// <summary>
    /// Checks if the tag of a given GameObject can push the button and returns a boolean value. 
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    bool isValidObject(GameObject go) {
        //Check if Player
        if (go.tag == "Player")
            return true;

        //Check if Cube
        if (go.tag == "Cube")
            return true;

        //Check if Sphere
        if (go.tag == "Sphere")
            return true;

        //Return false if the tag is not equal to "Player"
        return false;
    }
}