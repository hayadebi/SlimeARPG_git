using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UItext : MonoBehaviour
{
    public string bossname;
    private GameObject boss;
    private enemyS es;
    public string InputText = "";
    private Text scoreText = null;
    public Image Picon;
    private int oldInt = 0;
    private float oldFloat = 0;
    private string oldString = "";
    private Sprite oldSprite = null;
    private int oldEnglish = 0;
    public Animator textgetanim;
    private float stime = 0;
    public enemyS estatus;
    private int newlv = 1;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        if (InputText == "textget")
        {
            scoreText.text = GManager.instance.txtget;
            oldString = GManager.instance.txtget;
            Invoke("TextGetAnimEnd", 4.3f);
        }
        else if(InputText == "boss")
        {
            boss = GameObject.Find(bossname);
            es = boss.GetComponent<enemyS>();
            if(GManager.instance.isEnglish == 0)
            {
                scoreText.text = "-" + es.enemyName + "-";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                scoreText.text = "-" + es.enemyName2 + "-";
            }
            
            oldString = es.enemyName;
        }
        else if (InputText == "elv" && estatus != null)
        {
            newlv = estatus.Estatus.Lv + 1;
            scoreText.text = "Lv：" + newlv;
            oldInt = estatus.Estatus.Lv;
        }
        else if (InputText == "stage")
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.text = "ステージ" + GManager.instance.stageNumber;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                scoreText.text = "Stage" + GManager.instance.stageNumber;
            }
            oldInt = GManager.instance.stageNumber;
        }
        else if (InputText == "coin")
        {
            scoreText.text = GManager.instance.Coin + "×";
            oldInt = GManager.instance.Coin;
        }
        else if (InputText == "stone")
        {
            scoreText.text = GManager.instance.ItemID[10].itemnumber + "×";
            oldInt = GManager.instance.ItemID[10].itemnumber;
        }
        else if (InputText == "picon")
        {
            Picon.sprite = GManager.instance.Pstatus[GManager.instance.playerselect].pimage;
            oldSprite = GManager.instance.Pstatus[GManager.instance.playerselect].pimage;
        }
        else if (InputText == "pname")
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.text = GManager.instance.Pstatus[GManager.instance.playerselect].pname;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                scoreText.text = GManager.instance.Pstatus[GManager.instance.playerselect].pname2;
            }
            oldString = GManager.instance.Pstatus[GManager.instance.playerselect].pname;
        }
        else if (InputText == "hp")
        {
            scoreText.text =GManager.instance.Pstatus[GManager.instance.playerselect].hp.ToString();
            oldInt = GManager.instance.Pstatus[GManager.instance.playerselect].hp;
        }
        else if (InputText == "mp")
        {
            scoreText.text = GManager.instance.Pstatus[GManager.instance.playerselect].mp.ToString();
            oldInt = GManager.instance.Pstatus[GManager.instance.playerselect].mp;
        }
        else if (InputText == "lv")
        {
            scoreText.text = GManager.instance.Pstatus[GManager.instance.playerselect].Lv.ToString();
            oldFloat = GManager.instance.Pstatus[GManager.instance.playerselect].Lv;
        }
        else if (InputText == "hpS")
        {
            scoreText.text = "MaxHP/"+ GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
        }
        else if (InputText == "lvS")
        {
            scoreText.text = "Lv/" + GManager.instance.Pstatus[GManager.instance.playerselect].Lv;
        }
        else if (InputText == "expS")
        {
            scoreText.text = "Next Lv/" + (GManager.instance.Pstatus[GManager.instance.playerselect].maxExp - GManager.instance.Pstatus[GManager.instance.playerselect].inputExp);
        }
        else if (InputText == "atS")
        {
            if (GManager.instance.itemhand != -1)
            {
                scoreText.text = "AT/" + GManager.instance.Pstatus[GManager.instance.playerselect].attack;
            }
            else if (GManager.instance.itemhand == -1)
            {
                scoreText.text = "AT/" + GManager.instance.Pstatus[GManager.instance.playerselect].attack;
            }
        }
        else if (InputText == "dfS")
        {
            scoreText.text = "DF/" + GManager.instance.Pstatus[GManager.instance.playerselect].defense;
        }
        else if (InputText == "spS")
        {
            scoreText.text = "SP/" + GManager.instance.Pstatus[GManager.instance.playerselect].speed;
        }
        if(InputText == "itemcl")
        {
            //for (int i = 0; i < (GManager.instance.ItemID.Length);)
            //{
            //    GManager.instance.ItemID[i].itemnumber = PlayerPrefs.GetInt("itemnumber" + i, 0);
            //    GManager.instance.ItemID[i].gettrg = PlayerPrefs.GetInt("itemget" + i, 0);
            //    i += 1;
            //}
            int allitem = GManager.instance.ItemID.Length;
            int getitem = 0;
            for(int i = 0; i < GManager.instance.ItemID.Length;)
            {
                if(GManager.instance.ItemID[i].gettrg == 1)
                {
                    getitem += 1;
                }
                i++;
            }
            int percent = (100 / allitem) * getitem;
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.text = "アイテム収集率:" + percent + "%";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                scoreText.text = "Item collection:" + percent + "%";
            }
        }
        else if(InputText == "reduction")
        {
            if(GManager.instance.reduction == 0)
            {
                if(GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "軽量化:OFF";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Weight reduction: OFF";
                }
                oldInt = 0;
            }
            else if(GManager.instance.reduction == 1)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "軽量化:ON";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Weight reduction: ON";
                }
                oldInt = 1;
            }
        }
        else if (InputText == "オートビュー")
        {
            if (GManager.instance.autoviewup == 0)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "自動視点移動:OFF";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Auto viewpoint move: OFF";
                }
                oldInt = 0;
            }
            else if (GManager.instance.autoviewup == 1)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "自動視点移動:ON";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Auto viewpoint move: ON";
                }
                oldInt = 1;
            }
        }
        else if (InputText == "オートダッシュ")
        {
            if (GManager.instance.autolongdash == 0)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "自動長押しダッシュ:OFF";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Auto long-press dash: OFF";
                }
                oldInt = 0;
            }
            else if (GManager.instance.autolongdash == 1)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "自動長押しダッシュ:ON";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Auto long-press dash: ON";
                }
                oldInt = 1;
            }
        }
        else if(InputText == "status")
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.text = "MaxHP:"+GManager.instance.Pstatus[GManager.instance.playerselect].maxHP
                    + "\nMaxMP:"+GManager.instance.Pstatus[GManager.instance.playerselect].maxMP
                    + "\nAT:" + GManager.instance.Pstatus[GManager.instance.playerselect].attack
                    + "\nDF:" + GManager.instance.Pstatus[GManager.instance.playerselect].defense
                    + "\nLV:" + GManager.instance.Pstatus[GManager.instance.playerselect].Lv
                    + "\n次のLvUPまで:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxExp
                    + "\n手に入れたEXP:" + GManager.instance.Pstatus[GManager.instance.playerselect].inputExp
                    + "\n攻撃タイプ:" + GManager.instance.Pstatus[GManager.instance.playerselect].attacktype
                    + "\n弱点タイプ:" + GManager.instance.Pstatus[GManager.instance.playerselect].badtype;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                scoreText.text = "MaxHP:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxHP
                    + "\nMaxMP:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxMP
                    + "\nAT:" + GManager.instance.Pstatus[GManager.instance.playerselect].attack
                    + "\nDF:" + GManager.instance.Pstatus[GManager.instance.playerselect].defense
                    + "\nLV:" + GManager.instance.Pstatus[GManager.instance.playerselect].Lv
                    + "\nNext Lv UP.:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxExp
                    + "\nEXP obtained:" + GManager.instance.Pstatus[GManager.instance.playerselect].inputExp
                    + "\nAttack Type:" + GManager.instance.Pstatus[GManager.instance.playerselect].attacktype2
                    + "\nWeakness Type:" + GManager.instance.Pstatus[GManager.instance.playerselect].badtype2;
            }
            oldInt = GManager.instance.playerselect;
        }
        else if (InputText == "suntime")
        {
            stime = GManager.instance.sunTime / (180 / 24);
            stime = Mathf.Floor(stime);
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.text = "時刻：" + stime + "時" + "　所持金：" + GManager.instance.Coin + "　日数：" + GManager.instance.daycount;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                scoreText.text = "Time：" + stime + " hours" + "　Coin：" + GManager.instance.Coin + "　Days：" + GManager.instance.daycount;
            }
            oldString = GManager.instance.sunTime.ToString();
        }
        else if (InputText == "stagename")
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.text = "現在地：" + GManager.instance.stageName[GManager.instance.stageNumber];
            }
            if (GManager.instance.isEnglish == 1)
            {
                scoreText.fontSize = 28;
                scoreText.text = "Current location：" + GManager.instance.stageName2[GManager.instance.stageNumber];
            }
            oldString = GManager.instance.stageNumber.ToString();
        }
        else if (InputText == "modetext")
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.fontSize = 24;
                if (GManager.instance.mode == 0)
                {
                    scoreText.text = "難易度：スライム";
                }
                else if (GManager.instance.mode == 1)
                {
                    scoreText.text = "難易度：勇者";
                }
                else if (GManager.instance.mode == 2)
                {
                    scoreText.text = "難易度：魔王";
                }
            }
            if (GManager.instance.isEnglish == 1)
            {
                scoreText.fontSize = 21;
                if (GManager.instance.mode == 0)
                {
                    scoreText.text = "Difficulty: Slime";
                }
                else if (GManager.instance.mode == 1)
                {
                    scoreText.text = "Difficulty: Hero";
                }
                else if (GManager.instance.mode == 2)
                {
                    scoreText.text = "Difficulty: Demon king";
                }
            }
            oldInt = GManager.instance.mode;
        }
        else if (InputText == "modetext_2")
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.fontSize = 24;
                if (GManager.instance.mode == 0)
                {
                    scoreText.text = "難易度：空腹";
                }
                else if (GManager.instance.mode == 1)
                {
                    scoreText.text = "難易度：満腹";
                }
                else if (GManager.instance.mode == 2)
                {
                    scoreText.text = "難易度：暴食";
                }
            }
            if (GManager.instance.isEnglish == 1)
            {
                scoreText.fontSize = 21;
                if (GManager.instance.mode == 0)
                {
                    scoreText.text = "Difficulty: Hunger";
                }
                else if (GManager.instance.mode == 1)
                {
                    scoreText.text = "Difficulty: Full stomach";
                }
                else if (GManager.instance.mode == 2)
                {
                    scoreText.text = "Difficulty: Surfeit";
                }
            }
            oldInt = GManager.instance.mode;
        }
        else if(InputText == "ストーリー")
        {
            if(GManager.instance.isEnglish == 1)
            {
                scoreText.fontSize = 18;
            }
            scoreText.text = GManager.instance.storyUI;
            oldString = GManager.instance.storyUI;
        }
        else if (InputText == "tips")
        {
            if (GManager.instance.Triggers[105] != GManager.instance.Triggers[106])
            {
                Picon.enabled = true;
                oldInt = 1;
            }
            else
            {
                Picon.enabled = false;
                oldInt = 0;
            }
        }
        else if(InputText=="titlespell")
        {
            if (GManager.instance.isEnglish == 1)
            {
                scoreText.fontSize = 30;
            }
            scoreText.text = GManager.instance.temp_text[0];
            oldString = GManager.instance.temp_text[0];
        }
    }
    void TextGetAnimEnd()
    {
        textgetanim.SetInteger("Anumber", 1);
        Invoke(nameof(TextResetEnd), 1.3f);
    }

    void TextResetEnd()
    {
        GManager.instance.txtget = "";
        oldString = "";
    }
    // Update is called once per frame
    void Update()
    {
        if(oldEnglish != GManager.instance.isEnglish )
        {
            oldEnglish = GManager.instance.isEnglish;
            if (InputText == "itemcl")
            {
                int allitem = GManager.instance.ItemID.Length;
                int getitem = 0;
                for (int i = 0; i < GManager.instance.ItemID.Length;)
                {
                    if (GManager.instance.ItemID[i].gettrg == 1)
                    {
                        getitem += 1;
                    }
                    i++;
                }
                int percent = (100 / allitem) * getitem;
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "アイテム収集率:" + percent + "%";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Item collection:" + percent + "%";
                }
            }
        }
        if (InputText == "textget" && oldString != GManager.instance.txtget && GManager.instance.txtget != "" && GManager.instance.setmenu <= 0)
        {
            scoreText.text = GManager.instance.txtget;
            oldString = GManager.instance.txtget;
            textgetanim.SetInteger("Anumber", 0);
            Invoke(nameof(TextGetAnimEnd), 4.3f);
        }
        else if (InputText == "coin" && oldInt != GManager.instance.Coin)
        {
            scoreText.text = GManager.instance.Coin + " ×";
            oldInt = GManager.instance.Coin;
        }
        else if (InputText == "stone" && oldInt != GManager.instance.ItemID[10].itemnumber)
        {
            scoreText.text = GManager.instance.ItemID[10].itemnumber + "×";
            oldInt = GManager.instance.ItemID[10].itemnumber;
        }
        else if (InputText == "picon" && oldSprite != GManager.instance.Pstatus[GManager.instance.playerselect].pimage)
        {
            Picon.sprite = GManager.instance.Pstatus[GManager.instance.playerselect].pimage;
            oldSprite = GManager.instance.Pstatus[GManager.instance.playerselect].pimage;
        }
        else if (InputText == "pname" && oldString != GManager.instance.Pstatus[GManager.instance.playerselect].pname)
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.text = GManager.instance.Pstatus[GManager.instance.playerselect].pname;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                scoreText.text = GManager.instance.Pstatus[GManager.instance.playerselect].pname2;
            }
            oldString = GManager.instance.Pstatus[GManager.instance.playerselect].pname;
        }
        else if (InputText == "hp" && oldInt != GManager.instance.Pstatus[GManager.instance.playerselect].hp)
        {
            scoreText.text = GManager.instance.Pstatus[GManager.instance.playerselect].hp.ToString();
            oldInt = GManager.instance.Pstatus[GManager.instance.playerselect].hp;
        }
        else if (InputText == "mp" && oldInt != GManager.instance.Pstatus[GManager.instance.playerselect].mp)
        {
            scoreText.text = GManager.instance.Pstatus[GManager.instance.playerselect].mp.ToString();
            oldInt = GManager.instance.Pstatus[GManager.instance.playerselect].mp;
        }
        else if (InputText == "lv" && oldFloat != GManager.instance.Pstatus[GManager.instance.playerselect].Lv)
        {
            scoreText.text = GManager.instance.Pstatus[GManager.instance.playerselect].Lv.ToString();
            oldFloat = GManager.instance.Pstatus[GManager.instance.playerselect].Lv;
        }
        else if (InputText == "reduction" && oldInt != GManager.instance.reduction)
        {
            if (GManager.instance.reduction == 0)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "軽量化:OFF";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Weight reduction: OFF";
                }
                oldInt = 0;
            }
            else if (GManager.instance.reduction == 1)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "軽量化:ON";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Weight reduction: ON";
                }
                oldInt = 1;
            }
        }
        else if (InputText == "オートビュー" && oldInt != GManager.instance.autoviewup)
        {
            if (GManager.instance.autoviewup == 0)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "自動視点移動:OFF";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Auto viewpoint move: OFF";
                }
                oldInt = 0;
            }
            else if (GManager.instance.autoviewup == 1)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "自動視点移動:ON";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Auto viewpoint move: ON";
                }
                oldInt = 1;
            }
        }
        else if (InputText == "オートダッシュ" && oldInt != GManager.instance.autolongdash)
        {
            if (GManager.instance.autolongdash == 0)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "自動長押しダッシュ:OFF";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Auto long-press dash: OFF";
                }
                oldInt = 0;
            }
            else if (GManager.instance.autolongdash == 1)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    scoreText.text = "自動長押しダッシュ:ON";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    scoreText.text = "Auto long-press dash: ON";
                }
                oldInt = 1;
            }
        }

        else if (InputText == "status" && oldInt != GManager.instance.playerselect)
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.text = "MaxHP:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxHP
                    + "\nMaxMP:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxMP
                    + "\nAT:" + GManager.instance.Pstatus[GManager.instance.playerselect].attack
                    + "\nDF:" + GManager.instance.Pstatus[GManager.instance.playerselect].defense
                    + "\nLV:" + GManager.instance.Pstatus[GManager.instance.playerselect].Lv
                    + "\n次のLvUPまで:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxExp
                    + "\n手に入れたEXP:" + GManager.instance.Pstatus[GManager.instance.playerselect].inputExp
                    + "\n攻撃タイプ:" + GManager.instance.Pstatus[GManager.instance.playerselect].attacktype
                    + "\n弱点タイプ:" + GManager.instance.Pstatus[GManager.instance.playerselect].badtype;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                scoreText.text = "MaxHP:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxHP
                    + "\nMaxMP:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxMP
                    + "\nAT:" + GManager.instance.Pstatus[GManager.instance.playerselect].attack
                    + "\nDF:" + GManager.instance.Pstatus[GManager.instance.playerselect].defense
                    + "\nLV:" + GManager.instance.Pstatus[GManager.instance.playerselect].Lv
                    + "\nNext Lv UP.:" + GManager.instance.Pstatus[GManager.instance.playerselect].maxExp
                    + "\nEXP obtained:" + GManager.instance.Pstatus[GManager.instance.playerselect].inputExp
                    + "\nAttack Type:" + GManager.instance.Pstatus[GManager.instance.playerselect].attacktype2
                    + "\nWeakness Type:" + GManager.instance.Pstatus[GManager.instance.playerselect].badtype2;
            }
            oldInt = GManager.instance.playerselect;
        }
        else if (InputText == "suntime" && oldString != GManager.instance.sunTime.ToString())
        {
            stime = GManager.instance.sunTime / (180 / 24);
            stime = Mathf.Floor(stime);
            if (stime > 24)
            {
                stime = 24;
            }
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.text = "時刻：" + stime + "時" + "　所持金：" + GManager.instance.Coin + "　日数：" + GManager.instance.daycount;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                scoreText.text = "Time：" + stime + " hours" + "　Coin：" + GManager.instance.Coin + "　Days：" + GManager.instance.daycount;
            }
            oldString = GManager.instance.sunTime.ToString();
        }
        else if (InputText == "elv" && estatus != null && oldInt != estatus.Estatus.Lv)
        {
            newlv = estatus.Estatus.Lv + 1;
            scoreText.text = "Lv：" + newlv;
            oldInt = estatus.Estatus.Lv;
        }
        else if (InputText == "modetext" && oldInt != GManager.instance.mode)
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.fontSize = 24;
                if (GManager.instance.mode == 0)
                {
                    scoreText.text = "難易度：スライム";
                }
                else if (GManager.instance.mode == 1)
                {
                    scoreText.text = "難易度：勇者";
                }
                else if (GManager.instance.mode == 2)
                {
                    scoreText.text = "難易度：魔王";
                }
            }
            if (GManager.instance.isEnglish == 1)
            {
                scoreText.fontSize = 21;
                if (GManager.instance.mode == 0)
                {
                    scoreText.text = "Difficulty: Slime";
                }
                else if (GManager.instance.mode == 1)
                {
                    scoreText.text = "Difficulty: Hero";
                }
                else if (GManager.instance.mode == 2)
                {
                    scoreText.text = "Difficulty: Demon king";
                }
            }
            oldInt = GManager.instance.mode;
        }
        else if (InputText == "modetext_2" && oldInt != GManager.instance.mode)
        {
            if (GManager.instance.isEnglish == 0)
            {
                scoreText.fontSize = 24;
                if (GManager.instance.mode == 0)
                {
                    scoreText.text = "難易度：空腹";
                }
                else if (GManager.instance.mode == 1)
                {
                    scoreText.text = "難易度：満腹";
                }
                else if (GManager.instance.mode == 2)
                {
                    scoreText.text = "難易度：暴食";
                }
            }
            if (GManager.instance.isEnglish == 1)
            {
                scoreText.fontSize = 21;
                if (GManager.instance.mode == 0)
                {
                    scoreText.text = "Difficulty: Hunger";
                }
                else if (GManager.instance.mode == 1)
                {
                    scoreText.text = "Difficulty: Full stomach";
                }
                else if (GManager.instance.mode == 2)
                {
                    scoreText.text = "Difficulty: Surfeit";
                }
            }
            oldInt = GManager.instance.mode;
        }
        else if (InputText == "tips" && GManager.instance.Triggers[105] != GManager.instance.Triggers[106] && oldInt == 0)
        {
            Picon.enabled = true;
            GManager.instance.setrg = 34;
            oldInt = 1;
        }
        else if (InputText == "tips" && GManager.instance.Triggers[105] == GManager.instance.Triggers[106] && oldInt == 1)
        {
            Picon.enabled = false;
            oldInt = 0;
        }
    }
}