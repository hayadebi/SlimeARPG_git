using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tips_mainUI : MonoBehaviour
{
    public Image tips_image;
    public Text tips_title;
    public Text tips_script;
    public bool handset = false;
    public int handtipsid = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (GManager.instance.Triggers[108] != -1&&!handset)
        {
            tips_image.sprite = GManager.instance._Tips[GManager.instance.Triggers[108]].tips_image;
            if (GManager.instance.isEnglish == 0)
            {
                tips_title.text = GManager.instance._Tips[GManager.instance.Triggers[108]].tips_name[0];
                tips_script.text = GManager.instance._Tips[GManager.instance.Triggers[108]].tips_script[0];
            }
            else if (GManager.instance.isEnglish == 1)
            {
                tips_title.text = GManager.instance._Tips[GManager.instance.Triggers[108]].tips_name[1];
                tips_script.text = GManager.instance._Tips[GManager.instance.Triggers[108]].tips_script[1];
            }
            GManager.instance.Triggers[108] = -1;
        }
        else if (handset)
        {
            tips_image.sprite = GManager.instance._Tips[handtipsid].tips_image;
            if (GManager.instance.isEnglish == 0)
            {
                tips_title.text = GManager.instance._Tips[handtipsid].tips_name[0];
                tips_script.text = GManager.instance._Tips[handtipsid].tips_script[0];
            }
            else if (GManager.instance.isEnglish == 1)
            {
                tips_title.text = GManager.instance._Tips[handtipsid].tips_name[1];
                tips_script.text = GManager.instance._Tips[handtipsid].tips_script[1];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
