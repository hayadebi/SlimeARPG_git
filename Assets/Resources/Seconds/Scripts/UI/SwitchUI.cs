using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUI : MonoBehaviour
{
    public GameObject EnableObject;
    public GameObject DisableObject;
    public int checkTrg = 1;
    public int changeTrg = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SwitchObject()
    {
        if (DataManager.instance.Triggers[4] == checkTrg)
        {
            DataManager.instance.Triggers[4] = changeTrg;
            GManager.instance.setrg = 6;
            EnableObject.SetActive(true);
            DisableObject.SetActive(false);
        }
        else GManager.instance.setrg = 2;
    }
}
