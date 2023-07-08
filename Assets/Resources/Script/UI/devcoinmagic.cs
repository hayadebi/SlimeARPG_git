using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System.IO;
using System;
public class devcoinmagic : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip getse;
    public AudioClip notse;
    public int inputshopID;
    public Sprite nullsprite;
    public Text magicname;
    public Image magicsprite;
    public Text magicscript;
    public Text magicprice;
    public Text magicinputget;
    public Image slimesprite;
    public Text slimename;
    bool pushtrg = false;
    float pushtime = 0;
    private bool notget = true;
    public GameObject jackshop = null;
    public StoreManager storem;
    private double selectprice;
    private float cooltime;
    public GameObject BuyCheckUI;
    public Text getdevcoin_text;
    private string shop_name = "devcoin_magic";
    private int shopYear;
    private int shopMonth;
    private int shopDay;
    private DateTime checkday;
    private int shopID_0;
    private int shopID_1;
    private int shopID_2;
    public Transform parentobj=null;
    // Start is called before the first frame update
    void Start()
    {
        if (parentobj == null) parentobj = this.transform;
        if (GameObject.Find("online-jackshop(Clone)")) jackshop = GameObject.Find("online-jackshop(Clone)");
        if (jackshop != null && jackshop.GetComponent<StoreManager>()) storem = jackshop.GetComponent<StoreManager>();
        //-----------------------
        //ショップ毎日更新
        if (PlayerPrefs.GetString(shop_name, "None") != "None")
        {
            DateTime setdate = DateTime.Today;
            shopYear = PlayerPrefs.GetInt(shop_name + "_Year", setdate.Year);
            shopMonth = PlayerPrefs.GetInt(shop_name + "_Month", setdate.Month);
            shopDay = PlayerPrefs.GetInt(shop_name + "_Day", setdate.Day);
            checkday = new DateTime(shopYear, shopMonth, shopDay);
        }
        if (shop_name != "None" && (PlayerPrefs.GetString(shop_name, "None") == "None" || (PlayerPrefs.GetString(shop_name, "None") != "None" && Math.Abs(GManager.instance.AllSpanCheck(checkday)) > 0)))
        {
            DateTime setdate = GManager.instance.GetGameDay();
            PlayerPrefs.SetString(shop_name, setdate.ToBinary().ToString());
            PlayerPrefs.SetInt(shop_name + "_Year", setdate.Year);
            PlayerPrefs.SetInt(shop_name + "_Month", setdate.Month);
            PlayerPrefs.SetInt(shop_name + "_Day", setdate.Day);

            shopID_0 = UnityEngine.Random.Range(0, GManager.instance.MagicID.Length);
            int tmp_result = shopID_0;
            PlayerPrefs.SetInt(shop_name + "0", tmp_result);
            while (tmp_result == shopID_0)
            {
                tmp_result = UnityEngine.Random.Range(0, GManager.instance.MagicID.Length);
                if (tmp_result != shopID_0) break;
            }
            shopID_1 = tmp_result;
            PlayerPrefs.SetInt(shop_name + "1", tmp_result);
            while (tmp_result == shopID_0 || tmp_result == shopID_1)
            {
                tmp_result = UnityEngine.Random.Range(0, GManager.instance.MagicID.Length);
                if (tmp_result != shopID_0 && tmp_result != shopID_1) break;
            }
            shopID_2 = tmp_result;
            PlayerPrefs.SetInt(shop_name + "2", tmp_result);
            PlayerPrefs.Save();
        }
        else if (shop_name != "None" && PlayerPrefs.GetString(shop_name, "None") != "None")
        {
            shopID_0 = PlayerPrefs.GetInt(shop_name + "0", 0);
            shopID_1 = PlayerPrefs.GetInt(shop_name + "1", 0);
            shopID_2 = PlayerPrefs.GetInt(shop_name + "2", 0);
        }
        GManager.instance.shopID[0] = shopID_0;
        GManager.instance.shopID[1] = shopID_1;
        GManager.instance.shopID[2] = shopID_2;
        //-----------------------
        if (GManager.instance.shopID[inputshopID] > -1) selectprice = (double)GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicprice / (double)100;
        SetUIMagic();
    }
    void SetUIMagic()
    {
        getdevcoin_text.text = GManager.instance.get_devcoin.ToString() + "(DC)×";
        if (GManager.instance.shopID[inputshopID] == -1)
        {
            magicname.text = "？？？？？？";
            magicsprite.sprite = nullsprite;
            magicscript.text = "？？？？？？？？？？";
            magicprice.text = "？×";
            magicinputget.text = "？???";
            slimesprite.sprite = nullsprite;
            if (inputshopID == 0)
            {
                slimename.text = "????:????";
            }
        }
        else if (GManager.instance.shopID[inputshopID] != -1)
        {
            magicsprite.sprite = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicimage;
            slimesprite.sprite = GManager.instance.Pstatus[GManager.instance.playerselect].pimage;
            if (GManager.instance.isEnglish == 0)
            {
                magicscript.text = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicscript;
                magicname.text = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicname;
                if (inputshopID == 0)
                {
                    slimename.text = "選択中のスライム：" + GManager.instance.Pstatus[GManager.instance.playerselect].pname;

                }
            }
            else if (GManager.instance.isEnglish == 1)
            {
                magicscript.text = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicscript2;
                magicname.text = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicname2;
                if (inputshopID == 0)
                {
                    slimename.text = "Slime in selection：" + GManager.instance.Pstatus[GManager.instance.playerselect].pname2;
                }
            }
            magicprice.text = selectprice + "(DC)×";
            for (int i = 0; i < GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length;)
            {
                notget = true;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].magicid == GManager.instance.shopID[inputshopID] && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].gettrg < 1)
                {
                    if (GManager.instance.isEnglish == 0)
                    {
                        magicinputget.text = "覚えられる：";
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        magicinputget.text = "be able to remember：";
                    }
                    notget = false;
                    i = GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length;
                }
                i++;
            }
            if (notget == true)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    magicinputget.text = "覚えられない：";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    magicinputget.text = "I can't remember：";
                }
            }

        }
        else
        {
            magicname.text = "？？？？？？";
            magicsprite.sprite = nullsprite;
            magicscript.text = "？？？？？？？？？？";
            magicprice.text = "？×";
            magicinputget.text = "？???";
            slimesprite.sprite = nullsprite;
            if (inputshopID == 0)
            {
                slimename.text = "????:????";
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (pushtrg == true)
        {
            pushtime += Time.deltaTime;
            if (pushtime > 1f)
            {
                pushtime = 0;
                pushtrg = false;
            }
        }
    }
    public void ShopPlay()
    {
        if (GManager.instance.shopID[inputshopID] != -1 && notget == false && cooltime <= 0 && selectprice <= GManager.instance.get_devcoin)
        {
            cooltime = 99f;
            audioS.PlayOneShot(getse);
            GManager.instance.setmenu += 1;
            //処理
            GameObject tmpobj = Instantiate(BuyCheckUI, parentobj.transform.position, parentobj.transform.rotation, parentobj.transform);
            GManager.instance.select_buyid = GManager.instance.shopID[inputshopID];
            tmpobj.GetComponent<DataBuySystem>().get_buytype = 2;
            
        }
        else
        {
            GManager.instance.setrg = 27;
        }
    }
}
