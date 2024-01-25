using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum CameraMode {
    Pause = 0,
    Play, 
    Rewind,
    Forward
}

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;

    [SerializeField] private CameraInfluence _activeCam;

    /// <summary>
    /// Shows which camera is currently being used.
    /// If it's null, it means player is not looking at the screen, also locks input of camera
    /// </summary>
    public CameraInfluence activeCam {
        get => _activeCam;

        set {
            if (_activeCam != null)
                _activeCam.DeAssign();

            // Pause game when there is no active cam present
            if (value == null) {
                lockInput = true;
                Application.targetFrameRate = 1000;
            }
            else {
                Application.targetFrameRate = 24;
                
                cameraMode = CameraMode.Pause;

                lockInput = false;
                value.Assign();
            }

            _activeCam = value;
        }
    }

    [Header("Assignables")]

    public RenderTexture mainRenderTexture;

    /// <summary>
    /// Parent of all the cameras in the scene
    /// </summary>
    [Tooltip("Parent of all the cameras in the scene")]
    [SerializeField] Transform camParent;

    /// <summary>
    /// List of all cameraInfluences player must interact with
    /// </summary>
    private List<CameraInfluence> cameraInfluences = new List<CameraInfluence>();
    int activeCamIndex = 0;

    // Convert this to an enum later
    [Header("Camera Action Booleans")]

    [SerializeField] private bool _isRewinding = false;
    [SerializeField] private bool _isForwarding = false;
    [SerializeField] private bool _isPlaying = false;

    // set to pause mode by default
    //public bool isRewinding {
    //    get => _isRewinding;
    //    private set { _isRewinding = value; }
    //}
    //public bool isForwarding {
    //    get => _isForwarding;
    //    private set { _isForwarding = value; }
    //}
    //public bool isPlaying {
    //    get => _isPlaying;
    //    private set {
    //        _isPlaying = value;
    //        LimitRigidbody();
    //    }
    //}

    [SerializeField] private CameraMode _cameraMode = CameraMode.Pause;

    public CameraMode cameraMode {
        get => _cameraMode;
        private set {
            _cameraMode = value;
            LimitRigidbody();

            if (activeCam != null) {
                activeCam.cameraUI.DisplayPlayerInput(_cameraMode);
                //activeCam.SetPlaybackSpeed(_cameraMode);
            }
        }
    }

    public delegate void CameraActions();
    public CameraActions Rewind;
    public CameraActions Forward;
    public CameraActions Play;
    public CameraActions OnPlayPress;
    public CameraActions LimitRigidbody;

    [SerializeField] bool lockInput = true;

    private void Awake() {
        instance = this;

        Rewind += EmptyFunc;
        Forward += EmptyFunc;
        Play += EmptyFunc;
        LimitRigidbody += EmptyFunc;
        //OnPlayPress += () => isPlaying = !isPlaying;
        OnPlayPress +=  WhenPressedPlay;

        GetAllCameras(camParent);
    }

    private void Update() {
        GetInput();
    }

    private void FixedUpdate() {
        if (lockInput)
            return;

        switch (cameraMode) {
            case CameraMode.Pause:
                break;

            case CameraMode.Play:
                MakeCameraRecord();
                break;

            case CameraMode.Rewind:
                MakeCameraRewind();
                break;

            case CameraMode.Forward:
                MakeCameraForward();
                break;
        }

        //if (isRewinding)
        //    MakeCameraRewind();

        //else if (isForwarding)
        //    MakeCameraForward();

        //else if (isPlaying)
        //    MakeCameraRecord();
    }

    void GetInput() {
        if (lockInput)
            return;

        #region Camera Actions

        if (Input.GetKeyDown(KeyCode.A)) {
            OnRewindPress();
        }

        //if (Input.GetKeyUp(KeyCode.A) && isRewinding) {
        //    isRewinding = false;
        //}

        if (Input.GetKeyUp(KeyCode.A) && cameraMode == CameraMode.Rewind) {
            cameraMode = CameraMode.Pause;
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            OnForwardPress();
        }

        //if (Input.GetKeyUp(KeyCode.D) && isForwarding) {
        //    isForwarding = false;
        //}

        if (Input.GetKeyUp(KeyCode.D) && cameraMode == CameraMode.Forward) {
            cameraMode = CameraMode.Pause;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            OnPlayPress();

        #endregion

        #region Camera Switching

        if (Input.GetKeyDown(KeyCode.W)) {
            ChangeActiveCamera(1);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            ChangeActiveCamera(-1);
        }

        #endregion
    }

    public CameraInfluence FindActiveCam() {
        if (activeCamIndex >= cameraInfluences.Count || activeCamIndex < 0)
            activeCamIndex = 0;

        return cameraInfluences[activeCamIndex];
    }

    public void ChangeActiveCamera(int inc) {
        activeCamIndex += inc;

        activeCamIndex %= cameraInfluences.Count;

        if(activeCamIndex < 0)
            activeCamIndex = cameraInfluences.Count - 1;

        activeCam = cameraInfluences[activeCamIndex];
        activeCam.ChangeActiveCamText();

        UIManager.Instance.playerHelperUI.hasSwitchedCamera = true;
    }

    public void ChangeCameraParent(Transform parent) {
        camParent = parent;
        GetAllCameras(camParent);

        UIManager.Instance.playerHelperUI.ResetBools();
    }

    void GetAllCameras(Transform cameraParent) {
        cameraInfluences.Clear();

        for (int i = 0; i < cameraParent.childCount; i++) {
            cameraInfluences.Add(cameraParent.GetChild(i).GetComponent<CameraInfluence>());
        }
    }

    void MakeCameraRecord() {
        if (activeCam == null)
            return;

        if (activeCam.ShouldStopRecordingPlay())
            return;

        activeCam.videoPlaybackTime += Time.fixedDeltaTime;
        Play();
    }

    void MakeCameraForward() {
        if (activeCam == null)
            return;

        if (activeCam.ShouldStopRecordingForward())
            return;

        activeCam.videoPlaybackTime += Time.fixedDeltaTime;
        Forward();
    }

    void MakeCameraRewind() {
        if (activeCam == null)
            return;

        if (activeCam.ShouldStopRecordingRewind())
            return;

        activeCam.videoPlaybackTime -= Time.fixedDeltaTime;
        Rewind();
    }

    /// <summary>
    /// Sets all bools to false.
    /// Sets isRewinding to true.
    /// </summary>
    void OnRewindPress() {
        //isRewinding = true;
        //isForwarding = false;
        //isPlaying = false;

        cameraMode = CameraMode.Rewind;
        UIManager.Instance.playerHelperUI.hasRewind = true;
    }

    /// <summary>
    /// Sets all bools to false.
    /// Sets isForwarding to true.
    /// </summary>
    void OnForwardPress() {
        //isForwarding = true;
        //isRewinding = false;
        //isPlaying = false;

        cameraMode = CameraMode.Forward;
        UIManager.Instance.playerHelperUI.hasForward = true;
    }

    void WhenPressedPlay() {
        if (cameraMode == CameraMode.Play)
            cameraMode = CameraMode.Pause;
        else
            cameraMode = CameraMode.Play;

        UIManager.Instance.playerHelperUI.hasPlay = true;
    }

    void EmptyFunc() { }
}
