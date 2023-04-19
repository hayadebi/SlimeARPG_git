using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUI : MonoBehaviour
{
    public bool E_trg = false;
    bool ongettrg = false;
    public GameObject ui;
    public bool Retuntrg = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Retuntrg == true && ongettrg == true & Input.GetKeyDown(KeyCode.Escape))
        {
            ongettrg = false;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && ongettrg == false && GManager.instance.walktrg == true && E_trg == false)
        {
            ongettrg = true;

            GManager.instance.walktrg = false;
            Instantiate(ui, transform.position, transform.rotation);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && ongettrg == false && GManager.instance.walktrg == true && E_trg == true && Input.GetKeyDown (KeyCode.E))
        {
            ongettrg = true;
            GManager.instance.walktrg = false;
            Instantiate(ui, transform.position, transform.rotation);
        }
    }
}
