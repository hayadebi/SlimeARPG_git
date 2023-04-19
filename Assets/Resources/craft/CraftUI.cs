using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CraftUI : MonoBehaviour
{
    //【クラフトUIのスクリプト】※スライムディストピアではスライムが素材を食べてクラフトします。

    public Image buttonImage; //ボタン自体をいじる時用
    [Header("取得")]
    public Animator slimeAnim; //クラフト時の食べるアニメーション用
    public Animator effectAnim; //クラフト時の爆発、煙アニメーション用
    public Animator popAnim; //クラフト対象のアイテムをアニメーションで動かす用
    public AudioSource audioS; 
    public AudioClip selectse; //項目を切り替えた時の効果音
    public AudioClip onse; //決定などの効果音
    public AudioClip notse; //キャンセルなどの効果音
    public AudioClip slimese; //スライムの鳴き声、効果音
    public AudioClip createse; //クラフトの効果音
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
    public int[] onItem;
    private int boxnumber = 0;
    private int inputnumber = 0;
    private int uiMode = -1;
    private float _timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        uiMode = -1;//現在のクラフトUI内の状態をリセット
        //エフェクト、アニメーション関連もリセット
        effectAnim.StopPlayback();
        effectAnim.gameObject.SetActive(false);
        popAnim.SetInteger("Anumber", 0);
        //ゲームマネージャー内のクラフトレシピ達を格納
        for (int i = 0; GManager.instance._craftRecipe.Length > i;)
        {
            if(GManager.instance._craftRecipe[i].get_recipe > 0)
                boxnumber += 1;
            i += 1;
        }
        onItem = new int[boxnumber];
        for (int i = 0; GManager.instance._craftRecipe.Length > i;)
        {
            if (GManager.instance._craftRecipe[i].get_recipe > 0)
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
        //クラフトUI内の状態に応じて色や表示を切り替える
        if (GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id != -1 && uiMode == 3 && craftItem_image.color != new Color(1, 1, 1, 1f))
        {
            craftItem_image.color = new Color(1, 1, 1, 1f);
            craftItem_number.color = new Color(1, 1, 1, 1f);
        }
        else if (GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id != -1 && uiMode != 3 && craftItem_image.color != new Color(1, 1, 1, 0.5f))
        {
            craftItem_image.color = new Color(1, 1, 1, 0.5f);
            craftItem_number.color = new Color(1, 1, 1, 0.5f);
        }
        if(uiMode == 4)
        {
            if(_timer >= 0.5f)
            {
                _timer -= (Time.deltaTime / 2);
                craftItem_image.color = new Color(1, 1, 1, _timer);
            }
            else if(_timer < 0.5f)
            {
                uiMode = 0;
                buttonImage.color = new Color(1, 1, 1, 1f);
                SetUI();
            }
        }
    }
    //クラフトUIを表示(呼び出して使う)
    public void SetUI()
    {
        if (onItem == null || onItem.Length == 0)//想定外
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
        else if (onItem[selectnumber] >= 0 && onItem.Length > 0 && selectnumber != -1 && onItem.Length <= GManager.instance._craftRecipe.Length && GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id != -1)
        {
            //それぞれ条件に応じてレシピ情報を表示
            craftItem_image.sprite = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemimage;
            craftItem_number.text = ""+GManager.instance._craftRecipe[onItem[selectnumber]].craftGet_number;
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
            uiMode = 0;
            for (int i = 0; i < material_image.Length;)
            {
                if (GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i] != -1 && GManager.instance._craftRecipe[onItem[selectnumber]].materialSet_number[i] <= GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemnumber)
                {
                    if (uiMode != -1)
                    {
                        uiMode = 1;
                    }
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
                    material_number[i].text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemnumber+"/" +GManager.instance._craftRecipe[onItem[selectnumber]].materialSet_number[i];
                }
                else if (GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i] != -1 && GManager.instance._craftRecipe[onItem[selectnumber]].materialSet_number[i] > GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemnumber)
                {
                    uiMode = -1;
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
                    material_number[i].text = GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemnumber+"/" +GManager.instance._craftRecipe[onItem[selectnumber]].materialSet_number[i];
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
    }

    public void SelectMinus() //レシピ項目を戻って切り替える、セレクトボタン
    {
        if (onItem.Length == 0)
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
    public void SelectPlus() //レシピ項目を進んで切り替える、セレクトボタン
    {
        if (onItem.Length == 0)
        {
            audioS.PlayOneShot(notse);
        }
        else if (selectnumber < (onItem.Length - 1))
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
    //----
    public void MenuUI() //クラフトボタンが押された時
    {
        if (onItem != null && onItem.Length != 0)
        {
            if (uiMode == 1)//クラフトUI内の状態が条件と一致する場合はクラフト開始
            {
                uiMode = 2;
                audioS.PlayOneShot(onse);
                buttonImage.color = new Color(1, 1, 1, 0.5f);
                StartCoroutine(CreatePlay());
            }
            else //想定外
            {
                audioS.PlayOneShot(notse);
            }
        }
    }

    IEnumerator CreatePlay()
    {
        //選択中のクラフトレシピ内にある素材を調べ、反映
        for (int i = 0; i < material_image.Length;)
        {
            if (GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i] != -1)
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
            i++;
        }
        //スライムの食べるアニメーションを再生
        slimeAnim.SetInteger("Anumber", 1);
        audioS.PlayOneShot(slimese);
        //ちょっとだけ、アニメーション用に待つ
        yield return new WaitForSeconds(0.45f);

        //エフェクトも表示させる
        effectAnim.gameObject.SetActive(true);
        effectAnim.Play("geteffect");
        uiMode = 3;//状態を進行
        craftItem_image.color = new Color(1, 1, 1, 1f);
        craftItem_number.color = new Color(1, 1, 1, 1f);
        
        audioS.PlayOneShot(createse);
        //対象アイテムを手持ちに増やす
        GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftItem_id].itemnumber += GManager.instance._craftRecipe[onItem[selectnumber]].craftGet_number;
        //各クラフトレシピ内でついでに用意されてる副産物項目も、指定されてる場合はそれも手持ちに増やす
        if(GManager.instance._craftRecipe[onItem[selectnumber]].craftSub_id != -1)
        {
            GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].craftSub_id].itemnumber += GManager.instance._craftRecipe[onItem[selectnumber]].craftSub_number;
        }
        for (int i = 0; i < GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id.Length;)
        {
            if (GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i] != -1)
            {
                GManager.instance.ItemID[GManager.instance._craftRecipe[onItem[selectnumber]].materialItem_id[i]].itemnumber -= GManager.instance._craftRecipe[onItem[selectnumber]].materialSet_number[i];
            }
            i++;
        }
        //ここからクラフト状態を終わらせる準備をし、◆のタイマー指定から一定時間後にまたクラフトできるようにする
        popAnim.SetInteger("Anumber", 1);
        yield return new WaitForSeconds(1f);
        
        slimeAnim.SetInteger("Anumber", 0);
        effectAnim.StopPlayback ();
        effectAnim.gameObject.SetActive(false);

        popAnim.SetInteger("Anumber", 0);
        uiMode = 4;
        _timer = 1;//◆
    }
    
}