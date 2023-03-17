using UnityEngine;

public class CameraInfluence : MonoBehaviour {
    public Color colour = Color.green;
    [SerializeField] bool showCameraRange = false;

    /// <summary>
    /// Creates a mesh that displays the range of that camera. Used for debugging and level design
    /// </summary>
    public void ShowCameraRange() {
        if (!showCameraRange)
            return;

        // Creates a mesh that displays the range of that camera
    }
}
