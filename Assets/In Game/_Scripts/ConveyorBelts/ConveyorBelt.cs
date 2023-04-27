using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public enum Direction {
        Forward,
        Backward,
        Right,
        Left
    }

    [SerializeField] private float speed = 2f;
    [SerializeField] private bool CanMove = false;

    [SerializeField] Direction dir = Direction.Forward;

    Vector3 direction {
        get {
            switch (dir) {
                case Direction.Forward:
                    return transform.forward;

                case Direction.Backward:
                    return -transform.forward;

                case Direction.Right:
                    return transform.right;

                case Direction.Left:
                    return -transform.right;

                default: 
                    return -transform.forward;
            }
        }
    }

    List<Rigidbody> rigidbodies = new List<Rigidbody>();

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.TryGetComponent(out Rigidbody rb))
        {
            //Debug.Log(rb.name + " is added rigibodies to the list");
            rigidbodies.Add(rb);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.TryGetComponent(out Rigidbody rb))
        {
            //Debug.Log(rb.name + "is removed rigibodies to the list");
            rigidbodies.Remove(rb);
            rb.AddForce(speed * direction * 50);
        }
    }

    
    private void FixedUpdate()
    {
        if(rigidbodies.Count > 0)
        {
            //Debug.Log("Moving rigibodies in the list");
            foreach (Rigidbody rb in rigidbodies)
            {
                Vector3 pos = rb.position;
                pos += direction * speed * Time.fixedDeltaTime;
                rb.MovePosition(pos);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawRay(transform.position, direction * 3);
    }

}
