using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NCMB;
using System.IO;
using System;

public class UserMarketUI : MonoBehaviour
{
    [Header("取得")]
    public AudioSource audioS;
    public AudioClip selectse; //項目を切り替えた時の効果音
    public AudioClip onse;
    public AudioClip notse; //キャンセルなどの効果音
    public Sprite nullimage; //想定外な場合の画像
    [Header("作成するアイテム")]
    public Text craftItem_name; //アイテム名
    public Text craftItem_script; //アイテム説明
    public Image craftItem_image; //アイテム画像
    [Header("格納、一時的な変数達")]
    private int selectnumber = 0;
    private int maxlist = 0;
    public List<string> onItem;
    public List<int> onId;
    public List<string> onDay;
    [Header("追加課金UI")]
    public Text pricedevcoin;
    public Text get_userdevcoin;
    public Text get_buyday;
    public GameObject jackshop = null;
    public StoreManager storem;
    private double selectprice;
    public GameObject BuyCheckUI;
    private float cooltime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("online-jackshop(Clone)")) jackshop = GameObject.Find("online-jackshop(Clone)");
        if (jackshop != null && jackshop.GetComponent<StoreManager>()) storem = jackshop.GetComponent<StoreManager>();
        NCMBQuery<NCMBObject> query = null;
        query = new NCMBQuery<NCMBObject>("BlackMarket");
        query.OrderByDescending("useritems");
        //検索件数を設定
        query.Limit = 99;
        int i = 1;
        //データストアでの検索を行う
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                onItem = null;
            }
            else
            {
                maxlist = objList.Count;
                //検索成功時の処理
                foreach (NCMBObject obj in objList)
                {
                    onItem.Add(obj.ObjectId);
                    var tmp = obj["itemid"].ToString();
                    onId.Add(int.Parse(tmp));
                    onDay.Add(obj["buyday"].ToString());
                    i++;
                }
            }
        });
        Invoke(nameof(SetUI),0.31f);//UIを表示
    }
    private void Update()
    {
        ;//if (cooltime >= 0) cooltime -= Time.deltaTime;
    }
    //クラフトUIを表示(呼び出して使う)
    public void SetUI()
    {
        if (onItem!=null && onItem.Count>0 &&storem != null)
        {
            selectprice = (double)GManager.instance.ItemID[onId[selectnumber]].itemprice / (double)600;
            if (GManager.instance.isEnglish == 0)
            {
                get_buyday.text = "出品日：" + onDay[selectnumber];
                pricedevcoin.text = selectprice.ToString() + "デビコインで購入";
                get_userdevcoin.text = "所持デビコイン：" + GManager.instance.get_devcoin.ToString();
            }
            else
            {
                get_buyday.text = "Sale Date：" + onDay[selectnumber];
                pricedevcoin.text = "Purchased for" + selectprice.ToString() + " 0.03 devilcoins.";
                get_userdevcoin.text = "DevilCoins you have：" + GManager.instance.get_devcoin.ToString();
            }

        }
        if (onItem == null || onItem.Count == 0)//想定外
        {
            craftItem_image.sprite = nullimage;
            craftItem_name.text = "????";
            craftItem_script.text = "????????";
        }
        //大雑把に条件を言うと、表示可能なレシピがあるかどうか AND 選択しているか AND 選択してるクラフトレシピの対象アイテムIDが指定されているか
        else if (onItem != null && onItem.Count > 0 && onId.Count>0 &&onId[selectnumber] >= 0 && selectnumber != -1)
        {
            //それぞれ条件に応じてレシピ情報を表示
            craftItem_image.sprite = GManager.instance.ItemID[onId[selectnumber]].itemimage;
            if (GManager.instance.isEnglish == 0)
            {
                craftItem_name.text = GManager.instance.ItemID[onId[selectnumber]].itemname;
                craftItem_script.text = GManager.instance.ItemID[onId[selectnumber]].itemscript;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                craftItem_name.text = GManager.instance.ItemID[onId[selectnumber]].itemname2;
                craftItem_script.text = GManager.instance.ItemID[onId[selectnumber]].itemscript2;
            }
            craftItem_image.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            //一切想定していない状況な場合の表示
            craftItem_image.sprite = nullimage;
            craftItem_name.text = "????";
            craftItem_script.text = "????????";
        }
    }

    public void SelectMinus() //レシピ項目を戻って切り替える、セレクトボタン
    {
        if (onItem == null || onItem.Count == 0)
        {
            audioS.PlayOneShot(notse);
        }
        else if (onItem != null && onItem.Count > 0 && selectnumber > 0)
        {
            audioS.PlayOneShot(selectse);
            selectnumber -= 1;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SelectPlus() //レシピ項目を進んで切り替える、セレクトボタン
    {
        if (onItem==null||onItem.Count == 0)
        {
            audioS.PlayOneShot(notse);
        }
        else if (onItem != null && onItem.Count > 0 && selectnumber < (maxlist - 1))
        {
            audioS.PlayOneShot(selectse);
            selectnumber += 1;
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void ShopPlay()
    {
        if (onItem != null && onItem.Count > 0 && onId[selectnumber] >= 0 && cooltime <= 0 && selectprice<=GManager.instance.get_devcoin)
        {
            cooltime = 99f;
            audioS.PlayOneShot(onse);
            GManager.instance.setmenu += 1;
            //処理
            GameObject tmpobj = Instantiate(BuyCheckUI, transform.position, transform.rotation, transform);
            GManager.instance.select_buyid = onId[selectnumber];
            tmpobj.GetComponent<DataBuySystem>().get_buytype = 0;
            NCMBObject tmp = new NCMBObject("BlackMarket");
            tmp.ObjectId = onItem[selectnumber];
            tmp.SaveAsync();
            tmpobj.GetComponent<DataBuySystem>().deltarget_data = tmp;
        }
        else
        {
            GManager.instance.setrg = 27;
        }
    }

}