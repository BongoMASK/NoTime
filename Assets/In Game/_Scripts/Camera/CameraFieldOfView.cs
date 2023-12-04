using UnityEngine;

public class CameraFieldOfView : MonoBehaviour {

    [SerializeField] Recorder recorder;
    [SerializeField] VisualPath visualPath;

    [SerializeField] new Collider collider;
    Plane[] cameraFrustum;

    private bool _isVisibleToCam;

    private bool isPlaying => CameraManager.instance.cameraMode == CameraMode.Play;

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
        //Debug.Log(isVisibleToCam);
    }

    /// <summary>
    /// If active camera can see it, return true.
    /// If it cant, return false.
    /// </summary>
    /// <param name="cam"></param>
    public bool CheckIfInCamera() {
        if (CameraManager.instance.activeCam == null)
            return false;

        // Check distance to Camera
        if((transform.position - CameraManager.instance.activeCam.transform.position).magnitude > CameraManager.instance.activeCam.cameraRange)
            return false;

        Camera cam = CameraManager.instance.activeCam.cam;

        Bounds bounds = collider.bounds;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
            return true;

        /// This was some code we tried out to keep objects far away from the camera undetectable 
        /// It worked fine in that regard, but makes it so that any object will obstruct the camera view
        /// We changed it to a simple distance 

        //Vector3 dir = transform.position - CameraManager.instance.activeCam.transform.position;
        //if (Physics.Raycast(CameraManager.instance.activeCam.transform.position, dir, out RaycastHit hitInfo)) {
        //    if(hitInfo.transform== transform)
        //        return true;
        //}

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