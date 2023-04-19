using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chicken : MonoBehaviour
{
    public objAngle oa;
    public bool bossmove = false;
    public ColEvent atCol;
    enemyS objE;
    GameObject p;
    Vector3 target;
    Rigidbody rb;
    bool stoptrg = false;
    int attrg = 0;
    public GameObject[] atMagic;
    public Transform atpos;
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
                        if (objE.noground == false && bossmove == false && GManager.instance.bossbattletrg == 0)
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
                            if (objE.Eanim.GetInteger("Anumber") != 0)
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
                    if (objE.Eanim.GetInteger("Anumber") != 0)
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
            Event1();
        }
    }

    void Event1()
    {
        if (attrg == 0 || attrg == 1)
        {
            attrg = 2;
            objE.Eanim.SetInteger("Anumber", 2);
            Invoke("Ev1_0", 0.3f);
        }
    }
    void Ev1_0()
    {
        if (attrg == 2)
        {
            attrg = 3;
            summonobj = Instantiate(atMagic[0], atpos.position, this.transform.rotation,this.transform);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                }
            }
            Invoke("Ev1_1", 1.3f);
        }
    }
    void Ev1_1()
    {
        if (attrg == 3)
        {
            attrg = 4;
            objE.Eanim.SetInteger("Anumber", 3);
            objE.audioS.PlayOneShot(ase[0]);
            Invoke("Ev1_2", 0.3f);
        }
    }
    void Ev1_2()
    {
        if (attrg == 4)
        {
            attrg = 5;
            Instantiate(GManager.instance.effectobj[13], this.transform.position, this.transform.rotation);
            Invoke("Ev1_3", 1.3f);
        }
    }
    void Ev1_3()
    {
        if (attrg == 5)
        {
            attrg = 6;
            summonobj = Instantiate(atMagic[1], atpos.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                }
            }
            Invoke("Ev1_4", 1.3f);
        }
    }
    void Ev1_4()
    {
        objE.Eanim.SetInteger("Anumber", 0);
        Invoke("atReset", 2f);
    }

    void atReset()
    {
        attrg = 0;
    }
}
