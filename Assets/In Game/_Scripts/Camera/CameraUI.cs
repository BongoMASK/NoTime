using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    [SerializeField] Slider videoSlider;
    [SerializeField] Image cameraActionImage;

    [SerializeField] Sprite pauseSprite;
    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite rewindSprite;
    [SerializeField] Sprite forwardSprite;

    private void Update() {
        // Show Camera video seconds
        if (CameraManager.instance.activeCam != null)
            videoSlider.value = CameraManager.instance.activeCam.videoPlaybackTime;
    }

    public void DisplayPlayerInput(CameraMode camMode) {
        switch(camMode) {
            case CameraMode.Pause:
                cameraActionImage.sprite = pauseSprite;
                break;

            case CameraMode.Play:
                cameraActionImage.sprite = playSprite;
                break;

            case CameraMode.Rewind:
                cameraActionImage.sprite = rewindSprite;
                break;

            case CameraMode.Forward:
                cameraActionImage.sprite = forwardSprite;
                break;
        }
    }
}
