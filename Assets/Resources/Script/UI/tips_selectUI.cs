using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class tips_selectUI : MonoBehaviour
{
    public GameObject tipsUI;
    public AudioSource audioS;
    public AudioClip selectse;
    public AudioClip onse;
    public AudioClip notse;
    public Text[] tname;
    public Image[] timage;
    public int[] tipsID;
    public int boxnumber = 0;
    public int addnumber = 0;
    public int selectnumber = 0;
    public tips_selectUI tcode = null;
    public int child_buttonID = -1;
    private bool set_trg = false;
    // Start is called before the first frame update
    void Start()
    {
        if (tcode == null)
        {
            for (int i = 0; GManager.instance._Tips.Length > i;)
            {
                if (GManager.instance.Triggers[GManager.instance._Tips[i].tips_trgID] > 0)
                {
                    boxnumber += 1;
                }
                i += 1;
            }
            tipsID = new int[boxnumber];
            if (boxnumber != 0)
            {
                for (int i = 0; GManager.instance._Tips.Length > i;)
                {
                    if (GManager.instance.Triggers[GManager.instance._Tips[i].tips_trgID] > 0)
                    {
                        tipsID[addnumber] = i;
                        addnumber += 1;
                    }
                    i += 1;
                }
            }
            SetUI();
        }
    }
    void SetUI()
    {
        for (int i = 0; i < tname.Length;)
        {
            if (tipsID == null || GManager.instance._Tips.Length < i + selectnumber)
            {
                tname[i].text = "「????」";
                timage[i].enabled = false;
            }
            else if (tipsID != null && tipsID.Length > (i + selectnumber) )
            {
                for (int l = 0; l < tipsID.Length;)
                {
                    set_trg = false;
                    if (GManager.instance.Triggers[GManager.instance._Tips[tipsID[i + selectnumber]].tips_trgID] > 0)
                    {
                        set_trg = true;
                        if (GManager.instance.isEnglish == 0)
                        {
                            tname[i].text = GManager.instance._Tips[tipsID[i + selectnumber]].tips_name[0];
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            tname[i].text = GManager.instance._Tips[tipsID[i + selectnumber]].tips_name[1];
                        }
                        if (GManager.instance.Triggers[GManager.instance._Tips[tipsID[i + selectnumber]].tips_trgID] < 2)
                        {
                            timage[i].enabled = true;
                        }
                        else
                        {
                            timage[i].enabled = false;
                        }
                    }
                    l += 1;
                }
            }
            else
            {
                tname[i].text = "「????」";
                timage[i].enabled = false;
            }
            i++;
        }
    }
    public void SelectMinus()
    {
        if (selectnumber >= 5)
        {
            audioS.PlayOneShot(selectse);
            selectnumber -= 5;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SelectPlus()
    {
        if (selectnumber + 5 < (tipsID.Length))
        {
            audioS.PlayOneShot(selectse);
            selectnumber += 5;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void TipsOpen()
    {
        if(tcode != null)
        {
            if (tcode.tipsID == null || tcode.tipsID.Length < child_buttonID + tcode.selectnumber)
            {
                audioS.PlayOneShot(notse);
            }
            else if (tcode.tipsID != null && tcode.tipsID.Length > (child_buttonID + tcode.selectnumber) && GManager.instance.Triggers[GManager.instance._Tips[tcode.tipsID[child_buttonID + tcode.selectnumber]].tips_trgID] > 0)
            {
                audioS.PlayOneShot(onse);
                GManager.instance.walktrg = false;
                GManager.instance.setmenu = 2;
                Instantiate(tipsUI, transform.position, transform.rotation);
                if (GManager.instance.Triggers[GManager.instance._Tips[tcode.tipsID[child_buttonID + tcode.selectnumber]].tips_trgID] < 2)
                {
                    GManager.instance.Triggers[GManager.instance._Tips[tcode.tipsID[child_buttonID + tcode.selectnumber]].tips_trgID] = 2;
                }
                if (GManager.instance.Triggers[GManager.instance._Tips[tcode.tipsID[child_buttonID + tcode.selectnumber]].tips_trgID] < 2)
                {
                    GManager.instance.Triggers[GManager.instance._Tips[tcode.tipsID[child_buttonID + tcode.selectnumber]].tips_trgID] = 2;
                }
                GManager.instance.Triggers[105] = GManager.instance.Triggers[106];
                GManager.instance.Triggers[108] = tcode.tipsID[child_buttonID + tcode.selectnumber];
            }
            else
            {
                audioS.PlayOneShot(notse);
            }
        }
    }

}
