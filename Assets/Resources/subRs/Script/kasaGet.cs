using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kasaGet : MonoBehaviour
{
    public GameObject kasaMain = null;
    private bool Trg = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider col)
    {
     if(col.tag == "Player" && Input.GetKey(KeyCode.Return) && Trg == false)
        {
            Trg = true;
            var killef = Instantiate(GManager.instance.effectobj[3], col.gameObject.transform.position, col.gameObject.transform.rotation);
            var scaleef = killef.gameObject.transform.localScale;
            scaleef /= 2;
            killef.gameObject.transform.localScale = scaleef;
            GManager.instance.setrg = 29;
            kasaMain.SetActive(true);
            Destroy(gameObject, 0.1f);
        }
    }
}
