using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startSetTrans : MonoBehaviour
{
    [System.Serializable]
    public struct transobj
    {
        public Transform trans;
        public Vector3 pos;
        public Vector3 rot;
    }
    public transobj[] target;

    // Start is called before the first frame update
    void Start()
    {
        if(target != null && target.Length != 0)
        {
            for(int i = 0; i< target.Length;)
            {
                target[0].trans.parent = null;
                if(target[0].pos.x != 0 || target[0].pos.y != 0 || target[0].pos.z != 0)
                {
                    target[0].trans.position = target[0].pos;
                }
                if (target[0].rot.x != 0 || target[0].rot.y != 0 || target[0].rot.z != 0)
                {
                    target[0].trans.eulerAngles = target[0].rot;
                }
                i++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
