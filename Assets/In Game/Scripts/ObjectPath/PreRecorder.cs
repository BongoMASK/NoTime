using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Recorder))]
public class PreRecorder : MonoBehaviour
{
    [SerializeField] public List<Playback> rewindList = new List<Playback>();

    [SerializeField] bool shouldRecord = true;

    private void FixedUpdate() {
        if (shouldRecord)
            PreRecord();
    }

    void PreRecord() {
        rewindList.Add(new Playback(transform.localPosition, transform.rotation));
    }
}
