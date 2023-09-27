using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySystem : MonoBehaviour
{

    //Skyboxを入れる変数、[]があると複数入れられるように
    //public Material[] skybox;
    public float upSpeed = 1;
    public Light _light;
    private void Start()
    {
        _light = this.GetComponent<Light>();
        _light.shadowStrength = 0.5f;
    }
    void Update()
    {
        //1日のスピードを調節する
        GManager.instance.sunTime += (Time.deltaTime / 16) * upSpeed;

        if (GManager.instance.setmenu == 0 && GManager.instance.houseTrg == 0)
        {
            //X軸を回転させる
            transform.localEulerAngles = new Vector3((GManager.instance.sunTime), 0, 0);
            //0度以上72度未満の時に一つ目のSkyboxを出す
           // if (GManager.instance.sunTime >= 0 && 40 > GManager.instance.sunTime)
                //RenderSettings.skybox = skybox[0];

            //elseがないと切り替わらない
            //else if (GManager.instance.sunTime >= 40 && 80 > GManager.instance.sunTime)
                //RenderSettings.skybox = skybox[1];

            //else if (GManager.instance.sunTime >= 80 && 120 > GManager.instance.sunTime)
               // RenderSettings.skybox = skybox[2];

            //else if (GManager.instance.sunTime >= 120 && 160 > GManager.instance.sunTime)
               // RenderSettings.skybox = skybox[3];

           // else if (GManager.instance.sunTime >= 160 && 180 > GManager.instance.sunTime)
                //RenderSettings.skybox = skybox[4];

            //180度以上になったら0に戻す
            if (GManager.instance.sunTime > 180)
            {
                GManager.instance.sunTime = 0;
                GManager.instance.daycount += 1;
                for (int i = 0; i < GManager.instance.mobDsTrg.Length;)
                {
                    GManager.instance.mobDsTrg[i] = 0;
                    i++;
                }
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
                PlayerPrefs.SetFloat("posx", GManager.instance.posX);
                PlayerPrefs.SetFloat("posy", GManager.instance.posY);
                PlayerPrefs.SetFloat("posz", GManager.instance.posZ);
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
                PlayerPrefs.SetInt("minigame_indexTrg", GManager.instance._minigame.input_indexTrg);
                for (int i = 0; i < GManager.instance._minigame.input_missionID.Length;)
                {
                    PlayerPrefs.SetInt("minigame_missionID" + i, GManager.instance._minigame.input_missionID[i]);
                    i++;
                }
                GManager.instance._minigame.input_missionID[3] = 7;
                PlayerPrefs.SetString("itemscript46", GManager.instance.ItemID[46].itemscript);

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
    }
}