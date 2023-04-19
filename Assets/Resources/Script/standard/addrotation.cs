using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addrotation : MonoBehaviour
{
    public float rotspeed = 1;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rotspeed *= 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GManager.instance.walktrg && !GManager.instance.over && GManager.instance.bossbattletrg == 0)
        {
            //rb.AddTorque(transform.up * rotspeed, ForceMode.Impulse);
            //Vector3 rotvec = transform.up * rotspeed;
            //Quaternion deltaR = Quaternion.Euler(rotvec * Time.deltaTime);
            rb.AddTorque(Vector3.up * rotspeed ,ForceMode.Impulse );
            //Vector3 rotvec = Vector3.up * rotspeed ;
            //rb.MoveRotation(Quaternion.Euler(rotvec * Time.deltaTime));
            //rb.rotation = deltaR; 
            //rb.angularVelocity = Vector3.up * rotspeed;
        }
    }
        
}
