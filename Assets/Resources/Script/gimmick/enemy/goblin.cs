using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblin : MonoBehaviour
{
    public bool bossmove = false;
    public ColEvent roomCol = null;
    public ColEvent atCol;
    enemyS objE;
    GameObject p;
    Vector3 target;
    Rigidbody rb;
    bool stoptrg = false;
    bool attrg = false;
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
                    if (objE.ren.isVisible && !objE.stoptrg && roomCol == null)
                    {
                        //if (objE.noground == true && Physics.Linecast(this.transform.position, (this.transform.position + (this.transform.up * -8)), out objE.hit, objE.obstacleLayer))
                        //{
                        //    objE.noground = false;
                        //}
                        //else if (objE.noground == false && !Physics.Linecast(this.transform.position, (this.transform.position + (this.transform.up * -8)), out objE.hit, objE.obstacleLayer))
                        //{
                        //    objE.noground = true;
                        //}
                        //
                        if (objE.noground == false && bossmove == false && GManager.instance.bossbattletrg == 0)
                        {
                            Run();
                        }
                        else if (objE.noground == false && bossmove == true)
                        {
                            Run();
                        }
                    }
                    else if (objE.ren.isVisible && !objE.stoptrg && roomCol != null && roomCol.ColTrigger == true)
                    {
                        //if (objE.noground == true && Physics.Linecast(this.transform.position, (this.transform.position + (this.transform.up * -8)), out objE.hit, objE.obstacleLayer))
                        //{
                        //    objE.noground = false;
                        //}
                        //else if (objE.noground == false && !Physics.Linecast(this.transform.position, (this.transform.position + (this.transform.up * -8)), out objE.hit, objE.obstacleLayer))
                        //{
                        //    objE.noground = true;
                        //}
                        //
                        if (objE.noground == false && bossmove == false && GManager.instance.bossbattletrg == 0)
                        {
                            Run();
                        }
                        else if (objE.noground == false && bossmove == true)
                        {
                            Run();
                        }
                    }
                    else
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
        if (atCol.ColTrigger == false && attrg == false)
        {
            rb.velocity = target;
            objE.Eanim.SetInteger("Anumber", 1);
            if (stoptrg != false)
            {
                stoptrg = false;
            }
        }
        else if (atCol.ColTrigger == true && attrg == false)
        {
            attrg = true;
            objE.Eanim.SetInteger("Anumber", 4);
            objE.audioS.PlayOneShot(ase);
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            Invoke("ShotMagic1", 0.3f);
        }
    }

    void ShotMagic1()
    {
        summonobj = Instantiate(atMagic[0], this.transform.position, this.transform.rotation, this.transform);
        if (summonobj != null)
        {
            addsummon = summonobj.GetComponent<AddMagic>();
            if (addsummon != null)
            {
                addsummon.enemytrg = true;
                addsummon.Damage = 0;
            }
        }
        Invoke("ShotMagic2", 1f);
    }
    void ShotMagic2()
    {
        summonobj = Instantiate(atMagic[1], this.transform.position, this.transform.rotation, this.transform);
        if (summonobj != null)
        {
            addsummon = summonobj.GetComponent<AddMagic>();
            if (addsummon != null)
            {
                addsummon.enemytrg = true;
                addsummon.Damage = (objE.Estatus.attack / 3);
            }
        }
        Invoke("AnimReset", 1f);
    }
    void AnimReset()
    {
        objE.Eanim.SetInteger("Anumber", 0);
        Invoke("atReset", 2f);
    }
    void atReset()
    {
        attrg = false;
    }
}