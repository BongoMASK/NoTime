using UnityEngine;

public class PlayerMovement : HeroUnitBase {

    //Assingables
    [Header("Assignables")]
    [SerializeField] Transform playerCam;
    [SerializeField] Transform orientation;

    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    public static float sensitivity = 50;
    private readonly float sensMultiplier = 1f;

    //Movement
    [Header("Move Speeds")]
    [SerializeField] float moveSpeed = 4500;
    [SerializeField] float _maxSpeed = 20;
    [SerializeField] bool grounded;
    [SerializeField] LayerMask whatIsGround;

    private float maxSpeed {
        get {
            if(grounded)
                return _maxSpeed;

            return _maxSpeed * 1.35f;
        }
    }

    [SerializeField] float counterMovement = 0.175f;
    [SerializeField] float stopMovement = 0.3f;
    private float threshold = 0.01f;
    [SerializeField] float maxSlopeAngle = 35f;

    //Crouch & slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;

    private bool _lockInput = false;

    public bool lockInput {
        get {
            return _lockInput;
        }
        set {
            _lockInput = value;
            //playerCam.GetComponentInChildren<Camera>().enabled = !value;
        }
    }

    //jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    [SerializeField] float jumpForce = 550f;
    [SerializeField] float jumpGraceTime;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    [SerializeField] float downForce = 20;

    //Input
    protected struct UserInput {
        public int x, y;
        public bool jumping;
        public bool pressed;

        public bool IsPressed() {
            return x == 0 && y == 0;
        }
    }

    protected UserInput userInput;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    //Photon Networking Variables

    float soundTimer = 0f;

    Vector3 currentGravity;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        currentGravity = Physics.gravity;
    }

    void Start() {
        playerScale = transform.localScale;
        crouchScale = playerScale;
        crouchScale.y = 0.5f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate() {
        Movement();
        Gravity();
    }

    private void Update() {
        MyInput();
        Look();

        //Animations(); 
        StopMoving();
    }

    void StopMoving() {
        if (userInput.x == 0 && userInput.y == 0 && rb.velocity.magnitude < 0.3f)
            rb.velocity = Vector3.zero;
    }

    void Gravity() {
        rb.AddForce(currentGravity, ForceMode.Acceleration);   //does gravity based on slope of floor
    }

    bool crouchSound = false;
    bool hasJumped = false;
    float currentYPos;

    private void MyInput() {
        if (lockInput == true)
            return;

        if (Input.GetKey(KeyCode.D))
            userInput.x = 1;     //Input.GetAxisRaw("Horizontal");
        else if (Input.GetKey(KeyCode.A))
            userInput.x = -1;
        else
            userInput.x = 0;

        if (Input.GetKey(KeyCode.W))
            userInput.y = 1;
        else if (Input.GetKey(KeyCode.S))
            userInput.y = -1;
        else
            userInput.y = 0;

        userInput.jumping = Input.GetKey(KeyCode.Space);
    }

    private void StartCrouch() {
        ChangePlayerHeight(crouchScale);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }

    private void StopCrouch() {
        transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        ChangePlayerHeight(playerScale);
    }

    void ChangePlayerHeight(Vector3 scale) {
        transform.localScale = scale;
    }

    void Animations() {
        if (readyToJump && userInput.jumping && grounded) {
            Debug.Log("jumped");
            //jump animation
        }

        else if (grounded) {
            //if (rb.velocity.magnitude > 1f) {
            Debug.Log("walkin");
            //do walk animations
            //}
            //else {
            Debug.Log("Idle");
            //do idle animation
            //}
        }

        else {
            //in air animation
            if (rb.velocity.y > 0) {
                Debug.Log("goin up");
                //animation of going up
            }
            else {
                Debug.Log("comin down");
                //animation of coming down
            }
        }
    }

    private void Movement() {
        // Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * downForce);

        // Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();      //mag = magnitude

        // Counteract sliding and sloppy movement
        CounterMovement(mag);

        // Limit running diagonally
        LimitDiagonalRunning();

        // If holding jump && ready to jump, then jump
        Jump();

        // Some multipliers
        float multiplier = 1f;
        float multiplierV = 1f;

        // Movement in air
        if (!grounded) {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * userInput.y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * userInput.x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump() {
        if (grounded && readyToJump) {
            lastGroundedTime = Time.time;
        }

        if (userInput.jumping) {
            jumpButtonPressedTime = Time.time;
        }

        //if (grounded && readyToJump) {
        if (readyToJump && Time.time - lastGroundedTime <= jumpGraceTime)
            if (Time.time - jumpButtonPressedTime <= jumpGraceTime) {
                readyToJump = false;

                //Add jump forces
                //rb.AddForce(Vector2.up * jumpForce * 1.5f);
                rb.AddForce(normalVector * jumpForce * 2);
                rb.AddForce(orientation.transform.forward * userInput.y * moveSpeed * Time.deltaTime);

                //If userInput.jumping while falling, reset userInput.y velocity.
                Vector3 vel = rb.velocity;
                if (rb.velocity.y < 0.5f) {
                    rb.velocity = new Vector3(vel.x, 0, vel.z);
                }
                else if (rb.velocity.y > 0) {
                    rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
                }

                Invoke(nameof(ResetJump), jumpCooldown);

                lastGroundedTime = null;
                jumpButtonPressedTime = null;
            }

        // This is a bad way to fix the bug, but i have no idea whats causing it
        if (userInput.IsPressed() && userInput.jumping)
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

    }

    private void ResetJump() {
        readyToJump = true;
    }

    private float desiredX;
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.rotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(Vector2 mag) {
        //Counter movement
        CounterMovementForAxis(mag.x, userInput.x, orientation.transform.right);
        CounterMovementForAxis(mag.y, userInput.y, orientation.transform.forward);
    }

    void CounterMovementForAxis(float x, float input, Vector3 direction) {
        // Counter movement in a certain direction
        if (x < -threshold && input > 0.05f || x > threshold && input < -0.05f) {
            Vector3 force = moveSpeed * direction * Time.deltaTime * x * counterMovement;
            rb.AddForce(force * -1);
        }

        // Come to rest if there is no input
        else if (x < -threshold && input == 0 || x > threshold && input == 0) {
            if (grounded) {
                Vector3 force = moveSpeed * direction * Time.deltaTime * x * stopMovement;
                rb.AddForce(force * -1);
            }
        }
    }

    private void LimitDiagonalRunning() {
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-userInput.crouching, so not optimal.
        if (Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2)) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 vel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Vector3 n = vel.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other) {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;

                currentGravity = -normal * Physics.gravity.magnitude;

                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() {
        grounded = false;
        currentGravity = Physics.gravity;
    }
}
