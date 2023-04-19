using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColAnim : MonoBehaviour
{
    public Animator anim;
    public string colname = "Player";
    public string startanim = "";
    public string endanim = "";
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == colname )
        {
            anim.Play(startanim);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == colname)
        {
            anim.Play(endanim);
        }
    }
}
