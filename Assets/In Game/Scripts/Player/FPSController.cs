using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour
{
    #region functional Toggles
    [Header("Functional toggle"), Tooltip("To on and off some player functionality")]
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool canWalk = true;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool canAirMove = true;
    [SerializeField] private bool canJump = true;

    public bool lockInput = false;

    #endregion

    #region Internal references
    private Rigidbody rb;
    private CapsuleCollider capCollider;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        capCollider = GetComponentInChildren<CapsuleCollider>();
        rb.freezeRotation = true;
        stepRayUpper.position = new Vector3(stepRayUpper.position.x, maxStepHeight, stepRayUpper.position.z);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalDrag = rb.drag;
        //Debug.Log(originalDrag);

    }


    private void Update()
    {
        if (!isActive)
            return;

        TakeInput();
        RotatePlayer();

        if (ShouldJump())
            HandleJump();
        HandleGravity();
        SpeedControl();
        AccelerateSpeed();
        ApplyFriction();
        StairStep();
        // Debug.Log(rb.velocity);
    }

    private void FixedUpdate()
    {
        //Debug.Log(rb.velocity);
        if (IsGrounded() && (canWalk || canSprint) && (moveDir.x != 0 || moveDir.z != 0))
            Move();
        else if (!IsGrounded() && canAirMove)
        {
            //Debug.Log("In the air");
            Move(airMovementMultiplier);
        }
        // Debug.Log(IsGrounded());

    }

    #endregion

    #region Basic Control,Movement,Inputs

    [Header("MOVE")]
    [Header("Walk")]
    [SerializeField] private float walkSpeed = 4f;

    [Header("Acceleration and multiplier")]
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float airMovementMultiplier = .5f;
    [SerializeField] private float frictionWhenStoping = 1f;
    [SerializeField] private float groundMovementMultiplier = 10f;

    [Header("Sprint")]
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    private float originalDrag;

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



    private float currentSpeed = 0f;


    private void TakeInput()
    {
        if (lockInput)
            return;

        xMoveInput = Input.GetAxisRaw(Horizontal);
        zMoveInput = Input.GetAxisRaw(Vertical);

        xMouseInput = Input.GetAxisRaw(MouseX);
        yMouseInput = Input.GetAxisRaw(MouseY);

        isJumpPressed = allowJumpButtonHold ? Input.GetKey(jumpKey) : Input.GetKeyDown(jumpKey);

        sprintKeyPressed = canSprint ? Input.GetKey(sprintKey) : false;

        moveDir = transform.forward * zMoveInput + transform.right * xMoveInput;
        moveDir = moveDir.normalized;
    }

    private void Move(float movementMultiplier = 1)
    {
        currentSpeed = IsSprinting ? sprintSpeed : walkSpeed;
        rb.AddForce(moveDir * currentSpeed * groundMovementMultiplier * movementMultiplier, ForceMode.Acceleration);
        //Debug.Log(movementMultiplier + " the drag is: "+rb.drag);
    }

    private void AccelerateSpeed()
    {
        if (IsSprinting)
        {
            currentSpeed = Mathf.SmoothStep(currentSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.SmoothStep(currentSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    private void ApplyFriction()
    {
        if (moveDir.x != 0 || moveDir.z != 0 && IsGrounded())
        {
            //Debug.Log("Reseting drag");
            ResetDrag();
            return;
        }

        rb.drag = frictionWhenStoping;
    }

    private void ResetDrag()
    {
        rb.drag = originalDrag;
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
        float tempX = xMouseInput * camSensi * Time.deltaTime;
        float tempY = yMouseInput * camSensi * Time.deltaTime;

        rotationX -= tempY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);

        cam.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, tempX, 0f);


    }

    #endregion

    #region Jump and gravity
    [Header("JUMPING")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private bool allowJumpButtonHold = false;
    [SerializeField] private float jumpCooldown = .5f;

    private bool isJumpPressed;
    private bool readyToJump = true;

    [Header("GROUND CHECK")]
    [SerializeField] private float groundCheckDistance = .2f;
    [SerializeField] private float sphereRadius = 1f;
    [SerializeField] private LayerMask groundLayer;
    //private float playerHieght;
    private Vector3 pos;

    [Header("Gravity")]
    [SerializeField] private float gravityForce = 10f;
    [SerializeField] private float maxFallingSpeed = 10f;

    private void HandleGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.AddForce(-transform.up * gravityForce * Time.deltaTime, ForceMode.Acceleration);
        }

        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxFallingSpeed, Mathf.Infinity), rb.velocity.z);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius, groundLayer);

        //Debug.Log("IS Grounded: "+ (colliders.Length > 0));
        return colliders.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
        Gizmos.DrawRay(stepRayLower.position, -transform.up * .3f);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void HandleJump()
    {
        readyToJump = false;

        Jump();
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private bool ShouldJump()
    {
        return isJumpPressed && readyToJump && IsGrounded() && canJump;
    }

    #endregion


    #region Stairs step
    [Header("Stair Step Up")]
    [SerializeField] private float maxStepHeight = .5f;
    [SerializeField] private float stepUpSpeed = .1f;
    [SerializeField] private Transform stepRayUpper;
    [SerializeField] private Transform stepRayLower;
    [SerializeField,Range(.1f,.3f)] private float lowerRayDetectRange = .3f; 

    RaycastHit firstRayHit;
    float colliderHieght;

    private void StairStep()
    {
        if (CheckForStairs())
        {
            Debug.Log("Name of the object is : " + firstRayHit.transform.name);
            Debug.Log("Steping up stairs");

            transform.position -= new Vector3(0f, -stepUpSpeed, 0f);
            /* colliderHieght = firstRayHit.collider.bounds.size.y;
             moveDir.y += colliderHieght;*/
            //Debug.Log("Move Dir is: " + moveDir);

        }
    }

    private bool CheckForStairs()
    {
        bool firstRayCheck = Physics.Raycast(stepRayLower.position, -transform.up, out firstRayHit, lowerRayDetectRange);
        if (!firstRayCheck)
        {
            return false;
        }
        bool secondRayCheck = Physics.Raycast(stepRayUpper.position, -transform.up, .2f);
        if (secondRayCheck)
        {
            return false;
        }

        return true;




    }

    #endregion

    #region Properties

    private bool IsSprinting => canSprint && sprintKeyPressed;
    public bool CanInteract { get => canInteract; set => canInteract = value; }

    #endregion


}
