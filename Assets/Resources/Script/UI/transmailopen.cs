using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transmailopen : MonoBehaviour
{
    public ChildMail targetchild;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChildClick()
    {
        if (targetchild != null) targetchild.MailBonus();
    }
}
