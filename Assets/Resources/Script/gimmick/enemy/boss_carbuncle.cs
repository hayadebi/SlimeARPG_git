using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using Effekseer;

public class boss_carbuncle : MonoBehaviour
{
    public objAngle oa;
    public int maxrandom = 3;
    public int minrandom = 0;
    bool resettrg = false;
    [Header("1は動き始める時間、4はイベントリセット")] public float[] time;
    public int ontrg = 0;
    [Header("0と1はランダム配置の範囲")] public Transform[] Bpos;
    public Vector3[] vec;
    public int eventnumber = 1;
    public AudioClip[] Ase;
    public GameObject[] obj;
    public Renderer ren;
    enemyS objE;
    public bool[] trg;
    int oldevent = 1;
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
    //public EffekseerEmitter[] eyePos;
    public float[] numbers;
    public GameObject CmObj;
    public float godSpeed = 1.6f;
    public bool attackTrg = false;

    // Start is called before the first frame update
    void Start()
    {
        flowChart = this.GetComponent<Flowchart>();
        rb = this.GetComponent<Rigidbody>();
        p = GameObject.Find("Player");
        objE = this.GetComponent<enemyS>();
        //eventnumber = Random.Range(2, maxrandom);
        attackTrg = false;
        eventnumber = 1;
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
        attackTrg = true;
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
                attackTrg = false;
                numbers[0] = 0;
                numbers[1] = 0;
                eventnumber = -1;
                ontrg = 999;
                obj[0].SetActive(false);
                rb.velocity = Vector3.zero;
                Instantiate(GManager.instance.effectobj[13], this.transform.position, this.transform.rotation, this.transform);
                GManager.instance.setrg = 20;
                objE.Estatus.defence += 4;
                maxrandom = 8;
                minrandom = 1;
                objE.Eanim.SetInteger("Anumber", 0);
                StartCoroutine(Talk());
            }
            if (resettrg == true && eventnumber != -1 && ontrg < 999 )
            {
                objE.damageOn = true;
                resettrg = false;
                ontrg = 0;
                numbers[0] = 0;
                numbers[1] = 0;
                trg[1] = false;
                objE.audioS.Stop();
                rb.velocity = Vector3.zero;
                obj[0].SetActive(false);
                if (oa.enabled == false)
                {
                    oa.enabled = true;
                }
                if (trg[2])
                    eventnumber = Random.Range(2, maxrandom);
                else
                    eventnumber = Random.Range(1, maxrandom);
                for (int i = oldevent; i == eventnumber ;)
                {
                    if (trg[2])
                        eventnumber = Random.Range(2, maxrandom);
                    else
                        eventnumber = Random.Range(1, maxrandom);
                }
                oldevent = eventnumber;
                attackTrg = true;
            }
            if (GManager.instance.over == false && GManager.instance.walktrg == true && objE.deathtrg == false && trg[1] == false && eventnumber != -1 && ontrg < 999 && objE.DsMove == false)
            {
                if (time[4] > 0 )
                {
                    time[4] -= Time.deltaTime;
                    if (time[4] <= 0)
                    {
                        time[4] = 0;
                        objE.Eanim.SetInteger("Anumber", 0);
                        resettrg = true;
                    }
                }
                if (GManager.instance.bossbattletrg == 1)
                {
                    if (p != null && eventnumber != -1 && attackTrg)
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
        if (ontrg == 0 && numbers[1] <= 0)
        {
            attackTrg = true;
            objE.damageOn = false;
            ontrg = 1;
            oa.enabled = false;
            objE.Eanim.SetInteger("Anumber", 2);
            obj[0].SetActive(true);
        }
        else if (ontrg == 1 && numbers[0] > 0)
        {
            numbers[0] -= Time.deltaTime;
            transform.position += transform.forward * godSpeed;
        }
        else if (ontrg == 1 && numbers[1] < 4 && numbers[0] <= 0)
        {
            objE.Estatus.speed = 40;
            rb.velocity = Vector3.zero;

            objE.audioS.PlayOneShot(Ase[0]);
            Instantiate(GManager.instance.effectobj[4], this.transform.position, this.transform.rotation);
            vec[0] = Bpos[Mathf.FloorToInt(numbers[1])].transform.position;
            this.transform.position = vec[0];
            Instantiate(obj[1], this.transform.position, this.transform.rotation);
            var rotation = Quaternion.LookRotation(p.transform.position - this.transform.position);
            rotation.x = 0;
            rotation.z = 0;
            this.transform.rotation = rotation;
            objE.Estatus.speed = 0.02f;
            numbers[0] = 1f;
            numbers[1] += 1;
        }
        else if (ontrg == 1 && numbers[1] >= 4)
        {
            ontrg = 4;
            numbers[0] = 0;
            numbers[1] = 0;
            objE.Estatus.speed = 40;

            obj[0].SetActive(false);
            objE.audioS.PlayOneShot(Ase[0]);
            Instantiate(GManager.instance.effectobj[4], this.transform.position, this.transform.rotation);
            vec[0] = Bpos[4].transform.position;

            if (vec[0].x > Bpos[1].position.x)
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
            Instantiate(obj[2], this.transform.position, this.transform.rotation);

            oa.enabled = true;
            rb.velocity = Vector3.zero;
            objE.Eanim.SetInteger("Anumber", 0);
            attackTrg = false;
            time[4] = 2.5f;
        }
    }
    void Event2()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            attackTrg = true;
            numbers[2] = 0;
            objE.audioS.PlayOneShot(Ase[1]);
            objE.Eanim.SetInteger("Anumber", 1);
            Invoke("Ev2_1", 0.4f);
        }
        
    }
    void Ev2_1()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            vec[0] = this.transform.position;
            vec[0].y = p.transform.position.y;
            summonobj = Instantiate(magicObj[0], vec[0], this.transform.rotation);
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
            Invoke("Ev2_2", 1.3f);
        }
        
    }
    void Ev2_2()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        attackTrg = false;
        time[4] = 1.2f;
    }

    void Event3()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            attackTrg = true;
            objE.Eanim.SetInteger("Anumber", 3);
            objE.audioS.PlayOneShot(Ase[0]);
            Instantiate(GManager.instance.effectobj[4], this.transform.position, this.transform.rotation);
            vec[0] = Bpos[4].transform.position;

            if (vec[0].x > Bpos[1].position.x)
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
            Invoke("Ev3_0", 0.4f);
        }
        
    }
    void Ev3_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            summonobj = Instantiate(magicObj[1], this.transform.position, this.transform.rotation,this.transform);
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
            Invoke("Ev3_1", 1f);
        }
        
    }
    void Ev3_1()
    {
        ontrg = 3;
        attackTrg = false;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 2.15f;
    }

    void Event4()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            attackTrg = true;
            objE.Eanim.SetInteger("Anumber", 3);
            objE.audioS.PlayOneShot(Ase[1]);
            Invoke("Ev4_0", 0.4f);
        }
        
    }
    void Ev4_0()
    {
        if (ontrg == 1)
        {
            ontrg = 2;
            vec[0] = this.transform.position;
            vec[0].y = p.transform.position.y;
            summonobj = Instantiate(magicObj[2], vec[0], this.transform.rotation);
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
            Invoke("Ev4_1", 1f);
        }
        
    }
    void Ev4_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        attackTrg = false;
        time[4] = 1.3f;
    }

    void Event5()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 4);
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
                    addsummon.Damage = 0;
                    addsummon.nokill = false;
                }
            }
            Invoke("Ev5_1", 1.4f);
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
            objE.Eanim.SetInteger("Anumber", 3);
            Invoke("Ev6_0", 0.4f);
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
            Invoke("Ev6_1", 1f);
        }
    }
    void Ev6_1()
    {
        ontrg = 3;
        rb.velocity = Vector3.zero;
        objE.Eanim.SetInteger("Anumber", 0);
        time[4] = 1.3f;
    }
    void Event7()
    {
        if (ontrg == 0)
        {
            ontrg = 1;
            objE.Eanim.SetInteger("Anumber", 1);
            Invoke("Ev7_0", 0.4f);
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
                    var assc = addsummon.transform.localScale;
                    assc.x *= 2;
                    assc.y *= 2;
                    assc.z *= 2;
                    addsummon.transform.localScale = assc;
                    addsummon.enemytrg = true;
                    addsummon.Damage = 0;
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
        time[4] = 1f;
    }
    //void Eventreset()
    //{
    //    if (trg[1] == false && eventnumber != -1 && ontrg < 999)
    //    {
    //        //ateffect.SetActive(false);
    //        objE.Eanim.SetInteger("Anumber", 0);
    //        resettrg = true;
    //    }
    //}
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
        objE.Eanim.SetInteger("Anumber", 4);
        objE.audioS.PlayOneShot(Ase[2]);
        iTween.ShakePosition(CmObj.gameObject, iTween.Hash("x", 2f, "y", 2f, "time", 1f));
        Invoke("NextBoss", 2f);
    }
    void NextBoss()
    {
        objE.Eanim.SetInteger("Anumber", 0);
        rb.velocity = Vector3.zero;
        ontrg = 60;
        //第二形態になったら切り替えるやつ
        eventnumber = 1;
        attackTrg = false;
        objE.damageOn = true;
        eventnumber = 2;
        ontrg = 99;
        resettrg = true;
    }
}
