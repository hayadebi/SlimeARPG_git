using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MagicView : MonoBehaviour
{
    [Header("選択中キャラにセットされている魔法アイコン")]
    public Image[] selectedMagicIcon=new Image[5];
    [Header("選択されていない状態のキャラアイコン画像")]
    public Sprite noneIcon;
    [Header("選択中キャラにセットされている魔法名")]
    public Text[] selectedMagicName=new Text[5];
    [HideInInspector]
    public int selectedId = -1;//表示と使用時に扱うパーティー(playerselect)ID
    [HideInInspector]
    public int setId = -1;
    [Header("編成中パーティー")]
    public GameObject[] partyContent = new GameObject[3];
    public GameObject[] partySelecticon = new GameObject[3];

    private List<GameObject> magicList = new List<GameObject>();
    [Header("編成変更を実行時にActiveを切り替えるオブジェクト")]
    public GameObject changeEnableObejct;
    public GameObject changeDisableObejct;
    [System.Serializable]
    public struct TypeText
    {
        public DataManager.MagicType magicType;
        public Color typeColor;
    }
    [Header("属性ごとの変化設定")]
    public TypeText[] typeText = new TypeText[7];
    [Header("詳細データオブジェクト")]
    public GameObject dataObject;
    [Header("生成Transform")]
    public Transform contentTransform;
    [Header("選択魔法オリジナル")]
    public MagicContent contentMain;
    public GameObject br;
    public Image contentIcon;
    public Text contentName;
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
        foreach (GameObject content in partyContent)
        {
            if (content.activeSelf) content.SetActive(false);
            yield return null;
            content.SetActive(true);
        }
        for (int i = 0; i < partySelecticon.Length;)
        {
            if (i == selectedId) partySelecticon[i].SetActive(true);
            else partySelecticon[i].SetActive(false);
            i++;
        }
        if(dataObject.activeSelf) dataObject.SetActive(false);
        if (selectedId != -1 && DataManager.instance.playerselect[selectedId] != -1 && DataManager.instance.playerselect[selectedId] >= 0)
        {
            for (int i = 0; i < DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet.Length;)
            {
                if (DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[i] != -1)
                {
                    selectedMagicIcon[i].sprite = DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[i]].magicimage;
                    string tmpname = DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[i]].magicname;
                    string typename = "なし";
                    for (int c = 0; c < typeText.Length;)
                    {
                        if (typeText[c].magicType == DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[i]].magicType)
                        {
                            Color tmpColor = selectedMagicIcon[i].color;
                            tmpColor = typeText[c].typeColor;
                            selectedMagicIcon[i].color = tmpColor;
                        }
                        c++;
                    }
                    typename = TypeString(DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[i]].magicType);
                    selectedMagicName[i].text = "名前：" + tmpname;
                }
                else
                {
                    selectedMagicIcon[i].sprite = noneIcon;
                    Color tmpColor = selectedMagicName[i].color;
                    tmpColor = typeText[0].typeColor;
                    selectedMagicIcon[i].color = tmpColor;
                    selectedMagicName[i].text = "魔法枠" + (i + 1).ToString();
                }
                i++;
            }
        }
        else
        {
            for (int i = 0; i < 5;)
            {
                selectedMagicIcon[i].sprite = noneIcon;
                Color tmpColor = selectedMagicIcon[i].color;
                tmpColor = typeText[0].typeColor;
                selectedMagicName[i].color = tmpColor;
                selectedMagicName[i].text = "魔法枠"+(i+1).ToString();
                i++;
            }
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
    public void SelectMagic(int id)
    {
        if (id != 0 && selectedId != -1 && DataManager.instance.playerselect[selectedId] != -1)
        {
            setId = id;
            GManager.instance.setrg = 6;
            changeEnableObejct.SetActive(false);
            changeDisableObejct.SetActive(true);
            DataManager.instance.Triggers[4] = 3;
            foreach(GameObject obj in magicList)
            {
                Destroy(obj);
            }
            magicList = null;
            magicList = new List<GameObject>();
            for (int m = 0; m < DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].getMagic.Length;)
            {
                if (DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].getMagic[m].gettrg > 0 && DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[0] != DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].getMagic[m].magicid && DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[1] != DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].getMagic[m].magicid && DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[2] != DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].getMagic[m].magicid && DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[3] != DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].getMagic[m].magicid && DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[4] != DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].getMagic[m].magicid)
                {
                    var magics = DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].getMagic[m].magicid];
                    contentIcon.sprite = magics.magicimage;
                    contentName.text = magics.magicname;
                    contentMain.magicId = DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].getMagic[m].magicid;
                    for (int c = 0; c < typeText.Length;)
                    {
                        if (typeText[c].magicType == magics.magicType)
                        {
                            Color tmpColor = contentName.color;
                            tmpColor = typeText[c].typeColor;
                            contentName.color = tmpColor;
                        }
                        c++;
                    }
                    GameObject tmpobject = Instantiate(contentMain.gameObject, contentTransform.position, contentTransform.rotation, contentTransform);
                    tmpobject.SetActive(true);
                    magicList.Add(tmpobject);
                }
                m++;
            }
            GameObject brobject = Instantiate(br, contentTransform.position, contentTransform.rotation, contentTransform);
            brobject.SetActive(true);
            magicList.Add(brobject);
            foreach (Text tmp in selectedMagicName)
            {
                Color tmpColor = tmp.color;
                tmpColor = typeText[0].typeColor;
                tmp.color = tmpColor;
            }
            if (dataObject.activeSelf) dataObject.SetActive(false);
        }
        else GManager.instance.setrg = 2;
    }
    public void SetMagic(int magicId)
    {
        GManager.instance.setrg = 6;
        DataManager.instance.Pstatus[DataManager.instance.playerselect[selectedId]].magicSet[setId] = magicId;
        changeEnableObejct.SetActive(true);
        changeDisableObejct.SetActive(false);
        DataManager.instance.Triggers[4] = 2;
        selectedId = -1;
        foreach (Text tmp in selectedMagicName)
        {
            Color tmpColor = tmp.color;
            tmpColor = typeText[0].typeColor;
            tmp.color = tmpColor;
        }
        ViewReset();
    }
}
