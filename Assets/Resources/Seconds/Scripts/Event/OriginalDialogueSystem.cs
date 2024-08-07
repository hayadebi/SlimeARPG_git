using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

namespace UniLang
{
    public class OriginalDialogueSystem : MonoBehaviour
    {
        public int eventId = 0;
        public AudioClip se;
        private AudioSource audioSource;

        public Animator SayDialog_CharaImage;
        public Animator DialogueUI;
        public Text CommentText; // 会話内容を表示するUIテキスト
        public Text CharaNameText; // キャラクター名を表示するUIテキスト
        
        private string[] CommentBoxs; // 会話内容を区切る配列
        private string[] Boxs; // 会話内容の各ボックスを区切る配列
        private string[] CharaData; // キャラクターデータを区切る配列

        private int currentIndex; // 現在表示中の会話のインデックス

        public bool isEnter;

        [Header("必要な場合に限り格納")]
        public EncountBattle encountBattle;
        [Header("必要な場合に限り格納")]
        public UniLang.BattleSystem battleSystem;

        private bool saytrg = false;
        private bool eventtrg = false;
        float cooltime = 0f;
        void Start()
        {
            SayDialog_CharaImage = GameObject.Find("SayDialog_CharaImage").GetComponent<Animator>();
            DialogueUI = GameObject.Find("SayDialog").GetComponent<Animator>();
            CommentText = GameObject.Find("SayDialog_Doc").GetComponent<Text>(); // 会話内容を表示するUIテキスト
            CharaNameText = GameObject.Find("SayDialog_Name").GetComponent<Text>(); ; // キャラクター名を表示するUIテキスト
            audioSource = GetComponent<AudioSource>();
            currentIndex = 0;

            // テキストファイルから会話内容を読み込む
            string textContent = DataManager.instance.sayEvent[eventId].Dialogue;
            CommentBoxs = textContent.Split(new string[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        }
        private void Update()
        {
            if (cooltime >= 0) cooltime -= Time.deltaTime;
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!saytrg&&DataManager.instance.Triggers[0]>0&&isEnter&&other.CompareTag("Player")&&cooltime<=0)
            {
                saytrg = true;
                DataManager.instance.Triggers[0] = 0;
                DialogueUI.SetInteger("trg", 1);
                StartCoroutine(ShowNextComment());
            }
        }
        public void SayStart()
        {
            DataManager.instance.Triggers[0] = 0;
            DialogueUI.SetInteger("trg", 1);
            StartCoroutine(ShowNextComment());
        }
        public IEnumerator ShowNextComment()
        {
            currentIndex = 0;

            // テキストファイルから会話内容を読み込む
            string textContent = DataManager.instance.sayEvent[eventId].Dialogue;
            CommentBoxs = textContent.Split(new string[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            DataManager.instance.Triggers[2] = 1;
            if (!eventtrg)
            {
                eventtrg = true;
                foreach (string commentBoxs in CommentBoxs)
                {
                    CommentText.text = "";
                    CharaNameText.text = "";
                    Boxs = commentBoxs.Split(new char[] { '\n' });

                    // キャラクターデータを分割
                    CharaData = Boxs[0].Split(new char[] { ':' });
                    //キャラ画像反映
                    if (CharaData[1]!="-1")
                    {
                        SayDialog_CharaImage.runtimeAnimatorController = DataManager.instance.Pstatus[int.Parse(CharaData[1])].animC;
                        SayDialog_CharaImage.SetInteger("trg", int.Parse(CharaData[2]));
                    }
                    var translator = Translator.Create(Language.Auto, GManager.instance.Languages[GManager.instance.isEnglish]);
                    //翻訳
                    translator.Run(CharaData[0], results =>
                    {
                        foreach (var n in results)
                        {
                        // キャラクター名をUIに表示
                        CharaNameText.text += n.translated;
                        }
                    });
                    translator = Translator.Create(Language.Auto, GManager.instance.Languages[GManager.instance.isEnglish]);
                   // bool isTranslationDone = false;
                    for (int i = 1; i < Boxs.Length; i++)
                    {
                        audioSource.PlayOneShot(se);
                        if (battleSystem) battleSystem.audioSource[1].PlayOneShot(se);
                        //翻訳
                        translator.Run(Boxs[i], results =>
                        {
                            foreach (var n in results)
                            {
                                StartCoroutine(TypeText(n.translated));
                            }
                            
                            //isTranslationDone = true;
                        });
                        yield return new WaitForSeconds(1f);
                        //yield return new WaitUntil(() => isTranslationDone);
                        audioSource.Stop();
                        if (battleSystem) battleSystem.audioSource[1].Stop();
                        //CommentText.text += "\n"; // 改行を挿入
                        // クリックを待つ
                        yield return WaitForClick();
                       
                    }

                    currentIndex++;
                }
                // 会話終了時の処理（ここではUIをクリア）
                
                yield return new WaitForSeconds(0.3f);
                yield return WaitForClick();
                CommentText.text = "";
                CharaNameText.text = "";
                //DialogueUI.SetInteger("trg", 0);
                DialogueUI.SetInteger("trg", 0);
                yield return new WaitForSeconds(0.3f);
                cooltime = 2;
                if(CharaData[3]!="3") DataManager.instance.Triggers[0] = 1;
                saytrg = false;
                eventtrg = false;
                if (CharaData[3] == "1" && encountBattle) encountBattle.SetSystem();
                else if (CharaData[3] == "2" && encountBattle) encountBattle.SetEvent();
            }
            DataManager.instance.Triggers[2] = 0;
            IEnumerator TypeText(string text)
            {
                text += "\n";
                string[] boxs = text.ToCharArray().Select(c => c.ToString()).ToArray();
                for (int l = 0; l < boxs.Length; l++)
                {
                    
                    CommentText.text += boxs[l];
                    yield return new WaitForSeconds(0.05f);
                    
                }
            }
            IEnumerator WaitForClick()
            {
                // 左クリックまたはEnterキーが押されるまで待つ
                yield return new WaitUntil(() => (Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z)));
            }
        }
    }

}
