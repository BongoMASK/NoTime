using UnityEngine;

public class BGMusicPlayer : MonoBehaviour
{

    [SerializeField] AudioSource source;

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        source.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
