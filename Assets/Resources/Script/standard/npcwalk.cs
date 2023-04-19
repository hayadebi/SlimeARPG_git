using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //NavMeshAgentを使うために必要

public class npcwalk : MonoBehaviour
{
    public Animator anim;
    public GameObject[] targets;
    public Renderer ren;
    public float agentSP = 2;
    // 目的地
    private int nextIndex = 0; //次の目的地のインデックス
    private NavMeshAgent agent; //自動で動くオブジェクト
    private float walktime = 0;
    private Rigidbody rb;
    public bool frendTrg = false;
    private bool areaTrg = false;
    void Start()
    {
        // アタッチされているオブジェクトのNaveMeshAgentを取得
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        //// 目的地情報の取得(順番通りに取得できるわけではない)
        //targets = GameObject.FindGameObjectsWithTag("POS");

        // 最初の目的地を設定
        nextIndex = Random.Range(0, targets.Length);
        agent.destination = targets[nextIndex].transform.position;
        anim.SetInteger("Anumber", 1);
        agent.speed = 0;
        if(frendTrg == true)
        {
            agent.enabled = false;
        }
    }
    private void FixedUpdate()
    {
        if ( !GManager.instance.over && ren.isVisible)
        {
            if (frendTrg == false)
            {
                walktime += Time.deltaTime;
                if (walktime > 15)
                {
                    walktime = 0;
                    nextIndex = Random.Range(0, targets.Length);
                    agent.destination = targets[nextIndex].transform.position;
                }
                if (agent.speed != agentSP)
                {
                    agent.destination = targets[nextIndex].transform.position;
                    agent.speed = agentSP;
                    anim.SetInteger("Anumber", 1);
                }
            }
            else if(frendTrg == true && areaTrg == false)
            {
                rb.velocity = this.transform.forward * agentSP;
                if (anim.GetInteger("Anumber") != 1)
                {
                    anim.SetInteger("Anumber", 1);
                }
            }
        }
        else if (GManager.instance.over || !ren.isVisible)
        {
            if (agent.speed != 0)
            {
                rb.velocity = Vector3.zero;
                agent.speed = 0;
                anim.SetInteger("Anumber", 0);
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "npc" && agent.speed != 0 && frendTrg == false)
        {
            nextIndex = Random.Range(0, targets.Length);
            agent.destination = targets[nextIndex].transform.position;
        }
        else if (col.tag == "Player" && frendTrg == true && areaTrg == false)
        {
            areaTrg = true;
            rb.velocity = Vector3.zero;
            anim.SetInteger("Anumber", 0);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player" && frendTrg == true && areaTrg == true)
        {
            areaTrg = false;
        }
    }
}