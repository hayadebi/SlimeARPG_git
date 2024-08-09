using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
namespace UniLang
{
    public class BattleSystem : MonoBehaviour
    {
        [Header("会話イベント格納")]
        public UniLang.OriginalDialogueSystem dialogueSystem;
        [Header("BattleSystem表示本体")]
        public GameObject systemMain;
        [Header("None画像")]
        public Sprite NoneSprite;
        [Header("オーディオソース格納")]
        public AudioSource[] audioSource;
        [Multiline]
        public string sememo;
        public AudioClip[] se;
        [Header("バトルシステム本体のアニメータ―格納")]
        public Animator systemAnim;
        [Header("背景画像元格納")]
        public SpriteRenderer backGround;

        [System.Serializable]
        public struct AllDialogUI
        {
            [System.Serializable]
            public struct LogDialog
            {
                public Image logBackground;
                public Text logText;
            }
            [Header("戦闘ログ関連")]
            public LogDialog logDialog;
            [System.Serializable]
            public struct CommandDialog
            {
                public Image commandBackground;
                [Header("攻撃コマンド")]
                public Image attackDialog;
                public Button[] attackButton;
                public Text[] attackName;
                public Text[] removeMp;
                [Header("アイテムコマンド")]
                public Image itemDialog;
                public Transform ItemContent;
                public Button[] itemButton;
                public Text[] itemName;
                public Text[] itemNum;
                public CommandId[] commandId;
            }
            [Header("戦闘コマンド関連")]
            public CommandDialog commandDialog;

            [System.Serializable]
            public struct PlayerSelectDialog
            {
                public Image pselectBackground;
                [Header("味方選択のボタン自体")]
                public Button[] charaButton;
                public Text[] charaName;
                [Header("コマンド予約済みの時に表示する用")]
                public GameObject[] selectedBackground;
                public Text[] selectedCommandText;

            }
            [Header("行動する味方選択関連")]
            public PlayerSelectDialog playerSelectDialog;

            [System.Serializable]
            public struct TargetSelectDialogE
            {
                public Image targetBackground;
                public Button[] targetButton;
                public Text[] targetName;
            }
            [Header("対象選択(敵)関連")]
            public TargetSelectDialogE targetSelectDialogE;
            [System.Serializable]
            public struct TargetSelectDialogP
            {
                public Image targetBackground;
                public Button[] targetButton;
                public Text[] targetName;
            }
            [Header("対象選択(味方)関連")]
            public TargetSelectDialogP targetSelectDialogP;

            [System.Serializable]
            public struct PlayerState
            {
                public GameObject content;
                public Image icon;
                public Image[] stateAnomaly;
                public Text lvName;
                public Slider hpSlider;
                public Slider mpSlider;
                public Slider cooltimeSlider;
                public Text nextCommand;
                public GameObject isOverUI;
            }
            [Header("味方サイドステータス関連")]
            public PlayerState[] playerState;

            [System.Serializable]
            public struct EnemyState
            {
                public GameObject content;
                public Image[] stateAnomaly;
                public Text lvName;
                public Slider hpSlider;
                public Slider cooltimeSlider;
                public Text nextCommand;
                public GameObject isOverUI;
            }
            [Header("敵サイドステータス関連")]
            public EnemyState[] enemyState;
            [System.Serializable]
            public struct BossUIObeject
            {
                public GameObject[] uiObject;
            }
            [Header("敵サイドステータス関連")]
            public BossUIObeject[] bossUIObject;

        }
        [Header("各Dialog格納")]
        public AllDialogUI allDialogUI;

        [System.Serializable]
        public struct RandomData
        {
            public int[] adventEnemyId;
            public int minLv;
            public int maxLv;
            public AudioClip setBgm;
            public Sprite battleBackground;
            public int maxaddState;
            public int maxAdvent;
            public int mingetcoin;
            public int maxgetcoin;
        }
        [Header("ランダム範囲*バトル開始メソッド呼び出す前に設定")]
        public RandomData randomData;
        public enum CommandType
        {
            None,
            Attack,//通常攻撃、魔法(4つまで)
            Guard,//数秒間守る
            Search,//敵の弱点を探る
            Item,//アイテム使用
            Escape,//逃げる
        }


        [System.Serializable]
        public struct Enemy
        {
            public int enemyId;
            public int lv;
            public float coolTime;
            public int[] magicId;
            public int maxHP;
            public int hp;
            public int maxMP;
            public int mp;
            public int defense;
            public int attack;
            public int getExp;
            public int add_hp;
            public int add_mp;
            public int add_at;
            public int add_df;
            public int tmpadd_hp;
            public int tmpadd_mp;
            public int tmpadd_at;
            public int tmpadd_df;
            public DataManager.AiType aiType;
            public int[] stateAnomaly;
            //コマンド選択状態関連
            public CommandType selectCommandType;
            public int selectCommandId;//コマンド自体ではなくて、コマンドで選択した技やアイテム等のDataManagerのID
            public DataManager.TargetType targetType;
            public int targetId;
            public bool isSelected;
            public string name;
            [HideInInspector]
            public bool isGuard;
            public bool isOver;
            public Animator charaAnim;
            public SpriteRenderer spriteRen;
            public Animator effectAnim;
            [HideInInspector]
            public bool isGuardUnUsed;
            [HideInInspector]
            public bool isSupport;
            [HideInInspector]
            public bool isCommand;
            [HideInInspector]
            public int isEffect_Guarddamage;
        }
        [Header("出現した戦闘中敵情報(3体まで)")]
        public Enemy[] enemyData;
        public enum UIMode
        {
            NoSelected,
            CharaSelected,
            TargetSelected,
            AttackSelected,
            ItemSelected,
        }
        [System.Serializable]
        public struct Player
        {
            public int playerId;
            public int lv;
            public float coolTime;
            public int[] magicId;
            public int maxHP;
            public int hp;
            public int maxMP;
            public int mp;
            public int defense;
            public int attack;
            public int tmpadd_hp;
            public int tmpadd_mp;
            public int tmpadd_at;
            public int tmpadd_df;
            public int[] stateAnomaly;
            //コマンド選択状態関連
            public CommandType selectCommandType;
            public int selectCommandId;//コマンド自体ではなくて、コマンドで選択した技やアイテム等のDataManagerのID
            public DataManager.TargetType targetType;
            public int targetId;
            public bool isSelected;
            [HideInInspector]
            public bool isGuard;
            public bool isOver;
            public Animator charaAnim;
            public SpriteRenderer spriteRen;
            public Animator effectAnim;
            [HideInInspector]
            public bool isCommand;
            [HideInInspector]
            public int isEffect_Guarddamage;
        }
        [Header("戦闘中味方情報(3体まで)")]
        public Player[] playerData;
        [Header("行動させようとしているキャラ")]
        public int selectPlayer = 1;
        [Header("戦闘画面の状態(モード)")]
        [SerializeField]
        public UIMode uiMode;
        [Header("戦闘不能時のお墓アニメーター")]
        public RuntimeAnimatorController graveAnimator;
        private List<GameObject> ItemButton = new List<GameObject>();
        private AudioClip defaultBGM;
        private Translator translator;
        private float shakePower = 6f;
        private bool isEnd = false;
        [System.Serializable]
        public struct TypeText
        {
            public string typeName;
            public DataManager.MagicType magicType;
            public Color colorSet;
        }
        [Header("属性の色設定")]
        public TypeText[] typeText;
        private bool enemyBattleStart = false;
        private float updateTime = 0;
        private int bossEnemydataIndex = 0;
        private int effectoldid = -1;
        private Animator effectoldanim;
        
