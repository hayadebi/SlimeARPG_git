using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System.IO;
using System;

public class DataBuySystem : MonoBehaviour
{
    public int get_buytype = 0;
    public Text check_text;
    private GameObject jackshop = null;
    private StoreManager storem;
    private float buyprice = 0f;
    private string buyname="";
    public NCMBObject deltarget_data = null;

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
            buyprice = GManager.instance.ItemID[GManager.instance.select_buyid].itemprice / 400f;
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
            buyprice = GManager.instance.ItemID[GManager.instance._craftRecipe[GManager.instance.select_buyid].craftItem_id].itemprice / 100f;
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
        else if (get_buytype == 2)
        {
            buyprice = GManager.instance.MagicID[GManager.instance.select_buyid].magicprice / 100f;
            if (GManager.instance.isEnglish == 0)
            {
                buyname = GManager.instance.MagicID[GManager.instance.select_buyid].magicname;
                check_text.text = buyprice.ToString() + "デビコイン消費して【" + buyname + "】\nを選択スライムに習得しようとしています。\n貴重なデビコインを消費して本当に購入しますか？";
            }
            else
            {
                buyname = GManager.instance.MagicID[GManager.instance.select_buyid].magicname2;
                check_text.text = buyprice.ToString() + " I am trying \nto learn  【" + buyname + "】\n on select slimes by consuming devi-coins. \nDo I really want to spend my precious devi-coins to purchase it?";
            }

        }
    }
    public void BuyBtn()
    {
        if (storem != null)
        {
            GManager.instance.setrg = 36;
            storem.BuyAddData(-buyprice);
            if (deltarget_data != null)
            {
                NCMBObject objDelete = deltarget_data;
                objDelete.DeleteAsync();
                deltarget_data = null;
            }
            if (get_buytype == 0)
            {
                GManager.instance.ItemID[GManager.instance.select_buyid].itemnumber += 1;
                GManager.instance.ItemID[GManager.instance.select_buyid].gettrg = 1;

                PlayerPrefs.SetInt("itemnumber" + GManager.instance.select_buyid, GManager.instance.ItemID[GManager.instance.select_buyid].itemnumber);
                PlayerPrefs.SetInt("itemget" + GManager.instance.select_buyid, GManager.instance.ItemID[GManager.instance.select_buyid].gettrg);
                PlayerPrefs.Save();
            }
            else if (get_buytype == 1)
            {
                GManager.instance._craftRecipe[GManager.instance.select_buyid].get_recipe = 1;
                PlayerPrefs.SetInt("get_recipe" + GManager.instance.select_buyid, 1);
                PlayerPrefs.Save();
            }
            else if (get_buytype == 2)
            {
                for (int i = 0; i < GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length;)
                {
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].magicid == GManager.instance.select_buyid && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].gettrg < 1)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].gettrg = 1;
                        i = GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length;
                    }
                    i++;
                }
                for (int i = 0; i < GManager.instance.Pstatus.Length;)
                {
                    for (int j = 0; j < GManager.instance.Pstatus[i].getMagic.Length;)
                    {
                        PlayerPrefs.SetInt("pgetmagictrg" + i + "" + j, GManager.instance.Pstatus[i].getMagic[j].gettrg);
                        j++;
                    }
                    i++;
                }
                PlayerPrefs.Save();
            }
            GManager.instance.ESCtrg = true;
        }
    }
}
