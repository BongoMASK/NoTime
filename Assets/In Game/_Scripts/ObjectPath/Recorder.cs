using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VisualPath))]
[RequireComponent(typeof(CameraFieldOfView))]
[RequireComponent(typeof(Rigidbody))]

public class Recorder : MonoBehaviour {

    public Stack<Playback> rewindPath = new Stack<Playback>();
    public Stack<Playback> forwardPath = new Stack<Playback>();

    [Header("Assignables")]
    [SerializeField] Rigidbody rb;

    [Header("Values")]
    [SerializeField] Vector3 startForce;

    bool isPlaying => CameraManager.instance.cameraMode == CameraMode.Play;

    private void Awake() {
        if (TryGetComponent(out PreRecorder playback))
            rewindPath = new Stack<Playback>(playback.rewindList);
    }

    private void Start() {
        rb.AddForce(startForce);
    }

    private void OnEnable() {
        CameraManager.instance.Rewind += Rewind;
        CameraManager.instance.Forward += Forward;

        CameraManager.instance.Play += Play;
        CameraManager.instance.OnPlayPress += OnPlayPress;

        CameraManager.instance.LimitRigidbody += LimitRigidbody;
    }

    private void OnDisable() {
        CameraManager.instance.Rewind -= Rewind;

        CameraManager.instance.Forward -= Forward;

        CameraManager.instance.Play -= Play;
        CameraManager.instance.OnPlayPress -= OnPlayPress;

        CameraManager.instance.LimitRigidbody -= LimitRigidbody;
    }

    /// <summary>
    /// Adds position data to the Rewind stack on FixedUpdate.
    /// Used when physics needs to be applied on the object
    /// </summary>
    public void Play() {
        rewindPath.Push(new Playback(transform.localPosition, transform.rotation));
    }

    /// <summary>
    /// Adds position data to the Forward stack on FixedUpdate.
    /// Used when we need the object to trace back its Rewind path.
    /// No physics are being applied on the object.
    /// </summary>
    public void Rewind() {
        if (rewindPath.Count <= 0)
            return;

        Playback lastPlayback = rewindPath.Pop();

        transform.localPosition = lastPlayback.position;
        transform.rotation = lastPlayback.rotation;

        forwardPath.Push(lastPlayback);
    }

    /// <summary>
    /// Gives back position data to the Rewind Stack on FixedUpdate
    /// Used when we need the object to trace back its Forward path.
    /// No physics are being applied on the object.
    /// </summary>
    public void Forward() {
        if (forwardPath.Count <= 0)
            return;

        Playback lastPlayback = forwardPath.Pop();

        transform.localPosition = lastPlayback.position;
        transform.rotation = lastPlayback.rotation;

        rewindPath.Push(lastPlayback);
    }

    public void OnPlayPress() {
        if (isPlaying) {
            rb.AddForce(rb.mass * CalculateForce());
            forwardPath.Clear();
        }
    }

    Vector3 CalculateForce() {
        if (rewindPath.Count <= 0)
            return startForce;

        Vector3 distance = (transform.localPosition - rewindPath.Peek().position);
        Vector3 velocity = distance / Time.fixedDeltaTime;
        Vector3 acceleration = velocity / Time.fixedDeltaTime;

        return acceleration;
    }

    public void LimitRigidbody(bool isPlaying) {
        if (isPlaying) {
            rb.constraints = RigidbodyConstraints.None;
            return;
        }

        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    public void LimitRigidbody() {
        if (isPlaying) {
            rb.constraints = RigidbodyConstraints.None;
            return;
        }

        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawRay(transform.position, startForce / 10);
    }
}