        public IEnumerator EvBattleStart()
        {
            //導入アニメーション開始
            systemAnim.SetInteger("trg", 1);
            //ここから内部の情報設定
            enemyBattleStart = false;
            isEnd = false;
            //戦闘状態にする。
            DataManager.instance.Triggers[0] = 0;
            DataManager.instance.Triggers[1] = 1;
            //背景とBGM設定
            backGround.sprite = randomData.battleBackground;
            audioSource[0].Stop();
            defaultBGM = audioSource[0].clip;
            audioSource[0].clip = randomData.setBgm;
            audioSource[0].loop = true;
            audioSource[0].Play();
            int tmpenemyMax = randomData.maxAdvent;
            //敵情報設定
            for (int i = 0; i < enemyData.Length;)
            {
                if (i < tmpenemyMax)
                {
                    //IDとレベル設定
                    enemyData[i].enemyId = randomData.adventEnemyId[i];
                    if (enemyData[i].enemyId != -1)
                    {
                        enemyData[i].lv = UnityEngine.Random.Range(randomData.minLv, randomData.maxLv + 1);
                        enemyData[i].coolTime = 0;
                        List<int> tmpMagicList = new List<int>();
                        for (int m = 1; m < DataManager.instance.enemynoteID[enemyData[i].enemyId].getMagic.Length;)
                        {
                            if (DataManager.instance.enemynoteID[enemyData[i].enemyId].getMagic[m].inputlevel <= enemyData[i].lv) tmpMagicList.Add(DataManager.instance.enemynoteID[enemyData[i].enemyId].getMagic[m].magicid);
                            m++;
                        }
                        //魔法設定
                        enemyData[i].magicId[0] = 0;
                        for (int l = 1; l < enemyData[i].magicId.Length;)
                        {
                            if (tmpMagicList.Count > 0)
                            {
                                tmpMagicList = tmpMagicList.OrderBy(a => Guid.NewGuid()).ToList();
                                enemyData[i].magicId[l] = tmpMagicList[0];
                                tmpMagicList.Remove(tmpMagicList[0]);
                            }
                            else enemyData[i].magicId[l] = -1;
                            l++;
                        }
                        //ステータス設定
                        int maxaddState = UnityEngine.Random.Range(0, randomData.maxaddState);
                        //hp
                        enemyData[i].add_hp = UnityEngine.Random.Range(0, maxaddState);
                        maxaddState -= enemyData[i].add_hp;
                        enemyData[i].maxHP = (int)((DataManager.instance.enemynoteID[enemyData[i].enemyId].maxHP + enemyData[i].add_hp) * (1 + (enemyData[i].lv / 1.5f)));
                        enemyData[i].hp = enemyData[i].maxHP;
                        //mp
                        enemyData[i].add_mp = UnityEngine.Random.Range(0, maxaddState);
                        maxaddState -= enemyData[i].add_mp;
                        enemyData[i].maxMP = (int)((DataManager.instance.enemynoteID[enemyData[i].enemyId].maxMP + enemyData[i].add_mp) * (1 + (enemyData[i].lv / 1.5f)));
                        enemyData[i].mp = enemyData[i].maxMP;
                        //at
                        enemyData[i].add_at = UnityEngine.Random.Range(0, maxaddState);
                        maxaddState -= enemyData[i].add_at;
                        enemyData[i].attack = (int)((DataManager.instance.enemynoteID[enemyData[i].enemyId].attack + enemyData[i].add_at) * (1 + (enemyData[i].lv / 1.5f)));
                        //df
                        enemyData[i].add_df = UnityEngine.Random.Range(0, maxaddState);
                        maxaddState -= enemyData[i].add_df;
                        enemyData[i].defense = (int)((DataManager.instance.enemynoteID[enemyData[i].enemyId].defense + enemyData[i].add_df) * (1 + (enemyData[i].lv / 1.5f)));

                        enemyData[i].tmpadd_hp = 0;
                        enemyData[i].tmpadd_mp = 0;
                        enemyData[i].tmpadd_at = 0;
                        enemyData[i].tmpadd_df = 0;
                        enemyData[i].stateAnomaly[0] = -1;
                        enemyData[i].stateAnomaly[1] = -1;
                        enemyData[i].stateAnomaly[2] = -1;

                        enemyData[i].getExp = DataManager.instance.enemynoteID[enemyData[i].enemyId].getExp;
                        enemyData[i].aiType = DataManager.instance.enemynoteID[enemyData[i].enemyId].aiType;
                        enemyData[i].selectCommandType = CommandType.None;
                        enemyData[i].selectCommandId = 0;
                        enemyData[i].isGuard = false;
                        enemyData[i].isGuardUnUsed = false;
                        enemyData[i].isSupport = false;
                        enemyData[i].isOver = false;
                        enemyData[i].isSelected = false;
                        enemyData[i].isCommand = false;
                        allDialogUI.enemyState[i].isOverUI.SetActive(false);
                        //キャラアニメーション反映
                        enemyData[i].charaAnim.gameObject.SetActive(true);
                        enemyData[i].charaAnim.runtimeAnimatorController = DataManager.instance.enemynoteID[enemyData[i].enemyId].animC;


                        Color tmpcolor = enemyData[i].spriteRen.color;
                        tmpcolor.r = 1;
                        tmpcolor.g = 1;
                        tmpcolor.b = 1;
                        tmpcolor.a = 1;
                        enemyData[i].spriteRen.color = tmpcolor;
                        //表示サイズ反映
                        if (enemyData[i].enemyId != -1)
                        {
                            Vector3 tmppos = enemyData[i].charaAnim.gameObject.transform.position;
                            tmppos.y = DataManager.instance.enemynoteID[enemyData[i].enemyId].yPosition;
                            enemyData[i].charaAnim.gameObject.transform.position = tmppos;

                            var tmpscale = enemyData[i].charaAnim.gameObject.transform.localScale;
                            tmpscale.x = DataManager.instance.enemynoteID[enemyData[i].enemyId].battleSize;
                            tmpscale.y = DataManager.instance.enemynoteID[enemyData[i].enemyId].battleSize;
                            tmpscale.z = DataManager.instance.enemynoteID[enemyData[i].enemyId].battleSize;
                            enemyData[i].charaAnim.gameObject.transform.localScale = tmpscale;
                        }
                    }
                    else
                    {
                        enemyData[i].enemyId = -1;
                        //キャラアニメーション反映
                        enemyData[i].charaAnim.gameObject.SetActive(false);
                    }
                    SettingNewState(i, DataManager.TargetType.Enemy);
                }
                else
                {
                    enemyData[i].enemyId = -1;
                    //キャラアニメーション反映
                    enemyData[i].charaAnim.gameObject.SetActive(false);
                }
                i++;
            }
            //プレイヤー情報設定
            for (int i = 0; i < playerData.Length;)
            {
                if (DataManager.instance.playerselect[i] != -1)
                {
                    //IDとレベル設定
                    playerData[i].playerId = DataManager.instance.playerselect[i];
                    playerData[i].lv = DataManager.instance.Pstatus[playerData[i].playerId].Lv;
                    playerData[i].coolTime = 0;
                    //魔法設定
                    playerData[i].magicId = DataManager.instance.Pstatus[playerData[i].playerId].magicSet;
                    //ステータス設定
                    playerData[i].maxHP = (int)((DataManager.instance.Pstatus[playerData[i].playerId].maxHP + DataManager.instance.Pstatus[playerData[i].playerId].add_hp) * (1 + (playerData[i].lv / 1.5f)));
                    if (DataManager.instance.Pstatus[playerData[i].playerId].maxHP == DataManager.instance.Pstatus[playerData[i].playerId].hp) playerData[i].hp = playerData[i].maxHP;
                    else playerData[i].hp = DataManager.instance.Pstatus[playerData[i].playerId].hp;
                    playerData[i].maxMP = (int)((DataManager.instance.Pstatus[playerData[i].playerId].maxMP + DataManager.instance.Pstatus[playerData[i].playerId].add_mp) * (1 + (playerData[i].lv / 1.5f)));
                    if (DataManager.instance.Pstatus[playerData[i].playerId].maxMP == DataManager.instance.Pstatus[playerData[i].playerId].mp) playerData[i].mp = playerData[i].maxMP;
                    else playerData[i].mp = DataManager.instance.Pstatus[playerData[i].playerId].mp;
                    playerData[i].attack = (int)((DataManager.instance.Pstatus[playerData[i].playerId].attack + DataManager.instance.Pstatus[playerData[i].playerId].add_at) * (1 + (playerData[i].lv / 1.5f)));
                    playerData[i].defense = (int)((DataManager.instance.Pstatus[playerData[i].playerId].defense + DataManager.instance.Pstatus[playerData[i].playerId].add_df) * (1 + (playerData[i].lv / 1.5f)));
                    playerData[i].tmpadd_hp = 0;
                    playerData[i].tmpadd_mp = 0;
                    playerData[i].tmpadd_at = 0;
                    playerData[i].tmpadd_df = 0;
                    playerData[i].stateAnomaly[0] = -1;
                    playerData[i].stateAnomaly[1] = -1;
                    playerData[i].stateAnomaly[2] = -1;
                    playerData[i].selectCommandType = CommandType.None;
                    playerData[i].selectCommandId = 0;
                    playerData[i].targetType = DataManager.TargetType.Enemy;
                    playerData[i].isSelected = false;
                    playerData[i].isGuard = false;
                    allDialogUI.playerState[i].isOverUI.SetActive(false);
                    
                   playerData[i].isCommand = false;
                    selectPlayer = 1;
                    
                    //キャラアニメーション反映
                    playerData[i].charaAnim.gameObject.SetActive(true);
                    playerData[i].charaAnim.runtimeAnimatorController = DataManager.instance.Pstatus[playerData[i].playerId].animC;
                    Color tmpcolor = playerData[i].spriteRen.color;
                    tmpcolor.r = 1;
                    tmpcolor.g = 1;
                    tmpcolor.b = 1;
                    tmpcolor.a = 1;
                    playerData[i].spriteRen.color = tmpcolor;
                    if (playerData[i].hp > 0) playerData[i].isOver = false;
                    else
                    {
                        playerData[i].isOver = true;
                        allDialogUI.playerState[i].isOverUI.gameObject.SetActive(true);
                        playerData[i].charaAnim.runtimeAnimatorController = graveAnimator;
                    }
                    SettingNewState(i, DataManager.TargetType.Player);
                }
                else
                {
                    playerData[i].playerId = -1;
                    //キャラアニメーション反映
                    playerData[i].charaAnim.gameObject.SetActive(false);
                }
                i++;
            }
            allDialogUI.logDialog.logText.text = "";
            //ここまで内部の情報設定

            //ここからUI上に各ステータスを反映
            UpdateState(true);
            yield return new WaitForSeconds(3f);
            //ログに反映
            //敵のログ出力
            for (int i = 0; i < enemyData.Length;)
            {
                if (enemyData[i].enemyId != -1)
                {
                    if (!DataManager.instance.enemynoteID[enemyData[i].enemyId].isBoss)
                    {
                        //翻訳
                        translator.Run(enemyData[i].name + "が現れた！", results =>
                        {
                            foreach (var n in results)
                            {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                    }
                    else if (DataManager.instance.enemynoteID[enemyData[i].enemyId].isBoss)
                    {
                        //翻訳
                        translator.Run("強敵 "+enemyData[i].name + "が目の前に立ちはだかった！", results =>
                        {
                            foreach (var n in results)
                            {
                                // ログに表示
                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                    }
                    yield return new WaitForSeconds(0.3f);
                    //スキル内容確認
                    for (int s = 0; s < DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill.Length;)
                    {
                        if (DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill[s] == 0)//急所突き
                        {
                            //翻訳
                            translator.Run(enemyData[i].name + "は祝福を受けている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        //主人公力は発動時に別で表記
                        if (DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill[s] == 2)//急所突き
                        {
                            //翻訳
                            translator.Run(enemyData[i].name + "は鋭く構えている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        if (DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill[s] == 3)//俊敏
                        {
                            //翻訳
                            translator.Run(enemyData[i].name + "は俊敏に動いている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        if (DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill[s] == 4)//逃走癖
                        {
                            //翻訳
                            translator.Run(enemyData[i].name + "は怯えて隙を伺っている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        s++;
                    }
                }
                i++;
            }
            //味方のログ出力
            for (int i = 0; i < playerData.Length;)
            {
                if (playerData[i].playerId != -1)
                {
                    //スキル内容確認
                    for (int s = 0; s < DataManager.instance.Pstatus[playerData[i].playerId].inputskill.Length;)
                    {
                        if (DataManager.instance.Pstatus[playerData[i].playerId].inputskill[s] == 0)//急所突き
                        {
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[i].playerId].pname + "は祝福を受けている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        //主人公力は発動時に別で表記
                        if (DataManager.instance.Pstatus[playerData[i].playerId].inputskill[s] == 2)//急所突き
                        {
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[i].playerId].pname + "は鋭く構えている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        if (DataManager.instance.Pstatus[playerData[i].playerId].inputskill[s] == 3)//俊敏
                        {
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[i].playerId].pname + "は俊敏に動いている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        if (DataManager.instance.Pstatus[playerData[i].playerId].inputskill[s] == 4)//逃走癖
                        {
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[i].playerId].pname + "は怯えて隙を伺っている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        s++;
                    }
                }
                i++;
            }
            //表示の切り替え
            allDialogUI.commandDialog.attackDialog.gameObject.SetActive(false);
            allDialogUI.commandDialog.itemDialog.gameObject.SetActive(false);

            allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
            allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
            //その他
            allDialogUI.commandDialog.commandBackground.gameObject.SetActive(false);
            allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
            translator.Run("*クリックで敵との戦闘を開始する", results =>
            {
                foreach (var n in results)
                {
                    // ログに表示
                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                }
            });
            yield return WaitForClick();
            yield return new WaitForSeconds(2f);
            enemyBattleStart = true;
            IEnumerator WaitForClick()
            {
                // 左クリックまたはEnterキーが押されるまで待つ
                yield return new WaitUntil(() => (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)));
            }
        }
        public IEnumerator BattleStart()
        {
            //導入アニメーション開始
            systemAnim.SetInteger("trg", 1);
            //ここから内部の情報設定
            enemyBattleStart = false;
            isEnd = false;
            //戦闘状態にする。
            DataManager.instance.Triggers[0] = 0;
            DataManager.instance.Triggers[1] = 1;
            //背景とBGM設定
            backGround.sprite = randomData.battleBackground;
            audioSource[0].Stop();
            defaultBGM = audioSource[0].clip;
            audioSource[0].clip = randomData.setBgm;
            audioSource[0].loop = true;
            audioSource[0].Play();
            int tmpenemyMax = UnityEngine.Random.Range(1, randomData.maxAdvent);
            //敵情報設定
            for (int i = 0; i < enemyData.Length;)
            {
                if (i < tmpenemyMax)
                {
                    int tmprandom = UnityEngine.Random.Range(0, randomData.adventEnemyId.Length);
                    //IDとレベル設定
                    enemyData[i].enemyId = randomData.adventEnemyId[tmprandom];
                    enemyData[i].lv = UnityEngine.Random.Range(randomData.minLv, randomData.maxLv + 1);
                    enemyData[i].coolTime = 0;
                    List<int> tmpMagicList = new List<int>();
                    for (int m = 1; m < DataManager.instance.enemynoteID[enemyData[i].enemyId].getMagic.Length;)
                    {
                        if (DataManager.instance.enemynoteID[enemyData[i].enemyId].getMagic[m].inputlevel <= enemyData[i].lv) tmpMagicList.Add(DataManager.instance.enemynoteID[enemyData[i].enemyId].getMagic[m].magicid);
                        m++;
                    }
                    //魔法設定
                    enemyData[i].magicId[0] = 0;
                    for (int l = 1; l < enemyData[i].magicId.Length;)
                    {
                        if (tmpMagicList.Count > 0)
                        {
                            tmpMagicList= tmpMagicList.OrderBy(a => Guid.NewGuid()).ToList();
                            enemyData[i].magicId[l] = tmpMagicList[0];
                            tmpMagicList.Remove(tmpMagicList[0]);
                        }
                        else enemyData[i].magicId[l] = -1;
                        l++;
                    }
                    //ステータス設定
                    int maxaddState = UnityEngine.Random.Range(0, randomData.maxaddState);
                    //hp
                    enemyData[i].add_hp = UnityEngine.Random.Range(0, maxaddState);
                    maxaddState -= enemyData[i].add_hp;
                    enemyData[i].maxHP = (int)((DataManager.instance.enemynoteID[enemyData[i].enemyId].maxHP + enemyData[i].add_hp) * (1 + (enemyData[i].lv / 1.5f)));
                    enemyData[i].hp = enemyData[i].maxHP;
                    //mp
                    enemyData[i].add_mp = UnityEngine.Random.Range(0, maxaddState);
                    maxaddState -= enemyData[i].add_mp;
                    enemyData[i].maxMP = (int)((DataManager.instance.enemynoteID[enemyData[i].enemyId].maxMP + enemyData[i].add_mp) * (1 + (enemyData[i].lv / 1.5f)));
                    enemyData[i].mp = enemyData[i].maxMP;
                    //at
                    enemyData[i].add_at = UnityEngine.Random.Range(0, maxaddState);
                    maxaddState -= enemyData[i].add_at;
                    enemyData[i].attack = (int)((DataManager.instance.enemynoteID[enemyData[i].enemyId].attack + enemyData[i].add_at) * (1 + (enemyData[i].lv / 1.5f)));
                    //df
                    enemyData[i].add_df = UnityEngine.Random.Range(0, maxaddState);
                    maxaddState -= enemyData[i].add_df;
                    enemyData[i].defense = (int)((DataManager.instance.enemynoteID[enemyData[i].enemyId].defense + enemyData[i].add_df) * (1 + (enemyData[i].lv / 1.5f)));

                    enemyData[i].tmpadd_hp = 0;
                    enemyData[i].tmpadd_mp = 0;
                    enemyData[i].tmpadd_at = 0;
                    enemyData[i].tmpadd_df = 0;
                    enemyData[i].stateAnomaly[0] = -1;
                    enemyData[i].stateAnomaly[1] = -1;
                    enemyData[i].stateAnomaly[2] = -1;

                    enemyData[i].getExp = DataManager.instance.enemynoteID[enemyData[i].enemyId].getExp;
                    enemyData[i].aiType = DataManager.instance.enemynoteID[enemyData[i].enemyId].aiType;
                    enemyData[i].selectCommandType = CommandType.None;
                    enemyData[i].selectCommandId = 0;
                    enemyData[i].isGuard = false;
                    enemyData[i].isGuardUnUsed = false;
                    enemyData[i].isSupport = false;
                    enemyData[i].isOver = false;
                    enemyData[i].isSelected = false;
                    enemyData[i].isCommand = false;
                    allDialogUI.enemyState[i].isOverUI.SetActive(false);
                    //キャラアニメーション反映
                    enemyData[i].charaAnim.gameObject.SetActive(true);
                    enemyData[i].charaAnim.runtimeAnimatorController = DataManager.instance.enemynoteID[enemyData[i].enemyId].animC;


                    Color tmpcolor = enemyData[i].spriteRen.color;
                    tmpcolor.r = 1;
                    tmpcolor.g = 1;
                    tmpcolor.b = 1;
                    tmpcolor.a = 1;
                    enemyData[i].spriteRen.color = tmpcolor;
                    //表示サイズ反映
                    if (enemyData[i].enemyId != -1)
                    {
                        Vector3 tmppos = enemyData[i].charaAnim.gameObject.transform.position;
                        tmppos.y = DataManager.instance.enemynoteID[enemyData[i].enemyId].yPosition;
                        enemyData[i].charaAnim.gameObject.transform.position = tmppos;

                        var tmpscale = enemyData[i].charaAnim.gameObject.transform.localScale;
                        tmpscale.x = DataManager.instance.enemynoteID[enemyData[i].enemyId].battleSize;
                        tmpscale.y = DataManager.instance.enemynoteID[enemyData[i].enemyId].battleSize;
                        tmpscale.z = DataManager.instance.enemynoteID[enemyData[i].enemyId].battleSize;
                        enemyData[i].charaAnim.gameObject.transform.localScale = tmpscale;
                    }
                    SettingNewState(i, DataManager.TargetType.Enemy);
                }
                else
                {
                    enemyData[i].enemyId = -1;
                    //キャラアニメーション反映
                    enemyData[i].charaAnim.gameObject.SetActive(false);
                }
                i++;
            }
            //プレイヤー情報設定
            for (int i = 0; i < playerData.Length;)
            {
                if (DataManager.instance.playerselect[i] != -1)
                {
                    //IDとレベル設定
                    playerData[i].playerId = DataManager.instance.playerselect[i];
                    playerData[i].lv = DataManager.instance.Pstatus[playerData[i].playerId].Lv;
                    playerData[i].coolTime = 0;
                    //魔法設定
                    playerData[i].magicId = DataManager.instance.Pstatus[playerData[i].playerId].magicSet;
                    //ステータス設定
                    playerData[i].maxHP = (int)((DataManager.instance.Pstatus[playerData[i].playerId].maxHP + DataManager.instance.Pstatus[playerData[i].playerId].add_hp) * (1 + (playerData[i].lv / 1.5f)));
                    if (DataManager.instance.Pstatus[playerData[i].playerId].maxHP == DataManager.instance.Pstatus[playerData[i].playerId].hp) playerData[i].hp = playerData[i].maxHP;
                    else playerData[i].hp = DataManager.instance.Pstatus[playerData[i].playerId].hp;
                    playerData[i].maxMP = (int)((DataManager.instance.Pstatus[playerData[i].playerId].maxMP + DataManager.instance.Pstatus[playerData[i].playerId].add_mp) * (1 + (playerData[i].lv / 1.5f)));
                    if (DataManager.instance.Pstatus[playerData[i].playerId].maxMP == DataManager.instance.Pstatus[playerData[i].playerId].mp) playerData[i].mp = playerData[i].maxMP;
                    else playerData[i].mp = DataManager.instance.Pstatus[playerData[i].playerId].mp;
                    playerData[i].attack = (int)((DataManager.instance.Pstatus[playerData[i].playerId].attack + DataManager.instance.Pstatus[playerData[i].playerId].add_at) * (1 + (playerData[i].lv / 1.5f)));
                    playerData[i].defense = (int)((DataManager.instance.Pstatus[playerData[i].playerId].defense + DataManager.instance.Pstatus[playerData[i].playerId].add_df) * (1 + (playerData[i].lv / 1.5f)));
                    playerData[i].tmpadd_hp = 0;
                    playerData[i].tmpadd_mp = 0;
                    playerData[i].tmpadd_at = 0;
                    playerData[i].tmpadd_df = 0;
                    playerData[i].stateAnomaly[0] = -1;
                    playerData[i].stateAnomaly[1] = -1;
                    playerData[i].stateAnomaly[2] = -1;
                    playerData[i].selectCommandType = CommandType.None;
                    playerData[i].selectCommandId = 0;
                    playerData[i].targetType = DataManager.TargetType.Enemy;
                    playerData[i].isSelected = false;
                    playerData[i].isGuard = false;
                    allDialogUI.playerState[i].isOverUI.SetActive(false);
                    

                    playerData[i].isCommand = false;
                    selectPlayer = 1;
                    
                    //キャラアニメーション反映
                    playerData[i].charaAnim.gameObject.SetActive(true);
                    playerData[i].charaAnim.runtimeAnimatorController = DataManager.instance.Pstatus[playerData[i].playerId].animC;
                    Color tmpcolor = playerData[i].spriteRen.color;
                    tmpcolor.r = 1;
                    tmpcolor.g = 1;
                    tmpcolor.b = 1;
                    tmpcolor.a = 1;
                    playerData[i].spriteRen.color = tmpcolor;
                    if (playerData[i].hp > 0) playerData[i].isOver = false;
                    else
                    {
                        playerData[i].isOver = true;
                        allDialogUI.playerState[i].isOverUI.gameObject.SetActive(true);
                        playerData[i].charaAnim.runtimeAnimatorController = graveAnimator;
                    }
                    SettingNewState(i, DataManager.TargetType.Player);
                }
                else
                {
                    playerData[i].playerId = -1;
                    //キャラアニメーション反映
                    playerData[i].charaAnim.gameObject.SetActive(false);
                }
                i++;
            }
            allDialogUI.logDialog.logText.text = "";
            //ここまで内部の情報設定
            //実績
            if (DataManager.instance.achievementsID[0].gettrg < 1)
            {
                DataManager.instance.TextGet = "実績：" + DataManager.instance.achievementsID[0].name;
                var achi = DataManager.instance.achievementsID[0];
                achi.gettrg = 1;
                DataManager.instance.achievementsID[0] = achi;
            }
            //ここからUI上に各ステータスを反映
            UpdateState(true);
            yield return new WaitForSeconds(3f);
            //ログに反映
            //敵のログ出力
            for (int i = 0; i < enemyData.Length;)
            {
                if (enemyData[i].enemyId != -1)
                {
                    //翻訳
                    translator.Run(enemyData[i].name + "が現れた！", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return new WaitForSeconds(0.3f);
                    //スキル内容確認
                    for (int s = 0; s < DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill.Length;)
                    {
                        if (DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill[s] == 0)//急所突き
                        {
                            //翻訳
                            translator.Run(enemyData[i].name + "は祝福を受けている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        //主人公力は発動時に別で表記
                        if (DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill[s] == 2)//急所突き
                        {
                            //翻訳
                            translator.Run(enemyData[i].name + "は鋭く構えている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        if (DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill[s] == 3)//俊敏
                        {
                            //翻訳
                            translator.Run(enemyData[i].name + "は俊敏に動いている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        if (DataManager.instance.enemynoteID[enemyData[i].enemyId].inputskill[s] == 4)//逃走癖
                        {
                            //翻訳
                            translator.Run(enemyData[i].name + "は怯えて隙を伺っている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        s++;
                    }
                }
                i++;
            }
            //味方のログ出力
            for (int i = 0; i < playerData.Length;)
            {
                if (playerData[i].playerId != -1)
                {
                    //スキル内容確認
                    for (int s = 0; s < DataManager.instance.Pstatus[playerData[i].playerId].inputskill.Length;)
                    {
                        if (DataManager.instance.Pstatus[playerData[i].playerId].inputskill[s] == 0)//急所突き
                        {
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[i].playerId].pname + "は祝福を受けている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        //主人公力は発動時に別で表記
                        if (DataManager.instance.Pstatus[playerData[i].playerId].inputskill[s] == 2)//急所突き
                        {
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[i].playerId].pname + "は鋭く構えている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        if (DataManager.instance.Pstatus[playerData[i].playerId].inputskill[s] == 3)//俊敏
                        {
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[i].playerId].pname + "は俊敏に動いている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        if (DataManager.instance.Pstatus[playerData[i].playerId].inputskill[s] == 4)//逃走癖
                        {
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[i].playerId].pname + "は怯えて隙を伺っている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        s++;
                    }
                }
                i++;
            }
            //表示の切り替え
            allDialogUI.commandDialog.attackDialog.gameObject.SetActive(false);
            allDialogUI.commandDialog.itemDialog.gameObject.SetActive(false);

            allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
            allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);

            for(int b = 0; b < allDialogUI.bossUIObject.Length;)
            {
                for(int u = 0; u < allDialogUI.bossUIObject[b].uiObject.Length;)
                {
                    allDialogUI.bossUIObject[b].uiObject[u].SetActive(false);
                    u++;
                }
                b++;
            }
            //その他
            allDialogUI.commandDialog.commandBackground.gameObject.SetActive(false);
            allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
            translator.Run("*クリックで敵との戦闘を開始する", results =>
            {
                foreach (var n in results)
                {
                    // ログに表示
                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                }
            });
            yield return WaitForClick();
            yield return new WaitForSeconds(2f);
            enemyBattleStart = true;
            IEnumerator WaitForClick()
            {
                // 左クリックまたはEnterキーが押されるまで待つ
                yield return new WaitUntil(() => (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)));
            }
        }
        //追加の値は今後ここで初期設定
        void SettingNewState(int index,DataManager.TargetType targetType)
        {
            if (targetType == DataManager.TargetType.Enemy)
            {
                enemyData[index].isEffect_Guarddamage = 0;
            }
            else
            {
                playerData[index].isEffect_Guarddamage = 0;
            }
        }
        public void BoneInvoke(GameObject ViewObject ,AudioClip sound)
        {
            audioSource[1].PlayOneShot(sound);
            ViewObject.SetActive(false);
        }
        //ステータス表示アップデート
        void UpdateState(bool isStart = false)
        {
            //ここからUI上に各ステータスを反映
            //味方のステータスを反映
            for (int i = 0; i < 3;)
            {
                if (playerData[i].playerId != -1)
                {
                    if (!allDialogUI.playerState[i].content.activeSelf) allDialogUI.playerState[i].content.SetActive(true);
                    allDialogUI.playerState[i].icon.sprite = DataManager.instance.Pstatus[playerData[i].playerId].pimage;
                    for (int l = 0; l < allDialogUI.playerState[i].stateAnomaly.Length;)
                    {
                        if (playerData[i].stateAnomaly[l] != -1) allDialogUI.playerState[i].stateAnomaly[l].sprite = DataManager.instance.anomalyID[playerData[i].stateAnomaly[l]].icon;
                        else allDialogUI.playerState[i].stateAnomaly[l].sprite = NoneSprite;
                        l++;
                    }
                    allDialogUI.playerState[i].lvName.text = "Lv" + playerData[i].lv.ToString() + "　" + DataManager.instance.Pstatus[playerData[i].playerId].pname;
                    allDialogUI.playerState[i].hpSlider.maxValue = playerData[i].maxHP + playerData[i].tmpadd_hp;
                    if (isStart) allDialogUI.playerState[i].hpSlider.value = allDialogUI.playerState[i].hpSlider.maxValue;
                    allDialogUI.playerState[i].mpSlider.maxValue = playerData[i].maxMP + playerData[i].tmpadd_mp;
                    if (isStart) allDialogUI.playerState[i].mpSlider.value = allDialogUI.playerState[i].mpSlider.maxValue;
                    if (isStart) allDialogUI.playerState[i].cooltimeSlider.maxValue = 1;
                    if (isStart) allDialogUI.playerState[i].cooltimeSlider.value = 0;
                    if (isStart) allDialogUI.playerState[i].nextCommand.text = "次の行動:未選択\n対象:未選択";
                    if (isStart) allDialogUI.playerSelectDialog.selectedCommandText[i].text = "次の行動:未選択\n対象:未選択";
                    if(playerData[i].hp<=0||playerData[i].isOver)allDialogUI.playerState[i].isOverUI.SetActive(true);
                }
                else
                {
                    if (allDialogUI.playerState[i].content.activeSelf) allDialogUI.playerState[i].content.SetActive(false);
                }
                i++;
            }
            //コマンド内の攻撃ボタンも反映

            //敵のステータスを反映
            List<int> tmpid = new List<int>();
            for (int i = 0; i < 3;)
            {
                if (enemyData[i].enemyId != -1)
                {
                    string abc = "";
                    if (tmpid.Count > 0)
                    {
                        int tmpnum = 0;
                        foreach (int id in tmpid)
                        {
                            if (id == enemyData[i].enemyId) tmpnum += 1;
                        }
                        if (tmpnum == 0) abc = "A";
                        else if (tmpnum == 1) abc = "B";
                        else abc = "C";
                    }
                    else if (tmpid.Count < 1) abc = "A";
                    else abc = "A";
                    if(!DataManager.instance.enemynoteID[enemyData[i].enemyId].isBoss)enemyData[i].name = DataManager.instance.enemynoteID[enemyData[i].enemyId].name + abc;
                    else if (DataManager.instance.enemynoteID[enemyData[i].enemyId].isBoss) enemyData[i].name = DataManager.instance.enemynoteID[enemyData[i].enemyId].name +"【BOSS】";
                    tmpid.Add(enemyData[i].enemyId);

                    if (!allDialogUI.enemyState[i].content.activeSelf) allDialogUI.enemyState[i].content.SetActive(true);

                    for (int l = 0; l < allDialogUI.enemyState[i].stateAnomaly.Length;)
                    {
                        if (enemyData[i].stateAnomaly[l] != -1) allDialogUI.enemyState[i].stateAnomaly[l].sprite = DataManager.instance.anomalyID[enemyData[i].stateAnomaly[l]].icon;
                        else allDialogUI.enemyState[i].stateAnomaly[l].sprite = NoneSprite;
                        l++;
                    }
                    allDialogUI.enemyState[i].lvName.text = "Lv" + enemyData[i].lv.ToString() + "　" + enemyData[i].name;
                    if (DataManager.instance.enemynoteID[enemyData[i].enemyId].isBoss)
                    {
                        Color tmpcolor = allDialogUI.enemyState[i].lvName.color;
                        tmpcolor.r = 1;
                        tmpcolor.g = 0;
                        tmpcolor.b = 0;
                        tmpcolor.a = 1;
                        allDialogUI.enemyState[i].lvName.color = tmpcolor;
                    }
                    else if (!DataManager.instance.enemynoteID[enemyData[i].enemyId].isBoss)
                    {
                        Color tmpcolor = allDialogUI.enemyState[i].lvName.color;
                        tmpcolor.r = 1;
                        tmpcolor.g = 1;
                        tmpcolor.b = 1;
                        tmpcolor.a = 1;
                        allDialogUI.enemyState[i].lvName.color = tmpcolor;
                    }
                    allDialogUI.enemyState[i].hpSlider.maxValue = enemyData[i].maxHP + enemyData[i].tmpadd_hp;
                    if (isStart) allDialogUI.enemyState[i].hpSlider.value = allDialogUI.enemyState[i].hpSlider.maxValue;
                    if (isStart) allDialogUI.enemyState[i].cooltimeSlider.maxValue = 1;
                    if (isStart) allDialogUI.enemyState[i].cooltimeSlider.value = 0;
                    if (isStart) allDialogUI.enemyState[i].nextCommand.text = "次の行動:未選択\n対象:未選択";

                }
                else
                {
                    if (allDialogUI.enemyState[i].content.activeSelf) allDialogUI.enemyState[i].content.SetActive(false);
                }
                i++;
            }
            if (isStart) uiMode = UIMode.NoSelected;
            //ここまでUI上に各ステータスを反映
        }

        public void allmoveSet()//アニメーション内から呼び出す
        {
            systemMain.SetActive(true);
        }
        public void allmoveOff()//アニメーション内から呼び出す
        {
            systemMain.SetActive(false);
        }
        public void PlayerSelect(int id = 0)//行動キャラ選択メソッド(CommandIdから呼び出す)
        {
            if (!playerData[id].isSelected && !isEnd && !playerData[id].isOver)
            {
                selectPlayer = id;//一時的に行動させようとしているキャラの情報を保存
                audioSource[1].PlayOneShot(se[0]);
                uiMode = UIMode.CharaSelected;
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        public void ItemSelect(int id = 0)//使用アイテム選択(CommandIdから呼び出す)
        {
            if (DataManager.instance.ItemID[id].itemnumber > 0 && !isEnd && !playerData[selectPlayer].isOver)
            {
                playerData[selectPlayer].targetType = DataManager.instance.ItemID[id].targetType;
                playerData[selectPlayer].selectCommandId = id;
                playerData[selectPlayer].selectCommandType = CommandType.Item;
                uiMode = UIMode.TargetSelected;
                audioSource[1].PlayOneShot(se[0]);
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        public void MagicSelect(int id = 0)//使用攻撃技選択(CommandIdから呼び出す)
        {
            if (DataManager.instance.MagicID[playerData[selectPlayer].magicId[id]].removeMp <= playerData[selectPlayer].mp && !isEnd && !playerData[selectPlayer].isOver)
            {
                playerData[selectPlayer].targetType = DataManager.instance.MagicID[playerData[selectPlayer].magicId[id]].targetType;
                playerData[selectPlayer].selectCommandId = id;
                playerData[selectPlayer].selectCommandType = CommandType.Attack;
                uiMode = UIMode.TargetSelected;
                audioSource[1].PlayOneShot(se[0]);
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        public void TargetEnemySelect(int id = 0)//対象(敵)選択(CommandIdから呼び出す)
        {
            if (!isEnd && !playerData[selectPlayer].isOver && DataManager.instance.Triggers[2] <= 0)
            {
                switch (playerData[selectPlayer].selectCommandType)
                {
                    case (CommandType.Attack):
                        if (!enemyData[id].isOver)
                        {
                            audioSource[1].PlayOneShot(se[1]);
                            playerData[selectPlayer].targetId = id;
                            if (playerData[selectPlayer].coolTime <= 0)
                            {
                                //スキル干渉(俊敏)
                                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                                {
                                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                    {
                                        playerData[selectPlayer].coolTime = DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime / 4 * 3;
                                    }
                                    else playerData[selectPlayer].coolTime = DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime;
                                    i++;
                                }
                                allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue = playerData[selectPlayer].coolTime;
                                allDialogUI.playerState[selectPlayer].cooltimeSlider.value = allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue;
                            }
                            else
                            {
                                //スキル干渉(俊敏)
                                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                                {
                                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                    {
                                        playerData[selectPlayer].coolTime += (DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime/2) / 4 * 3;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += (DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime / 2) / 4 * 3;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += (DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime / 2) / 4 * 3;
                                    }
                                    else
                                    {
                                        playerData[selectPlayer].coolTime += (DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime / 2);
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += (DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime / 2);
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += (DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime / 2);
                                    }
                                    i++;
                                }
                            }

                            playerData[selectPlayer].targetType = DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].targetType;
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname + "は" + enemyData[playerData[selectPlayer].targetId].name + "に対して" + DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].useWord + "しようとしている", results =>
                             {
                                 foreach (var n in results)
                                 {
                                     // ログに表示
                                     allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                 }
                             });
                            playerData[selectPlayer].isSelected = true;
                            uiMode = UIMode.NoSelected;
                        }
                        else audioSource[1].PlayOneShot(se[3]);
                        break;
                    case (CommandType.Item):
                        if (!enemyData[id].isOver)
                        {
                            audioSource[1].PlayOneShot(se[1]);
                            playerData[selectPlayer].targetId = id;
                            if (playerData[selectPlayer].coolTime <= 0)
                            {
                                //スキル干渉(俊敏)
                                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                                {
                                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                    {
                                        playerData[selectPlayer].coolTime = DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 4 * 3;
                                    }
                                    else playerData[selectPlayer].coolTime = DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime;
                                    i++;
                                }
                                allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue = playerData[selectPlayer].coolTime;
                                allDialogUI.playerState[selectPlayer].cooltimeSlider.value = allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue;
                            }
                            else
                            {
                                //スキル干渉(俊敏)
                                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                                {
                                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                    {
                                        playerData[selectPlayer].coolTime += (DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime/2) / 4 * 3;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += (DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 2) / 4 * 3;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += (DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 2) / 4 * 3;
                                    }
                                    else
                                    {
                                        playerData[selectPlayer].coolTime += (DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 2);
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += (DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 2);
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += (DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 2);
                                    }
                                    i++;
                                }
                            }

                            playerData[selectPlayer].targetType = DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].targetType;
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname + "は" + enemyData[playerData[selectPlayer].targetId].name + "に対して" + DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].itemname + "を使おうとしている", results =>
                             {
                                 foreach (var n in results)
                                 {
                                     // ログに表示
                                     allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                 }
                             });
                            playerData[selectPlayer].isSelected = true;
                            uiMode = UIMode.NoSelected;
                        }
                        else audioSource[1].PlayOneShot(se[3]);
                        break;
                    case (CommandType.Search):
                        audioSource[1].PlayOneShot(se[1]);
                        playerData[selectPlayer].targetId = id;
                        if (playerData[selectPlayer].coolTime <= 0)
                        {
                            //スキル干渉(俊敏)
                            for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                            {
                                if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                {
                                    playerData[selectPlayer].coolTime = 2f / 4 * 3;
                                }
                                else playerData[selectPlayer].coolTime = 2f;
                                i++;
                            }
                            allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue = playerData[selectPlayer].coolTime;
                            allDialogUI.playerState[selectPlayer].cooltimeSlider.value = allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue;
                        }
                        else
                        {
                            //スキル干渉(俊敏)
                            for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                            {
                                if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                {
                                    playerData[selectPlayer].coolTime += 1f / 4 * 3;
                                    allDialogUI.playerState[selectPlayer].cooltimeSlider.value += 1f / 4 * 3;
                                }
                                else
                                {
                                    playerData[selectPlayer].coolTime += 1f;
                                    allDialogUI.playerState[selectPlayer].cooltimeSlider.value += 1f;
                                }
                                i++;
                            }
                        }

                        playerData[selectPlayer].targetType = DataManager.TargetType.Enemy;
                        //翻訳
                        translator.Run(DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname + "は" + enemyData[playerData[selectPlayer].targetId].name + "について探ろうとしている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                        playerData[selectPlayer].isSelected = true;
                        uiMode = UIMode.NoSelected;
                        break;
                    default:
                        audioSource[1].PlayOneShot(se[1]);
                        playerData[selectPlayer].targetId = id;
                        break;
                }
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        public void TargetPlayerSelect(int id = 0)//対象(味方)選択(CommandIdから呼び出す)
        {
            if (!isEnd && !playerData[selectPlayer].isOver && DataManager.instance.Triggers[2] <= 0)
            {
                string tmptarget = "自分";
                switch (playerData[selectPlayer].selectCommandType)
                {
                    case (CommandType.Attack):
                        if (!playerData[id].isOver)
                        {
                            audioSource[1].PlayOneShot(se[1]);
                            playerData[selectPlayer].targetId = id;
                            if (playerData[selectPlayer].coolTime <= 0)
                            {
                                //スキル干渉(俊敏)
                                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                                {
                                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                    {
                                        playerData[selectPlayer].coolTime = DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime / 4 * 3;
                                    }
                                    else playerData[selectPlayer].coolTime = DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime;
                                    i++;
                                }
                                allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue = playerData[selectPlayer].coolTime;
                                allDialogUI.playerState[selectPlayer].cooltimeSlider.value = allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue;
                            }
                            else
                            {
                                //スキル干渉(俊敏)
                                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                                {
                                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                    {
                                        playerData[selectPlayer].coolTime += DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime/2 / 4 * 3;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime/2 / 4 * 3;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime/2 / 4 * 3;
                                    }
                                    else
                                    {
                                        playerData[selectPlayer].coolTime += DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime/2;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime/2;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].starttime/2;
                                    }
                                    i++;
                                }
                            }

                            playerData[selectPlayer].targetType = DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].targetType;
                            if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname != DataManager.instance.Pstatus[playerData[playerData[selectPlayer].targetId].playerId].pname) tmptarget = DataManager.instance.Pstatus[playerData[playerData[selectPlayer].targetId].playerId].pname;
                            //翻訳
                            translator.Run(DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname + "は" + tmptarget + "に対して" + DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].useWord + "しようとしている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            playerData[selectPlayer].isSelected = true;
                            uiMode = UIMode.NoSelected;
                        }
                        else audioSource[1].PlayOneShot(se[3]);
                        break;
                    case (CommandType.Item):
                        if (!playerData[id].isOver)
                        {
                            audioSource[1].PlayOneShot(se[1]);
                            playerData[selectPlayer].targetId = id;
                            if (playerData[selectPlayer].coolTime <= 0)
                            {
                                //スキル干渉(俊敏)
                                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                                {
                                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                    {
                                        playerData[selectPlayer].coolTime = DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 4 * 3;
                                    }
                                    else playerData[selectPlayer].coolTime = DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime;
                                    i++;
                                }
                                allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue = playerData[selectPlayer].coolTime;
                                allDialogUI.playerState[selectPlayer].cooltimeSlider.value = allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue;
                            }
                            else
                            {
                                //スキル干渉(俊敏)
                                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                                {
                                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                    {
                                        playerData[selectPlayer].coolTime += DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 4 * 3;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 4 * 3;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime / 4 * 3;
                                    }
                                    else
                                    {
                                        playerData[selectPlayer].coolTime += DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime;
                                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].starttime;
                                    }
                                    i++;
                                }
                            }

                            playerData[selectPlayer].targetType = DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].targetType;
                            //翻訳
                            if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname != DataManager.instance.Pstatus[playerData[playerData[selectPlayer].targetId].playerId].pname) tmptarget = DataManager.instance.Pstatus[playerData[playerData[selectPlayer].targetId].playerId].pname;
                            translator.Run(DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname + "は" + tmptarget + "に対して" + DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].itemname + "を使おうとしている", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            playerData[selectPlayer].isSelected = true;
                            uiMode = UIMode.NoSelected;
                        }
                        else audioSource[1].PlayOneShot(se[3]);
                        break;
                    case (CommandType.Guard):
                        audioSource[1].PlayOneShot(se[11]);
                        StartCoroutine(EffectAnimation(playerData[selectPlayer].charaAnim, 4));

                        playerData[selectPlayer].targetId = id;
                        StartCoroutine(EffectAnimation(playerData[playerData[selectPlayer].targetId].effectAnim, 1));
                        if (playerData[selectPlayer].coolTime <= 0)
                        {
                            //スキル干渉(俊敏)
                            for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                            {
                                if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                {
                                    playerData[selectPlayer].coolTime = 5f * 4 / 3;
                                }
                                else playerData[selectPlayer].coolTime = 5f;
                                i++;
                            }
                            allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue = playerData[selectPlayer].coolTime;
                            allDialogUI.playerState[selectPlayer].cooltimeSlider.value = allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue;
                        }
                        else
                        {
                            //スキル干渉(俊敏)
                            for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                            {
                                if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                                {
                                    playerData[selectPlayer].coolTime += 5f/2 * 4 / 3;
                                    allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += 5f/2 / 4 * 3;
                                    allDialogUI.playerState[selectPlayer].cooltimeSlider.value += 5f/2 / 4 * 3;
                                }
                                else
                                {
                                    playerData[selectPlayer].coolTime += 5f/2;
                                    allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += 5f/2;
                                    allDialogUI.playerState[selectPlayer].cooltimeSlider.value += 5f/2;
                                }
                                i++;
                            }
                        }

                        playerData[selectPlayer].targetType = DataManager.TargetType.Player;
                        playerData[playerData[selectPlayer].targetId].isGuard = true;
                        //翻訳
                        if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname != DataManager.instance.Pstatus[playerData[playerData[selectPlayer].targetId].playerId].pname) tmptarget = DataManager.instance.Pstatus[playerData[playerData[selectPlayer].targetId].playerId].pname;
                        translator.Run(DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname + "は" + tmptarget + "を守ろうと、5秒間程防御態勢になっている", results =>
                        {
                            foreach (var n in results)
                            {
                                // ログに表示
                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                        //エフェクト処理
                        iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                        iTween.ShakePosition(allDialogUI.playerState[playerData[selectPlayer].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));

                        playerData[selectPlayer].isSelected = true;
                        uiMode = UIMode.NoSelected;
                        break;
                    default:
                        audioSource[1].PlayOneShot(se[1]);
                        playerData[selectPlayer].targetId = id;
                        break;
                }
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        public IEnumerator EffectAnimation(Animator anim, int effectId)
        {
            anim.SetInteger("trg", effectId);
            yield return null;
            var state = anim.GetCurrentAnimatorStateInfo(0);
            effectoldid = effectId;
            effectoldanim = anim;
            yield return new WaitForSeconds(state.length);
            if((effectoldid==effectId&&effectoldanim==anim)||(effectoldanim!=anim))anim.SetInteger("trg", 0);
        }
        public void SearchCommand()//調べるコマンド
        {
            if (!isEnd && !playerData[selectPlayer].isOver && DataManager.instance.Triggers[2] <= 0)
            {
                playerData[selectPlayer].targetType = DataManager.TargetType.Enemy;
                playerData[selectPlayer].selectCommandType = CommandType.Search;
                uiMode = UIMode.TargetSelected;
                audioSource[1].PlayOneShot(se[0]);
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        public void GuardCommand()//調べるコマンド
        {
            if (!isEnd && !playerData[selectPlayer].isOver&&playerData[selectPlayer].isEffect_Guarddamage<=0 && DataManager.instance.Triggers[2] <= 0)
            {
                playerData[selectPlayer].targetType = DataManager.TargetType.Player;
                playerData[selectPlayer].selectCommandType = CommandType.Guard;
                uiMode = UIMode.TargetSelected;
                audioSource[1].PlayOneShot(se[0]);
            }
            else
            {
                audioSource[1].PlayOneShot(se[3]);
                if (playerData[selectPlayer].isEffect_Guarddamage > 0)
                {
                    translator.Run(DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname + "は防御崩し状態で守る行動ができない…！", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                }
            }
        }
        public void ItemCommand()//調べるコマンド
        {
            if (!isEnd && !playerData[selectPlayer].isOver && DataManager.instance.Triggers[2] <= 0)
            {
                playerData[selectPlayer].selectCommandType = CommandType.Item;
                uiMode = UIMode.ItemSelected;
                audioSource[1].PlayOneShot(se[0]);
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        public void AttackCommand()//調べるコマンド
        {
            if (!isEnd && !playerData[selectPlayer].isOver && DataManager.instance.Triggers[2] <= 0)
            {
                playerData[selectPlayer].selectCommandType = CommandType.Attack;
                uiMode = UIMode.AttackSelected;
                audioSource[1].PlayOneShot(se[0]);
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        public void EscapeCommand()//逃げるコマンド
        {
            if (!isEnd && !playerData[selectPlayer].isOver&&DataManager.instance.Triggers[2] <= 0)
            {
                StartCoroutine(EscapeEvent());
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        public IEnumerator EscapeEvent()
        {
            playerData[selectPlayer].selectCommandType = CommandType.Escape;
            playerData[selectPlayer].targetId = -1;
            //スキル干渉(俊敏)
            if (playerData[selectPlayer].coolTime <= 0)
            {
                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                {
                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                    {
                        playerData[selectPlayer].coolTime = 5f / 4 * 3;
                    }
                    else playerData[selectPlayer].coolTime = 5f;
                    i++;
                }
                allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue = playerData[selectPlayer].coolTime;
                allDialogUI.playerState[selectPlayer].cooltimeSlider.value = allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue;
            }
            else
            {
                for (int i = 0; i < DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill.Length;)
                {
                    if (DataManager.instance.Pstatus[playerData[selectPlayer].playerId].inputskill[i] == 3)
                    {
                        playerData[selectPlayer].coolTime += 5f / 4 * 3;
                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += 2.5f / 4 * 3;
                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += 2.5f / 4 * 3;
                    }
                    else
                    {
                        playerData[selectPlayer].coolTime += 5;
                        allDialogUI.playerState[selectPlayer].cooltimeSlider.maxValue += 2.5f;
                        allDialogUI.playerState[selectPlayer].cooltimeSlider.value += 2.5f;
                    }
                    i++;
                }
            }
            yield return new WaitForSeconds(0.3f);
            //翻訳
            translator.Run(DataManager.instance.Pstatus[playerData[selectPlayer].playerId].pname + "は逃げる準備をしている", results =>
            {
                foreach (var n in results)
                {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                }
            });
            playerData[selectPlayer].isSelected = true;
            uiMode = UIMode.NoSelected;
            audioSource[1].PlayOneShot(se[1]);
        }
        public void ReturnCommand()//戻るコマンド
        {
            if (!isEnd)
            {
                audioSource[1].PlayOneShot(se[0]);
                switch (uiMode)
                {
                    case UIMode.CharaSelected:
                        uiMode = UIMode.NoSelected;
                        break;
                    case UIMode.ItemSelected:
                        uiMode = UIMode.CharaSelected;
                        break;
                    case UIMode.AttackSelected:
                        uiMode = UIMode.CharaSelected;
                        break;
                    //コマンド選択後の対象選択
                    case UIMode.TargetSelected:
                        if (playerData[selectPlayer].selectCommandType == CommandType.Attack) uiMode = UIMode.AttackSelected;
                        else if (playerData[selectPlayer].selectCommandType == CommandType.Item) uiMode = UIMode.ItemSelected;
                        else if (playerData[selectPlayer].selectCommandType == CommandType.Guard || playerData[selectPlayer].selectCommandType == CommandType.Search) uiMode = UIMode.CharaSelected;
                        break;
                    default:
                        break;
                }
            }
            else audioSource[1].PlayOneShot(se[3]);
        }
        private void Start()
        {
            audioSource[0] = GameObject.Find("BGM").GetComponent<AudioSource>();
            translator = Translator.Create(Language.Auto, GManager.instance.Languages[GManager.instance.isEnglish]);
        }
        // Update is called once per frame
        void NoSelected()
        {
            //表示の切り替え
            if (allDialogUI.commandDialog.attackDialog.gameObject.activeSelf) allDialogUI.commandDialog.attackDialog.gameObject.SetActive(false);
            if (allDialogUI.commandDialog.itemDialog.gameObject.activeSelf) allDialogUI.commandDialog.itemDialog.gameObject.SetActive(false);
            if (!allDialogUI.playerSelectDialog.pselectBackground.gameObject.activeSelf)
            {
                allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(true);
                //操作候補キャラのコマンド情報設定
                for (int i = 0; i < allDialogUI.playerSelectDialog.charaButton.Length;)
                {
                    if (playerData[i].playerId != -1)
                    {
                        if (!allDialogUI.playerSelectDialog.charaButton[i].gameObject.activeSelf) allDialogUI.playerSelectDialog.charaButton[i].gameObject.SetActive(true);
                        allDialogUI.playerSelectDialog.charaName[i].text = DataManager.instance.Pstatus[playerData[i].playerId].pname;
                        //既にコマンド予約済みの場合は選択できないようにする
                        if (playerData[i].isSelected)
                        {
                            if (!allDialogUI.playerSelectDialog.selectedBackground[i].gameObject.activeSelf) allDialogUI.playerSelectDialog.selectedBackground[i].gameObject.SetActive(true);
                            //予約したコマンド名とターゲットの表示
                            string tmpcommand = "";
                            string tmptarget = "";
                            //選択コマンド名
                            switch (playerData[i].selectCommandType)
                            {
                                case (CommandType.Attack):
                                    tmpcommand = DataManager.instance.MagicID[playerData[i].magicId[playerData[i].selectCommandId]].magicname;
                                    break;
                                case (CommandType.Guard):
                                    tmpcommand = "守る";
                                    break;
                                case (CommandType.Item):
                                    tmpcommand = DataManager.instance.ItemID[playerData[i].selectCommandId].itemname;
                                    break;
                                case (CommandType.Search):
                                    tmpcommand = "調べる";
                                    break;
                                case (CommandType.Escape):
                                    tmpcommand = "逃げる";
                                    break;
                                default:
                                    tmpcommand = "未選択";
                                    break;
                            }
                            //選択ターゲット名
                            if (playerData[i].targetId != -1)
                            {
                                switch (playerData[i].targetType)
                                {
                                    case (DataManager.TargetType.Enemy):
                                        tmptarget = enemyData[playerData[i].targetId].name;
                                        break;
                                    case (DataManager.TargetType.Player):
                                        tmptarget = DataManager.instance.Pstatus[playerData[playerData[i].targetId].playerId].pname;
                                        break;
                                    default:
                                        tmptarget = "未選択";
                                        break;
                                }
                            }
                            else tmptarget = "未選択";
                            allDialogUI.playerSelectDialog.selectedCommandText[i].text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
                            allDialogUI.playerState[i].nextCommand.text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
                        }
                        else if (allDialogUI.playerSelectDialog.selectedBackground[i].gameObject.activeSelf) allDialogUI.playerSelectDialog.selectedBackground[i].gameObject.SetActive(false);
                        //瀕死の場合
                        if (playerData[i].isOver)
                        {
                            if (!allDialogUI.playerSelectDialog.selectedBackground[i].gameObject.activeSelf) allDialogUI.playerSelectDialog.selectedBackground[i].gameObject.SetActive(true);
                            //予約したコマンド名とターゲットの表示
                            string tmpcommand = "選択不可";
                            string tmptarget = "選択不可";
                            allDialogUI.playerSelectDialog.selectedCommandText[i].text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
                            allDialogUI.playerState[i].nextCommand.text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
                        }

                    }
                    else
                    {
                        if (allDialogUI.playerSelectDialog.charaButton[i].gameObject.activeSelf) allDialogUI.playerSelectDialog.charaButton[i].gameObject.SetActive(false);
                    }
                    i++;
                }
            }
            if (allDialogUI.commandDialog.commandBackground.gameObject.activeSelf) allDialogUI.commandDialog.commandBackground.gameObject.SetActive(false);
            if (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
            if (allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
        }
        void NoSelectedEnemy()
        {
            //操作候補キャラのコマンド情報設定
            for (int i = 0; i < allDialogUI.enemyState.Length;)
            {
                if (enemyData[i].enemyId != -1)
                {
                    if (enemyData[i].isSelected)
                    {
                        //予約したコマンド名とターゲットの表示
                        string tmpcommand = "";
                        string tmptarget = "";
                        //選択コマンド名
                        switch (enemyData[i].selectCommandType)
                        {
                            case (CommandType.Attack):
                                tmpcommand = DataManager.instance.MagicID[enemyData[i].magicId[enemyData[i].selectCommandId]].magicname;
                                break;
                            case (CommandType.Guard):
                                tmpcommand = "守る";
                                break;
                            case (CommandType.Item):
                                tmpcommand = DataManager.instance.ItemID[enemyData[i].selectCommandId].itemname;
                                break;
                            case (CommandType.Search):
                                tmpcommand = "調べる";
                                break;
                            case (CommandType.Escape):
                                tmpcommand = "逃げる";
                                break;
                            default:
                                tmpcommand = "未選択";
                                break;
                        }
                        //選択ターゲット名
                        if (enemyData[i].targetId != -1)
                        {
                            switch (enemyData[i].targetType)
                            {
                                case (DataManager.TargetType.Enemy):
                                    tmptarget = enemyData[enemyData[i].targetId].name;
                                    break;
                                case (DataManager.TargetType.Player):
                                    tmptarget = DataManager.instance.Pstatus[playerData[enemyData[i].targetId].playerId].pname;
                                    break;
                                default:
                                    tmptarget = "未選択";
                                    break;
                            }
                        }
                        else tmptarget = "未選択";
                        allDialogUI.enemyState[i].nextCommand.text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
                    }
                    //瀕死の場合
                    if (enemyData[i].isOver)
                    {
                        //予約したコマンド名とターゲットの表示
                        string tmpcommand = "選択不可";
                        string tmptarget = "選択不可";
                        allDialogUI.enemyState[i].nextCommand.text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
                    }

                }
                i++;
            }
        }
        void GuardCommandEnemy(int e)
        {
            if (!enemyData[e].isCommand)
            {
                ////ここからガード選択処理////
                string tmptarget = "自分";
                enemyData[e].targetType = DataManager.TargetType.Enemy;
                enemyData[e].selectCommandType = CommandType.Guard;
                audioSource[1].PlayOneShot(se[11]);
                StartCoroutine(EffectAnimation(enemyData[e].charaAnim, 4));
                enemyData[e].targetId = e;
                StartCoroutine(EffectAnimation(enemyData[enemyData[e].targetId].effectAnim, 1));
                if (enemyData[e].coolTime <= 0)
                {
                    //スキル干渉(俊敏)
                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                    {
                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                        {
                            enemyData[e].coolTime = 5f * 4 / 3;
                        }
                        else enemyData[e].coolTime = 5f;
                        i++;
                    }
                    allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                    allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                }
                else
                {
                    //スキル干渉(俊敏)
                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                    {
                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                        {
                            enemyData[e].coolTime += 5f / 2 * 4 / 3;
                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += 5f / 2 / 4 * 3;
                            allDialogUI.enemyState[e].cooltimeSlider.value += 5f / 2 / 4 * 3;
                        }
                        else
                        {
                            enemyData[e].coolTime += 5f / 2;
                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += 5f / 2;
                            allDialogUI.enemyState[e].cooltimeSlider.value += 5f / 2;
                        }
                        i++;
                    }
                }
                enemyData[enemyData[e].targetId].isGuard = true;
                //翻訳
                if (enemyData[e].name != enemyData[enemyData[e].targetId].name) tmptarget = enemyData[enemyData[e].targetId].name;
                translator.Run(enemyData[e].name + "は" + tmptarget + "を守ろうと、5秒間程防御態勢になっている", results =>
                {
                    foreach (var n in results)
                    {
                    // ログに表示
                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                    }
                });
                //エフェクト処理
                iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                iTween.ShakePosition(allDialogUI.enemyState[enemyData[e].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));

                enemyData[e].isSelected = true;

                ////ここまででガード選択処理終了////
            }
        }
        void EscapeCommandEnemy(int e)
        {
            if (!enemyData[e].isCommand)
            {
                enemyData[e].selectCommandType = CommandType.Escape;
                enemyData[e].targetType = DataManager.TargetType.Enemy;
                enemyData[e].targetId = e;
                //スキル干渉(俊敏)
                if (enemyData[e].coolTime <= 0)
                {
                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                    {
                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                        {
                            enemyData[e].coolTime = 25 / 4 * 3;
                        }
                        else enemyData[e].coolTime = 25f;
                        i++;
                    }
                    allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                    allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                }
                else
                {
                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                    {
                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                        {
                            enemyData[e].coolTime += 25f / 2 / 4 * 3;
                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += 25f / 2 / 4 * 3;
                            allDialogUI.enemyState[e].cooltimeSlider.value += 25f / 2 / 4 * 3;
                        }
                        else
                        {
                            enemyData[e].coolTime += 20;
                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += 25f / 2;
                            allDialogUI.enemyState[e].cooltimeSlider.value += 25f / 2;
                        }
                        i++;
                    }
                }
                //翻訳
                translator.Run(enemyData[e].name + "は逃げる準備をしている", results =>
                {
                    foreach (var n in results)
                    {
                    // ログに表示
                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                    }
                });
                enemyData[e].isSelected = true;
                audioSource[1].PlayOneShot(se[1]);
            }
        }
        void AttackEnemyCommand(int e, DataManager.AiType aiType)
        {
            if (!enemyData[e].isCommand)
            {
                enemyData[e].selectCommandType = CommandType.Attack;
                List<int> validityMagics = new List<int>();
                int randomMagic = -1;
                int targetId = -1;
                switch (aiType)
                {
                    case DataManager.AiType.Random://ランダムAI
                                                   //攻撃をランダムに決定
                        validityMagics = new List<int>();
                        for (int m = 0; m < enemyData[e].magicId.Length;)
                        {
                            if (enemyData[e].magicId[m] != -1 && DataManager.instance.MagicID[enemyData[e].magicId[m]].removeMp <= enemyData[e].mp) validityMagics.Add(m);
                            m++;
                        }
                        randomMagic = validityMagics[(int)UnityEngine.Random.Range(0, validityMagics.Count)];
                        //攻撃を設定
                        if (DataManager.instance.MagicID[enemyData[e].magicId[randomMagic]].targetType == DataManager.TargetType.Player)
                            enemyData[e].targetType = DataManager.TargetType.Enemy;
                        else if (DataManager.instance.MagicID[enemyData[e].magicId[randomMagic]].targetType == DataManager.TargetType.Enemy)
                            enemyData[e].targetType = DataManager.TargetType.Player;
                        enemyData[e].selectCommandId = randomMagic;
                        //ターゲットを設定
                        //攻撃対象等の設定
                        List<int> vilidityTarget = new List<int>();
                        int randomTarget = -1;
                        switch (enemyData[e].targetType)
                        {
                            case DataManager.TargetType.Enemy://敵
                                                              //ランダムに決定
                                vilidityTarget = new List<int>();
                                for (int i = 0; i < enemyData.Length;)
                                {
                                    if (enemyData[i].enemyId != -1 && !enemyData[i].isOver) vilidityTarget.Add(i);
                                    i++;
                                }
                                randomTarget = vilidityTarget[(int)UnityEngine.Random.Range(0, vilidityTarget.Count)];
                                //ターゲット設定
                                enemyData[e].targetId = randomTarget;
                                if (enemyData[e].coolTime <= 0)
                                {
                                    //スキル干渉(俊敏)
                                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                    {
                                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                        {
                                            enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                        }
                                        else enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                        i++;
                                    }
                                    allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                                    allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                                }
                                else
                                {
                                    //スキル干渉(俊敏)
                                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                    {
                                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                        {
                                            enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                            allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                        }
                                        else
                                        {
                                            enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                            allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                        }
                                        i++;
                                    }
                                }

                                string tmptarget = "自分";
                                if (enemyData[e].name != enemyData[enemyData[e].targetId].name) tmptarget = enemyData[enemyData[e].targetId].name;
                                //翻訳
                                translator.Run(enemyData[e].name + "は" + tmptarget + "に対して" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].useWord + "しようとしている", results =>
                                {
                                    foreach (var n in results)
                                    {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                    }
                                });
                                enemyData[e].isSelected = true;
                                break;
                            case DataManager.TargetType.Player://プレイヤー
                                                               //ランダムに決定
                                vilidityTarget = new List<int>();
                                for (int i = 0; i < playerData.Length;)
                                {
                                    if (playerData[i].playerId != -1 && !playerData[i].isOver) vilidityTarget.Add(i);
                                    i++;
                                }
                                randomTarget = vilidityTarget[(int)UnityEngine.Random.Range(0, vilidityTarget.Count)];
                                //ターゲット設定
                                enemyData[e].targetId = randomTarget;
                                if (enemyData[e].coolTime <= 0)
                                {
                                    //スキル干渉(俊敏)
                                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                    {
                                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                        {
                                            enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                        }
                                        else enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                        i++;
                                    }
                                    allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                                    allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                                }
                                else
                                {
                                    //スキル干渉(俊敏)
                                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                    {
                                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                        {
                                            enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                            allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                        }
                                        else
                                        {
                                            enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                            allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                        }
                                        i++;
                                    }
                                }

                                //翻訳
                                translator.Run(enemyData[e].name + "は" + DataManager.instance.Pstatus[playerData[enemyData[e].targetId].playerId].pname + "に対して" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].useWord + "しようとしている", results =>
                                {
                                    foreach (var n in results)
                                    {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                    }
                                });
                                enemyData[e].isSelected = true;
                                break;
                        }
                        break;
                    case DataManager.AiType.Optimization://最適化AI
                        bool isSelected = false;
                        //回復技がある上に、敵(味方)の中にHPが三分の一しか残っていないのがいたら優先的に使う
                        for (int m = 0; m < enemyData[e].magicId.Length;)
                        {
                            if (enemyData[e].magicId[m] != -1 && DataManager.instance.MagicID[enemyData[e].magicId[m]].removeMp <= enemyData[e].mp && DataManager.instance.MagicID[enemyData[e].magicId[m]].magicEvent == DataManager.MagicEvent.HpCure && DataManager.instance.MagicID[enemyData[e].magicId[m]].targetType == DataManager.TargetType.Player)
                            {
                                for (int other = 0; other < enemyData.Length;)
                                {
                                    if (enemyData[other].enemyId != -1 && enemyData[other].hp <= enemyData[other].maxHP / 5 * 2 && !enemyData[other].isOver)//回復対象がいた
                                    {
                                        targetId = other;
                                        //回復を設定
                                        if (DataManager.instance.MagicID[enemyData[e].magicId[m]].targetType == DataManager.TargetType.Player)
                                            enemyData[e].targetType = DataManager.TargetType.Enemy;
                                        else if (DataManager.instance.MagicID[enemyData[e].magicId[m]].targetType == DataManager.TargetType.Enemy)
                                            enemyData[e].targetType = DataManager.TargetType.Player;
                                        enemyData[e].selectCommandId = m;
                                        enemyData[e].targetId = targetId;

                                        if (enemyData[e].coolTime <= 0)
                                        {
                                            //スキル干渉(俊敏)
                                            for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                            {
                                                if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                {
                                                    enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                }
                                                else enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                i++;
                                            }
                                            allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                                            allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                                        }
                                        else
                                        {
                                            //スキル干渉(俊敏)
                                            for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                            {
                                                if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                {
                                                    enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                    allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                    allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                }
                                                else
                                                {
                                                    enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                    allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                    allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                }
                                                i++;
                                            }
                                        }

                                        string tmptarget = "自分";
                                        if (enemyData[e].name != enemyData[enemyData[e].targetId].name) tmptarget = enemyData[enemyData[e].targetId].name;
                                        //翻訳
                                        translator.Run(enemyData[e].name + "は" + tmptarget + "に対して" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].useWord + "しようとしている", results =>
                                        {
                                            foreach (var n in results)
                                            {
                                            // ログに表示
                                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                            }
                                        });
                                        enemyData[e].isSelected = true;
                                        enemyData[e].isSupport = true;
                                        isSelected = true;
                                        break;
                                    }
                                    other++;
                                }
                                break;
                            }
                            m++;
                        }
                        if (!isSelected)
                        {
                            if (!enemyData[e].isSupport)//サポート選択可能
                            {
                                bool nosupport = true;
                                for (int m = 0; m < enemyData[e].magicId.Length;)
                                {
                                    //ダメージも回復も無いサポート魔法がある場合
                                    if (enemyData[e].magicId[m] != -1 && DataManager.instance.MagicID[enemyData[e].magicId[m]].removeMp <= enemyData[e].mp && DataManager.instance.MagicID[enemyData[e].magicId[m]].magicEvent != DataManager.MagicEvent.HpCure && DataManager.instance.MagicID[enemyData[e].magicId[m]].Damage == 0 && DataManager.instance.MagicID[enemyData[e].magicId[m]].targetType == DataManager.TargetType.Player)
                                    {
                                        nosupport = false;
                                        bool isSupport = false;
                                        for (int other = 0; other < enemyData.Length;)
                                        {
                                            if (enemyData[other].enemyId != -1 && !enemyData[other].isOver && DataManager.instance.enemynoteID[enemyData[other].enemyId].isBoss)//絶対サポートすべき対象がいた
                                            {
                                                targetId = other;
                                                enemyData[e].targetId = targetId;
                                                //設定
                                                if (DataManager.instance.MagicID[enemyData[e].magicId[m]].targetType == DataManager.TargetType.Player)
                                                    enemyData[e].targetType = DataManager.TargetType.Enemy;
                                                else if (DataManager.instance.MagicID[enemyData[e].magicId[m]].targetType == DataManager.TargetType.Enemy)
                                                    enemyData[e].targetType = DataManager.TargetType.Player;
                                                enemyData[e].selectCommandId = m;

                                                if (enemyData[e].coolTime <= 0)
                                                {
                                                    //スキル干渉(俊敏)
                                                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                    {
                                                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                        {
                                                            enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                        }
                                                        else enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                        i++;
                                                    }
                                                    allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                                                    allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                                                }
                                                else
                                                {
                                                    //スキル干渉(俊敏)
                                                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                    {
                                                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                        {
                                                            enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                            allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                        }
                                                        else
                                                        {
                                                            enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                            allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                            allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                        }
                                                        i++;
                                                    }
                                                }

                                                string tmptarget = "自分";
                                                if (enemyData[e].name != enemyData[enemyData[e].targetId].name) tmptarget = enemyData[enemyData[e].targetId].name;
                                                //翻訳
                                                translator.Run(enemyData[e].name + "は" + tmptarget + "に対して" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].useWord + "しようとしている", results =>
                                                {
                                                    foreach (var n in results)
                                                    {
                                                    // ログに表示
                                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                                    }
                                                });
                                                enemyData[e].isSelected = true;

                                                enemyData[e].isSupport = true;
                                                isSupport = true;
                                                break;
                                            }
                                            other++;
                                        }
                                        if (!isSupport)
                                        {
                                            //サポートすべきボスがいない場合タンダム
                                            List<int> randomList = new List<int>();
                                            for (int r = 0; r < enemyData.Length;)
                                            {
                                                if (enemyData[r].enemyId != -1 && !enemyData[r].isOver) randomList.Add(r);
                                                r++;
                                            }
                                            randomTarget = randomList[UnityEngine.Random.Range(0, randomList.Count)];
                                            targetId = randomTarget;
                                            enemyData[e].targetId = targetId;
                                            //設定
                                            if (DataManager.instance.MagicID[enemyData[e].magicId[m]].targetType == DataManager.TargetType.Player)
                                                enemyData[e].targetType = DataManager.TargetType.Enemy;
                                            else if (DataManager.instance.MagicID[enemyData[e].magicId[m]].targetType == DataManager.TargetType.Enemy)
                                                enemyData[e].targetType = DataManager.TargetType.Player;
                                            enemyData[e].selectCommandId = m;

                                            if (enemyData[e].coolTime <= 0)
                                            {
                                                //スキル干渉(俊敏)
                                                for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                {
                                                    if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                    {
                                                        enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                    }
                                                    else enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                    i++;
                                                }
                                                allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                                                allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                                            }
                                            else
                                            {
                                                //スキル干渉(俊敏)
                                                for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                {
                                                    if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                    {
                                                        enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                        allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                        allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                    }
                                                    else
                                                    {
                                                        enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                        allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                        allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                    }
                                                    i++;
                                                }
                                            }

                                            string tmptarget = "自分";
                                            if (enemyData[e].name != enemyData[enemyData[e].targetId].name) tmptarget = enemyData[enemyData[e].targetId].name;
                                            //翻訳
                                            translator.Run(enemyData[e].name + "は" + tmptarget + "に対して" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].useWord + "しようとしている", results =>
                                            {
                                                foreach (var n in results)
                                                {
                                                // ログに表示
                                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                                }
                                            });
                                            enemyData[e].isSelected = true;

                                            enemyData[e].isSupport = true;
                                            break;
                                        }
                                    }
                                    else nosupport = true;
                                    m++;
                                }
                                if (nosupport)//サポート選択不可
                                {
                                    //攻撃魔法選択
                                    AttackSelect();
                                    enemyData[e].isSupport = false;
                                }
                            }
                            else//サポート選択不可
                            {
                                //攻撃魔法選択
                                AttackSelect();
                                enemyData[e].isSupport = false;
                            }
                        }
                        break;
                }
                audioSource[1].PlayOneShot(se[1]);
                //攻撃魔法選択呼出し
                void AttackSelect()
                {
                    List<int> exceptionallyEffective = new List<int>();
                    List<int> exceptionallyTarget = new List<int>();
                    for (int m = 0; m < enemyData[e].magicId.Length;)
                    {
                        for (int p = 0; p < playerData.Length;)
                        {
                            if (playerData[p].playerId != -1 && !playerData[p].isOver && DataManager.instance.MagicID[enemyData[e].magicId[m]].removeMp <= enemyData[e].mp && DataManager.instance.MagicID[enemyData[e].magicId[m]].targetType == DataManager.TargetType.Enemy && DataManager.instance.MagicID[enemyData[e].magicId[m]].Damage < 0 && (DataManager.instance.MagicID[enemyData[e].magicId[m]].magicType == DataManager.instance.Pstatus[playerData[p].playerId].badtype || DataManager.instance.MagicID[enemyData[e].magicId[m]].magicType == DataManager.instance.Pstatus[playerData[p].playerId].badtype2))
                            {
                                exceptionallyEffective.Add(m);
                                exceptionallyTarget.Add(p);
                            }
                            p++;
                        }
                        m++;
                    }
                    //弱点技が一個でもある場合。
                    if (exceptionallyEffective.Count > 0)
                    {
                        int maxdamage = 0;
                        int maxmagicid = 0;
                        for (int x = 0; x < exceptionallyEffective.Count;)
                        {
                            if (DataManager.instance.MagicID[enemyData[e].magicId[exceptionallyEffective[x]]].Damage < maxdamage)
                            {
                                maxdamage = DataManager.instance.MagicID[enemyData[e].magicId[exceptionallyEffective[x]]].Damage;
                                maxmagicid = x;
                            }
                            x++;
                        }
                        enemyData[e].targetType = DataManager.TargetType.Player;
                        enemyData[e].targetId = exceptionallyTarget[maxmagicid];
                        enemyData[e].selectCommandId = exceptionallyEffective[maxmagicid];

                        if (enemyData[e].coolTime <= 0)
                        {
                            //スキル干渉(俊敏)
                            for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                            {
                                if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                {
                                    enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 2) / 4 * 3;
                                }
                                else enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 2);
                                i++;
                            }
                            allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                            allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                        }
                        else
                        {
                            //スキル干渉(俊敏)
                            for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                            {
                                if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                {
                                    enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 2) / 4 * 3;
                                    allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 2) / 4 * 3;
                                }
                                else
                                {
                                    enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 2);
                                    allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 2);
                                }
                                i++;
                            }
                        }

                        //翻訳
                        translator.Run(enemyData[e].name + "は" + DataManager.instance.Pstatus[playerData[enemyData[e].targetId].playerId].pname + "に対して" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].useWord + "しようとしている", results =>
                        {
                            foreach (var n in results)
                            {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                        enemyData[e].isSelected = true;
                    }
                    //弱点技が無い場合
                    else
                    {
                        //最大HPが少ないプレイヤーを洗い出してターゲットに設定
                        int minhp = 9999999;
                        int minplayerid = 0;
                        for (int m = 0; m < playerData.Length;)
                        {
                            if (playerData[m].playerId != -1 && playerData[m].isOver && playerData[m].hp < minhp)
                            {
                                minhp = playerData[m].mp;
                                minplayerid = m;
                            }
                            m++;
                        }
                        enemyData[e].targetType = DataManager.TargetType.Player;
                        enemyData[e].targetId = minplayerid;
                        //最大ダメージの魔法を選択
                        int maxdamage = 0;
                        int maxmagicid = 0;
                        for (int x = 0; x < enemyData[e].magicId.Length;)
                        {
                            if (DataManager.instance.MagicID[enemyData[e].magicId[x]].Damage < maxdamage && DataManager.instance.MagicID[enemyData[e].magicId[x]].removeMp <= enemyData[e].mp && DataManager.instance.MagicID[enemyData[e].magicId[x]].targetType == DataManager.TargetType.Enemy && DataManager.instance.MagicID[enemyData[e].magicId[x]].Damage < 0)
                            {
                                maxdamage = DataManager.instance.MagicID[enemyData[e].magicId[x]].Damage;
                                maxmagicid = x;
                            }
                            x++;
                        }
                        enemyData[e].selectCommandId = maxmagicid;

                        if (enemyData[e].coolTime <= 0)
                        {
                            //スキル干渉(俊敏)
                            for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                            {
                                if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                {
                                    enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                }
                                else enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                i++;
                            }
                            allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                            allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                        }
                        else
                        {
                            //スキル干渉(俊敏)
                            for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                            {
                                if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                {
                                    enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                    allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                }
                                else
                                {
                                    enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                    allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                }
                                i++;
                            }
                        }

                        //翻訳
                        translator.Run(enemyData[e].name + "は" + DataManager.instance.Pstatus[playerData[enemyData[e].targetId].playerId].pname + "に対して" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].useWord + "しようとしている", results =>
                        {
                            foreach (var n in results)
                            {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                        enemyData[e].isSelected = true;
                    }
                }
            }
        }
        void Update()
        {
            if (DataManager.instance.Triggers[1] == 1)
            {
                //UI関連
                switch (uiMode)
                {
                    //操作キャラ選択前の状態
                    case UIMode.NoSelected:
                        NoSelected();
                        break;
                    //行動キャラ選択後のコマンド
                    case UIMode.CharaSelected:
                        //表示の切り替え
                        if (allDialogUI.commandDialog.attackDialog.gameObject.activeSelf) allDialogUI.commandDialog.attackDialog.gameObject.SetActive(false);
                        if (allDialogUI.commandDialog.itemDialog.gameObject.activeSelf) allDialogUI.commandDialog.itemDialog.gameObject.SetActive(false);
                        if (!allDialogUI.commandDialog.commandBackground.gameObject.activeSelf)
                        {
                            allDialogUI.commandDialog.commandBackground.gameObject.SetActive(true);
                            //選択中キャラの通常攻撃含む魔法をコマンドUIに反映
                            if (allDialogUI.commandDialog.attackButton[allDialogUI.commandDialog.attackButton.Length - 1].gameObject.activeSelf) allDialogUI.commandDialog.attackButton[allDialogUI.commandDialog.attackButton.Length - 1].gameObject.SetActive(false);
                            for (int i = 0; i < allDialogUI.commandDialog.attackButton.Length - 1;)
                            {
                                if (playerData[selectPlayer].magicId[i] != -1)
                                {
                                    if (!allDialogUI.commandDialog.attackButton[i].gameObject.activeSelf) allDialogUI.commandDialog.attackButton[i].gameObject.SetActive(true);
                                    allDialogUI.commandDialog.attackName[i].text = DataManager.instance.MagicID[playerData[selectPlayer].magicId[i]].magicname;
                                    for (int m = 0; m < typeText.Length;)
                                    {
                                        if (typeText[m].magicType == DataManager.instance.MagicID[playerData[selectPlayer].magicId[i]].magicType)
                                        {
                                            Color _color = allDialogUI.commandDialog.attackName[i].color;
                                            _color = typeText[m].colorSet;
                                            allDialogUI.commandDialog.attackName[i].color = _color;
                                        }
                                        m++;
                                    }
                                    allDialogUI.commandDialog.removeMp[i].text = "消費MP:" + DataManager.instance.MagicID[playerData[selectPlayer].magicId[i]].removeMp.ToString();
                                }
                                else if (allDialogUI.commandDialog.attackButton[i].gameObject.activeSelf) allDialogUI.commandDialog.attackButton[i].gameObject.SetActive(false);
                                i++;
                            }
                            if (!allDialogUI.commandDialog.attackButton[allDialogUI.commandDialog.attackButton.Length - 1].gameObject.activeSelf) allDialogUI.commandDialog.attackButton[allDialogUI.commandDialog.attackButton.Length - 1].gameObject.SetActive(true);
                            //選択できるアイテムのボタンUIを反映
                            if (allDialogUI.commandDialog.itemButton[1].gameObject.activeSelf) allDialogUI.commandDialog.itemButton[1].gameObject.SetActive(false);
                            if (ItemButton.Count > 0)
                            {
                                foreach (GameObject obj in ItemButton)
                                {
                                    Destroy(obj.gameObject);
                                }
                            }
                            ItemButton = new List<GameObject>();
                            for (int i = 0; i < DataManager.instance.ItemID.Count;)
                            {
                                if (DataManager.instance.ItemID[i].itemnumber > 0 && DataManager.instance.ItemID[i].isBattleUse)
                                {
                                    allDialogUI.commandDialog.itemName[0].text = DataManager.instance.ItemID[i].itemname;
                                    allDialogUI.commandDialog.itemNum[0].text = "残り個数:" + DataManager.instance.ItemID[i].itemnumber.ToString();
                                    allDialogUI.commandDialog.commandId[0].SelectId = i;
                                    GameObject tmpobj = Instantiate(allDialogUI.commandDialog.itemButton[0].gameObject, transform.position, transform.rotation, allDialogUI.commandDialog.ItemContent);
                                    tmpobj.SetActive(true);
                                    ItemButton.Add(tmpobj);
                                }
                                i++;
                            }
                            if (!allDialogUI.commandDialog.itemButton[1].gameObject.activeSelf) allDialogUI.commandDialog.itemButton[1].gameObject.SetActive(true);
                        }
                        if (allDialogUI.playerSelectDialog.pselectBackground.gameObject.activeSelf) allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
                        if (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                        if (allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);

                        break;
                    //コマンド選択後の対象選択
                    case UIMode.TargetSelected:
                        //表示の切り替え
                        if (allDialogUI.commandDialog.attackDialog.gameObject.activeSelf) allDialogUI.commandDialog.attackDialog.gameObject.SetActive(false);
                        if (allDialogUI.commandDialog.itemDialog.gameObject.activeSelf) allDialogUI.commandDialog.itemDialog.gameObject.SetActive(false);
                        //調べる
                        if (playerData[selectPlayer].selectCommandType == CommandType.Search && (!allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf || allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf))
                        {
                            allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(true);
                            allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
                        }
                        //防御
                        else if (playerData[selectPlayer].selectCommandType == CommandType.Guard && (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf || !allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf))
                        {
                            allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                            allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(true);
                        }
                        //攻撃
                        else if (playerData[selectPlayer].selectCommandType == CommandType.Attack && DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].targetType == DataManager.TargetType.Enemy && (!allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf || allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf))
                        {
                            allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(true);
                            allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
                        }
                        else if (playerData[selectPlayer].selectCommandType == CommandType.Attack && DataManager.instance.MagicID[playerData[selectPlayer].magicId[playerData[selectPlayer].selectCommandId]].targetType == DataManager.TargetType.Player && (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf || !allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf))
                        {
                            allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                            allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(true);
                        }
                        //アイテム
                        else if (playerData[selectPlayer].selectCommandType == CommandType.Item && DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].targetType == DataManager.TargetType.Enemy && (!allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf || allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf))
                        {
                            allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(true);
                            allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
                        }
                        else if (playerData[selectPlayer].selectCommandType == CommandType.Item && DataManager.instance.ItemID[playerData[selectPlayer].selectCommandId].targetType == DataManager.TargetType.Player && (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf || !allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf))
                        {
                            allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                            allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(true);
                        }
                        //対象敵いるボタンだけ有効化
                        for (int i = 0; i < allDialogUI.targetSelectDialogE.targetButton.Length;)
                        {
                            if (enemyData[i].enemyId != -1 && enemyData[i].hp > 0)
                            {
                                if (!allDialogUI.targetSelectDialogE.targetButton[i].gameObject.activeSelf) allDialogUI.targetSelectDialogE.targetButton[i].gameObject.SetActive(true);
                                allDialogUI.targetSelectDialogE.targetName[i].text = enemyData[i].name;
                            }
                            else if (allDialogUI.targetSelectDialogE.targetButton[i].gameObject.activeSelf)
                            {
                                allDialogUI.targetSelectDialogE.targetButton[i].gameObject.SetActive(false);
                            }
                            i++;
                        }
                        //対象味方いるボタンだけ有効化
                        for (int i = 0; i < allDialogUI.targetSelectDialogP.targetButton.Length;)
                        {
                            if (playerData[i].playerId != -1 && ((playerData[i].hp > 0 && playerData[i].selectCommandType != CommandType.Item) || (playerData[i].selectCommandType == CommandType.Item && (playerData[i].hp > 0 || DataManager.instance.ItemID[playerData[i].selectCommandId].isDeadUsed))))
                            {
                                if (!allDialogUI.targetSelectDialogP.targetButton[i].gameObject.activeSelf) allDialogUI.targetSelectDialogP.targetButton[i].gameObject.SetActive(true);
                                allDialogUI.targetSelectDialogP.targetName[i].text = DataManager.instance.Pstatus[playerData[i].playerId].pname;
                            }
                            else if (allDialogUI.targetSelectDialogP.targetButton[i].gameObject.activeSelf)
                            {
                                allDialogUI.targetSelectDialogP.targetButton[i].gameObject.SetActive(false);
                            }
                            i++;
                        }
                        //その他
                        if (allDialogUI.commandDialog.commandBackground.gameObject.activeSelf) allDialogUI.commandDialog.commandBackground.gameObject.SetActive(false);
                        if (allDialogUI.playerSelectDialog.pselectBackground.gameObject.activeSelf) allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);

                        break;
                    case UIMode.ItemSelected:
                        //表示の切り替え  
                        if (!allDialogUI.commandDialog.commandBackground.gameObject.activeSelf) allDialogUI.commandDialog.commandBackground.gameObject.SetActive(true);
                        if (allDialogUI.playerSelectDialog.pselectBackground.gameObject.activeSelf) allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
                        if (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                        if (allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
                        if (allDialogUI.commandDialog.attackDialog.gameObject.activeSelf) allDialogUI.commandDialog.attackDialog.gameObject.SetActive(false);
                        //アイテム
                        if (!allDialogUI.commandDialog.itemDialog.gameObject.activeSelf) allDialogUI.commandDialog.itemDialog.gameObject.SetActive(true);
                        break;
                    case UIMode.AttackSelected:
                        //表示の切り替え  
                        if (!allDialogUI.commandDialog.commandBackground.gameObject.activeSelf) allDialogUI.commandDialog.commandBackground.gameObject.SetActive(true);
                        if (allDialogUI.playerSelectDialog.pselectBackground.gameObject.activeSelf) allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
                        if (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                        if (allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
                        //魔法
                        if (!allDialogUI.commandDialog.attackDialog.gameObject.activeSelf) allDialogUI.commandDialog.attackDialog.gameObject.SetActive(true);
                        if (allDialogUI.commandDialog.itemDialog.gameObject.activeSelf) allDialogUI.commandDialog.itemDialog.gameObject.SetActive(false);
                        break;
                    default:
                        ;
                        break;
                }
                //キャラの行動関連
                if (!isEnd && updateTime <= 0)
                {
                    updateTime = 0.2f;
                    //味方
                    for (int p = 0; p < playerData.Length;)
                    {
                        //コマンド発動
                        if (playerData[p].playerId != -1 && !playerData[p].isOver && playerData[p].isSelected && playerData[p].coolTime <= 0 && DataManager.instance.Triggers[2]<=0)
                        {
                            switch (playerData[p].selectCommandType)
                            {
                                case CommandType.Escape://逃げる
                                    if (playerData[p].isSelected) StartCoroutine(EscapeInvoke(p));
                                    playerData[p].isSelected = false;

                                    break;
                                case CommandType.Search://調べる
                                    if (playerData[p].isSelected) StartCoroutine(SearchInvoke(p));
                                    playerData[p].isSelected = false;

                                    break;
                                case CommandType.Item://アイテム
                                    if (playerData[p].isSelected) StartCoroutine(ItemInvoke(p));
                                    playerData[p].isSelected = false;

                                    break;
                                case CommandType.Attack:
                                    if (playerData[p].isSelected) StartCoroutine(AttackInvoke(p));
                                    playerData[p].isSelected = false;

                                    break;
                                case CommandType.Guard:
                                    if (playerData[p].isSelected) StartCoroutine(GuardInvoke(p));
                                    playerData[p].isSelected = false;

                                    break;
                                default:
                                    break;
                            }
                        }
                        p++;
                    }
                    //敵
                    for (int e = 0; e < enemyData.Length;)
                    {
                        //コマンド発動
                        if (enemyData[e].enemyId != -1 && !enemyData[e].isOver && enemyData[e].enemyId != -1 && enemyBattleStart && DataManager.instance.Triggers[2] <= 0)
                        {
                            if (!enemyData[e].isSelected&&!enemyData[e].isCommand)//未選択状態
                            {
                                //AIごとに処理を分ける
                                switch (DataManager.instance.enemynoteID[enemyData[e].enemyId].aiType)
                                {
                                    case DataManager.AiType.Random://ランダムAI
                                                                   //相手のいずれかが自分を狙っているかチェック
                                        for (int t = 0; t < playerData.Length;)
                                        {
                                            //ガードを使っていいかつコマンド選択済みかつ攻撃コマンドを選択しているかつターゲットタイプが敵かつターゲットIDが自分ならかつクールタイムがないなら
                                            if (!enemyData[e].isSelected && !enemyData[e].isCommand&&!enemyData[e].isSelected&& enemyData[e].isEffect_Guarddamage<=0 && !enemyData[e].isGuardUnUsed && playerData[t].isSelected && playerData[t].selectCommandType == CommandType.Attack && playerData[t].targetType == DataManager.TargetType.Enemy && playerData[t].targetId == e)
                                            {
                                                //狙われてるぞ！危ない
                                                //守る行動を選択
                                                GuardCommandEnemy(e);
                                                enemyData[e].isCommand = true;
                                                enemyData[e].isGuardUnUsed = true;
                                                NoSelectedEnemy();
                                            }
                                            else if (!enemyData[e].isSelected && !enemyData[e].isCommand&&enemyData[e].coolTime <= 0 ) //あとは基本ランダム行動
                                            {
                                                bool isAction = false;
                                                bool isEscape = false;
                                                enemyData[e].selectCommandType = CommandType.Attack;
                                                List<int> validityMagics = new List<int>();
                                                int randomMagic = 4;
                                                //もしボスなら、HP半分以下の時に高確率で専用技を使う
                                                if (DataManager.instance.enemynoteID[enemyData[e].enemyId].isBoss && enemyData[e].hp <= enemyData[e].maxHP / 2 && UnityEngine.Random.Range(0, 3) == 0&& DataManager.instance.MagicID[enemyData[e].magicId[4]].removeMp <= enemyData[e].mp)
                                                {
                                                    validityMagics = new List<int>();
                                                    for (int m = 0; m < enemyData[e].magicId.Length;)
                                                    {
                                                        if (enemyData[e].magicId[m] != -1 && DataManager.instance.MagicID[enemyData[e].magicId[m]].removeMp <= enemyData[e].mp) validityMagics.Add(m);
                                                        m++;
                                                    }
                                                    //攻撃を設定
                                                    if (DataManager.instance.MagicID[enemyData[e].magicId[randomMagic]].targetType == DataManager.TargetType.Player)
                                                        enemyData[e].targetType = DataManager.TargetType.Enemy;
                                                    else if (DataManager.instance.MagicID[enemyData[e].magicId[randomMagic]].targetType == DataManager.TargetType.Enemy)
                                                        enemyData[e].targetType = DataManager.TargetType.Player;
                                                    enemyData[e].selectCommandId = randomMagic;
                                                    //ターゲットを設定
                                                    //攻撃対象等の設定
                                                    List<int> vilidityTarget = new List<int>();
                                                    int randomTarget = -1;
                                                    switch (enemyData[e].targetType)
                                                    {
                                                        case DataManager.TargetType.Enemy://敵
                                                                                          //ランダムに決定
                                                            vilidityTarget = new List<int>();
                                                            for (int i = 0; i < enemyData.Length;)
                                                            {
                                                                if (enemyData[i].enemyId != -1 && !enemyData[i].isOver) vilidityTarget.Add(i);
                                                                i++;
                                                            }
                                                            randomTarget = vilidityTarget[(int)UnityEngine.Random.Range(0, vilidityTarget.Count)];
                                                            //ターゲット設定
                                                            enemyData[e].targetId = randomTarget;
                                                            if (enemyData[e].coolTime <= 0)
                                                            {
                                                                //スキル干渉(俊敏)
                                                                for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                                {
                                                                    if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                                    {
                                                                        enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                                    }
                                                                    else enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                                    i++;
                                                                }
                                                                allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                                                                allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                                                            }
                                                            else
                                                            {
                                                                //スキル干渉(俊敏)
                                                                for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                                {
                                                                    if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                                    {
                                                                        enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                                        allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                                        allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                                    }
                                                                    else
                                                                    {
                                                                        enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                                        allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                                        allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                                    }
                                                                    i++;
                                                                }
                                                            }

                                                            string tmptarget = "自分";
                                                            if (enemyData[e].name != enemyData[enemyData[e].targetId].name) tmptarget = enemyData[enemyData[e].targetId].name;
                                                            //翻訳
                                                            translator.Run(enemyData[e].name + "は" + tmptarget + "に対して" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].useWord + "しようとしている", results =>
                                                            {
                                                                foreach (var n in results)
                                                                {
                                                                    // ログに表示
                                                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                                                }
                                                            });
                                                            enemyData[e].isSelected = true;
                                                            break;
                                                        case DataManager.TargetType.Player://プレイヤー
                                                                                           //ランダムに決定
                                                            vilidityTarget = new List<int>();
                                                            for (int i = 0; i < playerData.Length;)
                                                            {
                                                                if (playerData[i].playerId != -1 && !playerData[i].isOver) vilidityTarget.Add(i);
                                                                i++;
                                                            }
                                                            randomTarget = vilidityTarget[(int)UnityEngine.Random.Range(0, vilidityTarget.Count)];
                                                            //ターゲット設定
                                                            enemyData[e].targetId = randomTarget;
                                                            if (enemyData[e].coolTime <= 0)
                                                            {
                                                                //スキル干渉(俊敏)
                                                                for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                                {
                                                                    if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                                    {
                                                                        enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                                    }
                                                                    else enemyData[e].coolTime = (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                                    i++;
                                                                }
                                                                allDialogUI.enemyState[e].cooltimeSlider.maxValue = enemyData[e].coolTime;
                                                                allDialogUI.enemyState[e].cooltimeSlider.value = allDialogUI.enemyState[e].cooltimeSlider.maxValue;
                                                            }
                                                            else
                                                            {
                                                                //スキル干渉(俊敏)
                                                                for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                                {
                                                                    if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 3)
                                                                    {
                                                                        enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                                        allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                                        allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4) / 4 * 3;
                                                                    }
                                                                    else
                                                                    {
                                                                        enemyData[e].coolTime += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                                        allDialogUI.enemyState[e].cooltimeSlider.maxValue += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                                        allDialogUI.enemyState[e].cooltimeSlider.value += (DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].starttime + 4);
                                                                    }
                                                                    i++;
                                                                }
                                                            }

                                                            //翻訳
                                                            translator.Run(enemyData[e].name + "は" + DataManager.instance.Pstatus[playerData[enemyData[e].targetId].playerId].pname + "に対して" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[e].magicId[enemyData[e].selectCommandId]].useWord + "しようとしている", results =>
                                                            {
                                                                foreach (var n in results)
                                                                {
                                                                    // ログに表示
                                                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                                                }
                                                            });
                                                            enemyData[e].isSelected = true;
                                                            break;
                                                    }
                                                    audioSource[1].PlayOneShot(se[1]);

                                                    enemyData[e].isCommand = true;
                                                    enemyData[e].isGuardUnUsed = false;
                                                    NoSelectedEnemy();
                                                }
                                                //スキル確認 (逃げ癖)
                                                if (!isAction)
                                                {
                                                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                    {
                                                        //逃げ癖かつHPが半分以下かつ四分の一の確率が当たったら逃走準備開始
                                                        if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 4 && enemyData[e].maxHP / 2 >= enemyData[e].hp && (int)UnityEngine.Random.Range(0, 3) == 0)
                                                        {
                                                            isEscape = true;
                                                            //敵用逃走コマンド呼び出す
                                                            EscapeCommandEnemy(e);
                                                            enemyData[e].isCommand = true;
                                                            enemyData[e].isGuardUnUsed = false;
                                                            NoSelectedEnemy();
                                                        }
                                                        i++;
                                                    }
                                                }
                                                if (!isEscape&&!isAction)
                                                {
                                                    //攻撃系コマンド呼び出す
                                                    AttackEnemyCommand(e, DataManager.instance.enemynoteID[enemyData[e].enemyId].aiType);
                                                    enemyData[e].isCommand = true;
                                                    enemyData[e].isGuardUnUsed = false;
                                                    NoSelectedEnemy();
                                                }
                                            }
                                            t++;
                                        }
                                        break;
                                    case DataManager.AiType.Optimization://最適化AI
                                                                         //相手のいずれかが自分を狙っているかチェック
                                        for (int t = 0; t < playerData.Length;)
                                        {
                                            //ガードを使っていいかつコマンド選択済みかつ攻撃コマンドを選択しているかつターゲットタイプが敵かつターゲットIDが自分ならかつクールタイムがないなら
                                            if (!enemyData[e].isSelected && !enemyData[e].isCommand&&!enemyData[e].isSelected&& enemyData[e].isEffect_Guarddamage <= 0 && !enemyData[e].isGuardUnUsed && playerData[t].isSelected && playerData[t].selectCommandType == CommandType.Attack && playerData[t].targetType == DataManager.TargetType.Enemy && playerData[t].targetId == e )
                                            {
                                                //狙われてるぞ！危ない
                                                //守る行動を選択
                                                GuardCommandEnemy(e);
                                                enemyData[e].isCommand = true;
                                                enemyData[e].isGuardUnUsed = true;
                                                NoSelectedEnemy();
                                            }
                                            else if (!enemyData[e].isSelected && !enemyData[e].isCommand&&enemyData[e].coolTime <= 0 )//あとは他最適化行動
                                            {
                                                bool isEscape = false;
                                                //スキル確認 (逃げ癖)
                                                for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill.Length;)
                                                {
                                                    //逃げ癖かつHPが三分の二以下かつ三分の一の確率が当たったら逃走準備開始
                                                    if (DataManager.instance.enemynoteID[enemyData[e].enemyId].inputskill[i] == 4 && enemyData[e].maxHP / 2 >= enemyData[e].hp)
                                                    {
                                                        isEscape = true;
                                                        //敵用逃走コマンド呼び出す
                                                        EscapeCommandEnemy(e);
                                                        enemyData[e].isCommand = true;
                                                        enemyData[e].isGuardUnUsed = false;
                                                        NoSelectedEnemy();
                                                    }
                                                   
                                                    i++;
                                                }
                                                if(!isEscape)
                                                {
                                                    //攻撃系コマンド呼び出す
                                                    AttackEnemyCommand(e, DataManager.instance.enemynoteID[enemyData[e].enemyId].aiType);
                                                    enemyData[e].isCommand = true;
                                                    enemyData[e].isGuardUnUsed = false;
                                                    NoSelectedEnemy();
                                                }
                                            }
                                            t++;
                                        }
                                        break;
                                }
                            }
                            else if (enemyData[e].isSelected && enemyData[e].coolTime <= 0)//コマンド発動
                            {
                                switch (enemyData[e].selectCommandType)
                                {
                                    case CommandType.Guard:
                                        if (enemyData[e].isSelected&& enemyData[e].isCommand) StartCoroutine(GuardEnemyInvoke(e));
                                        enemyData[e].isCommand = false;
                                        break;
                                    case CommandType.Escape:
                                        if (enemyData[e].isSelected&& enemyData[e].isCommand) StartCoroutine(EscapeEnemyInvoke(e));
                                        enemyData[e].isCommand = false;
                                        break;
                                    case CommandType.Attack:
                                        if (enemyData[e].isSelected&& enemyData[e].isCommand) StartCoroutine(AttackEnemyInvoke(e));
                                        enemyData[e].isCommand = false;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        e++;
                    }

                }
                if (updateTime >= 0) updateTime -= Time.deltaTime;
            }
        }
        private IEnumerator DamageSprite(SpriteRenderer sprite)
        {
            Color tmpcolor = sprite.color;
            tmpcolor.r = 1;
            tmpcolor.g = 0;
            tmpcolor.b = 0;
            sprite.color = tmpcolor;
            yield return new WaitForSeconds(0.2f);
            tmpcolor = sprite.color;
            tmpcolor.r = 1;
            tmpcolor.g = 1;
            tmpcolor.b = 1;
            sprite.color = tmpcolor;
            yield return new WaitForSeconds(0.1f);
            tmpcolor = sprite.color;
            tmpcolor.r = 1;
            tmpcolor.g = 0;
            tmpcolor.b = 0;
            sprite.color = tmpcolor;
            yield return new WaitForSeconds(0.2f);
            tmpcolor = sprite.color;
            tmpcolor.r = 1;
            tmpcolor.g = 1;
            tmpcolor.b = 1;
            sprite.color = tmpcolor;
        }
        private IEnumerator EscapeSprite(SpriteRenderer sprite)
        {
            Color tmpcolor = sprite.color;

            while (tmpcolor.a > 0)
            {
                tmpcolor.a -= Time.deltaTime;
                sprite.color = tmpcolor;
                yield return null;
            }
            sprite.gameObject.SetActive(false);
            tmpcolor = sprite.color;
            tmpcolor.a = 1;
            sprite.color = tmpcolor;
        }
        private IEnumerator fadeinSprite(Image sprite)
        {
            Color tmpcolor = sprite.color;
            tmpcolor.a = 0;
            sprite.color = tmpcolor;
            tmpcolor = sprite.color;
            while (tmpcolor.a < 1)
            {
                tmpcolor.a += Time.deltaTime;
                sprite.color = tmpcolor;
                yield return null;
            }
        }
        public IEnumerator AttackInvoke(int id = 0)
        {
            if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicSound != null) audioSource[1].PlayOneShot(DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicSound);
            int tmpanim = DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].charaAnimId;
            if (tmpanim > DataManager.instance.Pstatus[playerData[id].playerId].maxAnime) tmpanim = 3;
            StartCoroutine(EffectAnimation(playerData[id].charaAnim, tmpanim));
            
            //mpを減らす
            playerData[id].mp -= DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].removeMp;
            //ログ出力
            //翻訳
            if (playerData[id].targetType == DataManager.TargetType.Enemy)//敵対象の時
            {
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].isShake)
                {
                    iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                    iTween.ShakePosition(allDialogUI.enemyState[playerData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                }
                StartCoroutine(EffectAnimation(enemyData[playerData[id].targetId].effectAnim, DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].effectId));
                StartCoroutine(EffectAnimation(enemyData[playerData[id].targetId].charaAnim, 4));
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage < 0) StartCoroutine(DamageSprite(enemyData[playerData[id].targetId].spriteRen));
                translator.Run(DataManager.instance.Pstatus[playerData[id].playerId].pname + "は" + enemyData[playerData[id].targetId].name + "に対して" + DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].useWord + "した！", results =>
                 {
                     foreach (var n in results)
                     {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                     }
                 });
                yield return new WaitForSeconds(0.3f);

                //基本ダメージ計算処理
                float maindamage = (((DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage * -1) + playerData[id].attack + playerData[id].tmpadd_at + 1) / 2) - ((enemyData[playerData[id].targetId].defense + enemyData[playerData[id].targetId].tmpadd_df + 1) / 6);
                float lastdamage = maindamage * UnityEngine.Random.Range(0.85f, 1.15f);
                bool isVitalpoint = false;
                bool isBadType = false;
                //急所ダメージ計算処理
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].VitalpointAttack > 0)
                {
                    for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
                    {
                        //スキル干渉(急所突き)
                        if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] == 2 && (int)UnityEngine.Random.Range(0, (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].VitalpointAttack / 3 * 2)) == 0)
                        {
                            lastdamage *= 2;
                            isVitalpoint = true;
                        }
                        //スキル持ちじゃない場合
                        else if ((int)UnityEngine.Random.Range(0, DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].VitalpointAttack) == 0)
                        {
                            lastdamage *= 2;
                            isVitalpoint = true;
                        }
                        i++;
                    }
                }
                //弱点タイプによるダメージ計算処理
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage != 0)
                {
                    if (((DataManager.instance.Pstatus[playerData[id].playerId].attacktype == DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype || DataManager.instance.Pstatus[playerData[id].playerId].attacktype2 == DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype) && DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype != DataManager.MagicType.None) || ((DataManager.instance.Pstatus[playerData[id].playerId].attacktype == DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype2 || DataManager.instance.Pstatus[playerData[id].playerId].attacktype2 == DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype2) && DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype2 != DataManager.MagicType.None))
                    {
                        lastdamage *= 3 / 2;
                        isBadType = true;
                    }
                    if ((DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicType == DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype && DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype != DataManager.MagicType.None) || (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicType == DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype2 && DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype2 != DataManager.MagicType.None))
                    {
                        lastdamage *= 2;
                        isBadType = true;
                    }
                }
                //主人公力スキル干渉によるダメージ計算処理
                //攻撃側処理
                for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
                {
                    //スキル干渉(主人公力)
                    if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] !=-1&& DataManager.instance.skillData[DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)playerData[id].maxHP / 100 * 50 >= (int)playerData[id].hp)
                    {
                        lastdamage *= 15 / 12;
                    }
                    i++;
                }
                //守る側処理
                for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].inputskill.Length;)
                {
                    //スキル干渉(主人公力)
                    if (DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].inputskill[i] !=-1&& DataManager.instance.skillData[DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)enemyData[playerData[id].targetId].maxHP / 100 * 50 >= (int)enemyData[playerData[id].targetId].hp)
                    {
                        lastdamage /= 2;
                    }
                    i++;
                }
                //守られている場合のダメージ計算処理
                if (enemyData[playerData[id].targetId].isGuard)
                {
                    lastdamage /= 3;
                }
                //ダメージが1を下回っている場合は1に
                if ((int)lastdamage < 1) lastdamage = 1;
                //攻撃魔法ではない場合
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage >= 0) lastdamage = DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage;
                //攻撃魔法なら
                else lastdamage *= -1;
                //ダメージ反映処理
                float oldhp = enemyData[playerData[id].targetId].hp;
                enemyData[playerData[id].targetId].hp += (int)lastdamage;
                //エフェクト処理

                //ダメージのログ表示(会心の一撃含む)
                if (isVitalpoint)
                {
                    translator.Run("会心の一撃！", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                }
                if (isBadType)
                {
                    translator.Run("弱点を突いている！", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                }
                float tmptextdamage = lastdamage;
                if (tmptextdamage < 0) tmptextdamage *= -1;
                string lasttmptextdamage = ((int)tmptextdamage).ToString();
                if (tmptextdamage == 0) lasttmptextdamage = "";
                translator.Run(enemyData[playerData[id].targetId].name + "は" + lasttmptextdamage + DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].damageWord + "を受けた！", results =>
               {
                   foreach (var n in results)
                   {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                   }
               });
                yield return new WaitForSeconds(0.5f);
                //HPを0にして倒せた場合
                if (enemyData[playerData[id].targetId].hp <= 0)
                {
                    enemyData[playerData[id].targetId].isOver = true;
                    //図鑑取得
                    var enemy = DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId];
                    enemy.gettrg = 1;
                    DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId] = enemy;
                    //Triggerの変更
                    if (DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].isTrg != -1) DataManager.instance.Triggers[DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].isTrg] = 1;
                    if (DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].isTargetEvent != -1) DataManager.instance.Triggers[DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].isTargetEvent] += DataManager.instance.Triggers[DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].isAddEvent];
                    if (DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].getAchievements != -1 && DataManager.instance.achievementsID[DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].getAchievements].gettrg < 1)
                    {
                        DataManager.instance.TextGet = "実績：" + DataManager.instance.achievementsID[DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].getAchievements].name;
                        var achi = DataManager.instance.achievementsID[DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].getAchievements];
                        achi.gettrg = 1;
                        DataManager.instance.achievementsID[DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].getAchievements] = achi;
                    }
                    allDialogUI.enemyState[playerData[id].targetId].isOverUI.gameObject.SetActive(true);
                    enemyData[playerData[id].targetId].charaAnim.runtimeAnimatorController = graveAnimator;
                    StartCoroutine(DamageSprite(enemyData[playerData[id].targetId].spriteRen));
                    if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].isShake)
                    {
                        iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                        iTween.ShakePosition(allDialogUI.enemyState[playerData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                        iTween.ShakePosition(enemyData[playerData[id].targetId].spriteRen.gameObject, iTween.Hash("x", shakePower / 32, "y", shakePower / 32, "time", 0.5f));
                    }
                    audioSource[1].PlayOneShot(se[5]);
                    translator.Run(enemyData[playerData[id].targetId].name + "は力尽きてしまった。。", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return new WaitForSeconds(0.3f);
                    bool allover = true;
                    for (int e = 0; e < enemyData.Length;)
                    {
                        if (enemyData[e].enemyId != -1 && !enemyData[e].isOver)
                        {
                            allover = false;
                        }
                        e++;
                    }
                    //全滅処理
                    if (allover)
                    {
                        isEnd = true;
                        StartCoroutine(GameOverEvent(DataManager.TargetType.Enemy));
                    }
                }


                if (!enemyData[playerData[id].targetId].isOver)
                {
                    //魔法効果適用
                    switch (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicEvent)
                    {
                        case DataManager.MagicEvent.None://HP回復
                            break;
                        default:
                            break;
                    }

                    //スキル干渉 ダメージ受けて主人公力が丁度解禁した場合
                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].inputskill.Length;)
                    {
                        //スキル干渉(主人公力)
                        if (DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].inputskill[i] !=-1&& DataManager.instance.skillData[DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)enemyData[playerData[id].targetId].maxHP / 100 * 50 >= (int)enemyData[playerData[id].targetId].hp && (float)enemyData[playerData[id].targetId].maxHP / 100 * 50 < oldhp)
                        {
                            translator.Run(enemyData[playerData[id].targetId].name + "はまだ、勝負を諦めていない…！", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                            translator.Run(enemyData[playerData[id].targetId].name + "の攻守が根性で上がった", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        i++;
                    }
                    //ボスかどうかチェックし、HPを見てイベントを挟むかどうか判断
                    if (DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].isBoss && (float)enemyData[playerData[id].targetId].maxHP / 100 * 50 >= (int)enemyData[playerData[id].targetId].hp && (float)enemyData[playerData[id].targetId].maxHP / 100 * 50 < oldhp && enemyData[playerData[id].targetId].hp > 0)
                    {
                        DataManager.instance.Triggers[2] = 1;
                        bossEnemydataIndex = playerData[id].targetId;
                        dialogueSystem.DialogueUI.SetInteger("trg", 1);
                        dialogueSystem.eventId = DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].bossSayEvent;
                        yield return null;
                        StartCoroutine(dialogueSystem.ShowNextComment());
                        enemyData[playerData[id].targetId].magicId[enemyData[playerData[id].targetId].magicId.Length - 1] = DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].bossLastMagicId;
                        enemyData[playerData[id].targetId].mp = enemyData[playerData[id].targetId].maxMP * 2;
                    }
                }
            }
            else if (playerData[id].targetType == DataManager.TargetType.Player)//味方対象の時
            {
                string tmptarget = "自分";
                StartCoroutine(EffectAnimation(playerData[playerData[id].targetId].effectAnim, DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].effectId));
                StartCoroutine(EffectAnimation(playerData[playerData[id].targetId].charaAnim, 4));
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage < 0) StartCoroutine(DamageSprite(enemyData[playerData[id].targetId].spriteRen));
                if (DataManager.instance.Pstatus[playerData[id].playerId].pname != DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname) tmptarget = DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname;
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].isShake)
                {
                    iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                    iTween.ShakePosition(allDialogUI.playerState[playerData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                }
                translator.Run(DataManager.instance.Pstatus[playerData[id].playerId].pname + "は" + tmptarget + "に対して" + DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].useWord + "した！", results =>
                {
                    foreach (var n in results)
                    {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                    }
                });
                yield return new WaitForSeconds(0.3f);
                //基本ダメージ計算処理
                float maindamage = (((DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage * -1) + playerData[id].attack + playerData[id].tmpadd_at + 1) / 2) - ((playerData[playerData[id].targetId].defense + playerData[playerData[id].targetId].tmpadd_df + 1) / 6);
                float lastdamage = maindamage * UnityEngine.Random.Range(0.85f, 1.15f);
                bool isVitalpoint = false;
                bool isBadType = false;
                //急所ダメージ計算処理
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].VitalpointAttack > 0)
                {
                    for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
                    {
                        //スキル干渉(急所突き)
                        if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] == 2 && (int)UnityEngine.Random.Range(0, (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].VitalpointAttack / 3 * 2)) == 0)
                        {
                            lastdamage *= 3 / 2;
                            isVitalpoint = true;
                        }
                        //スキル持ちじゃない場合
                        else if ((int)UnityEngine.Random.Range(0, DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].VitalpointAttack) == 0)
                        {
                            lastdamage *= 3 / 2;
                            isVitalpoint = true;
                        }
                        i++;
                    }
                }
                //弱点タイプによるダメージ計算処理
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage != 0)
                {
                    if (((DataManager.instance.Pstatus[playerData[id].playerId].attacktype == DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype || DataManager.instance.Pstatus[playerData[id].playerId].attacktype2 == DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype) && DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype != DataManager.MagicType.None) || ((DataManager.instance.Pstatus[playerData[id].playerId].attacktype == DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype2 || DataManager.instance.Pstatus[playerData[id].playerId].attacktype2 == DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype2) && DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype2 != DataManager.MagicType.None))
                    {
                        lastdamage *= 3 / 2;
                        isBadType = true;
                    }
                    if ((DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicType == DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype && DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype != DataManager.MagicType.None) || (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicType == DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype2 && DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].badtype2 != DataManager.MagicType.None))
                    {
                        lastdamage *= 2;
                        isBadType = true;
                    }
                }
                //主人公力スキル干渉によるダメージ計算処理
                //攻撃側処理
                for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
                {
                    //スキル干渉(主人公力)
                    if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] !=-1&& DataManager.instance.skillData[DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)playerData[id].maxHP / 100 * 50 >= (int)playerData[id].hp)
                    {
                        lastdamage *= 15 / 12;
                    }
                    i++;
                }
                //守る側処理
                for (int i = 0; i < DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].inputskill.Length;)
                {
                    //スキル干渉(主人公力)
                    if (DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].inputskill[i]!=-1&&DataManager.instance.skillData[DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)playerData[playerData[id].targetId].maxHP / 100 * 50 >= (int)playerData[playerData[id].targetId].hp)
                    {
                        lastdamage /= 2;
                    }
                    i++;
                }
                //守られている場合のダメージ計算処理
                if (playerData[playerData[id].targetId].isGuard)
                {
                    lastdamage /= 3;
                }
                //ダメージが1を下回っている場合は1に
                if ((int)lastdamage < 1) lastdamage = 1;
                //攻撃魔法ではない場合
                if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage >= 0) lastdamage = DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].Damage;
                //攻撃魔法なら
                else lastdamage *= -1;
                //ダメージ反映処理
                float oldhp = playerData[playerData[id].targetId].hp;
                playerData[playerData[id].targetId].hp += (int)lastdamage;
                //エフェクト処理
                //ダメージのログ表示(会心の一撃含む)
                if (isVitalpoint)
                {
                    translator.Run("会心の一撃！", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                }
                if (isBadType)
                {
                    translator.Run("弱点を突いている！", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                }
                float tmptextdamage = lastdamage;
                if (tmptextdamage < 0) tmptextdamage *= -1;
                string lasttmptextdamage = tmptextdamage.ToString();
                if (tmptextdamage == 0) lasttmptextdamage = "";
                translator.Run(DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname + "は" + lasttmptextdamage + DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].damageWord + "を受けた！", results =>
                {
                    foreach (var n in results)
                    {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                    }
                });
                yield return new WaitForSeconds(0.5f);
                //HPを0にして倒せた場合
                if (playerData[playerData[id].targetId].hp <= 0)
                {
                    playerData[playerData[id].targetId].isOver = true;
                    allDialogUI.playerState[playerData[id].targetId].isOverUI.gameObject.SetActive(true);
                    audioSource[1].PlayOneShot(se[5]);
                    playerData[playerData[id].targetId].charaAnim.runtimeAnimatorController = graveAnimator;
                    StartCoroutine(DamageSprite(playerData[playerData[id].targetId].spriteRen));
                    if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].isShake)
                    {
                        iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                        iTween.ShakePosition(allDialogUI.playerState[playerData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                        iTween.ShakePosition(playerData[playerData[id].targetId].spriteRen.gameObject, iTween.Hash("x", shakePower / 32, "y", shakePower / 32, "time", 0.5f));
                    }
                    translator.Run(DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname + "は力尽きてしまった。。", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return new WaitForSeconds(0.3f);

                    bool allover = true;
                    for (int p = 0; p < playerData.Length;)
                    {
                        if (playerData[p].playerId != -1 && !playerData[p].isOver)
                        {
                            allover = false;
                        }
                        p++;
                    }
                    //全滅処理
                    if (allover)
                    {
                        isEnd = true;
                        StartCoroutine(GameOverEvent(DataManager.TargetType.Player));
                    }
                }
                if (!playerData[playerData[id].targetId].isOver)
                {
                    //魔法効果適用
                    switch (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicEvent)
                    {
                        case DataManager.MagicEvent.None://HP回復
                            break;
                        default:
                            break;
                    }

                    //スキル干渉 ダメージ受けて主人公力が丁度解禁した場合
                    for (int i = 0; i < DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].inputskill.Length;)
                    {
                        //スキル干渉(主人公力)
                        if (DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].inputskill[i]!=-1&&DataManager.instance.skillData[DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)playerData[playerData[id].targetId].maxHP / 100 * 50 >= (int)playerData[playerData[id].targetId].hp && (float)playerData[playerData[id].targetId].maxHP / 100 * 50 < oldhp)
                        {
                            translator.Run(DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname + "はまだ、勝負を諦めていない…！", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                            translator.Run(DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname + "の攻守が根性で上がった", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }
                        i++;
                    }
                }
            }
            yield return StartCoroutine(AttackEffect(id, DataManager.TargetType.Player));
            //クールタイム
            //スキル干渉(俊敏)
            for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
            {
                if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] == 3)
                {
                    playerData[id].coolTime = DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].cooltime / 4 * 3;
                }
                else playerData[id].coolTime = DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].cooltime;
                i++;
            }
            allDialogUI.playerState[id].cooltimeSlider.maxValue = playerData[id].coolTime;
            allDialogUI.playerState[id].cooltimeSlider.value = allDialogUI.playerState[id].cooltimeSlider.maxValue;
            ViewReset(id);
        }

        public IEnumerator AttackEnemyInvoke(int id = 0)
        {
            if (enemyData[id].isCommand)
            {
                if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicSound != null) audioSource[1].PlayOneShot(DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicSound);

                int tmpanim = DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].charaAnimId;
                if (tmpanim > DataManager.instance.enemynoteID[enemyData[id].enemyId].maxAnim) tmpanim = 3;
                StartCoroutine(EffectAnimation(enemyData[id].charaAnim, tmpanim));
                //mpを減らす
                enemyData[id].mp -= DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].removeMp;

               

                //ログ出力
                //翻訳
                if (enemyData[id].targetType == DataManager.TargetType.Enemy)//敵対象の時
                {
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].isShake)
                    {
                        iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                        iTween.ShakePosition(allDialogUI.enemyState[enemyData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                    }
                    StartCoroutine(EffectAnimation(enemyData[enemyData[id].targetId].effectAnim, DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].effectId));
                    StartCoroutine(EffectAnimation(enemyData[enemyData[id].targetId].charaAnim, 4));
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage < 0) StartCoroutine(DamageSprite(enemyData[enemyData[id].targetId].spriteRen));
                    string tmptarget = "自分";
                    if (enemyData[id].name != enemyData[enemyData[id].targetId].name) tmptarget = enemyData[enemyData[id].targetId].name;
                    translator.Run(enemyData[id].name + "は" + tmptarget + "に対して" + DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].useWord + "した！", results =>
                {
                    foreach (var n in results)
                    {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                    }
                });
                    //ボス技の影響反映
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicEvent == DataManager.MagicEvent.BoneScreen)
                    {
                        yield return new WaitForSeconds(0.3f);
                        for (int u = 0; u < allDialogUI.bossUIObject[0].uiObject.Length;)
                        {
                            allDialogUI.bossUIObject[0].uiObject[u].SetActive(true);
                            StartCoroutine(fadeinSprite(allDialogUI.bossUIObject[0].uiObject[u].GetComponent<Image>()));
                            yield return new WaitForSeconds(0.1f);
                            u++;
                        }
                    }
                    yield return new WaitForSeconds(0.3f);

                    //基本ダメージ計算処理
                    float maindamage = (((DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage * -1) + enemyData[id].attack + enemyData[id].tmpadd_at + 1) / 2) - ((enemyData[enemyData[id].targetId].defense + enemyData[enemyData[id].targetId].tmpadd_df + 1) / 6);
                    float lastdamage = maindamage * UnityEngine.Random.Range(0.85f, 1.15f);
                    bool isVitalpoint = false;
                    bool isBadType = false;
                    //急所ダメージ計算処理
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].VitalpointAttack > 0)
                    {
                        for (int i = 0; i < DataManager.instance.Pstatus[enemyData[id].enemyId].inputskill.Length;)
                        {
                            //スキル干渉(急所突き)
                            if (DataManager.instance.Pstatus[enemyData[id].enemyId].inputskill[i] == 2 && (int)UnityEngine.Random.Range(0, (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].VitalpointAttack / 3 * 2)) == 0)
                            {
                                lastdamage *= 2;
                                isVitalpoint = true;
                            }
                            //スキル持ちじゃない場合
                            else if ((int)UnityEngine.Random.Range(0, DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].VitalpointAttack) == 0)
                            {
                                lastdamage *= 2;
                                isVitalpoint = true;
                            }
                            i++;
                        }
                    }
                    //弱点タイプによるダメージ計算処理
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage != 0)
                    {
                        if (((DataManager.instance.enemynoteID[enemyData[id].enemyId].attacktype == DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype || DataManager.instance.enemynoteID[enemyData[id].enemyId].attacktype2 == DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype) && DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype != DataManager.MagicType.None) || ((DataManager.instance.enemynoteID[enemyData[id].enemyId].attacktype == DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype2 || DataManager.instance.enemynoteID[enemyData[id].enemyId].attacktype2 == DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype2) && DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype2 != DataManager.MagicType.None))
                        {
                            lastdamage *= 3 / 2;
                            isBadType = true;
                        }
                        if ((DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicType == DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype && DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype != DataManager.MagicType.None) || (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicType == DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype2 && DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].badtype2 != DataManager.MagicType.None))
                        {
                            lastdamage *= 2;
                            isBadType = true;
                        }
                    }
                    //主人公力スキル干渉によるダメージ計算処理
                    //攻撃側処理
                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill.Length;)
                    {
                        //スキル干渉(主人公力)
                        if (DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].inputskill[i]!=-1&&DataManager.instance.skillData[DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)enemyData[enemyData[id].targetId].maxHP / 100 * 50 >= (int)enemyData[enemyData[id].targetId].hp)
                        {
                            lastdamage *= 15 / 12;
                        }
                        i++;
                    }
                    //守る側処理
                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].inputskill.Length;)
                    {
                        //スキル干渉(主人公力)
                        if (DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].inputskill[i]!=-1&&DataManager.instance.skillData[DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower&& (float)enemyData[enemyData[id].targetId].maxHP / 100 * 50 >= (int)enemyData[enemyData[id].targetId].hp)
                        {
                            lastdamage /= 2;
                        }
                        i++;
                    }
                    //守られている場合のダメージ計算処理
                    if (enemyData[enemyData[id].targetId].isGuard)
                    {
                        lastdamage /= 3;
                    }
                    //ダメージが1を下回っている場合は1に
                    if ((int)lastdamage < 1) lastdamage = 1;
                    //攻撃魔法ではない場合
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage >= 0) lastdamage = DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage;
                    //攻撃魔法なら
                    else lastdamage *= -1;
                    //ダメージ反映処理
                    float oldhp = enemyData[enemyData[id].targetId].hp;
                    enemyData[enemyData[id].targetId].hp += (int)lastdamage;
                    //エフェクト処理

                    //ダメージのログ表示(会心の一撃含む)
                    if (isVitalpoint)
                    {
                        translator.Run("会心の一撃！", results =>
                        {
                            foreach (var n in results)
                            {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                    }
                    if (isBadType)
                    {
                        translator.Run("弱点を突いている！", results =>
                        {
                            foreach (var n in results)
                            {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                    }
                    float tmptextdamage = lastdamage;
                    if (tmptextdamage < 0) tmptextdamage *= -1;
                    int tmpint = (int)tmptextdamage;
                    string lasttmptextdamage = tmpint.ToString();
                    if (tmptextdamage == 0) lasttmptextdamage = "";
                    translator.Run(enemyData[enemyData[id].targetId].name + "は" + lasttmptextdamage + DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].damageWord + "を受けた！", results =>
                    {
                        foreach (var n in results)
                        {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return new WaitForSeconds(0.5f);
                    //HPを0にして倒せた場合
                    if (enemyData[enemyData[id].targetId].hp <= 0)
                    {
                        enemyData[enemyData[id].targetId].isOver = true;
                        //図鑑取得
                        var enemy = DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId];
                        enemy.gettrg = 1;
                        DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId] = enemy;

                        allDialogUI.enemyState[enemyData[id].targetId].isOverUI.gameObject.SetActive(true);
                        enemyData[enemyData[id].targetId].charaAnim.runtimeAnimatorController = graveAnimator;
                        StartCoroutine(DamageSprite(enemyData[enemyData[id].targetId].spriteRen));
                        if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].isShake)
                        {
                            iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                            iTween.ShakePosition(allDialogUI.enemyState[enemyData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                            iTween.ShakePosition(enemyData[enemyData[id].targetId].spriteRen.gameObject, iTween.Hash("x", shakePower / 32, "y", shakePower / 32, "time", 0.5f));
                        }
                        audioSource[1].PlayOneShot(se[5]);
                        translator.Run(enemyData[enemyData[id].targetId].name + "は力尽きてしまった。。", results =>
                        {
                            foreach (var n in results)
                            {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                        yield return new WaitForSeconds(0.3f);
                        bool allover = true;
                        for (int e = 0; e < enemyData.Length;)
                        {
                            if (enemyData[e].enemyId != -1 && !enemyData[e].isOver)
                            {
                                allover = false;
                            }
                            e++;
                        }
                        //全滅処理
                        if (allover)
                        {
                            isEnd = true;
                            StartCoroutine(GameOverEvent(DataManager.TargetType.Enemy));
                        }
                    }


                    if (!enemyData[enemyData[id].targetId].isOver)
                    {
                        //魔法効果適用
                        switch (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicEvent)
                        {
                            case DataManager.MagicEvent.None://HP回復
                                break;
                            default:
                                break;
                        }

                        //スキル干渉 ダメージ受けて主人公力が丁度解禁した場合
                        for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].inputskill.Length;)
                        {
                            //スキル干渉(主人公力)
                            if (DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].inputskill[i]!=-1&&DataManager.instance.skillData[DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)enemyData[enemyData[id].targetId].maxHP / 100 * 50 >= (int)enemyData[enemyData[id].targetId].hp && (float)enemyData[enemyData[id].targetId].maxHP / 100 * 50 < oldhp)
                            {
                                translator.Run(enemyData[enemyData[id].targetId].name + "はまだ、勝負を諦めていない…！", results =>
                                {
                                    foreach (var n in results)
                                    {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                    }
                                });
                                yield return new WaitForSeconds(0.3f);
                                translator.Run(enemyData[enemyData[id].targetId].name + "の攻守が根性で上がった", results =>
                                {
                                    foreach (var n in results)
                                    {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                    }
                                });
                                yield return new WaitForSeconds(0.3f);
                            }
                            i++;
                        }
                    }
                }
                else if (enemyData[id].targetType == DataManager.TargetType.Player)//味方対象の時
                {
                    StartCoroutine(EffectAnimation(playerData[enemyData[id].targetId].effectAnim, DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].effectId));
                    StartCoroutine(EffectAnimation(playerData[enemyData[id].targetId].charaAnim, 4));
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage < 0) StartCoroutine(DamageSprite(playerData[enemyData[id].targetId].spriteRen));
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].isShake)
                    {
                        iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                        iTween.ShakePosition(allDialogUI.playerState[enemyData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                    }
                    if (playerData[enemyData[id].targetId].playerId != -1)
                        translator.Run(enemyData[id].name + "は" + DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].pname + "に対して" + DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicname + "で" + DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].useWord + "した！", results =>
                        {
                            foreach (var n in results)
                            {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                    //ボス技の影響反映
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicEvent == DataManager.MagicEvent.BoneScreen)
                    {
                        yield return new WaitForSeconds(0.3f);
                        for (int u = 0; u < allDialogUI.bossUIObject[0].uiObject.Length;)
                        {
                            allDialogUI.bossUIObject[0].uiObject[u].SetActive(true);
                            StartCoroutine(fadeinSprite(allDialogUI.bossUIObject[0].uiObject[u].GetComponent<Image>()));
                            yield return new WaitForSeconds(0.1f);
                            u++;
                        }
                    }
                    yield return new WaitForSeconds(0.3f);
                    //基本ダメージ計算処理
                    float maindamage = (((DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage * -1) + enemyData[id].attack + enemyData[id].tmpadd_at + 1) / 2) - ((enemyData[enemyData[id].targetId].defense + enemyData[enemyData[id].targetId].tmpadd_df + 1) / 6);
                    float lastdamage = maindamage * UnityEngine.Random.Range(0.85f, 1.15f);
                    bool isVitalpoint = false;
                    bool isBadType = false;
                    //急所ダメージ計算処理
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].VitalpointAttack > 0)
                    {
                        for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill.Length;)
                        {
                            //スキル干渉(急所突き)
                            if (DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill[i] == 2 && (int)UnityEngine.Random.Range(0, (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].VitalpointAttack / 3 * 2)) == 0)
                            {
                                lastdamage *= 3 / 2;
                                isVitalpoint = true;
                            }
                            //スキル持ちじゃない場合
                            else if ((int)UnityEngine.Random.Range(0, DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].VitalpointAttack) == 0)
                            {
                                lastdamage *= 3 / 2;
                                isVitalpoint = true;
                            }
                            i++;
                        }
                    }
                    //弱点タイプによるダメージ計算処理
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage != 0 && playerData[enemyData[id].targetId].playerId != -1)
                    {
                        if (((DataManager.instance.enemynoteID[enemyData[id].enemyId].attacktype == DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype || DataManager.instance.enemynoteID[enemyData[id].enemyId].attacktype2 == DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype) && DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype != DataManager.MagicType.None) || ((DataManager.instance.enemynoteID[enemyData[id].enemyId].attacktype == DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype2 || DataManager.instance.enemynoteID[enemyData[id].enemyId].attacktype2 == DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype2) && DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype2 != DataManager.MagicType.None))
                        {
                            lastdamage *= 3 / 2;
                            isBadType = true;
                        }
                        if ((DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicType == DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype && DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype != DataManager.MagicType.None) || (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicType == DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype2 && DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].badtype2 != DataManager.MagicType.None))
                        {
                            lastdamage *= 2;
                            isBadType = true;
                        }
                    }
                    //主人公力スキル干渉によるダメージ計算処理
                    //攻撃側処理
                    for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill.Length;)
                    {
                        //スキル干渉(主人公力)
                        if (DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill[i]!=-1&&DataManager.instance.skillData[DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)enemyData[id].maxHP / 100 * 50 >= (int)enemyData[id].hp)
                        {
                            lastdamage *= 15 / 12;
                        }
                        i++;
                    }
                    //守る側処理
                    if (playerData[enemyData[id].targetId].playerId != -1)
                        for (int i = 0; i < DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].inputskill.Length;)
                        {
                                //スキル干渉(主人公力)
                                if (playerData[enemyData[id].targetId].playerId != -1&& DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].inputskill[i]!=-1 && DataManager.instance.skillData[DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)playerData[enemyData[id].targetId].maxHP / 100 * 50 >= (int)playerData[enemyData[id].targetId].hp)
                                {
                                    lastdamage /= 2;
                                }
                            i++;
                        }
                    //守られている場合のダメージ計算処理
                    if (playerData[enemyData[id].targetId].isGuard)
                    {
                        lastdamage /= 3;
                    }
                    //ダメージが1を下回っている場合は1に
                    if ((int)lastdamage < 1) lastdamage = 1;
                    //攻撃魔法ではない場合
                    if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage >= 0) lastdamage = DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].Damage;
                    //攻撃魔法なら
                    else lastdamage *= -1;
                    //ダメージ反映処理
                    float oldhp = playerData[enemyData[id].targetId].hp;
                    playerData[enemyData[id].targetId].hp += (int)lastdamage;
                    //エフェクト処理
                    //ダメージのログ表示(会心の一撃含む)
                    if (isVitalpoint)
                    {
                        translator.Run("会心の一撃！", results =>
                        {
                            foreach (var n in results)
                            {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                    }
                    if (isBadType)
                    {
                        translator.Run("弱点を突いている！", results =>
                        {
                            foreach (var n in results)
                            {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                    }
                    float tmptextdamage = lastdamage;
                    if (tmptextdamage < 0) tmptextdamage *= -1;
                    int tmpint = (int)tmptextdamage;
                    string lasttmptextdamage = tmpint.ToString();
                    if (tmptextdamage == 0) lasttmptextdamage = "";
                    if (playerData[enemyData[id].targetId].playerId != -1)
                        translator.Run(DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].pname + "は" + lasttmptextdamage + DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].damageWord + "を受けた！", results =>
                    {
                        foreach (var n in results)
                        {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return new WaitForSeconds(0.5f);
                    //HPを0にして倒せた場合
                    if (playerData[enemyData[id].targetId].hp <= 0)
                    {
                        playerData[enemyData[id].targetId].isOver = true;
                        allDialogUI.playerState[enemyData[id].targetId].isOverUI.gameObject.SetActive(true);
                        audioSource[1].PlayOneShot(se[5]);
                        playerData[enemyData[id].targetId].charaAnim.runtimeAnimatorController = graveAnimator;
                        StartCoroutine(DamageSprite(playerData[enemyData[id].targetId].spriteRen));
                        if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].isShake)
                        {
                            iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                            iTween.ShakePosition(allDialogUI.playerState[enemyData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                            iTween.ShakePosition(playerData[enemyData[id].targetId].spriteRen.gameObject, iTween.Hash("x", shakePower / 32, "y", shakePower / 32, "time", 0.5f));
                        }
                        if (playerData[enemyData[id].targetId].playerId != -1)
                            translator.Run(DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].pname + "は力尽きてしまった。。", results =>
                        {
                            foreach (var n in results)
                            {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                        yield return new WaitForSeconds(0.3f);

                        bool allover = true;
                        for (int p = 0; p < playerData.Length;)
                        {
                            if (playerData[p].playerId != -1 && !playerData[p].isOver)
                            {
                                allover = false;
                            }
                            p++;
                        }
                        //全滅処理
                        if (allover)
                        {
                            isEnd = true;
                            StartCoroutine(GameOverEvent(DataManager.TargetType.Player));
                        }
                    }
                    if (!playerData[enemyData[id].targetId].isOver)
                    {
                        //魔法効果適用
                        switch (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicEvent)
                        {
                            case DataManager.MagicEvent.None://HP回復
                                break;
                            default:
                                break;
                        }

                        //スキル干渉 ダメージ受けて主人公力が丁度解禁した場合
                        if (playerData[enemyData[id].targetId].playerId != -1)
                        {
                            for (int i = 0; i < DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].inputskill.Length;)
                            {
                                //スキル干渉(主人公力)
                                if (DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].inputskill[i]!=-1&&DataManager.instance.skillData[DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].inputskill[i]].skillEvent == DataManager.SkillEvent.LastPower && (float)playerData[enemyData[id].targetId].maxHP / 100 * 50 >= (int)playerData[enemyData[id].targetId].hp && (float)playerData[enemyData[id].targetId].maxHP / 100 * 50 < oldhp)
                                {
                                    translator.Run(DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].pname + "はまだ、勝負を諦めていない…！", results =>
                                    {
                                        foreach (var n in results)
                                        {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                        }
                                    });
                                    yield return new WaitForSeconds(0.3f);
                                    translator.Run(DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].pname + "の攻守が根性で上がった", results =>
                                    {
                                        foreach (var n in results)
                                        {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                        }
                                    });
                                    yield return new WaitForSeconds(0.3f);
                                }
                                i++;
                            }
                        }
                    }
                }
                yield return StartCoroutine(AttackEffect(id, DataManager.TargetType.Enemy));
                //クールタイム
                //スキル干渉(俊敏)
                for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill.Length;)
                {
                    if (DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill[i] == 3)
                    {
                        enemyData[id].coolTime = (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].cooltime + 6) / 4 * 3;
                    }
                    else enemyData[id].coolTime = (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].cooltime + 6);
                    i++;
                }
                allDialogUI.enemyState[id].cooltimeSlider.maxValue = enemyData[id].coolTime;
                allDialogUI.enemyState[id].cooltimeSlider.value = allDialogUI.enemyState[id].cooltimeSlider.maxValue;
                enemyData[id].selectCommandType = CommandType.None;
                enemyData[id].selectCommandId = -1;
                enemyData[id].targetId = -1;
                ViewResetEnemy(id);
            }
            enemyData[id].isSelected = false;
            
        }
        
        public IEnumerator AttackEffect(int id,DataManager.TargetType targetType)//今後攻撃後の効果等は個々で処理する
        {
            if (targetType == DataManager.TargetType.Enemy)
            {
                //状態付与効果やイベント処理について
                if (enemyData[id].targetType == DataManager.TargetType.Enemy)
                {
                    for (int a = 0; a < enemyData[enemyData[id].targetId].stateAnomaly.Length;)
                    {
                        //防御崩壊付与処理
                        if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].addAnomaly == 1 && (int)UnityEngine.Random.Range(0, 4) == 0 && !DataManager.instance.enemynoteID[enemyData[enemyData[id].targetId].enemyId].isDFDamageResistance && enemyData[enemyData[id].targetId].isEffect_Guarddamage <= 0 && enemyData[enemyData[id].targetId].stateAnomaly[a] == -1)
                        {
                            enemyData[enemyData[id].targetId].stateAnomaly[a] = 1;
                            enemyData[enemyData[id].targetId].isEffect_Guarddamage = 2;
                            audioSource[1].PlayOneShot(DataManager.instance.anomalyID[enemyData[enemyData[id].targetId].stateAnomaly[a]].se);
                            translator.Run(enemyData[enemyData[id].targetId].name + "は防御崩し状態になった！", results =>
                            {
                                foreach (var n in results)
                                {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                            translator.Run("防御崩し状態の間は守る行動ができない…！", results =>
                            {
                                foreach (var n in results)
                                {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }

                        //防御崩壊解除処理
                        if (enemyData[enemyData[id].targetId].stateAnomaly[a] == 1)//防御崩壊
                        {
                            enemyData[enemyData[id].targetId].isEffect_Guarddamage -= 1;
                            if (enemyData[enemyData[id].targetId].isEffect_Guarddamage <= 0)
                            {
                                enemyData[enemyData[id].targetId].stateAnomaly[a] = -1;
                                translator.Run(enemyData[enemyData[id].targetId].name + "の防御崩し状態が治った！", results =>
                                {
                                    foreach (var n in results)
                                    {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                    }
                                });
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        a++;
                    }
                }
                else
                {
                    for (int a = 0; a < playerData[enemyData[id].targetId].stateAnomaly.Length;)
                    {
                        //防御崩壊付与処理
                        if (DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].addAnomaly == 1 && (int)UnityEngine.Random.Range(0, 4) == 0 && !DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].isDFDamageResistance && playerData[enemyData[id].targetId].isEffect_Guarddamage <= 0 && playerData[enemyData[id].targetId].stateAnomaly[a] == -1)
                        {
                            playerData[enemyData[id].targetId].stateAnomaly[a] = 1;
                            playerData[enemyData[id].targetId].isEffect_Guarddamage = 2;
                            audioSource[1].PlayOneShot(DataManager.instance.anomalyID[playerData[enemyData[id].targetId].stateAnomaly[a]].se);
                            translator.Run(DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].pname + "は防御崩し状態になった！", results =>
                            {
                                foreach (var n in results)
                                {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                            translator.Run("防御崩し状態の間は守る行動ができない…！", results =>
                            {
                                foreach (var n in results)
                                {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }

                        //防御崩壊解除処理
                        if (playerData[enemyData[id].targetId].stateAnomaly[a] == 1)
                        {
                            playerData[enemyData[id].targetId].isEffect_Guarddamage -= 1;
                            if (playerData[enemyData[id].targetId].isEffect_Guarddamage <= 0)
                            {
                                playerData[enemyData[id].targetId].stateAnomaly[a] = -1;
                                translator.Run(DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].pname + "の防御崩し状態が治った！", results =>
                                {
                                    foreach (var n in results)
                                    {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                    }
                                });
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        a++;
                    }
                }

            }
            else
            {
                //状態付与効果やイベント処理について
                if (playerData[id].targetType == DataManager.TargetType.Enemy)
                {
                    for (int a = 0; a < enemyData[playerData[id].targetId].stateAnomaly.Length;)
                    {
                        //防御崩壊付与処理
                        if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].addAnomaly == 1 && (int)UnityEngine.Random.Range(0, 4) == 0 && !DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].isDFDamageResistance && enemyData[playerData[id].targetId].isEffect_Guarddamage <= 0 && enemyData[playerData[id].targetId].stateAnomaly[a] == -1)
                        {
                            enemyData[playerData[id].targetId].stateAnomaly[a] = 1;
                            enemyData[playerData[id].targetId].isEffect_Guarddamage = 2;
                            audioSource[1].PlayOneShot(DataManager.instance.anomalyID[enemyData[playerData[id].targetId].stateAnomaly[a]].se);
                            translator.Run(enemyData[playerData[id].targetId].name + "は防御崩し状態になった！", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                            translator.Run("防御崩し状態の間は守る行動ができない…！", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }

                        //防御崩壊解除処理
                        if (enemyData[playerData[id].targetId].stateAnomaly[a] == 1)//防御崩壊
                        {
                            enemyData[playerData[id].targetId].isEffect_Guarddamage -= 1;
                            if (enemyData[playerData[id].targetId].isEffect_Guarddamage <= 0)
                            {
                                enemyData[playerData[id].targetId].stateAnomaly[a] = -1;
                                translator.Run(enemyData[playerData[id].targetId].name + "の防御崩し状態が治った！", results =>
                                {
                                    foreach (var n in results)
                                    {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                    }
                                });
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        a++;
                    }
                }
                else
                {
                    for (int a = 0; a < playerData[playerData[id].targetId].stateAnomaly.Length;)
                    {
                        //防御崩壊付与処理
                        if (DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].addAnomaly == 1 && (int)UnityEngine.Random.Range(0, 4) == 0 && !DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].isDFDamageResistance && playerData[playerData[id].targetId].isEffect_Guarddamage <= 0 && playerData[playerData[id].targetId].stateAnomaly[a] == -1)
                        {
                            playerData[playerData[id].targetId].stateAnomaly[a] = 1;
                            playerData[playerData[id].targetId].isEffect_Guarddamage = 2;
                            audioSource[1].PlayOneShot(DataManager.instance.anomalyID[playerData[playerData[id].targetId].stateAnomaly[a]].se);
                            translator.Run(DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname + "は防御崩し状態になった！", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                            translator.Run("防御崩し状態の間は守る行動ができない…！", results =>
                            {
                                foreach (var n in results)
                                {
                                    // ログに表示
                                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                }
                            });
                            yield return new WaitForSeconds(0.3f);
                        }

                        //防御崩壊解除処理
                        if (playerData[playerData[id].targetId].stateAnomaly[a] == 1)
                        {
                            playerData[playerData[id].targetId].isEffect_Guarddamage -= 1;
                            if (playerData[playerData[id].targetId].isEffect_Guarddamage <= 0)
                            {
                                playerData[playerData[id].targetId].stateAnomaly[a] = -1;
                                translator.Run(DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname + "の防御崩し状態が治った！", results =>
                                {
                                    foreach (var n in results)
                                    {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                    }
                                });
                                yield return new WaitForSeconds(0.3f);
                            }
                        }
                        a++;
                    }
                }
            }
        }
        public IEnumerator ItemInvoke(int id = 0)//アイテム使用実行
        {
            if(DataManager.instance.ItemID[playerData[id].selectCommandId].itemSound!=null) audioSource[1].PlayOneShot(DataManager.instance.ItemID[playerData[id].selectCommandId].itemSound);
            int tmpanim = DataManager.instance.ItemID[playerData[id].selectCommandId].charaAnimId;
            if (tmpanim > DataManager.instance.Pstatus[playerData[id].playerId].maxAnime) tmpanim = 3;
            StartCoroutine(EffectAnimation(playerData[id].charaAnim, tmpanim));
            //個数を減らす
            // 値を修正するために、まず要素を取得する
            var item = DataManager.instance.ItemID[playerData[id].selectCommandId];
            item.itemnumber -= 1; // 必要な変更を行う
            // 修正した要素を再度リストにセットする
            DataManager.instance.ItemID[playerData[id].selectCommandId] = item;
            //ログ出力
            //翻訳
            if (playerData[id].targetType == DataManager.TargetType.Enemy)//敵対象の時
            {
                StartCoroutine(EffectAnimation(enemyData[playerData[id].targetId].effectAnim, DataManager.instance.ItemID[playerData[id].selectCommandId].effectId));
                translator.Run(DataManager.instance.Pstatus[playerData[id].playerId].pname + "は" + enemyData[playerData[id].targetId].name + "に対して"+ DataManager.instance.ItemID[playerData[id].selectCommandId].itemname + "を使った！", results =>
                {
                    foreach (var n in results)
                    {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                    }
                });
                yield return new WaitForSeconds(0.3f);
                if (DataManager.instance.ItemID[playerData[id].selectCommandId].isShake)
                {
                    iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                    iTween.ShakePosition(allDialogUI.enemyState[playerData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                }
                //ここからアイテム効果処理
                switch (DataManager.instance.ItemID[playerData[id].selectCommandId].itemType)
                {
                    case DataManager.ItemType.HpCure://HP回復
                        enemyData[playerData[id].targetId].hp += (int)DataManager.instance.ItemID[playerData[id].selectCommandId].fluctuationeffect;
                        if (enemyData[playerData[id].targetId].maxHP < enemyData[playerData[id].targetId].hp) enemyData[playerData[id].targetId].hp = enemyData[playerData[id].targetId].maxHP;
                        //回復文
                        translator.Run(enemyData[playerData[id].targetId].name + "は" + DataManager.instance.ItemID[playerData[id].selectCommandId].itemname + "で、HPが"+ ((int)DataManager.instance.ItemID[playerData[id].selectCommandId].fluctuationeffect).ToString()+ "回復した！", results =>
                        {
                            foreach (var n in results)
                            {
                                // ログに表示
                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                        break;
                    case DataManager.ItemType.MpCure://MP回復
                        enemyData[playerData[id].targetId].mp += (int)DataManager.instance.ItemID[playerData[id].selectCommandId].fluctuationeffect;
                        if (enemyData[playerData[id].targetId].maxMP < enemyData[playerData[id].targetId].mp) enemyData[playerData[id].targetId].mp = enemyData[playerData[id].targetId].maxMP;
                        //回復文
                        translator.Run(enemyData[playerData[id].targetId].name + "は" + DataManager.instance.ItemID[playerData[id].selectCommandId].itemname + "で、MPが" + ((int)DataManager.instance.ItemID[playerData[id].selectCommandId].fluctuationeffect).ToString() + "回復した！", results =>
                        {
                            foreach (var n in results)
                            {
                                // ログに表示
                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                        break;
                    case DataManager.ItemType.Damage:
                        break;
                    case DataManager.ItemType.AtStatusUp:
                        break;
                    case DataManager.ItemType.DfStatusUp:
                        break;
                    default:
                        break;
                }
            }
            else if(playerData[id].targetType == DataManager.TargetType.Player)//味方対象の時
            {
                string tmptarget = "自分";
                if (DataManager.instance.Pstatus[playerData[id].playerId].pname != DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname) tmptarget = DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname;
                StartCoroutine(EffectAnimation(playerData[playerData[id].targetId].effectAnim, DataManager.instance.ItemID[playerData[id].selectCommandId].effectId));
                translator.Run(DataManager.instance.Pstatus[playerData[id].playerId].pname + "は" +tmptarget + "に対して" + DataManager.instance.ItemID[playerData[id].selectCommandId].itemname + "を使った！", results =>
                {
                    foreach (var n in results)
                    {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                    }
                });
                yield return new WaitForSeconds(0.3f);
                if (DataManager.instance.ItemID[playerData[id].selectCommandId].isShake)
                {
                    iTween.ShakePosition(allDialogUI.logDialog.logBackground.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                    iTween.ShakePosition(allDialogUI.playerState[playerData[id].targetId].content.gameObject, iTween.Hash("x", shakePower, "y", shakePower, "time", 0.5f));
                }
                //ここからアイテム効果処理
                switch (DataManager.instance.ItemID[playerData[id].selectCommandId].itemType)
                {
                    case DataManager.ItemType.HpCure://HP回復
                        playerData[playerData[id].targetId].hp += (int)DataManager.instance.ItemID[playerData[id].selectCommandId].fluctuationeffect;
                        if (playerData[playerData[id].targetId].maxHP < playerData[playerData[id].targetId].hp) playerData[playerData[id].targetId].hp = playerData[playerData[id].targetId].maxHP;
                        //回復文
                        translator.Run(DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname + "は" + DataManager.instance.ItemID[playerData[id].selectCommandId].itemname + "で、HPが" + ((int)DataManager.instance.ItemID[playerData[id].selectCommandId].fluctuationeffect).ToString() + "回復した！", results =>
                        {
                            foreach (var n in results)
                            {
                                // ログに表示
                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                        break;
                    case DataManager.ItemType.MpCure://MP回復
                        playerData[playerData[id].targetId].mp += (int)DataManager.instance.ItemID[playerData[id].selectCommandId].fluctuationeffect;
                        if (playerData[playerData[id].targetId].maxMP < playerData[playerData[id].targetId].mp) playerData[playerData[id].targetId].mp = playerData[playerData[id].targetId].maxMP;
                        //回復文
                        translator.Run(DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname + "は" + DataManager.instance.ItemID[playerData[id].selectCommandId].itemname + "で、MPが" + ((int)DataManager.instance.ItemID[playerData[id].selectCommandId].fluctuationeffect).ToString() + "回復した！", results =>
                        {
                            foreach (var n in results)
                            {
                                // ログに表示
                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                        break;
                    case DataManager.ItemType.Damage:
                        break;
                    case DataManager.ItemType.AtStatusUp:
                        break;
                    case DataManager.ItemType.DfStatusUp:
                        break;
                    default:
                        break;
                }
            }
            //クールタイム
            //スキル干渉(俊敏)
            for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
            {
                if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] == 3)
                {
                    playerData[id].coolTime = DataManager.instance.ItemID[playerData[id].selectCommandId].cooltime / 4 * 3;
                }
                else playerData[id].coolTime = DataManager.instance.ItemID[playerData[id].selectCommandId].cooltime;
                i++;
            }
            allDialogUI.playerState[id].cooltimeSlider.maxValue = playerData[id].coolTime;
            allDialogUI.playerState[id].cooltimeSlider.value = allDialogUI.playerState[id].cooltimeSlider.maxValue;
            ViewReset(id);
        }
        public IEnumerator SearchInvoke(int id = 0)//調べる行動実行
        {
            audioSource[1].PlayOneShot(se[4]);
            StartCoroutine(EffectAnimation(playerData[id].charaAnim, 5));
            StartCoroutine(EffectAnimation(enemyData[playerData[id].targetId].effectAnim, 4));
            //ログ出力
            //翻訳
            translator.Run(DataManager.instance.Pstatus[playerData[id].playerId].pname + "は" + enemyData[playerData[id].targetId].name + "について探り、上記の情報が判明した！", results =>
             {
                 foreach (var n in results)
                 {
                     // ログに表示
                     allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                 }
             });
            yield return new WaitForSeconds(0.3f);
            string tmpstring = "弱点属性:";
            switch (DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype)
            {
                case DataManager.MagicType.Dark:
                    tmpstring += "闇属性";
                    break;
                case DataManager.MagicType.Holy:
                    tmpstring += "光属性";
                    break;
                case DataManager.MagicType.God:
                    tmpstring += "神属性";
                    break;
                case DataManager.MagicType.Fire:
                    tmpstring += "火属性";
                    break;
                case DataManager.MagicType.Water:
                    tmpstring += "水属性";
                    break;
                case DataManager.MagicType.Natural:
                    tmpstring += "自然属性";
                    break;
                case DataManager.MagicType.Normal:
                    tmpstring += "無属性";
                    break;
                default:
                    break;
            }
            switch (DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].badtype2)
            {
                case DataManager.MagicType.Dark:
                    tmpstring += ",闇属性";
                    break;
                case DataManager.MagicType.Holy:
                    tmpstring += ",光属性";
                    break;
                case DataManager.MagicType.God:
                    tmpstring += ",神属性";
                    break;
                case DataManager.MagicType.Fire:
                    tmpstring += ",火属性";
                    break;
                case DataManager.MagicType.Water:
                    tmpstring += ",水属性";
                    break;
                case DataManager.MagicType.Natural:
                    tmpstring += ",自然属性";
                    break;
                case DataManager.MagicType.Normal:
                    tmpstring += ",無属性";
                    break;
                default:
                    if (tmpstring == "弱点属性:") tmpstring += "無し";
                    break;
            }
            int tmprate = 100;
            if (DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].escapeRate != -1) tmprate = tmprate / DataManager.instance.enemynoteID[enemyData[playerData[id].targetId].enemyId].escapeRate;
            else tmprate = 0;
            tmpstring += ", 逃走確率:" + tmprate.ToString();
            //翻訳
            translator.Run(tmpstring, results =>
            {
                foreach (var n in results)
                {
                    // ログに表示
                    allDialogUI.logDialog.logText.text = n.translated + "%\n" + allDialogUI.logDialog.logText.text;
                }
            });

            string tmpstring2 = "HP:" + enemyData[playerData[id].targetId].hp + "/" + enemyData[playerData[id].targetId].maxHP + ", AT:" + enemyData[playerData[id].targetId].attack + ", DF:" + enemyData[playerData[id].targetId].defense;
            //翻訳
            translator.Run(tmpstring2, results =>
            {
                foreach (var n in results)
                {
                    // ログに表示
                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                }
            });
            //スキル干渉(俊敏)
            for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
            {
                if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] == 3)
                {
                    playerData[id].coolTime = 1f / 4 * 3;
                }
                else playerData[id].coolTime = 1f;
                i++;
            }
            allDialogUI.playerState[id].cooltimeSlider.maxValue = playerData[id].coolTime;
            allDialogUI.playerState[id].cooltimeSlider.value = allDialogUI.playerState[id].cooltimeSlider.maxValue;
            ViewReset(id);
        }
        public IEnumerator GuardInvoke(int id = 0)
        {
            audioSource[1].PlayOneShot(se[11]);
            //ログ出力
            //翻訳
            string tmptarget = "自分";
            if (DataManager.instance.Pstatus[playerData[id].playerId].pname != DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname) tmptarget = DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname;
            translator.Run(DataManager.instance.Pstatus[playerData[id].playerId].pname + "は" + tmptarget + "を守るための防御態勢を解除した", results =>
            {
                foreach (var n in results)
                {
                    // ログに表示
                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                }
            });
            playerData[playerData[id].targetId].isGuard = false;
            yield return new WaitForSeconds(0.3f);
            
            //スキル干渉(俊敏)
            for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
            {
                if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] == 3)
                {
                    playerData[id].coolTime = 1f / 4 * 3;
                }
                else playerData[id].coolTime = 1f;
                i++;
            }
            allDialogUI.playerState[id].cooltimeSlider.maxValue = playerData[id].coolTime;
            allDialogUI.playerState[id].cooltimeSlider.value = allDialogUI.playerState[id].cooltimeSlider.maxValue;
            ViewReset(id);
        }
        public IEnumerator GuardEnemyInvoke(int id = 0)
        {
            audioSource[1].PlayOneShot(se[11]);
            //ログ出力
            //翻訳
            string tmptarget = "自分";
            if (enemyData[id].name != enemyData[enemyData[id].targetId].name) tmptarget = enemyData[enemyData[id].targetId].name;
            translator.Run(enemyData[id].name + "は" + tmptarget + "を守るための防御態勢を解除した", results =>
            {
                foreach (var n in results)
                {
                    // ログに表示
                    allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                }
            });
            enemyData[enemyData[id].targetId].isGuard = false;
            yield return new WaitForSeconds(0.3f);

            //スキル干渉(俊敏)
            for (int i = 0; i < DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill.Length;)
            {
                if (DataManager.instance.enemynoteID[enemyData[id].enemyId].inputskill[i] == 3)
                {
                    enemyData[id].coolTime = 6f / 4 * 3;
                }
                else enemyData[id].coolTime = 6f;
                i++;
            }
            allDialogUI.enemyState[id].cooltimeSlider.maxValue = enemyData[id].coolTime;
            allDialogUI.enemyState[id].cooltimeSlider.value = allDialogUI.enemyState[id].cooltimeSlider.maxValue;
            enemyData[id].selectCommandType = CommandType.None;
            enemyData[id].selectCommandId = -1;
            enemyData[id].targetId = -1;
            ViewResetEnemy(id);
            enemyData[id].isSelected = false;
        }
        public IEnumerator EscapeInvoke(int id = 0)//逃げだす行動実行
        {
            //逃走確率を計算
            int minrate = 999;
            for(int i = 0; i < enemyData.Length;)
            {
                if (enemyData[i].enemyId != -1 && DataManager.instance.enemynoteID[enemyData[i].enemyId].escapeRate < minrate)
                {
                    minrate = DataManager.instance.enemynoteID[enemyData[i].enemyId].escapeRate;
                }
                i++;
            }
            if (minrate != -1)
            {
                //スキルで確率操作
                for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
                {
                    if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] == 4)
                    {
                        minrate = minrate / 3 * 2;
                    }
                    i++;
                }

                //確率で成功
                if (UnityEngine.Random.Range(0, minrate) == 0)
                {
                    audioSource[1].PlayOneShot(se[2]);
                    StartCoroutine(EffectAnimation(playerData[id].charaAnim, 2));
                    //ログ出力
                    //翻訳
                    translator.Run(DataManager.instance.Pstatus[playerData[id].playerId].pname + "達は逃げることに成功した！(クリックして戦闘終了)", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    DataManager.instance.Triggers[1] = 0;
                    yield return WaitForClick();
                    yield return WaitForClick();
                    //バトル終了アニメーション開始
                    systemAnim.SetInteger("trg", 2);
                    yield return new WaitForSeconds(0.3f);
                    //プレイヤーのHPとMPをDataManagerに反映
                    for(int pp = 0; pp < playerData.Length;)
                    {
                        if (playerData[pp].playerId != -1)
                        {
                            // 値を修正するために、まず要素を取得する
                            var tmpplayer = DataManager.instance.Pstatus[playerData[pp].playerId];
                            tmpplayer.hp=playerData[pp].hp; // 必要な変更を行う
                            tmpplayer.mp = playerData[pp].mp; // 必要な変更を行う
                            // 修正した要素を再度リストにセットする
                            DataManager.instance.Pstatus[playerData[pp].playerId] = tmpplayer;
                        }
                        pp++;
                    }
                    //ダイアログを終了
                    if (allDialogUI.commandDialog.commandBackground.gameObject.activeSelf) allDialogUI.commandDialog.commandBackground.gameObject.SetActive(false);
                    if (allDialogUI.playerSelectDialog.pselectBackground.gameObject.activeSelf) allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
                    if (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                    if (allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
                    DataManager.instance.Triggers[0] = 1;
                    //BGMを元に戻す
                    audioSource[0].Stop();
                    audioSource[0].clip = defaultBGM;
                    audioSource[0].loop = true;
                    audioSource[0].Play();
                    DataManager.instance.Triggers[0] = 1;
                    DataManager.instance.Triggers[1] = 0;
                    DataManager.instance.Triggers[2] = 0;
                    DataManager.instance.isSave = true;
                    DataManager.instance.AllSaveInvoke();
                    yield return new WaitUntil(() => (!DataManager.instance.isSave));
                    yield return new WaitForSeconds(1);
                    systemAnim.SetInteger("trg", 0);
                }
                //逃走失敗
                else
                {
                    audioSource[1].PlayOneShot(se[3]);
                    //ログ出力
                    //翻訳
                    translator.Run(DataManager.instance.Pstatus[playerData[id].playerId].pname + "達は逃げることに失敗した。。", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    //スキル干渉(俊敏)
                    for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
                    {
                        if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] == 3)
                        {
                            playerData[id].coolTime = 4f / 4 * 3;
                        }
                        else playerData[id].coolTime = 4f;
                        i++;
                    }
                    allDialogUI.playerState[id].cooltimeSlider.maxValue = playerData[id].coolTime;
                    allDialogUI.playerState[id].cooltimeSlider.value = allDialogUI.playerState[id].cooltimeSlider.maxValue;
                    playerData[id].isSelected = false;
                    ViewReset(id);
                }
            }
            //逃走失敗
            else
            {
                audioSource[1].PlayOneShot(se[3]);
                //ログ出力
                //翻訳
                translator.Run(DataManager.instance.Pstatus[playerData[id].playerId].pname + "達は逃げだすことができない。。", results =>
                {
                    foreach (var n in results)
                    {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                    }
                });
                //スキル干渉(俊敏)
                for (int i = 0; i < DataManager.instance.Pstatus[playerData[id].playerId].inputskill.Length;)
                {
                    if (DataManager.instance.Pstatus[playerData[id].playerId].inputskill[i] == 3)
                    {
                        playerData[id].coolTime = 4f / 4 * 3;
                    }
                    else playerData[id].coolTime = 4f;
                    i++;
                }
                allDialogUI.playerState[id].cooltimeSlider.maxValue = playerData[id].coolTime;
                allDialogUI.playerState[id].cooltimeSlider.value = allDialogUI.playerState[id].cooltimeSlider.maxValue;
                playerData[id].isSelected = false;
                ViewReset(id);
            }
            IEnumerator WaitForClick()
            {
                // 左クリックまたはEnterキーが押されるまで待つ
                yield return new WaitUntil(() => (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)));
            }
        }
        public IEnumerator EscapeEnemyInvoke(int id = 0)//逃げだす行動実行
        {
            //逃走で成功
            audioSource[1].PlayOneShot(se[2]);
            StartCoroutine(EffectAnimation(enemyData[id].charaAnim, 2));
            StartCoroutine(EscapeSprite(enemyData[id].spriteRen));
            //ログ出力
            //翻訳
            translator.Run(enemyData[id].name + "は逃げることに成功した！", results =>
            {
                foreach (var n in results)
                {
                        // ログに表示
                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                }
            });
            yield return new WaitForSeconds(0.5f);
            enemyData[id].enemyId = -1;
            bool isEnemy = false;
            bool isKill = false;
            for(int i = 0; i< enemyData.Length;)
            {
                if (enemyData[i].enemyId != -1 && enemyData[i].hp > 0) isEnemy = true;
                if (enemyData[i].enemyId != -1 && enemyData[i].hp <= 0) isKill = true;
                i++;
            }
            //まだ残っている場合
            if (isEnemy)
            {
                ViewResetEnemy(id);
            }
            //敵がいなくなったら
            else 
            {
                if (isKill)
                {
                    isEnd = true;
                    StartCoroutine(GameOverEvent(DataManager.TargetType.Enemy));
                }
                else
                {
                    translator.Run("全ての敵がいなくなってしまった！(クリックで戦闘終了)", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return WaitForClick();

                    DataManager.instance.Triggers[1] = 0;
                    //バトル終了アニメーション開始
                    systemAnim.SetInteger("trg", 2);
                    yield return new WaitForSeconds(0.3f);
                    //プレイヤーのHPとMPをDataManagerに反映
                    for (int pp = 0; pp < playerData.Length;)
                    {
                        if (playerData[pp].playerId != -1)
                        {
                            // 値を修正するために、まず要素を取得する
                            var tmpplayer = DataManager.instance.Pstatus[playerData[pp].playerId];
                            tmpplayer.hp = playerData[pp].hp; // 必要な変更を行う
                            tmpplayer.mp = playerData[pp].mp; // 必要な変更を行う
                                                              // 修正した要素を再度リストにセットする
                            DataManager.instance.Pstatus[playerData[pp].playerId] = tmpplayer;
                        }
                        pp++;
                    }
                    //ダイアログを終了
                    if (allDialogUI.commandDialog.commandBackground.gameObject.activeSelf) allDialogUI.commandDialog.commandBackground.gameObject.SetActive(false);
                    if (allDialogUI.playerSelectDialog.pselectBackground.gameObject.activeSelf) allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
                    if (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                    if (allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
                    DataManager.instance.Triggers[0] = 1;
                    //BGMを元に戻す
                    audioSource[0].Stop();
                    audioSource[0].clip = defaultBGM;
                    audioSource[0].loop = true;
                    audioSource[0].Play();
                    DataManager.instance.Triggers[0] = 1;
                    DataManager.instance.Triggers[1] = 0;
                    DataManager.instance.Triggers[2] = 0;
                    DataManager.instance.isSave = true;
                    DataManager.instance.AllSaveInvoke();
                    yield return new WaitUntil(() => (!DataManager.instance.isSave));
                    yield return new WaitForSeconds(1);
                    systemAnim.SetInteger("trg", 0);
                    enemyData[id].selectCommandType = CommandType.None;
                    enemyData[id].selectCommandId = -1;
                    enemyData[id].targetId = -1;
                    ViewResetEnemy(id);
                }
            }
            enemyData[id].isSelected = false;
            IEnumerator WaitForClick()
            {
                // 左クリックまたはEnterキーが押されるまで待つ
                yield return new WaitUntil(() => (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)));
            }
        }
        public void ViewReset(int id = 0)
        {
            //予約したコマンド名とターゲットの表示
            string tmpcommand = "";
            string tmptarget = "";
            if (!playerData[id].isSelected)
            {
                playerData[id].selectCommandType = CommandType.None;
                playerData[id].targetId = -1;
            }
            //選択コマンド名
            switch (playerData[id].selectCommandType)
            {
                case (CommandType.Attack):
                    tmpcommand = DataManager.instance.MagicID[playerData[id].magicId[playerData[id].selectCommandId]].magicname;
                    break;
                case (CommandType.Guard):
                    tmpcommand = "守る";
                    break;
                case (CommandType.Item):
                    tmpcommand = DataManager.instance.ItemID[playerData[id].selectCommandId].itemname;
                    break;
                case (CommandType.Search):
                    tmpcommand = "調べる";
                    break;
                case (CommandType.Escape):
                    tmpcommand = "逃げる";
                    break;
                default:
                    tmpcommand = "未選択";
                    break;
            }
            //選択ターゲット名
            if (playerData[id].targetId != -1)
            {
                switch (playerData[id].targetType)
                {
                    case (DataManager.TargetType.Enemy):
                        tmptarget = enemyData[playerData[id].targetId].name;
                        break;
                    case (DataManager.TargetType.Player):
                        tmptarget = DataManager.instance.Pstatus[playerData[playerData[id].targetId].playerId].pname;
                        break;
                    default:
                        tmptarget = "未選択";
                        break;
                }
            }
            else tmptarget = "未選択";
            allDialogUI.playerSelectDialog.selectedCommandText[id].text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
            allDialogUI.playerState[id].nextCommand.text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
            //瀕死の場合
            if (playerData[id].isOver)
            {
                if (!allDialogUI.playerSelectDialog.selectedBackground[id].gameObject.activeSelf) allDialogUI.playerSelectDialog.selectedBackground[id].gameObject.SetActive(true);
                //予約したコマンド名とターゲットの表示
                tmpcommand = "選択不可";
                tmptarget = "選択不可";
                allDialogUI.playerSelectDialog.selectedCommandText[id].text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
                allDialogUI.playerState[id].nextCommand.text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
            }

            allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
            if (!playerData[id].isSelected)
            {
                playerData[id].selectCommandType = CommandType.None;
                playerData[id].targetId = -1;
            }
            UpdateState();
            if (uiMode == UIMode.NoSelected) NoSelected();
            Resources.UnloadUnusedAssets();
        }
        public void ViewResetEnemy(int id = 0)
        {
            //予約したコマンド名とターゲットの表示
            string tmpcommand = "";
            string tmptarget = "";
            if (!enemyData[id].isSelected)
            {
                enemyData[id].selectCommandType = CommandType.None;
                enemyData[id].targetId = -1;
            }

            //選択コマンド名
            switch (enemyData[id].selectCommandType)
            {
                case (CommandType.Attack):
                    tmpcommand = DataManager.instance.MagicID[enemyData[id].magicId[enemyData[id].selectCommandId]].magicname;
                    break;
                case (CommandType.Guard):
                    tmpcommand = "守る";
                    break;
                case (CommandType.Item):
                    tmpcommand = DataManager.instance.ItemID[enemyData[id].selectCommandId].itemname;
                    break;
                case (CommandType.Search):
                    tmpcommand = "調べる";
                    break;
                case (CommandType.Escape):
                    tmpcommand = "逃げる";
                    break;
                default:
                    tmpcommand = "未選択";
                    break;
            }
            //選択ターゲット名
            if (enemyData[id].targetId != -1)
            {
                switch (enemyData[id].targetType)
                {
                    case (DataManager.TargetType.Enemy):
                        tmptarget = enemyData[enemyData[id].targetId].name;
                        break;
                    case (DataManager.TargetType.Player):
                        tmptarget = DataManager.instance.Pstatus[playerData[enemyData[id].targetId].playerId].pname;
                        break;
                    default:
                        tmptarget = "未選択";
                        break;
                }
            }
            else tmptarget = "未選択";

            allDialogUI.enemyState[id].nextCommand.text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
            //瀕死の場合
            if (enemyData[id].isOver)
            {
                //予約したコマンド名とターゲットの表示
                tmpcommand = "選択不可";
                tmptarget = "選択不可";
                allDialogUI.enemyState[id].nextCommand.text = "次の行動:" + tmpcommand + "\n対象:" + tmptarget;
            }

            UpdateState();
            NoSelectedEnemy();
            Resources.UnloadUnusedAssets();
        }
        //全滅処理
        public IEnumerator GameOverEvent(DataManager.TargetType tmptargetType)
        {
            switch (tmptargetType)
            {
                case DataManager.TargetType.Enemy://全滅して勝利
                    audioSource[1].PlayOneShot(se[6]);
                    //クリア
                    translator.Run("全ての敵を倒して戦闘に勝利した！！", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return new WaitForSeconds(1f);
                    //コイン獲得
                    int getcoin = UnityEngine.Random.Range(randomData.mingetcoin, randomData.maxgetcoin);
                    bool noskill = true;
                    for(int p=0; p < playerData.Length;)
                    {
                        if (playerData[p].playerId != -1)
                        {
                            //味方内に幸運持ちがいる場合
                            for (int s = 0; s < DataManager.instance.Pstatus[playerData[p].playerId].inputskill.Length;)
                            {
                                if (DataManager.instance.Pstatus[playerData[p].playerId].inputskill[s] == 0)
                                {
                                    noskill = false;
                                    getcoin = UnityEngine.Random.Range((randomData.mingetcoin * 3 / 2), (randomData.maxgetcoin * 3 / 2));
                                    audioSource[1].PlayOneShot(se[8]);
                                    translator.Run("スライム達は幸運にも多めに" + getcoin.ToString() + "Gを獲得した！", results =>
                                    {
                                        foreach (var n in results)
                                        {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                        }
                                    });
                                }
                                s++;
                            }
                        }
                        p++;
                    }
                    DataManager.instance.Coin = getcoin;
                    if (noskill)//味方内に幸運持ちがいない場合
                    {
                        audioSource[1].PlayOneShot(se[8]);
                        translator.Run("スライム達は"+getcoin.ToString()+"Gを獲得した！", results =>
                        {
                            foreach (var n in results)
                            {
                                // ログに表示
                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                            }
                        });
                    }
                    yield return new WaitForSeconds(1f);
                    //ドロップ
                    noskill = true;
                    for (int p = 0; p < playerData.Length;)
                    {
                        if (playerData[p].playerId != -1)
                        {
                            //味方内に幸運持ちがいる場合
                            for (int s = 0; s < DataManager.instance.Pstatus[playerData[p].playerId].inputskill.Length;)
                            {
                                if (DataManager.instance.Pstatus[playerData[p].playerId].inputskill[s] == 0)
                                {
                                    //処理
                                    for (int e = 0; e < enemyData.Length;)
                                    {
                                        if (enemyData[e].enemyId != -1)
                                        {
                                            for (int d = 0; d < DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData.Length;)
                                            {
                                                int rate = DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData[d].dropRate / 4 * 3;
                                                if (rate < 1) rate = 1;
                                                if ((int)UnityEngine.Random.Range(0, rate) == 0)
                                                {
                                                    int getnum = UnityEngine.Random.Range(1, 2);
                                                    noskill = false;
                                                    var itemset = DataManager.instance.ItemID[DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData[d].dropItemId];
                                                    itemset.itemnumber += getnum;
                                                    if (itemset.itemnumber > 99) itemset.itemnumber = 99;
                                                    itemset.gettrg = 1;
                                                    DataManager.instance.ItemID[DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData[d].dropItemId] = itemset;
                                                    audioSource[1].PlayOneShot(se[8]);
                                                    translator.Run("スライム達は幸運にも" + DataManager.instance.ItemID[DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData[d].dropItemId].itemname + "を" + getnum.ToString() + "個獲得した！", results =>
                                                         {
                                                             foreach (var n in results)
                                                             {
                                                        // ログに表示
                                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                                             }
                                                         });
                                                    yield return new WaitForSeconds(0.5f);
                                                }
                                                d++;
                                            }
                                        }
                                        e++;
                                    }
                                }
                                s++;
                            }
                        }
                        p++;
                    }
                    DataManager.instance.Coin = getcoin;
                    if (noskill)//味方内に幸運持ちがいない場合
                    {
                        //処理
                        for (int e = 0; e < enemyData.Length;)
                        {
                            if (enemyData[e].enemyId != -1)
                            {
                                for (int d = 0; d < DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData.Length;)
                                {
                                    int rate = DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData[d].dropRate;
                                    if (rate < 1) rate = 1;
                                    if ((int)UnityEngine.Random.Range(0, rate) == 0)
                                    {
                                        int getnum = 1;
                                        noskill = false;
                                        var itemset = DataManager.instance.ItemID[DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData[d].dropItemId];
                                        itemset.itemnumber += getnum;
                                        if (itemset.itemnumber > 99) itemset.itemnumber = 99;
                                        itemset.gettrg = 1;
                                        DataManager.instance.ItemID[DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData[d].dropItemId] = itemset;
                                        audioSource[1].PlayOneShot(se[8]);
                                        translator.Run("スライム達は" + DataManager.instance.ItemID[DataManager.instance.enemynoteID[enemyData[e].enemyId].dropData[d].dropItemId].itemname + "を" + getnum.ToString() + "個獲得した！", results =>
                                        {
                                            foreach (var n in results)
                                            {
                                                // ログに表示
                                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                            }
                                        });
                                        yield return new WaitForSeconds(0.5f);
                                    }
                                    d++;
                                }
                            }
                            e++;
                        }
                    }
                    //獲得経験値計算処理
                    int getexp = 0;
                    for(int e=0; e < enemyData.Length;)
                    {
                        if (enemyData[e].enemyId != -1)
                        {
                            getexp += (enemyData[e].getExp * ((enemyData[e].lv+1)/2));
                        }
                        e++;
                    }
                    translator.Run("スライム達は" + getexp.ToString() + "経験値を獲得した！", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    audioSource[1].PlayOneShot(se[10]);
                    yield return new WaitForSeconds(1f);
                    //経験値反映
                    for(int p=0; p < playerData.Length;)
                    {
                        if (playerData[p].playerId != -1&& playerData[p].lv<100)
                        {
                            var pstatus = DataManager.instance.Pstatus[playerData[p].playerId];
                            pstatus.inputExp += getexp;
                            if (pstatus.inputExp >= pstatus.maxExp * pstatus.Lv)
                            {
                                //レベルアップ処理
                                int oldlv = pstatus.Lv;
                                pstatus.inputExp -= pstatus.maxExp * pstatus.Lv;
                                pstatus.Lv += 1;
                                playerData[p].maxHP = (int)((pstatus.maxHP + pstatus.add_hp) * (1 + (pstatus.Lv / 1.5f)));
                                playerData[p].hp = playerData[p].maxHP;
                                playerData[p].maxMP = (int)((pstatus.maxMP + pstatus.add_mp) * (1 + (pstatus.Lv / 1.5f)));
                                playerData[p].mp = playerData[p].maxMP;
                                translator.Run(pstatus.pname + "はレベルアップしてLv" + pstatus.Lv.ToString() + "になった！", results =>
                                  {
                                      foreach (var n in results)
                                      {
                                        // ログに表示
                                        allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                      }
                                  });
                                yield return new WaitForSeconds(1f);
                                //習得魔法情報表示
                                for (int m = 0; m < pstatus.getMagic.Length;)
                                {
                                    if (pstatus.getMagic[m].inputlevel <= pstatus.Lv)
                                    {
                                        pstatus.getMagic[m].gettrg = 1;
                                        if (pstatus.getMagic[m].inputlevel > oldlv)
                                        {
                                            for (int a = 0; a < pstatus.magicSet.Length;)
                                            {
                                                if (pstatus.magicSet[a] == -1)
                                                {
                                                    pstatus.magicSet[a] = pstatus.getMagic[m].magicid;
                                                    break;
                                                }
                                                a++;
                                            }
                                            translator.Run(pstatus.pname + "は" + DataManager.instance.MagicID[pstatus.getMagic[m].magicid].magicname + "を習得した！", results =>{
                                            foreach (var n in results)
                                            {
                                                // ログに表示
                                                allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                                            }
                                            });
                                            yield return new WaitForSeconds(0.5f);
                                        }
                                    }
                                    m++;
                                }
                            }
                            DataManager.instance.Pstatus[playerData[p].playerId] = pstatus;
                        }
                        p++;
                    }
                    translator.Run("(クリックして戦闘を終了する)", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return WaitForClick();
                    yield return WaitForClick();
                    //バトル終了アニメーション開始
                    systemAnim.SetInteger("trg", 2);
                    yield return new WaitForSeconds(1f);
                   
                    //ダイアログを終了
                    if (allDialogUI.commandDialog.commandBackground.gameObject.activeSelf) allDialogUI.commandDialog.commandBackground.gameObject.SetActive(false);
                    if (allDialogUI.playerSelectDialog.pselectBackground.gameObject.activeSelf) allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
                    if (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                    if (allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
                    DataManager.instance.Triggers[0] = 1;
                    //BGMを元に戻す
                    audioSource[0].Stop();
                    audioSource[0].clip = defaultBGM;
                    audioSource[0].loop = true;
                    audioSource[0].Play();
                    //ボスなら最後に会話イベントを挟む
                    //ボスかどうかチェックし、クリアイベントを挟むかどうか判断
                    for (int e = 0; e < enemyData.Length;)
                    {
                        if (enemyData[e].enemyId!=-1&&DataManager.instance.enemynoteID[enemyData[e].enemyId].isBoss)
                        {
                            DataManager.instance.Triggers[2] = 1;
                            bossEnemydataIndex = e;
                            dialogueSystem.DialogueUI.SetInteger("trg", 1);
                            dialogueSystem.eventId = DataManager.instance.enemynoteID[enemyData[e].enemyId].bossClearEvent;
                            yield return null;
                            StartCoroutine(dialogueSystem.ShowNextComment());
                        }
                        e++;
                    }

                    //プレイヤーのHPとMPをDataManagerに反映
                    for (int pp = 0; pp < playerData.Length;)
                    {
                        if (playerData[pp].playerId != -1)
                        {
                            // 値を修正するために、まず要素を取得する
                            var tmpplayer = DataManager.instance.Pstatus[playerData[pp].playerId];
                            tmpplayer.hp = playerData[pp].hp; // 必要な変更を行う
                            tmpplayer.mp = playerData[pp].mp; // 必要な変更を行う
                            // 修正した要素を再度リストにセットする
                            DataManager.instance.Pstatus[playerData[pp].playerId] = tmpplayer;
                        }
                        pp++;
                    }
                    DataManager.instance.Triggers[1] = 0;
                    DataManager.instance.Triggers[2] = 0;
                    DataManager.instance.AllSaveInvoke();
                    yield return new WaitUntil(() => (!DataManager.instance.isSave));
                    yield return new WaitForSeconds(1);
                    systemAnim.SetInteger("trg", 0);
                    break;
                case DataManager.TargetType.Player://全滅してゲームオーバー
                    audioSource[0].Stop();
                    audioSource[0].clip = se[7];
                    audioSource[0].loop = false;
                    audioSource[0].Play();
                    //全滅表示
                    translator.Run("全滅してしまった。。", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return new WaitForSeconds(1f);
                    //実績
                    if (DataManager.instance.achievementsID[3].gettrg < 1)
                    {
                        DataManager.instance.TextGet = "実績：" + DataManager.instance.achievementsID[3].name;
                        var achi = DataManager.instance.achievementsID[3];
                        achi.gettrg = 1;
                        DataManager.instance.achievementsID[3] = achi;
                    }
                    //死亡演出
                    translator.Run("？？？「おぉ スライムよ！死んでしまうとは情けない！」", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return new WaitForSeconds(1f);
                    //所持金減らしつつゲームオーバー処理
                    int coin = 1;
                    if (DataManager.instance.Coin > 0)
                    {
                        
                        if (DataManager.instance.Coin > 9999) coin = UnityEngine.Random.Range(1, 333);
                        else if (DataManager.instance.Coin > 999) coin = UnityEngine.Random.Range(1, 33);
                        if (DataManager.instance.Coin > 99) coin = UnityEngine.Random.Range(1, 3);
                        DataManager.instance.Coin -= coin;
                    }
                    PlayerPrefs.SetString("Coin", DataManager.instance.Coin.ToString());
                    audioSource[1].PlayOneShot(se[9]);
                    translator.Run(coin.ToString()+"Gを一部失ってゲームオーバーとなった。。(クリックしてセーブ地点から再開)", results =>
                    {
                        foreach (var n in results)
                        {
                            // ログに表示
                            allDialogUI.logDialog.logText.text = n.translated + "\n" + allDialogUI.logDialog.logText.text;
                        }
                    });
                    yield return WaitForClick();
                    yield return WaitForClick();
                    //バトル終了アニメーション開始
                    systemAnim.SetInteger("trg", 2);
                    yield return new WaitForSeconds(0.3f);
                    //プレイヤーのHPとMPをDataManagerに反映
                    for (int pp = 0; pp < playerData.Length;)
                    {
                        if (playerData[pp].playerId != -1)
                        {
                            // 値を修正するために、まず要素を取得する
                            var tmpplayer = DataManager.instance.Pstatus[playerData[pp].playerId];
                            tmpplayer.hp = playerData[pp].maxHP; // 必要な変更を行う
                            tmpplayer.mp = playerData[pp].maxMP; // 必要な変更を行う
                            // 修正した要素を再度リストにセットする
                            DataManager.instance.Pstatus[playerData[pp].playerId] = tmpplayer;
                        }
                        pp++;
                    }
                    //ダイアログを終了
                    if (allDialogUI.commandDialog.commandBackground.gameObject.activeSelf) allDialogUI.commandDialog.commandBackground.gameObject.SetActive(false);
                    if (allDialogUI.playerSelectDialog.pselectBackground.gameObject.activeSelf) allDialogUI.playerSelectDialog.pselectBackground.gameObject.SetActive(false);
                    if (allDialogUI.targetSelectDialogE.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogE.targetBackground.gameObject.SetActive(false);
                    if (allDialogUI.targetSelectDialogP.targetBackground.gameObject.activeSelf) allDialogUI.targetSelectDialogP.targetBackground.gameObject.SetActive(false);
                    //再開
                    DataManager.instance.Triggers[0] = 1;
                    DataManager.instance.Triggers[1] = 0;
                    DataManager.instance.Triggers[2] = 0;
                    DataManager.instance.isSave = true;
                    DataManager.instance.AllSaveInvoke();
                    yield return new WaitUntil(() => (!DataManager.instance.isSave));
                    DataManager.instance.AllLoadInvoke();
                    SceneManager.LoadScene("newmain");
                    break;
            }

            IEnumerator WaitForClick()
            {
                // 左クリックまたはEnterキーが押されるまで待つ
                yield return new WaitUntil(() => (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)));
            }
        }
    }
}
