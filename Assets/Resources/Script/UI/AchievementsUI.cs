using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchievementsUI : MonoBehaviour
{
    public bool titletrg = false;
    public AudioSource audioS;
    public AudioClip selectse;
    public AudioClip notse;
    public Text[] achiname;
    public Text[] achiscript;
    public int[] achiID;
    int boxnumber = 0;
    int addnumber = 0;
    public int selectnumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(titletrg == true)
        {
            for (int i = 0; i < GManager.instance.achievementsID.Length;)
            {
                GManager.instance.achievementsID[i].gettrg = PlayerPrefs.GetInt("achiget" + i, 0);
                i++;
            }
        }
        for (int i = 0; GManager.instance.achievementsID.Length > i;)
        {
            if(GManager.instance.achievementsID[i].gettrg > 0 )
            {
                boxnumber += 1;
            }
            i += 1;
        }
        achiID = new int[boxnumber];
        if (boxnumber != 0)
        {
            for (int i = 0; GManager.instance.achievementsID.Length > i;)
            {
                if (GManager.instance.achievementsID[i].gettrg > 0)
                {
                        achiID[addnumber] = i;
                        addnumber += 1;
                }
                i += 1;
            }
        }
        SetUI();
    }
    void SetUI()
    {
        for(int i = 0;i<achiname.Length;)
        {
            if(achiID == null || GManager.instance.achievementsID.Length < i + selectnumber)
            {
                achiname[i].text = "??????";
                achiscript[i].text = "????????";
            }
            else if (achiID != null && achiID.Length > (i + selectnumber) && GManager.instance.achievementsID[achiID[i+selectnumber]].gettrg > 0)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    achiname[i].text = GManager.instance.achievementsID[achiID[i + selectnumber]].name;
                    achiscript[i].text = GManager.instance.achievementsID[achiID[i + selectnumber]].script;
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    achiname[i].text = GManager.instance.achievementsID[achiID[i + selectnumber]].name2;
                    achiscript[i].text = GManager.instance.achievementsID[achiID[i + selectnumber]].script2;
                }
            }
            else
            {
                achiname[i].text = "??????";
                achiscript[i].text = "????????";
            }
            i++;
        }
    }
    public void SelectMinus()
    {
        if (selectnumber >= 3)
        {
            audioS.PlayOneShot(selectse);
            selectnumber -= 3;
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
        if (selectnumber+3 < (GManager.instance.achievementsID.Length))
        {
            audioS.PlayOneShot(selectse);
            selectnumber += 3;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }

}
