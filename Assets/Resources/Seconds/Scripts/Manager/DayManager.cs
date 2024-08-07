using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public Light directionalLight; // ディレクショナルライトを割り当てます
    //public float DataManager.instance.sunTime = 0f; // ゲーム内時間を追跡します
    public float dayLengthInMinutes = 360f; // 1日の長さ（分）
    //private float DataManager.instance.daycount = 0f; // 日数カウント
    public Renderer materials;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        DataManager.instance.sunTime += 1;
    }
    private void Update()
    {
        // ゲーム内時間を進める
        DataManager.instance.sunTime += (Time.deltaTime / 60f);

        // 時間カウントを0に戻し、日にちカウントを+1する
        if (DataManager.instance.sunTime >= dayLengthInMinutes)
        {
            DataManager.instance.sunTime = 0f;
            DataManager.instance.daycount += 1;
            DataManager.instance.AllSaveInvoke();
        }

        // 環境ライティングの強度とライト強度等を調整する
        UpdateLighting(DataManager.instance.sunTime);
    }

    private void UpdateLighting(float currentTime)
    {
        float directionalLightIntensity = 0f;
        float environmentLightingIntensity = 0f;

        // 環境ライティングの強度の乗数を調整
        if (currentTime >= 0f && currentTime <= 180f)
        {
            environmentLightingIntensity = Mathf.Lerp(0f, 1f, currentTime / 90f);
        }
        else if (currentTime >= 181f && currentTime <= 360f)
        {
            environmentLightingIntensity = Mathf.Lerp(1f, 0.05f, (currentTime - 271f) / 89f);
        }

        // ディレクショナルライトの強度を調整
        if (currentTime >= 0 && currentTime <= 180f)
        {
            directionalLightIntensity = Mathf.Lerp(0f, 1f, (currentTime - 91f) / 89f);
        }
        else if (currentTime >= 181f && currentTime <= 360f)
        {
            directionalLightIntensity = Mathf.Lerp(1f, 0.05f, (currentTime - 181f) / 89f);
        }

        // ライトと環境ライティングの強度を設定
        directionalLight.intensity = directionalLightIntensity;
        RenderSettings.ambientIntensity = environmentLightingIntensity;

        //日差しの強度を設定
        float sunPower = 0;
        if (currentTime >= 0f && currentTime <= 180f)
        {
            sunPower = Mathf.Lerp(0f, 0.9f, currentTime / 180f);
        }
        else if (currentTime >= 181f && currentTime <= 360f)
        {
            sunPower = Mathf.Lerp(0.9f, 0f, (currentTime - 181f) / 179f);
        }
            Color materialColor = materials.material.GetColor("_Color1");
        materialColor.a = sunPower;
        materials.material.SetColor("_Color1", materialColor);
    }
}
