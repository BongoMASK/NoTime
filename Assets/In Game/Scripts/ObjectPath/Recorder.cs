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
    [SerializeField] VisualPath vp;

    [Header("Values")]
    [SerializeField] Vector3 startForce;

    bool isPlaying { get => CameraManager.instance.isPlaying; }

    private void Awake() {

        if(TryGetComponent(out PreRecorder playback)) {
            rewindPath = new Stack<Playback>(playback.rewindList);

            foreach(Playback p in playback.rewindList) 
                vp.AddRewindPos(p.position);
        }
    }

    private void Start() {
        rb.AddForce(startForce);
    }

    public void Play() {
        rewindPath.Push(new Playback(transform.localPosition, transform.rotation));

        if (vp != null) 
            vp.AddRewindPos();
    }

    public void Rewind() {
        if (rewindPath.Count <= 0) 
            return;

        Playback lastPlayback = rewindPath.Pop();

        transform.localPosition = lastPlayback.position;
        transform.rotation = lastPlayback.rotation;

        forwardPath.Push(lastPlayback);

        if (vp != null) {
            vp.RemoveRewindPos();
            vp.AddForwardPos();
        }
    }

    public void Forward() {
        if (forwardPath.Count <= 0)
            return;

        Playback lastPlayback = forwardPath.Pop();

        transform.localPosition = lastPlayback.position;
        transform.rotation = lastPlayback.rotation;

        rewindPath.Push(lastPlayback);

        if (vp != null) {
            vp.RemoveForwardPos();
            vp.AddRewindPos();
        }
    }

    public void OnPlayPress() {
        if (isPlaying) {
            rb.AddForce(rb.mass * CalculateForce());
            forwardPath.Clear();
        }

        if (vp != null) {
            vp.ClearForwardLine();
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
        Debug.Log("hi");
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
}