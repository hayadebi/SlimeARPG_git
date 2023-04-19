using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tako : MonoBehaviour
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
    public AudioClip[] chargese;
    public float[] shottime ;
    public float[] endtime ;
    public int[] setanimint;
    private int setrandom = 0;
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
           
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            setrandom = Random.Range(0, 2);
            SetMagic();
        }
    }
    void SetMagic()
    {
        objE.Eanim.SetInteger("Anumber", setanimint[setrandom]);
        if (chargese != null && chargese.Length > 0)
        {
            objE.audioS.PlayOneShot(chargese[setrandom]);
        }
        Invoke("ShotMagic", shottime[setrandom]);
    }
    void ShotMagic()
    {
        summonobj = Instantiate(atMagic[setrandom], this.transform.position, this.transform.rotation,this.transform);
        if (summonobj != null)
        {
            addsummon = summonobj.GetComponent<AddMagic>();
            if (addsummon != null)
            {
                addsummon.enemytrg = true;
                addsummon.Damage = (objE.Estatus.attack );
            }
        }
        Invoke("AnimReset", endtime[setrandom]);
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