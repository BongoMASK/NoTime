using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class CameraInfluence : MonoBehaviour {

    public float maxTime = 10;
    public float videoPlaybackTime = 0;

    public Camera cam;

    public CameraUI cameraUI => UIManager.Instance.cameraUI;

    [SerializeField] VideoPlayer videoPlayer;

    public float cameraRange = 10;

    private void Start() {
        ChangeActiveCamText();
    }

    private void Update() {
        if (!videoPlayer.gameObject.activeInHierarchy)
            return;

        //SetPlaybackSpeed(CameraManager.instance.cameraMode);
    }

    public void SetPlaybackSpeed(CameraMode action) {
        switch (action) {
            case CameraMode.Play:
                videoPlayer.playbackSpeed = 1;
                break;

            case CameraMode.Pause: 
                videoPlayer.playbackSpeed = 0;
                break;

            case CameraMode.Rewind:
                RewindVideoPlayer();
                break;

            case CameraMode.Forward: 
                videoPlayer.playbackSpeed = 1;
                break;
        }
    }

    private void RewindVideoPlayer() {
        videoPlayer.time -= Time.deltaTime;

        if (videoPlayer.time == 0) {
            float duration = videoPlayer.frameCount / videoPlayer.frameRate;
            videoPlayer.time = duration;
        }

        Debug.Log(videoPlayer.time);

        //StartCoroutine(Cor_RewindVideoPlayer());
    }

    IEnumerator Cor_RewindVideoPlayer() {
        videoPlayer.playbackSpeed = 0;
        while(CameraManager.instance.cameraMode == CameraMode.Rewind) {
            videoPlayer.time -= Time.deltaTime;

            if (videoPlayer.time == 0) {
                float duration = videoPlayer.frameCount / videoPlayer.frameRate;
                videoPlayer.time = duration;
            }

            Debug.Log(videoPlayer.time);
            yield return null;
        }
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
        videoPlayer.playbackSpeed = 0;
        videoPlayer.gameObject.SetActive(false);
    }

    public void Assign() {
        cam.targetTexture = CameraManager.instance.mainRenderTexture;
        videoPlayer.gameObject.SetActive(true);
    }

    public void ChangeActiveCamText() {
        cameraUI.ChangeActiveCamText("CAM " + (transform.GetSiblingIndex() + 1) + "/" + transform.parent.childCount);
    }

    private void OnDrawGizmosSelected() {
        //Gizmos.DrawMesh()
        Gizmos.DrawWireSphere(cam.transform.position, cameraRange);
    }
}
