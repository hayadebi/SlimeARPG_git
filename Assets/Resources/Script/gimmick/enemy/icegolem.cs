using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class icegolem : MonoBehaviour
{
    
    public Transform eyePos;
    public EffekseerEmitter efsito;
    public bool bossmove = false;
    public ColEvent atCol_long;
    public ColEvent atCol_normal;
    public ColEvent atCol_min;
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
    private Vector3 vec;
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
        if (atCol_min.ColTrigger == false && atCol_normal.ColTrigger == false && atCol_long.ColTrigger == false && attrg == 0)
        {
            rb.velocity = target;
            objE.Eanim.SetInteger("Anumber", 1);
            if (stoptrg != false)
            {
                stoptrg = false;
            }
        }
        else if (atCol_min.ColTrigger == true && attrg == 0)
        {
            attrg = 1;
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            objE.Eanim.SetInteger("Anumber", 3);
            Event_min();
        }
        else if (atCol_normal.ColTrigger == true && attrg == 0)
        {
            attrg = 1;
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            objE.Eanim.SetInteger("Anumber", 4);
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
            objE.Eanim.SetInteger("Anumber", 2);
            Event_long();
        }

    }
    void Event_min()
    {
        if (attrg == 1)
        {
            attrg = 2;
            Invoke("Ev1_1", 0f);
        }
    }
    void Ev1_1()
    {
        if (attrg == 2)
        {
            attrg = 3;
            summonobj = Instantiate(atMagic[0], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack/4);
                }
            }
            Invoke("Ev_end", 2f);
        }
    }

    void Event_normal()
    {
        if (attrg == 1)
        {
            attrg = 2;
            Invoke("Ev2_1", 0.3f);
        }
    }
    void Ev2_1()
    {
        if (attrg == 2)
        {
            attrg = 3;
            summonobj = Instantiate(atMagic[1], this.transform.position, this.transform.rotation,this.transform);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                }
            }
            Invoke("Ev_end", 1f);
        }
    }

    void Event_long()
    {
        if (attrg == 1)
        {
            attrg = 2;
            Invoke("Ev3_1", 0.3f);
        }
    }
    void Ev3_1()
    {
        if (attrg == 2)
        {
            attrg = 3;
            objE.audioS.PlayOneShot(ase);
            vec = p.transform.position;
            efsito.gameObject.SetActive(true);
            efsito.Play();
            Invoke("Ev3_2", 1f);
        }
    }
    void Ev3_2()
    {
        if (attrg == 3)
        {
            attrg = 4;
            summonobj = Instantiate(atMagic[2], vec, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = (objE.Estatus.attack / 3);
                }
            }
            Invoke("Ev_end", 1.2f);
        }
    }
    void Ev_end()
    {
        efsito.Stop();
        efsito.gameObject.SetActive(false);
        objE.Eanim.SetInteger("Anumber", 0);
        Invoke("atReset", 2f);
    }

    void atReset()
    {
        attrg = 0;
    }
}
