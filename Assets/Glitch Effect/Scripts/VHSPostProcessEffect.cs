using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

//[ExecuteInEditMode]
[AddComponentMenu("Image Effects/GlitchEffect")]
[RequireComponent(typeof(VideoPlayer))]
public class VHSPostProcessEffect : MonoBehaviour
{
	public Shader shader;
	public VideoClip VHSClip;
	[SerializeField] RawImage rawImage;

	private float _yScanline;
	private float _xScanline;
	private Material _material = null;
	private VideoPlayer videoPlayer;

	void Start()
	{
		_material = new Material(shader);
		videoPlayer = GetComponent<VideoPlayer>();
		videoPlayer.isLooping = true;
		videoPlayer.renderMode = VideoRenderMode.APIOnly;
		videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
		videoPlayer.clip = VHSClip;
		videoPlayer.Play();

		rawImage.material = _material;
	}

    private void Update() {
        _material.SetTexture("_VHSTex", videoPlayer.texture);

        _yScanline += Time.deltaTime * 0.01f;
        _xScanline -= Time.deltaTime * 0.1f;

        if (_yScanline >= 1) {
            _yScanline = Random.value;
        }
        if (_xScanline <= 0 || Random.value < 0.05) {
            _xScanline = Random.value;
        }
        _material.SetFloat("_yScanline", _yScanline);
        _material.SetFloat("_xScanline", _xScanline);
        rawImage.material = _material;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Debug.Log("fdkakjfdjaskl;d");
		_material.SetTexture("_VHSTex", videoPlayer.texture);

		_yScanline += Time.deltaTime * 0.01f;
		_xScanline -= Time.deltaTime * 0.1f;

		if (_yScanline >= 1)
		{
			_yScanline = Random.value;
		}
		if (_xScanline <= 0 || Random.value < 0.05)
		{
			_xScanline = Random.value;
		}
		_material.SetFloat("_yScanline", _yScanline);
		_material.SetFloat("_xScanline", _xScanline);

		Graphics.Blit(source, destination, _material);
	}

	protected void OnDisable()
	{
		if (_material)
		{
			DestroyImmediate(_material);
		}
	}
}
