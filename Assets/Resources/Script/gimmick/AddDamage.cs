using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDamage : MonoBehaviour
{
    public enemyS es = null;
    public int Damage = 1;
    public string attacktype = "無属性";
    public bool nokill = false;
    public int outputEvent = -1;
    public string returnObj = "";
    public float returntime = 1;
    public float returnspeed;
    private bool returntrg = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(es != null && Damage != es.Estatus.attack )
        {
            Damage = es.Estatus.attack;
        }
        if (returnObj != "" && returntrg == false)
        {
            returntime -= Time.deltaTime;
            if (returntime < 0)
            {
                returntrg = true;
                GameObject obj = GameObject.Find(returnObj);
                if (obj != null)
                {
                    Vector3 vec = obj.transform.position - this.transform.position;
                    vec.Normalize();
                    vec = Quaternion.Euler(0, 0, 0) * vec;
                    vec *= returnspeed;
                    this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    this.GetComponent<Rigidbody>().velocity = vec;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
