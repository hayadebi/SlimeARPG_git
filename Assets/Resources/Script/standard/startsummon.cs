using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startsummon : MonoBehaviour
{
    public GameObject UI;
    public float starttime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(SummonUI), starttime);
    }
    void SummonUI()
    {
        GManager.instance.setrg = 6;
        Instantiate(UI, transform.position, transform.rotation);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
