using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//追加する！
using UnityEngine.UI;
using System;
using NCMB;

public class InputSpell : MonoBehaviour
{

    //オブジェクトと結びつける
    public InputField inputField;
    public GameObject start_bt;
    private float isenter = 0f;
    void Start()
    {
        //Componentを扱えるようにする
        inputField = inputField.GetComponent<InputField>();
    }
    private void Update()
    {
        if (isenter >=0) isenter -= Time.deltaTime;
    }
    void DataDelete(NCMBObject tmp)
    {
        tmp.DeleteAsync((NCMBException e) => {
            if (e != null)
            {
                //エラー処理
            }
            else
            {
                //成功時の処理
            }
        });
    }
    public void EnterSpell()
    {
        if (isenter <= 0 && inputField.text != "")
        {
            isenter = 4f;
            //テキストにinputFieldの内容を反映
            if (inputField.text == "そうぞうしんからのおくりもの" && PlayerPrefs.GetInt("gmtrg" + 179, GManager.instance.Triggers[179]) < 1)
            {
                inputField.text = "";
                GManager.instance.Triggers[179] = 1;
                if (GManager.instance.isEnglish == 0)
                    GManager.instance.temp_text[0] = "魔法の呪文が成功して、\n\n■アイテム：\"感情\"と\"理想\"の黄金像　1個\n■クラフトレシピ：デビ・コーラ の作り方\n\nを手に入れた！";
                else
                    GManager.instance.temp_text[0] = "The magic spell succeeded and I got a\n\n■Item:golden statue of \"emotion\" and \"ideal\".\n■Recipe:instructions on how to make Devi Cola.";
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
            else if (inputField.text == "からくりめいど")
            {
                inputField.text = "";
                if (GManager.instance.isEnglish == 0)
                    GManager.instance.temp_text[0] = "全データが削除されました。";
                else
                    GManager.instance.temp_text[0] = "All data has been deleted.";
                //----------------------------------
                PlayerPrefs.DeleteAll();
                //----------------------------------
                Instantiate(GManager.instance.effectobj[24], transform.position, transform.rotation);
                start_bt.SetActive(false);
                GManager.instance.setrg = 27;
            }
            else if (inputField.text.Contains("ふるーる"))
            {
                NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("SpellCode");
                //Scoreの値が7と一致するオブジェクト検索
                query.WhereEqualTo("spell", inputField.text);
                query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
                {
                    if (e != null)
                    {
                        //検索失敗時の処理
                        inputField.text = "";
                        if (GManager.instance.isEnglish == 0)
                            GManager.instance.temp_text[0] = "魔法の呪文に失敗した！";
                        else
                            GManager.instance.temp_text[0] = "Magic spell failed!";
                        //----------------------------------
                        Instantiate(GManager.instance.effectobj[24], transform.position, transform.rotation);
                        GManager.instance.setrg = 2;
                    }
                    else if (PlayerPrefs.GetInt("gmtrg" + 181, GManager.instance.Triggers[181]) < 1)
                    {
                        foreach (NCMBObject obj in objList)
                        {
                            obj.DeleteAsync();
                            obj.SaveAsync();
                            //------------------
                            //成功
                            inputField.text = "";
                            GManager.instance.Triggers[181] = 1;
                            if (GManager.instance.isEnglish == 0)
                                GManager.instance.temp_text[0] = "魔法の呪文が成功して、\n\n■アイテム：\"友情の証\"ツボミンのしおり　1個\n■クラフトレシピ：バールのようなもの の作り方\n\nを手に入れた！";
                            else
                                GManager.instance.temp_text[0] = "The magic spell succeeded and I got a\n\n■Item:One The bookmark of \"Proof of Friendship\" Tsubomin.\n■Recipe:How to make something like a crowbar.";
                            //----------------------------------
                            GManager.instance.ItemID[64].gettrg = 1;
                            GManager.instance.ItemID[64].itemnumber += 1;
                            GManager.instance._craftRecipe[33].get_recipe = 1;

                            PlayerPrefs.SetInt("gmtrg" + 181, GManager.instance.Triggers[181]);
                            PlayerPrefs.SetInt("get_recipe" + 33, GManager.instance._craftRecipe[33].get_recipe);
                            PlayerPrefs.SetInt("itemnumber" + 64, GManager.instance.ItemID[64].itemnumber);
                            PlayerPrefs.SetInt("itemget" + 64, GManager.instance.ItemID[64].gettrg);

                            PlayerPrefs.Save();
                            //----------------------------------
                            Instantiate(GManager.instance.effectobj[24], transform.position, transform.rotation);
                            start_bt.SetActive(false);
                            GManager.instance.setrg = 12;
                            //------------------
                            break;
                        }
                    }
                });
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
        else
        {
            GManager.instance.setrg = 2;
        }
    }

}