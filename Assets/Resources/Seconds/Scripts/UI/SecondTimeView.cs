using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SecondTimeView : MonoBehaviour
{
    public Text[] texts;
    private float oldtime;
    private int oldday;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oldday != DataManager.instance.daycount || oldtime != DataManager.instance.sunTime)
        {
            oldday = DataManager.instance.daycount;
            oldtime = DataManager.instance.sunTime;
            int justtime = (int)(DataManager.instance.sunTime / 24);
            foreach (Text _text in texts)
            {
                _text.text = "時刻：" + justtime.ToString() + "時\n経過日数："+ DataManager.instance.daycount .ToString()+ "日目";
            }
        }
    }
}
