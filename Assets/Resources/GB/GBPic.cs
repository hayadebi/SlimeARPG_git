using UnityEngine;


[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class GBPic : MonoBehaviour
{
	// 調整用パラメーター
	[SerializeField] float _resolution_ratio = 0.5f;								// ターゲットとなる解像度の比率
	[SerializeField] bool _dither = true;											// ディザ加工するか？
	[SerializeField] bool _scanning_line = true;									// 走査線演出を入れるか？
	[SerializeField] float _scanning_line_threshold = 0.35f;						// 走査線演出の閾値
	[SerializeField] float _contrast = 1.2f;										// コントラスト調整
	[SerializeField] Color _color_filter = new Color(0.650f, 0.901f, 0.3f, 1.0f);	// カラーフィルター

	private Material _material;
	private int _pid_ResolutionRatioP;
	private int _pid_DitherP;
	private int _pid_ScanningLineP;
	private int _pid_ScanningLineThresholdP;
	private int _pid_ContrastP;
	private int _pid_ColorFilterP;


	private void Awake()
	{
		Shader shader = Shader.Find("Hidden/GBPic");
		if (shader)
		{
			_material = new Material(shader);

			_pid_ResolutionRatioP = Shader.PropertyToID("_ResolutionRatioP");
			_pid_DitherP = Shader.PropertyToID("_DitherP");
			_pid_ScanningLineP = Shader.PropertyToID("_ScanningLineP");
			_pid_ScanningLineThresholdP = Shader.PropertyToID("_ScanningLineThresholdP");
			_pid_ContrastP = Shader.PropertyToID("_ContrastP");
			_pid_ColorFilterP = Shader.PropertyToID("_ColorFilterP");
		}
		else
		{
			Debug.LogError("not found shader [GBPic]");
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_material)
		{
			_material.SetFloat(_pid_ResolutionRatioP, _resolution_ratio);
			_material.SetFloat(_pid_DitherP, _dither ? 1.0f : 0.0f);
			_material.SetFloat(_pid_ScanningLineP, _scanning_line ? 1.0f : 0.0f);
			_material.SetFloat(_pid_ScanningLineThresholdP, _scanning_line_threshold);
			_material.SetFloat(_pid_ContrastP, _contrast);
			_material.SetColor(_pid_ColorFilterP, _color_filter);

			Graphics.Blit(source, destination, _material);
		}
	}
}

