using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseArea : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "enemy")
        {
            GManager.instance.mouseP = col.gameObject.transform.position;
        }
    }
}
