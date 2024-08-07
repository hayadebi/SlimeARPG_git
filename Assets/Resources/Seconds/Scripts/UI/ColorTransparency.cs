using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransparency : MonoBehaviour
{
    public Color targetColor; // 透過させたい色
    public float tolerance = 0.1f; // 色の許容範囲
    private Image uiImage; // 操作対象のUIImage
    private Sprite oldsprite;
    void Start()
    {
        if(GetComponent<Image>()) uiImage = GetComponent<Image>();
        if (uiImage == null)
        {
            Debug.LogError("UIImageが設定されていません。");
            return;
        }

        // UIImageのスプライトからテクスチャを取得
        Texture2D texture = uiImage.sprite.texture;

        // 透過処理を行う
        Texture2D newTexture = MakeColorTransparent(texture, targetColor, tolerance);

        // 新しいテクスチャを使用してスプライトを作成
        uiImage.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
        oldsprite = uiImage.sprite;
        
    }
    private void Update()
    {
        if(oldsprite != uiImage.sprite)
        {
            if (uiImage == null)
            {
                Debug.LogError("UIImageが設定されていません。");
                return;
            }

            // UIImageのスプライトからテクスチャを取得
            Texture2D texture = uiImage.sprite.texture;

            // 透過処理を行う
            Texture2D newTexture = MakeColorTransparent(texture, targetColor, tolerance);

            // 新しいテクスチャを使用してスプライトを作成
            uiImage.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
            oldsprite = uiImage.sprite;
            Resources.UnloadUnusedAssets();
        }
    }
    Texture2D MakeColorTransparent(Texture2D texture, Color color, float tolerance)
    {
        Texture2D newTexture = new Texture2D(texture.width, texture.height);
        newTexture.filterMode = texture.filterMode;
        newTexture.wrapMode = texture.wrapMode;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color pixelColor = texture.GetPixel(x, y);
                if (IsColorMatch(pixelColor, color, tolerance))
                {
                    newTexture.SetPixel(x, y, new Color(pixelColor.r, pixelColor.g, pixelColor.b, 0)); // アルファ値を0に設定
                }
                else
                {
                    newTexture.SetPixel(x, y, pixelColor);
                }
            }
        }

        newTexture.Apply();
        return newTexture;
    }

    bool IsColorMatch(Color color1, Color color2, float tolerance)
    {
        return Mathf.Abs(color1.r - color2.r) < tolerance &&
               Mathf.Abs(color1.g - color2.g) < tolerance &&
               Mathf.Abs(color1.b - color2.b) < tolerance;
    }
}
