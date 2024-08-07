using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemView : MonoBehaviour
{
    [Header("生成場所")]
    public Transform Content;
    [Header("オリジナルのコンテンツ")]
    public ContentItem itemObject;
    [Header("空白オブジェクト")]
    public GameObject br;
    [Header("選択中アイテムアイコン")]
    public Image selectedIcon;
    [Header("選択されていない状態のアイテムアイコン画像")]
    public Sprite noneIcon;
    [Header("選択中アイテム名等")]
    public Text selectedName;
    [Header("選択中アイテム説明")]
    public Text selectedDocument;
    [Header("選択中アイテム使用ボタン")]
    public GameObject selectedUsebutton;
    [HideInInspector]
    public int selectedId=-1;//表示と使用時に扱うアイテムID

    [Header("プレイヤー情報格納")]
    public SecondPlayer player;
    [Header("使用対象選択時のUI")]
    public GameObject selectPlayerUI;
    [Header("全使用対象")]
    public GameObject[] useTarget = new GameObject[3];

    private List<GameObject> itemList = new List<GameObject>();
    private DataManager.ItemType tmpItemType;
    // Start is called before the first frame update
    void Start()
    {
        ;
    }

    private void OnEnable()
    {
        AllUpdateView(false);
    }
    private void AllUpdateView(bool isSelectedview=false)
    {
        //選択中アイテムの表示リセット
        tmpItemType = DataManager.ItemType.None;
        selectedIcon.sprite = noneIcon;
        selectedName.text = "名前：????????\nレアリティ：N\n所持数：0個";
        selectedDocument.text = "????????????\n????????????";
        if (selectedUsebutton.activeSelf) selectedUsebutton.SetActive(false);
        //リストアップ
        if (itemList.Count > 0)
        {
            foreach (GameObject obj in itemList)
            {
                Destroy(obj.gameObject);
            }
            itemList = null;
            itemList = new List<GameObject>();

        }
        for (int i = 0; i < DataManager.instance.ItemID.Count;)
        {
            if (DataManager.instance.ItemID[i].gettrg > 0 && DataManager.instance.ItemID[i].itemnumber > 0)
            {
                itemObject.itemId = i;
                GameObject tmpobj = Instantiate(itemObject.gameObject, Content.position, Content.rotation, Content);
                tmpobj.SetActive(true);
                itemList.Add(tmpobj);
            }
            i++;
        }
        GameObject tmpbr = Instantiate(br, Content.position, Content.rotation, Content);
        tmpbr.SetActive(true);
        itemList.Add(tmpbr);

        //選択中アイテム表示
        if (isSelectedview&&selectedId==-1)
        {
            selectedIcon.sprite = noneIcon;
            selectedName.text = "名前：???????\nレアリティ：N\n所持数：0個";
            selectedDocument.text = "????????????????????\n????????????????????";
            selectedUsebutton.SetActive(false);
        }
        else if (isSelectedview)
        {
            selectedIcon.sprite = DataManager.instance.ItemID[selectedId].itemimage;
            selectedName.text = "名前：" + DataManager.instance.ItemID[selectedId].itemname + "\nレアリティ：" + DataManager.instance.ItemID[selectedId].rare + "\n所持数：" + DataManager.instance.ItemID[selectedId].itemnumber.ToString() + "個";
            selectedDocument.text = DataManager.instance.ItemID[selectedId].itemdescription;
            if (DataManager.instance.ItemID[selectedId].isMenuUse) selectedUsebutton.SetActive(true);
            else selectedUsebutton.SetActive(false);
        }
    }
    public void ContentViewUpdate(int id)
    {
        //選択中アイテム表示
        selectedId = id;
        GManager.instance.setrg = 6;
        selectedIcon.sprite = DataManager.instance.ItemID[id].itemimage;
        selectedName.text = "名前："+ DataManager.instance.ItemID[id].itemname+ "\nレアリティ："+ DataManager.instance.ItemID[id].rare+ "\n所持数："+ DataManager.instance.ItemID[id] .itemnumber.ToString()+ "個";
        selectedDocument.text = DataManager.instance.ItemID[id].itemdescription;
        if (DataManager.instance.ItemID[id].isMenuUse) selectedUsebutton.SetActive(true);
        else selectedUsebutton.SetActive(false);
    }

    public void ClickItemInvoke()
    {
        StartCoroutine(ClickItemEvent());
    }
    public IEnumerator ClickItemEvent()
    {
        var items = DataManager.instance.ItemID[selectedId];
        switch (DataManager.instance.ItemID[selectedId].itemType)
        {
            case DataManager.ItemType.HpCure:
                DataManager.instance.Triggers[4] =3;
                GManager.instance.setrg = 6;
                tmpItemType = DataManager.ItemType.HpCure;
                SelectPlayerView();
                selectPlayerUI.SetActive(true);
                break;
            case DataManager.ItemType.MpCure:
                DataManager.instance.Triggers[4] = 3;
                GManager.instance.setrg = 6;
                tmpItemType = DataManager.ItemType.MpCure;
                SelectPlayerView();
                selectPlayerUI.SetActive(true);
                break;
            case DataManager.ItemType.Fairy://精霊瓶
                yield return StartCoroutine(player.CodeEvent(selectedId, false));
                if (DataManager.instance.ItemID[selectedId].itemnumber <= 0) selectedId = -1;
                AllUpdateView(true);
                break;
            case DataManager.ItemType.Lamp://ランプ
                yield return StartCoroutine(player.CodeEvent(selectedId,false));
                if (DataManager.instance.ItemID[selectedId].itemnumber <= 0) selectedId = -1;
                AllUpdateView(true);
                break;
            case DataManager.ItemType.Save://セーブ
                yield return StartCoroutine(player.CodeEvent(selectedId,false));
                if (DataManager.instance.ItemID[selectedId].itemnumber <= 0) selectedId = -1;
                AllUpdateView(true);
                break;
            default:
                
                break;
        }
        yield return null;
        
    }
    private void SelectPlayerView()
    {
        for(int i = 0; i < DataManager.instance.playerselect.Length;)
        {
            if (DataManager.instance.playerselect[i] != -1 && DataManager.instance.playerselect[i] >= 0) useTarget[i].SetActive(true);
            else useTarget[i].SetActive(false);
            i++;
        }
    }
    public void ClickSelectedPlayer(int index)
    {
        int maxhp = (int)((DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].maxHP + DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].add_hp) * (1 + (DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].Lv / 1.5f)));
        int maxmp = (int)((DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].maxMP + DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].add_mp) * (1 + (DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].Lv / 1.5f)));
        var items = DataManager.instance.ItemID[selectedId];
        switch (DataManager.instance.ItemID[selectedId].itemType)
        {
            case DataManager.ItemType.HpCure:
                items.itemnumber -= 1;
                items = DataManager.instance.ItemID[selectedId];
                DataManager.instance.Triggers[4] = 2;
                if (DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].hp < maxhp && DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].hp > 0)
                {
                    player.audioSource.PlayOneShot(DataManager.instance.ItemID[selectedId].itemSound);
                    var tmphp = DataManager.instance.Pstatus[DataManager.instance.playerselect[index]];
                    tmphp.hp += (int)DataManager.instance.ItemID[selectedId].fluctuationeffect;
                    if (tmphp.hp > maxhp) tmphp.hp = maxhp;
                    DataManager.instance.Pstatus[DataManager.instance.playerselect[index]] = tmphp;
                    DataManager.instance.TextGet = DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].pname+"のHPは"+ ((int)DataManager.instance.ItemID[selectedId].fluctuationeffect ).ToString()+ "回復した";
                }
                else
                {
                    GManager.instance.setrg = 2;
                }
                selectPlayerUI.SetActive(false);
                break;
            case DataManager.ItemType.MpCure:
                items.itemnumber -= 1;
                items = DataManager.instance.ItemID[selectedId];
                DataManager.instance.Triggers[4] = 2;
                if (DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].mp < maxmp && DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].mp > 0)
                {
                    player.audioSource.PlayOneShot(DataManager.instance.ItemID[selectedId].itemSound);
                    var tmpmp = DataManager.instance.Pstatus[DataManager.instance.playerselect[index]];
                    tmpmp.mp += (int)DataManager.instance.ItemID[selectedId].fluctuationeffect;
                    if (tmpmp.mp > maxhp) tmpmp.mp = maxmp;
                    DataManager.instance.Pstatus[DataManager.instance.playerselect[index]] = tmpmp;
                    DataManager.instance.TextGet = DataManager.instance.Pstatus[DataManager.instance.playerselect[index]].pname + "のMPは" + ((int)DataManager.instance.ItemID[selectedId].fluctuationeffect).ToString() + "回復した";
                }
                else
                {
                    GManager.instance.setrg = 2;
                }
                selectPlayerUI.SetActive(false);
                break;
            default:

                break;
        }
        if(DataManager.instance.ItemID[selectedId].itemnumber<=0) selectedId = -1;
        AllUpdateView(true);
    }
}
