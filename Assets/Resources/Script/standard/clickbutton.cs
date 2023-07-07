using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class clickbutton : MonoBehaviour
{
    public string settingtype = "";
    public float addsettingfloat;
    public int addsettingint;
    public bool menutrg = false;
    public bool addstage = false;
    public bool stagetrg = false;
    public bool resettrg = false;
    public float maxUI = 0;
    public string nextscene;
    public GameObject settingUI;
    public GameObject fadeinUI;
    public AudioClip clickse;
    AudioSource audioSource;
    public bool subTrg = false;
    private int oldrandom = 0;
    public Animator anim_;
    public string a_name;
    public int a_setnumber;
    [Header("課金用")]
    public bool mpurseuser_trg = false;
    public clickbutton usercheck=null;
    public int set_buyid = -1;
    public int limit_onviewmax = -1;
    [Header("0=アイテム,1=クラフトレシピ")]
    public int set_buytype = -1;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void animClick()
    {
        if(anim_)
        {
            GManager.instance.setrg = 6;
            anim_.SetInteger(a_name, a_setnumber);
        }
    }
    public void settingClick()
    {
        if (!mpurseuser_trg || (mpurseuser_trg && GManager.instance.mpurseuser_on))
        {
            GameObject tmpobj=null;
            if (GManager.instance.setmenu < maxUI && GManager.instance.walktrg == true)
            {
                GManager.instance.setmenu += 1;
                GManager.instance.walktrg = false;
                audioSource.PlayOneShot(clickse);
                if (menutrg == false)
                {
                    tmpobj=Instantiate(settingUI, transform.position, transform.rotation);
                }
                else if (menutrg == true)
                {
                    tmpobj = Instantiate(GManager.instance.spawnUI, transform.position, transform.rotation);
                }
            }
            else if (GManager.instance.setmenu < maxUI && GManager.instance.setmenu > 0)
            {
                GManager.instance.setmenu += 1;
                GManager.instance.walktrg = false;
                audioSource.PlayOneShot(clickse);
                if (menutrg == false)
                {
                    tmpobj = Instantiate(settingUI, transform.position, transform.rotation);
                }
                else if (menutrg == true)
                {
                    tmpobj = Instantiate(GManager.instance.spawnUI, transform.position, transform.rotation);
                }
            }
            else if (GManager.instance.setmenu < maxUI && GManager.instance.walktrg == false)
            {
                GManager.instance.setmenu += 1;
                GManager.instance.walktrg = false;
                audioSource.PlayOneShot(clickse);
                if (menutrg == false)
                {
                    tmpobj = Instantiate(settingUI, transform.position, transform.rotation);
                }
                else if (menutrg == true)
                {
                    tmpobj = Instantiate(GManager.instance.spawnUI, transform.position, transform.rotation);
                }
            }
            if (set_buyid >= 0) GManager.instance.select_buyid = set_buyid;
            if (set_buytype >= 0 && tmpobj) tmpobj.GetComponent<DataBuySystem>().get_buytype = set_buytype;
        }
        else
        {
            if (usercheck != null) usercheck.gameObject.SetActive(true);
            GManager.instance.setrg = 27;
        }
    }
    public void NoSetThis()
    {
        GManager.instance.ESCtrg = true;
    }
    public void CheckOnView()
    {
        if (!mpurseuser_trg || (mpurseuser_trg && GManager.instance.mpurseuser_on))
        {
            if (limit_onviewmax == -1 || (limit_onviewmax > 0 && PlayerPrefs.GetInt("DayAds", 0) < 5))
            {
                var tmp = PlayerPrefs.GetInt("DayAds", 0) + 1;
                PlayerPrefs.SetInt("DayAds", tmp);
                PlayerPrefs.Save();
                settingUI.SetActive(true);
            }
            else
            {
                GManager.instance.setrg = 27;
                if (this.GetComponent<Image>())
                {
                    Image tmpimg = this.GetComponent<Image>();
                    Color tmpcl = tmpimg.color;
                    tmpcl.a /= 2;
                    tmpimg.color = tmpcl;
                }

            }
        }
        else
        {
            if (usercheck != null) usercheck.gameObject.SetActive(true);
            GManager.instance.setrg = 27;
        }
    }
    public void ChildOnView()
    {
        if (!mpurseuser_trg || (mpurseuser_trg && GManager.instance.mpurseuser_on))
        {
            if (limit_onviewmax == -1 || (limit_onviewmax > 0 && PlayerPrefs.GetInt("DayAds", 0) < 5))
            {
                var tmp = PlayerPrefs.GetInt("DayAds", 0) + 1;
                PlayerPrefs.SetInt("DayAds", tmp);
                PlayerPrefs.Save();
                Instantiate(settingUI, fadeinUI.transform.position, transform.rotation, fadeinUI.transform);
            }
            else
            {
                GManager.instance.setrg = 27;
                if (this.GetComponent<Image>())
                {
                    Image tmpimg = this.GetComponent<Image>();
                    Color tmpcl = tmpimg.color;
                    tmpcl.a /= 2;
                    tmpimg.color = tmpcl;
                }

            }
        }
        else
        {
            if (usercheck != null) usercheck.gameObject.SetActive(true);
            GManager.instance.setrg = 27;
        }
    }
    public void CheckNoView()
    {
        this.gameObject.SetActive(false);
    }
    public void startClick()
    {
        if (GManager.instance.bossbattletrg == 0)
        {
            audioSource.PlayOneShot(clickse);
            Instantiate(fadeinUI, transform.position, transform.rotation);
            Resources.UnloadUnusedAssets();
            if(subTrg == true)
            {
                GManager.instance.subgameTrg = true;
            }
            else
            {
                GManager.instance.subgameTrg = false;
            }
            Invoke("GameStart", 3.0f);
        }
    }

    public void quitClick()
    {
        Application.Quit();
    }
    public void DestroyClick()
    {
        GManager.instance.walktrg = true;
        Destroy(gameObject);
    }
    
    void GameStart()
    {
        if (menutrg == false && GManager.instance.bossbattletrg == 0)
        {
            if (resettrg == true)
            {
                PlayerPrefs.DeleteAll();
                _MiniGame(3);

                PlayerPrefs.SetInt("minigame_indexTrg", GManager.instance._minigame.input_indexTrg);
                for (int i = 0; i < GManager.instance._minigame.input_missionID.Length;)
                {
                    PlayerPrefs.SetInt("minigame_missionID" + i, GManager.instance._minigame.input_missionID[i]);
                    i++;
                }
                GManager.instance._minigame.input_missionID[3] = 7;
                PlayerPrefs.SetString("itemscript46", GManager.instance.ItemID[46].itemscript);
                PlayerPrefs.Save();
                resetN();
            }
            else if (resettrg == false)
            {
                loadN();
            }
        }
        if(menutrg == true)
        {
            SceneManager.LoadScene(GManager.instance.SceneText);
        }
        else if (stagetrg == false)
        {
            SceneManager.LoadScene(nextscene);
        }
        else if (stagetrg == true)
        {
            SceneManager.LoadScene(nextscene + GManager.instance.stageNumber);
        }
    }
    public void Slider()
    {
        if(settingtype == "audio")
        {
            GManager.instance.audioMax += addsettingfloat;
            if(GManager.instance.audioMax > 1)
            {
                GManager.instance.audioMax = 1;
            }
            else if (GManager.instance.audioMax < 0)
            {
                GManager.instance.audioMax = 0;
            }
        }
        if (settingtype == "se")
        {
            GManager.instance.seMax += addsettingfloat;
            if (GManager.instance.seMax > 1)
            {
                GManager.instance.seMax = 1;
            }
            else if (GManager.instance.seMax < 0)
            {
                GManager.instance.seMax = 0;
            }
        }
        else if (settingtype == "mode")
        {
            GManager.instance.mode += addsettingint;
            if (GManager.instance.mode> 2)
            {
                GManager.instance.mode = 2;
            }
            else if (GManager.instance.mode < 0)
            {
                GManager.instance.mode = 0;
            }
        }
        else if (settingtype == "kando")
        {
            GManager.instance.kando += addsettingint;
            if (GManager.instance.kando > 5)
            {
                GManager.instance.kando = 5;
            }
            else if (GManager.instance.kando < 1)
            {
                GManager.instance.kando = 1;
            }
        }
        else if(settingtype == "reduction")
        {
            if(GManager.instance.reduction == 0)
            {
                GManager.instance.reduction = 1;
            }
            else if(GManager.instance.reduction == 1)
            {
                GManager.instance.reduction = 0;
            }
        }
        else if (settingtype == "オートビュー")
        {
            if (GManager.instance.autoviewup == 0)
            {
                GManager.instance.autoviewup = 1;
            }
            else if (GManager.instance.autoviewup == 1)
            {
                GManager.instance.autoviewup = 0;
            }
        }
        else if (settingtype == "オートダッシュ")
        {
            if (GManager.instance.autolongdash == 0)
            {
                GManager.instance.autolongdash = 1;
            }
            else if (GManager.instance.autolongdash  == 1)
            {
                GManager.instance.autolongdash  = 0;
            }
        }
        else if (settingtype == "自動攻撃")
        {
            if (GManager.instance.autoattack == 0)
            {
                GManager.instance.autoattack = 1;
            }
            else if (GManager.instance.autoattack == 1)
            {
                GManager.instance.autoattack = 0;
            }
        }
        else if (settingtype == "回転速度")
        {
            GManager.instance.rotpivot += addsettingfloat;
            if (GManager.instance.rotpivot > 4)
            {
                GManager.instance.rotpivot = 4;
            }
            else if (GManager.instance.rotpivot < 1)
            {
                GManager.instance.rotpivot = 1;
            }
        }
    }

    public void JapaneseL()
    {
        GManager.instance.isEnglish = 0;
    }
    public void EnglishL()
    {
        GManager.instance.isEnglish = 1;
    }
    public void resetN()
    {
        
        GManager.instance.daycount = 0;
        GManager.instance.houseTrg = 0;
        GManager.instance.walktrg = true;
        GManager.instance.bossbattletrg = 0;
        GManager.instance.ESCtrg = false;
        GManager.instance.over = false;
        GManager.instance.setmenu = 0;
        GManager.instance.txtget = "";
        GManager.instance.endtitle = false;
        GManager.instance.pushtrg = false;
        GManager.instance.Coin = 0;
        for(int i = 0;i < GManager.instance.EventNumber.Length;)
        {
            GManager.instance.EventNumber[i] = 0;
           i++;
        }
        for (int i = 0; i < GManager.instance.freenums.Length;)
        {
            GManager.instance.freenums[i] = 0;
            i++;
        }
        GManager.instance.posX = 39f;
        GManager.instance.posY = 0.5f;
        GManager.instance.posZ = 0;
        PlayerPrefs.SetFloat("posX", GManager.instance.posX);
        PlayerPrefs.SetFloat("posY", GManager.instance.posY);
        PlayerPrefs.SetFloat("posZ", GManager.instance.posZ);
        GManager.instance.stageNumber = 0;
        for (int i = 0; i < GManager.instance.ItemID.Length;)
        {
            if (i == 59)
            {
                GManager.instance.ItemID[i].itemnumber = PlayerPrefs.GetInt("itemnumber" + 59, 0);
                GManager.instance.ItemID[i].gettrg = PlayerPrefs.GetInt("itemget" + 59, 0);
            }
            else
            {
                GManager.instance.ItemID[i].itemnumber = 0;
                GManager.instance.ItemID[i].gettrg = 0;
            }
            GManager.instance.ItemID[i]._quickset = -1;
            GManager.instance.ItemID[i]._equalsset = -1;
            GManager.instance.ItemID[i].pl_equalsselect = -1;
            i++;
        }
        for (int i = 0; i < GManager.instance.Quick_itemSet.Length;)
        {
            GManager.instance.Quick_itemSet[i] = -1;
            i++;
        }
        for (int i = 0; i < GManager.instance.P_equalsID.Length;)
        {
            GManager.instance.P_equalsID[i].hand_equals = -1;
            GManager.instance.P_equalsID[i].accessory_equals = -1;
            i++;
        }
        for (int i = 0; i < GManager.instance.Pstatus.Length;)
        {
            GManager.instance.Pstatus[i].add_hp = 0;
            GManager.instance.Pstatus[i].add_mp = 0;
            GManager.instance.Pstatus[i].add_at = 0;
            GManager.instance.Pstatus[i].add_df = 0;
            GManager.instance.Pstatus[i].maxHP = GManager.instance.Pclone[i].maxHP;
            GManager.instance.Pstatus[i].hp = GManager.instance.Pclone[i].hp;
            GManager.instance.Pstatus[i].maxMP = GManager.instance.Pclone[i].maxMP;
            GManager.instance.Pstatus[i].mp = GManager.instance.Pclone[i].mp;
            GManager.instance.Pstatus[i].defense = GManager.instance.Pclone[i].defense;
            GManager.instance.Pstatus[i].attack = GManager.instance.Pclone[i].attack;
            GManager.instance.Pstatus[i].Lv = GManager.instance.Pclone[i].Lv;
            GManager.instance.Pstatus[i].maxExp = GManager.instance.Pclone[i].maxExp;
            GManager.instance.Pstatus[i].inputExp = GManager.instance.Pclone[i].inputExp;
            GManager.instance.Pstatus[i].selectskill = -1;
            GManager.instance.Pstatus[i].magicselect = -1;
            for(int j = 0;j<GManager.instance.Pstatus[i].getMagic.Length;)
            {
                GManager.instance.Pstatus[i].getMagic[j].gettrg = 0;
                j++;
            }
            for (int j = 0; j < GManager.instance.Pstatus[i].magicSet.Length;)
            {
                    GManager.instance.Pstatus[i].magicSet[j] = -1;
                j++;
            }
            if (i == 0)
            {
                GManager.instance.Pstatus[i].getpl = 1;
            }
            else
            {
                GManager.instance.Pstatus[i].getpl = 0;
            }
            i++;
        }
        GManager.instance.playerselect = 0;
        for(int i = 0;i < GManager.instance.Triggers.Length;)
        {
            GManager.instance.Triggers[i] = 0;
            i++;
        }
        for (int i = 0; i < GManager.instance.missionID.Length;)
        {
            GManager.instance.missionID[i].inputmission = 0;
            i++;
        }
        for (int i = 0; i < GManager.instance.achievementsID.Length;)
        {
            GManager.instance.achievementsID[i].gettrg = 0;
            i++;
        }
        for (int i = 0; i < GManager.instance.enemynoteID.Length;)
        {
            GManager.instance.enemynoteID[i].gettrg = 0;
            i++;
        }
        GManager.instance.sunTime = 60;
        for (int i = 0; i < GManager.instance.mobDsTrg.Length;)
        {
            GManager.instance.mobDsTrg[i] = 0;
            i++;
        }
        for (int i = 0; i < GManager.instance._craftRecipe.Length;)
        {
            if (i > 14)
                GManager.instance._craftRecipe[i].get_recipe = PlayerPrefs.GetInt("get_recipe" + i, 0);
            else
                GManager.instance._craftRecipe[i].get_recipe = PlayerPrefs.GetInt("get_recipe" + i, 1);
            i++;
        }
    }
    int _MiniGame(int k)
    {
        //---------------
        GManager.instance._minigame.input_indexTrg = 0;
        for (int i = 0; i < k;)
        {
            oldrandom = Random.Range(0, 7);
            for (int j = 0; GManager.instance._minigame.input_missionID[0] == oldrandom || GManager.instance._minigame.input_missionID[1] == oldrandom || GManager.instance._minigame.input_missionID[2] == oldrandom; j++)
            {
                oldrandom = Random.Range(0, 7);
            }
            GManager.instance._minigame.input_missionID[i] = oldrandom;// PlayerPrefs.GetInt("minigame_missionID" + i, Random.Range(0,7));
            i++;
        }
        GManager.instance._minigame.input_missionID[3] = 7;
        GManager.instance.ItemID[46].itemscript = "";
        return k = oldrandom;
        //---------------
    }
    public void loadN()
    {
        //後でやる
        GManager.instance.houseTrg = PlayerPrefs.GetInt ("housetrg",0);
        GManager.instance.walktrg = true;
        GManager.instance.bossbattletrg = 0;
        GManager.instance.ESCtrg = false;
        GManager.instance.over = false;
        GManager.instance.setmenu = 0;
        GManager.instance.txtget = "";
        GManager.instance.endtitle = false;
        GManager.instance.pushtrg = false;
        GManager.instance.daycount = PlayerPrefs.GetInt ("dayc",0);
        GManager.instance.Coin = PlayerPrefs.GetInt ("coin",0);
        for (int i = 0; i < GManager.instance.colTrg.Length;)
        {
            GManager.instance.colTrg[i] = false;
            i++;
        }
        for (int i = 0; i < GManager.instance.EventNumber.Length;)
        {
            GManager.instance.EventNumber[i] = PlayerPrefs.GetInt("EvN"+i, 0);
            i++;
        }
        for (int i = 0; i < GManager.instance.freenums.Length;)
        {
            GManager.instance.freenums[i] = PlayerPrefs.GetFloat("freenums"+i,0);
            i++;
        }
        GManager.instance.posX = PlayerPrefs.GetFloat("posX", 39.0f);
        GManager.instance.posY = PlayerPrefs.GetFloat("posY", 0.5f); 
        GManager.instance.posZ = PlayerPrefs.GetFloat("posZ", 0f); 
        GManager.instance.stageNumber = PlayerPrefs.GetInt("stageN", 0); 
        //---------------
        GManager.instance._minigame.input_indexTrg = PlayerPrefs.GetInt("minigame_indexTrg", 0);
        for (int i = 0; i < GManager.instance._minigame.input_missionID.Length;)
        {
            GManager.instance._minigame.input_missionID[i] = PlayerPrefs.GetInt("minigame_missionID" + i, 0);
            i++;
        }
        GManager.instance._minigame.input_missionID[3] = 7;

        for (int i = 0; i < GManager.instance.ItemID.Length;)
        {
            GManager.instance.ItemID[i].itemnumber = PlayerPrefs.GetInt("itemnumber" + i, 0);
            GManager.instance.ItemID[i].gettrg = PlayerPrefs.GetInt("itemget" + i, 0);
            GManager.instance.ItemID[i]._quickset = PlayerPrefs.GetInt("item_quickset" + i, -1);
            GManager.instance.ItemID[i]._equalsset = PlayerPrefs.GetInt("item_equalsset" + i, -1);
            GManager.instance.ItemID[i].pl_equalsselect = PlayerPrefs.GetInt("pl_equalsselect" + i, -1);
            i++;
        }
        GManager.instance.ItemID[46].itemscript = PlayerPrefs.GetString("itemscript46", "");
        for (int i = 0; i < GManager.instance.Quick_itemSet.Length;)
        {
            GManager.instance.Quick_itemSet[i] = PlayerPrefs.GetInt("quick_itemset" + i, -1);
            i++;
        }
        for (int i = 0; i < GManager.instance.P_equalsID.Length;)
        {
            GManager.instance.P_equalsID[i].hand_equals = PlayerPrefs.GetInt("hand_equals" + i, -1);
            GManager.instance.P_equalsID[i].accessory_equals = PlayerPrefs.GetInt("accessory_equals" + i, -1);
            i++;
        }
        //---------------
        for (int i = 0; i < GManager.instance.Pstatus.Length;)
        {
            GManager.instance.Pstatus[i].add_hp = PlayerPrefs.GetInt("addhp" + i, 0);
            GManager.instance.Pstatus[i].add_mp = PlayerPrefs.GetInt("addmp" + i, 0);
            GManager.instance.Pstatus[i].add_at = PlayerPrefs.GetInt("addat" + i, 0);
            GManager.instance.Pstatus[i].add_df = PlayerPrefs.GetInt("adddf" + i, 0);
            GManager.instance.Pstatus[i].maxHP = PlayerPrefs.GetInt("pmaxhp"+i,GManager.instance.Pclone[i].maxHP);
            GManager.instance.Pstatus[i].hp = PlayerPrefs.GetInt("php" + i, GManager.instance.Pclone[i].hp);
            GManager.instance.Pstatus[i].maxMP = PlayerPrefs.GetInt("pmaxmp" + i, GManager.instance.Pclone[i].maxMP);
            GManager.instance.Pstatus[i].mp = PlayerPrefs.GetInt("pmp" + i, GManager.instance.Pclone[i].mp);
            GManager.instance.Pstatus[i].defense = PlayerPrefs.GetInt("pdf" + i, GManager.instance.Pclone[i].defense);
            GManager.instance.Pstatus[i].attack = PlayerPrefs.GetInt("pat" + i, GManager.instance.Pclone[i].attack);
            GManager.instance.Pstatus[i].Lv = PlayerPrefs.GetInt("plv" + i, GManager.instance.Pclone[i].Lv);
            GManager.instance.Pstatus[i].maxExp = PlayerPrefs.GetInt("pmaxexp" + i, GManager.instance.Pclone[i].maxExp);
            GManager.instance.Pstatus[i].inputExp = PlayerPrefs.GetInt("pinputexp" + i, GManager.instance.Pclone[i].inputExp);
            GManager.instance.Pstatus[i].selectskill = PlayerPrefs.GetInt("pselectskill" + i, -1); 
            GManager.instance.Pstatus[i].magicselect = PlayerPrefs.GetInt("pselectmagic" + i, -1);
            for (int j = 0; j < GManager.instance.Pstatus[i].inputskill.Length;)
            {
                GManager.instance.Pstatus[i].inputskill[j] = PlayerPrefs.GetInt("pinputskill" + i + "" + j, GManager.instance.Pstatus[i].inputskill[j]);
                j++;
            }
            for (int j = 0; j < GManager.instance.Pstatus[i].getMagic.Length;)
            {
                GManager.instance.Pstatus[i].getMagic[j].gettrg = PlayerPrefs.GetInt("pgetmagictrg" + i +""+j, 0) ;
                j++;
            }
            for (int j = 0; j < GManager.instance.Pstatus[i].magicSet.Length;)
            {
                    GManager.instance.Pstatus[i].magicSet[j] = PlayerPrefs.GetInt("pmagicset" + i + "" + j, -1);
                j++;
            }
            if (i == 0)
            {
                GManager.instance.Pstatus[i].getpl = PlayerPrefs.GetInt("getpl"+i,1);
            }
            else
            {
                GManager.instance.Pstatus[i].getpl = PlayerPrefs.GetInt("getpl" + i, 0);
            }
            i++;
        }
        GManager.instance.playerselect = PlayerPrefs.GetInt("plselect", 0);
        for (int i = 0; i < GManager.instance.Triggers.Length;)
        {
            GManager.instance.Triggers[i] = PlayerPrefs.GetInt("gmtrg"+i, 0); 
            i++;
        }
        for (int i = 0; i < GManager.instance.missionID.Length;)
        {
            GManager.instance.missionID[i].inputmission = PlayerPrefs.GetInt("inputmission" + i, 0);
            i++;
        }
        for (int i = 0; i < GManager.instance.achievementsID.Length;)
        {
            GManager.instance.achievementsID[i].gettrg = PlayerPrefs.GetInt("achiget" + i, 0);
            i++;
        }
        for (int i = 0; i < GManager.instance.enemynoteID.Length;)
        {
            GManager.instance.enemynoteID[i].gettrg = PlayerPrefs.GetInt("enemynoteget" + i, 0);
            i++;
        }
        if(GManager.instance.stageNumber != 0)
        {
            GManager.instance.audioMax = PlayerPrefs.GetFloat("audioMax", 0.16f);
            GManager.instance.seMax = PlayerPrefs.GetFloat("seMax", 0.12f);
            GManager.instance.mode = PlayerPrefs.GetInt("Mode", 1);
            GManager.instance.isEnglish = PlayerPrefs.GetInt("isEn", 0);
            GManager.instance.reduction = PlayerPrefs.GetInt("Reduction", 0);
            GManager.instance.autoviewup = PlayerPrefs.GetInt("viewUp", 1);
            GManager.instance.autolongdash = PlayerPrefs.GetInt("longDash", 1);
            GManager.instance.autoattack = PlayerPrefs.GetInt("autoattack", 1);
            GManager.instance.rotpivot = PlayerPrefs.GetFloat("rotpivot", 2f);
        }
        GManager.instance.sunTime = PlayerPrefs.GetFloat("suntime", 60);
        for (int i = 0; i < GManager.instance.mobDsTrg.Length;)
        {
            GManager.instance.mobDsTrg[i] = PlayerPrefs.GetInt("mdt"+i,0);
            i++;
        }
        for(int i=0;i<GManager.instance._craftRecipe.Length;)
        {
            if(i>14)
                GManager.instance._craftRecipe[i].get_recipe = PlayerPrefs.GetInt("get_recipe" + i, 0);
            else
                GManager.instance._craftRecipe[i].get_recipe = PlayerPrefs.GetInt("get_recipe" + i, 1);
            i++;
        }
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
