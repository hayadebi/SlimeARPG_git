using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class girlPlayer : MonoBehaviour
{
    public Transform cmPos;
    public AudioSource walkAudio = null;
    public GameObject body;
    public ColEvent groundCol;
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
    public GameObject overUI;
    public GameObject menuUI;
    public AudioClip jumpse;
    AudioSource audioSource;
    public Animator anim;
    Rigidbody rb;
    private int damage = 1;
    //状態異常関連の変数
    [Header("追加エフェクト")]
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
    // Start is called before the first frame update
    public Camera mainc;
    public Transform summonP;
    public int[] onMagic;
    public int inputnumber = 0;
    public int boxnumber = 0;
    public enemyS ES;
    private int oldplayer = 0;
    private GameObject plobj;
    private GameObject dsobj;
    private bool restarttrg = false;
    private Vector3 rotvec;
    [Header("サブゲーム用の仮ステータス")]
    public float loadtime = 0;
    public float maxload = 0;
    public float moveSpeed = 20;
    public int MaxHP = 20;
    public int HP = 20;
    public int MaxMP = 100;
    public int MP = 0;
    public int usegage = -1;
    public GameObject slimehead;
    public Transform headPos;
    public GameObject slimebody;
    public Transform ShotP;
    public Sprite slsp;
    [System.Serializable]
    public struct motionM
    {
        public int activeTrg;
        public int animInt;
        public int startseTime;
        public AudioClip se;
        public int seMnumber;
        public float startatTime;
        public GameObject atobj;
        public float addload;
        public int removemp;
        public int removeusegage;
        public Sprite atsp;
    }
    public motionM[] motionManager;
    public int selectMotion = 0;
    public int charachange = 0;
    private Vector3 movec;
    public bool eatTrg = false;
    //private bool rap_trg = false;
    void Start()
    {
        //GManager.instance.Triggers[23] = 0;
        audioSource = this.GetComponent<AudioSource>();
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        anim.SetInteger(Anumbername, 0);
    }
    //private float unloadtime = 0;
    // Update is called once per frame
    public void motionAT()
    {
        anim.SetInteger(Anumbername, motionManager[selectMotion].animInt);
        Invoke("ATTIME", motionManager[selectMotion].startatTime);
    }
    public void ATTIME()
    {
        if (motionManager[selectMotion].startseTime != 0)
        {
            if (motionManager[selectMotion].se != null)
            {
                audioSource.PlayOneShot(motionManager[selectMotion].se);
            }
            if (motionManager[selectMotion].seMnumber != -1)
            {
                GManager.instance.setrg = motionManager[selectMotion].seMnumber;
            }
        }
        if (motionManager[selectMotion].atobj != null)
        {
            var mousep = ShotP.position + (ShotP.right * 10);
            mousep = mousep.normalized;
            GManager.instance.mouseP = mousep; 
            Instantiate(motionManager[selectMotion].atobj, this.transform.position, ShotP.rotation, this.transform);
        }
        if(MP <= 0 && GManager.instance.subcharaTrg != 0)
        {
            GManager.instance.subcharaTrg = 0;
        }
    }
    void FixedUpdate()
    {
        if (!GManager.instance.over && GManager.instance.walktrg == true && GManager.instance.setmenu == 0)
        {
            //回復
            if(GManager.instance.hitcure )
            {
                GManager.instance.hitcure = false;
                if(HP < MaxHP)
                {
                    HP += 1;
                }
            }
            ////スライム切り替え
            if (charachange != GManager.instance.subcharaTrg)
            {
                
                var scef = Instantiate(GManager.instance.effectobj[13], this.transform.position, this.transform.rotation);
                var scalef = scef.gameObject.transform.localScale;
                scalef /= 2;
                scef.gameObject.transform.localScale = scalef;
                foreach (Transform child in headPos)
                {
                    Destroy(child.gameObject);
                }
                if (GManager.instance.subcharaTrg == 0)
                {
                    slimehead.SetActive(true);
                }
                else
                {
                    slimehead.SetActive(false);
                }
                if (!GManager.instance.charaM[GManager.instance.subcharaTrg].mainpartsTrg && GManager.instance.charaM[GManager.instance.subcharaTrg].partsobj != null )
                {
                    plobj = Instantiate(GManager.instance.charaM[GManager.instance.subcharaTrg].partsobj,headPos.position ,headPos.rotation ,headPos);
                }
                if(GManager.instance.charaM[GManager.instance.subcharaTrg].slImage != null)
                {
                    slsp = GManager.instance.charaM[GManager.instance.subcharaTrg].slImage;
                }
                for(int i = 0; i < GManager.instance.charaM[GManager.instance.subcharaTrg].subM.Length; )
                {
                    motionManager[i].activeTrg = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].activeTrg;
                    motionManager[i].animInt = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].animInt;
                    motionManager[i].startseTime = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].startseTime;
                    motionManager[i].se = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].se;
                    motionManager[i].seMnumber = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].seMnumber;
                    motionManager[i].startatTime = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].startatTime;
                    motionManager[i].atobj = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].atobj;
                    motionManager[i].addload = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].addload;
                    motionManager[i].removemp = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].removemp;
                    if(GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].activeTrg > 0 && GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].atImage != null)
                    {
                        motionManager[i].atsp = GManager.instance.charaM[GManager.instance.subcharaTrg].subM[i].atImage;
                    }
                    i++;
                }
                if(GManager.instance.charaM[GManager.instance.subcharaTrg].mainpartsTrg)
                {
                    GManager.instance.charaM[GManager.instance.subcharaTrg].useTrg = true;
                    
                    HP = GManager.instance.charaM[GManager.instance.subcharaTrg].HP;
                }
                moveSpeed = GManager.instance.charaM[GManager.instance.subcharaTrg].speed;
                MaxHP = GManager.instance.charaM[GManager.instance.subcharaTrg].MaxHP;
                MaxMP = GManager.instance.charaM[GManager.instance.subcharaTrg].MaxMP;
                MP = GManager.instance.charaM[GManager.instance.subcharaTrg].MP;
                maxjump = GManager.instance.charaM[GManager.instance.subcharaTrg].jump;
                jumpspeed = GManager.instance.charaM[GManager.instance.subcharaTrg].jump;
                charachange = GManager.instance.subcharaTrg;
            }
            //瀕死ですよ
            //if (GManager.instance.Pstatus[GManager.instance.playerselect].hp <= (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 4) && rap_trg == false)
            //{
            //    rap_trg = true;
            //    GManager.instance.setrg = 26;
            //    if (GManager.instance.isEnglish == 0)
            //    {
            //        GManager.instance.txtget = "瀕死になりそうです！回復しましょう";
            //    }
            //    else if (GManager.instance.isEnglish == 1)
            //    {
            //        GManager.instance.txtget = "I'm dying! Let's recover!";
            //    }
            //}
            //else if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 4) && rap_trg == true)
            //{
            //    rap_trg = false;
            //}
            //ダッシュモード
            if (loadtime == 0 && dashtrg == false)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    {
                        loadtime = 1f;
                        maxload = 1f;
                        dashtrg = true;
                        dasheffect.SetActive(true);
                        GManager.instance.setrg = 0;
                        anim.SetInteger(Anumbername, 2);
                    }
                }
            }
            else if (dashtrg == true && Input.GetKeyUp(KeyCode.LeftShift) && GManager.instance.autolongdash == 0)
            {
                dashtime = 0;
                dashtrg = false;
                dasheffect.SetActive(false);
            }
            else if (dashtrg == true)
            {
                dashtime += Time.deltaTime;
                if (dashtime > 0.45f)
                {
                    dashtime = 0;
                    dashtrg = false;
                    dasheffect.SetActive(false);
                }
            }
            //技、ダッシュ使用制限タイム
            if (loadtime != 0)
            {
                loadtime -= Time.deltaTime;
                if (loadtime < 0.1f || loadtime == 0)
                {
                    if (anim.GetInteger(Anumbername) != 0)
                    {
                        anim.SetInteger(Anumbername, 0);
                    }
                    eatTrg = false;
                    loadtime = 0;
                }
            }
            //移動してない時の重力操作
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && groundCol.ColTrigger == true && loadtime == 0 && damagetrg == false)
            {
                ySpeed -= gravity;
                if (anim.GetInteger(Anumbername) != 0)
                {
                    anim.SetInteger(Anumbername, 0);
                }
            }
            if (groundCol.ColTrigger == false && jumpmode == 0)
            {
                ySpeed -= gravity;
            }
            else if (groundCol.ColTrigger == true && jumpmode == 0 && ySpeed != 0)
            {
                oldjumpP = this.transform.position;
                inputjtime = 0;
                jumpmode = 0;
                ySpeed = 0;
            }
            ////異常状態
            if (mudtime >= 0)
            {
                mudtime -= Time.deltaTime;
            }
            if (watertime >= 0)
            {
                watertime -= Time.deltaTime;
            }
            if (icetime >= 0)
            {
                icetime -= Time.deltaTime;
            }
            else if (stoptrg == true)
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
                        damage = 1 + GManager.instance.Pstatus[GManager.instance.playerselect].defense;
                        OnDamage();
                    }
                }
            }
            //-------------
        }
        //メニュー画面出現
        //if (GManager.instance.setmenu < 1 && GManager.instance.walktrg == true && Input.GetKeyDown(KeyCode.Escape) && !stoptrg)
        //{
        //    GameObject m = GameObject.Find("menu(Clone)");
        //    GManager.instance.ESCtrg = false;
        //    GManager.instance.walktrg = true;
        //    anim.SetInteger(Anumbername, 0);
        //    ySpeed = 0;
        //    if (m == null)
        //    {
        //        GManager.instance.setmenu += 1;
        //        GManager.instance.walktrg = false;
        //        GManager.instance.setrg = 6;
        //        Instantiate(menuUI, transform.position, transform.rotation);
        //    }
        //}
        //--------------------------------------

        if (GManager.instance.walktrg == true && GManager.instance.over == false && stoptrg == false)
        {
            if (rb.useGravity)
            {
                rb.useGravity = false;
            }
            //攻撃
            if (GManager.instance.setmenu == 0 && loadtime == 0)
            {
                //強攻撃使用
                if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift) && motionManager[1].removemp <= MP && motionManager[1].activeTrg != 0)
                {
                    selectMotion = 1;
                    if (usegage != -1)
                    {
                        usegage -= motionManager[selectMotion].removeusegage;
                    }
                    loadtime = motionManager[selectMotion].addload;
                    maxload = motionManager[selectMotion].addload;
                    MP -= motionManager[selectMotion].removemp;
                    anim.SetInteger(Anumbername, 0);
                    if (motionManager[selectMotion].startseTime == 0)
                    {
                        if (motionManager[selectMotion].se != null)
                        {
                            audioSource.PlayOneShot(motionManager[selectMotion].se);
                        }
                        if (motionManager[selectMotion].seMnumber != -1)
                        {
                            GManager.instance.setrg = motionManager[selectMotion].seMnumber;
                        }
                    }
                    Invoke("motionAT", 0.1f);
                }
                //必殺魔法使用
                else if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftShift) && motionManager[3].removemp <= MP && motionManager[3].activeTrg != 0)
                {
                    selectMotion = 3;
                    loadtime = motionManager[selectMotion].addload;
                    maxload = motionManager[selectMotion].addload;
                    MP -= motionManager[selectMotion].removemp;
                    anim.SetInteger(Anumbername, 0);
                    if (motionManager[selectMotion].startseTime == 0)
                    {
                        if (motionManager[selectMotion].se != null)
                        {
                            audioSource.PlayOneShot(motionManager[selectMotion].se);
                        }
                        if (motionManager[selectMotion].seMnumber != -1)
                        {
                            GManager.instance.setrg = motionManager[selectMotion].seMnumber;
                        }
                    }
                    Invoke("motionAT", 0.1f);
                }
                //通常攻撃使用
               else if (Input.GetMouseButton(0) && motionManager[0].removemp <= MP && motionManager[0].activeTrg != 0)
                {
                    selectMotion = 0;
                    if(charachange == 0)
                    {
                        eatTrg = true;
                    }
                    loadtime = motionManager[selectMotion].addload;
                    maxload = motionManager[selectMotion].addload;
                    MP -= motionManager[selectMotion].removemp;
                    if(GManager.instance.subcharaTrg != 0 && GManager.instance.charaM[GManager.instance.subcharaTrg].mainpartsTrg == false)
                    {
                        MP = 0;
                    }
                    anim.SetInteger(Anumbername, 0);
                    if (motionManager[selectMotion].startseTime == 0)
                    {
                        if (motionManager[selectMotion].se != null)
                        {
                            audioSource.PlayOneShot(motionManager[selectMotion].se);
                        }
                        if (motionManager[selectMotion].seMnumber != -1)
                        {
                            GManager.instance.setrg = motionManager[selectMotion].seMnumber;
                        }
                    }
                    Invoke("motionAT", 0.1f);
                }
                //通常魔法使用
               else if (Input.GetMouseButton(1) && motionManager[2].removemp <= MP && motionManager[2].activeTrg != 0)
                {
                    selectMotion = 2;
                    loadtime = motionManager[selectMotion].addload;
                    maxload = motionManager[selectMotion].addload;
                    MP -= motionManager[selectMotion].removemp;
                    anim.SetInteger(Anumbername, 0);
                    if (motionManager[selectMotion].startseTime == 0)
                    {
                        if (motionManager[selectMotion].se != null)
                        {
                            audioSource.PlayOneShot(motionManager[selectMotion].se);
                        }
                        if (motionManager[selectMotion].seMnumber != -1)
                        {
                            GManager.instance.setrg = motionManager[selectMotion].seMnumber;
                        }
                    }
                    Invoke("motionAT", 0.1f);
                }
                
            }
            //落下ゲームオーバー
            if (overhight < 9999)
            {
                if (this.transform.position.y < overhight && !highttrg)
                {
                    highttrg = true;
                    Instantiate(GManager.instance.effectobj[3], transform.position, transform.rotation);
                    GameOver();
                }
            }
            //----
            if (damagetrg == false)
            {
                //----移動----
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                {
                    if (jumpmode == 0 && damagetrg == false && jumpmode == 0 && dashtrg == false && groundCol.ColTrigger == true)
                    {
                        anim.SetInteger(Anumbername, 1);
                    }
                    if (Input.GetKey(KeyCode.D) && body.transform.eulerAngles.y != 0)
                    {
                        rotvec = body.transform.eulerAngles;
                        rotvec.y = 0;
                        body.transform.eulerAngles = rotvec;
                    }
                    else if (Input.GetKey(KeyCode.A) && body.transform.eulerAngles.y != 180)
                    {
                        rotvec = body.transform.eulerAngles;
                        rotvec.y = 180;
                        body.transform.eulerAngles = rotvec;
                    }
                    if (walkAudio.enabled == false && jumpmode == 0 && dashtrg == false && groundCol.ColTrigger == true)
                    {
                        walkAudio.enabled = true;
                        walkAudio.loop = true;
                        walkAudio.Play();
                    }
                }
                else if (walkAudio.enabled == true)
                {
                    anim.SetInteger(Anumbername, 0);
                    walkAudio.enabled = false;
                    walkAudio.loop = false;
                    walkAudio.Stop();
                }
                if(Input.GetKey(KeyCode.W) && jumpmode < 1 && groundCol.ColTrigger == true && loadtime == 0)
                {
                    jumpmode = 1;
                    walkAudio.Stop();
                    walkAudio.loop = false;
                    walkAudio.enabled = false;
                    audioSource.PlayOneShot(jumpse);
                    anim.SetInteger(Anumbername, 2);
                    oldjumpP = this.transform.position;
                    var upp = transform.position;
                    upp.y += gravity;
                    transform.position = upp;
                    groundCol.ColTrigger = false;
                }
                if (jumpmode > 0)
                {
                    inputjtime += Time.deltaTime;
                    if (jumpmode == 1)
                    {
                        if (this.transform.position.y < oldjumpP.y + maxjump)
                        {
                            ySpeed += jumpspeed;
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
                dashspeed = 1;
                if (dashtrg == true)
                {
                    if (watertime > 0)
                    {
                        dashspeed = 1.25f;
                    }
                    else if (mudtime > 0)
                    {
                        dashspeed = 1.25f;
                    }
                    else
                    {
                        dashspeed = 2;
                    }
                }
                xSpeed = 0;
                if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                {
                    xSpeed = 1;
                }
                movec = (((body.transform.right * xSpeed) * dashspeed) * moveSpeed);
                if (loadtime != 0)
                {
                    movec = (((body.transform.right * xSpeed) * dashspeed) * ((moveSpeed / 3)*2));
                }
                else if (watertime > 0)
                {
                    movec = (((body.transform.right * xSpeed) * dashspeed) * (moveSpeed/3));
                }
                else if (mudtime > 0)
                {
                    movec = (((body.transform.right * xSpeed) * dashspeed) * (moveSpeed/2));
                }
                var movevec = movec + (Vector3.up * (ySpeed));
                //------------
                rb.velocity = movevec;
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
    public void OnDamage()
    {
        if (stoptrg == false && !poisontrg && !flametrg && !infinitytrg)
        {
            var rotation = Quaternion.LookRotation(colobj.transform.position - this.transform.position);
            rotation.x = 0;
            rotation.z = 0;
            if(rotation.y > 90)
            {
                rotation.y = 180;
            }
            else
            {
                rotation.y = 0;
            }
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
            else if (darktrg && 1 > damage)
            {
                HP -= 2;
            }
            else if (darktrg && 0 < damage)
            {
                HP -= (damage * 2);
            }
            else if (1 > damage)
            {
                HP -= 1;
            }
            else if (0 < damage)
            {
                HP -= damage;
            }
            if (colobj && colobj.tag == "ebullet")
            {
                Destroy(colobj.gameObject, 0.1f);
            }
        }
        if (HP > 0 && damagetrg == false)
        {
            GManager.instance.setrg = 5;
            anim.SetInteger(Anumbername, 5);
        }
        else if (HP < 1)
        {
            var killef = Instantiate(GManager.instance.effectobj[2], transform.position, transform.rotation);
            var scalef = killef.gameObject.transform.localScale;
            scalef /= 2;
            killef.gameObject.transform.localScale = scalef;
            GameOver();
        }
        if (!poisontrg && !flametrg && !infinitytrg)
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
                GameOver();
            }
            if (collision.tag == "enemy" || collision.tag == "ebullet")
            {
                //追加状態異常
                if (collision.GetComponent<AddEffect>() && collision.GetComponent<AddEffect>().enabled == true && dashtrg == false)//&& GManager.instance.skillselect == -1)
                {
                    AddEffect ef = collision.GetComponent<AddEffect>();
                    if (ef.effectnumber == 0 && mudtime <= 0)//泥
                    {
                        if (GManager.instance.isEnglish == 0)
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
                    else if (ef.effectnumber == 1 && !poisontrg)//毒
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
                    else if (ef.effectnumber == 2)//ノックバック
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
                    //else if (ef.effectnumber == 3 && GManager.instance.skillselect == -1)//反転操作
                    //{
                    //    GManager.instance.skillnumber = 3;
                    //}
                    else if (ef.effectnumber == 3 && !flametrg)//燃焼
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
                    else if (ef.effectnumber == 4 && !infinitytrg)//燃焼
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
                    else if (ef.effectnumber == 5 && watertime <= 0)//水
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
                    else if (ef.effectnumber == 6 && icetime <= 0)//氷
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
                        icetime = 2;
                        stoptrg = true;
                    }
                    else if (ef.effectnumber == 7 && !holytrg)//光
                    {
                        randomEffect = Random.Range(0, 4);
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
                    else if (ef.effectnumber == 8 && !holytrg)//光
                    {
                        EffectObj = Instantiate(GManager.instance.effectobj[7], this.transform.position, this.transform.rotation, this.transform);
                        flametrg = true;
                        flametime = 0;
                        randomEffect = Random.Range(0, 4);
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
                    else if (ef.effectnumber == 9 && !darktrg)//闇
                    {
                        randomEffect = Random.Range(0, 3);
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
                    //else if (ef.effectnumber == 4 && !stoptrg)//氷Lv2
                    //{
                    //    efevNumber = 4;
                    //    frostobj = Instantiate(GManager.instance.skillobj[5], this.transform.position, this.transform.rotation, this.transform);
                    //    Destroy(frostobj.gameObject, 3f);
                    //    stoptrg = true;
                    //}
                    //else if (ef.effectnumber == 7 && GManager.instance.skillselect == -1)//時限爆弾
                    //{
                    //    GManager.instance.skillnumber = 7;
                    //}
                    //else if (ef.effectnumber == 9)//致命傷
                    //{
                    //    GManager.instance.skillnumber = 11;
                    //}
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
                    //if (GManager.instance.skillselect == 11 && GManager.instance.SkillID[GManager.instance.skillselect].inputskillbar < 2)
                    //{
                    //    damage = damage * 3 / 2;
                    //}
                    if (collision.GetComponent<AddDamage>().nokill == true && damage > HP)
                    {
                        damage = 0;
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
        if(cmPos != null)
        {
            cmPos.parent = null;
        }
        if (rb.useGravity)
        {
            rb.useGravity = false;
        }
        GManager.instance.subcharaTrg = 0;
        //for (int i = 0; i < GManager.instance.Pstatus.Length;)
        //{
        //    if (GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp > 0)
        //    {
        //        if (GManager.instance.isEnglish == 0)
        //        {
        //            GManager.instance.txtget = GManager.instance.Pstatus[GManager.instance.playerselect].pname + "がダウンした";
        //        }
        //        else if (GManager.instance.isEnglish == 1)
        //        {
        //            GManager.instance.txtget = GManager.instance.Pstatus[GManager.instance.playerselect].pname2 + " is down";
        //        }
        //        GManager.instance.playerselect = i;
        //        restarttrg = true;
        //        i = GManager.instance.Pstatus.Length;
        //    }
        //    i++;
        //}
        if (restarttrg == false || highttrg == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (GManager.instance.bossbattletrg == 1)
            {
                GManager.instance.bossbattletrg = 0;
            }
            GManager.instance.over = true;
            walkAudio.Stop();
            audioSource.Stop();
            GManager.instance.setrg = 4;
            Instantiate(overUI, transform.position, transform.rotation);
            Destroy(gameObject, 0.1f);
        }
        else
        {
            GManager.instance.setrg = 13;
            restarttrg = false;
        }
    }
    void Damage()
    {
        if (GManager.instance.over == false)
        {
            anim.SetInteger(Anumbername, 0);
            rb.velocity = Vector3.zero;
            damagetrg = false;
        }
    }
}