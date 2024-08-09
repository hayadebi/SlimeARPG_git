using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MagicData : MonoBehaviour
{
    public MagicView magicView;
    public int setId;
    public Image img;
    public Text name;
    public Text doc;
    public Text status;
    public GameObject dataObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void EnterView()
    {
        if(magicView.selectedId!=-1&&setId!=-1&& DataManager.instance.playerselect[magicView.selectedId]!=-1&& DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]!=-1)
        {
            img.sprite = DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]].magicimage;
            string tmpname = DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]].magicname;
            string typename = "なし";
            for (int c = 0; c < magicView.typeText.Length;)
            {
                if (magicView.typeText[c].magicType == DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]].magicType)
                {
                    Color tmpColor = img.color;
                    tmpColor = magicView.typeText[c].typeColor;
                    img.color = tmpColor;
                }
                c++;
            }
            typename = TypeString(DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]].magicType);
            name.text = "名前：" + tmpname+ "\n魔法属性：" + typename;
            doc.text = DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]].magicdescription;
            int tmpat = DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]].Damage * -1;
            string tmpdamage = tmpat.ToString();
            if (tmpat <= 0) tmpdamage = "なし";
            status.text = "魔法ダメージ：" + tmpdamage + "\n消費MP：" + DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]].removeMp.ToString() + "\n行動前クールタイム：" + DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]].starttime.ToString() +  "秒\n行動後クールタイム：" + DataManager.instance.MagicID[DataManager.instance.Pstatus[DataManager.instance.playerselect[magicView.selectedId]].magicSet[setId]].cooltime.ToString() + "秒";   
        }
        else
        {
            img.sprite = magicView.noneIcon;
            Color tmpColor = name.color;
            tmpColor = magicView.typeText[0].typeColor;
            img.color = tmpColor;
            name.text = "名前：????????\n魔法属性：??";
            doc.text = "????????????\n????????????";
            status.text = "魔法ダメージ：0\n消費MP：0\n行動前クールタイム：0秒\n行動後クールタイム：0秒";
        }
        dataObject.SetActive(true);
    }
    public void ExitView()
    {
        dataObject.SetActive(false);
        img.sprite = magicView.noneIcon;
        Color tmpColor = name.color;
        tmpColor = magicView.typeText[0].typeColor;
        img.color = tmpColor;
        name.text = "名前：????????\n魔法属性：??";
        doc.text = "????????????\n????????????";
        status.text = "魔法ダメージ：0\n消費MP：0\n行動前クールタイム：0秒\n行動後クールタイム：0秒";
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

   

    public void SelectMagic()
    {
        magicView.SelectMagic(setId);
    }

}
