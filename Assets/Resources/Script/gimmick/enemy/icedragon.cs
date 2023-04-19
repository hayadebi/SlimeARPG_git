using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class icedragon : MonoBehaviour
{
    public objAngle oa;
    public Transform atPos;
    public bool bossmove = false;
    public ColEvent atCol_long;
    public ColEvent atCol_normal;
    public ColEvent roomCol = null;
    enemyS objE;
    GameObject p;
    Vector3 target;
    Rigidbody rb;
    bool stoptrg = false;
    int attrg = 0;
    public GameObject[] atMagic;
    private GameObject summonobj = null;
    private AddMagic addsummon = null;
    public AudioClip ase;
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
                            if (objE.Eanim.GetInteger("Anumber") > 0)
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
                    if (objE.Eanim.GetInteger("Anumber") > 0)
                    {
                        objE.Eanim.SetInteger("Anumber", 0);
                    }
                }
            }
        }
    }

    void Run()
    {
        target = this.transform.forward * objE.Estatus.speed;
        if (atCol_normal.ColTrigger == false && atCol_long.ColTrigger == false && attrg == 0)
        {
            rb.velocity = target;
            objE.Eanim.SetInteger("Anumber", 1);
            if (stoptrg != false)
            {
                stoptrg = false;
            }
        }
        else if (atCol_normal.ColTrigger == true && attrg == 0)
        {
            attrg = 1;
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            objE.Eanim.SetInteger("Anumber", 2);
            Event_normal();
        }
        else if (atCol_long.ColTrigger == true && attrg == 0)
        {
            attrg = 1;
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            objE.Eanim.SetInteger("Anumber", 3);
            Event_long();
        }

    }

    void Event_normal()
    {
        if (attrg == 1)
        {
            attrg = 2;
            Invoke("Ev2_1", 0f);
        }
    }
    void Ev2_1()
    {
        if (attrg == 2)
        {
            attrg = 3;
            summonobj = Instantiate(atMagic[0], atPos.position, this.transform.rotation, atPos);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                }
            }
            Invoke("Ev2_2", 1f);
        }
    }
    void Ev2_2()
    {
        if(attrg == 3)
        {
            attrg = 4;
            oa.enabled = false;
            Invoke("Ev_end", 4f);
        }
    }

    void Event_long()
    {
        if (attrg == 1)
        {
            attrg = 2;
            objE.audioS.PlayOneShot(ase);
            Invoke("Ev3_1", 0f);
        }
    }
    void Ev3_1()
    {
        if (attrg == 2)
        {
            attrg = 3;
            summonobj = Instantiate(atMagic[1], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                }
            }
            Invoke("Ev_end", 3f);
        }
    }
    void Ev_end()
    {
        oa.enabled = true;
        objE.Eanim.SetInteger("Anumber", 0);
        Invoke("atReset", 2f);
    }

    void atReset()
    {
        attrg = 0;
    }
}
