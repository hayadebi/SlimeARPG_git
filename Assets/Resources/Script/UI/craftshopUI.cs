using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class craftshopUI : MonoBehaviour
{
    public Image buttonImage; //ボタン自体をいじる時用
    [Header("取得")]
    public AudioSource audioS;
    public AudioClip selectse; //項目を切り替えた時の効果音
    public AudioClip onse;
    public AudioClip notse; //キャンセルなどの効果音
    public Sprite nullimage; //想定外な場合の画像
    [Header("作成するアイテム")]
    public Text craftItem_number; //獲得アイテム数
    public Text craftItem_name; //アイテム名
    public Text craftItem_script; //アイテム説明
    public Image craftItem_image; //アイテム画像
    [Header("使用する素材")]
    public Image[] material_image; //アイテム画像
    public Text[] material_name; //アイテム名
    public Text[] material_number; //要求アイテム数
    [Header("格納、一時的な変数達")]
    int selectnumber = 0;
    public int[] onItem=null;
    private int boxnumber = 0;
    private int inputnumber = 0;
    [Header("追加課金UI")]
    public Text pricedevcoin;
    public Text get_userdevcoin;
    public  GameObject jackshop = null;
    public StoreManager storem;
    private double selectprice;
    private string selectname;
    public GameObject BuyCheckUI;
    private float cooltime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("online-jackshop(Clone)")) jackshop = GameObject.Find("online-jackshop(Clone)");
        if (jackshop != null && jackshop.GetComponent<StoreManager>()) storem = jackshop.GetComponent<StoreManager>();
        //ゲームマネージャー内のクラフトレシピ達を格納
        for (int i = 0; GManager.instance._craftRecipe.Length > i;)
        {
            if (GManager.instance._craftRecipe[i].get_recipe <= 0 && !GManager.instance._craftRecipe[i].on_notgetview)
            {
                boxnumber += 1;
            }
            i += 1;
        }
        if(boxnumber>0) onItem = new int[boxnumber];
        for (int i = 0; GManager.instance._craftRecipe.Length > i;)
        {
            if (GManager.instance._craftRecipe[i].get_recipe <= 0&& !GManager.instance._craftRecipe[i].on_notgetview)
            {
                onItem[inputnumber] = i;
                inputnumber += 1;
            }
            i += 1;
        }
        SetUI();//UIを表示
    }
    private void Update()
    {
        ;// if (cooltime >= 0) cooltime -= Time.deltaTime;
    }
    //クラフトUIを表示(呼び出して使う)
    public void SetUI()
    {
        if (storem != null && boxnumber > 0)
        {
            selectprice = (double)GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemprice / (double)200;
            if (GManager.instance.isEnglish == 0)
            {
                selectname = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemname;
                pricedevcoin.text = selectprice.ToString() + "デビコインで購入";
                get_userdevcoin.text = "所持デビコイン：" + GManager.instance.get_devcoin.ToString();
            }
            else 
            {
                selectname = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemname2;
                pricedevcoin.text = "Purchased for" + selectprice.ToString() + " 0.03 devilcoins.";
                get_userdevcoin.text = "DevilCoins you have：" + GManager.instance.get_devcoin.ToString();
            }
            
        }
        if (boxnumber <= 0 || onItem == null || onItem.Length == 0)//想定外
        {
            craftItem_image.sprite = nullimage;
            craftItem_name.text = "????";
            craftItem_number.text = "??";
            craftItem_script.text = "????????";
            //追加
            for (int i = 0; i < material_image.Length;)
            {
                material_image[i].sprite = nullimage;
                material_name[i].text = "????";
                material_number[i].text = "?/?";
                i++;
            }
        }
        //大雑把に条件を言うと、表示可能なレシピがあるかどうか AND 選択しているか AND 選択してるクラフトレシピの対象アイテムIDが指定されているか
        else if (boxnumber > 0 && onItem[selectnumber] >= 0 && onItem.Length > 0 && selectnumber != -1 && onItem.Length <= GManager.instance._craftRecipe.Length && GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id != -1)
        {
            //それぞれ条件に応じてレシピ情報を表示
            craftItem_image.sprite = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemimage;
            craftItem_number.text = "" + GManager.instance._craftRecipe[onItem[selectnumber]].craftGet_number;
            if (GManager.instance.isEnglish == 0)
            {
                craftItem_name.text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemname;
                craftItem_script.text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemscript;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                craftItem_name.text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemname2;
                craftItem_script.text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemscript2;
            }
            craftItem_image.color = new Color(1, 1, 1, 0.5f);
            craftItem_number.color = new Color(1, 1, 1, 0.5f);
            for (int i = 0; i < material_image.Length;)
            {
                if (GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i] != -1 && GManager.instance._craftRecipe[onItem[selectnumber]].materialSet_number[i] <= GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemnumber)
                {
                    material_image[i].color = new Color(1, 1, 1, 1f);
                    material_name[i].color = new Color(0.5f, 0.3f, 0.2f, 1f);
                    material_number[i].color = new Color(1, 1, 1, 1f);
                    material_image[i].sprite = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemimage;
                    if (GManager.instance.isEnglish == 0)
                    {
                        material_name[i].text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemname;
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        material_name[i].text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemname2;
                    }
                    material_number[i].text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemnumber + "/" + GManager.instance._craftRecipe[onItem[selectnumber]].materialSet_number[i];
                }
                else if (GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i] != -1 && GManager.instance._craftRecipe[onItem[selectnumber]].materialSet_number[i] > GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemnumber)
                {
                    material_image[i].color = new Color(1, 1, 1, 0.5f);
                    material_name[i].color = new Color(1, 0, 0, 0.5f);
                    material_number[i].color = new Color(1, 0, 0, 0.5f);
                    material_image[i].sprite = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemimage;
                    if (GManager.instance.isEnglish == 0)
                    {
                        material_name[i].text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemname;
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        material_name[i].text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemname2;
                    }
                    material_number[i].text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemnumber + "/" + GManager.instance._craftRecipe[onItem[selectnumber]].materialSet_number[i];
                }
                else
                {
                    material_image[i].color = new Color(1, 1, 1, 0.5f);
                    material_name[i].color = new Color(0.5f, 0.3f, 0.2f, 0.5f);
                    material_number[i].color = new Color(1, 1, 1, 0.5f);
                    material_image[i].sprite = nullimage;
                    material_name[i].text = "????";
                    material_number[i].text = "?/?";
                }
                i++;
            }
        }
        else
        {
            //一切想定していない状況な場合の表示
            craftItem_image.sprite = nullimage;
            craftItem_name.text = "????";
            craftItem_number.text = "??";
            craftItem_script.text = "????????";
            //追加
            for (int i = 0; i < material_image.Length;)
            {
                material_image[i].sprite = nullimage;
                material_name[i].text = "????";
                material_number[i].text = "?/?";
                i++;
            }
        }
        if (boxnumber > 0 && GManager.instance._craftRecipe[onItem[selectnumber]].get_recipe > 0)
        {
            buttonImage.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            buttonImage.color = new Color(1, 1, 1, 1);
        }
    }

    public void SelectMinus() //レシピ項目を戻って切り替える、セレクトボタン
    {
        if (boxnumber <= 0 ||onItem.Length == 0)
        {
            audioS.PlayOneShot(notse);
        }
        else if (boxnumber > 0 && selectnumber > 0)
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
        if (boxnumber <= 0||onItem.Length == 0)
        {
            audioS.PlayOneShot(notse);
        }
        else if (boxnumber > 0 && selectnumber < (onItem.Length - 1))
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
        if (boxnumber > 0 && GManager.instance._craftRecipe[onItem[selectnumber]].get_recipe <= 0 && cooltime<=0 && selectprice <= GManager.instance.get_devcoin)
        {
            cooltime = 99f;
            audioS.PlayOneShot(onse);
            GManager.instance.setmenu += 1;
            //処理
            GameObject tmpobj = Instantiate(BuyCheckUI, transform.position, transform.rotation,transform);
            GManager.instance.select_buyid = onItem[selectnumber];
            tmpobj.GetComponent<DataBuySystem>().get_buytype = 1;
            buttonImage.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            GManager.instance.setrg = 27;
        }
    }

}