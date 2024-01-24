using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraUI : MonoBehaviour
{
    [SerializeField] Slider videoSlider;
    [SerializeField] Image cameraActionImage;
    [SerializeField] TMP_Text cameraActionText;

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

        DOTween.Sequence()
            .Append(cameraActionText.DOFade(0, 0.01f))
            .Append(cameraActionText.DOFade(0, 1f))
            .Append(cameraActionText.DOFade(1f, 0.01f))
            .Append(cameraActionText.DOFade(1, 1f))
            .SetLoops(-1, LoopType.Restart);
    }

    private void Update() {
        if (CameraManager.instance.activeCam == null)
            return;

        // Show Camera video seconds
        videoSlider.value = CameraManager.instance.activeCam.videoPlaybackTime;
        videoSlider.maxValue = CameraManager.instance.activeCam.maxTime;

        currentTime = now.AddSeconds(videoSlider.value);

        currentTimeText.text = currentTime.ToString("h:mm:ss:ff tt");
    }

    public void DisplayPlayerInput(CameraMode camMode) {
        switch(camMode) {
            case CameraMode.Pause:
                cameraActionImage.sprite = pauseSprite;
                cameraActionText.text = "PAUSE";
                break;

            case CameraMode.Play:
                cameraActionImage.sprite = playSprite;
                cameraActionText.text = "PLAY";
                break;

            case CameraMode.Rewind:
                cameraActionImage.sprite = rewindSprite;
                cameraActionText.text = "REWIND";
                break;

            case CameraMode.Forward:
                cameraActionImage.sprite = forwardSprite;
                cameraActionText.text = "FORWARD";
                break;
        }
    }

    public void ChangeActiveCamText(string text) {
        activeCamText.text = text;
    }
}
