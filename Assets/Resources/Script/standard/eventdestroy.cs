using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventdestroy : MonoBehaviour
{
    public int eventnumber;
    public int inputEvent = -1;
    [Header("1はe!=i,2はe>i,3はi>e,4はi==else")]public int inputmode = 1;//1はe!=i,2はe>i,3はi>e;
    // Start is called before the first frame update
    void Start()
    {
        GManager.instance.EventNumber[eventnumber] = PlayerPrefs.GetInt("EvN" + eventnumber, 0);
        if (inputEvent != -1)
        {
            if(inputEvent != GManager.instance.EventNumber[eventnumber] && inputmode == 1)
            {
                setDestroy();
            }
            else if (inputEvent < GManager.instance.EventNumber[eventnumber] && inputmode == 2)
            {
                setDestroy();
            }
            else if (inputEvent > GManager.instance.EventNumber[eventnumber] && inputmode == 3)
            {
                setDestroy();
            }
            else if (inputEvent == GManager.instance.EventNumber[eventnumber] && inputmode == 4)
            {
                setDestroy();
            }
        }
    }
    void setDestroy()
    {
            Destroy(gameObject,0.1f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
