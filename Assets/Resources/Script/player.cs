using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class player : MonoBehaviour
{
    public GameObject body;
    public ColEvent groundCol;
    public GameObject mouseA;
    [Header("停止気にするな")] public bool stoptrg = false;
    public float overhight = 9999;
    private bool highttrg = false;
    //public enemyS ES;
    public GameObject colobj;
    public float maxjump;
    public float jumptime = 1f;
    private float inputjtime;
    public int jumpmode = 0;
    public float jumpspeed;
    public Vector3 oldjumpP;
    public float gravity = 32;
    public bool dashtrg = false;
    public float dashtime;
    public float dashspeed;
    public GameObject dasheffect;
    public bool lostevent = false;
    private bool damagetrg = false;
    public string Anumbername = "Anumber";
    public float xSpeed = 0;
    //---------------------
    public float ySpeed = 0;
    //---------------------
    public float zSpeed = 0;
    public GameObject overUI;
    public GameObject menuUI;
    public AudioClip walkse;
    AudioSource audioSource;
    public Animator anim;
    Rigidbody rb;
    private int damage = 1;
    //状態異常関連の変数
    [Header ("追加エフェクト")]
    public GameObject EffectObj = null;
    private int randomEffect = 0;
    public bool holytrg = false;
    public bool darktrg = false;
    public bool flametrg = false;
    public float flametime = 0;
    private float damagetime = 0;
    public bool infinitytrg = false;
    public float infinitytime = 0;
    public bool poisontrg = false;
    public float poisontime;
    private float poisondamage;
    public float icetime;
    //private float frosttime = 0;
    //private int efevNumber = -1;
    //private GameObject frostobj = null;
    public float mudtime = 0;
    public float watertime = 0;
    public GameObject setblood;
    public bool bloodTrg = false;
    // Start is called before the first frame update
    public Camera mainc;
    public Transform summonP;
    [Header("※触るな")]
    public Vector3 mousepos;
    public int[] onMagic;
    public int inputnumber = 0;
    public int boxnumber = 0;
    public int[] onSkill;
    public int inputnumber2 = 0;
    public int boxnumber2 = 0;
    public int[] onItem;
    public int inputnumber3 = 0;
    public int boxnumber3 = 0;
    public enemyS ES;
    private int oldplayer = 0;
    private GameObject plobj;
    private GameObject dsobj;
    private bool rabbitTrg = false;
    private bool restarttrg = false;
    private float rocktime = 0;
    private int rockrandom = 0;
    private bool rap_trg = false;
    private float skillcureTime = 0;
    private float curecount = 0;
    private float itemcurecount = 0;
    public bool iceZoneTrg = false;
    private GameObject nearObj;
    private float searchTime = 0;

    //アイテム使用後の変数たち
    private float itemtime_tako = 0;
    public float itemtime_notWeather = 0;
    private int oldlampTrg = 0;
    public GameObject _Lamp = null;
    private float lampTime = 0;
    private GameObject bgmobj = null;
    private AudioSource bgmaudio = null;
    private Audiovolume bgmvolume = null;
    private int tmp_fieldcount = 0;
    public bool start_savepos = true;

    public int[] on_slimes;
    public int select_onslime;
    private float ftime = 0f;
    private float natural_curetime = 0f;
    GameObject serchTag(GameObject nowObj, string tagName)
    {
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        //string nearObjName = "";    //オブジェクト名称
        GameObject targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                //nearObjName = obs.name;
                targetObj = obs;
            }

        }
        //最も近かったオブジェクトを返す
        //return GameObject.Find(nearObjName);
        return targetObj;
    }

    void Start()
    {
        GManager.instance.Triggers[104] = 0;
        if(GManager.instance.stageNumber == 7 && !GManager.instance.walktrg )
        {
            GManager.instance.walktrg = true;
        }
        //GManager.instance.Triggers[23] = 0;
        bgmobj = GameObject.Find("BGM");
        if(bgmobj != null)
        {
            bgmaudio = bgmobj.GetComponent<AudioSource>();
            bgmvolume = bgmobj.GetComponent<Audiovolume>();
        }
        audioSource = this.GetComponent<AudioSource>();
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        GManager.instance.posX = PlayerPrefs.GetFloat("posX", 39f);
        GManager.instance.posY = PlayerPrefs.GetFloat("posY", 0.5f);
        GManager.instance.posZ = PlayerPrefs.GetFloat("posZ", 0f);
        //----
        Vector3 ppos = this.transform.position;
        ppos.x = GManager.instance.posX;
        ppos.y = GManager.instance.posY;
        ppos.z = GManager.instance.posZ;
        if(start_savepos )
            this.transform.position = ppos;
        anim.SetInteger(Anumbername, 0);

        if(GManager.instance.ItemID[64].itemnumber > 0 && GManager.instance.sunTime < 100 && GManager.instance.Pstatus[GManager.instance.playerselect].attacktype == "自然属性")
        {
            if(GManager.instance.isEnglish==0)GManager.instance.txtget = "現在\"ツボミンのしおり\"の、自然属性スライムを継続的にHP・MP回復させる効果が有効です。";
            else GManager.instance.txtget= "The effect of continuously restoring nature-aligned slimes' with the current \"Tsubomin Bookmark\" is active.";
        }
        setMagic();
        setSkill();
        setItem();
        OnSlimes();
        
    }
    void OnSlimes()
    {
        int onpl = 0;
        for(int i = 0; i<GManager.instance.Pstatus.Length;)
        {
            if (GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp > 0) onpl += 1;
            i++;
        }
        if (onpl > 0)
        {
            on_slimes = new int[onpl];
            int tmpindex = 0;
            select_onslime = 0;
            for (int i = 0; i < GManager.instance.Pstatus.Length;)
            {
                if (GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp > 0)
                {
                    on_slimes[tmpindex] = i;
                    if (i == GManager.instance.playerselect) select_onslime = tmpindex;
                    tmpindex += 1;
                }
                i++;
            }
        }
        else
        {
            ;
        }
    }
    //private float unloadtime = 0;
    // Update is called once per frame
    void SelectPlus(int tmp)
    {
        if (onMagic.Length == 0 && onMagic == null)
        {
            GManager.instance.setrg = 2;
        }
        else if(tmp < onMagic.Length)
        {
            GManager.instance.setrg = 1;
            GManager.instance.Pstatus[GManager.instance.playerselect].magicselect = tmp;
        }
        else
        {
            GManager.instance.setrg = 2;
        }
    }
    void SelectMinus()
    {
        if (onMagic.Length == 0 && onMagic == null)
        {
            GManager.instance.setrg = 2;
        }
        else if (GManager.instance.Pstatus[GManager.instance.playerselect].magicselect > 0 )
        {
            GManager.instance.setrg = 1;
            GManager.instance.Pstatus[GManager.instance.playerselect].magicselect -= 1;
        }
        else
        {
            GManager.instance.setrg = 2;
        }
    }
    void SelectPlus2(int tmp)
    {
        if (onSkill.Length == 0 && onSkill == null)
        {
            GManager.instance.setrg = 2;
        }
        else if(tmp < onSkill.Length)
        {
            GManager.instance.setrg = 1;
            GManager.instance.Pstatus[GManager.instance.playerselect].selectskill =tmp;
        }
        else
        {
            GManager.instance.setrg = 2;
        }
    }
    void SelectMinus2()
    {
        if (onSkill.Length == 0 && onSkill == null)
        {
            GManager.instance.setrg = 2;
        }
        else if (GManager.instance.Pstatus[GManager.instance.playerselect].selectskill > 0)
        {
            GManager.instance.setrg = 1;
            GManager.instance.Pstatus[GManager.instance.playerselect].selectskill -= 1;
        }
        else
        {
            GManager.instance.setrg = 2;
        }
    }
    void SelectPlus3(int tmp)
    {
        if (onItem.Length == 0 && onItem == null)
        {
            GManager.instance.setrg = 2;
        }
        else if (GManager.instance._quickSelect != -1 && tmp<onItem.Length)
        {
            GManager.instance.setrg = 1;
            GManager.instance._quickSelect = tmp;
        }
        else
        {
            GManager.instance.setrg = 2;
        }
    }
    void SelectMinus3()
    {
        if (onItem.Length == 0 && onItem == null)
        {
            GManager.instance.setrg = 2;
        }
        else if (GManager.instance._quickSelect != -1 && GManager.instance._quickSelect > 0)
        {
            GManager.instance.setrg = 1;
            GManager.instance._quickSelect -= 1;
        }
        else
        {
            GManager.instance.setrg = 2;
        }
    }

    void FixedUpdate()
    {
        if (GManager.instance.Pstatus[GManager.instance.playerselect].hp <= 0 && bgmobj != null && !bgmvolume.enabled && bgmaudio.volume > 0)
        {
            bgmaudio.volume -= (Time.deltaTime/8);
        }
        if (!GManager.instance.over && GManager.instance.walktrg == true && GManager.instance.setmenu == 0)
        {
            
            //ランプ
            if(oldlampTrg != GManager.instance.Triggers[102])
            {
                oldlampTrg = GManager.instance.Triggers[102];
                if(GManager.instance.Triggers[102] == 0 && _Lamp)
                {
                    _Lamp.SetActive(false);
                }
                else if (GManager.instance.Triggers[102] == 1 && !_Lamp)
                {
                    lampTime = 0;
                    var setPos = body.transform.position + (body.transform.right * 1.6f) + (body.transform.forward * 1.2f) + (body.transform.up * 0.8f);
                    _Lamp = Instantiate(GManager.instance.effectobj[18], setPos, GManager.instance.effectobj[18].transform.rotation, body.transform);
                }
                else if (GManager.instance.Triggers[102] == 1 && _Lamp)
                {
                    lampTime = 0;
                    _Lamp.SetActive(true);
                }
            }
            else if(GManager.instance.Triggers[102] == 1)
            {
                lampTime += Time.deltaTime;
                if(lampTime >= 15 && GManager.instance.Pstatus[GManager.instance.playerselect].mp > 0)
                {
                    lampTime = 0;
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp -= 1;
                }
                else if (GManager.instance.Pstatus[GManager.instance.playerselect].mp <= 0)
                {
                    lampTime = 0;
                    GManager.instance.setrg = 6;
                    GManager.instance.Triggers[102] = 0;
                }
            }
            //瀕死ですよ
            if(GManager.instance.Pstatus[GManager.instance.playerselect].hp <= (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 4) && rap_trg == false)
            {
                rap_trg = true;
                GManager.instance.setrg = 26;
                if(GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = "瀕死になりそうです！回復しましょう";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "I'm dying! Let's recover!";
                }
            }
            else if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 4) && rap_trg == true)
            {
                rap_trg = false;
            }
            //時間経過
            if (skillcureTime > 0 && GManager.instance.playerselect == 5)
            {
                skillcureTime -= Time.deltaTime;
                curecount += Time.deltaTime;
                if (curecount >= 2)
                {
                    curecount = 0;
                    for (int i = 0; i < GManager.instance.Pstatus.Length;)
                    {
                        if (GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp > 0)
                        {
                            if (GManager.instance.Pstatus[i].maxHP > GManager.instance.Pstatus[i].hp)
                            {
                                GManager.instance.Pstatus[i].hp += 1;
                            }
                        }
                        i++;
                    }
                }
            }
            if (GManager.instance.Triggers[178] > 0 && GManager.instance.Triggers[178] - 1 == GManager.instance.playerselect)
            {
                itemcurecount += Time.deltaTime;
                if(itemcurecount >= 180)
                {
                    itemcurecount = 0;
                    if(GManager.instance.isEnglish == 0)
                        GManager.instance.txtget = "デビ・コーラの効果が切れた！";
                    else
                        GManager.instance.txtget = "The Devi・Cola effect wore off!";
                    GManager.instance.Triggers[178] = 0;
                }
            }
            //持続回復
            if ((GManager.instance.Pstatus[GManager.instance.playerselect].maxHP > GManager.instance.Pstatus[GManager.instance.playerselect].hp && GManager.instance.playerselect == 7) ||
            (GManager.instance.Triggers[178] > 0 && GManager.instance.Triggers[178] - 1 == GManager.instance.playerselect))
            {
                curecount += Time.deltaTime;
                if (curecount >= 2)
                {
                    curecount = 0;
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > 0 && GManager.instance.Pstatus[GManager.instance.playerselect].maxHP > GManager.instance.Pstatus[GManager.instance.playerselect].hp)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].hp += 1;
                    }
                }
            }

            //しおり効果
            if (GManager.instance.ItemID[64].itemnumber>0 && GManager.instance.sunTime<100 && GManager.instance.Pstatus[GManager.instance.playerselect].attacktype == "自然属性")
            {
                natural_curetime += Time.deltaTime;
                if (natural_curetime >= 2)
                {
                    natural_curetime = 0;
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > 0 && GManager.instance.Pstatus[GManager.instance.playerselect].maxHP > GManager.instance.Pstatus[GManager.instance.playerselect].hp)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].hp += 1;
                    }
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > 0 && GManager.instance.Pstatus[GManager.instance.playerselect].maxMP > GManager.instance.Pstatus[GManager.instance.playerselect].mp)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].mp += 1;
                    }
                }
            }
            //if(itemtime_notWeather > 0)
            //{
            //    itemtime_notWeather -= Time.deltaTime;
            //    if(itemtime_notWeather <= 0)
            //    {
            //        itemtime_notWeather = 0;
            //    }
            //}
            if (GManager.instance.Triggers[94] == 2)
            {
                itemtime_tako += Time.deltaTime;
                if (itemtime_tako >= 120)
                {
                    itemtime_tako = 0;
                    GManager.instance.Triggers[94] = 0;
                }
            }
            else if (GManager.instance.Triggers[94] == 1)
            {
                itemtime_tako += Time.deltaTime;
                if(itemtime_tako >= 60)
                {
                    itemtime_tako = 0;
                    GManager.instance.Triggers[94] = 0;
                }
            }
            //ダッシュモード
            if (GManager.instance.Pstatus[GManager.instance.playerselect].loadtime == 0 && dashtrg == false)
            {
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].loadtime = 1.2f;
                    GManager.instance.Pstatus[GManager.instance.playerselect].maxload = 1.2f;
                    dashtrg = true;
                    dasheffect.SetActive(true);
                    audioSource.PlayOneShot(GManager.instance.managerSE[0]);
                    anim.SetInteger(Anumbername, 2);
                }
            }
            else if(dashtrg == true && Input.GetKeyUp(KeyCode.LeftShift) && GManager.instance.autolongdash == 0)
            {
                dashtime = 0;
                dashtrg = false;
                dasheffect.SetActive(false);
            }
            else if (dashtrg == true)
            {
                dashtime += Time.deltaTime;
                if (dashtime > 0.5f)
                {
                    dashtime = 0;
                    dashtrg = false;
                    dasheffect.SetActive(false);
                }
            }
            //スキル、技、ダッシュ使用制限タイム
            if (GManager.instance.Pstatus[GManager.instance.playerselect].loadtime != 0)
            {
                if (rabbitTrg == false)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].loadtime -= Time.deltaTime;
                }
                else if (rabbitTrg == true)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].loadtime -= (Time.deltaTime*2f);
                }
                if (GManager.instance.Pstatus[GManager.instance.playerselect].loadtime < 0.1f)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].loadtime = 0;
                }
            }
            if(rocktime > 0)//防御態勢タイム
            {
                rocktime -= Time.deltaTime;
            }
            //移動してない時の重力操作
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && groundCol.ColTrigger == true)
            {
                ySpeed -= gravity;
                if (anim.GetInteger(Anumbername) == 1)
                {
                    anim.SetInteger(Anumbername, 0);
                }
            }
            if (groundCol.ColTrigger == false && jumpmode == 0)
            {
                ySpeed -= gravity;
            }
            else if (groundCol.ColTrigger == true && jumpmode == 0 && ySpeed != 0 && !Input.GetKey(KeyCode.LeftShift))
            {
                oldjumpP = this.transform.position;
                inputjtime = 0;
                jumpmode = 0;
                ySpeed = 0;
            }
            //異常状態
            if(mudtime >= 0)
            {
                mudtime -= Time.deltaTime;
            }
            if (watertime >= 0)
            {
                watertime -= Time.deltaTime;
            }
            if(icetime >= 0)
            {
                icetime -= Time.deltaTime;
            }
            else if(stoptrg == true)
            {
                stoptrg = false;
            }
            if (flametrg)//燃焼
            {
                flametime += Time.deltaTime;
                damagetime += Time.deltaTime;
                if (flametime > 15)
                {
                    flametime = 0;
                    damagetime = 0;
                    flametrg = false;
                }
                else if (damagetime > 2)
                {
                    damagetime = 0;
                    damage = 1 + GManager.instance.Pstatus[GManager.instance.playerselect].defense;
                    OnDamage();
                }
            }
            if (infinitytrg)//燃焼
            {
                infinitytime += Time.deltaTime;
                if (infinitytime > 2)
                {
                    infinitytime = 0;
                    damage = 1 + GManager.instance.Pstatus[GManager.instance.playerselect].defense;
                    OnDamage();
                }
            }
            if (poisontrg)//毒
            {
                poisontime += Time.deltaTime;
                poisondamage += Time.deltaTime;
                if (poisontime > 30)
                {
                    poisontime = 0;
                    poisondamage = 0;
                    poisontrg = false;
                }
                else if (poisondamage > 2)
                {
                    poisondamage = 0;
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > 1)
                    {
                        damage = 1+ GManager.instance.Pstatus[GManager.instance.playerselect].defense;
                        OnDamage();
                    }
                }
            }
            //else if (stoptrg && efevNumber == 4)//氷
            //{
            //    frosttime += Time.deltaTime;
            //    if (frosttime > 1f)
            //    {
            //        frosttime = 0;
            //        efevNumber = -1;
            //        stoptrg = false;
            //        startjump = true;
            //    }
            //}
            //-------------
        }
        //メニュー画面出現
        if (GManager.instance.setmenu < 1 && GManager.instance.walktrg == true && Input.GetKey(KeyCode.Escape) && !stoptrg)
        {
            GameObject m = GameObject.Find("menu(Clone)");
            GManager.instance.ESCtrg = false;
            GManager.instance.walktrg = true;
            anim.SetInteger(Anumbername, 0);
            ySpeed = 0;
            if (m == null)
            {
                GManager.instance.setmenu = 1;
                GManager.instance.walktrg = false;
                GManager.instance.setrg = 6;
                Instantiate(menuUI, transform.position, transform.rotation);
            }
        }
        if (GManager.instance.setmenu < 1 && GManager.instance.walktrg && Input.GetKey(KeyCode.T) && !stoptrg)
        {
            GameObject m = GameObject.Find("selectTips(Clone)");
            GManager.instance.ESCtrg = false;
            GManager.instance.walktrg = true;
            anim.SetInteger(Anumbername, 0);
            ySpeed = 0;
            if (m == null)
            {
                GManager.instance.setmenu = 1;
                GManager.instance.walktrg = false;
                GManager.instance.setrg = 6;
                Instantiate(GManager.instance.effectobj[23], transform.position, transform.rotation);
            }
        }
        if (!GManager.instance.walktrg && (GManager.instance.instantP[0] != 0 || GManager.instance.instantP[1] != 0 || GManager.instance.instantP[2] != 0))
        {
            Vector3 setpp = this.transform.position;
            setpp.x = GManager.instance.instantP[0];
            setpp.y = GManager.instance.instantP[1];
            setpp.z = GManager.instance.instantP[2];
            GManager.instance.instantP[0] = 0;
            GManager.instance.instantP[1] = 0;
            GManager.instance.instantP[2] = 0;
            this.transform.position = setpp;
        }
        //--------------------------------------

        if (GManager.instance.walktrg && !GManager.instance.over && !stoptrg)
        {
            if(rb.useGravity)
            {
                rb.useGravity = false;
            }
            //視点移動＆魔法スキル
            if (GManager.instance.setmenu == 0 )
            {
                var distance = Vector3.Distance(this.transform.position, mainc.transform.position);
                var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                mousepos = mainc.ScreenToWorldPoint(mousePosition);
                mousepos.y = this.transform.position.y;
                var rotation = Quaternion.LookRotation(mousepos - body.transform.position);
                rotation.x = 0;
                body.transform.rotation = rotation;

                //切り替えモード変更  0=魔法 1=スキル 2=アイテム
                if (Input.GetKey(KeyCode.F1) && ftime<=0)
                {
                    ftime = 0.15f;
                    GManager.instance.Pstatus[GManager.instance.playerselect].changemode = 0;
                }
                else if (Input.GetKey(KeyCode.F2) && ftime <= 0)
                {
                    ftime = 0.15f;
                    GManager.instance.Pstatus[GManager.instance.playerselect].changemode = 1;
                }
                else if (Input.GetKey(KeyCode.F3) && ftime <= 0)
                {
                    ftime = 0.15f;
                    GManager.instance.Pstatus[GManager.instance.playerselect].changemode = 2;
                }
                //魔法切り替え
                if (onMagic != null && onMagic.Length != 0 && GManager.instance.over == false && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 0)
                {
                    if (Input.GetKey(KeyCode.Alpha1) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus(0);
                    }
                    else if (Input.GetKey(KeyCode.Alpha2) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus(1);
                    }
                    else if (Input.GetKey(KeyCode.Alpha3) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus(2);
                    }
                    else if (Input.GetKey(KeyCode.Alpha4) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus(3);
                    }
                }
                //スキル切り替え
                else if (onSkill != null && onSkill.Length != 0 && GManager.instance.over == false && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 1)
                {
                    if (Input.GetKey(KeyCode.Alpha1) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus2(0);
                    }
                    else if (Input.GetKey(KeyCode.Alpha2) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus2(1);
                    }
                    else if (Input.GetKey(KeyCode.Alpha3) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus2(2);
                    }
                }
                //アイテム切り替え
                else if (onItem != null && onItem.Length != 0 && GManager.instance._quickSelect != -1 && GManager.instance.over == false && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 2)
                {
                    if (Input.GetKey(KeyCode.Alpha1) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus3(0);
                    }
                    else if (Input.GetKey(KeyCode.Alpha2) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus3(1);
                    }
                    else if (Input.GetKey(KeyCode.Alpha3) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus3(2);
                    }
                    else if (Input.GetKey(KeyCode.Alpha4) && ftime <= 0)
                    {
                        ftime = 0.14f;
                        SelectPlus3(3);
                    }
                }
                //スライム切り替え
                if (Input.GetKey(KeyCode.F) && ftime <= 0 && oldplayer == GManager.instance.playerselect)
                {
                    ftime = 1f;
                    if (select_onslime < on_slimes.Length - 1)
                    {
                        select_onslime += 1;
                        GManager.instance.playerselect = on_slimes[select_onslime];
                    }
                    else if (select_onslime >= on_slimes.Length - 1)
                    {
                        select_onslime = 0;
                        GManager.instance.playerselect = on_slimes[select_onslime];
                    }
                    oldplayer = GManager.instance.playerselect;
                    GManager.instance.setrg = 9;
                    Instantiate(GManager.instance.effectobj[4], summonP.position, GManager.instance.effectobj[4].transform.rotation);
                    dsobj = summonP.transform.GetChild(0).gameObject;
                    plobj = Instantiate(GManager.instance.Pstatus[GManager.instance.playerselect].pobj, new Vector3(summonP.position.x, summonP.position.y + 0.05f, summonP.position.z), summonP.rotation, summonP);
                    anim = plobj.GetComponent<Animator>();
                    if (dsobj != null)
                    {
                        Destroy(dsobj.gameObject);
                    }
                    for (int i = 0; i < GManager.instance.colTrg.Length;)
                    {
                        GManager.instance.colTrg[i] = false;
                        i++;
                    }
                    if (GManager.instance.ItemID[64].itemnumber > 0 && GManager.instance.sunTime < 100 && GManager.instance.Pstatus[GManager.instance.playerselect].attacktype == "自然属性")
                    {
                        if (GManager.instance.isEnglish == 0) GManager.instance.txtget = "現在\"ツボミンのしおり\"の、自然属性スライムを継続的にHP・MP回復させる効果が有効です。";
                        else GManager.instance.txtget = "The effect of continuously restoring nature-aligned slimes' with the current \"Tsubomin Bookmark\" is active.";
                    }
                    setMagic();
                    setSkill();
                    OnSlimes();
                }
                else if (oldplayer != GManager.instance.playerselect && !Input.GetKey(KeyCode.F) && ftime <= 0)
                {
                    ftime = 0.3f;
                    oldplayer = GManager.instance.playerselect;
                    GManager.instance.setrg = 9;
                    Instantiate(GManager.instance.effectobj[4], summonP.position, GManager.instance.effectobj[4].transform.rotation);
                    dsobj = summonP.transform.GetChild(0).gameObject;
                    plobj = Instantiate(GManager.instance.Pstatus[GManager.instance.playerselect].pobj, new Vector3(summonP.position.x, summonP.position.y + 0.05f, summonP.position.z), summonP.rotation, summonP);
                    anim = plobj.GetComponent<Animator>();
                    if (dsobj != null)
                    {
                        Destroy(dsobj.gameObject);
                    }
                    for (int i = 0; i < GManager.instance.colTrg.Length;)
                    {
                        GManager.instance.colTrg[i] = false;
                        i++;
                    }
                    if (GManager.instance.ItemID[64].itemnumber > 0 && GManager.instance.sunTime < 100 && GManager.instance.Pstatus[GManager.instance.playerselect].attacktype == "自然属性")
                    {
                        if (GManager.instance.isEnglish == 0) GManager.instance.txtget = "現在\"ツボミンのしおり\"の、自然属性スライムを継続的にHP・MP回復させる効果が有効です。";
                        else GManager.instance.txtget = "The effect of continuously restoring nature-aligned slimes' with the current \"Tsubomin Bookmark\" is active.";
                    }
                    setMagic();
                    setSkill();
                }
                else if(ftime >= 0)
                {
                    ftime -= Time.deltaTime;
                }
                if (GManager.instance.Pstatus[GManager.instance.playerselect].loadtime == 0 && GManager.instance.Pstatus[GManager.instance.playerselect].magicselect != -1 && onMagic.Length != 0 && onMagic != null)//&& GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 0)
                {
                    //魔法使用
                    if (Input.GetMouseButton(0) && GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].inputmagicpower - 1 < GManager.instance.Pstatus[GManager.instance.playerselect].mp && GManager.instance.Pstatus[GManager.instance.playerselect].loadtime == 0)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].loadtime =
                            GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].shotmaxtime;
                        GManager.instance.Pstatus[GManager.instance.playerselect].maxload =
                            GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].shotmaxtime;

                        //——————————————————————————
                        GManager.instance.mouseP = mousepos;
                        mouseA.SetActive(true);
                        mouseA.transform.position = GManager.instance.mouseP;
                        //自動ターゲット設定がONなら
                        if (GManager.instance.autoattack == 1)
                        {
                            nearObj = serchTag(mouseA, "enemy");
                            if (nearObj != null)
                            {
                                GManager.instance.mouseP = nearObj.transform.position;
                            }
                        }
                        
                        Invoke("mouseAR", 0.3f);
                        GManager.instance.Pstatus[GManager.instance.playerselect].mp -=
                            GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].inputmagicpower;
                        GameObject magicattack = Instantiate(GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicobj, this.transform.position, body.transform.rotation, body.transform);
                        if (GManager.instance.playerselect == 7 && magicattack != null && magicattack.GetComponent<AddMagic>())
                        {
                            magicattack.GetComponent<AddMagic>().changeType = "神属性";
                        }
                        //ジャンプ
                        if (jumpmode < 1)
                        {
                            ySpeed = 0;
                            oldjumpP = this.transform.position;
                            var upp = transform.position;
                            upp.y += gravity;
                            transform.position = upp;
                            inputjtime = 0;
                            jumpmode = 1;
                            groundCol.ColTrigger = false;
                            if (dashtrg == false)
                            {
                                anim.SetInteger(Anumbername, 1);
                            }
                        }
                    }
                }
                if (GManager.instance.Pstatus[GManager.instance.playerselect].loadtime == 0 && GManager.instance.Pstatus[GManager.instance.playerselect].selectskill != -1 && onSkill.Length != 0 && onSkill != null &&GManager.instance.Pstatus[GManager.instance.playerselect].changemode != 2)// && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 1)
                {
                    //スキル使用
                    if (Input.GetMouseButton(1) && GManager.instance.Pstatus[GManager.instance.playerselect].loadtime == 0)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].loadtime =
                            GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillmaxbar;
                        GManager.instance.Pstatus[GManager.instance.playerselect].maxload =
                            GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillmaxbar;
                        GManager.instance.mouseP = mousepos;
                        mouseA.SetActive(true);
                        mouseA.transform.position = GManager.instance.mouseP;
                        Invoke("mouseAR", 0.3f);
                        if (GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].inputskillobj != null)
                        {
                            Instantiate(GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].inputskillobj, this.transform.position, body.transform.rotation, this.transform);
                        }
                        if (rabbitTrg == true)
                        {
                            ySpeed = 0;
                            oldjumpP = this.transform.position;
                            var upp = transform.position;
                            upp.y += gravity;
                            transform.position = upp;
                            inputjtime = 0;
                            jumpmode = 1;
                            groundCol.ColTrigger = false;
                            if (dashtrg == false)
                            {
                                anim.SetInteger(Anumbername, 1);
                                audioSource.PlayOneShot(walkse);
                            }
                        }
                        if(GManager.instance.playerselect == 5)
                        {
                            Instantiate(GManager.instance.effectobj[15], this.transform.position, this.transform.rotation, this.transform);
                            skillcureTime = 30;
                            if (GManager.instance.isEnglish == 0)
                            {
                                GManager.instance.txtget = "30秒間味方を持続回復します";
                            }
                            else if (GManager.instance.isEnglish == 1)
                            {
                                GManager.instance.txtget = "Sustained healing of allies for 30 seconds";
                            }
                        }
                    }
                }
                if (GManager.instance.Pstatus[GManager.instance.playerselect].loadtime == 0 && GManager.instance._quickSelect != -1 && onItem.Length != 0 && onItem != null && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 2)//&& GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 0)
                {
                    //アイテム使用
                    if (Input.GetMouseButton(1) && GManager.instance.Pstatus[GManager.instance.playerselect].loadtime == 0)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].loadtime = 0.3f;
                        GManager.instance.Pstatus[GManager.instance.playerselect].maxload = 0.3f;
                        //———消費———
                        if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber > 0)
                        {
                            if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 1)//HP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 5;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 2)//MP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 5;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 3 && GManager.instance.bossbattletrg == 0 && SceneManager.GetActiveScene().name == "stage" + GManager.instance.stageNumber && GManager.instance.stageNumber != 0)//セーブアイテム
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                //SaveDate();
                                GManager.instance.setrg = 8;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 4)//ユニムーシュ
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.Pstatus[GManager.instance.playerselect].inputExp = GManager.instance.Pstatus[GManager.instance.playerselect].maxExp;
                                GManager.instance.setrg = 7;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 5)//毒消し
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                poisontime = 0;
                                poisontrg = false;
                                if (EffectObj != null)
                                {
                                    Destroy(EffectObj.gameObject);
                                }
                                GManager.instance.setrg = 8;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 6)//火傷治し
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                flametime = 0;
                                flametrg = false;
                                infinitytime = 0;
                                infinitytrg = false;
                                if (EffectObj != null)
                                {
                                    Destroy(EffectObj.gameObject);
                                }
                                GManager.instance.setrg = 8;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 7)//エリクサー
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                for (int i = 0; i < GManager.instance.Pstatus.Length;)
                                {
                                    if (GManager.instance.Pstatus[i].getpl > 0)
                                    {
                                        GManager.instance.Pstatus[i].hp = GManager.instance.Pstatus[i].maxHP;
                                        GManager.instance.Pstatus[i].mp = GManager.instance.Pstatus[i].maxMP;
                                    }
                                    i++;
                                }
                                poisontime = 0;
                                poisontrg = false;
                                flametime = 0;
                                flametrg = false;
                                infinitytime = 0;
                                infinitytrg = false;
                                icetime = 0;
                                mudtime = 0;
                                watertime = 0;
                                darktrg = false;
                                if (setblood != null)
                                {
                                    Destroy(setblood.gameObject);
                                }
                                bloodTrg = false;
                                if (EffectObj != null)
                                {
                                    Destroy(EffectObj.gameObject);
                                }
                                GManager.instance.setrg = 14;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 8)//粉
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                if (Random.Range(0, 13) == 0)//hp小回復
                                {
                                    GManager.instance.setrg = 16;
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp += 5;
                                    if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                    {
                                        GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                    }
                                }
                                else if (Random.Range(0, 13) == 1)//mp小回復
                                {
                                    GManager.instance.setrg = 16;
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp += 5;
                                    if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                    {
                                        GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                    }
                                }
                                else if (Random.Range(0, 13) == 2)//hp中回復
                                {
                                    GManager.instance.setrg = 15;
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 3);
                                    if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                    {
                                        GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                    }
                                }
                                else if (Random.Range(0, 13) == 3)//mp中回復
                                {
                                    GManager.instance.setrg = 15;
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxMP / 3);
                                    if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                    {
                                        GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                    }
                                }
                                else if (Random.Range(0, 13) == 4)//毒回復
                                {
                                    poisontime = 0;
                                    poisontrg = false;
                                    if (EffectObj != null)
                                    {
                                        Destroy(EffectObj.gameObject);
                                    }
                                    GManager.instance.setrg = 8;
                                }
                                else if (Random.Range(0, 13) == 5)//火傷回復
                                {
                                    flametime = 0;
                                    flametrg = false;
                                    infinitytime = 0;
                                    infinitytrg = false;
                                    if (EffectObj != null)
                                    {
                                        Destroy(EffectObj.gameObject);
                                    }
                                    GManager.instance.setrg = 8;
                                }
                                else if (Random.Range(0, 13) == 6)//エリクサーもどき
                                {
                                    for (int i = 0; i < GManager.instance.Pstatus.Length;)
                                    {
                                        if (GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp < 1)
                                        {
                                            GManager.instance.Pstatus[i].hp = GManager.instance.Pstatus[i].maxHP / 3;
                                            GManager.instance.Pstatus[i].mp = GManager.instance.Pstatus[i].maxMP;
                                        }
                                        i++;
                                    }
                                    GManager.instance.setrg = 14;
                                }
                                else if (Random.Range(0, 13) == 7)//LvUP
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].inputExp = GManager.instance.Pstatus[GManager.instance.playerselect].maxExp;
                                    GManager.instance.setrg = 7;
                                }
                                else if (Random.Range(0, 13) == 8)//セーブ
                                {
                                    if (GManager.instance.bossbattletrg == 0 && SceneManager.GetActiveScene().name == "stage" + GManager.instance.stageNumber && GManager.instance.stageNumber != 0)
                                    {
                                        //SaveDate();
                                        GManager.instance.setrg = 8;
                                    }
                                }
                                else if (Random.Range(0, 13) == 9)//燃焼
                                {
                                    flametime = 0;
                                    flametrg = true;
                                    EffectObj = Instantiate(GManager.instance.effectobj[7], transform.position, transform.rotation, transform);
                                    GManager.instance.setrg = 18;
                                }
                                else if (Random.Range(0, 13) == 10)//毒
                                {
                                    poisontime = 0;
                                    poisontrg = true;
                                    EffectObj = Instantiate(GManager.instance.effectobj[6], transform.position, transform.rotation, transform);
                                    GManager.instance.setrg = 17;
                                }
                                else if (Random.Range(0, 13) == 11)//神の裁き
                                {
                                    holytrg = true;
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp /= 2;
                                    EffectObj = Instantiate(GManager.instance.effectobj[11], transform.position, transform.rotation, transform);
                                    GManager.instance.setrg = 19;
                                }
                                else if (Random.Range(0, 13) == 12)//闇の呪い
                                {
                                    darktrg = true;
                                    EffectObj = Instantiate(GManager.instance.effectobj[11], transform.position, transform.rotation, transform);
                                    GManager.instance.setrg = 20;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 9)//HP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 16;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 15;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 11)//HP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 15;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 3);
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 13)//HP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 14;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 2);
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 10)//MP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 16;
                                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 15;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 12)//MP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 15;
                                GManager.instance.Pstatus[GManager.instance.playerselect].mp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxMP / 3);
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 14)//MP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 14;
                                GManager.instance.Pstatus[GManager.instance.playerselect].mp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxMP / 2);
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 15)//人工エリクサー
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 14;
                                for (int i = 0; i < GManager.instance.Pstatus.Length;)
                                {
                                    if (GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp < 1)
                                    {
                                        GManager.instance.Pstatus[i].hp = GManager.instance.Pstatus[i].maxHP / 3;
                                        GManager.instance.Pstatus[i].mp = GManager.instance.Pstatus[i].maxMP;
                                    }
                                    i++;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 16)//HP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 23;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 10;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 17)//MP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 22;
                                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 10;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 18)//万能
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                poisontime = 0;
                                poisontrg = false;
                                flametime = 0;
                                flametrg = false;
                                infinitytime = 0;
                                infinitytrg = false;
                                icetime = 0;
                                mudtime = 0;
                                watertime = 0;
                                darktrg = false;
                                if (setblood != null)
                                {
                                    Destroy(setblood.gameObject);
                                }
                                bloodTrg = false;
                                if (EffectObj != null)
                                {
                                    Destroy(EffectObj.gameObject);
                                }
                                GManager.instance.setrg = 15;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 19)//爆炎
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.itemMagicID = 14;
                                Instantiate(GManager.instance.effectobj[12], transform.position, transform.rotation, transform);
                                GManager.instance.setrg = 1;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 20)//零水
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.itemMagicID = 22;
                                Instantiate(GManager.instance.effectobj[12], transform.position, transform.rotation, transform);
                                GManager.instance.setrg = 1;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 21)//風来
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.itemMagicID = 8;
                                Instantiate(GManager.instance.effectobj[12], transform.position, transform.rotation, transform);
                                GManager.instance.setrg = 1;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 22)//HP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 15;
                                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 15;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                                else if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 23)//HP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 5;
                                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 5;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                                else if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 24)//完全蘇生
                            {
                                GManager.instance.setmenu += 1;
                                Instantiate(GManager.instance.effectobj[16], this.transform.position, this.transform.rotation);
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 25)//HP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 31;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 10;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                                if (GManager.instance.isEnglish == 0)
                                {
                                    GManager.instance.txtget = "60秒間水属性のダメージを軽減します";
                                }
                                else if (GManager.instance.isEnglish == 1)
                                {
                                    GManager.instance.txtget = "Reduces water type damage for 60 seconds";
                                }
                                GManager.instance.Triggers[94] = 1;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 26)//瞬間移動
                            {
                                GManager.instance.setmenu += 1;
                                Instantiate(GManager.instance.effectobj[17], this.transform.position, this.transform.rotation);
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 27)//MP回復悪天候無効化
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 20;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                }
                                itemtime_notWeather = 300;
                                if (GManager.instance.isEnglish == 0)
                                {
                                    GManager.instance.txtget = "5分間フィールド効果を無効化します";
                                }
                                else if (GManager.instance.isEnglish == 1)
                                {
                                    GManager.instance.txtget = "Disables field effects for 5 minutes";
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 28)//MP回復悪天候無効化
                            {
                                GManager.instance.setrg = 6;
                                if (GManager.instance.Triggers[102] == 0)
                                {
                                    GManager.instance.Triggers[102] = 1;
                                }
                                else if (GManager.instance.Triggers[102] == 1)
                                {
                                    GManager.instance.Triggers[102] = 0;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 29)//HP回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 40;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 30)//最大HPMP増加
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].maxHP += 3;
                                GManager.instance.Pstatus[GManager.instance.playerselect].maxMP += 3;
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 34 && GManager.instance.Triggers[178] < 1)//持続回復
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 22;
                                GManager.instance.Triggers[178] = GManager.instance.playerselect + 1;
                                if (GManager.instance.isEnglish == 0)
                                {
                                    GManager.instance.txtget = "デビ・コーラの効果で3分間持続回復されます";
                                }
                                else if (GManager.instance.isEnglish == 1)
                                {
                                    GManager.instance.txtget = "The use of Devi・Cola gave a recovery effect that lasted for 3 minutes.";
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 31)//HP回復水属性攻撃無効
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 20;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                                if (GManager.instance.isEnglish == 0)
                                {
                                    GManager.instance.txtget = "120秒間水属性のダメージを無効化します";
                                }
                                else if (GManager.instance.isEnglish == 1)
                                {
                                    GManager.instance.txtget = "Disables water type damage for 120 seconds.";
                                }
                                GManager.instance.Triggers[94] = 2;
                            }

                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 32)//HP回復HP成長値1増加
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].add_hp += 1;
                                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 15;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                                }
                                if (GManager.instance.isEnglish == 0)
                                {
                                    GManager.instance.txtget = "LvUP時のHP上昇値が1増えた";
                                }
                                else if (GManager.instance.isEnglish == 1)
                                {
                                    GManager.instance.txtget = "The HP increase at LvUP was increased by 1.";
                                }
                            }
                            else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].eventnumber == 33)//MP回復全最大値成長値2増加
                            {
                                GManager.instance.ItemID[GManager.instance.Quick_itemSet[onItem[GManager.instance._quickSelect]]].itemnumber -= 1;
                                GManager.instance.setrg = 7;
                                GManager.instance.Pstatus[GManager.instance.playerselect].add_hp += 2;
                                GManager.instance.Pstatus[GManager.instance.playerselect].add_mp += 2;
                                GManager.instance.Pstatus[GManager.instance.playerselect].add_at += 2;
                                GManager.instance.Pstatus[GManager.instance.playerselect].add_df += 2;
                                GManager.instance.Pstatus[GManager.instance.playerselect].maxHP += 2;
                                GManager.instance.Pstatus[GManager.instance.playerselect].maxMP += 2;
                                GManager.instance.Pstatus[GManager.instance.playerselect].attack += 2;
                                GManager.instance.Pstatus[GManager.instance.playerselect].defense += 2;
                                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 40;
                                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                                {
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                }
                                if (GManager.instance.isEnglish == 0)
                                {
                                    GManager.instance.txtget = "LvUP時の各ステータス上昇値が2増えた";
                                }
                                else if (GManager.instance.isEnglish == 1)
                                {
                                    GManager.instance.txtget = "The value of each status increase at LvUP was increased by 2.";
                                }
                            }
                        }
                        else
                        {
                            GManager.instance.setrg = 27;
                        }
                        //------------------------------------
                        //ジャンプ
                        if (jumpmode < 1)
                        {
                            ySpeed = 0;
                            oldjumpP = this.transform.position;
                            var upp = transform.position;
                            upp.y += gravity;
                            transform.position = upp;
                            inputjtime = 0;
                            jumpmode = 1;
                            groundCol.ColTrigger = false;
                            if (dashtrg == false)
                            {
                                anim.SetInteger(Anumbername, 1);
                            }
                        }
                    }
                }
            }
            //落下ゲームオーバー
            if (overhight < 9999)
            {
                if (this.transform.position.y < overhight && !highttrg)
                {
                    highttrg = true;
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = 0;
                    Instantiate(GManager.instance.effectobj[3], transform.position, transform.rotation);
                    GameOver();
                }
            }
            //----
            if (damagetrg == false)
            {
                //----移動----
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    if (groundCol.ColTrigger == true && jumpmode == 0)
                    {
                        oldjumpP = this.transform.position;
                        var upp = transform.position;
                        upp.y += gravity;
                        transform.position = upp;
                        jumpmode = 1;
                        tmp_fieldcount += 1 ;
                        //--------------------------
                        if (GManager.instance.stageNumber == 8 && tmp_fieldcount % 9 == 0 && Random.Range(0,9)==0 && itemtime_notWeather <= 0)
                        {
                            GManager.instance.setrg = 35;
                            if (GManager.instance.isEnglish == 0)
                            {
                                GManager.instance.txtget = "フィールド効果で2秒間凍結状態になった";
                            }
                            else if (GManager.instance.isEnglish == 1)
                            {
                                GManager.instance.txtget = "The field effect froze for 2 seconds.";
                            }
                            EffectObj = Instantiate(GManager.instance.effectobj[10], this.transform.position, this.transform.rotation, this.transform);
                            icetime = 2f;
                            stoptrg = true;
                        }
                        //--------------------------
                        if (tmp_fieldcount >= 999)
                        {
                            tmp_fieldcount = 0;
                        }
                        groundCol.ColTrigger = false;
                        if (dashtrg == false)
                        {
                            anim.SetInteger(Anumbername, 1);
                            if (!audioSource.isPlaying)
                            {
                                audioSource.PlayOneShot(walkse);
                            }
                        }
                    }
                }
                if (jumpmode > 0)
                {
                    inputjtime += Time.deltaTime;
                    if (jumpmode == 1)
                    {
                        if (this.transform.position.y < oldjumpP.y + maxjump && !Input.GetKey(KeyCode.LeftShift))
                        {
                            if (rabbitTrg == false)
                            {
                                ySpeed += jumpspeed;
                            }
                            else if (rabbitTrg == true)
                            {
                                ySpeed += (jumpspeed * 1.5f);
                            }
                        }
                        if (inputjtime > jumptime)
                        {
                            jumpmode = 2;
                        }
                        else if (this.transform.position.y > oldjumpP.y + maxjump)
                        {
                            jumpmode = 2;
                        }
                        if (groundCol.ColTrigger == true)
                        {
                            oldjumpP = this.transform.position;
                            inputjtime = 0;
                            jumpmode = 0;
                            ySpeed = 0;
                        }
                    }
                    else if (jumpmode == 2)
                    {
                        ySpeed -= gravity;
                        if (groundCol.ColTrigger == true)
                        {
                            oldjumpP = this.transform.position;
                            inputjtime = 0;
                            jumpmode = 0;
                            ySpeed = 0;
                            anim.SetInteger(Anumbername, 0);
                        }
                    }
                }
                var inputX = Input.GetAxisRaw("Horizontal");
                var inputZ = Input.GetAxisRaw("Vertical");
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) inputZ = 0;
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) inputX = 0;
                //if (GManager.instance.skillselect == 3)
                //{
                //    inputX *= -1;
                //    inputZ *= -1;
                //}
                var tempVc = new Vector3(inputX, 0, inputZ);
                if (tempVc.magnitude > 1) tempVc = tempVc.normalized;
                dashspeed = 1;
                if (dashtrg == true)
                {
                    if (watertime > 0)
                    {
                        dashspeed = 3;
                    }
                    else if (mudtime > 0)
                    {
                        dashspeed = 3;
                    }
                    else
                    {
                        dashspeed = 4;
                    }
                }
                if(iceZoneTrg && GManager.instance.Pstatus[GManager.instance.playerselect].slskillID != 2 && itemtime_notWeather <= 0)
                {
                    dashspeed = (dashspeed / 3) * 2;
                }
                if(GManager.instance.playerselect == 6 && GManager.instance.colTrg[0])
                {
                    dashspeed = 1.5f;
                }
                var vec = body.transform.right * tempVc.x + ((body.transform.forward * tempVc.z) * dashspeed);

                var movevec = vec * GManager.instance.Pstatus[GManager.instance.playerselect].speed + Vector3.up * (ySpeed);
                if (watertime > 0)
                {
                    movevec = vec * (GManager.instance.Pstatus[GManager.instance.playerselect].speed / 2) + Vector3.up * (ySpeed);
                }
                else if(mudtime > 0)
                {
                    movevec = vec * ((GManager.instance.Pstatus[GManager.instance.playerselect].speed /3)*2) + Vector3.up * (ySpeed);
                }
                //------------
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        rb.velocity = movevec;
                    }
                }
                else
                {
                    rb.velocity = movevec;
                }
            }
        }
        else if (GManager.instance.walktrg == false || stoptrg == true)
        {
            if (!rb.useGravity)
            {
                dasheffect.SetActive(false);
                rb.useGravity = true;
                audioSource.Stop();
                rb.velocity = Vector3.zero;
            }
        }
    }
    void mouseAR()
    {
        mouseA.SetActive(false);
    }
    public void OnDamage()
    {
        if (stoptrg == false && !poisontrg && !flametrg && !infinitytrg)
        {
            var rotation = Quaternion.LookRotation(colobj.transform.position - this.transform.position);
            rotation.x = 0;
            rotation.z = 0;
            body.transform.rotation = rotation;
            Instantiate(GManager.instance.effectobj[1], colobj.transform.position, colobj.transform.rotation);
            rb.velocity = Vector3.zero;
            Vector3 velocity = -body.transform.forward * 12.8f;
            //風力を与える
            rb.AddForce(velocity, ForceMode.VelocityChange);
        }
        if (damagetrg == false)
        {
            if (damage == 0)
            {
                ;
            }
            else if (darktrg && 1 > (damage - (GManager.instance.Pstatus[GManager.instance.playerselect].defense / 2)))
            {
                GManager.instance.Pstatus[GManager.instance.playerselect].hp -= 1;
            }
            else if (darktrg && 0 < (damage - (GManager.instance.Pstatus[GManager.instance.playerselect].defense/2)))
            {
                GManager.instance.Pstatus[GManager.instance.playerselect].hp -= (damage - (GManager.instance.Pstatus[GManager.instance.playerselect].defense/2));
            }
            else if (1 > (damage - GManager.instance.Pstatus[GManager.instance.playerselect].defense))
            {
                GManager.instance.Pstatus[GManager.instance.playerselect].hp -= 1;
            }
            else if (0 < (damage - GManager.instance.Pstatus[GManager.instance.playerselect].defense))
            {
                GManager.instance.Pstatus[GManager.instance.playerselect].hp -= (damage - GManager.instance.Pstatus[GManager.instance.playerselect].defense);
            }
            if (colobj && colobj.tag == "ebullet")
            {
                Destroy(colobj.gameObject, 0.1f);
            }
        }
        if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > 0 && damagetrg == false)
        {
            GManager.instance.setrg = 5;
            anim.SetInteger(Anumbername, 1);
        }
        else if (GManager.instance.Pstatus[GManager.instance.playerselect].hp < 1)
        {
            Instantiate(GManager.instance.effectobj[2], transform.position, transform.rotation);
            GameOver();
        }
        if (!poisontrg && !flametrg && !infinitytrg )
        {
            damagetrg = true;
            Invoke("Damage", 1f);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (GManager.instance.over != true && GManager.instance.walktrg == true)
        {
            if (collision.gameObject.tag == "red")
            {
                Instantiate(GManager.instance.effectobj[3], transform.position, transform.rotation);
                GManager.instance.Pstatus[GManager.instance.playerselect].hp = 0;
                GameOver();
            }
            if (collision.tag == "enemy" || collision.tag == "ebullet")
            {
                //追加状態異常
                if (collision.GetComponent<AddEffect>() && collision.GetComponent<AddEffect>().enabled == true && !dashtrg && dashtime == 0)//&& GManager.instance.skillselect == -1)
                {
                    AddEffect ef = collision.GetComponent<AddEffect>();
                    if (ef.effectnumber == 0 && mudtime <= 0)//泥
                    {
                        if(GManager.instance.isEnglish == 0)
                        {
                            GManager.instance.txtget = "3秒間泥状態になった";
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            GManager.instance.txtget = "I was covered in mud for three seconds.";
                        }
                        EffectObj = Instantiate(GManager.instance.effectobj[5], this.transform.position, this.transform.rotation, this.transform);
                        mudtime = 3;
                    }
                    if (ef.effectnumber == 1 && !poisontrg)//毒
                    {
                        if (GManager.instance.isEnglish == 0)
                        {
                            GManager.instance.txtget = "30秒間毒状態になった";
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            GManager.instance.txtget = "Poisoned for 30 seconds.";
                        }
                        EffectObj = Instantiate(GManager.instance.effectobj[6], this.transform.position, this.transform.rotation, this.transform);
                        poisontrg = true;
                        poisontime = 0;
                    }
                    if(ef.effectnumber == 2)//ノックバック
                    {
                        var rotation = Quaternion.LookRotation(collision.transform.position - this.transform.position);
                        rotation.x = 0;
                        rotation.z = 0;
                        body.transform.rotation = rotation;
                        rb.velocity = Vector3.zero;
                        Vector3 velocity = -body.transform.forward * 24f;
                        //風力を与える
                        rb.AddForce(velocity, ForceMode.VelocityChange);
                    }
                    if (ef.effectnumber == 3 && !flametrg)//燃焼
                    {
                        if (GManager.instance.isEnglish == 0)
                        {
                            GManager.instance.txtget = "15秒間火傷状態になった";
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            GManager.instance.txtget = "He was in a burn state for 15 seconds.";
                        }
                        EffectObj = Instantiate(GManager.instance.effectobj[7], this.transform.position, this.transform.rotation, this.transform);
                        flametrg = true;
                        flametime = 0;
                    }
                    if (ef.effectnumber == 4 && !infinitytrg)//燃焼
                    {
                        if (GManager.instance.isEnglish == 0)
                        {
                            GManager.instance.txtget = "永久的に煉獄状態になった";
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            GManager.instance.txtget = "You'll be in a permanent state of purgatory.";
                        }
                        EffectObj = Instantiate(GManager.instance.effectobj[8], this.transform.position, this.transform.rotation, this.transform);
                        infinitytrg = true;
                        infinitytime = 0;
                    }
                    if (ef.effectnumber == 5 && watertime <= 0)//水
                    {
                        if (GManager.instance.isEnglish == 0)
                        {
                            GManager.instance.txtget = "5秒間びしょ濡れ状態になった";
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            GManager.instance.txtget = "I was soaking wet for five seconds.";
                        }
                        EffectObj = Instantiate(GManager.instance.effectobj[9], this.transform.position, this.transform.rotation, this.transform);
                        watertime = 5;
                    }
                    if (ef.effectnumber == 6 && icetime <= 0)//氷
                    {
                        if (GManager.instance.isEnglish == 0)
                        {
                            GManager.instance.txtget = "2秒間凍結状態になった";
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            GManager.instance.txtget = "He was frozen for two seconds.";
                        }
                        EffectObj = Instantiate(GManager.instance.effectobj[10], this.transform.position, this.transform.rotation, this.transform);
                        icetime = 1f;
                        stoptrg = true;
                    }
                    if ((ef.effectnumber == 7 || ef.effectnumber == 10)&& !holytrg)//光
                    {
                        randomEffect = Random.Range(0, 5);
                        if (randomEffect == 0)
                        {
                            holytrg = true;
                            if (GManager.instance.isEnglish == 0)
                            {
                                GManager.instance.txtget = "神の裁きによってHPを奪われた";
                            }
                            else if (GManager.instance.isEnglish == 1)
                            {
                                GManager.instance.txtget = "By God's judgment, you've lost your HP.";
                            }
                            EffectObj = Instantiate(GManager.instance.effectobj[11], this.transform.position, this.transform.rotation, this.transform);
                            GManager.instance.Pstatus[GManager.instance.playerselect].hp /= 2;
                        }
                    }
                    if (ef.effectnumber == 8 && !holytrg)//光
                    {
                        EffectObj = Instantiate(GManager.instance.effectobj[7], this.transform.position, this.transform.rotation, this.transform);
                        flametrg = true;
                        flametime = 0;
                        randomEffect = Random.Range(0, 5);
                        if (randomEffect == 0)
                        {
                            holytrg = true;
                            if (GManager.instance.isEnglish == 0)
                            {
                                GManager.instance.txtget = "神の裁きによってHPを奪われた";
                            }
                            else if (GManager.instance.isEnglish == 1)
                            {
                                GManager.instance.txtget = "By God's judgment, you've lost your HP.";
                            }
                            EffectObj = Instantiate(GManager.instance.effectobj[11], this.transform.position, this.transform.rotation, this.transform);
                            GManager.instance.Pstatus[GManager.instance.playerselect].hp /= 2;
                        }
                    }
                    if ((ef.effectnumber == 9 || ef.effectnumber == 10)&& !darktrg)//闇
                    {
                        randomEffect = Random.Range(0, 4);
                        if (randomEffect == 0)
                        {
                            darktrg = true;
                            if (GManager.instance.isEnglish == 0)
                            {
                                GManager.instance.txtget = "闇の呪いによって防御力を奪われた";
                            }
                            else if (GManager.instance.isEnglish == 1)
                            {
                                GManager.instance.txtget = "The curse robbed me of my defenses.";
                            }
                            EffectObj = Instantiate(GManager.instance.effectobj[11], this.transform.position, this.transform.rotation, this.transform);
                            
                        }
                    }
                    if (ef.effectnumber == 12 && !bloodTrg && !setblood)//致命傷
                    {
                        GManager.instance.setrg = 26;
                        if (GManager.instance.isEnglish == 0)
                        {
                            GManager.instance.txtget = "致命傷によって次のダメージが倍に！";
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            GManager.instance.txtget = "The next damage is doubled because it was fatal.";
                        }
                        EffectObj = Instantiate(GManager.instance.effectobj[21], this.transform.position, this.transform.rotation, this.transform);
                        setblood = EffectObj;
                    }
                }
                //-------------
                if (collision.GetComponent<AddDamage>())
                {
                    damage = collision.GetComponent<AddDamage>().Damage;
                    if (GManager.instance.mode == 0)
                    {
                        damage -= 1;
                    }
                    else if (GManager.instance.mode == 2)
                    {
                        damage += 1;
                    }
                    //軽減
                    if (GManager.instance.Triggers[94] == 2 && collision.GetComponent<AddDamage>().attacktype == "水属性")
                    {
                        damage = 0;
                    }
                    else if (GManager.instance.Triggers[94] == 1 && collision.GetComponent<AddDamage>().attacktype == "水属性")
                    {
                        damage -= (GManager.instance.Pstatus[GManager.instance.playerselect].defense / 3);
                    }
                    if (damage != 0 && GManager.instance.playerselect == 6 && collision.GetComponent<AddDamage>().attacktype == "火属性")
                    {
                        damage = (damage / 3) * 2;
                    }
                    if (collision.GetComponent<AddDamage>().nokill == true && damage - GManager.instance.Pstatus[GManager.instance.playerselect].defense > 0 && damage - GManager.instance.Pstatus[GManager.instance.playerselect].defense >= GManager.instance.Pstatus[GManager.instance.playerselect].hp)
                    {
                        damage = 0;
                    }
                    if (collision.GetComponent<AddDamage>().attacktype == GManager.instance.Pstatus[GManager.instance.playerselect].badtype || collision.GetComponent<AddDamage>().attacktype == "神属性")
                    {
                        damage = (damage * 3) / 2;
                    }
                }
                else if (collision.GetComponent<enemyS>())
                {
                    ES = collision.gameObject.GetComponent<enemyS>();
                    damage = ES.Estatus.attack;
                    if (GManager.instance.mode == 0)
                    {
                        damage -= 1;
                    }
                    else if (GManager.instance.mode == 2)
                    {
                        damage += 1;
                    }
                    if (GManager.instance.Triggers[94] == 2 && collision.GetComponent<AddDamage>().attacktype == "水属性")
                    {
                        damage = 0;
                    }
                    else if (GManager.instance.Triggers[94] == 1 && collision.GetComponent<AddDamage>().attacktype == "水属性")
                    {
                        damage -= (GManager.instance.Pstatus[GManager.instance.playerselect].defense / 3);
                    }
                    if (damage != 0 && GManager.instance.playerselect == 6 && ES.Estatus.attacktype == "火属性")
                    {
                        damage = (damage / 3) * 2;
                    }
                    //if (GManager.instance.skillselect == 11 && GManager.instance.SkillID[GManager.instance.skillselect].inputskillbar < 2)
                    //{
                    //    damage = damage * 3 / 2;
                    //}
                    if (ES.Estatus.attacktype == GManager.instance.Pstatus[GManager.instance.playerselect].badtype || ES.Estatus.attacktype == "神属性")
                    {
                        damage = (damage * 3) / 2;
                    }
                }
                if(rocktime > 0)
                {
                    damage /= 2;
                }
                if(GManager.instance.playerselect == 7 && (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP/2) >= GManager.instance.Pstatus[GManager.instance.playerselect].hp)
                {
                    damage /= 2;
                }
                if(setblood && bloodTrg )
                {
                    damage *= 2;
                    Destroy(setblood.gameObject);
                    bloodTrg = false;
                }
                if (GManager.instance.playerselect == 4)
                {
                    rockrandom = Random.Range(0, 7);
                    if(rockrandom == 1)
                    {
                        damage = 0;
                        GManager.instance.setrg = 25;
                    }
                }
                if (dashtrg == true)
                {
                    dashtime = 0;
                    dashtrg = false;
                    dasheffect.SetActive(false);
                    damage = 0;
                }
                //if (GManager.instance.skillselect == 10 && GManager.instance.stageNumber > 7)
                //{
                //    damage = 1;
                //}
                colobj = collision.gameObject;
                OnDamage();
            }
        }
    }
    private void GameOver()
    {
        if (rb.useGravity)
        {
            rb.useGravity = false;
        }
        if (GManager.instance.Triggers[178] > 0 && GManager.instance.Triggers[178] - 1 == GManager.instance.playerselect)
            itemcurecount = 0;
        for (int i = 0; i < GManager.instance.Pstatus.Length;)
        {
            if (GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp > 0)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = GManager.instance.Pstatus[GManager.instance.playerselect].pname + "がダウンした";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = GManager.instance.Pstatus[GManager.instance.playerselect].pname2 + " is down";
                }
                GManager.instance.playerselect = i;
                restarttrg = true;
                i = GManager.instance.Pstatus.Length;
            }
            i++;
        }
        if (restarttrg == false || highttrg == true)
        {
            if (!lostevent)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (GManager.instance.bossbattletrg == 1)
                {
                    GManager.instance.bossbattletrg = 0;
                }
                GManager.instance.over = true;
                audioSource.Stop();
                GManager.instance.setrg = 4;
                Resources.UnloadUnusedAssets();
                Instantiate(overUI, transform.position, transform.rotation);
                Destroy(gameObject, 0.1f);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (GManager.instance.bossbattletrg > 0)
                {
                    GManager.instance.bossbattletrg = 0;
                }
                audioSource.Stop();
                GManager.instance.setrg = 4;
                Instantiate(overUI, transform.position, transform.rotation);
                GManager.instance.stageNumber = 7;
                GManager.instance.EventNumber[7] = 9;
                GManager.instance.daycount += 14;
                GManager.instance.EventNumber[11] += 3;
                GManager.instance.posX = -2323.5f;
                GManager.instance.posY = 26;
                GManager.instance.posZ = 51;
                PlayerPrefs.SetFloat("posX", GManager.instance.posX);
                PlayerPrefs.SetFloat("posY", GManager.instance.posY);
                PlayerPrefs.SetFloat("posZ", GManager.instance.posZ);
                PlayerPrefs.SetInt("stageN", GManager.instance.stageNumber);
                for (int i = 0; i < GManager.instance.EventNumber.Length;)
                {
                    PlayerPrefs.SetInt("EvN" + i, GManager.instance.EventNumber[i]);
                    i++;
                }
                PlayerPrefs.Save();
                if(bgmobj != null)
                {
                    bgmvolume.enabled = false;
                }
                Invoke("sceneChange", 12);
            }
        }
        else
        {
            GManager.instance.setrg = 13;
            restarttrg = false;
        }
    }
    void sceneChange()
    {
        GManager.instance.setmenu = 0;
        if (bgmobj != null)
        {
            bgmaudio.volume = 0;
        }
        GManager.instance.Pstatus[0].hp = 1;
        
        SceneManager.LoadScene("stage" + GManager.instance.stageNumber);
    }
    void Damage()
    {
        if (GManager.instance.over == false)
        {
            if (setblood && !bloodTrg)
            {
                bloodTrg = true;
            }
            anim.SetInteger(Anumbername, 0);
            rb.velocity = Vector3.zero;
            damagetrg = false;
        }
    }
    public void setMagic()
    {
        onMagic = null;
        boxnumber = 0;
        inputnumber = 0;
        for (int i = 0; GManager.instance.Pstatus[GManager.instance.playerselect].magicSet.Length > i;)
        {
            for (int l = 0; l == 0 && GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i] != -1;)
            {
                boxnumber += 1;
                l++;
            }
            i += 1;
        }
        onMagic = new int[boxnumber];
        for (int i = 0; GManager.instance.Pstatus[GManager.instance.playerselect].magicSet.Length > i;)
        {
            for (int l = 0; l == 0 && GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i] != -1;)
            {
                onMagic[inputnumber] = i;
                inputnumber += 1;
                l++;
            }
            i += 1;
        }
        if(onMagic != null && onMagic.Length != 0)
        {
            GManager.instance.Pstatus[GManager.instance.playerselect].magicselect = 0;
        }
        else
        {
            GManager.instance.Pstatus[GManager.instance.playerselect].magicselect = -1;
        }
        //if (onMagic.Length != 0 && onMagic != null)
        //{
        //    for (int i = 0; i < onMagic.Length;)
        //    {
        //        if (onMagic[i] != -1)
        //        {
        //            GManager.instance.Pstatus[GManager.instance.playerselect].magicselect = i;
        //            i = onMagic.Length;
        //        }
        //        i++;
        //    }
        //}
    }
    public void setSkill()
    {
        onSkill = null;
        boxnumber2 = 0;
        inputnumber2 = 0;
        for (int i = 0; GManager.instance.Pstatus[GManager.instance.playerselect].inputskill.Length > i;)
        {
            for (int l = 0; l == 0 && GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[i] != -1 && GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[i]].notrg == false;)
            {
                boxnumber2 += 1;
                l++;
            }
            i += 1;
        }
        onSkill = new int[boxnumber2];
        for (int i = 0; GManager.instance.Pstatus[GManager.instance.playerselect].inputskill.Length > i;)
        {
            for (int l = 0; l == 0 && GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[i] != -1 && GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[i]].notrg == false;)
            {
                onSkill[inputnumber2] = i;
                inputnumber2 += 1;
                l++;
            }
            i += 1;
        }
        //取得スキル調べ(ダブルジャンプの場合)
        rabbitTrg = false;//これが最終的にtrueになると取得してる。
        if (onSkill != null )
        {
            GManager.instance.Pstatus[GManager.instance.playerselect].selectskill = 0;
            for (int i = 0; i < onSkill.Length;)
            {
                if (GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[i] == 4)//所持スキル内にダブルジャンプを確認。
                {
                    rabbitTrg = true;
                    break;
                }
                i++;
            }
        }
        else
        {
            GManager.instance.Pstatus[GManager.instance.playerselect].selectskill = -1;
        }
    }
    public void setItem()
    {
        onItem = null;
        boxnumber3 = 0;
        inputnumber3 = 0;
        for (int i = 0; GManager.instance.Quick_itemSet.Length > i;)
        {
            for (int l = 0; l == 0 && GManager.instance.Quick_itemSet[i] != -1 && GManager.instance.ItemID[GManager.instance.Quick_itemSet[i]].itemnumber > 0;)
            {
                boxnumber3 += 1;
                l++;
            }
            i += 1;
        }
        onItem = new int[boxnumber3];
        for (int i = 0; GManager.instance.Quick_itemSet.Length > i;)
        {
            for (int l = 0; l == 0 && GManager.instance.Quick_itemSet[i] != -1 && GManager.instance.ItemID[GManager.instance.Quick_itemSet[i]].itemnumber > 0;)
            {
                onItem[inputnumber3] = i;
                inputnumber3 += 1;
                l++;
            }
            i += 1;
        }
        if (onItem!= null && onItem.Length != 0)
        {
            GManager.instance._quickSelect = 0;
        }
        else
        {
            GManager.instance._quickSelect = -1;
        }
    }
}