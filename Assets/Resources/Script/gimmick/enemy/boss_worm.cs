using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Effekseer;

public class boss_worm : MonoBehaviour
{
    public objAngle oa;
    public int maxrandom = 3;
    public int minrandom = 0;
    bool resettrg = false;
    [Header("1は動き始める時間、4はイベントリセット")] public float[] time;
    public int ontrg = 0;
    [Header("0と1はランダム配置の範囲")]public Transform[] Bpos;
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
                if (oa.enabled == false)
                {
                    oa.enabled = true;
                }
                //Invoke("Event" + eventnumber, 0f);
                eventnumber = Random.Range(minrandom, maxrandom);// maxrandom);
                if(eventnumber == 7 && (wariaihp/3) <= objE.Estatus.health)
                {
                    eventnumber = 6;
                }
                for (int i = oldevent; i == eventnumber;)
                {
                    eventnumber = Random.Range(minrandom, maxrandom);
                    if (eventnumber == 7 && (wariaihp / 3) <= objE.Estatus.health)
                    {
                        eventnumber = 6;
                    }
                }
                oldevent = eventnumber;

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

    void Event1()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            oa.enabled = false;
            objE.Eanim.SetInteger("Anumber", 1);
            Invoke("Ev1_1", 1.15f);
        }
    }
    void Ev1_1()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            objE.audioS.PlayOneShot(Ase[0]);
            Instantiate(magicObj[0], p.transform.position, magicObj[0].transform.rotation);
            for(int i = 0; i<7;)
            {
                numbers[0] = Random.Range(Bpos[0].position.x, Bpos[1].position.x);
                numbers[1] = Random.Range(Bpos[0].position.z, Bpos[1].position.z);
                vec[0] = this.transform.position;
                vec[0].x = numbers[0];
                vec[0].z = numbers[1];
                Instantiate(magicObj[0], vec[0], magicObj[0].transform.rotation);
                i++;
            }
            Invoke("Ev1_2", 3f);
        }
    }
    void Ev1_2()
    {
        ontrg = 3;
        oa.enabled = true;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 2.3f;
    }

    void Event2()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            numbers[2] = 0;
            GManager.instance.setrg =33;
            objE.Eanim.SetInteger("Anumber", 2);
            Invoke("Ev2_1", 0.3f);
        }
    }
    void Ev2_1()
    {
        if (ontrg == 1 || ontrg == 3)
        {
            ontrg = 2;
            for(int i = 0; i<eyePos.Length;)
            {
                eyePos[i].gameObject.SetActive(true);
                eyePos[i].Play();
                i++;
            }
            objE.audioS.PlayOneShot(Ase[1]);
            vec[3] = p.transform.position;
            Invoke("Ev2_2", 0.5f);
        }
    }
    void Ev2_2()
    {
        if (ontrg == 2 || ontrg == 3)
        {
            ontrg = 3;
            summonobj = Instantiate(magicObj[1], vec[3], this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack /3;
                    addsummon.nokill = false;
                }
            }
            numbers[2] += 1;
            if (numbers[2] < 3)
            {
                for (int i = 0; i < eyePos.Length;)
                {
                    eyePos[i].Stop();
                    eyePos[i].gameObject.SetActive(false);
                    i++;
                }
                Invoke("Ev2_1", 0.3f);
            }
            else
            {
                for (int i = 0; i < eyePos.Length;)
                {
                    eyePos[i].Stop();
                    eyePos[i].gameObject.SetActive(false);
                    i++;
                }
                numbers[2] = 0;
                Invoke("Ev2_3", 1f);
            }
        }
    }
    void Ev2_3()
    {
        ontrg = 4;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 2f;
    }

    void Event3()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 3);
            objE.audioS.PlayOneShot(Ase[2]);
            Invoke("Ev3_0", 0.4f);
        }
    }
    void Ev3_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;

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
            Invoke("Ev3_1", 2f);
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
            objE.audioS.PlayOneShot(Ase[3]);
            objE.Eanim.SetInteger("Anumber", 3);
            Invoke("Ev4_0", 0.45f);
        }
    }
    void Ev4_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[3], this.transform.position, this.transform.rotation, this.transform);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 2);
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev4_1", 2f);
        }
    }
    void Ev4_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 2f;
    }

    void Event5()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 4);
            Invoke("Ev5_0", 1f);
        }
    }
    void Ev5_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            vec[0] = this.transform.position;
            vec[0].x = p.transform.position.x;
            vec[0].z = p.transform.position.z;
            this.transform.position = vec[0];
            Invoke("Ev5_1", 3f);
        }
    }
    void Ev5_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1f;
    }
    void Event6()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.audioS.PlayOneShot(Ase[4]);
            objE.Eanim.SetInteger("Anumber", 5);
            Invoke("Ev6_0", 1f);
        }
    }
    void Ev6_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[4], this.transform.position, this.transform.rotation, this.transform);
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
            Invoke("Ev6_1", 2f);
        }
    }
    void Ev6_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 3f;
    }
    void Event7()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            GManager.instance.setrg = 33;
            objE.Eanim.SetInteger("Anumber", 2);
            Invoke("Ev7_0", 0.3f);
        }
    }
    void Ev7_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[5], this.transform.position, this.transform.rotation, this.transform);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.inputEs = objE;
                }
            }
            Invoke("Ev7_1", 2f);
        }
    }
    void Ev7_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 2f;
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
        objE.Eanim.SetInteger("Anumber", 5);
        objE.audioS.PlayOneShot(Ase[4]);
        iTween.ShakePosition(CmObj.gameObject, iTween.Hash("x", 2f, "y", 2f, "time", 1f));
        Invoke("NextBoss", 2);
    }
    void NextBoss()
    {
        //第二形態になったら切り替えるやつ
        eventnumber = 6;
        resettrg = true;
    }
}
