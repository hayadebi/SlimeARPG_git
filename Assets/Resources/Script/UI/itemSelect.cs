using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class itemSelect : MonoBehaviour
{
    //【アイテムセレクトのスクリプト】

    public GameObject Fadein; //フェードインのUI
    public GameObject selectmenuUI; //アイテムセレクトのボタン自体
    public AudioSource audioS;
    public AudioClip selectse; //選択などの効果音
    public AudioClip onse; //決定などの効果音
    public AudioClip notse; //キャンセルなどの効果音
    public Sprite nullimage; //想定外な時に表示する画像
    public Text numberText; //アイテム数のテキスト取得
    public Text nameText; //アイテム名のテキスト取得
    public Text scriptText; //アイテム説明のテキスト取得
    public Image itemimage; //アイテムの画像取得

    //一時的に使う変数
    int selectnumber = 0;
    public int[] onItem;
    int boxnumber = 0;
    int inputnumber = 0;
    bool usetrg = false;

    private GameObject P; //プレイヤー自体を取得
    private player ps; //Pからスクリプトを取得
    [Header("アイテムスロット関連")]
    public Image[] quickItem_image; //アイテムスロットの各画像取得
    //アイテムスロットをセットするための
    public Toggle toggle; 
    public Text _toggleText;
    
    //その他一時的な変数
    private bool notOn = false;
    private int oldmode = 0;
    private int nextmode = 1;
    private int oldquickindex = -1;
    private bool allquick = false;
    private int randomN;

    //最初に取得や格納をする
    void Start()
    {
        P = GameObject.Find("Player");
        ps = P.GetComponent<player>();
        //ここでは表示可能なアイテムを調べ、格納している
        for (int i = 0; GManager.instance.ItemID.Length > i;)
        {
            if (GManager.instance.ItemID[i].itemnumber > 0)
            {
                boxnumber += 1;
            }
            i += 1;
        }
        onItem = new int[boxnumber];
        for (int i = 0; GManager.instance.ItemID.Length > i;)
        {
            if (GManager.instance.ItemID[i].itemnumber > 0)
            {
                onItem[inputnumber] = i;
                inputnumber += 1;
            }
            i += 1;
        }
        SetUI();//UI表示を呼び出す
    }

    //変化があるアイテムスロットのためにUpdate使用
    private void Update()
    {
        allquick = true;
        for (int i = 0; i < GManager.instance.Quick_itemSet.Length;)//ここのfor処理はセットされてるかどうか調べてる(いつか別の方法にする)
        {
            if (GManager.instance.Quick_itemSet[i] == -1 || GManager.instance.ItemID[GManager.instance.Quick_itemSet[i]].itemnumber <= 0)
            {
                allquick = false;
            }
            i++;
        }
        //セットできるか状況に応じて
        if (notOn && oldmode == 0 && toggle.isOn)
        {
            toggle.isOn = false;
            audioS.PlayOneShot(notse);
        }
        else if (notOn && oldmode == 1 && !toggle.isOn)
        {
            toggle.isOn = true;
            audioS.PlayOneShot(notse);
        }
        //セットできる状態ならクイックセットを呼び出す
        else if(!notOn && ((oldmode == 0 && toggle.isOn )||(oldmode == 1 && !toggle.isOn)))
        {
            if(oldmode == 0 && toggle.isOn && !allquick)//セットする場合
            {
                nextmode = 1;
                Quick_Set();
            }
            else if (oldmode == 0 && toggle.isOn && allquick)//状態と該当しない場合
            {
                toggle.isOn = false;
                audioS.PlayOneShot(notse);
            }
            else if(oldmode == 1 && !toggle.isOn)//セットを外す場合
            {
                nextmode = 0;
                Quick_Set();
            }
        }
    }

    //ここでアイテムスロットを変える処理、表示は最後のSetUIでやる
    void Quick_Set()
    {
        if (nextmode == 1)//セットする処理
        {
            oldquickindex = -1;
            for (int i = 0; i < GManager.instance.Quick_itemSet.Length;)
            {
                //セットできる状態を調べ、最初に見つかった可能箇所にセット
                if (GManager.instance.Quick_itemSet[i] == -1)
                {
                    oldquickindex = i;
                    GManager.instance.ItemID[onItem[selectnumber]]._quickset = oldquickindex;
                    GManager.instance.Quick_itemSet[oldquickindex] = onItem[selectnumber];
                    i += GManager.instance.Quick_itemSet.Length;
                }
                i++;
            }
            if(oldquickindex == -1)
            {
                //ヒットしない場合は、その中で所数数0以下のスロットにセット
                for (int i = 0; i < GManager.instance.Quick_itemSet.Length;)
                {
                    if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[i]].itemnumber <= 0)
                    {
                        oldquickindex = i;
                        GManager.instance.ItemID[onItem[selectnumber]]._quickset = oldquickindex;
                        GManager.instance.Quick_itemSet[oldquickindex] = onItem[selectnumber];
                        i += GManager.instance.Quick_itemSet.Length;
                    }
                    i++;
                }
            }
        }
        else if(nextmode == 0)//セットを外す処理
        {
            GManager.instance.Quick_itemSet[GManager.instance.ItemID[onItem[selectnumber]]._quickset] = -1;
            GManager.instance.ItemID[onItem[selectnumber]]._quickset = -1;
        }
        ps.setItem();//プレイヤースクリプトからセットしたものを発動できるようにしてるので、こっちも再セット
        SetUI();//表示呼び出し
    }
    
    //以下を呼び出してUI表示を行う
    public void SetUI()
    {
        if (onItem == null || onItem.Length == 0)//表示可能ではない場合
        {
            itemimage.sprite = nullimage;
            nameText.text = "????";
            if (GManager.instance.isEnglish == 0)
            {
                numberText.text = "所持数:???\n売値:???\nレア度:??";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                numberText.text = "Number:???\nPrice: ???\nRarity:??";
            }
            scriptText.text = "????????";
            //追加
            for (int i = 0; i < quickItem_image.Length;)
            {
                quickItem_image[i].sprite = nullimage;
                i++;
            }
            toggle.isOn = false;
            oldmode = 0;
            notOn = true;
            if (GManager.instance.isEnglish == 0)
            {
                _toggleText.text = "現在はセットできません";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                _toggleText.text = "Cannot be set at this time.";
            }
        }
        //簡単にここの条件を表すと、表示可能なアイテムはあるかどうか
        else if (onItem[selectnumber] >= 0 && onItem.Length > 0 && onItem.Length <= GManager.instance.ItemID.Length && GManager.instance.ItemID[onItem[selectnumber]].itemnumber > 0)
        {
            //アイテム情報の表示
            itemimage.sprite = GManager.instance.ItemID[onItem[selectnumber]].itemimage;
            if (GManager.instance.ItemID[onItem[selectnumber]].itemnumber > 99)
            {
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber = 99;
            }
            if (GManager.instance.isEnglish == 0)
            {
                numberText.text = "所持数:" + GManager.instance.ItemID[onItem[selectnumber]].itemnumber + "\n売値:" + GManager.instance.ItemID[onItem[selectnumber]].itemprice +
                    "\nレア度:" + GManager.instance.ItemID[onItem[selectnumber]].rare;
                nameText.text = GManager.instance.ItemID[onItem[selectnumber]].itemname;
                scriptText.text = GManager.instance.ItemID[onItem[selectnumber]].itemscript;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                numberText.text = "Number:" + GManager.instance.ItemID[onItem[selectnumber]].itemnumber + "\nPrice:" + GManager.instance.ItemID[onItem[selectnumber]].itemprice +
                    "\nRarity:" + GManager.instance.ItemID[onItem[selectnumber]].rare;
                nameText.text = GManager.instance.ItemID[onItem[selectnumber]].itemname2;
                scriptText.text = GManager.instance.ItemID[onItem[selectnumber]].itemscript2;
            }
            //アイテムスロットの表示
            for (int i = 0; i < quickItem_image.Length;)
            {
                if (GManager.instance.Quick_itemSet[i] == -1 || GManager.instance.ItemID[GManager.instance.Quick_itemSet[i]]._quickset == -1)
                {
                    quickItem_image[i].sprite = nullimage;
                }
                else if (GManager.instance.ItemID[GManager.instance.Quick_itemSet[i]]._quickset != -1)
                {
                    quickItem_image[i].sprite = GManager.instance.ItemID[GManager.instance.Quick_itemSet[i]].itemimage;
                }
                i++;
            }
            //選択中アイテムのセット状態を調べ、条件によってはボタンを押されても 変化しない状態にしたりする
            if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber <= 0 || GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 3)//効果があるアイテムではない場合
            {
                toggle.isOn = false;
                oldmode = 0;
                notOn = true;
                if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber != 3)
                {
                    if (GManager.instance.isEnglish == 0)
                    {
                        _toggleText.text = "使用するアイテムではありません";
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        _toggleText.text = "CNot an item to be used.";
                    }
                }
                else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 3)
                {
                    if (GManager.instance.isEnglish == 0)
                    {
                        _toggleText.text = "現在はセットできません";
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        _toggleText.text = "Cannot be set at this time.";
                    }
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]]._quickset == -1)//効果があり、まだセットされてないアイテムの場合
            {
                toggle.isOn = false;
                oldmode = 0;
                notOn = false;
                if (GManager.instance.isEnglish == 0)
                {
                    _toggleText.text = "アイテムスロットにセットする";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    _toggleText.text = "Set in item slot.";
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]]._quickset != -1)//既にセットしてあるアイテムの場合
            {
                toggle.isOn = true;
                oldmode = 1;
                notOn = false;
                if (GManager.instance.isEnglish == 0)
                {
                    _toggleText.text = "アイテムスロットにセット済み";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    _toggleText.text = "Already set in item slot.";
                }
            }
        }
        else //想定外な場合
        {
            itemimage.sprite = nullimage;
            nameText.text = "????";
            if (GManager.instance.isEnglish == 0)
            {
                numberText.text = "所持数:???\n売値:???\nレア度:??";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                numberText.text = "Number:???\nPrice: ???\nRarity:??";
            }
            scriptText.text = "????????";
            //追加
            for (int i = 0; i < quickItem_image.Length;)
            {
                quickItem_image[i].sprite = nullimage;
                i++;
            }
            toggle.isOn = false;
            oldmode = 0;
            notOn = true;
            if (GManager.instance.isEnglish == 0)
            {
                _toggleText.text = "現在はセットできません";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                _toggleText.text = "Cannot be set at this time.";
            }
        }
    }

    public void SelectMinus() //アイテム項目を戻って切り替える、セレクトボタン
    {
        if (onItem.Length  == 0)
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
    public void SelectPlus() //アイテム項目を進んで切り替える、セレクトボタン
    {
        if (onItem.Length == 0)
        {
            audioS.PlayOneShot(notse);
        }
        else if (selectnumber < (onItem.Length - 1))
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
    //ここでアイテムの使用確認を行う
    public void MenuUI()
    {
        if (onItem != null && onItem.Length != 0)
        {
            if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber != 0 && GManager.instance.ItemID[onItem[selectnumber]].eventnumber != -1)
            {
                audioS.PlayOneShot(onse);
                usetrg = false;
                selectmenuUI.SetActive(true);//効果があるアイテムの場合は、使用するかどうか確認するUIを表示
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 0 || GManager.instance.ItemID[onItem[selectnumber]].eventnumber == -1)
            {
                audioS.PlayOneShot(notse);
            }
        }
    }

    //ここで効果があるアイテムを使用
    public void ItemSet()
    {
        if (GManager.instance.ItemID[onItem[selectnumber]].itemnumber > 0)
        {
            if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 1)//HP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 5;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 2)//MP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 5;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 3 && GManager.instance.bossbattletrg == 0 && SceneManager.GetActiveScene().name == "stage" + GManager.instance.stageNumber && GManager.instance.stageNumber != 0)//セーブアイテム
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                SaveDate();
                GManager.instance.setrg = 8;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 4)//ユニムーシュ
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.Pstatus[GManager.instance.playerselect].inputExp = GManager.instance.Pstatus[GManager.instance.playerselect].maxExp;
                GManager.instance.setrg = 7;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 5 && P != null && ps != null)//毒消し
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                ps.poisontime = 0;
                ps.poisontrg = false;
                if (ps.EffectObj != null)
                {
                    Destroy(ps.EffectObj.gameObject);
                }
                GManager.instance.setrg = 8;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 6 && P != null && ps != null)//火傷治し
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                ps.flametime = 0;
                ps.flametrg = false;
                ps.infinitytime = 0;
                ps.infinitytrg = false;
                if (ps.EffectObj != null)
                {
                    Destroy(ps.EffectObj.gameObject);
                }
                GManager.instance.setrg = 8;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 7)//エリクサー
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                for (int i = 0; i < GManager.instance.Pstatus.Length;)
                {
                    if (GManager.instance.Pstatus[i].getpl > 0)
                    {
                        GManager.instance.Pstatus[i].hp = GManager.instance.Pstatus[i].maxHP;
                        GManager.instance.Pstatus[i].mp = GManager.instance.Pstatus[i].maxMP;
                    }
                    i++;
                }
                ps.poisontime = 0;
                ps.poisontrg = false;
                ps.flametime = 0;
                ps.flametrg = false;
                ps.infinitytime = 0;
                ps.infinitytrg = false;
                ps.icetime = 0;
                ps.mudtime = 0;
                ps.watertime = 0;
                ps.darktrg = false;
                if (ps.setblood != null)
                {
                    Destroy(ps.setblood.gameObject);
                }
                ps.bloodTrg = false;
                if (ps.EffectObj != null)
                {
                    Destroy(ps.EffectObj.gameObject);
                }
                GManager.instance.setrg = 14;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 8 && P != null && ps != null)//粉
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                randomN = Random.Range(0, 13);
                if (randomN == 0)//hp小回復
                {
                    GManager.instance.setrg = 16;
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp += 5;
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                    }
                }
                else if (randomN == 1)//mp小回復
                {
                    GManager.instance.setrg = 16;
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp += 5;
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                    }
                }
                else if (randomN == 2)//hp中回復
                {
                    GManager.instance.setrg = 15;
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 3);
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                    }
                }
                else if (randomN == 3)//mp中回復
                {
                    GManager.instance.setrg = 15;
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxMP / 3);
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                    {
                        GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                    }
                }
                else if (randomN == 4)//毒回復
                {
                    ps.poisontime = 0;
                    ps.poisontrg = false;
                    if (ps.EffectObj != null)
                    {
                        Destroy(ps.EffectObj.gameObject);
                    }
                    GManager.instance.setrg = 8;
                }
                else if (randomN == 5)//火傷回復
                {
                    ps.flametime = 0;
                    ps.flametrg = false;
                    ps.infinitytime = 0;
                    ps.infinitytrg = false;
                    if (ps.EffectObj != null)
                    {
                        Destroy(ps.EffectObj.gameObject);
                    }
                    GManager.instance.setrg = 8;
                }
                else if (randomN == 6)//エリクサーもどき
                {
                    for (int i = 0; i < GManager.instance.Pstatus.Length;)
                    {
                        if (GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp < 1)
                        {
                            GManager.instance.Pstatus[i].hp = GManager.instance.Pstatus[i].maxHP / 3;
                            GManager.instance.Pstatus[i].mp = GManager.instance.Pstatus[i].maxMP;
                        }
                        i++;
                    }
                    GManager.instance.setrg = 14;
                }
                else if (randomN == 7)//LvUP
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].inputExp = GManager.instance.Pstatus[GManager.instance.playerselect].maxExp;
                    GManager.instance.setrg = 7;
                }
                else if (randomN == 8)//セーブ
                {
                    if (GManager.instance.bossbattletrg == 0 && SceneManager.GetActiveScene().name == "stage" + GManager.instance.stageNumber && GManager.instance.stageNumber != 0)
                    {
                        SaveDate();
                        GManager.instance.setrg = 8;
                    }
                }
                else if (randomN == 9)//燃焼
                {
                    ps.flametime = 0;
                    ps.flametrg = true;
                    ps.EffectObj = Instantiate(GManager.instance.effectobj[7], P.transform.position, P.transform.rotation, P.transform);
                    GManager.instance.setrg = 18;
                }
                else if (randomN == 10)//毒
                {
                    ps.poisontime = 0;
                    ps.poisontrg = true;
                    ps.EffectObj = Instantiate(GManager.instance.effectobj[6], P.transform.position, P.transform.rotation, P.transform);
                    GManager.instance.setrg = 17;
                }
                else if (randomN == 11)//神の裁き
                {
                    ps.holytrg = true;
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp /= 2;
                    ps.EffectObj = Instantiate(GManager.instance.effectobj[11], P.transform.position, P.transform.rotation, P.transform);
                    GManager.instance.setrg = 19;
                }
                else if (randomN == 12)//闇の呪い
                {
                    ps.darktrg = true;
                    ps.EffectObj = Instantiate(GManager.instance.effectobj[11], P.transform.position, P.transform.rotation, P.transform);
                    GManager.instance.setrg = 20;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 9)//HP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 16;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 15;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 11)//HP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 15;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 3);
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 13)//HP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 14;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 2);
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 10)//MP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 16;
                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 15;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 12)//MP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 15;
                GManager.instance.Pstatus[GManager.instance.playerselect].mp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxMP / 3);
                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 14)//MP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 14;
                GManager.instance.Pstatus[GManager.instance.playerselect].mp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxMP / 2);
                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 15)//人工エリクサー
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 14;
                for (int i = 0; i < GManager.instance.Pstatus.Length;)
                {
                    if (GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp < 1)
                    {
                        GManager.instance.Pstatus[i].hp = GManager.instance.Pstatus[i].maxHP / 3;
                        GManager.instance.Pstatus[i].mp = GManager.instance.Pstatus[i].maxMP;
                    }
                    i++;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 16)//HP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 23;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 10;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 17)//MP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 22;
                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 10;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 18 && P != null && ps != null)//万能
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                ps.poisontime = 0;
                ps.poisontrg = false;
                ps.flametime = 0;
                ps.flametrg = false;
                ps.infinitytime = 0;
                ps.infinitytrg = false;
                ps.icetime = 0;
                ps.mudtime = 0;
                ps.watertime = 0;
                ps.darktrg = false;
                if (ps.setblood != null)
                {
                    Destroy(ps.setblood.gameObject);
                }
                ps.bloodTrg = false;
                if (ps.EffectObj != null)
                {
                    Destroy(ps.EffectObj.gameObject);
                }
                GManager.instance.setrg = 15;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 19 && P != null && ps != null)//爆炎
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.itemMagicID = 14;
                Instantiate(GManager.instance.effectobj[12], P.transform.position, P.transform.rotation, P.transform);
                GManager.instance.setrg = 1;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 20 && P != null && ps != null)//零水
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.itemMagicID = 22;
                Instantiate(GManager.instance.effectobj[12], P.transform.position, P.transform.rotation, P.transform);
                GManager.instance.setrg = 1;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 21 && P != null && ps != null)//風来
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.itemMagicID = 8;
                Instantiate(GManager.instance.effectobj[12], P.transform.position, P.transform.rotation, P.transform);
                GManager.instance.setrg = 1;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 22)//HP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 15;
                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 15;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
                else if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 23)//HP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 5;
                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 5;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
                else if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 24)//完全蘇生
            {
                usetrg = true;
                GManager.instance.setmenu += 1;
                Instantiate(GManager.instance.effectobj[16], this.transform.position, this.transform.rotation);
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 25)//HP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 31;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 10;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = "60秒間水属性のダメージを軽減します";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "Reduces water type damage for 60 seconds";
                }
                GManager.instance.Triggers[94] = 1;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 26)//瞬間移動
            {
                usetrg = true;
                GManager.instance.setmenu += 1;
                Instantiate(GManager.instance.effectobj[17], this.transform.position, this.transform.rotation);
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 27)//MP回復悪天候無効化
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 20;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                }
                ps.itemtime_notWeather = 300;
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = "5分間悪天候の影響を無効化します";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "Disables the effects of inclement weather for 5 minutes";
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 28)//ランプ
            {
                usetrg = true;
                GManager.instance.setrg = 6;
                if (GManager.instance.Triggers[102] == 0)
                {
                    GManager.instance.Triggers[102] = 1;
                }
                else if (GManager.instance.Triggers[102] == 1)
                {
                    GManager.instance.Triggers[102] = 0;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 29)//HP回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 40;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 30)//最大HPMP増加
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].maxHP += 3;
                GManager.instance.Pstatus[GManager.instance.playerselect].maxMP += 3;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 31)//HP回復水属性攻撃無効
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 20;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = "120秒間水属性のダメージを無効化します";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "Disables water type damage for 120 seconds.";
                }
                GManager.instance.Triggers[94] = 2;
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 32)//HP回復HP成長値1増加
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].add_hp += 1;
                GManager.instance.Pstatus[GManager.instance.playerselect].hp += 15;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                }
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = "LvUP時のHP上昇値が1増えた";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "The HP increase at LvUP was increased by 1.";
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 33)//MP回復全最大値成長値2増加
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 7;
                GManager.instance.Pstatus[GManager.instance.playerselect].add_hp += 2;
                GManager.instance.Pstatus[GManager.instance.playerselect].add_mp += 2;
                GManager.instance.Pstatus[GManager.instance.playerselect].add_at += 2;
                GManager.instance.Pstatus[GManager.instance.playerselect].add_df += 2;
                GManager.instance.Pstatus[GManager.instance.playerselect].maxHP += 2;
                GManager.instance.Pstatus[GManager.instance.playerselect].maxMP += 2;
                GManager.instance.Pstatus[GManager.instance.playerselect].attack += 2;
                GManager.instance.Pstatus[GManager.instance.playerselect].defense += 2;
                GManager.instance.Pstatus[GManager.instance.playerselect].mp += 40;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].mp > GManager.instance.Pstatus[GManager.instance.playerselect].maxMP)
                {
                    GManager.instance.Pstatus[GManager.instance.playerselect].mp = GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                }
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = "LvUP時の各ステータス上昇値が2増えた";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "The value of each status increase at LvUP was increased by 2.";
                }
            }
            else if (GManager.instance.ItemID[onItem[selectnumber]].eventnumber == 34 && GManager.instance.Triggers[178] < 1)//持続回復
            {
                usetrg = true;
                GManager.instance.ItemID[onItem[selectnumber]].itemnumber -= 1;
                GManager.instance.setrg = 22;
                GManager.instance.Triggers[178] = GManager.instance.playerselect + 1;
                if (GManager.instance.isEnglish == 0)
                {
                    GManager.instance.txtget = "デビ・コーラの効果で3分間持続回復されます";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    GManager.instance.txtget = "The use of Devi・Cola gave a recovery effect that lasted for 3 minutes.";
                }
            }
            else if (usetrg == false)
            {
                audioS.PlayOneShot(notse);
            }
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
        //----------------------------------------------
        SetUI();//使用後、UIを再表示
        selectmenuUI.SetActive(false);
    }
    
    //キャンセルボタン用
    public void NotSet()
    {
        audioS.PlayOneShot(notse);
        selectmenuUI.SetActive(false);
    }

    //PlayerPrefsでセーブする
    void SaveDate()
    {
        PlayerPrefs.SetInt("housetrg", GManager.instance.houseTrg);
        PlayerPrefs.SetInt("dayc", GManager.instance.daycount);
        PlayerPrefs.SetInt("coin", GManager.instance.Coin);
        for (int i = 0; i < GManager.instance.EventNumber.Length;)
        {
            PlayerPrefs.SetInt("EvN" + i, GManager.instance.EventNumber[i]);
            i++;
        }
        for (int i = 0; i < GManager.instance.freenums.Length;)
        {
            PlayerPrefs.SetFloat("freenums" + i, GManager.instance.freenums[i]);
            i++;
        }
        PlayerPrefs.SetFloat("posX", GManager.instance.posX);
        PlayerPrefs.SetFloat("posY", GManager.instance.posY);
        PlayerPrefs.SetFloat("posZ", GManager.instance.posZ);
        PlayerPrefs.SetInt("stageN", GManager.instance.stageNumber);
        for (int i = 0; i < GManager.instance.ItemID.Length;)
        {
            PlayerPrefs.SetInt("itemnumber" + i, GManager.instance.ItemID[i].itemnumber);
            PlayerPrefs.SetInt("itemget" + i, GManager.instance.ItemID[i].gettrg);
            PlayerPrefs.SetInt("item_quickset" + i, GManager.instance.ItemID[i]._quickset);
            PlayerPrefs.SetInt("item_equalsset" + i, GManager.instance.ItemID[i]._equalsset);
            PlayerPrefs.SetInt("pl_equalsselect" + i, GManager.instance.ItemID[i].pl_equalsselect);
            i++;
        }
        //---------------

        for (int i = 0; i < GManager.instance.Quick_itemSet.Length;)
        {
            PlayerPrefs.SetInt("quick_itemset" + i, GManager.instance.Quick_itemSet[i]);
            i++;
        }
        for (int i = 0; i < GManager.instance.P_equalsID.Length;)
        {
            PlayerPrefs.SetInt("hand_equals" + i, GManager.instance.P_equalsID[i].hand_equals);
            PlayerPrefs.SetInt("accessory_equals" + i, GManager.instance.P_equalsID[i].accessory_equals);
            i++;
        }
        //---------------
        for (int i = 0; i < GManager.instance.Pstatus.Length;)
        {
            PlayerPrefs.SetInt("addhp" + i, GManager.instance.Pstatus[i].add_hp);
            PlayerPrefs.SetInt("addmp" + i, GManager.instance.Pstatus[i].add_mp);
            PlayerPrefs.SetInt("addat" + i, GManager.instance.Pstatus[i].add_at);
            PlayerPrefs.SetInt("adddf" + i, GManager.instance.Pstatus[i].add_df);

            PlayerPrefs.SetInt("pmaxhp" + i, GManager.instance.Pstatus[i].maxHP);
            PlayerPrefs.SetInt("php" + i, GManager.instance.Pstatus[i].hp);
            PlayerPrefs.SetInt("pmaxmp" + i, GManager.instance.Pstatus[i].maxMP);
            PlayerPrefs.SetInt("pmp" + i, GManager.instance.Pstatus[i].mp);
            PlayerPrefs.SetInt("pdf" + i, GManager.instance.Pstatus[i].defense);
            PlayerPrefs.SetInt("pat" + i, GManager.instance.Pstatus[i].attack);
            PlayerPrefs.SetInt("plv" + i, GManager.instance.Pstatus[i].Lv);
            PlayerPrefs.SetInt("pmaxexp" + i, GManager.instance.Pstatus[i].maxExp);
            PlayerPrefs.SetInt("pinputexp" + i, GManager.instance.Pstatus[i].inputExp);
            PlayerPrefs.SetInt("pselectskill" + i, GManager.instance.Pstatus[i].selectskill);
            PlayerPrefs.SetInt("pselectmagic" + i, GManager.instance.Pstatus[i].magicselect);
            for (int j = 0; j < GManager.instance.Pstatus[i].inputskill.Length;)
            {
                PlayerPrefs.SetInt("pinputskill" + i + "" + j, GManager.instance.Pstatus[i].inputskill[j]);
                j++;
            }
            for (int j = 0; j < GManager.instance.Pstatus[i].getMagic.Length;)
            {
                PlayerPrefs.SetInt("pgetmagictrg" + i + "" + j, GManager.instance.Pstatus[i].getMagic[j].gettrg);
                j++;
            }
            for (int j = 0; j < GManager.instance.Pstatus[i].magicSet.Length;)
            {
                PlayerPrefs.SetInt("pmagicset" + i + "" + j, GManager.instance.Pstatus[i].magicSet[j]);
                j++;
            }
            PlayerPrefs.SetInt("getpl" + i, GManager.instance.Pstatus[i].getpl);
            i++;
        }
        PlayerPrefs.SetInt("plselect", GManager.instance.playerselect);
        for (int i = 0; i < GManager.instance.Triggers.Length;)
        {
            PlayerPrefs.SetInt("gmtrg" + i, GManager.instance.Triggers[i]);
            i++;
        }
        for (int i = 0; i < GManager.instance.missionID.Length;)
        {
            PlayerPrefs.SetInt("inputmission" + i, GManager.instance.missionID[i].inputmission);
            i++;
        }
        for (int i = 0; i < GManager.instance.achievementsID.Length;)
        {
            PlayerPrefs.SetInt("achiget" + i, GManager.instance.achievementsID[i].gettrg);
            i++;
        }
        for (int i = 0; i < GManager.instance.enemynoteID.Length;)
        {
            PlayerPrefs.SetInt("enemynoteget" + i, GManager.instance.enemynoteID[i].gettrg);
            i++;
        }
        PlayerPrefs.SetFloat("audioMax", GManager.instance.audioMax);
        PlayerPrefs.SetFloat("seMax", GManager.instance.seMax);
        PlayerPrefs.SetInt("Mode", GManager.instance.mode);
        PlayerPrefs.SetInt("isEn", GManager.instance.isEnglish);
        PlayerPrefs.SetInt("Reduction", GManager.instance.reduction);
        PlayerPrefs.SetFloat("suntime", GManager.instance.sunTime);
        PlayerPrefs.SetInt("viewUp", GManager.instance.autoviewup);
        PlayerPrefs.SetInt("longDash", GManager.instance.autolongdash);
        PlayerPrefs.SetInt("autoattack", GManager.instance.autoattack);
        PlayerPrefs.SetFloat("rotpivot", GManager.instance.rotpivot);
        for (int i = 0; i < GManager.instance.mobDsTrg.Length;)
        {
            PlayerPrefs.SetInt("mdt" + i, GManager.instance.mobDsTrg[i]);
            i++;
        }
        PlayerPrefs.Save();
    }
}