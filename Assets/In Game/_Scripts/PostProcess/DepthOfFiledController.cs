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
        depthOfField.focusDistance.value = hitDistance;
    }

    //private void OnDrawGizmos() {
    //    if (isHit) {
    //        Gizmos.DrawSphere(hit.point, 0.1f);
    //        Debug.DrawRay(cam.position, cam.forward * Vector3.Distance(cam.position, hit.point));
    //    }
    //    else
    //        Debug.DrawRay(cam.position, cam.forward * 100f);
    //}
}
