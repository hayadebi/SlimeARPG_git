using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace UniLang
{
    public class StageNameView : MonoBehaviour
    {
        [SerializeField, Header("先頭文字")]
        public string leadText = "";
        [SerializeField, Header("末端文字")]
        public string endText = "";
        private Text text = null;
        private string stagenametext;
        private string stagetypetext;
        // Start is called before the first frame update
        void Start()
        {
            if (GetComponent<Text>()) text = GetComponent<Text>();
            if (text)
            {
                StartCoroutine(StartUI());
            }
        }

          IEnumerator StartUI()
        {
            text.text = leadText;
            stagenametext = DataManager.instance.stageData[DataManager.instance.stageNumber].name;
            switch (DataManager.instance.stageData[DataManager.instance.stageNumber].stageType)
            {
                case DataManager.StageType.Field:
                    stagetypetext = "フィールド";
                    break;
                case DataManager.StageType.Safe:
                    stagetypetext = "セーフゾーン";
                    break;
                case DataManager.StageType.Dungeon:
                    stagetypetext = "ダンジョン";
                    break;
                default:
                    stagetypetext = "フィールド";
                    break;
            }

            var translator = Translator.Create(Language.Auto, GManager.instance.Languages[GManager.instance.isEnglish]);
            //翻訳
            translator.Run(stagetypetext, results =>
            {
                foreach (var n in results)
                {
                    text.text += n.translated + ":";
                }
            });
            yield return new WaitForSeconds(0.3f);
            //翻訳
            translator.Run(stagenametext, results =>
            {
                foreach (var n in results)
                {
                    text.text += n.translated + endText;
                }
            });
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
