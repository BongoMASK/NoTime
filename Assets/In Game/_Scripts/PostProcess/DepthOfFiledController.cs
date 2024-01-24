using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthOfFiledController : MonoBehaviour
{
    Ray raycast;
    RaycastHit hit;
    bool isHit;
    float hitDistance;

    public Volume volume;

    DepthOfField depthOfField;

    Transform cam;

    private void Start() {
        cam = Camera.main.transform;
        volume.profile.TryGet(out depthOfField);
    }

    private void Update() {
        if (CameraManager.instance.activeCam != null) {
            depthOfField.active = false;
            return;
        }
        else
            depthOfField.active = true;

        raycast = new Ray(cam.position, cam.forward * 100);

        isHit = false;

        if (Physics.Raycast(raycast, out hit, 100f)) {
            isHit = true;
            hitDistance = Vector3.Distance(cam.position, hit.point);
        }
        else {
            if (hitDistance < 100f)
                hitDistance++;
        }

        SetFocus();
    }

    private void SetFocus() {
        if (hitDistance < 1) {
            return;
        }

        depthOfField.focusDistance.value = hitDistance;
    }
}
