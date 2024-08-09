using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlimeView : MonoBehaviour
{
    [Header("選択中キャラアイコン")]
    public Image selectedIcon;
    [Header("選択されていない状態のキャラアイコン画像")]
    public Sprite noneIcon;
    [Header("選択中キャラ名等")]
    public Text selectedName;
    [Header("選択中キャラ説明")]
    public Text selectedDocument;
    [Header("選択中キャラステータス")]
    public Text selectedStatus;
    [HideInInspector]
    public int selectedId = -1;//表示と使用時に扱うパーティー(playerselect)ID
    [Header("編成中パーティー")]
    public GameObject[] partyContent=new GameObject[3];
    public GameObject[] partySelecticon = new GameObject[3];

    private List<GameObject> slimeList = new List<GameObject>();
    [Header("編成変更を実行時にActiveを切り替えるオブジェクト")]
    public GameObject changeEnableObejct;
    public GameObject changeDisableObejct;

    public GameObject useButton;
    // Start is called before the first frame update
    void Start()
    {
        ;
    }

    private void OnEnable()
    {
       ViewReset();
    }

    public void ViewReset()
    {
        StartCoroutine(ViewResetInvoke());
    }
    private IEnumerator ViewResetInvoke()
    {
        foreach(GameObject content in partyContent)
        {
            if(content.activeSelf)content.SetActive(false);
            yield return null;
            content.SetActive(true);
        }
        for(int i = 0; i < partySelecticon.Length;)
        {
            if (i == selectedId) partySelecticon[i].SetActive(true);
            else partySelecticon[i].SetActive(false);
            i++;
        }

        if(selectedId!=-1&&DataManager.instance.playerselect[selectedId]!=-1&& DataManager.instance.playerselect[selectedId] >= 0)
        {
            selectedIcon.sprite = DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].pimage;
            int maxhp = (int)((DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].maxHP + DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].add_hp) * (1 + (DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].Lv / 1.5f)));
            int maxmp = (int)((DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].maxMP + DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].add_mp) * (1 + (DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].Lv / 1.5f)));
            string tmpname = DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].pname;
            if (DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].hp <= 0) tmpname = "<color=red>" + tmpname+"</color>";
            selectedName.text = "名前："+ tmpname+ "\n攻撃属性："+TypeString(DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].attacktype) +","+TypeString(DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].attacktype2) +"\n弱点属性："+TypeString(DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].badtype) +","+TypeString(DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].badtype2) ;
            selectedDocument.text = DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].pdescription;
            selectedStatus.text = "LV："+ DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]] .Lv+ "\nHP："+ DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].hp.ToString()+ "/"+maxhp+"\nMP："+ DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].mp.ToString()+ "/"+maxmp+"\nAT："+ (DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]] .attack+ DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].add_at).ToString()+ "\nDF："+ (DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].defense+ DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].add_df).ToString();
            useButton.SetActive(true);
        }
        else
        {
            selectedIcon.sprite = noneIcon;
            selectedName.text = "名前：????????\n攻撃属性：??,??\n弱点属性：??,??";
            selectedDocument.text = "????????????\n????????????";
            selectedStatus.text = "LV：1\nHP：0/1\nMP：0/1\nAT：1\nDF：0";
            if(selectedId!=-1) useButton.SetActive(true);
            else useButton.SetActive(false);
        }
    }

    private string TypeString(DataManager.MagicType magicType)
    {
        switch (magicType)
        {
            case DataManager.MagicType.None:
                return "なし";
            case DataManager.MagicType.Dark:
                return "闇";
            case DataManager.MagicType.Holy:
                return "光";
            case DataManager.MagicType.God:
                return "神";
            case DataManager.MagicType.Normal:
                return "無";
            case DataManager.MagicType.Fire:
                return "炎";
            case DataManager.MagicType.Natural:
                return "自然";
            case DataManager.MagicType.Water:
                return "水";
            default:
                return "なし";
        }
    }

    public void PartyClick(int partyselectId)
    {
        GManager.instance.setrg = 6;
        selectedId = partyselectId;
        ViewReset();
    }

    public void SelectSlime(int slimeId)
    {
        GManager.instance.setrg = 6;
        DataManager.instance.playerselect[selectedId] = slimeId;
        changeEnableObejct.SetActive(true);
        changeDisableObejct.SetActive(false);
        DataManager.instance.Triggers[4] = 2;
        selectedId = -1;
        ViewReset();
    }
}
