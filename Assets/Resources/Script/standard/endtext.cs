using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class endtext : MonoBehaviour
{
    public bool scenetrg = false;
    [Multiline]
    public string[] EndText;
    [Multiline]
    public string[] EndText2;
    public float[] maxviewTime;
    private float viewTime;
    private float fadetime;
    private Text scoreText = null;
    private int oldScore = 9999;
    private int textnumber = 0;
    private int eventtrg = 0;
    public float Rcolor;
    public float Gcolor;
    public float Bcolor;
    public GameObject fadein;
    bool endtrg = false;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (EndText.Length != textnumber)
        {
            if (oldScore != textnumber)
            {
                eventtrg = 1;
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = EndText[textnumber];
                }
                else if (EndText2 != null && EndText2.Length != 0 && GManager.instance.isEnglish == 1)
                {
                    scoreText.text = EndText2[textnumber];
                }
                else
                {
                    scoreText.text = EndText[textnumber];
                }
                oldScore = textnumber;
            }
            if (eventtrg == 1)
            {
                fadetime += Time.deltaTime;
                if (fadetime < 1.0f)
                {
                    scoreText.color = new Color(Rcolor, Gcolor, Bcolor, fadetime);
                    scoreText = this.GetComponent<Text>();
                }
                else if (fadetime > 1.1f)
                {
                    scoreText.color = new Color(Rcolor, Gcolor, Bcolor, 1.0f);
                    scoreText = this.GetComponent<Text>();
                    eventtrg = 0;
                }
            }
            else if (eventtrg == 0)
            {
                viewTime += Time.deltaTime;
                if (viewTime > maxviewTime[textnumber])
                {
                    viewTime = 0;
                    eventtrg = 2;
                }
            }
            if (eventtrg == 2)
            {
                fadetime -= Time.deltaTime;
                if (fadetime > 0.0f)
                {
                    scoreText.color = new Color(Rcolor, Gcolor, Bcolor, fadetime);
                    scoreText = this.GetComponent<Text>();
                }
                else if (fadetime < -0.1f)
                {
                    scoreText.color = new Color(Rcolor, Gcolor, Bcolor, 0.0f);
                    scoreText = this.GetComponent<Text>();
                    textnumber += 1;
                }
            }
        }
        else if (endtrg == false)
        {
            endtrg = true;
            if (scenetrg == true)
            {
                GManager.instance.EventNumber[11] = 1;
                GManager.instance.stageNumber = 2;
                GManager.instance.posX = 0;
                GManager.instance.posY = 0.5f;
                GManager.instance.posX = 0;
                SaveDate();
                Instantiate(fadein, transform.position, transform.rotation);
                Invoke("sceneMove", 3);
            }
        }
    }
    void sceneMove()
    {
        SceneManager.LoadScene("title");
    }
    void SaveDate()
    {
       //後でやる
    }
}