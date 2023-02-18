using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour
{
    [Header("Functional toggle"), Tooltip("To on and off some player functionality")]
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool canWalk = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canInteract = true;

    //References.
    private Rigidbody rb;



   

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        if (!isActive)
            return;
        
        TakeInput();
        SpeedControl();
        AccelerateSpeed();
        Debug.Log(rb.velocity);


    }

    private void FixedUpdate()
    {
        Move();
    }



    [Header("MOVE")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    private Vector3 moveDir = Vector3.zero;
    private float xInput, zInput;
    private bool sprintKeyPressed;


    private bool IsSprinting => canSprint && sprintKeyPressed;
    private float currentSpeed = 0f;

    

    private void TakeInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        moveDir = transform.forward * zInput + transform.right * xInput;
        moveDir = moveDir.normalized;
    }

    private void Move()
    {
      

        currentSpeed = IsSprinting ? sprintSpeed : walkSpeed; 

        rb.AddForce(moveDir * currentSpeed * 10f,ForceMode.Acceleration);


    }

    private void AccelerateSpeed()
    {
        if (IsSprinting)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        /*  if (IsSprinting)
          {
              rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -sprintSpeed, sprintSpeed), 0f,Mathf.Clamp(rb.velocity.z,-sprintSpeed, sprintSpeed));
          }

          else
          {
              rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -walkSpeed, walkSpeed), 0f, Mathf.Clamp(rb.velocity.z, -walkSpeed, walkSpeed));

          }*/
    }

}
