using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat : MonoBehaviour
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
    public GameObject atMagic;
    private GameObject summonobj = null;
    private AddMagic addsummon = null;
    public AudioClip se;
    private float cureTime = 0;
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
                            cureTime += Time.deltaTime;
                            Run();
                        }
                        else if (objE.noground == false && bossmove == true && GManager.instance.bossbattletrg > 0)
                        {
                            cureTime += Time.deltaTime;
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
                            cureTime += Time.deltaTime;
                            Run();
                        }
                        else if (objE.noground == false && bossmove == true)
                        {
                            cureTime += Time.deltaTime;
                            Run();
                        }
                    }
                    else if (objE.noground == false && stoptrg == false)
                    {
                        stoptrg = true;
                        rb.velocity = Vector3.zero;
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
            if(cureTime >= 20)
            {
                cureTime = 0;
                objE.Eanim.SetInteger("Anumber", 3);
                Invoke("ShotMagic", 0.1f);
            }
            else
            {
                objE.Eanim.SetInteger("Anumber", 2);
                objE.damageOn = false;
                objE.audioS.PlayOneShot(se);
                Invoke("AnimReset", 1.3f);
            }
        }
    }

    void ShotMagic()
    {
        summonobj = Instantiate(atMagic, this.transform.position, this.transform.rotation, this.transform);
        if (summonobj != null)
        {
            addsummon = summonobj.GetComponent<AddMagic>();
            if (addsummon != null)
            {
                addsummon.enemytrg = true;
                addsummon.inputEs = objE;
            }
        }
        Invoke("AnimReset", 2f);
    }
    void AnimReset()
    {
        objE.Eanim.SetInteger("Anumber", 0);
        objE.damageOn = true;
        Invoke("atReset", 2f);
    }
    void atReset()
    {
        
        attrg = false;
    }
}