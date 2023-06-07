using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class browrabbit : MonoBehaviour
{
    public ColEvent roomCol = null;
    public bool bossmove = false;
    public ColEvent stopCol;
    public enemyS objE;
    private GameObject p;
    private Vector3 target;
    private Rigidbody rb;
    private bool stoptrg = false;
    public ColEvent secondAttack;
    private float tmptime = 0;
    public AudioClip attackse;
    public AudioClip jumpse;
    // Start is called before the first frame update
    void Start()
    {
        objE = this.GetComponent<enemyS>();
        p = GameObject.Find("Player");
        rb = this.GetComponent<Rigidbody>();
        Invoke(nameof(StartTrg), 2f);
    }
    public void StartTrg()
    {
        objE.Eanim.SetBool("starttrg", true);
        objE.audioS.Stop();
        objE.audioS.clip = null;
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
                    if (!objE.stoptrg && roomCol == null)
                    {
                        if (bossmove == false && GManager.instance.bossbattletrg == 0)
                        {
                            Run();
                        }
                        else if (bossmove == true)
                        {
                            Run();
                        }
                    }
                    else if (!objE.stoptrg && roomCol != null && roomCol.ColTrigger == true)
                    {
                        if (bossmove == false && GManager.instance.bossbattletrg == 0)
                        {
                            Run();
                        }
                        else if (bossmove == true)
                        {
                            Run();
                        }
                    }
                    else
                    {
                        if (objE.noground == false && stoptrg == false)
                        {
                            stoptrg = true;
                            if (objE.Eanim.GetInteger("animindex") == 1)
                            {
                                objE.Eanim.SetInteger("animindex", 0);
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
                    if (objE.Eanim.GetInteger("animindex") == 1)
                    {
                        objE.Eanim.SetInteger("animindex", 0);
                    }
                }
            }
        }
    }
    public void AnimationAudio()
    {
        objE.audioS.PlayOneShot(attackse);
    }
    public void RunanimAudio()
    {
        objE.audioS.PlayOneShot(jumpse);
    }
    void Run()
    {
        target = this.transform.forward * objE.Estatus.speed;
        if (secondAttack.ColTrigger && objE.Eanim.GetInteger("animindex")!=2&&tmptime<=0)
        {
            objE.Eanim.SetInteger("animindex", 2);
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            tmptime = 0.3f;
        }
        else if (stopCol.ColTrigger)
        {
            if (tmptime <= 0) objE.Eanim.SetInteger("animindex", 0);
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
        }
        else if (stopCol.ColTrigger == false && objE.damagetrg == false && objE.Eanim.GetBool("starttrg") && !secondAttack.ColTrigger&&tmptime<=0)
        {
            rb.velocity = target;
            objE.Eanim.SetInteger("animindex", 1);
            if (stoptrg )
            {
                stoptrg = false;
            }
        }
        else if (stoptrg == false)
        {
            stoptrg = true;
            if(rb.velocity != Vector3.zero) rb.velocity = Vector3.zero;
        }
        if (tmptime >= 0f) tmptime -= Time.deltaTime;
    }
}
