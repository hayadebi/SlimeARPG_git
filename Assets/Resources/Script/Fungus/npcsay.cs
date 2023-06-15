using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System.IO;
using System;
[RequireComponent(typeof(Flowchart))]
public class npcsay : MonoBehaviour
{
    public localLnpc llnpc = null;
    public sayTrigger saySc = null;
    public Transform P = null;
    public float py = 0;
    public objAngle objangle = null;
    public Animator eanim = null;
    public CameraController ccm = null;
    public bool audiostop = false;
    public GameObject bossoffobj = null;
    public int eventnumber = 0;
    public int defaulteventadd = 1;
    public int[] addevent;
    public int[] addEnumber;
    public GameObject wall;
    public AudioClip bgm;
    public AudioClip eventsound;
    public int GetCoin;
    public int Trigger = -1;
    public int allID;
    public int inputitemnumber;
    public float returnTime = 3;
    public GameObject[] UI;
    public bool nulleventdestroy = false;
    public int nullversion = 1;//1はe!=i、2はe<i、3はe>i
    public int[] shopID;
    public int[] stoneID;
    //-----------
    public int EventNumber = -1;
    public string DestroyOBJtext = "";
    public bool nextevent = true;
    public bool sayreturn;
    public int inputNumber;
    public int npctype;
    public int missionID;
    public int subID = -1;
    public int missionnumber;
    public string PlayerTag = "Player";
    bool saytrg = false;
    public string message = "test";
    bool isTalking = false;
    Flowchart flowChart;
    public string storyText = "";
    public string storyText2 = "";
    private GameObject mainc = null;
    private GameObject cmrot = null;
    private GameObject dfpos = null;
    private Vector3 cmvec;
    public int inputslskill = -1;
    public BoxCollider boxc;
    public SphereCollider spcol;
    public int getItem = -1;
    public int getPlayer = -1;
    public bool notCMRmove = false;
    public int selectSlime = -1;
    public int _AddMelancholy = 0;
    [Header("隠れんぼ用")]
    public int input_HintID = -1;
    public bool set_itemHint = false;
    public GameObject _notGameObject = null;
    public bool UIsummon = false;
    public int _inputLocal = 0;
    public int[] tips_trg;
    public int subEv_index = 0;
    public int subEv_numover = 0;
    public int set_minigameindex = -1;
    [Header("ショップ用追加")]
    public string shop_name="None";//ショップ固有Name(SaveLoad用)
    public int[] item_randomlist;
    private int shopYear;
    private int shopMonth;
    private int shopDay;
    private DateTime checkday=DateTime.Today;
    public bool itemshoptrg = false;
    // Start is called before the first frame update
    void Start()
    {
        if (Trigger != -1)
        {
            if (GManager.instance.Triggers[Trigger] == 1)
            {
                Destroy(gameObject);
            }
        }
        flowChart = this.GetComponent<Flowchart>();
        if (nulleventdestroy == true && inputNumber != GManager.instance.EventNumber[eventnumber] && nullversion == 1)
        {
            Destroy(gameObject);
        }
        else if (nulleventdestroy == true && inputNumber > GManager.instance.EventNumber[eventnumber] && nullversion == 2)
        {
            Destroy(gameObject);
        }
        else if (nulleventdestroy == true && inputNumber < GManager.instance.EventNumber[eventnumber] && nullversion == 3)
        {
            Destroy(gameObject);
        }
        llnpc = this.GetComponent<localLnpc>();
        //隠れんぼ処理
        if ( input_HintID != -1 && set_itemHint && GManager.instance._minigame.input_indexTrg <= 3 && GManager.instance._minigame.input_missionID[GManager.instance._minigame.input_indexTrg] == input_HintID)
        {
            if(GManager.instance._minigame.input_indexTrg == 0)
            {
                inputNumber = 3;
                missionID = 14;
                subID = 16;
            }
            else if (GManager.instance._minigame.input_indexTrg == 1)
            {
                inputNumber = 4;
                missionID = 16;
                subID = 17;
            }
            else if (GManager.instance._minigame.input_indexTrg == 2)
            {
                inputNumber = 5;
                missionID = 17;
                subID = 18;
            }
            else if (GManager.instance._minigame.input_indexTrg == 3)
            {
                EventNumber = 3;
                inputNumber = 6;
                missionID = 18;
            }
            if(GManager.instance.missionID[missionID].inputmission < 1)
            {
                _notGameObject.SetActive(false);
            }
        }
        else if ((input_HintID != -1 && set_itemHint && GManager.instance._minigame.input_indexTrg > 3) || (input_HintID != -1 && set_itemHint && GManager.instance._minigame.input_missionID[GManager.instance._minigame.input_indexTrg] != input_HintID))
        {
            _notGameObject.SetActive(false);
        }
        //------------
        mainc = GameObject.Find("MainC");
        cmrot = GameObject.Find("cmrot");
        dfpos = GameObject.Find("dfpos");
        //ショップ毎日更新
        if (PlayerPrefs.GetString(shop_name, "None") != "None" && itemshoptrg)
        {
            DateTime setdate = DateTime.Today;
            shopYear = PlayerPrefs.GetInt(shop_name+"_Year", setdate.Year);
            shopMonth = PlayerPrefs.GetInt(shop_name + "_Month", setdate.Month);
            shopDay = PlayerPrefs.GetInt(shop_name + "_Day", setdate.Day);
            checkday = new DateTime(shopYear, shopMonth, shopDay);
        }
        if (shop_name!= "None"&& itemshoptrg&&(PlayerPrefs.GetString(shop_name,"None")=="None" || (PlayerPrefs.GetString(shop_name, "None") != "None" && Math.Abs(GManager.instance.AllSpanCheck(checkday)) > 0)))
        {
            DateTime setdate = GManager.instance.GetGameDay();
            PlayerPrefs.SetString(shop_name, setdate.ToBinary().ToString());
            PlayerPrefs.SetInt(shop_name + "_Year", setdate.Year);
            PlayerPrefs.SetInt(shop_name + "_Month", setdate.Month);
            PlayerPrefs.SetInt(shop_name + "_Day", setdate.Day);

            shopID[0] = item_randomlist[UnityEngine.Random.Range(0, item_randomlist.Length)];
            int tmp_result = shopID[0];
            PlayerPrefs.SetInt(shop_name + "0", tmp_result);
            while (tmp_result == shopID[0])
            {
                tmp_result = item_randomlist[UnityEngine.Random.Range(0, item_randomlist.Length)];
                if (tmp_result != shopID[0]) break;
            }
            shopID[1] = tmp_result;
            PlayerPrefs.SetInt(shop_name + "1", tmp_result);
            while (tmp_result == shopID[0] || tmp_result == shopID[1])
            {
                tmp_result = item_randomlist[UnityEngine.Random.Range(0, item_randomlist.Length)];
                if (tmp_result != shopID[0] && tmp_result != shopID[1]) break;
            }
            shopID[2] = tmp_result;
            PlayerPrefs.SetInt(shop_name + "2", tmp_result);
            PlayerPrefs.Save();
        }
        else if(itemshoptrg && shop_name != "None" && PlayerPrefs.GetString(shop_name, "None") != "None")
        {
            shopID[0]= PlayerPrefs.GetInt(shop_name + "0", 0);
            shopID[1] = PlayerPrefs.GetInt(shop_name + "1", 0);
            shopID[2] = PlayerPrefs.GetInt(shop_name + "2", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (saytrg == false && npctype == 2 && GManager.instance.walktrg == true)
        {
            saytrg = true;
            StartCoroutine(Talk());
        }
        else if (saytrg == false && npctype == 3 && inputNumber == GManager.instance.EventNumber[eventnumber] && GManager.instance.walktrg == true)
        {
            saytrg = true;
            StartCoroutine(Talk());
        }
        else if (saytrg == false && npctype == 5 && GManager.instance.walktrg == true && GManager.instance.Sgetsay == true)
        {
            GManager.instance.Sgetsay = false;
            message = GManager.instance.skillsay;
            saytrg = true;
            StartCoroutine(Talk());
        }
        else if(npctype == 9 && inputslskill != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].slskillID != inputslskill && boxc && boxc.isTrigger)
        {
            boxc.isTrigger = false;
        }
        else if (npctype == 9 && inputslskill != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].slskillID == inputslskill && boxc && !boxc.isTrigger)
        {
            boxc.isTrigger = true;
        }
        else if (npctype == 9 && inputslskill != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].slskillID != inputslskill && spcol && spcol.isTrigger)
        {
            spcol.isTrigger = false;
        }
        else if (npctype == 9 && inputslskill != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].slskillID == inputslskill && spcol && !spcol.isTrigger)
        {
            spcol.isTrigger = true;
        }
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (GManager.instance.bossbattletrg == 0)
        {
            if (col.tag == PlayerTag && saytrg == false && npctype == 0 && GManager.instance.walktrg == true)
            {
                if (saySc == null)
                {
                    saytrg = true;
                    StartCoroutine(Talk());
                }
                else if (saySc != null && saySc.saystop == false)
                {
                    saytrg = true;
                    StartCoroutine(Talk());
                }
            }
            else if (col.tag == PlayerTag && saytrg == false && npctype == 1 && inputNumber == GManager.instance.EventNumber[eventnumber] && GManager.instance.walktrg == true)
            {
                if (saySc == null)
                {
                    saytrg = true;
                    StartCoroutine(Talk());
                }
                else if (saySc != null && saySc.saystop == false)
                {
                    saytrg = true;
                    StartCoroutine(Talk());
                }
            }
            else if (col.tag == PlayerTag && saytrg == false && npctype == 4 && GManager.instance.walktrg == true)
            {
                if (saySc == null)
                {
                    if (GManager.instance.ItemID[allID].itemnumber > (inputitemnumber - 1))
                    {
                        sayreturn = false;
                        GManager.instance.ItemID[allID].itemnumber -= inputitemnumber;
                        GManager.instance.Triggers[Trigger] = 1;
                        message = "itemget";
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                    else if (GManager.instance.ItemID[allID].itemnumber < inputitemnumber)
                    {
                        message = "itemnot";
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                }
                else if (saySc != null && saySc.saystop == false)
                {
                    if (GManager.instance.ItemID[allID].itemnumber > (inputitemnumber - 1))
                    {
                        sayreturn = false;
                        GManager.instance.ItemID[allID].itemnumber -= inputitemnumber;
                        GManager.instance.Triggers[Trigger] = 1;
                        message = "itemget";
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                    else if (GManager.instance.ItemID[allID].itemnumber < inputitemnumber)
                    {
                        message = "itemnot";
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                }
            }
            else if (col.tag == PlayerTag && saytrg == false && npctype == 6 && GManager.instance.walktrg == true && inputNumber == GManager.instance.EventNumber[eventnumber] )
            {
                if (saySc == null)
                {
                    if ((GManager.instance.missionID[missionID].inputitemnumber - 1) < GManager.instance.ItemID[GManager.instance.missionID[missionID].inputitemid].itemnumber && GManager.instance.missionID[missionID].inputmission > 0)
                    {
                        sayreturn = false;
                        GManager.instance.ItemID[GManager.instance.missionID[missionID].inputitemid].itemnumber -= GManager.instance.missionID[missionID].inputitemnumber;
                        message = "missinclear";
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                    else if (GManager.instance.ItemID[allID].itemnumber < inputitemnumber)
                    {
                        message = "nissionnot";
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                }
                else if(saySc != null && saySc.saystop == false)
                {
                    if ((GManager.instance.missionID[missionID].inputitemnumber - 1) < GManager.instance.ItemID[GManager.instance.missionID[missionID].inputitemid].itemnumber && GManager.instance.missionID[missionID].inputmission > 0)
                    {
                        sayreturn = false;
                        GManager.instance.ItemID[GManager.instance.missionID[missionID].inputitemid].itemnumber -= GManager.instance.missionID[missionID].inputitemnumber;
                        
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                    else if (GManager.instance.ItemID[allID].itemnumber < inputitemnumber)
                    {
                        
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                }
            }
            if (col.tag == PlayerTag && saytrg == false && npctype == 9 && inputslskill != -1 && GManager.instance.walktrg == true && GManager.instance.Pstatus[GManager.instance.playerselect].slskillID != inputslskill)
            {
                if (saySc == null)
                {
                    saytrg = true;
                    StartCoroutine(Talk());
                }
                else if (saySc != null && saySc.saystop == false)
                {
                    saytrg = true;
                    StartCoroutine(Talk());
                }
            }
            else if (col.tag == PlayerTag && !saytrg && npctype == 10 && inputNumber == GManager.instance.EventNumber[eventnumber] && GManager.instance.walktrg)
            {
                if (subEv_numover <= GManager.instance.EventNumber[subEv_index])
                {
                    message = "trueEv" + inputNumber.ToString();
                }
                else
                {
                    message = "badEv" + inputNumber.ToString();
                }

                if (saySc == null)
                {
                    saytrg = true;
                    StartCoroutine(Talk());
                }
                else if (saySc != null && !saySc.saystop)
                {
                    saytrg = true;
                    StartCoroutine(Talk());
                }
            }
            else if (col.tag == PlayerTag && saytrg == false && npctype == 11 && GManager.instance.walktrg && inputNumber == GManager.instance.EventNumber[eventnumber])
            {
                if (subEv_numover <= GManager.instance.EventNumber[subEv_index])
                {
                    message = "trueEv" + inputNumber.ToString();
                }
                else
                {
                    message = "badEv" + inputNumber.ToString();
                }
                if (saySc == null)
                {
                    if ((GManager.instance.missionID[missionID].inputitemnumber - 1) < GManager.instance.ItemID[GManager.instance.missionID[missionID].inputitemid].itemnumber && GManager.instance.missionID[missionID].inputmission > 0)
                    {
                        sayreturn = false;
                        GManager.instance.ItemID[GManager.instance.missionID[missionID].inputitemid].itemnumber -= GManager.instance.missionID[missionID].inputitemnumber;
                        message = "missinclear";
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                    else if (GManager.instance.ItemID[allID].itemnumber < inputitemnumber)
                    {
                        message = "nissionnot";
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                }
                else if (saySc != null && !saySc.saystop)
                {
                    if ((GManager.instance.missionID[missionID].inputitemnumber - 1) < GManager.instance.ItemID[GManager.instance.missionID[missionID].inputitemid].itemnumber && GManager.instance.missionID[missionID].inputmission > 0)
                    {
                        sayreturn = false;
                        GManager.instance.ItemID[GManager.instance.missionID[missionID].inputitemid].itemnumber -= GManager.instance.missionID[missionID].inputitemnumber;

                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                    else if (GManager.instance.ItemID[allID].itemnumber < inputitemnumber)
                    {
                        saytrg = true;
                        StartCoroutine(Talk());
                    }
                }
            }
        }
    }

    public IEnumerator Talk()
    {
        if(selectSlime != -1 && GManager.instance.playerselect != selectSlime && GManager.instance.Pstatus[selectSlime].hp > 0)
        {
            GManager.instance.playerselect = selectSlime;
        }
        GManager.instance.walktrg = false;
        GManager.instance.EventNumber[11] += _AddMelancholy;
        if(eanim != null)
        {
            eanim.enabled = false;
        }
        if (objangle != null)
        {
            objangle.enabled = false;
        }
        if(P != null)
        {
            var ppos = P.position;
            ppos.y = py+0.1f;
            P.position = ppos;
        }
        if(ccm != null)
        {
            ccm.enablecamera = false;
        }
        if(mainc != null && cmrot != null && dfpos != null && !notCMRmove)
        {
            cmvec = cmrot.transform.eulerAngles;
            cmvec.x = 0;
            cmvec.y = 0;
            cmvec.z = 0;
            cmrot.transform.eulerAngles = cmvec;
            mainc.transform.position = dfpos.transform.position;
            mainc.transform.eulerAngles = dfpos.transform.eulerAngles;
        }
        if (GetCoin != 0)
        {
            GManager.instance.txtget = "+" + GetCoin + "Coin！";
            GManager.instance.setrg = 1;
            GManager.instance.Coin += GetCoin;
        }
        //隠れんぼ処理
        if ( input_HintID != -1 && set_itemHint ) //&& GManager.instance._minigame.input_missionID[GManager.instance._minigame.input_indexTrg] == input_HintID)
        {
            if (GManager.instance._minigame.input_indexTrg == 0)
            {
                GManager.instance._minigame.input_indexTrg = 1;
            }
            else if (GManager.instance._minigame.input_indexTrg == 1)
            {
                GManager.instance._minigame.input_indexTrg = 2;
            }
            else if (GManager.instance._minigame.input_indexTrg == 2)
            {
                GManager.instance._minigame.input_indexTrg = 3;
            }
            else if (GManager.instance._minigame.input_indexTrg == 3)
            {
                GManager.instance._minigame.input_indexTrg = 4;
            }
            message = "mini"+GManager.instance._minigame.input_indexTrg ;
        }
        //------------
        if (npctype == 6 || npctype == 11)
        {
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.txtget = GManager.instance.missionID[missionID].name + "のミッションを達成！";
            }
            if (GManager.instance.isEnglish == 1)
            {
                GManager.instance.txtget = "Complete the " + GManager.instance.missionID[missionID].name2 + " mission！";
            }
            GManager.instance.setrg = 1;
            if (GManager.instance.missionID[missionID].getachievementsid != -1)
            {
                GManager.instance.achievementsID[GManager.instance.missionID[missionID].getachievementsid].gettrg = 1;
            }
            if (GManager.instance.missionID[missionID].getcoin > 0)
            {
                GManager.instance.Coin += GManager.instance.missionID[missionID].getcoin;
            }
            if (GManager.instance.missionID[missionID].getitemid != -1)
            {
                GManager.instance.ItemID[GManager.instance.missionID[missionID].getitemid].gettrg = 1;
                GManager.instance.ItemID[GManager.instance.missionID[missionID].getitemid].itemnumber += 1;
            }
            if (GManager.instance.missionID[missionID].getpl != -1)
            {
                GManager.instance.Pstatus[GManager.instance.missionID[missionID].getpl].getpl = 1;
            }
            GManager.instance.missionID[missionID].inputmission = 0;
        }
        if (EventNumber == 1 )
        {
            SaveDate();
            GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
        }
        else if(EventNumber == 3)
        {
            GameObject P = GameObject.Find("Player");
            wall.SetActive(true);
            GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
            GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
        }
        if(bgm != null )
        {
            GameObject BGM = GameObject.Find("BGM");
            AudioSource bgmA = BGM.GetComponent<AudioSource>();
            bgmA.Stop();
        }
        if (isTalking)
        {
            yield break;
        }
        isTalking = true;
        flowChart.SendFungusMessage(message);
        yield return new WaitUntil(() => flowChart.GetExecutingBlocks().Count == 0);
        isTalking = false;
        GManager.instance.walktrg = true;
        if(set_minigameindex != -1)
        {
            GManager.instance._minigame.input_indexTrg = set_minigameindex;
        }
        if (tips_trg != null)
        {
            for (int i = 0; i < tips_trg.Length;)
            {
                if (GManager.instance.Triggers[GManager.instance._Tips[tips_trg[i]].tips_trgID] < 1)
                {
                    GManager.instance.Triggers[GManager.instance._Tips[tips_trg[i]].tips_trgID] = 1;
                    GManager.instance.Triggers[106] += 1;
                }
                i += 1;
            }
        }
        if (getItem != -1  && set_itemHint && GManager.instance._minigame.input_indexTrg < 4)
        {
            GManager.instance.ItemID[46].itemscript = GManager.instance._minigame.set_itemScript[GManager.instance._minigame.input_missionID[GManager.instance._minigame.input_indexTrg]];
            GManager.instance.ItemID[46].itemscript2 = GManager.instance._minigame.set_itemScript2[GManager.instance._minigame.input_missionID[GManager.instance._minigame.input_indexTrg]];
            GManager.instance.ItemID[getItem].itemnumber += 1;
            GManager.instance.ItemID[getItem].gettrg = 1;
        }
        else if (getItem != -1)
        {
            GManager.instance.ItemID[getItem].itemnumber += 1;
            GManager.instance.ItemID[getItem].gettrg = 1;
        }
        if (getPlayer != -1)
        {
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.txtget = GManager.instance.Pstatus[getPlayer].pname + "を仲間にしました";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                GManager.instance.txtget = GManager.instance.Pstatus[getPlayer].pname + " is now a member of our group.";
            }
            GManager.instance.setrg = 1;
            GManager.instance.Pstatus[getPlayer].getpl = 1;
        }
        if(storyText != "" && UI != null && UI.Length != 0)
        {
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.storyUI = storyText;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                GManager.instance.storyUI = storyText2;
            }
            Instantiate(UI[0], transform.position, transform.rotation);
        }
        if(saySc != null)
        {
            saySc.saystop = true;
        }
        if (eanim != null)
        {
            eanim.enabled = true;
        }
        if (objangle != null)
        {
            objangle.enabled = true;
        }
        if (ccm != null)
        {
            ccm.enablecamera = true;
        }
        if (nextevent == true)
        {
            GManager.instance.EventNumber[eventnumber] += defaulteventadd;
            if(addevent.Length != 0)
            {
                for (int i = 0; i < addevent.Length;)
                {
                    if(addEnumber.Length != 0)
                    {
                        GManager.instance.EventNumber[addevent[i]] = addEnumber[i];
                    }
                    else
                    {
                        GManager.instance.EventNumber[addevent[i]] += defaulteventadd;
                    }
                    i++;
                }
            }
        }
        if (shopID.Length != 0)
        {
            for (int i = 0; i < (shopID.Length);)
            {
                GManager.instance.shopID[i] = shopID[i];
                i += 1;
            }
        }
        if (EventNumber == 2 && stoneID != null && stoneID.Length != 0 )
        {
            for (int i = 0; i < (stoneID.Length);)
            {
                GManager.instance.stoneID[i] = stoneID[i];
                i += 1;
            }
        }
        if (sayreturn == true)
        {
            Invoke("SayTrg", returnTime);
        }
        if (EventNumber == 2 || UIsummon)
        {
            GManager.instance.walktrg = false;
            GManager.instance.setmenu = 1;
            Instantiate(UI[0], transform.position, transform.rotation);
        }
        if (UI != null && _inputLocal != 0)
        {
            GManager.instance.walktrg = false;
            GManager.instance.setmenu = 1;
            GManager.instance.EventNumber[16] = _inputLocal;
            Instantiate(UI[0], transform.position, transform.rotation);
        }
        if (EventNumber == 6)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Application.OpenURL("https://twitter.com/hayadebi");
            Application.Quit();
        }
        else if (EventNumber == 7)
        {
            if(GManager.instance.isEnglish == 0)
            {
                GManager.instance.txtget = GManager.instance.ItemID[allID].itemname + "を手に入れた！";
            }
            else if(GManager.instance.isEnglish == 1)
            {
                GManager.instance.txtget = "I've got the " + GManager.instance.ItemID[allID].itemname2 + "！";
            }
            GManager.instance.ItemID[allID].itemnumber += 1;
            GManager.instance.ItemID[allID].gettrg = 1;
        }
        else if (EventNumber == 8)
        {
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.txtget = GManager.instance.achievementsID[allID].name + "の実績を解除した！";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                GManager.instance.txtget = "I released the " + GManager.instance.achievementsID[allID].name2 + " achievement！";
            }
            GManager.instance.setrg = 1;
            GManager.instance.achievementsID[allID].gettrg = 1;
        }
        else if (EventNumber == 9)
        {
            if (subID == -1)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = GManager.instance.missionID[missionID].name + "のミッションが開始しました";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "The " + GManager.instance.missionID[missionID].name + " mission has begun.";
                }
                GManager.instance.setrg = 1;
                GManager.instance.missionID[missionID].inputmission = 1;
            }
            else if(subID != -1)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = GManager.instance.missionID[subID].name + "のミッションが開始しました";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "The " + GManager.instance.missionID[subID].name + " mission has begun.";
                }
                GManager.instance.setrg = 1;
                GManager.instance.missionID[subID].inputmission = 1;
            }
        }
        else if (EventNumber == 10)
        {
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.txtget = GManager.instance.Pstatus[allID].pname + "が仲間になりました";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                GManager.instance.txtget = GManager.instance.Pstatus[allID].pname2 + " has joined our group.";
            }
            GManager.instance.setrg = 12;
            GManager.instance.Pstatus[allID].getpl = 1;
        }

        if (bgm != null&& audiostop == false && llnpc != null && !llnpc.bgmplay)
        {
            GameObject BGM = GameObject.Find("BGM");
            AudioSource bgmA = BGM.GetComponent<AudioSource>();
            bgmA.Stop();
            bgmA.clip = bgm;
            bgmA.Play();
        }
        if (EventNumber == 3)
        {
            if (bossoffobj != null)
            {
                bossoffobj.SetActive(false);
            }
            Boss();
        }
        if (DestroyOBJtext != "" && EventNumber != 3)
        {
            GameObject obj = GameObject.Find(DestroyOBJtext);
            Destroy(obj.gameObject);
        }
    }
    void Boss()
    {
        GManager.instance.bossbattletrg = 1;
        if (ccm != null)
        {
            ccm.zoomtrg = false;
        }
        if (bgm != null && audiostop == false && llnpc != null && !llnpc.bgmplay )
        {
            GameObject BGM = GameObject.Find("BGM");
            AudioSource bgmA = BGM.GetComponent<AudioSource>();
            bgmA.Stop();
            bgmA.clip = bgm;
            bgmA.Play();
        }
        Instantiate(UI[0], transform.position, transform.rotation);
        Destroy(gameObject, 1);
    }
    void SayTrg()
    {
        saytrg = false;
    }
    void SaveDate()
    {
        //後でやる
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
