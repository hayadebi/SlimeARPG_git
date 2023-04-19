using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class secondCommand : MonoBehaviour
{
    public Text mainQ;
    public GameObject fadein;
    public GameObject fadeout;
    public AudioSource audioS;
    public AudioClip[] se;
    public Animator ui;
    public string[] text;
    public float[] time;
    public bool[] trg;
    [Header("0はプレイヤー")]
    public GameObject[] obj;
    // Start is called before the first frame update
    void Start()
    {
        obj[0] = GameObject.Find("Player");
        if(GManager.instance.EventNumber[16] == 1)
        {
            if(GManager.instance.isEnglish == 0)
            {
                mainQ.text = "この先へ進みますか？";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                mainQ.text = "Proceed to the next area?";
            }
        }
    }
    public void cancelCommand()
    {
        audioS.PlayOneShot(se[1]);
        GManager.instance.ESCtrg = false;
        GManager.instance.setmenu = 0;
        GManager.instance.walktrg = true;
        Destroy(gameObject, 1f);
        if (ui != null)
        {
            ui.Play("normaluiend");
        }
    }

    public void playCommand()
    {
        if (GManager.instance.EventNumber[16] == 1)
        {
            GManager.instance.EventNumber[16] = 0;
            audioS.PlayOneShot(se[0]);
            Instantiate(fadein, transform.position, transform.rotation);

            GManager.instance.stageNumber = 4;
            GManager.instance.EventNumber[7] = 7;
            GManager.instance.posX = 30;
            GManager.instance.posY = 20;
            GManager.instance.posZ = 1630;
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.villageName = "極寒への入口";
            }
            else
            {
                GManager.instance.villageName = "Entrance to the extreme cold";
            }
            saveN();
            Invoke("cPlay", 1);
        }
        else
        {
            audioS.PlayOneShot(se[1]);
            cancelCommand();
        }
    }
    void cPlay()
    {
        GManager.instance.ESCtrg = false;
        GManager.instance.setmenu = 0;
        GManager.instance.walktrg = true;
        SceneManager.LoadScene("stage" + GManager.instance.stageNumber);
    }

    public void saveN()
    {
        PlayerPrefs.SetInt("housetrg", GManager.instance.houseTrg);
        PlayerPrefs.SetInt("dayc", GManager.instance.daycount);
        PlayerPrefs.SetInt("coin", GManager.instance.Coin);
        for (int i = 0; i < GManager.instance.EventNumber.Length;)
        {
            PlayerPrefs.SetInt("EvN" + i, GManager.instance.EventNumber[i]);
            i++;
        }
        for (int i = 0; i < GManager.instance.freenums.Length;)
        {
            PlayerPrefs.SetFloat("freenums" + i, GManager.instance.freenums[i]);
            i++;
        }
        PlayerPrefs.SetFloat("posX", GManager.instance.posX);
        PlayerPrefs.SetFloat("posY", GManager.instance.posY);
        PlayerPrefs.SetFloat("posZ", GManager.instance.posZ);
        PlayerPrefs.SetInt("stageN", GManager.instance.stageNumber);
        for (int i = 0; i < GManager.instance.ItemID.Length;)
        {
            PlayerPrefs.SetInt("itemnumber" + i, GManager.instance.ItemID[i].itemnumber);
            PlayerPrefs.SetInt("itemget" + i, GManager.instance.ItemID[i].gettrg);
            PlayerPrefs.SetInt("item_quickset" + i, GManager.instance.ItemID[i]._quickset);
            PlayerPrefs.SetInt("item_equalsset" + i, GManager.instance.ItemID[i]._equalsset);
            PlayerPrefs.SetInt("pl_equalsselect" + i, GManager.instance.ItemID[i].pl_equalsselect);
            i++;
        }
        //---------------

        for (int i = 0; i < GManager.instance.Quick_itemSet.Length;)
        {
            PlayerPrefs.SetInt("quick_itemset" + i, GManager.instance.Quick_itemSet[i]);
            i++;
        }
        for (int i = 0; i < GManager.instance.P_equalsID.Length;)
        {
            PlayerPrefs.SetInt("hand_equals" + i, GManager.instance.P_equalsID[i].hand_equals);
            PlayerPrefs.SetInt("accessory_equals" + i, GManager.instance.P_equalsID[i].accessory_equals);
            i++;
        }
        //---------------
        for (int i = 0; i < GManager.instance.Pstatus.Length;)
        {
            PlayerPrefs.SetInt("addhp" + i, GManager.instance.Pstatus[i].add_hp);
            PlayerPrefs.SetInt("addmp" + i, GManager.instance.Pstatus[i].add_mp);
            PlayerPrefs.SetInt("addat" + i, GManager.instance.Pstatus[i].add_at);
            PlayerPrefs.SetInt("adddf" + i, GManager.instance.Pstatus[i].add_df);

            PlayerPrefs.SetInt("pmaxhp" + i, GManager.instance.Pstatus[i].maxHP);
            PlayerPrefs.SetInt("php" + i, GManager.instance.Pstatus[i].hp);
            PlayerPrefs.SetInt("pmaxmp" + i, GManager.instance.Pstatus[i].maxMP);
            PlayerPrefs.SetInt("pmp" + i, GManager.instance.Pstatus[i].mp);
            PlayerPrefs.SetInt("pdf" + i, GManager.instance.Pstatus[i].defense);
            PlayerPrefs.SetInt("pat" + i, GManager.instance.Pstatus[i].attack);
            PlayerPrefs.SetInt("plv" + i, GManager.instance.Pstatus[i].Lv);
            PlayerPrefs.SetInt("pmaxexp" + i, GManager.instance.Pstatus[i].maxExp);
            PlayerPrefs.SetInt("pinputexp" + i, GManager.instance.Pstatus[i].inputExp);
            PlayerPrefs.SetInt("pselectskill" + i, GManager.instance.Pstatus[i].selectskill);
            PlayerPrefs.SetInt("pselectmagic" + i, GManager.instance.Pstatus[i].magicselect);
            for (int j = 0; j < GManager.instance.Pstatus[i].inputskill.Length;)
            {
                PlayerPrefs.SetInt("pinputskill" + i + "" + j, GManager.instance.Pstatus[i].inputskill[j]);
                j++;
            }
            for (int j = 0; j < GManager.instance.Pstatus[i].getMagic.Length;)
            {
                PlayerPrefs.SetInt("pgetmagictrg" + i + "" + j, GManager.instance.Pstatus[i].getMagic[j].gettrg);
                j++;
            }
            for (int j = 0; j < GManager.instance.Pstatus[i].magicSet.Length;)
            {
                PlayerPrefs.SetInt("pmagicset" + i + "" + j, GManager.instance.Pstatus[i].magicSet[j]);
                j++;
            }
            PlayerPrefs.SetInt("getpl" + i, GManager.instance.Pstatus[i].getpl);
            i++;
        }
        PlayerPrefs.SetInt("plselect", GManager.instance.playerselect);
        for (int i = 0; i < GManager.instance.Triggers.Length;)
        {
            PlayerPrefs.SetInt("gmtrg" + i, GManager.instance.Triggers[i]);
            i++;
        }
        for (int i = 0; i < GManager.instance.missionID.Length;)
        {
            PlayerPrefs.SetInt("inputmission" + i, GManager.instance.missionID[i].inputmission);
            i++;
        }
        for (int i = 0; i < GManager.instance.achievementsID.Length;)
        {
            PlayerPrefs.SetInt("achiget" + i, GManager.instance.achievementsID[i].gettrg);
            i++;
        }
        for (int i = 0; i < GManager.instance.enemynoteID.Length;)
        {
            PlayerPrefs.SetInt("enemynoteget" + i, GManager.instance.enemynoteID[i].gettrg);
            i++;
        }
        PlayerPrefs.SetFloat("audioMax", GManager.instance.audioMax);
        PlayerPrefs.SetFloat("seMax", GManager.instance.seMax);
        PlayerPrefs.SetInt("Mode", GManager.instance.mode);
        PlayerPrefs.SetInt("isEn", GManager.instance.isEnglish);
        PlayerPrefs.SetInt("Reduction", GManager.instance.reduction);
        PlayerPrefs.SetFloat("suntime", GManager.instance.sunTime);
        PlayerPrefs.SetInt("viewUp", GManager.instance.autoviewup);
        PlayerPrefs.SetInt("longDash", GManager.instance.autolongdash);
        PlayerPrefs.SetInt("autoattack", GManager.instance.autoattack);
        PlayerPrefs.SetFloat("rotpivot", GManager.instance.rotpivot);
        for (int i = 0; i < GManager.instance.mobDsTrg.Length;)
        {
            PlayerPrefs.SetInt("mdt" + i, GManager.instance.mobDsTrg[i]);
            i++;
        }
        PlayerPrefs.Save();
    }
}
