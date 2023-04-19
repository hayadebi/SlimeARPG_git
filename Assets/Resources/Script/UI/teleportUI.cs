using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class teleportUI : MonoBehaviour
{
    public GameObject fade;
    public AudioSource audioS;
    public AudioClip[] se;
    public Animator ui;
    public string animname;
    private float stime = 0;
    private bool curetrg = true;
    private GameObject P;
    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.Find("Player");
    }
    public void cancelBed()
    {
        audioS.PlayOneShot(se[2]);
        GManager.instance.ESCtrg = false;
        GManager.instance.setmenu = 0;
        GManager.instance.walktrg = true;
        Destroy(gameObject, 1f);
        if (ui != null)
        {
            ui.Play(animname);
        }
    }

    public void Plan1()
    {
        if (GManager.instance.Coin >= 20)
        {
            GManager.instance.Coin -= 20;
            audioS.PlayOneShot(se[0]);
            Instantiate(fade, transform.position, transform.rotation);
            
            Invoke("sunT", 1);
        }
        else
        {
            audioS.PlayOneShot(se[2]);
        }
    }
    public void Plan2()
    {
        if (GManager.instance.Coin >= 0)
        {
            GManager.instance.Coin -= 10;
            audioS.PlayOneShot(se[1]);
            GManager.instance.EventNumber[14] = GManager.instance.stageNumber;
            GManager.instance.freenums[0] = P.transform.position.x;
            GManager.instance.freenums[1] = P.transform.position.y;
            GManager.instance.freenums[2] = P.transform.position.z;
            saveN();
            GManager.instance.ESCtrg = false;
            GManager.instance.setmenu = 0;
            GManager.instance.walktrg = true;
            Destroy(gameObject, 1f);
            if (ui != null)
            {
                ui.Play(animname);
            }
        }
        else
        {
            audioS.PlayOneShot(se[2]);
        }
    }
    void sunT()
    {
        GManager.instance.stageNumber = GManager.instance.EventNumber[14];
        GManager.instance.posX = GManager.instance.freenums[0];
        GManager.instance.posY = GManager.instance.freenums[1];
        GManager.instance.posZ = GManager.instance.freenums[2];
        saveN();
        GManager.instance.ESCtrg = false;
        GManager.instance.setmenu = 0;
        GManager.instance.walktrg = true;
        SceneManager.LoadScene("stage" + GManager.instance.EventNumber[14]);
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
