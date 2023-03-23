using UnityEngine;

public class CameraFieldOfView : MonoBehaviour {

    [SerializeField] Recorder recorder;

    [SerializeField] new Collider collider;
    Plane[] cameraFrustum;

    private bool _isVisibleToCam;

    public bool isVisibleToCam {
        get => _isVisibleToCam;

        private set {
            if(value != isVisibleToCam) {
                if(value)
                    OnEnteredCameraFrame();
                else
                    OnExitCameraFrame();
            }
            _isVisibleToCam = value;
        }
    }

    public void Update() {
        isVisibleToCam = CheckIfInCamera();
    }

    /// <summary>
    /// Checks if the object is inside the camera field of view
    /// </summary>
    /// <param name="cam"></param>
    public bool CheckIfInCamera() {
        if (CameraManager.instance.activeCam == null)
            return false;

        Camera cam = CameraManager.instance.activeCam.cam;

        Bounds bounds = collider.bounds;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
            return true;

        return false;
    }

    public void OnEnteredCameraFrame() {
        recorder.enabled = true;

        CameraManager.instance.Rewind += recorder.Rewind;
        CameraManager.instance.Forward += recorder.Forward;
        CameraManager.instance.Play += recorder.Play;
        CameraManager.instance.OnPlayPress += recorder.OnPlayPress;
    }

    public void OnExitCameraFrame() {
        CameraManager.instance.Rewind -= recorder.Rewind;
        CameraManager.instance.Forward -= recorder.Forward;
        CameraManager.instance.Play -= recorder.Play;
        CameraManager.instance.OnPlayPress -= recorder.OnPlayPress;

        recorder.enabled = false;
    }
}
