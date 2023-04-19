using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkMagic : MonoBehaviour
{
    private bool settrg = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GManager.instance.walktrg == true && GManager.instance.setmenu < 1 && settrg == false)
        {
            settrg = true;
            Invoke("magicPlay", 0.3f);
        }
    }

    void magicPlay()
    {
        Instantiate(GManager.instance.MagicID[GManager.instance.itemMagicID].magicobj, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject, 0.3f);
    }
}
