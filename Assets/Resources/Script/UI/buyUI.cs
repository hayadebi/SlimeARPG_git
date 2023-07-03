using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System.IO;
using System;
public class buyUI : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip selectse;
    public AudioClip notse;
    public AudioClip buyse;
    public Text[] itemname;
    public Text[] itemscript;
    public int[] itemID;
    int boxnumber = 0;
    int addnumber = 0;
    public int selectnumber = 0;
    public Image[] itemsprite;
    public Text[] buttonText;
    public Sprite nullsprite;
    private int oldbuycoin;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; GManager.instance.ItemID.Length > i;)
        {
            if (GManager.instance.ItemID[i].itemnumber > 0)
            {
                boxnumber += 1;
            }
            i += 1;
        }
        itemID = new int[boxnumber];
        if (boxnumber != 0)
        {
            for (int i = 0; GManager.instance.ItemID.Length > i;)
            {
                if (GManager.instance.ItemID[i].itemnumber > 0)
                {
                        itemID[addnumber] = i;
                        addnumber += 1;
                }
                i += 1;
            }
        }
        SetUI();
    }
    void SetUI()
    {
        for (int i = 0; i < 4;)
        {
            if (itemID == null || GManager.instance.ItemID.Length < i + selectnumber)
            {
                itemname[i].text = "??????";
                itemscript[i].text = "???? ???? ??";
                buttonText[i].text = "????";
                itemsprite[i].sprite = nullsprite;
            }
            else if (itemID != null && itemID.Length > (i + selectnumber) && GManager.instance.ItemID[itemID[i + selectnumber]].itemnumber > 0)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    itemname[i].text = GManager.instance.ItemID[itemID[i + selectnumber]].itemname;
                    itemsprite[i].sprite = GManager.instance.ItemID[itemID[i + selectnumber]].itemimage;
                    oldbuycoin = (GManager.instance.ItemID[itemID[i + selectnumber]].itemprice / 3) * 2;
                    if (GManager.instance.ItemID[itemID[i + selectnumber]].itemprice < 9999)
                    {
                        buttonText[i].text = "売る";
                        itemscript[i].text = "買取値：" + oldbuycoin + "　所持数：" + GManager.instance.ItemID[itemID[i + selectnumber]].itemnumber + "　レア度：" + GManager.instance.ItemID[itemID[i + selectnumber]].rare;
                    }
                    else
                    {
                        buttonText[i].text = "買取不可";
                        itemscript[i].text = "買取値：??" + "　所持数：" + GManager.instance.ItemID[itemID[i + selectnumber]].itemnumber + "　レア度：" + GManager.instance.ItemID[itemID[i + selectnumber]].rare;
                    }
                }
                
                else if (GManager.instance.isEnglish == 1)
                {
                    itemname[i].text = GManager.instance.ItemID[itemID[i + selectnumber]].itemname2;
                    itemsprite[i].sprite = GManager.instance.ItemID[itemID[i + selectnumber]].itemimage;
                    itemscript[i].fontSize = 18;
                    if (GManager.instance.ItemID[itemID[i + selectnumber]].itemprice != 9999)
                    {
                        buttonText[i].text = "Sell";
                        itemscript[i].text = "Purchase price：" + GManager.instance.ItemID[itemID[i + selectnumber]].itemprice + "　Number：" + GManager.instance.ItemID[itemID[i + selectnumber]].itemnumber + "　Rarity：" + GManager.instance.ItemID[itemID[i + selectnumber]].rare;
                    }
                    else
                    {
                        buttonText[i].text = "Not for sale";
                        itemscript[i].text = "Purchase price：??" + "　Number：" + GManager.instance.ItemID[itemID[i + selectnumber]].itemnumber + "　Rarity：" + GManager.instance.ItemID[itemID[i + selectnumber]].rare;
                    }
                }
            }
            else
            {
                itemname[i].text = "??????";
                itemscript[i].text = "???? ???? ??";
                buttonText[i].text = "????";
                itemsprite[i].sprite = nullsprite;
            }
            i++;
        }
    }
    public void SelectMinus()
    {
        if (selectnumber >= 4)
        {
            audioS.PlayOneShot(selectse);
            selectnumber -= 4;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SelectPlus()
    {
        if (selectnumber + 4 < (GManager.instance.ItemID.Length))
        {
            audioS.PlayOneShot(selectse);
            selectnumber += 4;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void Sell_1()
    {
        if (itemID == null || itemID.Length <= 0 + selectnumber )
        {
            audioS.PlayOneShot(notse);
        }
        else if (itemID != null && itemID.Length > (0 + selectnumber) && GManager.instance.ItemID[itemID[0 + selectnumber]].itemnumber > 0 && GManager.instance.ItemID[itemID[0 + selectnumber]].itemprice < 9999 )
        {
            //--------闇市へ追加--------
            // クラスのNCMBObjectを作成
            var tmpidnum = itemID[0 + selectnumber];
            string tmpdaystext = GManager.instance.tmpdays.Year.ToString() + "/" + GManager.instance.tmpdays.Month.ToString() + "/" + GManager.instance.tmpdays.Day.ToString();
            NCMBObject testClass = new NCMBObject("BlackMarket");
            // オブジェクトに値を設定
            testClass["itemname"] = GManager.instance.ItemID[tmpidnum].itemname;
            testClass["itemid"] = tmpidnum;
            testClass["buyday"] = tmpdaystext;
            testClass["datagroup"] = "useritems";
            // データストアへの登録
            testClass.SaveAsync();
            //--------------------------
            GManager.instance.ItemID[itemID[0 + selectnumber]].itemnumber -= 1;
            oldbuycoin = (GManager.instance.ItemID[itemID[0 + selectnumber]].itemprice / 3) * 2;
            GManager.instance.Coin += oldbuycoin;
            audioS.PlayOneShot(buyse);

            PlayerPrefs.SetInt("coin", GManager.instance.Coin);
            PlayerPrefs.SetInt("itemnumber" + tmpidnum, GManager.instance.ItemID[tmpidnum].itemnumber);
            PlayerPrefs.SetInt("itemget" + tmpidnum, GManager.instance.ItemID[tmpidnum].gettrg);
            PlayerPrefs.Save();
            SetUI();
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void Sell_2()
    {
        if (itemID == null || itemID.Length <= 1 + selectnumber)
        {
            audioS.PlayOneShot(notse);
        }
        else if (itemID != null && itemID.Length > (1 + selectnumber) && GManager.instance.ItemID[itemID[1 + selectnumber]].itemnumber > 0 && GManager.instance.ItemID[itemID[1 + selectnumber]].itemprice < 9999)
        {
            //--------闇市へ追加--------
            // クラスのNCMBObjectを作成
            var tmpidnum = itemID[1 + selectnumber];
            string tmpdaystext = GManager.instance.tmpdays.Year.ToString() + "/" + GManager.instance.tmpdays.Month.ToString() + "/" + GManager.instance.tmpdays.Day.ToString();
            NCMBObject testClass = new NCMBObject("BlackMarket");
            // オブジェクトに値を設定
            testClass["itemname"] = GManager.instance.ItemID[tmpidnum].itemname;
            testClass["itemid"] = tmpidnum;
            testClass["buyday"] = tmpdaystext;
            testClass["datagroup"] = "useritems";
            // データストアへの登録
            testClass.SaveAsync();
            //--------------------------
            GManager.instance.ItemID[itemID[1 + selectnumber]].itemnumber -= 1;
            oldbuycoin = (GManager.instance.ItemID[itemID[1 + selectnumber]].itemprice / 3) * 2;
            GManager.instance.Coin += oldbuycoin;
            audioS.PlayOneShot(buyse);

            PlayerPrefs.SetInt("coin", GManager.instance.Coin);
            PlayerPrefs.SetInt("itemnumber" + tmpidnum, GManager.instance.ItemID[tmpidnum].itemnumber);
            PlayerPrefs.SetInt("itemget" + tmpidnum, GManager.instance.ItemID[tmpidnum].gettrg);
            PlayerPrefs.Save();
            SetUI();
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void Sell_3()
    {
        if (itemID == null || itemID.Length <= 2 + selectnumber)
        {
            audioS.PlayOneShot(notse);
        }
        else if (itemID != null && itemID.Length > (2 + selectnumber) && GManager.instance.ItemID[itemID[2 + selectnumber]].itemnumber > 0 && GManager.instance.ItemID[itemID[2 + selectnumber]].itemprice < 9999)
        {
            //--------闇市へ追加--------
            // クラスのNCMBObjectを作成
            var tmpidnum = itemID[2 + selectnumber];
            string tmpdaystext = GManager.instance.tmpdays.Year.ToString() + "/" + GManager.instance.tmpdays.Month.ToString() + "/" + GManager.instance.tmpdays.Day.ToString();
            NCMBObject testClass = new NCMBObject("BlackMarket");
            // オブジェクトに値を設定
            testClass["itemname"] = GManager.instance.ItemID[tmpidnum].itemname;
            testClass["itemid"] = tmpidnum;
            testClass["buyday"] = tmpdaystext;
            testClass["datagroup"] = "useritems";
            // データストアへの登録
            testClass.SaveAsync();
            //--------------------------
            GManager.instance.ItemID[itemID[2 + selectnumber]].itemnumber -= 1;
            oldbuycoin = (GManager.instance.ItemID[itemID[2 + selectnumber]].itemprice / 3) * 2;
            GManager.instance.Coin += oldbuycoin;
            audioS.PlayOneShot(buyse);

            PlayerPrefs.SetInt("coin", GManager.instance.Coin);
            PlayerPrefs.SetInt("itemnumber" + tmpidnum, GManager.instance.ItemID[tmpidnum].itemnumber);
            PlayerPrefs.SetInt("itemget" + tmpidnum, GManager.instance.ItemID[tmpidnum].gettrg);
            PlayerPrefs.Save();
            SetUI();
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void Sell_4()
    {
        if (itemID == null || itemID.Length <= 3 + selectnumber)
        {
            audioS.PlayOneShot(notse);
        }
        else if (itemID != null && itemID.Length > (3 + selectnumber) && GManager.instance.ItemID[itemID[3 + selectnumber]].itemnumber > 0 && GManager.instance.ItemID[itemID[3 + selectnumber]].itemprice < 9999)
        {
            //--------闇市へ追加--------
            // クラスのNCMBObjectを作成
            var tmpidnum = itemID[3 + selectnumber];
            string tmpdaystext = GManager.instance.tmpdays.Year.ToString() + "/" + GManager.instance.tmpdays.Month.ToString() + "/" + GManager.instance.tmpdays.Day.ToString();
            NCMBObject testClass = new NCMBObject("BlackMarket");
            // オブジェクトに値を設定
            testClass["itemname"] = GManager.instance.ItemID[tmpidnum].itemname;
            testClass["itemid"] = tmpidnum;
            testClass["buyday"] = tmpdaystext;
            testClass["datagroup"] = "useritems";
            // データストアへの登録
            testClass.SaveAsync();
            //--------------------------
            GManager.instance.ItemID[itemID[3 + selectnumber]].itemnumber -= 1;
            oldbuycoin = (GManager.instance.ItemID[itemID[3 + selectnumber]].itemprice / 3) * 2;
            GManager.instance.Coin += oldbuycoin;
            audioS.PlayOneShot(buyse);

            PlayerPrefs.SetInt("coin", GManager.instance.Coin);
            PlayerPrefs.SetInt("itemnumber" + tmpidnum, GManager.instance.ItemID[tmpidnum].itemnumber);
            PlayerPrefs.SetInt("itemget" + tmpidnum, GManager.instance.ItemID[tmpidnum].gettrg);
            PlayerPrefs.Save();
            SetUI();
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }

}
