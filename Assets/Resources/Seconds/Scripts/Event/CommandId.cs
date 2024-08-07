using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandId : MonoBehaviour
{
    [Header("アタッチしているコマンドのID")]
    public int SelectId;
    [Header("バトルシステム格納")]
    public UniLang.BattleSystem battleSystem;
    // Start is called before the first frame update
    void Start()
    {
        ;
    }
    public void PlayerSelect()
    {
        battleSystem.PlayerSelect(SelectId);
    }
    public void ItemSelect()
    {
        battleSystem.ItemSelect(SelectId);
    }
    public void MagicSelect()
    {
        battleSystem.MagicSelect(SelectId);
    }
    public void TargetEnemySelect()
    {
        battleSystem.TargetEnemySelect(SelectId);
    }
    public void TargetPlayerSelect()
    {
        battleSystem.TargetPlayerSelect(SelectId);
    }
}
