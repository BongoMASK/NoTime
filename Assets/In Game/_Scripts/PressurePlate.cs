using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {
    [SerializeField] float speed = 1f;
    [SerializeField] float pressPosY = 0.2f;

    Vector3 ogPos;

    bool canMoveBack = false;

    public static List<string> tags = new List<string>() {
        "Player",
        "Sphere",
        "Cube",
    };

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
            //collision.gameObject.GetComponent<Rigidbody>().Sleep();
            // Delays time to move back so that it doesnt move inside player hitbox
            Invoke(nameof(DelayCanMoveBack), 0.1f);
        }
    }

    private void DelayCanMoveBack() {
        canMoveBack = true;
    }

    #endregion

    /// <summary>
    /// Checks if the tag of a given GameObject can push the button and returns a boolean value. 
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    bool isValidObject(GameObject go) {
        return tags.Contains(go.tag);
    }
}