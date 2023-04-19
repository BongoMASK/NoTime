using UnityEngine;

public class CameraFieldOfView : MonoBehaviour {

    [SerializeField] Recorder recorder;
    [SerializeField] VisualPath visualPath;

    [SerializeField] new Collider collider;
    Plane[] cameraFrustum;

    private bool _isVisibleToCam;

    private bool isPlaying => CameraManager.instance.isPlaying;

    public bool isVisibleToCam {
        get => _isVisibleToCam;

        private set {
            if (value != isVisibleToCam) {
                if (value) {
                    recorder.LimitRigidbody(isPlaying);
                    OnEnteredCameraFrame();
                }
                else {
                    recorder.LimitRigidbody(false);
                    OnExitCameraFrame();
                }
            }
            _isVisibleToCam = value;
        }
    }

    public void Update() {
        isVisibleToCam = CheckIfInCamera();
    }

    /// <summary>
    /// If active camera can see it, return true.
    /// If it cant, return false.
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
        visualPath.enabled = true;
    }

    public void OnExitCameraFrame() {
        recorder.enabled = false;
        visualPath.enabled = false;
    }
}