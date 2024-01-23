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
    [SerializeField] TMP_Text activeCamText;

    DateTime now;
    DateTime currentTime;

    private void Start() {
        now = DateTime.Now;
    }

    private void Update() {
        // Show Camera video seconds
        if (CameraManager.instance.activeCam != null) {
            videoSlider.value = CameraManager.instance.activeCam.videoPlaybackTime;
            videoSlider.maxValue = CameraManager.instance.activeCam.maxTime;

            currentTime = now.AddSeconds(videoSlider.value);

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

    public void ChangeActiveCamText(string text) {
        activeCamText.text = text;
    }
}
