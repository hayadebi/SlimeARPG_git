using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shop : MonoBehaviour
{
    //【ショップUIのスクリプト】
    //※大体はUIのボタンを押した時に動かします。
    public AudioSource audioS;
    public AudioClip getse;//入手時、決定などの音
    public AudioClip notse;//キャンセル、想定外な時の音
    //ゲームマネージャーからショップIDを取得するためのやつ。
    //会話イベントのスクリプトからマネージャーのショップを操作する。
    public int inputshopID;
    public Sprite nullsprite;//想定外な時の画像
    public Text itemname;//アイテム名のテキスト取得
    public Image itemsprite;//アイテムの画像取得
    public Text itemscript;//アイテム説明のテキスト取得
    public Text itemprice;//アイテム値段のテキスト取得
    public Text itemgetnumber;//アイテム所持数のテキスト取得

    //その他一時的な変数
    bool pushtrg = false;
    float pushtime = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (GManager.instance.shopID[inputshopID] == -1)//想定外な時のUI操作
        {
            //何も無い状態
            itemname.text = "？？？？？？";
            itemsprite.sprite = nullsprite;
            itemscript.text = "？？？？？？？？？？";
            itemprice.text = "？×";
            itemgetnumber.text = "？×";
        }
        else if (GManager.instance.shopID[inputshopID] != -1)//想定してる時のUI操作
        {
            //UIにショップ情報を反映
            itemsprite.sprite = GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemimage;
            if (GManager.instance.isEnglish == 0)
            {
                itemscript.text = GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemscript;
                itemname.text = GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemname;

            }
            else if (GManager.instance.isEnglish == 1)
            {
                itemscript.text = GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemscript2;
                itemname.text = GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemname2;

            }
            itemprice.text = GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemprice + "×";
            itemgetnumber.text = GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemnumber + "×";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //押されたのを検知し、タイマーを動かす。
        //タイマーが動いてる間はボタンを押しても反応しない。
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

    public void ShopClick()
    {
        //購入ボタン部分を押した時に動く、UIのボタン用
        if (pushtrg == false)
        {
            pushtrg = true;//押したことにする
            if (GManager.instance.shopID[inputshopID] == -1)//想定外な時
            {
                audioS.PlayOneShot(notse);
            }
            else if (GManager.instance.shopID[inputshopID] != -1)//ショップIDが示すとこが、商品があるショップだった時
            {
                if (GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemprice > GManager.instance.Coin)//所持金が価格未満の時
                {
                    audioS.PlayOneShot(notse);
                }
                else if (GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemprice <= GManager.instance.Coin)//価格以上の所持金時
                {
                    //価格分所持金を減らし、該当するアイテムを手持ちに増やす
                    audioS.PlayOneShot(getse);
                    GManager.instance.Coin -= GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemprice;
                    GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemnumber += 1;
                    itemgetnumber.text = GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].itemnumber + "×";
                    GManager.instance.ItemID[GManager.instance.shopID[inputshopID]].gettrg = 1;
                    
                }
            }
        }
    }
}
