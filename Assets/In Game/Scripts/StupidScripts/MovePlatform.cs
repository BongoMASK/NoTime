using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] bool move = false;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 10f;

    void Update()
    {
        if(move) {
            rb.velocity = new Vector3(50, 0, 0) * Time.deltaTime;
        }
    }
}
