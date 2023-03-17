using UnityEngine;

public class CameraFieldOfView : MonoBehaviour {
    
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] new Collider collider;
    Plane[] cameraFrustum;

    [SerializeField] bool displayColour = false;

    /// <summary>
    /// Checks if the object is inside the camera field of view
    /// </summary>
    /// <param name="cam"></param>
    public bool CheckIfInCamera(Camera cam) {
        Bounds bounds = collider.bounds;
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(cameraFrustum, bounds))
            return true;

        return false;
    }

    /// <summary>
    /// Change material based on which camera is looking at 
    /// </summary>
    /// <param name="cam"></param>
    private void ChangeMatColour(Camera cam) {
        if (!displayColour)
            return;

        if (cam == null) {
            meshRenderer.material.color = Color.black;
            return;
        }

        meshRenderer.material.color = GetCameraColour(cam);
    }

    /// <summary>
    /// Gets the colour of the camera that is affecting the object.
    /// Used for Debugging and Level Design
    /// </summary>
    /// <param name="cam"></param>
    /// <returns></returns>
    private Color GetCameraColour(Camera cam) {
        return cam.gameObject.GetComponent<CameraInfluence>().colour;
    }
}
