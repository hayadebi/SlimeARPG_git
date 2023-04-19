using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmrRot : MonoBehaviour
{
    private Vector3 vec;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GManager.instance.walktrg && GManager.instance.setmenu < 1 && !GManager.instance.over )
        {
            if(!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.E))
            {
                vec = this.transform.eulerAngles;
                vec.y += GManager.instance.rotpivot;
                if(vec.y > 360 ||vec.y < -360)
                {
                    vec.y = 0;
                }
                this.transform.eulerAngles = vec;
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Q))
            {
                vec = this.transform.eulerAngles;
                vec.y -= GManager.instance.rotpivot;
                if (vec.y > 360 || vec.y < -360)
                {
                    vec.y = 0;
                }
                this.transform.eulerAngles = vec;
            }
        }
    }
}
