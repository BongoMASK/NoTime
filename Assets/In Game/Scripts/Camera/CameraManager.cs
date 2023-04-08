using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CameraInfluence _activeCam;

    /// <summary>
    /// Shows which camera is currently being used.
    /// If it's null, it means player is not looking at the screen, also locks input of camera
    /// </summary>
    public CameraInfluence activeCam {
        get => _activeCam;

        set {
            // Pause game when there is no active cam present
            if (value == null) {
                lockInput = true;
            }
            else {
                // It's better to set these values to false when you assign a camera,
                // than set it to false when you remove the camera
                // slightly messy and difficult to understand
                // review later
                isPlaying = false;
                isRewinding = false;
                isForwarding = false;

                lockInput = false;
            }

            _activeCam = value;
        }
    }

    [Header("Assignables")]
    /// <summary>
    /// Parent of all the cameras in the scene
    /// </summary>
    [Tooltip("Parent of all the cameras in the scene")]
    [SerializeField] Transform camParent;

    /// <summary>
    /// List of all cameraInfluences player must interact with
    /// </summary>
    private List<CameraInfluence> cameraInfluences = new List<CameraInfluence>();

    [Header("Camera Action Booleans")]

    [SerializeField] private bool _isRewinding = false;
    [SerializeField] private bool _isForwarding = false;
    [SerializeField] private bool _isPlaying = false;

    // set to pause mode by default
    public bool isRewinding {
        get => _isRewinding;
        private set { _isRewinding = value; }
    }
    public bool isForwarding {
        get => _isForwarding;
        private set { _isForwarding = value; }
    }
    public bool isPlaying {
        get => _isPlaying;
        private set { _isPlaying = value; }
    }

    public delegate void CameraActions();
    public CameraActions Rewind;
    public CameraActions Forward;
    public CameraActions Play;
    public CameraActions OnPlayPress;
    public CameraActions OnCameraStop;

    [SerializeField] bool lockInput = true;

    private void Awake() {
        instance = this;

        Rewind += EmptyFunc;
        Forward += EmptyFunc;
        Play += EmptyFunc;
        OnPlayPress += () => isPlaying = !isPlaying;

        GetAllCameras(camParent);
    }

    private void Update() {
        if (lockInput)
            return;

        if (Input.GetKeyDown(KeyCode.A)) {
            OnRewindPress();
        }

        if (Input.GetKeyUp(KeyCode.A) && isRewinding) {
            isRewinding = false;
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            OnForwardPress();
        }

        if (Input.GetKeyUp(KeyCode.D) && isForwarding) {
            isForwarding = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            OnPlayPress();
    }

    private void FixedUpdate() {
        if (lockInput)
            return;

        if (isRewinding)
            Rewind();

        else if (isForwarding)
            Forward();

        else if (isPlaying)
            Play();
    }

    void GetAllCameras(Transform cameraParent) {
        for (int i = 0; i < cameraParent.childCount; i++) {
            cameraInfluences.Add(cameraParent.GetChild(i).GetComponent<CameraInfluence>());
        }
    }

    void OnRewindPress() {
        isRewinding = true;
        isForwarding = false;
        isPlaying = false;
    }

    void OnForwardPress() {
        isForwarding = true;
        isRewinding = false;
        isPlaying = false;
    }

    void EmptyFunc() { }
}
