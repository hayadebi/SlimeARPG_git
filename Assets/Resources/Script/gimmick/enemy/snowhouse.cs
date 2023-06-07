using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowhouse : MonoBehaviour
{
    public objAngle oa;
    public bool bossmove = false;
    public ColEvent roomCol;
    public ColEvent atCol;
    enemyS objE;
    public Transform[] target;
    Rigidbody rb;
    bool stoptrg = false;
    int attrg = 0;
    public GameObject[] atMagic;
    public GameObject[] summonobj;
    public GameObject shield_effect;
    private AddMagic addsummon = null;
    public AudioClip[] ase;
    private bool specalATtrg = false;
    public GameObject cmp;
    public float count_time = 0;
    private bool summon_trg = false;
    public Animator shieldAnim;
    // Start is called before the first frame update
    void Start()
    {
        cmp = GameObject.Find("MainC");
        objE = this.GetComponent<enemyS>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!objE.absoluteStop)
        {
            
            if (!GManager.instance.over && GManager.instance.walktrg && !objE.damagetrg && !objE.deathtrg)
            {
                if (objE.ren.isVisible && !objE.stoptrg)
                {
                    if (!objE.noground && !bossmove && GManager.instance.bossbattletrg == 0 && roomCol && roomCol.ColTrigger)
                    {
                        count_time += Time.deltaTime;
                        Run();
                    }
                    else if (!objE.noground && !bossmove && GManager.instance.bossbattletrg == 0 && !roomCol)
                    {
                        Run();
                    }
                    else if (!objE.noground && bossmove)
                    {
                        Run();
                    }
                }
                else if (!objE.ren.isVisible && !objE.noground && !stoptrg)
                {
                    stoptrg = true;
                    rb.velocity = Vector3.zero;
                    if (objE.Eanim.GetInteger("Anumber") > 0)
                    {
                        objE.Eanim.SetInteger("Anumber", 0);
                    }
                }
            }
            else if ((GManager.instance.over || !GManager.instance.walktrg) && (!objE.noground && !stoptrg))
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

    void Run()
    {
        if(!objE.damageOn && !summonobj[3] && !summonobj[4] && summon_trg)
        {
            objE.damageOn = true;
            shieldAnim.SetInteger("Anumber", 1);
            Invoke(nameof(shield_off), 1.1f);
        }
        if (!atCol.ColTrigger && attrg == 0)
        {
            objE.Eanim.SetInteger("Anumber", 0);
            if (stoptrg != false)
            {
                stoptrg = false;
            }
        }
        else if (atCol.ColTrigger && attrg == 0 && count_time > 3)
        {
            attrg = 1;
            if (!stoptrg)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            if (!specalATtrg )
            {
                specalATtrg = true;
                objE.Eanim.SetInteger("Anumber", 1);
                
                Invoke("Event0", 1.05f);
            }
            else if (specalATtrg)
            {
                Event1();
            }
        }

    }
    void shield_off()
    {
        shield_effect.SetActive(false);
    }
    void Event0()
    {
        if (attrg == 1)
        {
            attrg = 2;
            shield_effect.SetActive(true);
            objE.damageOn = false;
            objE.audioS.PlayOneShot(ase[3]);
            objE.audioS.PlayOneShot(ase[2]);
            summonobj[0] = Instantiate(atMagic[0], target[1].position, transform.rotation);
            summonobj[1] = Instantiate(atMagic[0], target[2].position, transform.rotation);
            
            Invoke("Ev0_1", 1.45f);
        }
    }
    void Ev0_1()
    {
        if (attrg == 2)
        {
            attrg = 3;
            objE.Eanim.SetInteger("Anumber", 2);
            objE.audioS.PlayOneShot(ase[1]);
            objE.audioS.PlayOneShot(ase[0]);
            summonobj[3] = Instantiate(atMagic[3], summonobj[0].transform.position, summonobj[0].transform.rotation);
            summonobj[4] = Instantiate(atMagic[3], summonobj[1].transform.position, summonobj[1].transform.rotation);
            summon_trg = true;
            Destroy(summonobj[0].gameObject, 0.1f);
            Destroy(summonobj[1].gameObject, 0.1f);
            iTween.ShakePosition(cmp.gameObject, iTween.Hash("x", 1f, "y", 1f, "time", 1f));
            Invoke("Ev1_end", 3.5f);
        }
    }
    void Event1()
    {
        if (attrg == 1)
        {
            attrg = 2;
            objE.Eanim.SetInteger("Anumber", 1);
            Invoke("Ev1_1", 1.05f);
        }
    }
    void Ev1_1()
    {
        if (attrg == 2)
        {
            attrg = 3;
            summonobj[2] = Instantiate(atMagic[4], this.transform.position, this.transform.rotation);
            if (summonobj != null)
            {
                addsummon = summonobj[2].GetComponent<AddMagic>();
                if (addsummon != null)
                {
                    addsummon.enemytrg = true;
                    addsummon.Damage = objE.Estatus.attack;
                }
            }
            Invoke("Ev1_end", 2f);
        }
    }
    void Ev1_end()
    {
        
        objE.Eanim.SetInteger("Anumber", 0);
        Invoke("atReset", 1f);
    }

    void atReset()
    {
        attrg = 0;
    }
}
