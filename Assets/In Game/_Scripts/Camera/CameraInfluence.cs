using UnityEngine;

public class CameraInfluence : MonoBehaviour {

    public float maxTime = 10;
    public float videoPlaybackTime = 0;

    public Camera cam;

    public CameraUI cameraUI => UIManager.Instance.cameraUI;

    public float cameraRange = 10;

    private void Start() {
        cameraUI.ChangeActiveCamText("CAM " + (transform.GetSiblingIndex() + 1) + "/" + transform.parent.childCount);
    }

    public bool ShouldStopRecordingRewind() {
        if (videoPlaybackTime < 0)
            return true;

        return false;
    }

    public bool ShouldStopRecordingForward() {
        if (videoPlaybackTime > maxTime)
            return true;

        return false;
    }

    public bool ShouldStopRecordingPlay() {
        if (videoPlaybackTime > maxTime)
            return true;

        return false;
    }

    public void DeAssign() {
        cam.targetTexture = null;
    }

    public void Assign() {
        cam.targetTexture = CameraManager.instance.mainRenderTexture;
    }

    private void OnDrawGizmosSelected() {
        //Gizmos.DrawMesh()
        Gizmos.DrawWireSphere(cam.transform.position, cameraRange);
    }
}
