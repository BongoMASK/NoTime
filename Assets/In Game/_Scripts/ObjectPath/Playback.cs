using UnityEngine;

[System.Serializable]
public class Playback {

    /// <summary>
    /// Stores position data of the object
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// Stores the rotation of the object
    /// </summary>
    public Quaternion rotation;

    public Playback(Vector3 position, Quaternion rotation) {
        this.position = position;
        this.rotation = rotation;
    }
}
