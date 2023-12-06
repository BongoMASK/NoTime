using System;
using TMPro;
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

    [SerializeField] TMP_Text currentTimeText;

    DateTime now;
    DateTime currentTime;

    private void Start() {
        now = DateTime.Now;
        Debug.Log(now.ToString("h:mm:ss tt"));
    }

    private void Update() {
        // Show Camera video seconds
        if (CameraManager.instance.activeCam != null) {
            videoSlider.value = CameraManager.instance.activeCam.videoPlaybackTime;

            currentTime = now.AddSeconds(videoSlider.value);
            //Debug.Log(currentTime.ToString("h:mm:ss:ff tt"));

            currentTimeText.text = currentTime.ToString("h:mm:ss:ff tt");
        }
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
