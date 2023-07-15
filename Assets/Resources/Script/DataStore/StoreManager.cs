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
    private string address_num = " ";
    public string coin_name = "adsCoin";
    private int query_limit = 200;
    public InputField get_addressfield;
    public Text get_devcoinviewtext;
    private bool CheckEnable = false;
    private bool onuser = false;
    public NCMBObject get_ncmbobj=null;
    // Start is called before the first frame update
    void Start()
    {
        get_addressfield.text = PlayerPrefs.GetString("MpurseAddress", "");
        if (get_addressfield.text != "")
        {
            FetchStage();
        }
        else
        {
            if (GManager.instance.isEnglish == 0) get_devcoinviewtext.text = "所持デビコイン：0.0";
            else if (GManager.instance.isEnglish != 0) get_devcoinviewtext.text = "Devil coins you have：0.0";
        }
    }
    public void CheckUser()
    {
        string tmp = "https://unitygamehayadebi.jimdofree.com/devusercheck/#";
        Application.OpenURL(tmp);
    }
    public void FetchStage()
    {
        //ユーザーチェック
        NCMBQuery<NCMBObject> query = null;
        query = new NCMBQuery<NCMBObject>(check_name);
        query.OrderByDescending(get_addressfield.text);
        //検索件数を設定
        query.Limit = 4;
        int i = 1;
        if (!GManager.instance.mpurseuser_on) onuser = false;
        //データストアでの検索を行う
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                if (!GManager.instance.mpurseuser_on)
                {
                    if (GManager.instance.isEnglish == 0) get_addressfield.text = "本人確認に失敗しました。";
                    else if (GManager.instance.isEnglish != 0) get_addressfield.text = "Identification failed.";
                    GManager.instance.setrg = 27;
                }
            }
            else
            {
                onuser = true;
                GManager.instance.mpurseuser_on = true;
                //検索成功時の処理
                foreach (NCMBObject obj in objList)
                {
                    break;
                }
            }
        });
        Resources.UnloadUnusedAssets();
        CheckEnable = false;
    }
    private void Update()
    {
        if (onuser)
        {
            onuser = false;
            //保存ムーブ
            if (!GManager.instance.mpurseuser_on)
            {
                PlayerPrefs.SetString("MpurseAddress", get_addressfield.text);
                PlayerPrefs.Save();
            }
            address_num = PlayerPrefs.GetString("MpurseAddress", get_addressfield.text);
            NCMBQuery<NCMBObject> query2 = null;
            //取得から配置
            query2 = null;
            query2 = new NCMBQuery<NCMBObject>(class_name);
            //Scoreフィールドの降順でデータを取得
            query2.OrderByDescending("mpurseAddress");
            //検索件数を設定
            query2.Limit = query_limit;
            int i = 1;
            //データストアでの検索を行う
            query2.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                if (e != null)
                {
                    //検索失敗時の処理
                }
                else
                {
                    //検索成功時の処理
                    foreach (NCMBObject obj in objList)
                    {
                        if (obj["mpurseAddress"].ToString() == address_num)//
                        {
                            if (get_ncmbobj == null) get_ncmbobj = obj;
                            var tmpnum = obj[coin_name].ToString();
                            GManager.instance.get_devcoin = float.Parse(tmpnum);
                            break;
                        }
                    }
                    if (GManager.instance.isEnglish == 0) get_devcoinviewtext.text = "所持デビコイン：" + GManager.instance.get_devcoin.ToString();
                    else if (GManager.instance.isEnglish != 0) get_devcoinviewtext.text = "Devil coins you have：" + GManager.instance.get_devcoin.ToString();
                }
            });
        }
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            //アプリが一時停止(バックグラウンドに行った)
        }
        else if(!CheckEnable)
        {
            CheckEnable = true;
            //アプリが再開(バックグラウンドから戻った)
            FetchStage();
        }
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && !CheckEnable)
        {
            CheckEnable = true;
            //アプリが選択された(バックグラウンドから戻った)
            FetchStage();
        }
        else
        {
            //アプリが選択されなくなった(バックグラウンドに行った)
        }
    }
    public void BuyAddData(float setf=0f)
    {
        if (get_ncmbobj != null)
        {
            var tmptext = get_ncmbobj[coin_name].ToString();
            var tmpnum = float.Parse(tmptext);
            GManager.instance.get_devcoin = tmpnum + setf;
            if (GManager.instance.isEnglish == 0) get_devcoinviewtext.text = "所持デビコイン：" + GManager.instance.get_devcoin.ToString();
            else if (GManager.instance.isEnglish != 0) get_devcoinviewtext.text = "Devil coins you have：" + GManager.instance.get_devcoin.ToString();
            NCMBObject obj = get_ncmbobj;
            print((tmpnum + setf).ToString());
            obj[coin_name] = tmpnum + setf;
            //obj.Increment("adsCoin", -setf);
            obj.SaveAsync();
        }
    }
}
