using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionUI : MonoBehaviour
{
    //【ミッションUIのスクリプト】※表示用
    public AudioSource audioS;
    public AudioClip selectse;//選択時の音
    public AudioClip notse;//キャンセル時などの音
    public Sprite nullimage;//想定外な時の画像
    public Image CLimage;//依頼主の画像
    public Image ITimage;//ミッションが指定してるアイテム画像
    public Text Mname;//指定してるアイテム名のテキスト取得
    public Text CLname;//依頼主名のテキスト取得
    public Text Mscript; //ミッション説明のテキスト取得
    public Text Minputitem;//ミッション達成するまでの、残り指定アイテム数テキスト取得
    public Text getText;//報酬のテキスト取得

    //その他一時的な変数
    int selectnumber = 0;
    public int[] onMission;
    int boxnumber = 0;
    int inputnumber = 0;
    bool usetrg = false;
    // Start is called before the first frame update
    void Start()
    {
        //最初にゲームマネージャー内のミッションを調べ、現在表示可能なミッションを格納する
        for (int i = 0; GManager.instance.missionID.Length > i;)
        {
            if(GManager.instance.missionID[i].inputmission > 0)
            {
                boxnumber += 1;
            }
            i += 1;
        }
        onMission = new int[boxnumber];
        for (int i = 0; GManager.instance.missionID.Length > i;)
        {
            if(GManager.instance.missionID[i].inputmission > 0)
            {
                onMission[inputnumber] = i;
                inputnumber += 1;
            }
            i += 1;
        }
        SetUI();

    }

    //呼び出してUIの表示を操作するもの
    void SetUI()
    {
        if (onMission == null)//表示可能なミッションが無い場合
        {
            //何もない状態のUI表示
            CLimage.sprite = nullimage;
            ITimage.sprite = nullimage;
            if (GManager.instance.isEnglish == 0)
            {
                CLname.text = "依頼主:??????";
                Mname.text = "????????";
                if (GManager.instance.missionID[onMission[selectnumber]].subtrg == false)
                {
                    Mscript.text = "メインミッション:\n" + "????????";
                }
                else if (GManager.instance.missionID[onMission[selectnumber]].subtrg == true)
                {
                    Mscript.text = "サブミッション:\n" + "????????";
                }
                Minputitem.text = "対象アイテム:\n　　" + "???? ?/??";
                getText.text = "報酬:\n" +
                    "コイン　??\n" +
                    "アイテム　????\n" +
                    "実績　??????\n" +
                    "仲間　????\n";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                CLname.text = "Client:??????";
                Mname.text = "????????";
                if (GManager.instance.missionID[onMission[selectnumber]].subtrg == false)
                {
                    Mscript.text = "Main mission:\n" + "????????";
                }
                else if (GManager.instance.missionID[onMission[selectnumber]].subtrg == true)
                {
                    Mscript.text = "Sub mission:\n" + "????????";
                }
                Minputitem.text = "Target items:\n　　" + "???? ?/??";
                getText.text = "Reward:\n" +
                    "Coin　??\n" +
                    "Item　????\n" +
                    "Achievements　??????\n" +
                    "Fellow　????\n";
            }
        }
        else if (onMission != null && onMission.Length != 0)//表示可能であり、想定通りな場合のUI表示
        {
            CLimage.sprite = GManager.instance.missionID[onMission[selectnumber]].clientimage;
            ITimage.sprite = GManager.instance.ItemID[GManager.instance.missionID[onMission[selectnumber]].inputitemid].itemimage;
            if (GManager.instance.isEnglish == 0)//日本語設定の時
            {
                CLname.text = "依頼主:"+ GManager.instance.missionID[onMission[selectnumber]].client;
                Mname.text = GManager.instance.missionID[onMission[selectnumber]].name;
                if (GManager.instance.missionID[onMission[selectnumber]].subtrg == false)
                {
                    Mscript.text = "メインミッション:\n" + GManager.instance.missionID[onMission[selectnumber]].script;
                }
                else if (GManager.instance.missionID[onMission[selectnumber]].subtrg == true)
                {
                    Mscript.text = "サブミッション:\n" + GManager.instance.missionID[onMission[selectnumber]].script;
                }
                Minputitem.text = "対象アイテム:\n　　" + GManager.instance.ItemID[GManager.instance.missionID[onMission[selectnumber]].inputitemid].itemname + GManager.instance.ItemID[GManager.instance.missionID[onMission[selectnumber]].inputitemid ].itemnumber +"/"+ GManager.instance.missionID[onMission[selectnumber]].inputitemnumber ;
                getText.text = "報酬:\n" +
                    "コイン　" + GManager.instance.missionID[onMission[selectnumber]].getcoin;
                if(GManager.instance.missionID[onMission[selectnumber]].getitemid != -1)
                {
                    getText.text += "\nアイテム　" + GManager.instance.ItemID[GManager.instance.missionID[onMission[selectnumber]].getitemid].itemname;
                }
                if (GManager.instance.missionID[onMission[selectnumber]].getachievementsid != -1)
                {
                    getText.text += "\n実績　" + GManager.instance.achievementsID[GManager.instance.missionID[onMission[selectnumber]].getachievementsid].name;
                }
                if (GManager.instance.missionID[onMission[selectnumber]].getpl != -1)
                {
                    getText.text += "\n仲間　" + GManager.instance.Pstatus[GManager.instance.missionID[onMission[selectnumber]].getpl].pname;
                }
                if (GManager.instance.missionID[onMission[selectnumber]].getsource != "")
                {
                    getText.text += "\n"+GManager.instance.missionID[onMission[selectnumber]].getsource;
                }
            }
            else if (GManager.instance.isEnglish == 1)//英語設定の時
            {
                CLname.text = "Client:"+ GManager.instance.missionID[onMission[selectnumber]].client2;
                Mname.text = GManager.instance.missionID[onMission[selectnumber]].name2 ;
                if (GManager.instance.missionID[onMission[selectnumber]].subtrg == false)
                {
                    Mscript.text = "Main mission:\n" + GManager.instance.missionID[onMission[selectnumber]].script2;
                }
                else if (GManager.instance.missionID[onMission[selectnumber]].subtrg == true)
                {
                    Mscript.text = "Sub mission:\n" + GManager.instance.missionID[onMission[selectnumber]].script2;
                }
                Minputitem.text = "Target items:\n　　" + GManager.instance.ItemID[GManager.instance.missionID[onMission[selectnumber]].inputitemid].itemname2 + GManager.instance.ItemID[GManager.instance.missionID[onMission[selectnumber]].inputitemid].itemnumber + "/" + GManager.instance.missionID[onMission[selectnumber]].inputitemnumber;
                getText.text = "Reward:\n" +
                   "Coin　" + GManager.instance.missionID[onMission[selectnumber]].getcoin;
                if (GManager.instance.missionID[onMission[selectnumber]].getitemid != -1)
                {
                    getText.text += "\nItem　" + GManager.instance.ItemID[GManager.instance.missionID[onMission[selectnumber]].getitemid].itemname;
                }
                if (GManager.instance.missionID[onMission[selectnumber]].getachievementsid != -1)
                {
                    getText.text += "\nAchievements　" + GManager.instance.achievementsID[GManager.instance.missionID[onMission[selectnumber]].getachievementsid].name;
                }
                if (GManager.instance.missionID[onMission[selectnumber]].getpl != -1)
                {
                    getText.text += "\nFellow　" + GManager.instance.Pstatus[GManager.instance.missionID[onMission[selectnumber]].getpl].pname;
                }
                if (GManager.instance.missionID[onMission[selectnumber]].getsource != "")
                {
                    getText.text += "\n"+GManager.instance.missionID[onMission[selectnumber]].getsource2;
                }
            }
        }
        else//全く想定していない時の表示
        {
            CLimage.sprite = nullimage;
            ITimage.sprite = nullimage;
            if (GManager.instance.isEnglish == 0)
            {
                CLname.text = "依頼主:??????";
                Mname.text = "????????";
                    Mscript.text = "ミッション:\n" + "????????";
                Minputitem.text = "対象アイテム:\n　　" + "???? ?/??";
                getText.text = "報酬:\n" +
                    "コイン　??\n" +
                    "アイテム　????\n" +
                    "実績　??????\n" +
                    "仲間　????\n";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                CLname.text = "Client:??????";
                Mname.text = "????????";
                    Mscript.text = "Mission:\n" + "????????";
                Minputitem.text = "Target items:\n　　" + "???? ?/??";
                getText.text = "Reward:\n" +
                    "Coin　??\n" +
                    "Item　????\n" +
                    "Achievements　??????\n" +
                    "Fellow　????\n";
            }
        }
    }

    public void SelectMinus()//ミッション項目を戻って切り替える、セレクトボタン
    {
        if (onMission == null)
        {
            audioS.PlayOneShot(notse);
        }
        else if (selectnumber > 0)
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
    public void SelectPlus()//ミッション項目を進んで切り替える、セレクトボタン
    {
        if (onMission == null)
        {
            audioS.PlayOneShot(notse);
        }
        else if (selectnumber < (onMission.Length - 1))
        {
            audioS.PlayOneShot(selectse);
            selectnumber += 1;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    //----
}