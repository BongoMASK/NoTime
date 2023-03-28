using System.Collections.Generic;
using UnityEngine;

public class PreRecorder : MonoBehaviour
{
    [SerializeField] public List<Playback> rewindList = new List<Playback>();

    private void FixedUpdate() {
        //PreRecord();
    }

    void PreRecord() {
        rewindList.Add(new Playback(transform.localPosition, transform.rotation));
    }
}
