using UnityEngine;

public class Playback {

    /// <summary>
    /// Stores position data of the object
    /// </summary>
    public Vector3 position { get; set; }

    /// <summary>
    /// Stores the rotation of the object
    /// </summary>
    public Quaternion rotation { get; set; }

    public Playback(Vector3 position, Quaternion rotation) {
        this.position = position;
        this.rotation = rotation;
    }
}
