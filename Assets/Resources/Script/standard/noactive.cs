using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noactive : MonoBehaviour
{
    bool ontrg = true;
    Canvas can;
    // Start is called before the first frame update
    void Start()
    {
        can = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1) && ontrg == true)
        {
            ontrg = false;
            can.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.F1) && ontrg == false)
        {
            ontrg = true;
            can.enabled = true;
        }
    }
}
