using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColAudio : MonoBehaviour
{
    public ColEvent objcol;
    public AudioSource audioS;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(objcol.ColTrigger == true && audioS.enabled == false)
        {
            audioS.enabled = true;
        }
        else if (objcol.ColTrigger == false && audioS.enabled == true)
        {
            audioS.enabled = false;
        }
    }
}
