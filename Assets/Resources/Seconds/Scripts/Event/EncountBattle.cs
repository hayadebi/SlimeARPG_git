using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountBattle : MonoBehaviour
{
    [Header("BattleSystem本体格納")]
    public UniLang.BattleSystem battleSystem;
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
    [Header("ランダム範囲")]
    public RandomData randomData;

    public AudioSource audioSource;
    public AudioClip[] se;

    [Header("エフェクト")]
    public GameObject[] Effect;
    [Header("ダイアログシスタム格納")]
    public UniLang.OriginalDialogueSystem originalDialogueSystem;
    [Header("エンカウント確率")]
    public int encountRate = 1;
    [Header("Triggerの有無")]
    public int isTrg = -1;
    public int checkTrg = 0;
    private bool isDestroy = false;

    public bool isEnter = true;
    // Start is called before the first frame update
    void Start()
    {
        battleSystem = GameObject.Find("BattleDialog").GetComponent<UniLang.BattleSystem>();
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDestroy&&isTrg != -1 && DataManager.instance.Triggers[isTrg] !=checkTrg)
        {
            isDestroy = true;
            this.gameObject.SetActive(false);
        }
    }
    public void SetSystem()
    {
        battleSystem.randomData.adventEnemyId = randomData.adventEnemyId;
        battleSystem.randomData.minLv = randomData.minLv;
        battleSystem.randomData.maxLv = randomData.maxLv;
        battleSystem.randomData.setBgm = randomData.setBgm;
        battleSystem.randomData.battleBackground = randomData.battleBackground;
        battleSystem.randomData.maxAdvent = randomData.maxAdvent;
        battleSystem.randomData.maxaddState = randomData.maxaddState;
        battleSystem.randomData.mingetcoin = randomData.mingetcoin;
        battleSystem.randomData.maxgetcoin = randomData.maxgetcoin;
        StartCoroutine(battleSystem.BattleStart());
    }
    public void SetEvent()
    {
        battleSystem.randomData.adventEnemyId = randomData.adventEnemyId;
        battleSystem.randomData.minLv = randomData.minLv;
        battleSystem.randomData.maxLv = randomData.maxLv;
        battleSystem.randomData.setBgm = randomData.setBgm;
        battleSystem.randomData.battleBackground = randomData.battleBackground;
        battleSystem.randomData.maxAdvent = randomData.maxAdvent;
        battleSystem.randomData.maxaddState = randomData.maxaddState;
        battleSystem.randomData.mingetcoin = randomData.mingetcoin;
        battleSystem.randomData.maxgetcoin = randomData.maxgetcoin;
        StartCoroutine(battleSystem.EvBattleStart());
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player"&&isEnter&&(isTrg==-1||(isTrg!=-1&&DataManager.instance.Triggers[isTrg]<1)))
        {
            if (!audioSource.isPlaying) audioSource.PlayOneShot(se[0]);
            Instantiate(Effect[0], col.transform.position, Effect[0].transform.rotation);
            int tmp = (int)Random.Range(0, encountRate);
            if (tmp == 0)
            {
                GManager.instance.Triggers[0] = 0;
                audioSource.PlayOneShot(se[1]);
                Instantiate(Effect[0], col.transform.position, Effect[0].transform.rotation);
                //print("会話開始");
                originalDialogueSystem.SayStart();
            }
        }
    }
}
