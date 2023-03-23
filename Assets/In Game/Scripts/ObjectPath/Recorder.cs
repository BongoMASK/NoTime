using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour {

    public Stack<Playback> rewindPath = new Stack<Playback>();
    public Stack<Playback> forwardPath = new Stack<Playback>();

    [Header("Assignables")]
    [SerializeField] Rigidbody rb;
    [SerializeField] VisualPath vp;

    [Header("Values")]
    [SerializeField] Vector3 startForce;

    bool isRewinding = false;
    bool isForwarding = false;
    bool isPlaying { get => CameraManager.instance.isPlaying; }
    bool limitRB { get => !isPlaying; }

    private void Start() {
        rb.AddForce(startForce);
    }

    private void Update() {
        LimitRigidbody();

        //if (Input.GetKeyDown(KeyCode.A)) {
        //    isRewinding = true;
        //    isForwarding = false;
        //    isPlaying = false;
        //    //rb.isKinematic = true;
        //    limitRB = true;
        //}

        //if (Input.GetKeyUp(KeyCode.A) && isRewinding) {
        //    isRewinding = false;
        //}

        //if (Input.GetKeyDown(KeyCode.D)) {
        //    isForwarding = true;
        //    isRewinding = false;
        //    isPlaying = false;
        //    //rb.isKinematic = true;
        //    limitRB = true;
        //}

        //if (Input.GetKeyUp(KeyCode.D) && isForwarding) {
        //    isForwarding = false;
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //    OnPlayPress();
    }

    private void FixedUpdate() {
        //if (isRewinding)
        //    Rewind();

        //else if (isForwarding)
        //    Forward();

        //else if (isPlaying)
        //    Play();
    }

    public void Play() {
        rewindPath.Push(new Playback(transform.localPosition, transform.rotation));
        vp.AddRewindPos();
    }

    public void Rewind() {
        if (rewindPath.Count <= 0)
            return;

        Playback lastPlayback = rewindPath.Pop();

        transform.localPosition = lastPlayback.position;
        transform.rotation = lastPlayback.rotation;

        forwardPath.Push(lastPlayback);

        vp.RemoveRewindPos();
        vp.AddForwardPos();
    }

    public void Forward() {
        if (forwardPath.Count <= 0)
            return;

        Playback lastPlayback = forwardPath.Pop();

        transform.localPosition = lastPlayback.position;
        transform.rotation = lastPlayback.rotation;

        rewindPath.Push(lastPlayback);

        vp.RemoveForwardPos();
        vp.AddRewindPos();
    }

    public void OnPlayPress() {
        if (isPlaying) {
            rb.AddForce(rb.mass * CalculateForce());
            forwardPath.Clear();
        }

        vp.ClearForwardLine();
    }

    Vector3 CalculateForce() {
        if (rewindPath.Count <= 0)
            return startForce;

        Vector3 distance = (transform.localPosition - rewindPath.Peek().position);
        Vector3 velocity = distance / Time.fixedDeltaTime;
        Vector3 acceleration = velocity / Time.fixedDeltaTime;

        return acceleration;
    }

    public void LimitRigidbody() {
        if (!limitRB) {
            rb.constraints = RigidbodyConstraints.None;
            return;
        }

        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }
    
    private void OnDrawGizmosSelected() {
        // TODO: Show path stored inside of object in editor window
    }
}