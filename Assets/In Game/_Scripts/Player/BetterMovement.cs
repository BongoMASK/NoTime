using UnityEngine;

public class BetterMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed;
    [SerializeField] bool grounded = false;

    [SerializeField] Transform rayCastShootPoint;
    [SerializeField] float rayCastHeight = 2f;

    [SerializeField] int maxSlopeAngle = 50;

    [SerializeField] LayerMask whatIsGround;

    [SerializeField] Transform player;


    void Move() {
        
    }

    void ApplyGravity() {

    }

    void CheckForGround() {
        if(Physics.Raycast(rayCastShootPoint.position, -rayCastShootPoint.up, out RaycastHit hit, rayCastHeight + 0.5f, whatIsGround)) {
            
            grounded = true;
            player.position = new Vector3(player.position.x, hit.point.y + rayCastHeight, player.position.z);
        }
        else {
            grounded = true;
            ApplyGravity();
        }
    }
}
