using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hanagolem : MonoBehaviour
{
    public objAngle oa;
    public bool bossmove = false;
    public ColEvent atCol;
    public ColEvent roomCol = null;
    enemyS objE;
    GameObject p;
    Vector3 target;
    Rigidbody rb;
    bool stoptrg = false;
    int attrg = 0;
    public GameObject atMagic;
    public Transform handpos;
    private GameObject summonobj = null;
    private AddMagic addsummon = null;
    private Vector3 vec;
    private float time;
    public AudioClip[] ase;
    // Start is called before the first frame update
    void Start()
    {
        objE = this.GetComponent<enemyS>();
        p = GameObject.Find("Player");
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (objE.absoluteStop == false)
        {
            if (GManager.instance.over == false && GManager.instance.walktrg == true)
            {
                if (objE.damagetrg == false && objE.deathtrg == false)
                {
                    if (objE.ren.isVisible && !objE.stoptrg)
                    {
                        //if (objE.noground == true && Physics.Linecast(this.transform.position, (this.transform.position + (this.transform.up * -8)), out objE.hit, objE.obstacleLayer))
                        //{
                        //    objE.noground = false;
                        //}
                        //else if (objE.noground == false && !Physics.Linecast(this.transform.position, (this.transform.position + (this.transform.up * -8)), out objE.hit, objE.obstacleLayer))
                        //{
                        //    objE.noground = true;
                        //}
                        if (objE.noground == false && bossmove == false && GManager.instance.bossbattletrg == 0 && roomCol && roomCol.ColTrigger == true)
                        {
                            Run();
                        }
                        else if (objE.noground == false && bossmove == false && GManager.instance.bossbattletrg == 0 && !roomCol)
                        {
                            Run();
                        }
                        else if (objE.noground == false && bossmove == true)
                        {
                            Run();
                        }
                    }
                    else if (!objE.ren.isVisible)
                    {
                        if (objE.noground == false && stoptrg == false)
                        {
                            stoptrg = true;
                            rb.velocity = Vector3.zero;
                            if (objE.Eanim.GetInteger("Anumber") == 1 || objE.Eanim.GetInteger("Anumber") == 2)
                            {
                                objE.Eanim.SetInteger("Anumber", 0);
                            }
                        }
                    }
                }
            }
            else if (GManager.instance.over == true || GManager.instance.walktrg == false)
            {
                if (objE.noground == false && stoptrg == false)
                {
                    stoptrg = true;
                    rb.velocity = Vector3.zero;
                    if (objE.Eanim.GetInteger("Anumber") == 1 || objE.Eanim.GetInteger("Anumber") == 2)
                    {
                        objE.Eanim.SetInteger("Anumber", 0);
                    }
                }
            }
        }
    }

    void Run()
    {
        target = this.transform.forward * objE.Estatus.speed ;
        if (atCol.ColTrigger == false && attrg == 0)
        {
            rb.velocity = target;
            objE.Eanim.SetInteger("Anumber", 1);
            if (stoptrg != false)
            {
                stoptrg = false;
            }
        }
        else if (atCol.ColTrigger == true && attrg == 0)
        {
            attrg = 1;
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            Event3();
        }
        else if (attrg == 2)
        {
            Event3();
        }
    }

    void Event3()
    {
        if (attrg == 0 || attrg == 1)
        {
            attrg = 1;
            oa.enabled = false;
            vec = p.transform.position;
            vec.y = 0.5f;
            time = Vector3.Distance(this.transform.position, vec);
            objE.Eanim.SetInteger("Anumber", 2);
            Ev3_0();
        }
        else if (attrg == 2)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, vec, time / 30f);
        }
    }
    void Ev3_0()
    {
        if (attrg == 1)
        {
            attrg = 2;
            objE.audioS.PlayOneShot(ase[0]);
            Invoke("Ev3_1", 0.5f);
        }
    }
    void Ev3_1()
    {
        if (attrg == 2)
        {
            attrg = 3;
            objE.audioS.PlayOneShot(ase[1]);
            Instantiate(GManager.instance.effectobj[2], handpos.position, this.transform.rotation,handpos);
            oa.enabled = true;
            rb.velocity = Vector3.zero;
            Invoke("Ev3_2", 1.3f);
        }
    }
    void Ev3_2()
    {
        if (attrg == 3)
        {
            attrg = 4;
            objE.Eanim.SetInteger("Anumber", 3);
            Invoke("Ev3_3", 0.3f);
        }
    }
    void Ev3_3()
    {
        if (attrg == 4)
        {
            attrg = 5;
            summonobj = Instantiate(atMagic, handpos.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                }
            }
            Invoke("Ev3_4", 0.4f);
        }
    }
    void Ev3_4()
    {
        objE.Eanim.SetInteger("Anumber", 0);
        Invoke("atReset", 2f);
    }

    void atReset()
    {
        attrg = 0;
    }
}
