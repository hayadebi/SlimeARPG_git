using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataBuySystem : MonoBehaviour
{
    public int get_buytype = 0;
    public Text check_text;
    private GameObject jackshop = null;
    private StoreManager storem;
    private float buyprice = 0f;
    private string buyname="";
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("online-jackshop(Clone)")) jackshop = GameObject.Find("online-jackshop(Clone)");
        if (jackshop != null && jackshop.GetComponent<StoreManager>()) storem = jackshop.GetComponent<StoreManager>();
        //文章改変・翻訳
        Invoke(nameof(UpdateText), 0.1f);
    }
    void UpdateText()
    {
        if (get_buytype == 0)
        {
            buyprice = GManager.instance.ItemID[GManager.instance.select_buyid].itemprice / 500f;
            if (GManager.instance.isEnglish == 0)
            {
                buyname = GManager.instance.ItemID[GManager.instance.select_buyid].itemname;
                check_text.text = buyprice.ToString()+"デビコイン消費して【"+buyname+"】\nを購入しようとしています。\n貴重なデビコインを消費して本当に購入しますか？";
            }
            else
            {
                buyname = GManager.instance.ItemID[GManager.instance.select_buyid].itemname2;
                check_text.text = "I am about to spend "+buyprice.ToString()+" devicoins \nto purchase 【"+buyname+"】.\n Do you really want to spend your precious \ndevicoins to purchase it?";
            }

        }
        else if (get_buytype == 1)
        {
            buyprice = GManager.instance.ItemID[GManager.instance._craftRecipe[GManager.instance.select_buyid].craftItem_id].itemprice / 250f;
            if (GManager.instance.isEnglish == 0)
            {
                buyname = GManager.instance.ItemID[GManager.instance._craftRecipe[GManager.instance.select_buyid].craftItem_id].itemname;
                check_text.text = buyprice.ToString() + "デビコイン消費して【" + buyname + "のレシピ】\nを購入しようとしています。\n貴重なデビコインを消費して本当に購入しますか？";
            }
            else
            {
                buyname = GManager.instance.ItemID[GManager.instance._craftRecipe[GManager.instance.select_buyid].craftItem_id].itemname2;
                check_text.text = "I am about to spend " + buyprice.ToString() + " devicoins \nto purchase 【Recipe for" + buyname + "】.\n Do you really want to spend your precious \ndevicoins to purchase it?";
            }
            
        }
    }
    public void BuyBtn()
    {
        if (storem != null)
        {
            GManager.instance.setrg = 36;
            storem.BuyAddData(buyprice);
            if (get_buytype == 0)
            {
                GManager.instance.ItemID[GManager.instance.select_buyid].itemnumber += 1;
                GManager.instance.ItemID[GManager.instance.select_buyid].gettrg = 1;
            }
            else if (get_buytype == 1)
            {
                GManager.instance._craftRecipe[GManager.instance.select_buyid].get_recipe = 1;
                PlayerPrefs.SetInt("get_recipe" + GManager.instance.select_buyid, 1);
                PlayerPrefs.Save();
            }
            GManager.instance.ESCtrg = true;
        }
    }
}
