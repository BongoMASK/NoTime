using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour {
    public Stack<Playback> rewindPath = new Stack<Playback>();
    public Stack<Playback> forwardPath = new Stack<Playback>();
    [SerializeField] Rigidbody rb;
    [SerializeField] VisualPath vp;

    [SerializeField] Vector3 startForce;

    bool isRewinding = false;
    bool isForwarding = false;
    bool isPlaying = true;

    private Vector3 _lastPlayerPos = Vector3.zero;
    Vector3 startPos;

    Vector3 lastPlayerPos {
        get {
            if (rewindPath.Count > 0)
                return _lastPlayerPos;
            return startPos;
        }
        set {
            _lastPlayerPos = value;
        }
    }

    private void Start() {
        rb.AddForce(startForce);
        startPos = transform.position;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            isRewinding = true;
            isForwarding = false;
            isPlaying = false;
            rb.isKinematic = true;
        }

        if (Input.GetKeyUp(KeyCode.A) && isRewinding) {
            isRewinding = false;
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            isForwarding = true;
            isRewinding = false;
            isPlaying = false;
            rb.isKinematic = true;
        }

        if (Input.GetKeyUp(KeyCode.D) && isForwarding) {
            isForwarding = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Play();
    }

    private void FixedUpdate() {
        if (isRewinding)
            Rewind();

        else if (isForwarding)
            Forward();

        else if (isPlaying)
            RecordData();

        lastPlayerPos = transform.position;
    }

    void RecordData() {
        rewindPath.Push(new Playback(transform.position - lastPlayerPos, transform.rotation));
        vp.AddRewindPos();
    }

    void Rewind() {
        isPlaying = false;

        if (rewindPath.Count <= 0)
            return;

        Playback lastPlayback = rewindPath.Pop();

        transform.position -= lastPlayback.position;
        transform.rotation = lastPlayback.rotation;

        forwardPath.Push(lastPlayback);

        vp.RemoveRewindPos();
        vp.AddForwardPos();
    }

    void Forward() {
        isPlaying = false;

        if (forwardPath.Count <= 0)
            return;

        Playback lastPlayback = forwardPath.Pop();

        transform.position += lastPlayback.position;
        transform.rotation = lastPlayback.rotation;

        rewindPath.Push(lastPlayback);

        vp.RemoveForwardPos();
        vp.AddRewindPos();
    }

    void Play() {
        isPlaying = !isPlaying;
        rb.isKinematic = !isPlaying;

        if (isPlaying) {
            rb.AddForce(rb.mass * CalculateForce());
            forwardPath.Clear();
        }

        vp.ClearForwardLine();
    }

    Vector3 CalculateForce() {
        if (forwardPath.Count <= 0 || rewindPath.Count <= 0)
            return startForce;

        Vector3 distance = (forwardPath.Peek().position);
        Vector3 velocity = distance / Time.fixedDeltaTime;
        Vector3 acceleration = velocity / Time.fixedDeltaTime;

        return acceleration;
    }
}