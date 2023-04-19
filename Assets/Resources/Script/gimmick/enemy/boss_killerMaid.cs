using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Effekseer;

public class boss_killerMaid : MonoBehaviour
{
    public objAngle oa;
    public ColEvent cl;
    public int maxrandom = 5;
    public int minrandom = 1;
    bool resettrg = false;
    [Header("1は動き始める時間、4はイベントリセット")] public float[] time;
    public int ontrg = 0;
    public Transform[] Bpos;
    public Vector3[] vec;
    public int eventnumber = 0;
    public AudioClip[] Ase;
    public Renderer[] ren;
    public Material[] mat;
    public GameObject[] objs;
    enemyS objE;
    public bool[] trg;
    int oldevent = 0;
    Rigidbody rb;
    GameObject p;
    player ps;
    int wariaihp = 40;
    bool stoptrg = false;
    private bool isTalking;
    private Flowchart flowChart;
    public string message = "second";
    private GameObject summonobj = null;
    private AddMagic addsummon = null;
    private bool attrg = false;
    public GameObject[] magicObj;
    public GameObject ateffect;
    public bool slashtrg = false;
    public int secondMode = 0;
    private float secondTime = 0;
    public Sprite _setimage;
    private int cure_maxhp;

    // Start is called before the first frame update
    void Start()
    {
        flowChart = this.GetComponent<Flowchart>();
        rb = this.GetComponent<Rigidbody>();
        p = GameObject.Find("Player");
        ps = p.GetComponent<player>();
        objE = this.GetComponent<enemyS>();
        eventnumber = Random.Range(minrandom, maxrandom);
        oldevent = eventnumber;
        if (GManager.instance.mode == 0)
        {
            wariaihp = objE.Estatus.health / 3 * 2 / 2;
        }
        else if (GManager.instance.mode == 1)
        {
            wariaihp = objE.Estatus.health / 3 * 3 / 2;
        }
        else if (GManager.instance.mode == 2)
        {
            wariaihp = objE.Estatus.health / 3 * 4 / 2;
        }
        cure_maxhp = objE.Estatus.health;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(secondMode > 0 )
        {
            secondTime += Time.deltaTime;
            if(secondMode == 1 && secondTime >= 1f)
            {
                secondTime = 0;
                secondMode = 2;
                objE.Eanim.SetInteger("Anumber", 1);
            }
            if(secondMode == 2 && secondTime >= 2f)
            {
                objE.Estatus.health = cure_maxhp;
                secondTime = 0;
                secondMode = 0;
                objE.damageOn = true;
                eventnumber = 4;
                ontrg = 99;
                attrg = false;
                resettrg = true;
            }
        }
        if (trg[0] == false && GManager.instance.bossbattletrg == 1)
        {
            time[0] += Time.deltaTime;
            ps.lostevent = true;
            ps.overUI = objs[7];
            if (time[0] > time[1])
            {
                trg[0] = true;
                time[0] = 0;
            }
        }
        else if (trg[0] == true)
        {
            if (wariaihp > objE.Estatus.health && trg[1] == false && trg[2] == false)
            {
                trg[1] = true;
                trg[2] = true;
                slashtrg = false;
                ateffect.SetActive(false);
                cl.ColTrigger = false;
                oa.enabled = true;
                oa.RotSpeed = 3;
                eventnumber = -1;
                ontrg = 999;
                objE.Eanim.SetInteger("Anumber", 0);
                objE.Estatus.speed = 30;
                if (stoptrg == false)
                {
                    stoptrg = true;
                    rb.velocity = Vector3.zero;
                }
                Instantiate(GManager.instance.effectobj[19], this.transform.position, this.transform.rotation, this.transform);
                GManager.instance.setrg = 20;
                objE.Estatus.defence += 8;
                maxrandom = 10;
                minrandom = 4;
                attrg = true;
                //会話イベント
                StartCoroutine(Talk());

            }
            if (resettrg == true && eventnumber != -1 && ontrg < 999)
            {
                resettrg = false;
                ontrg = 0;
                trg[1] = false;
                attrg = false;
                cl.ColTrigger = false;
                slashtrg = false;
                ateffect.SetActive(false);
                oa.enabled = true;
                oa.RotSpeed = 3;
                objE.audioS.Stop();
                if (oa.enabled == false)
                {
                    oa.enabled = true;
                }
                objs[6].SetActive(true);
            }
            if (GManager.instance.over == false && GManager.instance.walktrg == true && objE.deathtrg == false && trg[1] == false && eventnumber != -1 && ontrg < 999 && objE.DsMove == false)
            {
                if (time[4] > 0)
                {
                    time[4] -= Time.deltaTime;
                    if (time[4] < 0 || time[4] == 0)
                    {
                        time[4] = 0;
                        Eventreset();
                    }
                }
                if (GManager.instance.bossbattletrg == 1)
                {
                    if (p != null && eventnumber != -1)
                    {
                        //Invoke("Event" + eventnumber, 0f);
                        moveBoss();
                    }
                }
            }
            else if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
        }
    }
    void moveBoss()
    {
        vec[0] = this.transform.forward * objE.Estatus.speed;
        if (eventnumber == 6 && ontrg == 2)
        {
            vec[0] = this.transform.forward * 40;
            rb.velocity = vec[0];
            if (stoptrg != false)
            {
                stoptrg = false;
            }
        }
        if (cl.ColTrigger == false && attrg == false && ontrg != 2)
        {
            rb.velocity = vec[0];
            objE.Eanim.SetInteger("Anumber", 5);
            if (stoptrg != false)
            {
                stoptrg = false;
            }
        }
        else if (cl.ColTrigger == true && attrg == false)
        {
            attrg = true;
            objE.Eanim.SetInteger("Anumber", 0);
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            eventnumber = Random.Range(minrandom, maxrandom);// maxrandom);
            //同じ技防止
            for (int i = oldevent; i == eventnumber;)
            {
                eventnumber = Random.Range(minrandom, maxrandom);
            }
            oldevent = eventnumber;

            Invoke("Event" + eventnumber, 0.1f);
        }
        else if (ontrg == 2 && slashtrg == true && eventnumber == 4)
        {
            Event4();
        }
    }
    void Event1()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 1);
            Invoke("Ev1_0", 0.3f);
        }
    }
    void Ev1_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[0], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack / 3;
                }
            }
            Invoke("Ev1_2", 1.3f);
        }
    }
    void Ev1_2()
    {
        ontrg = 4;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.2f;
    }

    void Event2()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 2);
            Invoke("Ev2_0", 0.35f);
        }
    }
    void Ev2_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[1], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev2_1", 1.3f);
        }
    }
    void Ev2_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.45f;
    }

    void Event3()
    {
        if (ontrg == 0)
        {
            oa.RotSpeed = 1;
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 4);
            Ev3_0();
        }
    }
    void Ev3_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[2], this.transform.position, this.transform.rotation, this.transform);
            Invoke("Ev3_1", 3f);
        }
    }
    void Ev3_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.45f;
    }
    void Event4()
    {
        if (ontrg == 0)
        {
            ontrg = 2;
            slashtrg = true;
            ateffect.SetActive(true);
            objE.audioS.PlayOneShot(Ase[0]);
            vec[1] = p.transform.position;
            vec[1].y = this.transform.position.y;
            time[6] = Vector3.Distance(this.transform.position, vec[1]);
            objE.Eanim.SetInteger("Anumber", 3);
            Invoke("Ev4_1", 1.05f);
        }
        else if (ontrg == 2)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, vec[1], time[6] / 55f);
        }
    }
    void Ev4_1()
    {
        if (ontrg == 2)
        {
            ontrg = 3;
            slashtrg = false;
            ateffect.SetActive(false);
            summonobj = Instantiate(magicObj[3], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack / 4;
                }
            }
            Invoke("Ev4_3", 1.2f);
        }
    }
    void Ev4_3()
    {
        ontrg = 4;
        objE.Eanim.SetInteger("Anumber", 0);
        rb.velocity = Vector3.zero;
        time[4] = 1.3f;
    }
    void Event5()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 1);
            Invoke("Ev5_0", 0.3f);
        }
    }
    void Ev5_0()
    {
        objs[6].SetActive(false);
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[4], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack / 4;
                }
            }
            Invoke("Ev5_2", 1.3f);
        }
    }
    void Ev5_2()
    {
        ontrg = 4;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.2f;
    }

    void Event6()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 6);
            Invoke("Ev6_0", 0.1f);
        }
    }
    void Ev6_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[5], this.transform.position, this.transform.rotation,this.transform);
            Invoke("Ev6_2", 2.3f);
        }
    }
    void Ev6_2()
    {
        ontrg = 4;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.2f;
    }
    void Event7()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 2);
            Invoke("Ev7_0", 0.35f);
        }
    }
    void Ev7_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[1], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev7_1", 1.3f);
        }
    }
    void Ev7_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.3f;
    }

    void Event8()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 7);
            Ev8_0();
        }
    }
    void Ev8_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[6], this.transform.position, this.transform.rotation, this.transform);
            Invoke("Ev8_2", 4f);
        }
    }
    void Ev8_2()
    {
        ontrg = 4;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.2f;
    }
    void Event9()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 8);
            Ev9_0();
        }
    }
    void Ev9_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[7], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev9_1", 10f);
        }
    }
    void Ev9_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.3f;
    }
    void Eventreset()
    {
        if (trg[1] == false && eventnumber != -1 && ontrg < 999)
        {
            ateffect.SetActive(false);
            objE.Eanim.SetInteger("Anumber", 0);
            resettrg = true;
        }
    }

    public IEnumerator Talk()
    {
        objE.damageOn = false;
        ateffect.SetActive(false);
        if (isTalking)
        {
            yield break;
        }
        isTalking = true;
        flowChart.SendFungusMessage(message);
        yield return new WaitUntil(() => flowChart.GetExecutingBlocks().Count == 0);
        isTalking = false;
        //第二形態になったら切り替えるやつ
        objE.greetboss_resistance = true;
        ren[0].material = mat[0];
        ren[4].material = mat[0];
        ren[1].material = mat[1];

        objs[0].SetActive(true);
        objs[1].SetActive(true);
        objs[2].SetActive(true);
        objs[5].SetActive(true);

        ren[2].material = mat[2];
        ren[3].material = mat[2];
        GManager.instance.enemynoteID[30].inputattacktype = "死神属性";
        GManager.instance.enemynoteID[30].inputattacktype2 = "God attribute";
        GManager.instance.enemynoteID[30].script = "魔王様の力を一時的に借りて疑似魔王化した姿。\nどんな者でも切り刻める斬撃と、\n絶対に焼き尽くす炎で対象を確実に仕留める。";
        GManager.instance.enemynoteID[30].script2 = "A figure that has temporarily borrowed the power of the Demon King\n and become a pseudo-Demon King. \nHe can slash at any person,\n and his flames burn down the target with absolute certainty.";
        objE.Estatus.attacktype = "神属性";
        GManager.instance.enemynoteID[30].image = _setimage;
        objE.Eanim.SetInteger("Anumber", 4);
        secondMode = 1;
    }
}
