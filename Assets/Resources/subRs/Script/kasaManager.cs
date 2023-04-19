using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kasaManager : MonoBehaviour
{
    public girlPlayer glpy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(glpy != null && this.tag == "kill" && glpy.eatTrg == false)
        {
            this.tag = "OnMask";
        }
        else if (glpy != null && this.tag != "kill" && glpy.eatTrg == true)
        {
            this.tag = "kill";
        }
    }
}
