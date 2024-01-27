using UnityEngine;

public class BGMusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public static BGMusicPlayer instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }

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
