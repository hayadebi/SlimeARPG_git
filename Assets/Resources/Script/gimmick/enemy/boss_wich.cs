using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Effekseer;

public class boss_wich : MonoBehaviour
{
    public objAngle oa;
    public int maxrandom = 3;
    public int minrandom = 0;
    bool resettrg = false;
    [Header("1は動き始める時間、4はイベントリセット")] public float[] time;
    public int ontrg = 0;
    [Header("0と1はランダム配置の範囲")] public Transform[] Bpos;
    public Vector3[] vec;
    public int eventnumber = 0;
    public AudioClip[] Ase;
    public GameObject[] obj;
    public Renderer ren;
    enemyS objE;
    public bool[] trg;
    int oldevent = 0;
    Rigidbody rb;
    GameObject p;
    int wariaihp = 40;
    bool stoptrg = false;
    private bool isTalking;
    private Flowchart flowChart;
    public string message = "second";
    private GameObject summonobj = null;
    private AddMagic addsummon = null;
    public GameObject[] magicObj;
    public EffekseerEmitter[] eyePos;
    public float[] numbers;
    public GameObject CmObj;
    //private bool cureTrg = false;
    private int stack_event = 0;

    // Start is called before the first frame update
    void Start()
    {
        flowChart = this.GetComponent<Flowchart>();
        rb = this.GetComponent<Rigidbody>();
        p = GameObject.Find("Player");
        objE = this.GetComponent<enemyS>();
        eventnumber = Random.Range(2, maxrandom);
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
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (trg[0] == false && GManager.instance.bossbattletrg == 1)
        {
            time[0] += Time.deltaTime;
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
                eventnumber = -1;
                ontrg = 999;

                if (stoptrg == false)
                {
                    stoptrg = true;
                    rb.velocity = Vector3.zero;
                }
                Instantiate(GManager.instance.effectobj[13], this.transform.position, this.transform.rotation, this.transform);
                GManager.instance.setrg = 20;
                objE.Estatus.defence += 2;
                maxrandom = 8;
                minrandom = 1;
                objE.Eanim.SetInteger("Anumber", 0);
                StartCoroutine(Talk());

            }
            if (resettrg == true && eventnumber != -1 && ontrg < 999)
            {
                resettrg = false;
                ontrg = 0;
                trg[1] = false;
                objE.audioS.Stop();
                rb.velocity = Vector3.zero;
                if (oa.enabled == false)
                {
                    oa.enabled = true;
                }
                //Invoke("Event" + eventnumber, 0f);
                if (objE.mirror_Trg)
                {
                    objE.mirror_Trg = false;
                    objE.Eanim.SetInteger("Anumber", 0);
                    if (stoptrg == false)
                    {
                        stoptrg = true;
                        rb.velocity = Vector3.zero;
                    }
                    eventnumber = 13;
                }
                else if (oldevent != 1)
                {
                    eventnumber = 1;
                    oldevent = 1;
                }
                else
                {
                    if (objE.Estatus.health < (wariaihp/3)*2 && trg[2])
                    {
                        eventnumber = Random.Range(2, 9);
                        stack_event = eventnumber;
                    }
                    else
                    {
                        eventnumber = Random.Range(2, maxrandom);
                        stack_event = eventnumber;
                    }
                    oldevent = stack_event;
                }

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
                        Invoke("Event" + eventnumber, 0.1f);
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
    void Event13()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 5);
            summonobj = Instantiate(GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicobj, this.transform.position,transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.changeType = "闇属性";
                    addsummon.Damage = (objE.Estatus.attack / 2);
                }
            }
            Invoke("Ev13_end", 2f);
        }
    }
    void Ev13_end()
    {
        ontrg = 2;
        oa.enabled = true;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 0.3f;
    }
    void Event1()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            oa.enabled = false;
            objE.Eanim.SetInteger("Anumber", 1);
            Invoke("Ev1_1", 0.1f);
        }
    }
    void Ev1_1()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            objE.audioS.PlayOneShot(Ase[0]);
            Instantiate(GManager.instance.effectobj[4], this.transform.position, this.transform.rotation);
            vec[0] = Bpos[5].position;
            vec[0].y += 0.5f;
            if(vec[0].x > Bpos[1].position.x)
            {
                vec[0].x = Bpos[1].position.x;
            }
            else if (vec[0].x < Bpos[0].position.x)
            {
                vec[0].x = Bpos[0].position.x;
            }
            if (vec[0].z > Bpos[1].position.z)
            {
                vec[0].z = Bpos[1].position.z;
            }
            else if (vec[0].z < Bpos[0].position.z)
            {
                vec[0].z = Bpos[0].position.z;
            }
            this.transform.position = vec[0];
            Instantiate(GManager.instance.effectobj[4], this.transform.position, this.transform.rotation);
            Invoke("Ev1_2", 1.3f);
        }
    }
    void Ev1_2()
    {
        ontrg = 3;
        oa.enabled = true;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.45f;
    }

    void Event2()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            numbers[2] = 0;
            objE.audioS.PlayOneShot(Ase[1]);
            Instantiate(GManager.instance.effectobj[13], this.transform.position, this.transform.rotation, this.transform);
            objE.Eanim.SetInteger("Anumber", 3);
            Invoke("Ev2_1", 1f);
        }
    }
    void Ev2_1()
    {
        if (ontrg == 1 )
        {
            ontrg = 2;
            vec[0] = p.transform.position;
            summonobj = Instantiate(magicObj[0], vec[0], this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack / 2;
                    addsummon.nokill = false;
                }
            }
            for (int i = 0; i < 4;)
            {
                numbers[0] = Random.Range(Bpos[0].position.x, Bpos[1].position.x);
                numbers[1] = Random.Range(Bpos[0].position.z, Bpos[1].position.z);
                vec[0] = this.transform.position;
                vec[0].x = numbers[0];
                vec[0].z = numbers[1];
                summonobj = Instantiate(magicObj[0], vec[0], this.transform.rotation);
                if (summonobj != null)
                {
                    addsummon = summonobj.GetComponent<AddMagic>();
                    if (addsummon != null)
                    {
                        addsummon.enemytrg = true;
                        addsummon.Damage = objE.Estatus.attack / 2;
                        addsummon.nokill = false;
                    }
                }
                i++;
            }
            Invoke("Ev2_2", 2f);
        }
    }
    void Ev2_2()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.3f;
    }

    void Event3()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 3);
            Ev3_0();
        }
    }
    void Ev3_0()
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
                    addsummon.Damage = 0;
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev3_1", 3f);
        }
    }
    void Ev3_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.3f;
    }

    void Event4()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 4);
            Invoke("Ev4_0", 0.3f);
        }
    }
    void Ev4_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            oa.enabled = false;
            summonobj = Instantiate(magicObj[2], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = 0;
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev4_1", 3f);
        }
    }
    void Ev4_1()
    {
        ontrg = 3;
        oa.enabled = true;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.3f;
    }

    void Event5()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 2);
            objE.audioS.PlayOneShot(Ase[2]);
            Invoke("Ev5_0", 0.3f);
        }
    }
    void Ev5_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[3], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack / 3;
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev5_1", 1.3f);
        }
    }
    void Ev5_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.3f;
    }
    void Event6()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 3);
            Invoke("Ev6_0", 0.3f);
        }
    }
    void Ev6_0()
    {
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
                    addsummon.Damage = 0;
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev6_1", 3f);
        }
    }
    void Ev6_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.45f;
    }
    void Event7()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 4);
            Ev7_0();
        }
    }
    void Ev7_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[5], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = 0;
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev7_1", 3f);
        }
    }
    void Ev7_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.45f;
    }
    void Event8()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            //cureTrg = true;
            objE.audioS.PlayOneShot(Ase[4]);
            objE.Eanim.SetInteger("Anumber", 4);
            Invoke("Ev8_0", 0.3f);
        }
    }
    void Ev8_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[6], this.transform.position, this.transform.rotation, this.transform);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.inputEs = objE;
                }
            }
            Invoke("Ev8_1", 2f);
        }
    }
    void Ev8_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.2f;
    }
    void Eventreset()
    {
        if (trg[1] == false && eventnumber != -1 && ontrg < 999)
        {
            //ateffect.SetActive(false);
            objE.Eanim.SetInteger("Anumber", 0);
            resettrg = true;
        }
    }
    public IEnumerator Talk()
    {
        objE.damageOn = false;
        if (isTalking)
        {
            yield break;
        }
        isTalking = true;
        flowChart.SendFungusMessage(message);
        yield return new WaitUntil(() => flowChart.GetExecutingBlocks().Count == 0);
        isTalking = false;
        //第二形態になったら切り替えるやつ
        objE.damageOn = true;
        ontrg = 99;
        objE.Eanim.SetInteger("Anumber", 1);
        objE.audioS.PlayOneShot(Ase[3]);
        Invoke("NextBoss", 2f);
    }
    void NextBoss()
    {
        //第二形態になったら切り替えるやつ
        eventnumber = 9;
        resettrg = true;
    }
}
