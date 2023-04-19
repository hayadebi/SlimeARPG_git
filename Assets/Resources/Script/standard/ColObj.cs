using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColObj : MonoBehaviour
{
    public ColEvent objcol;
    public GameObject switchobj;
    private bool settrg = false;
    // Start is called before the first frame update
    private void Awake()
    {
        switchobj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (objcol.ColTrigger == true && settrg == false)
        {
            settrg = true;
            switchobj.SetActive(false);
        }
        else if (objcol.ColTrigger == false && settrg == true)
        {
            settrg = false;
            switchobj.SetActive(true);
        }
    }
}
