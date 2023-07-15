using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System.IO;
using System;
using UnityEngine.UI;
public class ChildMail : MonoBehaviour
{
    public bool bonus_trg = false;
    public Text objtitletext;
    public Image ischeck_icon;
    public bool movetrg = false;
    private bool oldmove = false;
    private NCMBObject child_obj;
    public bool parenttrg = true;
    private AudioSource audioS;
    public AudioClip onse;
    [Header("以下メッセージ内容について")]
    public GameObject checkUI;
    public Text messagetitletext;
    public Text messagedoctext;
    public Text bonusgettext;
    public transmailopen transbtn;
    public string tmpchildobj;
    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!parenttrg && movetrg && movetrg != oldmove && tmpchildobj != "")
        {
            oldmove = movetrg;
            child_obj = new NCMBObject("GameNews");
            child_obj.ObjectId = tmpchildobj;
            child_obj.FetchAsync();
            Invoke(nameof(CoolMail), 0.31f);
        }
    }
    public void CoolMail()
    {
        oldmove = movetrg;
        objtitletext.text = child_obj["messagetitle"].ToString();
        ischeck_icon.enabled = true;
        if (PlayerPrefs.GetString(child_obj.ObjectId.ToString(), "false") == "false")
        {
            ;
        }
        else
        {
            ischeck_icon.enabled = false;
        }
        if ("YpY9012nWJzXaBuS" == tmpchildobj && PlayerPrefs.GetString(tmpchildobj, "false") == "true") Destroy(gameObject);
    }
    public void MailOpen()
    {
        if(bonus_trg)
        {
            bonusgettext.text = "ボーナスを受け取る";
        }
        else if(!bonus_trg)
        {
            bonusgettext.text = "ボーナス：無し";
            PlayerPrefs.SetString(child_obj.ObjectId.ToString(), "true");
            PlayerPrefs.Save();
        }
        audioS.PlayOneShot(onse);
        //処理
        checkUI.SetActive(true);
        messagetitletext.text = child_obj["messagetitle"].ToString();
        messagedoctext.text = child_obj["messagedoc"].ToString();
        transbtn.targetchild = this.GetComponent<ChildMail>();
    }
    public void MailBonus()
    {
        if (bonus_trg)
        {
            bonus_trg = false;
            GManager.instance.setrg = 12;
            long tmpitemnum = (long)child_obj["additemid"];
            long tmpcoinnum = (long)child_obj["addnormalcoin"];
            if ((int)tmpitemnum != -1)
            {
                GManager.instance.ItemID[(int)tmpitemnum].gettrg = 1;
                GManager.instance.ItemID[(int)tmpitemnum].itemnumber += 1;
                PlayerPrefs.SetInt("itemnumber" + (int)tmpitemnum, GManager.instance.ItemID[(int)tmpitemnum].itemnumber);
                PlayerPrefs.SetInt("itemget" + (int)tmpitemnum, GManager.instance.ItemID[(int)tmpitemnum].gettrg);
            }
            GManager.instance.Coin += (int)tmpcoinnum;
            PlayerPrefs.SetInt("coin", GManager.instance.Coin);
            PlayerPrefs.SetString(child_obj.ObjectId.ToString(), "true");
            PlayerPrefs.Save();
            bonusgettext.text = "ボーナス：無し";
        }
        else
        {
            GManager.instance.setrg = 27;
        }
    }
}
