using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//追加する！
using UnityEngine.UI;

public class InputSpell : MonoBehaviour
{

    //オブジェクトと結びつける
    public InputField inputField;
    public GameObject start_bt;
    void Start()
    {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<InputField>();
    }

    public void EnterSpell()
    {
        //テキストにinputFieldの内容を反映
        if (inputField.text == "そうぞうしんからのおくりもの" && PlayerPrefs.GetInt("gmtrg" + 179, GManager.instance.Triggers[179])<1)
        {
            inputField.text = "";
            GManager.instance.Triggers[179] = 1;
            if(GManager.instance.isEnglish == 0)
                GManager.instance.temp_text[0] = "魔法の呪文が成功して、\n\n■アイテム：\"感情\"と\"理想\"の黄金像　1個\n■クラフトレシピ：デビ・コーラの作り方\n\nを手に入れた！";
            else
                GManager.instance.temp_text[0] = "The magic spell succeeded and I got a\n\n■golden statue of \"emotion\" and \"ideal\".\n■instructions on how to make Devi Cola.";
            //----------------------------------
            GManager.instance.ItemID[59].gettrg = 1;
            GManager.instance.ItemID[59].itemnumber += 1;
            GManager.instance._craftRecipe[32].get_recipe = 1;

            PlayerPrefs.SetInt("gmtrg" + 179, GManager.instance.Triggers[179]);
            PlayerPrefs.SetInt("get_recipe" + 32, GManager.instance._craftRecipe[32].get_recipe);
            PlayerPrefs.SetInt("itemnumber" + 59, GManager.instance.ItemID[59].itemnumber);
            PlayerPrefs.SetInt("itemget" + 59, GManager.instance.ItemID[59].gettrg);

            PlayerPrefs.Save();
            //----------------------------------
            Instantiate(GManager.instance.effectobj[24], transform.position, transform.rotation);
            start_bt.SetActive(false);
            GManager.instance.setrg = 12;
        }
        else
        {
            inputField.text = "";
            if (GManager.instance.isEnglish == 0)
                GManager.instance.temp_text[0] = "魔法の呪文に失敗した！";
            else
                GManager.instance.temp_text[0] = "Magic spell failed!";
            //----------------------------------
            Instantiate(GManager.instance.effectobj[24], transform.position, transform.rotation);
            GManager.instance.setrg = 2;
        }
    }

}