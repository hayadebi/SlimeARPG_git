using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StatusSlider : MonoBehaviour
{
    public enum SliderType
    {
        HP,
        MP,
        CoolTime,
    }
    public enum TargetType
    {
        Player,
        Enemy,
    }
    [Header("スライダーUIの種類")]
    public SliderType sliderType;
    private Slider slider;
    [Header("バトルシステム格納")]
    public UniLang.BattleSystem battleSystem;
    [Header("ID")]
    public int id = 0;
    private int oldInt = 0;
    [Header("スライダーの対象")]
    public TargetType targetType = TargetType.Player;
    private int oldtrg = 999;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    void OnEnable()
    {
        oldInt = -999;
        oldtrg = 999;
    }
    // Update is called once per frame
    void Update()
    {
        if (targetType == TargetType.Player)
        {
            switch (sliderType)
            {
                case SliderType.HP:
                    if ((oldInt != battleSystem.playerData[id].hp)||oldtrg!=DataManager.instance.Triggers[1])
                    {
                        oldtrg = DataManager.instance.Triggers[1];
                        oldInt = battleSystem.playerData[id].hp;
                        slider.maxValue = battleSystem.playerData[id].maxHP;
                        slider.value = battleSystem.playerData[id].hp;
                        
                    }
                    break;
                case SliderType.MP:
                    if ((oldInt != battleSystem.playerData[id].mp && battleSystem.playerData[id].hp > 0 )||oldtrg != DataManager.instance.Triggers[1])
                    {
                        oldtrg = DataManager.instance.Triggers[1];
                            oldInt = battleSystem.playerData[id].mp;
                            slider.maxValue = battleSystem.playerData[id].maxMP;
                            slider.value = battleSystem.playerData[id].mp;
                    }
                    break;
                case SliderType.CoolTime:
                    if ((slider.value > 0 || battleSystem.playerData[id].coolTime > 0) && battleSystem.playerData[id].hp > 0 && DataManager.instance.Triggers[2] <= 0)
                    {
                        battleSystem.playerData[id].coolTime -= Time.deltaTime;
                        slider.value = battleSystem.playerData[id].coolTime;
                    }
                    break;
            }
        }
        else if (targetType == TargetType.Enemy)
        {
            switch (sliderType)
            {
                case SliderType.HP:
                    if ((oldInt != battleSystem.enemyData[id].hp&& battleSystem.enemyData[id].enemyId!=-1 )|| oldtrg != DataManager.instance.Triggers[1])
                    {
                        oldtrg = DataManager.instance.Triggers[1];
                        oldInt = battleSystem.enemyData[id].hp;
                        slider.maxValue = battleSystem.enemyData[id].maxHP;
                        slider.value = battleSystem.enemyData[id].hp;
                    }
                    break;
                case SliderType.CoolTime:
                    if ((slider.value > 0 || battleSystem.enemyData[id].coolTime > 0 )&& battleSystem.enemyData[id].enemyId != -1 && battleSystem.enemyData[id].hp > 0 && DataManager.instance.Triggers[2] <= 0)
                    {
                        battleSystem.enemyData[id].coolTime -= Time.deltaTime;
                        slider.value = battleSystem.enemyData[id].coolTime;
                    }
                    break;
            }
        }
    }
}
