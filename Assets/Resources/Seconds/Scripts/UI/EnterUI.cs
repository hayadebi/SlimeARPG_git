using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnterUI : MonoBehaviour
{
    public CommandId commandId;
    public GameObject EnterViewObject;
    public Text commandText;
    public UniLang.BattleSystem.CommandType commandType;
    // Start is called before the first frame update
    void Start()
    {
        ;
    }
    public void EnterView()
    {
        EnterViewObject.SetActive(true);
        if(commandType == UniLang.BattleSystem.CommandType.Attack)
        {
            commandText.text = DataManager.instance.MagicID[commandId.battleSystem.playerData[commandId.battleSystem.selectPlayer].magicId[commandId.SelectId]].magicdescription;
        }
        else if (commandType == UniLang.BattleSystem.CommandType.Item)
        {
            commandText.text = DataManager.instance.ItemID[commandId.SelectId].itemdescription;
        }
    }
    public void ExitView()
    {
        EnterViewObject.SetActive(false);
    }

    private void OnDisable()
    {
        ExitView();
    }
}
