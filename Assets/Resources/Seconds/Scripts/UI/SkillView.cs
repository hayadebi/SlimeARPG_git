using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillView : MonoBehaviour
{
    public int setId = 0;
    public Text skillName;
    public Text skillDoc;
    public Image skillIcon;
    public Sprite noneIconimage;
    public SlimeView slimeView;
    private int oldselectId = -1;
    public GameObject skillDialog;
    // Start is called before the first frame update
    void Start()
    {
        oldselectId = slimeView.selectedId;
        skillIcon.sprite = noneIconimage;
        skillName.text = "????????";
        skillDoc.text = "????????????";
    }
    private void Update()
    {
        if (slimeView.selectedId != oldselectId&& slimeView.selectedId!=-1&&DataManager.instance.playerselect[slimeView.selectedId]!=-1&& DataManager.instance.playerselect[slimeView.selectedId]>=0&&setId!=-1&&setId>=0&&DataManager.instance.Pstatus[DataManager.instance.playerselect[slimeView.selectedId]].inputskill[setId]!=-1&& DataManager.instance.Pstatus[DataManager.instance.playerselect[slimeView.selectedId]].inputskill[setId]>=0)
        {
            oldselectId = slimeView.selectedId;
            var tmpskill = DataManager.instance.Pstatus[DataManager.instance.playerselect[slimeView.selectedId]].inputskill[setId];
            skillIcon.sprite = DataManager.instance.skillData[tmpskill].skillicon;
        }
        else if (slimeView.selectedId != oldselectId)
        {
            oldselectId = slimeView.selectedId;
            skillIcon.sprite = noneIconimage;
        }
    }

    public void EnableDialog()
    {
        if (!skillDialog.activeSelf)
        {
            skillDialog.SetActive(true);
            if (slimeView.selectedId != -1 && DataManager.instance.playerselect[slimeView.selectedId] != -1 && DataManager.instance.playerselect[slimeView.selectedId] >= 0 && DataManager.instance.Pstatus[DataManager.instance.playerselect[slimeView.selectedId]].inputskill[setId] != -1 && DataManager.instance.Pstatus[DataManager.instance.playerselect[slimeView.selectedId]].inputskill[setId] >= 0)
            {
                oldselectId = slimeView.selectedId;
                var tmpskill = DataManager.instance.Pstatus[DataManager.instance.playerselect[slimeView.selectedId]].inputskill[setId];
                skillName.text = DataManager.instance.skillData[tmpskill].skillname;
                skillDoc.text = DataManager.instance.skillData[tmpskill].skilldescription;
            }
            else 
            {
                oldselectId = slimeView.selectedId;
                skillName.text = "????????";
                skillDoc.text = "????????????";
            }
        }
    }
    public void DisableDialog()
    {
        skillDialog.SetActive(false);
    }
}
