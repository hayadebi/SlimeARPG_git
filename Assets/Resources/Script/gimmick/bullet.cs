using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int senumber = -1;
    public int destroyEvent = -1;
    public float destroyTime = 1.6f;
    public bool pbullet = false;
    public GameObject bossboom = null;
    public string returnObj = "";
    public float returntime = 1f;
    public float returnspeed = 24;
    private bool returntrg = false;
    [Header("イベントに使うオブジェクト")] public GameObject obj;
    public bool _startShot = false;
    // Start is called before the first frame update
    void Start()
    {
        if (_startShot)
        {
            Invoke("startShot", 0.1f);
        }
        Invoke("Gdestroy", destroyTime);
    }
    void startShot()
    {
        GameObject obj = GameObject.Find("Player");
        if (obj != null)
        {
            var rotation = Quaternion.LookRotation(obj.transform.position - this.transform.position);
            rotation.x = 0;
            rotation.z = 0;
            this.transform.rotation = rotation;

            Vector3 vec = obj.transform.position - this.transform.position;
            vec.Normalize();
            vec = Quaternion.Euler(0, 0, 0) * vec;
            vec *= 200;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().velocity = vec;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -30 || this.transform.position.y > 2000 || this.transform.position.x < -4000 || this.transform.position.x > 4000 || this.transform.position.z < -4000 || this.transform.position.z > 4000)
        {
            Destroy(gameObject);
        }
        else if (returnObj != "" && returntrg == false)
        {
            returntime -= Time.deltaTime;
            if(returntime < 0)
            {
                returntrg = true;
                GameObject obj = GameObject.Find(returnObj);
                if(obj != null)
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

    private void OnTriggerEnter(Collider col)
    {
        if (bossboom == null)
        {
            if (col.tag == "wall")
            {
                GManager.instance.setrg = 3;
                Instantiate(GManager.instance.effectobj[1], this.transform.position, this.transform.rotation);
                if (destroyEvent != -1)
                {
                    dsEvent();
                }
                Destroy(gameObject);
            }
        }
        else if(bossboom != null)
        {
            if ( col.tag == "wall")
            {
                if (senumber != -1)
                {
                    GManager.instance.setrg = senumber;
                }
                Instantiate(bossboom, this.transform.position, bossboom.transform.rotation);
                Destroy(gameObject,0.1f);
            }
        }
    }
    void Gdestroy()
    {
        GManager.instance.setrg = 3;
        Instantiate(GManager.instance.effectobj[2], this.transform.position, this.transform.rotation);
        Destroy(gameObject, 0.1f);
    }
    void dsEvent()
    {
        if(destroyEvent == 1)
        {
            
        }
    }
}
