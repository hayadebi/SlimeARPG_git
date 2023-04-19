using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startDestroy : MonoBehaviour
{
    public string objName;
    GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find(objName);
        if(obj != null)
        {
            Destroy(obj.gameObject,0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
