using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DemoView()
    {
        GManager.instance.setrg = 6;
        DataManager.instance.TextGet = "そこの項目は現在開発中の要素です！";
    }
}
