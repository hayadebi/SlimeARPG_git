using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TempTextUI : MonoBehaviour
{
    public Animator animC;
    public Text tempText;
    private string oldText="";
    // Start is called before the first frame update
    void Start()
    {
        animC.SetInteger("trg", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(DataManager.instance.TextGet!=""&& DataManager.instance.TextGet != oldText)
        {
            oldText = DataManager.instance.TextGet;
            tempText.text = DataManager.instance.TextGet;
            GManager.instance.setrg = 37;
            StartCoroutine(animMove(DataManager.instance.TextGet));
        }
    }

    IEnumerator animMove(string waittext)
    {
        animC.SetInteger("trg", 1);
        yield return new WaitForSeconds(3f);
        animC.SetInteger("trg", 0);
        if(DataManager.instance.TextGet == waittext)
        {
            DataManager.instance.TextGet = "";
            oldText = "";
        }
    }
}
