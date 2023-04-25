using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

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
                pos += transform.forward * speed * Time.fixedDeltaTime;
                rb.MovePosition(pos);
            }
        }
    }

}
