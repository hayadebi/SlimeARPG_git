using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System.IO;
using System;
public class devconvert : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip getse;
    public AudioClip notse;
    public int inputshopID;
    public Sprite nullsprite;
    public GameObject jackshop = null;
    public StoreManager storem;
    private double selectprice;
    private float cooltime;
    public GameObject BuyCheckUI;
    public Text getdevcoin_text;
    public Text getnormalcoin_text;
    public Text getdevnugget_text;
    public Text getreplica_text;
    public Transform parentobj = null;
    // Start is called before the first frame update
    void Start()
    {
        if (parentobj == null) parentobj = this.transform;
        if (GameObject.Find("online-jackshop(Clone)")) jackshop = GameObject.Find("online-jackshop(Clone)");
        if (jackshop != null && jackshop.GetComponent<StoreManager>()) storem = jackshop.GetComponent<StoreManager>();
        
        if (inputshopID==0) selectprice = 20;
        else if (inputshopID == 1) selectprice = 1;
        else if (inputshopID == 2) selectprice = 0.5;
        SetUIConvert();
    }
    void SetUIConvert()
    {
        getdevcoin_text.text = GManager.instance.get_devcoin.ToString() + "DC×";
        getnormalcoin_text.text = GManager.instance.Coin.ToString() + "G×";
        getdevnugget_text.text = GManager.instance.ItemID[62].itemnumber.ToString() + "original×";
        getreplica_text.text = GManager.instance.ItemID[63].itemnumber.ToString() + "個所持中 ×";
    }
    // Update is called once per frame
    void Update()
    {
        ;
    }
    public void ShopPlay()
    {
        if (inputshopID == 0 && cooltime <= 0 && selectprice <= GManager.instance.ItemID[62].itemnumber)
        {
            cooltime = 99f;
            audioS.PlayOneShot(getse);
            GManager.instance.setmenu += 1;
            //処理
            GameObject tmpobj = Instantiate(BuyCheckUI, parentobj.transform.position, parentobj.transform.rotation, parentobj.transform);
            GManager.instance.select_buyid = inputshopID;
            tmpobj.GetComponent<DataBuySystem>().get_buytype = 3;

        }
        else if (inputshopID == 1 && cooltime <= 0 && selectprice <= GManager.instance.get_devcoin)
        {
            cooltime = 99f;
            audioS.PlayOneShot(getse);
            GManager.instance.setmenu += 1;
            //処理
            GameObject tmpobj = Instantiate(BuyCheckUI, parentobj.transform.position, parentobj.transform.rotation, parentobj.transform);
            GManager.instance.select_buyid = inputshopID;
            tmpobj.GetComponent<DataBuySystem>().get_buytype = 4;

        }
        else if (inputshopID == 2 && cooltime <= 0 && selectprice <= GManager.instance.get_devcoin)
        {
            cooltime = 99f;
            audioS.PlayOneShot(getse);
            GManager.instance.setmenu += 1;
            //処理
            GameObject tmpobj = Instantiate(BuyCheckUI, parentobj.transform.position, parentobj.transform.rotation, parentobj.transform);
            GManager.instance.select_buyid = 63;
            tmpobj.GetComponent<DataBuySystem>().get_buytype = 5;

        }
        else
        {
            GManager.instance.setrg = 27;
        }
    }
}
