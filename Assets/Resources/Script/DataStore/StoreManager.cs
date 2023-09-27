
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System.IO;
using System;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public string class_name = "AdsClass";
    public string check_name = "UserCheck";
    public string coin_name = "adsCoin";
    private int query_limit = 200;
    public InputField get_username;
    public InputField get_userpass;
    public InputField get_repass;
    public Text get_devcoinviewtext;
    public Text loginstatus_text;
    public NCMBObject get_ncmbobj = null;
    public NCMBUser get_ncmbuser = null;
    public clickbutton cbtn;
    private string tmp_name = "";
    public bool onuser = false;
    // Start is called before the first frame update
    void Start()
    {
        GManager.instance.mpurseuser_on = false;
        if (GManager.instance.isEnglish == 0) get_devcoinviewtext.text = "所持デビコイン：0.0";
        else if (GManager.instance.isEnglish != 0) get_devcoinviewtext.text = "Devil coins you have：0.0";

        NCMBUser currentUser = NCMBUser.CurrentUser;
        if (currentUser != null)
        {
            tmp_name= currentUser.UserName;
            GManager.instance.mpurseuser_on = true;
            loginstatus_text.text = "ログイン中";
            onuser = false;
            DevNumCheck();
        }
    }
    //public void CheckUser()
    //{
    //    string tmp = "https://unitygamehayadebi.jimdofree.com/devusercheck/#";
    //    Application.OpenURL(tmp);
    //}
    private void Update()
    {
        if (onuser)
        {
            onuser = false;
            DevNumCheck();
        }
    }
    public void FetchStage()
    {
        //ユーザー登録
        if (get_username.text != "" && get_userpass.text != "" && get_userpass.text == get_repass.text)//(get_ncmbuser != null||!GManager.instance.mpurseuser_on||!onuser) &&
        {
            get_ncmbuser = new NCMBUser();
            // ユーザー名・パスワードを設定
            tmp_name = get_username.text;
            get_ncmbuser.UserName = get_username.text; /* ユーザー名 */
            get_ncmbuser.Password = get_userpass.text; /* パスワード */
            // ユーザーの新規登録処理
            get_ncmbuser.SignUpAsync((NCMBException e) =>
            {
                if (e != null)
                {
                    //新規登録失敗、ログインに移る
                    NCMBUser.LogInAsync(get_username.text, get_userpass.text, (NCMBException _e) =>
                    {
                        if (_e != null)
                        {

                            NCMBUser currentUser = NCMBUser.CurrentUser;
                            if (currentUser != null)
                            {
                                tmp_name = currentUser.UserName;
                                GManager.instance.mpurseuser_on = true;
                                GManager.instance.setrg = 6;
                                loginstatus_text.text = "ログイン中";
                                DevNumCheck();
                            }
                            else
                            {
                                GManager.instance.setrg = 27;
                            }
                        }
                        else
                        {
                            //UnityEngine.Debug.Log("ログインに成功！");
                            GManager.instance.mpurseuser_on = true;
                            //FetchDev();
                            GManager.instance.setrg = 6;
                            loginstatus_text.text = "ログイン中";
                            DevNumCheck();

                        }
                    });
                }
                else
                {
                    //UnityEngine.Debug.Log("ユーザーの新規登録に成功");
                    GManager.instance.setrg = 6;
                    loginstatus_text.text = "ログイン中";
                    AdsAdd();
                }
            });
        }
        else
        {
            GManager.instance.setrg = 27;
            Resources.UnloadUnusedAssets();
        }

    }
    void DevNumCheck()
    {
        if (tmp_name != "")
        {
            NCMBQuery<NCMBObject> query2 = new NCMBQuery<NCMBObject>(class_name);
            //Scoreフィールドの降順でデータを取得
            query2.WhereEqualTo("mpurseAddress", tmp_name);
            query2.Limit = query_limit;
            //検索件数を設定
            //データストアでの検索を行う
            query2.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                if (e != null)
                {
                    //検索失敗時の処理
                    GManager.instance.setrg = 27;
                    AdsAdd();
                }
                else
                {
                    //検索成功時の処理
                    foreach (NCMBObject obj in objList)
                    {
                        if (get_ncmbobj == null) get_ncmbobj = obj;
                        var tmpnum = obj[coin_name].ToString();
                        GManager.instance.get_devcoin = float.Parse(tmpnum);
                        var getminidev = PlayerPrefs.GetFloat("getdc", 0);
                        PlayerPrefs.SetFloat("getdc", 0);
                        PlayerPrefs.Save();
                        BuyAddData(getminidev);
                        break;
                    }
                    if (GManager.instance.isEnglish == 0) get_devcoinviewtext.text = "所持デビコイン：" + GManager.instance.get_devcoin.ToString();
                    else if (GManager.instance.isEnglish != 0) get_devcoinviewtext.text = "Devil coins you have：" + GManager.instance.get_devcoin.ToString();

                }
            });
            cbtn.CheckNoView();
        }
    }
    void AdsAdd()
    {
        NCMBObject obj = new NCMBObject(class_name);
        obj.Add("mpurseAddress", tmp_name);
        obj.Add(coin_name, 0);
        obj.SaveAsync();
        GManager.instance.mpurseuser_on = true;
        DevNumCheck();
    }
    public void BuyAddData(float setf = 0f)
    {
        if (get_ncmbobj != null)
        {
            var tmptext = get_ncmbobj[coin_name].ToString();
            var tmpnum = float.Parse(tmptext);
            GManager.instance.get_devcoin = tmpnum + setf;
            if (GManager.instance.isEnglish == 0) get_devcoinviewtext.text = "所持デビコイン：" + GManager.instance.get_devcoin.ToString();
            else if (GManager.instance.isEnglish != 0) get_devcoinviewtext.text = "Devil coins you have：" + GManager.instance.get_devcoin.ToString();
            NCMBObject obj = get_ncmbobj;
            obj[coin_name] = tmpnum + setf;
            //obj.Increment("adsCoin", -setf);
            obj.SaveAsync();
        }
    }
}

