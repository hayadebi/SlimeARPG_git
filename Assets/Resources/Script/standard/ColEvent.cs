using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColEvent : MonoBehaviour
{
    public bool ColTrigger = false;
    public bool onAction = true;
    public string tagName = "Player";
    public bool managerTrg = false;
    public int managerIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == tagName && onAction == true)
        {
            ColTrigger = true;
            if(managerTrg )
            {
                GManager.instance.colTrg[managerIndex] = true;
            }
        }
        else if (tagName == "" && onAction == true && col.tag != "Player" && col.tag != "OnMask" && col.tag != "enemy" && col.tag != "noactive" && col.tag != "Water" && col.tag != "pbullet" && col.tag != "ebullet" && col.tag != "npc")
        {
            ColTrigger = true;
            if (managerTrg)
            {
                GManager.instance.colTrg[managerIndex] = true;
            }
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.tag == tagName && onAction == true && ColTrigger == false)
        {
            ColTrigger = true;
            if (managerTrg)
            {
                GManager.instance.colTrg[managerIndex] = true;
            }
        }
        else if (tagName == "" && onAction == true && ColTrigger == false && col.tag != "Player" && col.tag != "OnMask" && col.tag != "enemy" && col.tag != "noactive" && col.tag != "Water" && col.tag != "pbullet" && col.tag != "ebullet" && col.tag != "npc")
        {
            ColTrigger = true;
            if (managerTrg)
            {
                GManager.instance.colTrg[managerIndex] = true;
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Water" && tagName == "")
        {
            GManager.instance.setrg = 10;
        }
        if (col.tag == tagName && onAction == true)
        {
            ColTrigger = false;
            if (managerTrg)
            {
                GManager.instance.colTrg[managerIndex] = false;
            }
        }
        else if (tagName == "" && onAction == true && col.tag != "Player" && col.tag != "OnMask" && col.tag != "enemy" && col.tag != "noactive" && col.tag != "Water" && col.tag != "pbullet" && col.tag != "ebullet" && col.tag != "npc") 
        {
            ColTrigger = false;
            if (managerTrg)
            {
                GManager.instance.colTrg[managerIndex] = false;
            }
        }
    }
}
