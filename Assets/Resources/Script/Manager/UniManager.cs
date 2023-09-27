using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace UniLang
{
    public class UniManager : MonoBehaviour
    {
        private Text thistext;
        private string oldtext;
        private bool converttrg = false;
        public int fontsized = -1;
        private int old_fontsize = 0;
        // Start is called before the first frame update
        void Start()
        {
            thistext = this.GetComponent<Text>();
            old_fontsize = thistext.fontSize;
            oldtext = thistext.text;
            converttrg = true;
            if(GManager.instance.isEnglish != 0) Invoke(nameof(LangConvert), 0.08f);
        }

        // Update is called once per frame
        void Update()
        {
            if (thistext.text != oldtext && !converttrg && GManager.instance.isEnglish != 0)
            {
                converttrg = true;
                oldtext = thistext.text;
                if(fontsized!=-1)thistext.fontSize = fontsized;
                LangConvert();
            }
            else if (GManager.instance.isEnglish == 0 && thistext.fontSize != old_fontsize) thistext.fontSize = old_fontsize;
        }

        void LangConvert()
        {
            string[] tmparrey = oldtext.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            thistext.text = "";
            oldtext = "";
            for (int i = 0; i < tmparrey.Length;)
            {
                var translator = Translator.Create(Language.Auto, Language.English);
                translator.Run(tmparrey[i], results =>
                {
                    foreach (var n in results)
                    {
                        thistext.text += n.translated +"\n";
                        oldtext += n.translated+ "\n";
                    }
                    thistext.text.TrimEnd('\r', '\n');
                    oldtext.TrimEnd('\r', '\n');
                });
                i++;
            }
            converttrg = false;
        }
    }
}
