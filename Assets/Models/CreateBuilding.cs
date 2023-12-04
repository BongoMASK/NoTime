using System.Collections.Generic;
using UnityEngine;

public class CreateBuilding : MonoBehaviour
{
    [SerializeField] float force = 600f;

    // Stores all physics objects
    List<Rigidbody> rigidbodies = new List<Rigidbody>();

    private void Start() {
        // Make Building go BOOM
        Invoke(nameof(EnableAllRigidbodies), 0.7f);
    }

    private void Awake() {
        // Finds all cubes (Physics Objects / Rigidbodies) in the building gameobject
        rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
    }

    void EnableAllRigidbodies() {
        foreach (var item in rigidbodies) {

            // activating the rigidbodies. Now physics will be calculated
            item.isKinematic = false;

            // add fake force to rigidbody
            item.AddExplosionForce(force, new Vector3(4, 11, -8), 10);
        }
    }
}
