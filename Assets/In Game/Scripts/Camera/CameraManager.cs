using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CameraInfluence _activeCam;

    /// <summary>
    /// Shows which camera is currently being used.
    /// If it's null, it means player is not looking at the screen
    /// </summary>
    public CameraInfluence activeCam {
        get => _activeCam;
        set {
            // Pause game when there is no active cam present
            if (value == null)
                isPlaying = false;

            _activeCam = value;
        }
    }

    /// <summary>
    /// Parent of all the cameras in the scene
    /// </summary>
    [SerializeField] Transform camParent;

    /// <summary>
    /// List of all cameraInfluences player must interact with
    /// </summary>
    private List<CameraInfluence> cameraInfluences = new List<CameraInfluence>();

    // set to pause mode by default
    public bool isRewinding { get; private set; } = false;
    public bool isForwarding { get; private set; } = false;
    public bool isPlaying { get; private set; } = false;

    public delegate void CameraActions();
    public CameraActions Rewind;
    public CameraActions Forward;
    public CameraActions Play;
    public CameraActions OnPlayPress;

    private void Awake() {
        instance = this;

        Rewind += EmptyFunc;
        Forward += EmptyFunc;
        Play += EmptyFunc;
        OnPlayPress += () => isPlaying = !isPlaying;

        GetAllCameras(camParent);
    }

    private void Update() {
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
