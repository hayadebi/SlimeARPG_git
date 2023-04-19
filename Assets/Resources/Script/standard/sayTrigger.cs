using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sayTrigger : MonoBehaviour
{
    public bool saystop = false;
    private float saytime = 0;
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        if(saystop)
        {
            saytime += Time.deltaTime;
            if(saytime > 5)
            {
                saytime = 0;
                saystop = false;
            }
        }
    }
}
