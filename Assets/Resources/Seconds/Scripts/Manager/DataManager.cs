using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using NCMB;
public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;
    [Multiline]
    public string TrgMemo;
    public int[] Triggers;
    [Header("所持金")]
    public int Coin = 0; //所持金
    [Header("様々なとこから一時的に格納する文章")]
    public string TextGet; //様々なとこから一時的に格納する文章
    [Multiline]
    public string NumMemo;
    public float[] FreeNums; //各々のスクリプトが使う、一時的な数値
    [Header("効果音制御")]
    public int setrg = -1;
    [Header("現在のステージ")]
    public int stageNumber = 1; //現在のステージID
    //[Header("設定関連")]
    //public float audioMax = 0.16f; //音量設定に使用
    //public float seMax = 0.16f;//効果音設定に使用
    //public int mode = 1; //難易度設定に使用
    //public int isEnglish = 0; //言語設定に使用
    //public int reduction = 0; //画面効果設定に使用
     public enum TargetType
    {
        Enemy,
        Player,
    }
    public enum ItemType
    {
        HpCure,
        MpCure,
        Damage,
        AtStatusUp,
        DfStatusUp,
        Save,
        Lamp,
        Fairy,
        None,
    }
    public enum MagicType
    {
        Normal,
        Water,
        Fire,
        Natural,
        Dark,
        Holy,
        God,
        None,
    }
    public enum MagicEvent
    {
        None,
        HpCure,
        BoneScreen,
    }
    public enum MagicAttackScale
    {
        Small,
        Normal,
        Big,
        Special,
    }

    public enum StateAnomaly
    {
        None,
        Poison,
        DfDamage,
    }
    [System.Serializable]
    public struct AnomalyID
    {
        public StateAnomaly stateAnomaly;
        public Sprite icon;
        public string anomalyName;
        public AudioClip se;
    }
    [Header("状態異常")]
    public AnomalyID[] anomalyID;
    public enum SkillEvent
    {
        DropUp,
        LastPower,
        VitalpointUp,
        MinCooltime,
        EscaperateUp,
    }
    [System.Serializable]
    public struct item
    {
        //各アイテム情報
        public string itemname;
        [Multiline]
        public string itemdescription;
        public Sprite itemimage;
        public ItemType itemType;
        public float fluctuationeffect;
        public int itemprice;
        public int itemnumber;
        //public GameObject itemobj;
        public AudioClip itemSound;
        public int gettrg;
        public string rare;
       
        public float cooltime;
        public float starttime;
        public TargetType targetType;
        public bool isBattleUse;
        public bool isShake;
        public int effectId;
        public bool isDeadUsed;
        public bool isMenuUse;
        public int charaAnimId;
        public bool isFieldUse;
        [HideInInspector]
        public bool isUsed;
        [Multiline]
        public string dropMonster;
    }
    [Header("アイテムデータ")]
    public List<item> ItemID;

    //------------------------------


    [System.Serializable]
    public struct magic
    {
        //各魔法情報
        public string magicname;
        [Multiline]
        public string magicdescription;
        public Sprite magicimage;
        public int magicprice;
        public MagicType magicType;
        //public MagicAttackScale magicAttackScale;
        public MagicEvent magicEvent;
        //public GameObject itemobj;
        //public GameObject magicobj;
        public AudioClip magicSound;
        public int Damage;
        public int removeMp;
        public float cooltime;
        public float starttime;
        public int VitalpointAttack;
        public TargetType targetType;
        public bool isShake;
        public string useWord;
        public string damageWord;
        public int effectId;
        public int charaAnimId;
        public int addAnomaly;
    }
    [Header("魔法データ")]
    public List<magic> MagicID;
    [Header("現在選択しているアイテム")]
    public int itemselect; //現在選択しているアイテム
    [System.Serializable]
    public struct player
    {
        //各プレイヤーの情報
        public Sprite pimage;
        public string pname;
        [Multiline]
        public string pdescription;
        public int maxHP;
        public int hp;
        public int maxMP;
        public int mp;
        public float speed;
        public int defense;
        public int attack;
        public int Lv;
        public int maxExp;
        public int inputExp;
        public int[] inputskill;//秘伝用にセーブ
        [System.Serializable]
        public struct pmagic
        {
            public int magicid;
            public int gettrg;
            public int inputlevel;
        }
        public pmagic[] getMagic;
        public int[] magicSet;
        public MagicType badtype;
        public MagicType badtype2;
        public MagicType attacktype;
        public MagicType attacktype2;
        public GameObject pobj;
        public int getpl;
        public int add_hp;
        public int add_mp;
        public int add_at;
        public int add_df;
        public RuntimeAnimatorController animC;
        public int maxAnime;
        public bool isPoisonResistance;
        public bool isDFDamageResistance;
    }
    [Header("プレイヤーデータ")]
    public List<player> Pstatus;

    [Header("プレイヤー編成")]
    public int[] playerselect; //現在操作しているプレイヤー(スライム)。中央が歩く

    [Header("セーブ地点湧き位置")]
    public Vector3 playerStartPos;
   
    [System.Serializable]
    public struct Skill
    {
        //各スキル情報
        [Header("スキル名")]
        public string skillname;
        [Header("スキル説明")]
        [Multiline]
        public string skilldescription;
        [Header("スキルアイコン")]
        public Sprite skillicon;
        [Header("スキル効果によって発生するオブジェクト")]
        public GameObject skillobject;
        public SkillEvent skillEvent;
        public AudioClip skillSound;
    }
    [Header("スキルデータ")]
    public List<Skill> skillData;
   
    [Header("エフェクト全般はここ")]
    public GameObject[] effectobj; //汎用的なエフェクトを格納
    [Header("一時的な情報関連")]
    public int animmode = -1; //一時的な、アニメーションを再生するための変数
    public string SceneText; //一時的なステージ名を指定する用
    public GameObject spawnUI; //表示させるUIを、会話イベントスクリプト等から一時的に格納
    public AudioClip ase; //一時的な効果音を格納する用
    public string sayobjname; //会話イベントで一時的に使用
    [System.Serializable]
    public struct mission
    {
        //各ミッション情報
        [Multiline]
        public string name;
        [Multiline]
        public string description;
        public bool subtrg;
        public int inputmission;
        public string client;
        public string client2;
        public Sprite clientimage;
        public int inputitemid;
        public int inputitemnumber;
        public int getcoin;
        public int getitemid;
        public int getachievementsid;
        public int getpl;
        [Multiline] public string getsource;
        [Multiline] public string getsource2;
    }
    //new
    [Header("ミッションデータ")]
    public List<mission> missionID;
    [System.Serializable]
    public struct achievements
    {
        //各実績情報
        [Multiline]
        public string name;
        [Multiline]
        public string description;
        public int gettrg;
        public Sprite image;
    }
    //new
    [Header("実績データ")]
    public List<achievements> achievementsID;

    public enum AiType
    {
        Random,
        Optimization,
    }

    [System.Serializable]
    public struct enemynote
    {
        //敵図鑑内の各敵情報
        [Multiline]
        public string name;
        [Multiline]
        public string description;

        public int maxHP;
        public int maxMP;
        public float speed;
        public int defense;
        public int attack;
        public int Lv;
        public int getExp;
        public int[] inputskill;//秘伝用にセーブ
        [System.Serializable]
        public struct emagic
        {
            public int magicid;
            public int inputlevel;
        }
        public emagic[] getMagic;

        public MagicType badtype;
        public MagicType badtype2;
        public MagicType attacktype;
        public MagicType attacktype2;
        public int gettrg;
        public int getAchievements;
        public Sprite image;
        public GameObject eobj;
        public AiType aiType;
        public int escapeRate;
        [System.Serializable]
        public struct DropData
        {
            public int dropItemId;
            public int dropRate;
        }
        [Header("ドロップアイテム設定")]
        public DropData[] dropData;
        public RuntimeAnimatorController animC;
        public bool isBoss;
        public int bossLastMagicId;
        public int bossSayEvent;
        public int bossClearEvent;
        public int maxAnim;
        public float battleSize;
        public float yPosition;
        public bool isPoisonResistance;
        public bool isDFDamageResistance;
        public int isTrg;
        public int isTargetEvent;
        public int isAddEvent;
        [Multiline]
        public string habitatDescription;
    }
    [Header("敵データ")]
    //new
    public List<enemynote> enemynoteID;
    [Header("現在のマウス位置")]
    public Vector3 mouseP; //現在のマウス位置

    public enum StageType
    {
        Field,
        Safe,
        Dungeon,
    }

    [System.Serializable]
    public struct StageData
    {
        //敵図鑑内の各敵情報
        public string name;
        public string realname;
        public StageType stageType;
        public GameObject stageObject;
        public AudioClip defaultBGM;
    }
    [Header("ステージデータ")]
    //new
    public List<StageData> stageData;
    [Header("ゲーム内時間")]
    public float sunTime = 72; //現在のゲーム内時間(太陽の移動にも使用)
    [Header("経過日数")]
    public int daycount = 0; //ゲーム内経過日数
    [Header("一時的なショップデータ")]
    public int[] shopID; //一時的な、ショップIDに該当する商品内容(会話イベントから各NPCに応じてショップを変えるため)
    public int[] stoneID; //旧ショップ内容、現在はほとんど使用していない
    public int itemMagicID; //アイテムから魔法を発動させるために、一時的な魔法ID格納
    public string villageName = ""; //集落関連の一時的な名前
    public string storyUI = ""; //章の始終で使用する、一時的な短い文章

    [System.Serializable]
    public struct CraftID
    {
        //各クラフトレシピの情報
        public string recipeName;
        public int get_recipe;
        //public int input_getRecipe;
        [Header("作成するアイテム")]
        public int craftItem_id;
        public int craftGet_number;
        public int craftSub_id;
        public int craftSub_number;
        [Header("使用する素材(6種類まで)")]
        public int[] materialItem_id;
        public int[] materialSet_number;
        public bool on_notgetview;
        public int isOnlyone;
    }
    [Header("クラフトレシピを追加、いじるならココ")]
    public List<CraftID> _craftRecipe;

    [Multiline]
    public string[] temp_text;
    private float reloadtime = 0;
    [Header("日付チェック関連")]
    public int old_year = 2023;
    [System.Serializable]
    public struct DevDateTime
    {
        public int year;
        public int month;
        public int day;
    }
    public DevDateTime devdays;
    public DateTime checkdev = new DateTime(2003, 7, 28);
    //課金要素について
    [Header("課金要素関連")]
    public float get_devcoin = 0f;
    public float get_devnugget = 0f;
    public string mpurse_address = "";
    public bool mpurseuser_on = false;
    public int select_buyid = 0;
    public DateTime tmpdays;

    [System.Serializable]
    public struct SayEvent
    {
        public string EventName;
        [Multiline]
        public string Dialogue;
    }
    [Header("会話イベント")]
    public List<SayEvent> sayEvent;

    public bool dxtrg = false;
    public string tmpchildobj;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public struct SayView
    {
        public string[] characterNames;
        public int[] characterIDs;
        public int[] animationIDs;
        public int[] eventIDs;
        public string[] targetNames;
        [Multiline]
        public string[] dialogText;
    }
    public SayView[] sayView;
    [HideInInspector]
    public bool isSave;
    //private void Start()
    //{
    //    string tmplist = "";
    //    for (int i = 0; i < GManager.instance.ItemID.Length;)
    //    {
    //        tmplist += GManager.instance.ItemID[i].itemname + ",";
    //        i++;
    //    }
    //    //日替わり処理
    //    DateTime tmpdays = instance.GetGameDay();
    //    var oldYear = PlayerPrefs.GetInt("oldallYear", (tmpdays.Year - 1));
    //    var oldMonth = PlayerPrefs.GetInt("oldallMonth", (tmpdays.Month - 1));
    //    var oldDay = PlayerPrefs.GetInt("oldallDay", (tmpdays.Day - 1));
    //    DateTime olddays = new DateTime(oldYear, oldMonth, oldDay);
    //    if (Math.Abs(GManager.instance.AllSpanCheck(olddays)) > 0)
    //    {
    //        PlayerPrefs.SetInt("oldallYear", tmpdays.Year);
    //        PlayerPrefs.SetInt("oldallMonth", tmpdays.Month);
    //        PlayerPrefs.SetInt("oldallDay", tmpdays.Day);
    //        //その他日替わりセーブ
    //        PlayerPrefs.SetInt("DayAds", 0);
    //        PlayerPrefs.SetString("YpY9012nWJzXaBuS", "false");
    //        PlayerPrefs.Save();
    //        //日替わり処理
    //    }
    //}
    private void Update()
    {
        if (GManager.instance.walktrg && SceneManager.GetActiveScene().name.Contains("stage"))
        {
            reloadtime += Time.deltaTime;
            if (reloadtime > 15f)
            {
                reloadtime = 0f;
                Resources.UnloadUnusedAssets();
            }
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }
    }
    public void AllSaveInvoke()
    {
        isSave = true;
        for(int i=0; i < instance.Triggers.Length;)
        {
           PlayerPrefs.SetString("Triggers" + i.ToString(), instance.Triggers[i].ToString());
            i++;
        }
        PlayerPrefs.SetString("Coin", instance.Coin.ToString());
        for (int i = 0; i < instance.FreeNums.Length;)
        {
            PlayerPrefs.SetString("FreeNums" + i.ToString(), instance.FreeNums[i].ToString());
            i++;
        }
        PlayerPrefs.SetString("stageNumber", instance.stageNumber.ToString());
        for(int i=0; i < instance.ItemID.Count;)
        {
            PlayerPrefs.SetString("ItemID.gettrg" + i.ToString(), instance.ItemID[i].gettrg.ToString());

            PlayerPrefs.SetString("ItemID.itemnumber" + i.ToString(), instance.ItemID[i].itemnumber.ToString());
            i++;
        }
        for (int i = 0; i < instance.Pstatus.Count;)
        {
            PlayerPrefs.SetString("Pstatus.getpl" + i.ToString(), instance.Pstatus[i].getpl.ToString());
            PlayerPrefs.SetString("Pstatus.hp" + i.ToString(), instance.Pstatus[i].hp.ToString());
            PlayerPrefs.SetString("Pstatus.mp" + i.ToString(), instance.Pstatus[i].mp.ToString());
            PlayerPrefs.SetString("Pstatus.Lv" + i.ToString(), instance.Pstatus[i].Lv.ToString());
            PlayerPrefs.SetString("Pstatus.inputExp" + i.ToString(), instance.Pstatus[i].inputExp.ToString());
            for(int gm=0; gm < instance.Pstatus[i].getMagic.Length;)
            {
                PlayerPrefs.SetString("Pstatus.getMagic" + i.ToString() + ".gettrg" + gm.ToString(), instance.Pstatus[i].getMagic[gm].gettrg.ToString());
                gm++;
            }
            for (int sm = 0; sm < instance.Pstatus[i].magicSet.Length;)
            {
                PlayerPrefs.SetString("Pstatus.magicSet" + i.ToString() + "." + sm.ToString(), instance.Pstatus[i].magicSet[sm].ToString());
                sm++;
            }
            PlayerPrefs.SetString("Pstatus.add_at" + i.ToString(), instance.Pstatus[i].add_at.ToString());
            PlayerPrefs.SetString("Pstatus.add_df" + i.ToString(), instance.Pstatus[i].add_df.ToString());
            i++;
        }
        for (int i = 0; i < instance.playerselect.Length;)
        {
            PlayerPrefs.SetString("playerselect" + i.ToString(), instance.playerselect[i].ToString());
            i++;
        }
        PlayerPrefs.SetString("playerStartPos.x", instance.playerStartPos.x.ToString());
        PlayerPrefs.SetString("playerStartPos.y", instance.playerStartPos.y.ToString());
        PlayerPrefs.SetString("playerStartPos.z", instance.playerStartPos.z.ToString());
        for(int i=0; i < instance.missionID.Count;)
        {
            PlayerPrefs.SetString("missionID.inputmission" + i.ToString(), instance.missionID[i].inputmission.ToString());
            i++;
        }
        for (int i = 0; i < instance.achievementsID.Count;)
        {
            PlayerPrefs.SetString("achievementsID.gettrg" + i.ToString(), instance.achievementsID[i].gettrg.ToString());
            i++;
        }
        for (int i = 0; i < instance.enemynoteID.Count;)
        {
            PlayerPrefs.SetString("enemynoteID.gettrg" + i.ToString(), instance.enemynoteID[i].gettrg.ToString());
            i++;
        }
        for (int i = 0; i < instance._craftRecipe.Count;)
        {
            PlayerPrefs.SetString("craftRecipe.isOnlyone" + i.ToString(), instance._craftRecipe[i].isOnlyone.ToString());
            i++;
        }
        PlayerPrefs.SetString("sunTime", instance.sunTime.ToString());
        PlayerPrefs.SetString("daycount", instance.daycount.ToString());
        
        for (int i = 0; i < instance._craftRecipe.Count;)
        {
            PlayerPrefs.SetString("_craftRecipe.get_recipe" + i.ToString(), instance._craftRecipe[i].get_recipe.ToString());
            i++;
        }
        PlayerPrefs.SetString("old_year", instance.old_year.ToString());
        PlayerPrefs.SetString("isEnglish", GManager.instance.isEnglish.ToString());
        PlayerPrefs.SetString("audioMax", GManager.instance.audioMax.ToString());
        PlayerPrefs.SetString("seMax", GManager.instance.seMax.ToString());
        PlayerPrefs.Save();
        PlayerPrefs.Save();
        isSave = false;
    }
    public void AllLoadInvoke()
    {
        for (int i = 1; i < instance.Triggers.Length;)
        {
            instance.Triggers[i] = int.Parse(PlayerPrefs.GetString("Triggers" + i.ToString(), "0"));
            i++;
        }
        instance.Triggers[0] = 1;
        instance.Triggers[1] = 0;
        instance.Triggers[2] = 0;
        instance.Triggers[4] = 0;

        instance.Coin = int.Parse(PlayerPrefs.GetString("Coin", "0"));

        for (int i = 0; i < instance.FreeNums.Length;)
        {
            instance.FreeNums[i] = float.Parse(PlayerPrefs.GetString("FreeNums" + i.ToString(), "0"));
            i++;
        }

        instance.stageNumber = int.Parse(PlayerPrefs.GetString("stageNumber", "0"));

        for (int i = 0; i < instance.ItemID.Count;)
        {
            var item = instance.ItemID[i];
           if(i!=0) item.gettrg = int.Parse(PlayerPrefs.GetString("ItemID.gettrg" + i.ToString(), "0"));
           else item.gettrg = int.Parse(PlayerPrefs.GetString("ItemID.gettrg" + i.ToString(), "1"));

            if (i != 0) item.itemnumber = int.Parse(PlayerPrefs.GetString("ItemID.itemnumber" + i.ToString(), "0"));
            else item.itemnumber = int.Parse(PlayerPrefs.GetString("ItemID.itemnumber" + i.ToString(), "3"));
            instance.ItemID[i] = item;
            i++;
        }
        for (int i = 0; i < instance.Pstatus.Count;)
        {
            var pstatus = instance.Pstatus[i];
            pstatus.getpl = int.Parse(PlayerPrefs.GetString("Pstatus.getpl" + i.ToString(), "0"));
            if (i == 0) pstatus.getpl = 1;

            pstatus.hp = int.Parse(PlayerPrefs.GetString("Pstatus.hp" + i.ToString(), pstatus.maxHP.ToString()));

            pstatus.mp = int.Parse(PlayerPrefs.GetString("Pstatus.mp" + i.ToString(), pstatus.maxMP.ToString()));

            pstatus.Lv = int.Parse(PlayerPrefs.GetString("Pstatus.Lv" + i.ToString(), "1"));

            pstatus.inputExp = int.Parse(PlayerPrefs.GetString("Pstatus.inputExp" + i.ToString(), "0"));
            for (int gm = 1; gm < instance.Pstatus[i].getMagic.Length;)
            {
                pstatus.getMagic[gm].gettrg = int.Parse(PlayerPrefs.GetString("Pstatus.getMagic" + i.ToString() + ".gettrg" + gm.ToString(), "0"));
                gm++;
            }
            for (int sm = 1; sm < instance.Pstatus[i].magicSet.Length;)
            {
                pstatus.magicSet[sm] = int.Parse(PlayerPrefs.GetString("Pstatus.magicSet" + i.ToString() + "." + sm.ToString(), "-1"));
                sm++;
            }

            pstatus.add_at = int.Parse(PlayerPrefs.GetString("Pstatus.add_at" + i.ToString(), "0"));

            pstatus.add_df = int.Parse(PlayerPrefs.GetString("Pstatus.add_df" + i.ToString(), "0"));
            instance.Pstatus[i] = pstatus;
            i++;
        }
        var pstatus2 = instance.Pstatus[0];
        pstatus2.getpl = 1;
        instance.Pstatus[0] = pstatus2;
        for (int i = 0; i < instance.playerselect.Length;)
        {
            instance.playerselect[i] = int.Parse(PlayerPrefs.GetString("playerselect" + i.ToString(), "-1"));
            i++;
        }
        instance.playerselect[1] = 0;
        var tmpStartPos = instance.playerStartPos;
        tmpStartPos.x = float.Parse(PlayerPrefs.GetString("playerStartPos.x", "0"));
        tmpStartPos.y = float.Parse(PlayerPrefs.GetString("playerStartPos.y", "0"));
        tmpStartPos.z = float.Parse(PlayerPrefs.GetString("playerStartPos.z", "0"));
        instance.playerStartPos = tmpStartPos;

        for (int i = 0; i < instance.missionID.Count;)
        {
            var mission = instance.missionID[i];
            mission.inputmission = int.Parse(PlayerPrefs.GetString("missionID.inputmission" + i.ToString(), "0"));
            instance.missionID[i] = mission;
            i++;
        }
        for (int i = 0; i < instance.achievementsID.Count;)
        {
            var achiev = instance.achievementsID[i];
            achiev.gettrg = int.Parse(PlayerPrefs.GetString("achievementsID.gettrg" + i.ToString(), "0"));
            instance.achievementsID[i] = achiev;
            i++;
        }
        for (int i = 0; i < instance.enemynoteID.Count;)
        {
            var enemy = instance.enemynoteID[i];
            enemy.gettrg = int.Parse(PlayerPrefs.GetString("enemynoteID.gettrg" + i.ToString(), "0"));
            instance.enemynoteID[i] = enemy;
            i++;
        }
        for (int i = 0; i < instance._craftRecipe.Count;)
        {
            var craft = instance._craftRecipe[i];
            craft.isOnlyone=int.Parse(PlayerPrefs.GetString("craftRecipe.isOnlyone" + i.ToString(), "0"));
            instance._craftRecipe[i] = craft;
            i++;
        }
        instance.sunTime = float.Parse(PlayerPrefs.GetString("sunTime", "180"));

        instance.daycount = int.Parse(PlayerPrefs.GetString("daycount", "1"));

        for (int i = 0; i < instance._craftRecipe.Count;)
        {
            var craft = instance._craftRecipe[i];
            craft.get_recipe = int.Parse(PlayerPrefs.GetString("_craftRecipe.get_recipe" + i.ToString(), "0"));
            instance._craftRecipe[i] = craft;
            i++;
        }
        instance.old_year = int.Parse(PlayerPrefs.GetString("old_year", "2024"));

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Japanese:
                GManager.instance.isEnglish = int.Parse(PlayerPrefs.GetString("isEnglish", "0"));
                break;
            default:
                GManager.instance.isEnglish = int.Parse(PlayerPrefs.GetString("isEnglish", "1"));
                break;
        }

        float tmpaudio = 0.16f;
        GManager.instance.audioMax = float.Parse(PlayerPrefs.GetString("audioMax",tmpaudio.ToString()));
        tmpaudio = 0.32f;
        GManager.instance.seMax = float.Parse(PlayerPrefs.GetString("seMax",tmpaudio.ToString()));
    }

    //public void TxtDataAdd(string dataname, string adddata)
    //{
    //    bool dataExists = false;
    //    string[] reader = PlayerPrefs.GetString("AllData", "").Split('\n'); 
    //    string result = "";
    //    for (int i = 0; i < reader.Length;)
    //    {
    //        if (reader[i].StartsWith($"[\"{dataname}\","))
    //        {
    //            reader[i] = $"[\"{dataname}\",\"{adddata}\"]";
    //            dataExists = true;
    //            break;
    //        }
    //        i++;
    //    }
    //    if (!dataExists)
    //    {
    //        var tempList = new List<string>(reader);
    //        tempList.Add($"[\"{dataname}\",\"{adddata}\"]");
    //        reader = tempList.ToArray();
    //    }
    //    for (int i = 0; i < reader.Length;)
    //    {
    //        result += reader[i] + "\n";
    //        i++;
    //    }
    //    PlayerPrefs.SetString("AllData", result);
    //}

    //public void TxtDataUpdate(string dataname, string updatedata)
    //{

    //    bool dataExists = false;
    //    string[] reader = PlayerPrefs.GetString("AllData", "").Split('\n'); 
    //    string result = "";
    //    for (int i = 0; i < reader.Length;)
    //    {
    //        if (reader[i].StartsWith($"[\"{dataname}\","))
    //        {
    //            reader[i] = $"[\"{dataname}\",\"{updatedata}\"]";
    //            dataExists = true;
    //            break;
    //        }
    //        i++;
    //    }
    //    for (int i = 0; i < reader.Length;)
    //    {
    //        result += reader[i] + "\n";
    //        i++;
    //    }
    //    PlayerPrefs.SetString("AllData", result);
    //    if (!dataExists)
    //    {
    //        TxtDataAdd(dataname, updatedata);
    //    }
    //}

    //public string TxtDataGet(string dataname)
    //{
    //    string[] reader = PlayerPrefs.GetString("AllData", "").Split('\n'); ;
    //    foreach(string line in reader) { 
    //        if (line.StartsWith($"[\"{dataname}\","))
    //        {
    //            var splitData = line.Split(new[] { "\",\"" }, System.StringSplitOptions.None);
    //            if (splitData.Length == 2)
    //            {
    //                return splitData[1].TrimEnd(']').Trim('"');
    //            }
    //        }
    //    }
    //    return "None";
    //}

    public DateTime GetGameDay()
    {
        DateTime tmp = DateTime.Today;
        return tmp;
    }
    public int AllSpanCheck(DateTime tmp_time)
    {
        int check_result = 0;

        DateTime today = DateTime.Today;
        DateTime devday = new DateTime(instance.devdays.year, instance.devdays.month, instance.devdays.day);
        if (instance.checkdev != devday)
            today = devday;
        DateTime newday = new DateTime(today.Year, tmp_time.Month, tmp_time.Day);
        TimeSpan tmpdiff = newday - today;
        check_result = (int)tmpdiff.TotalDays;
        //print(check_result.ToString());
        return check_result;
    }
    public bool MonthBoolCheck(DateTime tmp_time)
    {
        bool check_result = false;
        DateTime today = DateTime.Today;
        DateTime devday = new DateTime(instance.devdays.year, instance.devdays.month, instance.devdays.day);
        if (instance.checkdev != devday)
            today = devday;
        DateTime newday = new DateTime(today.Year, tmp_time.Month, tmp_time.Day);
        if (newday.Month == today.Month)
            check_result = true;
        return check_result;
    }
    public int DaySpanCheck(DateTime tmp_time)
    {
        int check_result = 0;
        DateTime today = DateTime.Today;
        DateTime devday = new DateTime(instance.devdays.year, instance.devdays.month, instance.devdays.day);
        if (instance.checkdev != devday)
            today = devday;
        DateTime newday = new DateTime(today.Year, tmp_time.Month, tmp_time.Day);
        check_result = newday.Day - today.Day;
        //print(check_result.ToString());
        return check_result;
    }
    public int NewOldSpanCheck(DateTime new_time, DateTime old_time)
    {
        int check_result = 0;
        TimeSpan tmpdiff = new_time - old_time;
        check_result = (int)tmpdiff.TotalDays;
      //  print(check_result.ToString());
        return check_result;
    }
}