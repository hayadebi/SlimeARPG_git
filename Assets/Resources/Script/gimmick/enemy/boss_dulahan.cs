using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Effekseer;

public class boss_dulahan : MonoBehaviour
{
    public objAngle oa;
    public ColEvent cl;
    public int maxrandom = 3;
    public int minrandom = 0;
    bool resettrg = false;
    [Header("1は動き始める時間、4はイベントリセット")] public float[] time;
    public int ontrg = 0;
    public Transform[] Bpos;
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
    private bool attrg = false;
    public GameObject[] magicObj;
    private bool slashtrg = false;
    public GameObject ateffect;
    public EffekseerEmitter eyePos;

    // Start is called before the first frame update
    void Start()
    {
        flowChart = this.GetComponent<Flowchart>();
        rb = this.GetComponent<Rigidbody>();
        p = GameObject.Find("Player");
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
                objE.Eanim.SetInteger("Anumber", 0);
                if (stoptrg == false)
                {
                    stoptrg = true;
                    rb.velocity = Vector3.zero;
                }
                Instantiate(GManager.instance.effectobj[13], this.transform.position, this.transform.rotation, this.transform);
                GManager.instance.setrg = 20;
                objE.Estatus.defence += 2;
                maxrandom = 5;
                minrandom = 0;
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
                objE.audioS.Stop();
                if (oa.enabled == false)
                {
                    oa.enabled = true;
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
        if (cl.ColTrigger == false && attrg == false)
        {
            rb.velocity = vec[0];
            objE.Eanim.SetInteger("Anumber", 1);
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
        else if (ontrg == 2 && slashtrg == true && eventnumber == 1)
        {
            Event1();
        }
    }
    void Event0()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 4);
            Invoke("Ev0_0", 0.15f);
        }
    }
    void Ev0_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[0], this.transform.position, this.transform.rotation, this.transform);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack / 4;
                }
            }
            Invoke("Ev0_2", 1f);
        }
    }
    void Ev0_2()
    {
        ontrg = 4;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.45f;
    }
    void Event1()
    {
        if (ontrg == 0)
        {
            ontrg = 2;
            slashtrg = true;
            ateffect.SetActive(true);
            objE.audioS.PlayOneShot(Ase[0]);
            oa.enabled = false;
            vec[1] = p.transform.position;
            vec[1].y = this.transform.position.y;
            time[6] = Vector3.Distance(this.transform.position, vec[1]);
            objE.Eanim.SetInteger("Anumber", 3);
            Invoke("Ev1_1", 1.3f);
        }
        else if (ontrg == 2)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, vec[1], time[6] / 50f);
        }
    }
    void Ev1_1()
    {
        if (ontrg == 2)
        {
            ontrg = 3;
            slashtrg = false;
            ateffect.SetActive(false);
            oa.enabled = true;
            objE.Eanim.SetInteger("Anumber", 4);
            Invoke("Ev1_2", 0.15f);
        }
    }
    void Ev1_2()
    {
        if (ontrg == 3)
        {
            ontrg = 4;
            summonobj = Instantiate(magicObj[1], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack / 4;
                }
            }
            Invoke("Ev1_3", 1f);
        }
    }
    void Ev1_3()
    {
        ontrg = 5;
        oa.enabled = true;
        ateffect.SetActive(false);
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 2f;
    }
    void Event2()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            slashtrg = false;
            objE.audioS.PlayOneShot(Ase[1]);
            objE.Eanim.SetInteger("Anumber", 5);
            Invoke("Ev2_0", 1.3f);
        }
    }
    void Ev2_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[2], this.transform.position, this.transform.rotation, this.transform);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 4);
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev2_1", 2f);
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
            ontrg = 1;
            slashtrg = false;
            objE.Eanim.SetInteger("Anumber", 2);
            GManager.instance.setrg = 28;
            Invoke("Ev3_0", 0.3f);
        }
    }
    void Ev3_0()
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
                    addsummon.Damage = -10;
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
        time[4] = 2f;
    }

    void Event4()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            GManager.instance.setrg = 28;
            slashtrg = false;
            objE.Eanim.SetInteger("Anumber", 2);
            Invoke("Ev4_1", 0.3f);
        }
    }
    void Ev4_1()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            eyePos.gameObject.SetActive(true);
            eyePos.Play();
            objE.audioS.PlayOneShot(Ase[2]);
            vec[3] = p.transform.position;
            Invoke("Ev4_2", 1f);
        }
    }
    void Ev4_2()
    {
        if (ontrg == 2)
        {
            ontrg = 3;
            summonobj = Instantiate(magicObj[4], vec[3], this.transform.rotation);
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
            Invoke("Ev4_3", 2f);
        }
    }
    void Ev4_3()
    {
        ontrg = 4;
        eyePos.Stop();
        eyePos.gameObject.SetActive(false);
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
        objE.damageOn = true;
        eventnumber = 4;
        ontrg = 99;
        attrg = false;
        resettrg = true;
    }
}
