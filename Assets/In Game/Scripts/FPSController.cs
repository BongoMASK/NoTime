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
        cam = GetComponentInChildren<Camera>();
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
        RotatePlayer();
        SpeedControl();
        AccelerateSpeed();
        Debug.Log(rb.velocity);


    }

    private void FixedUpdate()
    {
        if(canWalk || canSprint)
           Move();
    }



    [Header("MOVE")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("CAMERA AND ROTATION")]
    [SerializeField] private float camSensi = 400f;
    [SerializeField] private float upperLookLimit = 90f;
    [SerializeField] private float lowerLookLimit = 70f;
    private Camera cam;
    private float rotationX = 0f;

    //Input varibales
    private Vector3 moveDir = Vector3.zero;
    private float xMoveInput, zMoveInput, yMouseInput, xMouseInput;
    private bool sprintKeyPressed;

    //Inputs strings
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";


    private bool IsSprinting => canSprint && sprintKeyPressed;
    private float currentSpeed = 0f;



    private void TakeInput()
    {
        xMoveInput = Input.GetAxisRaw(Horizontal);
        zMoveInput = Input.GetAxisRaw(Vertical);

        xMouseInput = Input.GetAxisRaw(MouseX);
        yMouseInput = Input.GetAxisRaw(MouseY);

        moveDir = transform.forward * zMoveInput + transform.right * xMoveInput;
        moveDir = moveDir.normalized;
    }

    private void Move()
    { 
        currentSpeed = IsSprinting ? sprintSpeed : walkSpeed;
        rb.AddForce(moveDir * currentSpeed * 10f, ForceMode.Acceleration);
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
    }

    private void RotatePlayer()
    {
        float tempX = xMouseInput  * camSensi * Time.deltaTime;
        float tempY = yMouseInput  * camSensi * Time.deltaTime;

        rotationX -= tempY;
        rotationX = Mathf.Clamp(rotationX,-upperLookLimit,lowerLookLimit);

        cam.transform.localRotation = Quaternion.Euler(rotationX,0f,0f);
        transform.rotation *= Quaternion.Euler(0f, tempX, 0f);

       
    }

}
