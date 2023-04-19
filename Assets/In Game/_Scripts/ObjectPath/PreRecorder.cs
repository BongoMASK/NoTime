using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Recorder))]
public class PreRecorder : MonoBehaviour {
    [SerializeField] public List<Playback> rewindList = new List<Playback>();
    [SerializeField] bool shouldRecord = true;

    public void Start() {
        if (!shouldRecord) {
            transform.localPosition = rewindList[rewindList.Count - 1].position;
            transform.rotation = rewindList[rewindList.Count - 1].rotation;
        }
    }

    private void FixedUpdate() {
        if (shouldRecord)
            PreRecord();
    }

    void PreRecord() {
        rewindList.Add(new Playback(transform.localPosition, transform.rotation));
    }
}