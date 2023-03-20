using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float pressPosY = 0.2f;
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
            if(transform.position.y > ogPos.y - pressPosY)
                transform.Translate(0, -0.1f * Time.deltaTime * speed, 0);

            canMoveBack = false;
        }
    }

    //This code checks if the tag of a given GameObject can push the button and returns a boolean value. 
    bool isValidObject(GameObject go) 
    {
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
