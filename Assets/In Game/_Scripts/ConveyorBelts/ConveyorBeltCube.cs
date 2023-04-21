using UnityEngine;

public class ConveyorBeltCube : MonoBehaviour
{
    public float speed = 3f;
    Rigidbody rb;
    Vector3 dir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision other)
    {
        dir = other.transform.forward;
        Debug.Log(other.transform.name + " collider with");
    }

    private void FixedUpdate()
    {
        Vector3 pos = rb.position;
        pos += dir * speed * Time.fixedDeltaTime;

        rb.MovePosition(pos);
    }

}
