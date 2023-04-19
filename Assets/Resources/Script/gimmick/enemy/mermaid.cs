using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mermaid : MonoBehaviour
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
    public GameObject[] atMagic;
    private GameObject summonobj = null;
    private AddMagic addsummon = null;
    private Vector3 vec;
    private float time;
    public AudioClip ase;
    private bool specalATtrg = false;
    public GameObject cmp;
    private player ps;
    // Start is called before the first frame update
    void Start()
    {
        objE = this.GetComponent<enemyS>();
        p = GameObject.Find("Player");
        ps = p.GetComponent<player>();
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
            if (!specalATtrg)
            {
                specalATtrg = true;
                objE.Eanim.SetInteger("Anumber", 2);
                Event0();
            }
            else if (specalATtrg)
            {
                Event1();
            }
        }
        
    }
    void Event0()
    {
        if (attrg == 1)
        {
            attrg = 2;
            objE.Eanim.SetInteger("Anumber", 2);
            objE.audioS.PlayOneShot(ase);

            ps.darktrg = true;
            GManager.instance.Pstatus[GManager.instance.playerselect].mp /= 3;
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.txtget = "呪いの奇声でDFとMPが奪われた";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                GManager.instance.txtget = "Cursed strange voice took away my DF and MP.";
            }
            Instantiate(GManager.instance.effectobj[11], p.transform.position, p.transform.rotation, p.transform);

            iTween.ShakePosition(cmp.gameObject, iTween.Hash("x", 2f, "y", 2f, "time", 1f));
            Invoke("Ev0_1", 2.1f);
        }
    }
    void Ev0_1()
    {
        if (attrg == 2)
        {
            attrg = 3;
            objE.Eanim.SetInteger("Anumber", 0);
            Invoke("Ev1_end", 0.3f);
        }
    }
    void Event1()
    {
        if (attrg == 1 )
        {
            attrg = 2;
            objE.Eanim.SetInteger("Anumber", 3);
            Invoke("Ev1_1", 0.2f);
        }
    }
    void Ev1_1()
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
                    addsummon.Damage = objE.Estatus.attack ;
                }
            }
            Invoke("Ev1_2", 1.1f);
        }
    }
    void Ev1_2()
    {
        if (attrg == 3)
        {
            attrg = 4;
            objE.Eanim.SetInteger("Anumber", 4);
            Invoke("Ev1_3", 0.4f);
        }
    }
    void Ev1_3()
    {
        if (attrg == 4)
        {
            attrg = 5;
            summonobj = Instantiate(atMagic[0], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj.GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack ;
                }
            }
            Invoke("Ev1_end", 1.1f);
        }
    }
    void Ev1_end()
    {
        objE.Eanim.SetInteger("Anumber", 0);
        Invoke("atReset", 2f);
    }

    void atReset()
    {
        attrg = 0;
    }
}
