using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//enumを使うために必要
using System;
public class SecondPlayer : MonoBehaviour
{
    [Header("キャラクター見た目本体")]
    public GameObject player;
    private Rigidbody2D rb;
    private float xSpeed = 0.0f;
    private float ySpeed = 0.0f;
    public Animator anim;
    private int oldPlayer = 0;
    [Header("クイックアイテムセレクト関連")]
    public Transform contentTransform;
    public GameObject itemCanvas;
    public GameObject itemObject;
    public Image itemIcon;
    public Text itemKeytext;
    public EnterCode enterCode;
    private List<GameObject> tmpSelect = null;
    [HideInInspector]
    public AudioSource audioSource;
    [Header("照明オブジェクト")]
    public GameObject lightObject;
    [Header("精霊オブジェクト")]
    public GameObject fairyObject;
    private float cooltime = 0;
    [Header("メインメニューUI")]
    public GameObject mainMenu;
    public struct QuickList
    {
        public string keyCode;
        public int itemId;
    }
    public QuickList[] quickList = new QuickList[9];
    private bool isSearch = false;
    void Start()
    {
        if (GetComponent<AudioSource>()) audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2Dコンポーネントを取得
        anim.runtimeAnimatorController = DataManager.instance.Pstatus[DataManager.instance.playerselect[1]].animC;
        oldPlayer = DataManager.instance.playerselect[1];
        anim.SetInteger("trg", 0);
    }

    void FixedUpdate()
    {
        if (DataManager.instance.Triggers[0] > 0)
        {
            if (oldPlayer != DataManager.instance.playerselect[1])//操作プレイヤー変更反映
            {
                anim.runtimeAnimatorController = DataManager.instance.Pstatus[DataManager.instance.playerselect[1]].animC;
                oldPlayer = DataManager.instance.playerselect[1];
                anim.SetInteger("trg", 0);
            }
            xSpeed = 0;
            ySpeed = 0;
            rb.velocity = Vector2.zero;
            //キー入力されたら行動する
            float horizontalKey = Input.GetAxis("Horizontal");
            float verticalKey = Input.GetAxis("Vertical");

            if (horizontalKey > 0)
            {
                anim.SetInteger("trg", 1);
                player.transform.localScale = new Vector3(-0.064f, 0.064f, 0.064f);
                xSpeed = DataManager.instance.Pstatus[DataManager.instance.playerselect[1]].speed * 16 * Time.fixedDeltaTime;
            }
            else if (horizontalKey < 0)
            {
                anim.SetInteger("trg", 1);
                player.transform.localScale = new Vector3(0.064f, 0.064f, 0.064f);
                xSpeed = -DataManager.instance.Pstatus[DataManager.instance.playerselect[1]].speed * 16 * Time.fixedDeltaTime;
            }
            else
            {
                xSpeed = 0.0f;
                rb.velocity = Vector2.zero;
            }
            if (verticalKey > 0)
            {
                anim.SetInteger("trg", 1);
                ySpeed = DataManager.instance.Pstatus[DataManager.instance.playerselect[1]].speed * 16 * Time.fixedDeltaTime;
            }
            else if (verticalKey < 0)
            {
                anim.SetInteger("trg", 1);
                ySpeed = -DataManager.instance.Pstatus[DataManager.instance.playerselect[1]].speed * 16 * Time.fixedDeltaTime;
            }
            else
            {
                ySpeed = 0.0f;
                if (rb.velocity != Vector2.zero) rb.velocity = Vector2.zero;
            }

            if (horizontalKey == 0 && verticalKey == 0)
            {
                xSpeed = 0;
                ySpeed = 0;
                if (rb.velocity != Vector2.zero) rb.velocity = Vector3.zero;
                anim.SetInteger("trg", 0);
            }
            rb.velocity = new Vector2(xSpeed, ySpeed);

            //クイックアイテムセレクト
            //UI表示
            if ((Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.Q)) && cooltime <= 0)
            {
                cooltime = 0.5f;
                
                int oldid = -1;
                if (DataManager.instance.Triggers[4] == 0)
                {
                    GManager.instance.setrg = 6;
                    if (tmpSelect != null)
                    {
                        foreach (GameObject tmp in tmpSelect)
                        {
                            Destroy(tmp.gameObject);
                        }
                        tmpSelect = null;
                    }
                    tmpSelect = new List<GameObject>();
                    DataManager.instance.Triggers[4] = 1;
                    itemCanvas.SetActive(true);
                    for (int n = 1; n < 10;)
                    {
                        for (int i = 0; i < DataManager.instance.ItemID.Count;)
                        {
                            if (oldid < i && DataManager.instance.ItemID[i].gettrg > 0 && DataManager.instance.ItemID[i].itemnumber > 0 && DataManager.instance.ItemID[i].isFieldUse)
                            {
                                oldid = i;

                                quickList[n - 1].keyCode = "Alpha" + n.ToString();
                                quickList[n - 1].itemId = i;
                                itemIcon.sprite = DataManager.instance.ItemID[i].itemimage;
                                itemKeytext.text = n.ToString();
                                enterCode.CodeNumber = n;
                                GameObject tmpobj = Instantiate(itemObject, contentTransform.position, contentTransform.rotation, contentTransform);
                                tmpobj.SetActive(true);
                                tmpSelect.Add(tmpobj);
                                n++;
                            }
                            i++;
                        }
                        n++;
                    }
                }
                else if (DataManager.instance.Triggers[4] > 0)
                {
                    SelectClose(true);
                }


            }

            //メインメニュー表示
            if (cooltime <= 0&&(Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.X)))
            {
                if (DataManager.instance.Triggers[4] == 0)
                {
                    cooltime = 0.5f;
                    GManager.instance.setrg = 6;
                    DataManager.instance.Triggers[4] = 1;
                    mainMenu.SetActive(true);
                }
                else if (DataManager.instance.Triggers[4] == 1)
                {
                    cooltime = 0.5f;
                    
                    MenuClose();
                }
            }

