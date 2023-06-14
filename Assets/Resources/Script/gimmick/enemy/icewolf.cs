using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class icewolf : MonoBehaviour
{
    public bool bossmove = false;
    public ColEvent atCol_long;
    public ColEvent atCol_normal;
    public ColEvent atCol_min;
    public ColEvent roomCol = null;
    enemyS objE;
    GameObject p;
    player ps;
    Vector3 target;
    Rigidbody rb;
    bool stoptrg = false;
    int attrg = 0;
    public GameObject[] atMagic;
    private GameObject summonobj = null;
    private AddMagic addsummon = null;
    public AudioClip[] ase;
    private Vector3 vec;
    private Image ice_ui;
    private bool attmp_uptrg = false;
    private bool attmp_downtrg = false;
    private Color defaultColor;
    private float ambient_time = 0f;
    public ParticleSystem[] effects;
    // Start is called before the first frame update
    void Start()
    {
        objE = this.GetComponent<enemyS>();
        p = GameObject.Find("Player");
        ps = p.GetComponent<player>();
        rb = this.GetComponent<Rigidbody>();
        ice_ui = GameObject.Find("ice").GetComponent<Image>();
        defaultColor = ice_ui.color;
    }
    public void EffectOn()
    {
        for(int i=0; i < effects.Length;)
        {
            effects[i].Play();
            i++; 
        }
        summonobj = Instantiate(atMagic[1], atMagic[2].transform.position, atMagic[2].transform.rotation, atMagic[2].transform);
        if (summonobj != null)
        {
            addsummon = summonobj.GetComponent<AddMagic>();
            if (addsummon != null)
            {
                addsummon.enemytrg = true;
                addsummon.Damage = (objE.Estatus.attack / 2);
            }
        }

    }
    public void EffectInstantiate()
    {
        Instantiate(atMagic[0], transform.position, transform.rotation);
    }
    public void EffectOff()
    {
        for (int i = 0; i < effects.Length;)
        {
            effects[i].Stop();
            i++;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (attmp_uptrg)
        {
            Color tmpc = ice_ui.color;
            tmpc.a = Mathf.SmoothStep(tmpc.a, defaultColor.a * 3f, Time.deltaTime * 3f); // Mathf.Lerp(tmpc.a, defaultColor.a * 3f, Time.deltaTime * 3f);
            ice_ui.color = tmpc;
            RenderSettings.fogDensity = Mathf.SmoothStep(RenderSettings.fogDensity, 0.04f, Time.deltaTime); //Mathf.Lerp(RenderSettings.fogDensity, 0.04f, Time.deltaTime);
        }
        else if (attmp_downtrg)
        {
            Color tmpc = ice_ui.color;
            tmpc.a = Mathf.SmoothStep(tmpc.a, defaultColor.a, Time.deltaTime * 4f);
            ice_ui.color = tmpc;
            RenderSettings.fogDensity = Mathf.SmoothStep(RenderSettings.fogDensity, 0.01f, Time.deltaTime*1.5f);
        }
        if (objE.absoluteStop == false)
        {
            if (GManager.instance.over == false && GManager.instance.walktrg == true && objE.damagetrg == false && objE.deathtrg == false)
            {
                if (objE.ren.isVisible && !objE.stoptrg)
                {
                    if (objE.noground == false && bossmove == false && GManager.instance.bossbattletrg == 0 && roomCol && roomCol.ColTrigger == true)
                        Run();
                    else if (objE.noground == false && bossmove == false && GManager.instance.bossbattletrg == 0 && !roomCol)
                        Run();
                    else if (objE.noground == false && bossmove == true)
                        Run();
                }
                else if (!objE.ren.isVisible && objE.noground == false && stoptrg == false)
                {
                    stoptrg = true;
                    rb.velocity = Vector3.zero;
                    if (objE.Eanim.GetInteger("Anumber") > 0 && objE.Eanim.GetInteger("Anumber") != 2 && objE.Eanim.GetInteger("Anumber") != 3)
                    {
                        objE.Eanim.SetInteger("Anumber", 0);
                    }
                }
            }
        }
        else if (objE.noground == false && stoptrg == false && (GManager.instance.over == true || GManager.instance.walktrg == false))
        {
            stoptrg = true;
            rb.velocity = Vector3.zero;
            if (objE.Eanim.GetInteger("Anumber") > 0 && objE.Eanim.GetInteger("Anumber") != 2 && objE.Eanim.GetInteger("Anumber") != 3)
            {
                objE.Eanim.SetInteger("Anumber", 0);
            }
        }
    }

    void Run()
    {
        target = this.transform.forward * objE.Estatus.speed;
        if(ambient_time >= 0f)
        {
            ambient_time -= Time.deltaTime;
        }
        if (!atCol_min.ColTrigger && !atCol_normal.ColTrigger && !atCol_long.ColTrigger && attrg == 0 && ambient_time <= 0)
        {
            attrg = 1;
            objE.Eanim.SetInteger("Anumber", 2);
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            objE.audioS.PlayOneShot(ase[0]);
            objE.audioS.PlayOneShot(ase[1]);
            //*氷エフェクト付与
            if (GManager.instance.isEnglish == 0)
            {
                GManager.instance.txtget = "5秒間凍結状態になった";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                GManager.instance.txtget = "He was frozen for five seconds.";
            }
            ps.EffectObj = Instantiate(GManager.instance.effectobj[25], p.transform.position, p.transform.rotation, p.transform);
            ps.icetime = 4f;
            ps.stoptrg = true;
            ps.anim.SetInteger(ps.Anumbername,0);
            Event1();
        }
        else if ((atCol_normal.ColTrigger || atCol_min.ColTrigger) && attrg == 0)
        {
            attrg = 1;
            if (stoptrg == false)
            {
                stoptrg = true;
                rb.velocity = Vector3.zero;
            }
            Event2();
        }
        else if (!atCol_min.ColTrigger && !atCol_normal.ColTrigger && attrg == 0)
        {
            rb.velocity = target;
            if (objE.Eanim.GetInteger("Anumber") != 2 && objE.Eanim.GetInteger("Anumber") != 3) objE.Eanim.SetInteger("Anumber", 1);
            if (stoptrg)
            {
                stoptrg = false;
            }
        }
        else if (stoptrg == false && attrg == 0)
        {
            stoptrg = true;
            rb.velocity = Vector3.zero;
            if(objE.Eanim.GetInteger("Anumber")!=2 && objE.Eanim.GetInteger("Anumber") != 3) objE.Eanim.SetInteger("Anumber", 0);
        }

    }
    void Event1()
    {
        if (attrg == 1)
        {
            ps.anim.SetInteger(ps.Anumbername, 0);
            attrg = 2;
            Ev1_1();
        }
    }
    void Ev1_1()
    {
        if (attrg == 2)
        {
            ps.anim.SetInteger(ps.Anumbername, 0);
            attrg = 3;
            attmp_uptrg = true;
            Invoke(nameof(Ev1_2), 1f);
        }
    }
    void Ev1_2()
    {
        if (attrg == 3)
        {
            ps.anim.SetInteger(ps.Anumbername, 0);
            attrg = 4;
            attmp_uptrg = false;
            attmp_downtrg = true;
            ambient_time = 12f;
            Invoke(nameof(Ev_end), 1f);
        }
    }
    void Event2()
    {
        if (attrg == 1)
        {
            objE.Eanim.SetInteger("Anumber", 3);
            
            attrg = 2;
            Invoke(nameof(Ev_end), 1.2f);
        }
    }
    void Ev_end()
    {
        attmp_uptrg = false;
        attmp_downtrg = false;
        if(RenderSettings.fogDensity!=0.01f) RenderSettings.fogDensity = 0.01f;
        Color tmpc = ice_ui.color;
        if (tmpc.a != defaultColor.a)
        {
            tmpc.a = defaultColor.a;
            ice_ui.color = tmpc;
        }
        objE.Eanim.SetInteger("Anumber", 0);
        Invoke("atReset", 1f);
    }

    void atReset()
    {
        attrg = 0;
    }
}
