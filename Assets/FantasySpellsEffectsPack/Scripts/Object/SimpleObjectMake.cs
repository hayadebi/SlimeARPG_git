using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectMake : _ObjectMakeBase {

    public Vector3 m_randomRotationValue;
    public bool newRotTrg = true;
    void Start(){
        for(int i = 0; i < m_makeObjs.Length; i++)
        { 
            GameObject m_obj = Instantiate(m_makeObjs[i], this.transform.position, this.transform.rotation);
            m_obj.transform.parent = this.transform;
            if (newRotTrg)
            {
                m_obj.transform.rotation *= Quaternion.Euler(GetRandomVector(m_randomRotationValue));
            }
            if (m_movePos){ 
                if(m_obj.GetComponent<MoveToObject>()){ 
                    MoveToObject m_script = m_obj.GetComponent<MoveToObject>();
                    m_script.m_movePos = m_movePos;
                }
            }
        }
    }
}