            if (cooltime >= 0) cooltime -= Time.deltaTime;
            //UI表示中のアイテム使用操作
            if (DataManager.instance.Triggers[4] > 0 && Input.anyKeyDown)
            {
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        CodeInvoke(code.ToString());
                        break;
                    }
                }
            }

            //
        }
        else if (rb.velocity != Vector2.zero || anim.GetInteger("trg") != 0)
        {
            anim.SetInteger("trg", 0);
            rb.velocity = Vector2.zero;
        }
    }
    void SelectClose(bool trg )
    {
        GManager.instance.setrg = 2;
        if(trg)DataManager.instance.Triggers[4] = 0;
        
        itemCanvas.SetActive(false);
    }
    public void MenuClose()
    {
        GManager.instance.setrg = 2;
        DataManager.instance.Triggers[4] = 0;

        mainMenu.SetActive(false);
    }
    //アイテムのイベント実行
    public IEnumerator CodeEvent(int id,bool trg)
    {
        DataManager.item ItemId = DataManager.instance.ItemID[id];
        switch (DataManager.instance.ItemID[id].itemType)
        {
            case DataManager.ItemType.Fairy://精霊瓶
                isSearch = true;
                DataManager.instance.TextGet = "精霊瓶から解放し、1分間程の道案内が有効化しました";
                if (audioSource) audioSource.PlayOneShot(DataManager.instance.ItemID[id].itemSound);

                ItemId = DataManager.instance.ItemID[id];
                ItemId.itemnumber -= 1;
                DataManager.instance.ItemID[id] = ItemId;
                fairyObject.SetActive(true);

                SelectClose(trg);
                break;
            case DataManager.ItemType.Lamp://ランプ
                isSearch = true;
                if (audioSource) audioSource.PlayOneShot(DataManager.instance.ItemID[id].itemSound);
                if (!DataManager.instance.ItemID[id].isUsed)
                {
                    ItemId = DataManager.instance.ItemID[id];
                    ItemId.isUsed = true;
                    DataManager.instance.ItemID[id] = ItemId;
                    lightObject.SetActive(true);
                }
                else
                {
                    ItemId = DataManager.instance.ItemID[id];
                    ItemId.isUsed = false;
                    DataManager.instance.ItemID[id] = ItemId;
                    lightObject.SetActive(false);
                }
                SelectClose(trg);
                break;
            case DataManager.ItemType.Save://セーブ
                isSearch = true;
                DataManager.instance.TextGet = "記録手帳に全てをセーブした";
                if (audioSource) audioSource.PlayOneShot(DataManager.instance.ItemID[id].itemSound);
                ItemId = DataManager.instance.ItemID[id];
                ItemId.itemnumber -= 1;
                DataManager.instance.ItemID[id] = ItemId;
                DataManager.instance.AllSaveInvoke();
                SelectClose(trg);
                break;
            default:
                isSearch = true;
                if (audioSource) audioSource.PlayOneShot(DataManager.instance.ItemID[id].itemSound);
                ItemId = DataManager.instance.ItemID[id];
                ItemId.itemnumber -= 1;
                DataManager.instance.ItemID[id] = ItemId;
                SelectClose(trg);
                break;
        }
        yield return null;
    }
    public void CodeInvoke(string code)
    {
        isSearch = false;
        for (int q = 0; q < quickList.Length;)
        {
            for (int i = 0; i < DataManager.instance.ItemID.Count;)
            {
                if (!isSearch &&quickList[q].keyCode != "" && quickList[q].keyCode != " " && quickList[q].keyCode == code && quickList[q].itemId >= 0 && DataManager.instance.ItemID[quickList[q].itemId].itemnumber > 0)
                {
                    StartCoroutine(CodeEvent(quickList[q].itemId,true));
                }
                i++;
            }
            q++;
        }
    }
}
