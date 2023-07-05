using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsUi : MonoBehaviour
{
    public float max_adstime = 15f;
    private int realtime_texttime=0;
    private float realtime_ads = 0f;
    public Text btn_counttext;
    public Text tips_text;
    public Button btn_close;
    public GameObject jackshop = null;
    public StoreManager storem;
    private bool gettrg = false;
    // Start is called before the first frame update
    void Start()
    {
        btn_close.enabled = false;
        if (GameObject.Find("online-jackshop(Clone)")) jackshop = GameObject.Find("online-jackshop(Clone)");
        if (jackshop != null && jackshop.GetComponent<StoreManager>()) storem = jackshop.GetComponent<StoreManager>();
        if (GManager.instance.isEnglish == 0) btn_counttext.fontSize=20;
        else btn_counttext.fontSize=18;
        //ヒント
        if (GManager.instance.isEnglish == 0) tips_text.text = GManager.instance.adstips[Random.Range(0,GManager.instance.adstips.Length)].jp_tips;
        else tips_text.text = GManager.instance.adstips[Random.Range(0, GManager.instance.adstips.Length)].en_tips;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gettrg && realtime_ads < max_adstime)
        {
            if (!btn_close.enabled) btn_close.enabled = true;
            if (gettrg) gettrg = false;
             realtime_ads += Time.deltaTime;
            if (realtime_texttime != (int)realtime_ads)
            {
                realtime_texttime = (int)realtime_ads;
                if(GManager.instance.isEnglish==0)btn_counttext.text = "報酬獲得まで残り"+(max_adstime-realtime_texttime).ToString() + "秒";
                else btn_counttext.text = (max_adstime - realtime_texttime).ToString() + " seconds left.";
            }
        }
        else if (!gettrg)
        {
            gettrg = true;
            GManager.instance.setrg = 12;
            realtime_ads = 0f;
            if (GManager.instance.isEnglish == 0) btn_counttext.text = "0.1デビコインGET！";
            else btn_counttext.text = (max_adstime - realtime_texttime).ToString() + "Got my 0.1DC!";
            storem.BuyAddData(0.1f);
            btn_close.enabled = true;
        }
    }
}
