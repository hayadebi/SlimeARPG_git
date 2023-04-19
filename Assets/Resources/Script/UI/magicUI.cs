using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class magicUI : MonoBehaviour
{
    //【現在操作しているスライムの魔法編成UIスクリプト】

    [Header("魔法UI内の選択側")]
    public AudioSource audioS; //魔法UIのAudioSourceを取得
    public AudioClip selectse; //選択時の効果音をセット
    public AudioClip notse; //キャンセル時等の効果音をセット
    public Text[] selectname; //魔法UI内の名前表示を取得
    public Text[] selectscript; //魔法UI内の説明表示を取得
    public Image[] selectimage; //魔法UI内の魔法画像表示を取得
    public Sprite nullimage; //想定外用に"?"画像をセット
    public int[] selectID; //各魔法UIIDを取得
    private int boxnumber = 0; //一時数値格納用
    private int addnumber = 0; //boxnumberからさらに調べて数値を格納
    public int selectnumber = 0; //選択中の魔法に対応するインデックス
    [Header("魔法UI内の編成側")]
    public AudioClip onse; //決定等の効果音をセット
    public Text[] setname; //各編成側の名前を取得
    public Text[] setscript; //各編成側の説明を取得
    public Image[] setimage; //各編成側の画像を取得
    public GameObject[] setUIenable; //各編成側のメインオブジェクトを取得
    public int[] setID; //各編成側のIDをセット
    int boxnumber2 = 0; //編成側用の一時数値格納用
    int addnumber2 = 0; //編成側用boxnumberからさらに調べて数値を格納
    public int selectnumber2; //編成側用選択中の魔法に対応するインデックス
    private int slsetM = 0; //選択中の位置とセレクトIDを照らし合わせるための一時変数
    private GameObject ms; //UI本体を取得
    private mainUI msmui; //UI本体内にメインUIスクリプト(本作内で大事なUI部分)があるか確認
    private GameObject P; //とあることのためにプレイヤーを取得
    private player ps; //とあることのためにプレイヤースクリプトを取得

    //ゲームマネージャー(本作において最も重要なスクリプト)内のマジックセットとマジックIDが合っているか確認するための一時変数
    private bool nottrg = false;
    // Start is called before the first frame update
    void Start()
    {
        //色々後から取得するやつ
        P = GameObject.Find("Player");
        ps = P.GetComponent<player>();
        ms = GameObject.Find("magic&skill");
        if(ms != null)
        {
            msmui = ms.GetComponent<mainUI>();
        }
        //ゲームマネージャー/プレイヤー情報/習得可能な魔法 を調べ、既に取得してる魔法IDだけ格納
        for (int i = 0; GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length > i;)
        {
            if (GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].gettrg > 0)
            {
                boxnumber += 1;
            }
            i += 1;
        }
        selectID = new int[boxnumber];
        if (boxnumber != 0)
        {
            for (int i = 0; GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length > i;)
            {
                if (GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].gettrg > 0)
                {
                    if (addnumber < boxnumber)
                    {
                        selectID[addnumber] = i;
                        addnumber += 1;
                    }
                }
                i += 1;
            }
        }
        //選択項目、セット項目の魔法情報表示を呼び出す
        SelectUI();
        SetUI();
    }
    void SetUI()//セット項目の魔法情報表示
    {
        setID = null;//一時格納IDをリセット
        boxnumber2 = 0;//一時数値変数をリセット
        addnumber2 = 0;//調べ用変数をリセット
        //選択中プレイヤーの魔法セットする位置が何も無いか調べ、無かったら一時数値変数に加算
        //(空白かどうかを別で調べないと後にエラー要因になる)
        for (int i = 0; GManager.instance.Pstatus[GManager.instance.playerselect].magicSet.Length > i;)
        {
            if (GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i] != -1)
            {
                boxnumber2 += 1;
            }
            i += 1;
        }
        setID = new int[boxnumber2];
        if (boxnumber2 != 0)//一時数値変数、つまり空白の位置が0個以外なのかしらべる
        {
            for (int i = 0; GManager.instance.Pstatus[GManager.instance.playerselect].magicSet.Length > i;)//再び魔法セットの位置を調べる
            {
                if (GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i] != -1)
                {
                    if (addnumber2 < 4)//さっきとは違い、調べ用変数が4未満の時だけ、セットIDにセットしつつ調べ用変数に加算
                    {
                        setID[addnumber2] = i;
                        addnumber2 += 1;
                    }
                }
                i += 1;
            }
        }
        //UIセット
        for (int i = 0; i < GManager.instance.Pstatus[GManager.instance.playerselect].magicSet.Length;)
        {
            if (setID == null)//セットIDがあるかどうか調べ、無い場合はそれ用のテキスト、画像などをセット
            {
                setname[i].text = "??? MP:?? ??????";
                setscript[i].text = "????????";
                setimage[i].sprite = nullimage;
                if (i == 0)
                {
                    setUIenable[i].SetActive(true);
                }
                else
                {
                    setUIenable[i].SetActive(false);
                }
            }
            else if (selectID != null && GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i] != -1)//セレクトIDや魔法セットを調べ、ある場合は機能してる間の表示をセット
            {
                setimage[i].sprite = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i]].magicimage;
                if (GManager.instance.isEnglish == 0)
                {
                    setname[i].text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i]].attacktype
                        + " MP:" + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i]].inputmagicpower
                        + " " + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i]].magicname;
                    setscript[i].text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i]].magicscript;
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    setname[i].text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i]].attacktype2
                        + " MP:" + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i]].inputmagicpower
                        + " " + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i]].magicname2;
                    setscript[i].text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[i]].magicscript2;
                }
                if (i == selectnumber2)
                {
                    setUIenable[i].SetActive(true);
                }
                else
                {
                    setUIenable[i].SetActive(false);
                }
            }
            else//上の条件以外の場合は、それ用のテキスト、画像などをセット
            {
                setname[i].text = "??? MP:?? ??????";
                setscript[i].text = "????????";
                setimage[i].sprite = nullimage;
                if (i == selectnumber2)
                {
                    setUIenable[i].SetActive(true);
                }
                else
                {
                    setUIenable[i].SetActive(false);
                }
            }
            i++;
        }
    }
    void SelectUI()//選択項目の魔法情報表示
    {
        for (int i = 0; i < selectname.Length;)
        {
            if (selectID != null && selectID.Length > (i + selectnumber) && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].gettrg > 0 && nottrg == false)
            {
                for (int j = 0; j < GManager.instance.Pstatus[GManager.instance.playerselect].magicSet.Length;)
                {
                    if (GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid != GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[j])
                    {
                        nottrg = false;
                    }
                    else
                    {
                        nottrg = true;
                        j = GManager.instance.Pstatus[GManager.instance.playerselect].magicSet.Length;
                    }
                    j++;
                }
            }
            if (selectID == null || GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length < i + selectnumber || nottrg == true)
            {
                selectname[i].text = "??? MP:?? ??????";
                selectscript[i].text = "????????";
                selectimage[i].sprite = nullimage;
            }
            else if (selectID != null && selectID.Length > (i + selectnumber) && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].gettrg > 0 && nottrg == false)
            {
                selectimage[i].sprite = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid].magicimage;
                if (GManager.instance.isEnglish == 0)
                {
                    selectname[i].text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid].attacktype + " MP:"+ GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid].inputmagicpower+ " "+ GManager.instance.MagicID [GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid ].magicname;
                    selectscript[i].text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid].magicscript;
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    selectname[i].text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid].attacktype2 + " MP:" + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid].inputmagicpower + " " + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid].magicname2;
                    selectscript[i].text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[i + selectnumber]].magicid].magicscript2;
                }
            }
            else
            {
                selectname[i].text = "??? MP:?? ??????";
                selectscript[i].text = "????????";
                selectimage[i].sprite = nullimage;
            }
            nottrg = false;
            i++;
        }
    }
    public void SelectMinus()//選択項目を戻って切り替える、セレクトボタン
    {
        if (selectnumber >= 4)
        {
            audioS.PlayOneShot(selectse);
            selectnumber -= 4;
            //----
            SelectUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SelectPlus()//選択項目を進んで切り替える、セレクトボタン
    {
        if (selectnumber + 4 < (GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length))
        {
            audioS.PlayOneShot(selectse);
            selectnumber += 4;
            //----
            SelectUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    //ここからの部分は、多分もっと他に良い方法があるかも。
    //とりあえずスライムディストピアでは、ややこしいやり方を採用してしまいました。

    //SetMagic関連はセット項目を押した時に操作するとこ
    public void SetMagic1()
    {
        if (selectnumber2 != 0)
        {
            audioS.PlayOneShot(selectse);
            selectnumber2 = 0;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SetMagic2()
    {
        if (selectnumber2 != 1 && GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[0] != -1)
        {
            audioS.PlayOneShot(selectse);
            selectnumber2 = 1;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SetMagic3()
    {
        if (selectnumber2 != 2 && GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[0] != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[1] != -1)
        {
            audioS.PlayOneShot(selectse);
            selectnumber2 = 2;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SetMagic4()
    {
        if (selectnumber2 != 3 && GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[0] != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[1] != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[2] != -1)
        {
            audioS.PlayOneShot(selectse);
            selectnumber2 = 3;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }

    //SelectMagic関連は選択項目を押した時に操作するとこ
    public void SelectMagic1()
    {
        slsetM = 0;
        if (selectID != null && selectID.Length > (slsetM + selectnumber) && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].gettrg > 0 && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].gettrg == 1)
        {
            audioS.PlayOneShot(onse);
            GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[selectnumber2] = GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].magicid;
            //----
            SelectUI();
            SetUI();
            ps.setMagic();
            if(msmui != null)
            {
                msmui.startUI();
                msmui.oldInt = "";
                msmui.oldmagicSprite = null;
                msmui.oldskillSprite = null;
            }

            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SelectMagic2()
    {
        slsetM = 1;
        if (selectID != null && selectID.Length > (slsetM + selectnumber) && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].gettrg > 0 && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].gettrg == 1)
        {
            audioS.PlayOneShot(onse);
            GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[selectnumber2] = GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].magicid;
            //----
            SelectUI();
            SetUI();
            ps.setMagic();
            if (msmui != null)
            {
                msmui.startUI();
                msmui.oldInt = "";
                msmui.oldmagicSprite = null;
                msmui.oldskillSprite = null;
            }
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SelectMagic3()
    {
        slsetM = 2;
        if (selectID != null && selectID.Length > (slsetM + selectnumber) && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].gettrg > 0 && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].gettrg == 1)
        {
            audioS.PlayOneShot(onse);
            GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[selectnumber2] = GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].magicid;
            //----
            SelectUI();
            SetUI();
            ps.setMagic();
            if (msmui != null)
            {
                msmui.startUI();
                msmui.oldInt = "";
                msmui.oldmagicSprite = null;
                msmui.oldskillSprite = null;
            }
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SelectMagic4()
    {
        slsetM = 3;
        if (selectID != null && selectID.Length > (slsetM + selectnumber) && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].gettrg > 0 && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].gettrg == 1)
        {
            audioS.PlayOneShot(onse);
            GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[selectnumber2] = GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[selectID[slsetM + selectnumber]].magicid;
            //----
            SelectUI();
            SetUI();
            ps.setMagic();
            if (msmui != null)
            {
                msmui.startUI();
                msmui.oldInt = "";
                msmui.oldmagicSprite = null;
                msmui.oldskillSprite = null;
            }
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
}
