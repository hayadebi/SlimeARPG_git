using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class enemyS : MonoBehaviour
{
    public int DsAnim = -1;
    public int Ds_slimeGet = -1;
    public bool Ds_bgmStop = false; 
    [Header("いじるな、死んでアニメ状態の時のTrigger")]
    public bool DsMove = false;
    [Header("第二形態があるかどうか")] public bool secondMode = false;
    [Header("別のスクリプトから効果付与")] public int neweffect = 0;
    [Header("ダメージを受ける状態か")]public bool damageOn = true;
    public bool knockbacktrg = true;
    public float knockspeed = 12.8f;
    public int movetrgnumber = -1;
    public int addEvent = 0;
    [Header("まだイベントが無い")]public int KillEvent = -1;
    public AudioClip bgm;
    public Renderer  ren;
    public int Limited = -1;
    public int maxgetexp = 20;
    [System.Serializable]
    public struct boss
    {
        public string message;
        public bool bosstrg;
        public string canvasname;
        //public int oldhp;
        public int eventnumber;
        public GameObject[] renobj;
        public GameObject BTend_objOn;
        public GameObject BADend_objOn;
        public bool endtrg;
        public int _addbad;
        
    }
    public boss BossTrigger;
    public int _AddEnd = 0;
    public int maxendDay = -1;
    Flowchart flowChart;
    bool isTalking = false;
    //--------
    public string enemyName;
    public string enemyName2;
    public GameObject partobj;
    public int getCoin;
    public int[] dropItemID;
    public int[] droprare;
    public GameObject dropmonster = null;
    public GameObject dropmonster_bad = null;
    bool Dtrg = false;
    public bool isDamage = false;
    public bool easyDestroy = false;
    public bool damagetrg = false;
    public bool deathtrg = false;
    [System.Serializable]
    public struct status
    {
        public float speed;
        public int maxhp;
        public int health;
        public int attack;
        public int defence;
        public string badtype;
        public int Lv;
        public int enemyID;
        public string attacktype;
        [Header("空腹スライムガールのやつ")]
        public int subgameCharaID;
    }
    public status Estatus;
    //スポーン関係
    public int destroyID = -1;
    public float minsunTime = -1;
    public float maxsunTime = -1;
    public int randomMob = -1;
    private int randomM = 0;
    public int eventSpID = -1;
    public int eventSpNumber = 0;
    public int minlv = -1;
    public int maxlv = -1;
    //------------
    public int curemp = 1;
    public int getAchievementsID = -1;
    public Animator Eanim;
    public AudioClip deathse;
    public  AudioClip damagese;
    public AudioSource audioS;
    private int inputattack;
    [Header("触る必要はない")]public int inputdamage =1;
    //---------------------------
    //異常状態の変数
    //private float frosttime = 0;
    [Header("状態異常で動き止める変数※いじらないで")] public bool stoptrg = false;
    [Header("ダメージを受けたよというトリガー")] public bool hitdamagetrg = false;
    [Header("強制キルトリガー")] public bool inputkilltrg = false;
    [Header("完全停止")] public bool absoluteStop = false;
    public GameObject bosskilloffobj = null;
    GameObject P;
    player ps;
    Rigidbody rb;
    [Header("追加エフェクト")]
    private int randomEffect = 0;
    private bool darktrg = false;
    private bool holytrg = false;
    private bool flametrg = false;
    private float flametime = 0;
    private bool infinitytrg = false;
    private float infinitytime = 0;
    private float damagetime = 0;
    public float mudtime;
    public float watertime;
    public float icetime;
    private float oldspeed;
    public bool poisontrg;
    public float poisontime;
    private float poisondamage;
    private CameraController ccm;
    private GameObject mainc;
    public float minus_DesHight = 20;
    [Header("耐性")]
    public bool weakness_resistance = false;
    public bool poison_resistance = false;
    public bool mirror_resistance = false;
    public bool greetboss_resistance = false;
    public bool mirror_Trg = false;
    public int random_mirror = 1;
    [Header("空腹スライム用限定破壊トリガー")]
    public int inputEvent = -1;
    [Header("地面かどうか")]
    public bool noground = false;
    public LayerMask obstacleLayer = 12;
    public RaycastHit hit;
    private GameObject damageobj = null;
    private Text damagetext = null;
    public int[] tips_trg;
    private int x_getnum = 1;
    private int l_getnum = 1;
    // Start is called before the first frame update
    void Start()
    {
        mainc = GameObject.Find("MainC");
        ccm = mainc.GetComponent<CameraController>();
        rb = this.GetComponent<Rigidbody>();
        if (BossTrigger.bosstrg == true)
        {
            flowChart = this.GetComponent<Flowchart>();
        }
        if (GManager.instance.ItemID[59].itemnumber > 0)
        {
            x_getnum = 3;
            l_getnum = 2;
        }
        if (GManager.instance.mode == 0)
        {
            Estatus.attack = ((Estatus.attack + 3) / 3) * 2;
            Estatus.health = ((Estatus.health + 3) / 3) * 2;
            Estatus.maxhp = Estatus.health;
            if (easyDestroy == true)
            {
                Destroy(gameObject);
            }
        }
        else if (GManager.instance.mode == 2)
        {
            Estatus.attack = ((Estatus.attack + 4) / 4) * 5;
            Estatus.health = ((Estatus.health + 4) / 4) * 5;
            Estatus.maxhp = Estatus.health;
            maxgetexp += 20;
        }
        if(minlv != -1 && maxlv != -1)
        {
            Estatus.Lv = Random.Range(minlv, maxlv);
        }
        if((Estatus.Lv - GManager.instance.Pstatus[GManager.instance.playerselect].Lv) > 10 )
        {
            Estatus.Lv = GManager.instance.Pstatus[GManager.instance.playerselect].Lv + 10;
        }
        if(Estatus.Lv != 0)
        {
            Estatus.health += (1 * Estatus.Lv);
            Estatus.maxhp = Estatus.health;
            Estatus.attack += ((1 * Estatus.Lv)/2);
            Estatus.defence += ((1 * Estatus.Lv)/2);
        }
        oldspeed = Estatus.speed;
        knockspeed *= 2;
        P = GameObject.Find("Player");
        ps = P.GetComponent<player>();
        if (Limited != -1)
        {
            if(GManager.instance.Triggers[Limited] > 0)
            {
                if (BossTrigger.BTend_objOn != null)
                {
                    BossTrigger.BTend_objOn.SetActive(true);
                }
                if (BossTrigger.BADend_objOn != null)
                {
                    BossTrigger.BADend_objOn.SetActive(true);
                }
                Destroy(gameObject);
            }
        }
        if(destroyID != -1 && GManager.instance.mobDsTrg[destroyID] == 1)
        {
            this.gameObject.SetActive(false);
        }
        else if(eventSpID != -1 && GManager.instance.EventNumber[eventSpID] != eventSpNumber)
        {
            this.gameObject.SetActive(false);
        }
        else if (minsunTime != -1 &&minsunTime >= GManager.instance.sunTime)
        {
            this.gameObject.SetActive(false);
        }
        else if (maxsunTime != -1 && maxsunTime <= GManager.instance.sunTime)
        {
            this.gameObject.SetActive(false);
        }
        else if (randomMob != -1)
        {
            randomM = Random.Range(0, randomMob);
            if (randomM != 0)
            {
                if(destroyID != -1)
                {
                    GManager.instance.mobDsTrg[destroyID] = 1;
                }
                this.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.position.y < -minus_DesHight  || this.transform.position.y > 2000 || this.transform.position.x < -4000 || this.transform.position.x > 4000 || this.transform.position.z < -4000 || this.transform.position.z > 4000)
        {
            if (destroyID != -1)
            {
                GManager.instance.mobDsTrg[destroyID] = 1;
            }
            killSet();
        }
        else if (absoluteStop == false)
        {
            if (partobj != null && partobj.GetComponent<enemyS>().Estatus.health < 1)
            {
                Animator partanim = partobj.GetComponent<Animator>();
                if (partanim.enabled == true)
                {
                    partanim.enabled = false;
                }
                partobj = null;
                Rigidbody rb = this.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.constraints = RigidbodyConstraints.None;
            }
            if (stoptrg && this.GetComponent<Rigidbody>().velocity != Vector3.zero)//止まってる時に動きも停止
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (GManager.instance.over == false)
            {
                if (inputkilltrg == true)
                {
                    inputkilltrg = false;
                    killSet();
                }

                //if (neweffect == 1 && !stoptrg)//氷Lv5
                //{
                //    neweffect = 0;
                //    efevNumber = 5;
                //    frostobj = Instantiate(GManager.instance.skillobj[5], this.transform.position, this.transform.rotation, this.transform);
                //    Destroy(frostobj.gameObject, 10f);
                //    stoptrg = true;
                //}
                ////異常状態
                if (watertime >= 0)
                {
                    watertime -= Time.deltaTime;
                }
                else if(mudtime >= 0)
                {
                    mudtime -= Time.deltaTime;
                }
                else if(oldspeed != Estatus.speed )
                {
                    Estatus.speed = oldspeed;
                }
                if(icetime >= 0)
                {
                    icetime -= Time.deltaTime;
                }
                else if(stoptrg == true)
                {
                    stoptrg = false;
                }
                //if (stoptrg && efevNumber == 3)//氷
                //{
                //    frosttime += Time.deltaTime;
                //    if (frosttime > 2f)
                //    {
                //        frosttime = 0;
                //        efevNumber = -1;
                //        stoptrg = false;
                //    }
                //}
                //else if (stoptrg && efevNumber == 4)
                //{
                //    frosttime += Time.deltaTime;
                //    if (frosttime > 4f)
                //    {
                //        frosttime = 0;
                //        efevNumber = -1;
                //        stoptrg = false;
                //    }
                //}
                //else if (stoptrg && efevNumber == 5)
                //{
                //    frosttime += Time.deltaTime;
                //    if (frosttime > 8f)
                //    {
                //        frosttime = 0;
                //        efevNumber = -1;
                //        stoptrg = false;
                //    }
                //}
                if (poisontrg && !greetboss_resistance)//毒
                {
                    poisontime += Time.deltaTime;
                    poisondamage += Time.deltaTime;
                    if (poisontime > 30)
                    {
                        poisontime = 0;
                        poisondamage = 0;
                        poisontrg = false;
                    }
                    else if (poisondamage >= 2)
                    {
                        poisondamage = 0;
                        if (Estatus.health > 1)
                        {
                            inputdamage = 1+Estatus.defence;
                            isDamage = true;
                        }
                    }
                }
                if (flametrg && !greetboss_resistance)//炎
                {
                    flametime += Time.deltaTime;
                    damagetime += Time.deltaTime;
                    if (flametime > 15)
                    {
                        flametime = 0;
                        damagetime = 0;
                        flametrg = false;
                    }
                    else if (damagetime >= 2)
                    {
                        damagetime = 0;
                        inputdamage = 1 + Estatus.defence;
                        isDamage = true;
                    }
                }
                if (infinitytrg)//燃焼
                {
                    infinitytime += Time.deltaTime;
                    if (infinitytime > 2)
                    {
                        infinitytime = 0;
                        inputdamage = 1 + GManager.instance.Pstatus[GManager.instance.playerselect].defense;
                        isDamage = true;
                    }
                }
                //-------------------
                if (isDamage == true && deathtrg == false)
                {
                    isDamage = false;
                    if (partobj == null)
                    {
                        if (GManager.instance.walktrg == true && deathtrg == false && damagetrg == false)
                        {
                            if (Dtrg == false && inputdamage != 0)
                            {
                                Dtrg = true;
                                inputattack = Estatus.attack;
                                Estatus.attack = 0;
                                if (ren == null || ren.isVisible || secondMode)
                                {
                                    if (darktrg && 1 > (inputdamage - (Estatus.defence/2)))
                                    {
                                        Estatus.health -= 1;
                                        //---------------------
                                        damageobj = Instantiate(GManager.instance.effectobj[22], transform.position, transform.rotation, transform);
                                        damagetext = damageobj.transform.GetChild(0).gameObject.GetComponent<Text>();
                                        if(damageobj != null && damagetext != null)
                                        {
                                            damagetext.text = 1 + " damage!";
                                        }
                                        //---------------------
                                    }
                                    else if (darktrg && 0 < (inputdamage - (Estatus.defence/2)))
                                    {
                                        Estatus.health -= (inputdamage - (Estatus.defence/2));
                                        //---------------------
                                        damageobj = Instantiate(GManager.instance.effectobj[22], transform.position, transform.rotation, transform);
                                        damagetext = damageobj.transform.GetChild(0).gameObject.GetComponent<Text>();
                                        if (damageobj != null && damagetext != null)
                                        {
                                            damagetext.text = (inputdamage - (Estatus.defence / 2)) + " damage!";
                                        }
                                        //---------------------
                                    }
                                    else if (1 > (inputdamage - Estatus.defence))
                                    {
                                        Estatus.health -= 1;
                                        //---------------------
                                        damageobj = Instantiate(GManager.instance.effectobj[22], transform.position, transform.rotation, transform);
                                        damagetext = damageobj.transform.GetChild(0).gameObject.GetComponent<Text>();
                                        if (damageobj != null && damagetext != null)
                                        {
                                            damagetext.text = 1 + " damage!";
                                        }
                                        //---------------------
                                    }
                                    else if (0 < (inputdamage - Estatus.defence))
                                    {
                                        Estatus.health -= (inputdamage - Estatus.defence);
                                        //---------------------
                                        damageobj = Instantiate(GManager.instance.effectobj[22], transform.position, transform.rotation, transform);
                                        damagetext = damageobj.transform.GetChild(0).gameObject.GetComponent<Text>();
                                        if (damageobj != null && damagetext != null)
                                        {
                                            damagetext.text = (inputdamage - Estatus.defence) + " damage!";
                                        }
                                        //---------------------
                                    }
                                    GManager.instance.Pstatus[GManager.instance.playerselect].mp += curemp;
                                    if(GManager.instance.playerselect == 5)
                                    {
                                        GManager.instance.Pstatus[GManager.instance.playerselect].mp += curemp;
                                    }
                                    if (GManager.instance.Pstatus[GManager.instance.playerselect].maxMP < GManager.instance.Pstatus[GManager.instance.playerselect].mp)
                                    {
                                        GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                                    }
                                }
                                else if (!ren.isVisible)
                                {
                                    Estatus.health -= 0;
                                }
                            }
                            if(GManager.instance.enemynoteID[Estatus.enemyID].gettrg < 1)
                            {
                                GManager.instance.enemynoteID[Estatus.enemyID].gettrg = 1;
                            }
                            if (Estatus.health > 0)
                            {
                                if (knockbacktrg == true )
                                {
                                    if (!poisontrg && !flametrg && !infinitytrg)
                                    {
                                        Vector3 velocity = -this.transform.forward * knockspeed;
                                        //風力を与える
                                        rb.AddForce(velocity, ForceMode.VelocityChange);
                                    }
                                }
                                if (inputEvent == -1)
                                {
                                    audioS.PlayOneShot(damagese);
                                }
                            }
                            else if (Estatus.health < 1)
                            {
                                killSet();
                            }
                            damagetrg = true;
                            Invoke("Damage", 0.2f);
                        }
                    }
                    else if (partobj != null)
                    {
                        enemyS partS = partobj.GetComponent<enemyS>();
                        partS.inputdamage = inputdamage;
                        partS.isDamage = true;
                        Invoke("Damage", 0.2f);
                    }
                }
            }
        }
    }
    public void killSet()
    {
        deathtrg = true;
        DsMove = true;
        this.gameObject.tag = "OnMask";
        if(maxendDay != -1 && maxendDay >= GManager.instance.daycount)
        {
            GManager.instance.EventNumber[10] += 1;
        }
        else if (maxendDay != -1 && maxendDay < GManager.instance.daycount)
        {
            GManager.instance.EventNumber[11] -= 1;
        }
        if (destroyID != -1)
        {
            GManager.instance.mobDsTrg[destroyID] = 1;
        }
        if (KillEvent != -1)
        {
            Invoke("Kill_" + KillEvent, 0);
        }
        if (bgm != null)
        {
            GameObject BGM = GameObject.Find("BGM");
            AudioSource bgmA = BGM.GetComponent<AudioSource>();
            bgmA.Stop();
            if (BossTrigger.endtrg == false && !Ds_bgmStop)
            {
                bgmA.clip = bgm;
                bgmA.Play();
            }
        }
        if (Limited != -1)
        {
            GManager.instance.Triggers[Limited] = 1;
        }
        var killef = Instantiate(GManager.instance.effectobj[2], this.transform.position, this.transform.rotation);
        if (GManager.instance.subgameTrg == true)
        {
            var scaleef = killef.gameObject.transform.localScale;
            scaleef /= 2;
            killef.gameObject.transform.localScale = scaleef;
        }
        if (tips_trg != null )
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
        if (dropItemID != null)
        {
            bool skilltrg = false;
            if (P != null && ps != null && ps.onSkill != null)
            {
                for (int i = 0; i < ps.onSkill.Length;)
                {
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[i]] == 2)
                    {
                        skilltrg = true;
                        i = ps.onSkill.Length;
                    }
                    i++;
                }
            }
            if(dropmonster != null)
            {
                if (maxendDay == -1 || maxendDay >= GManager.instance.daycount)
                {
                    Instantiate(dropmonster, this.transform.position, this.transform.rotation);
                }
                else if (maxendDay != -1 && maxendDay < GManager.instance.daycount)
                {
                    Instantiate(dropmonster_bad, this.transform.position, this.transform.rotation);
                }
            }
            GManager.instance.Coin += ((getCoin*x_getnum)/l_getnum);
            if(getCoin > 0)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = (getCoin * x_getnum) + "コインを手に入れた！";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "I got " + (getCoin * x_getnum) + " coins!";
                }
            }
            for (int i = 0; i < dropItemID.Length;)
            {
                if(skilltrg == true)
                {
                    droprare[i] -= 1;
                    if(droprare[i] < 1)
                    {
                        droprare[i] = 1;
                    }
                }
                int rareOn = Random.Range(0, (droprare[i]));
                if(rareOn == 0)
                {
                    GManager.instance.ItemID[dropItemID[i]].itemnumber += (1*l_getnum);
                    GManager.instance.ItemID[dropItemID[i]].gettrg = 1;
                    if (GManager.instance.isEnglish == 0)
                    {
                        GManager.instance.txtget = GManager.instance.ItemID[dropItemID[i]].itemname + "がドロップした！";
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        GManager.instance.txtget = "The " +GManager.instance.ItemID[dropItemID[i]].itemname2+ " dropped!";
                    }
                }
                i++;
            }
        }
        if (getAchievementsID != -1 && GManager.instance.achievementsID[getAchievementsID].gettrg == 0)
        {
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.txtget = GManager.instance.achievementsID[getAchievementsID].name + "の実績を解除した！";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                GManager.instance.txtget = "I released the " + GManager.instance.achievementsID[getAchievementsID].name2 + " achievement！";
            }
            GManager.instance.setrg = 1;
            GManager.instance.achievementsID[getAchievementsID].gettrg = 1;
        }
        audioS.Stop();
        GManager.instance.ase = deathse;
        if(P!=null && ps != null && ps.onSkill != null)
        {
            for (int i = 0; i < ps.onSkill.Length;)
            {
                if (GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[i]] == 0)
                {
                    maxgetexp += 20;
                    i = ps.onSkill.Length;
                }
                i++;
            }
        }
        for (int i = 0; i < GManager.instance.Pstatus.Length;)
        {
            if (GManager.instance.Pstatus[i].getpl == 1)
            {
                GManager.instance.Pstatus[i].inputExp += (((maxgetexp + (Estatus.Lv * 3))*x_getnum)/l_getnum);
            }
            i++;
        }
       
        if (BossTrigger.BTend_objOn != null && maxendDay == -1)
        {
            BossTrigger.BTend_objOn.SetActive(true);
        }
        else if (BossTrigger.BTend_objOn != null && maxendDay != -1 && maxendDay >= GManager.instance.daycount)
        {
            BossTrigger.BTend_objOn.SetActive(true);
        }
        if (BossTrigger.BTend_objOn != null && maxendDay != -1 && maxendDay < GManager.instance.daycount)
            {
            BossTrigger.BADend_objOn.SetActive(true);
        }
        if (BossTrigger.bosstrg == false)
        {
            if (DsAnim == -1)
            {
                Destroy(gameObject);
            }
            else if(DsAnim != -1)
            {
                Eanim.SetInteger("Anumber", DsAnim);
            }
        }
        else if (BossTrigger.bosstrg == true)
        {
            if(BossTrigger.renobj != null && DsAnim == -1)
            {
                for(int i = 0;i < BossTrigger.renobj.Length;)
                {
                    BossTrigger.renobj[i].SetActive (false);
                    i++;
                }
            }
            if(bosskilloffobj != null)
            {
                this.transform.position = bosskilloffobj.transform.position;
                bosskilloffobj.SetActive(false);
            }
            if(_AddEnd > 0)
            {
                GManager.instance.EventNumber[10] += _AddEnd;
            }
            else if (_AddEnd < 0)
            {
                GManager.instance.EventNumber[11] += (_AddEnd * -1);
            }
            StartCoroutine(Talk());
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (damageOn == true && secondMode == false && absoluteStop == false)
        {
            if (GManager.instance.subcharaTrg == 0 && inputEvent == -1)
            {
                if (collision.tag == "kill" && BossTrigger.bosstrg == false)
                {
                    if (Estatus.subgameCharaID != 0)
                    {
                        GManager.instance.subcharaTrg = Estatus.subgameCharaID;
                    }
                    GManager.instance.setrg = 31;
                    inputkilltrg = true;
                }
                else if (collision.tag == "kill" && BossTrigger.bosstrg == true)
                {
                    inputdamage = 1;
                    GManager.instance.setrg = 31;
                    isDamage = true;
                }
            }
            if(collision.tag == "red")
            {
                if (destroyID != -1)
                {
                    GManager.instance.mobDsTrg[destroyID] = 1;
                }
                killSet();
            }
            if (collision.tag == "pbullet" || collision.tag == "Player")
            {
                    hitdamagetrg = true;
                GManager.instance.hitcure = true;
                //状態異常
                if (collision.GetComponent<AddEffect>() && inputEvent == -1 && weakness_resistance == false)
                {
                    AddEffect ef = collision.GetComponent<AddEffect>();
                    //    if (collision.GetComponent<AddDamage>() && collision.GetComponent<AddDamage>().nokill == true && Estatus.health + 1 < collision.GetComponent<AddDamage>().Damage - Estatus.defence)
                    //    {
                    //        ;
                    //    }
                    //    else
                    //    {
                    if (ef.effectnumber == 0 && mudtime <= 0)//泥
                    {
                        Instantiate(GManager.instance.effectobj[5], this.transform.position, this.transform.rotation, this.transform);
                        mudtime = 3;
                        Estatus.speed = (Estatus.speed / 3) * 2;
                    }
                    if (ef.effectnumber == 1 && !poisontrg && !poison_resistance && !greetboss_resistance)//毒
                    {
                        Instantiate(GManager.instance.effectobj[6], this.transform.position, this.transform.rotation, this.transform);
                        poisontrg = true;
                        poisontime = 0;
                    }
                    if(ef.effectnumber == 2 )//ノックバック
                    {
                        Vector3 velocity = -this.transform.forward * 32f;
                        //風力を与える
                        rb.AddForce(velocity, ForceMode.VelocityChange);
                    }
                    if (ef.effectnumber == 3 && !flametrg && !greetboss_resistance)//燃焼
                    {
                        Instantiate(GManager.instance.effectobj[7], this.transform.position, this.transform.rotation, this.transform);
                        flametrg = true;
                        flametime = 0;
                    }
                    if (ef.effectnumber == 4 && !infinitytrg)//燃焼
                    {
                        Instantiate(GManager.instance.effectobj[8], this.transform.position, this.transform.rotation, this.transform);
                        infinitytrg = true;
                        infinitytime = 0;
                    }
                    if (ef.effectnumber == 5 && watertime <= 0)//水
                    {
                        Instantiate(GManager.instance.effectobj[9], this.transform.position, this.transform.rotation, this.transform);
                        watertime = 5;
                        Estatus.speed = (Estatus.speed / 2);
                    }
                    if (ef.effectnumber == 6 && icetime <= 0 && !greetboss_resistance)//水
                    {
                        Instantiate(GManager.instance.effectobj[10], this.transform.position, this.transform.rotation, this.transform);
                        icetime = 1;
                        stoptrg = true;
                    }
                    if ((ef.effectnumber == 7 || ef.effectnumber == 10 && !greetboss_resistance) && !holytrg)//光
                    {
                        randomEffect = Random.Range(0, 5);
                        if (randomEffect == 0)
                        {
                            holytrg = true;
                            Instantiate(GManager.instance.effectobj[11], this.transform.position, this.transform.rotation, this.transform);
                            Estatus.health /= 2;
                        }
                    }
                    if (ef.effectnumber == 8 && !holytrg)//光
                    {
                        Instantiate(GManager.instance.effectobj[7], this.transform.position, this.transform.rotation, this.transform);
                        flametrg = true;
                        flametime = 0;
                        randomEffect = Random.Range(0, 5);
                        if (randomEffect == 0)
                        {
                            holytrg = true;
                            Instantiate(GManager.instance.effectobj[11], this.transform.position, this.transform.rotation, this.transform);
                            Estatus.health /= 2;
                        }
                    }
                    if ((ef.effectnumber == 9 || ef.effectnumber == 10)&& !darktrg && !greetboss_resistance)//闇
                    {
                        randomEffect = Random.Range(0, 4);
                        if (randomEffect == 0)
                        {
                            darktrg = true;
                            Instantiate(GManager.instance.effectobj[11], this.transform.position, this.transform.rotation, this.transform);

                        }
                    }
                }
                //--------
                if(inputEvent != -1 && collision.GetComponent<AddDamage>() && collision.GetComponent<AddDamage>().outputEvent == inputEvent)
                {
                    inputkilltrg = true;
                }
                else if (inputEvent > 0  && GManager.instance.playerselect == inputEvent)
                {
                    inputkilltrg = true;
                    killSet();
                }
                else if (collision.GetComponent<AddDamage>() && collision.tag != "Player")
                {
                    inputdamage = collision.GetComponent<AddDamage>().Damage + ((GManager.instance.Pstatus[GManager.instance.playerselect].attack / 3) * 2);
                    if(collision.GetComponent<AddDamage>().attacktype == GManager.instance.Pstatus[GManager.instance.playerselect].attacktype)
                    {
                        inputdamage = (inputdamage * 9) / 8;
                    }
                    if ((collision.GetComponent<AddDamage>().attacktype == Estatus.attacktype && collision.GetComponent<AddDamage>().attacktype != "無属性") || (collision.GetComponent<AddDamage>().attacktype == GManager.instance.Pstatus[GManager.instance.playerselect].badtype && collision.GetComponent<AddDamage>().attacktype != "無属性"))
                    {
                        inputdamage = (inputdamage * 4) / 3;
                    }
                    if (("神属性" == collision.GetComponent<AddDamage>().attacktype || Estatus.badtype == collision.GetComponent<AddDamage>().attacktype) && GManager.instance.subgameTrg == false)
                    {
                        inputdamage = (inputdamage * 3) / 2;

                    }
                    if (weakness_resistance && Estatus.badtype != collision.GetComponent<AddDamage>().attacktype && GManager.instance.subgameTrg == false)
                    {
                        inputdamage = 0;
                    }
                    if(mirror_resistance )
                    {
                        if (Random.Range(0, random_mirror) == 0)
                        {
                            inputdamage = 0;
                            mirror_Trg = true;
                        }
                    }
                }
                else if (collision.GetComponent<AddDamage>() && collision.tag == "Player")
                {
                    inputdamage = collision.GetComponent<AddDamage>().Damage + GManager.instance.Pstatus[GManager.instance.playerselect].attack;
                    if (collision.GetComponent<AddDamage>().attacktype == GManager.instance.Pstatus[GManager.instance.playerselect].attacktype)
                    {
                        inputdamage = (inputdamage * 9) / 8;
                    }
                    if ((collision.GetComponent<AddDamage>().attacktype == Estatus.attacktype && collision.GetComponent<AddDamage>().attacktype != "無属性") || (collision.GetComponent<AddDamage>().attacktype == GManager.instance.Pstatus[GManager.instance.playerselect].badtype && collision.GetComponent<AddDamage>().attacktype != "無属性"))
                    {
                        inputdamage = (inputdamage * 4) / 3;
                    }
                    if (("神属性" == collision.GetComponent<AddDamage>().attacktype || Estatus.badtype == collision.GetComponent<AddDamage>().attacktype) && GManager.instance.subgameTrg == false)
                    {
                        inputdamage = (inputdamage * 3) / 2;
                    }
                    if (weakness_resistance && Estatus.badtype != collision.GetComponent<AddDamage>().attacktype && GManager.instance.subgameTrg == false)
                    {
                        inputdamage = 0;
                    }
                    if (mirror_resistance)
                    {
                        if (Random.Range(0, random_mirror) == 0)
                        {
                            inputdamage = 0;
                            mirror_Trg = true;
                        }
                    }
                }
                else
                {
                    inputdamage = GManager.instance.Pstatus[GManager.instance.playerselect].attack;
                    if (("神属性" == GManager.instance.Pstatus[GManager.instance.playerselect].attacktype || Estatus.badtype == GManager.instance.Pstatus[GManager.instance.playerselect].attacktype) && GManager.instance.subgameTrg == false)
                    {
                        inputdamage = (inputdamage * 3) / 2;
                    }
                    if (weakness_resistance && Estatus.badtype != GManager.instance.Pstatus[GManager.instance.playerselect].attacktype && GManager.instance.subgameTrg == false)
                    {
                        inputdamage = 0;
                    }
                }
                if (P != null && ps != null && ps.onSkill != null)
                {
                    for (int i = 0; i < ps.onSkill.Length;)
                    {
                        if (GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[i]] == 1 && GManager.instance.Pstatus[GManager.instance.playerselect].hp < (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 3))
                        {
                            inputdamage = (inputdamage * 3) / 2;
                            i = ps.onSkill.Length;
                        }
                        i++;
                    }
                }
                if (collision.GetComponent<AddDamage>() && collision.GetComponent<AddDamage>().nokill == true && Estatus.health + 1 < collision.GetComponent<AddDamage>().Damage - Estatus.defence)
                {
                    ;
                }
                else
                {
                    isDamage = true;
                }
                if (inputEvent == -1)
                {
                    Instantiate(GManager.instance.effectobj[1], collision.transform.position, collision.transform.rotation);
                }
                if (collision.tag == "pbullet")
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "ground" && noground == true)
        {
            noground = false;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "ground" && noground == false)
        {
            noground = true;
        }
    }
    void Damage()
    {
        Estatus.attack = inputattack;
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        Dtrg = false;
        damagetrg = false;
    }

    IEnumerator Talk()
    {
        GManager.instance.walktrg = false;
        GManager.instance.bossbattletrg = 0;
        if(maxendDay != -1 && maxendDay >= GManager.instance.daycount)
        {
            BossTrigger.message = "happy";
        }
        else if (maxendDay != -1 && maxendDay < GManager.instance.daycount)
        {
            BossTrigger.message = "bad";
        }
        if (BossTrigger.canvasname != "")
        {
            GameObject canvas = GameObject.Find(BossTrigger.canvasname + "(Clone)");
            if (canvas != null)
            {
                Destroy(canvas.gameObject, 0);
            }
        }
        if (DsAnim != -1)
        {
            Eanim.SetInteger("Anumber", DsAnim);
        }
        if (isTalking)
        {
            yield break;
        }
        isTalking = true;
        flowChart.SendFungusMessage(BossTrigger.message);
        yield return new WaitUntil(() => flowChart.GetExecutingBlocks().Count == 0);
        isTalking = false;
        GManager.instance.walktrg = true;
        if(Ds_slimeGet != -1)
        {
            GManager.instance.Pstatus[Ds_slimeGet].getpl = 1;
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.txtget = GManager.instance.Pstatus[Ds_slimeGet].pname + "が仲間になった！";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                GManager.instance.txtget = GManager.instance.Pstatus[Ds_slimeGet].pname2 + " is now a friend!";
            }
        }
        if(ccm != null && GManager.instance.houseTrg < 1)
        {
            ccm.zoomtrg = true;
        }
        if(addEvent != 0)
        {
            GManager.instance.EventNumber[BossTrigger.eventnumber] += addEvent;
        }
        if(BossTrigger._addbad > 0)
        {
            GManager.instance.EventNumber[10] += BossTrigger._addbad;
        }
        else if (BossTrigger._addbad < 0)
        {
            GManager.instance.EventNumber[11] -= BossTrigger._addbad;
        }
        //GameObject[] skills = GameObject.FindGameObjectsWithTag("skill");
        //if (skills.Length != 0 && skills != null)
        //{
        //    foreach (GameObject skill in skills)
        //    {
        //        if (skill.GetComponent<skillGet>())
        //        {
        //            skill.GetComponent<skillGet>().hole = true;
        //        }
        //    }
        //}
        if(movetrgnumber != -1)
        {
            GManager.instance.Triggers[movetrgnumber] = 1;
        }
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Destroy");
        if (cubes != null && cubes.Length != 0)
        {
            foreach (GameObject cube in cubes)
            {
                // 消す！
                Destroy(cube);
            }
        }
        //if (BossTrigger.BTend_objOn != null)
        //{
        //    BossTrigger.BTend_objOn.SetActive(true);
        //}
        //if (BossTrigger.BADend_objOn != null)
        //{
        //    BossTrigger.BADend_objOn.SetActive(true);
        //}
        if (BossTrigger.endtrg == false)
        {
            Destroy(gameObject, 0.1f);
        }
        
        //else if(BossTrigger.endtrg == true)
        //{
        //    SceneManager.LoadScene("end");
        //}
    }

    void Kill_1()
    {
        
    }

}
