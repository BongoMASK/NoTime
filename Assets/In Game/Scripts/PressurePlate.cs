using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Vector3 ogPos;
    bool canMoveBack = false;

    private void Start()
    {
        ogPos = transform.position;
    }


    private void Update()
    {
        if (canMoveBack)
        {
            if (transform.position.y < ogPos.y)
            {
                transform.Translate(0, 0.1f * Time.deltaTime * speed, 0);
            }
            else
            {
                canMoveBack = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isValidObject(collision.gameObject))
        {
            canMoveBack = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (isValidObject(collision.gameObject))
        {
            canMoveBack = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isValidObject(collision.gameObject))
        {
            transform.Translate(0, -0.1f * Time.deltaTime * speed, 0);
            canMoveBack = false;
        }
    }

    //This code checks if the tag of a given GameObject can push the button and returns a boolean value. 
    bool isValidObject(GameObject go)
    {
        //Check if the tag of the given GameObject is equal to "Player"
        if (go.tag == "Player")
        {
            //Return true if the tag is equal to "Player"
            return true;
        }
        else
        {
            //Return false if the tag is not equal to "Player"
            return false;
        }
    }

}
