using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public int targetId = 5;
    public int checkTrigger = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag != "NoneTarget" && DataManager.instance.Triggers[targetId] != checkTrigger) this.gameObject.tag = "NoneTarget";
        else if (this.gameObject.tag != "TargetPoint" && DataManager.instance.Triggers[targetId] == checkTrigger) this.gameObject.tag = "TargetPoint";
    }
}
