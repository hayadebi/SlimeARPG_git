using UnityEngine;
using System.Collections;
using UnityEngine.UI; // ←※これを忘れずに入れる

public class unityside_slider : MonoBehaviour
{
    public string sliderType = "";
    Slider _slider;
    float addnumber = 0;
    public enemyS es;
    public string bossname;
    int oldbosshp;
    GameObject boss;
    enemyS script;
    void Start()
    {
        // スライダーを取得する
        _slider = this.GetComponent<Slider>();
        GManager.instance.audioMax = PlayerPrefs.GetFloat("audioMax", GManager.instance.audioMax);
        GManager.instance.seMax = PlayerPrefs.GetFloat("seMax", GManager.instance.seMax);
        GManager.instance.mode = PlayerPrefs.GetInt("mode", GManager.instance.mode);

        GManager.instance.kando = PlayerPrefs.GetFloat("kando", GManager.instance.kando);
        if (sliderType == "audio")
        {
            _slider.maxValue = 1f;
            _slider.value = GManager.instance.audioMax;
        }
       else  if (sliderType == "se")
        {
            _slider.maxValue = 1f;
            _slider.value = GManager.instance.seMax;
        }
        else if (sliderType == "mode")
        {

            _slider.maxValue = 2;
            _slider.value = GManager.instance.mode;
        }
        else if (sliderType == "kando")
        {
            _slider.maxValue = 8f;
            _slider.minValue = 0.4f;
            _slider.value = GManager.instance.kando;
        }
    }

    void Update()
    {
        if (sliderType == "audio" && _slider.value != GManager.instance.audioMax)
        {
            GManager.instance.audioMax=_slider.value;
            PlayerPrefs.SetFloat("audioMax", GManager.instance.audioMax);
            PlayerPrefs.Save();
        }
        else if (sliderType == "se" && _slider.value != GManager.instance.seMax)
        {
            GManager.instance.seMax=_slider.value;
            PlayerPrefs.SetFloat("seMax", GManager.instance.seMax);
            PlayerPrefs.Save();
        }
        else if (sliderType == "mode" && (int)_slider.value != (int)GManager.instance.mode)
        {
            GManager.instance.mode=(int)_slider.value;
            PlayerPrefs.SetInt("mode", (int)GManager.instance.mode);
            PlayerPrefs.Save();
        }
        else if (sliderType == "kando" && _slider.value != GManager.instance.kando)
        {
            GManager.instance.kando=_slider.value;
            PlayerPrefs.SetFloat("kando", GManager.instance.kando);
            PlayerPrefs.Save();
        }

    }
}