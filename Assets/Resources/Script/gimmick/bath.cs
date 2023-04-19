using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bath : MonoBehaviour
{
    public int cureNumber = 1;
    public AudioSource audioS;
    public AudioClip se;
    public float cureTime = 0.15f;
    private float inputTime;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player" && GManager.instance.Pstatus[GManager.instance.playerselect].maxHP > GManager.instance.Pstatus[GManager.instance.playerselect].hp)
        {
            inputTime += Time.deltaTime;
            if(inputTime >= cureTime)
            {
                inputTime = 0;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += cureNumber;
                audioS.PlayOneShot(se);
                if(GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
            }
        }
    }
}
