using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
public class boss_sk : MonoBehaviour
{
    public objAngle oa;
    public int maxrandom = 4;
    public int minrandom = 1;
    bool resettrg = false;
    [Header("1は動き始める時間、4はイベントリセット")]public float[] time;
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
                minrandom = 1;
                //会話イベント
                StartCoroutine(Talk());
                
            }
            if (resettrg == true && eventnumber != -1 && ontrg < 999)
            {
                resettrg = false;
                ontrg = 0;
                trg[1] = false;
                objE.audioS.Stop();
                if(oa.enabled == false)
                {
                    oa.enabled = true;
                }
                eventnumber = Random.Range(minrandom, maxrandom);
                //同じ技防止
                for (int i = oldevent; i == eventnumber;)
                {
                    eventnumber = Random.Range(minrandom, maxrandom);
                }
                oldevent = eventnumber;

            }
            if (GManager.instance.over == false && GManager.instance.walktrg == true && objE.deathtrg == false && trg[1] == false && eventnumber != -1 && ontrg < 999)
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
                        Invoke("Event" + eventnumber, 0f);
                    }
                }
            }
        }
    }

    void Event1()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            oa.enabled = false;
            vec[0] = p.transform.position;
            vec[0].y = 0.5f;
            time[6] = Vector3.Distance(this.transform.position, vec[0]);
            objE.Eanim.SetInteger("Anumber", 1);
            Invoke("Ev1_0", 1.2f);
        }
        else if (ontrg == 2)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, vec[0], time[6] / 30f);
        }
    }
    void Ev1_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            obj[0].SetActive(true);
            objE.audioS.PlayOneShot(Ase[0]);
            Invoke("Ev1_1", 1.2f);
        }
    }
    void Ev1_1()
    {
        ontrg = 3;
        obj[0].SetActive(false);
        oa.enabled = true;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 2f;
    }
    void Event2()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 2);
            Invoke("Ev2_0", 0.4f);
        }
    }
    void Ev2_0()
    {
        if(ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(obj[1], this.transform.position, this.transform.rotation, this.transform);
            if(summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if(addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = -2;
                    addsummon.nokill = true;
                }
            }
            Invoke("Ev2_1", 1.2f);
        }
    }
    void Ev2_1()
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
            oa.enabled = false;
            vec[0] = p.transform.position;
            vec[0].y = 0.5f;
            time[6] = Vector3.Distance(this.transform.position, vec[0]);
            objE.Eanim.SetInteger("Anumber", 3);
            Ev3_0();
        }
        else if (ontrg == 2)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, vec[0], time[6] / 20f);
        }
    }
    void Ev3_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            obj[2].SetActive(true);
            objE.audioS.PlayOneShot(Ase[1]);
            Invoke("Ev3_1", 0.4f);
        }
    }
    void Ev3_1()
    {
        if (ontrg == 2)
        {
            ontrg = 3;
            objE.audioS.PlayOneShot(Ase[2]);
            obj[2].SetActive(false);
            oa.enabled = true;
            rb.velocity = Vector3.zero;
            Invoke("Ev3_2", 1f);
        }
    }
    void Ev3_2()
    {
        ontrg = 4;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.45f;
    }

    void Event4()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 4);
            Invoke("Ev4_0", 0.4f);
        }
    }
    void Ev4_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(obj[3], this.transform.position, this.transform.rotation, this.transform);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = -2;
                    addsummon.nokill = true;
                }
            }
            Invoke("Ev4_1", 1.3f);
        }
    }
    void Ev4_1()
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
            objE.Eanim.SetInteger("Anumber", 0);
            resettrg = true;
        }
    }
    
    public IEnumerator Talk()
    {
        objE.damageOn = false;
        obj[2].SetActive(false);
        obj[0].SetActive(false);
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
        resettrg = true;
    }
}
