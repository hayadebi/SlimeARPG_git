using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System.IO;
using System;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    public bool destroytrg = true;
    public string class_name = "GameNews";
    public string check_name = "スライムディストピア";
    private int query_limit = 99;
    public NCMBObject get_ncmbobj = null;
    public int[] getcoin_array = { 5, 10, 20, 40, 80, 120, 160 };
    public int[] getitemid_array = { 0, 1, 2, 3, 10, 20, 19,-1 };
    public GameObject setuiobj;
    // Start is called before the first frame update
    void Start()
    {
        FetchStage();
    }
    public void FetchStage()
    {
        //ユーザーチェック
        NCMBQuery<NCMBObject> query = null;
        query = new NCMBQuery<NCMBObject>(class_name);
        query.OrderByDescending(check_name);
        //検索件数を設定
        query.Limit = query_limit;
        //データストアでの検索を行う
        int i = 0;
        DateTime tmpdays = GManager.instance.GetGameDay();
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            if (e != null)
            {
                Destroy(gameObject);
            }
            else
            {
                //検索成功時の処理
                foreach (NCMBObject obj in objList)
                {
                    if ((bool)obj["isdx"]==false ||((bool)obj["isdx"] == true && GManager.instance.dxtrg))
                    {
                        if (obj.ObjectId == "YpY9012nWJzXaBuS" && PlayerPrefs.GetString(obj.ObjectId, "false") == "false")
                        {
                            NCMBObject loginobj = obj;
                            loginobj["messagetitle"] = "【" + tmpdays.Month.ToString() + "/" + tmpdays.Day + " ログインボーナス】創造神からの祝福。";
                            obj["targetYear"] = tmpdays.Year;
                            obj["targetMonth"] = tmpdays.Month;
                            obj["targetDay"] = tmpdays.Day;
                            int get_itemid = getitemid_array[UnityEngine.Random.Range(0, getitemid_array.Length)];
                            string tmpitemname = "";
                            if (get_itemid != -1) tmpitemname = GManager.instance.ItemID[get_itemid].itemname;
                            else tmpitemname = "無し";
                            int get_coin = getcoin_array[UnityEngine.Random.Range(0, getcoin_array.Length)];
                            obj["additemid"] = (long)get_itemid;
                            obj["addnormalcoin"] = (long)get_coin;
                            obj["messagedoc"] = "本日のログインボーナス！\n \n+獲得通常コイン："+get_coin+"G\n+獲得アイテム："+tmpitemname+" 1個";
                            obj.SaveAsync();


                            GManager.instance.tmpchildobj = obj.ObjectId;
                            GameObject tmpobj = Instantiate(setuiobj, transform.position, transform.rotation, transform);
                            ChildMail tmpchild = tmpobj.GetComponent<ChildMail>();
                            long tmpitemnum = (long)obj["additemid"];
                            long tmpcoinnum = (long)obj["addnormalcoin"];
                            if (((int)tmpitemnum != -1 || (int)tmpcoinnum > 0) && PlayerPrefs.GetString(obj.ObjectId.ToString(), "false") == "false")
                            {
                                tmpchild.bonus_trg = true;
                            }
                            else tmpchild.bonus_trg = false;
                            tmpchild.movetrg = true;
                            tmpchild.parenttrg = false;
                        }
                        else
                        {
                            GManager.instance.tmpchildobj = obj.ObjectId;
                            GameObject tmpobj = Instantiate(setuiobj, transform.position, transform.rotation, transform);
                            ChildMail tmpchild = tmpobj.GetComponent<ChildMail>();
                            long tmpitemnum = (long)obj["additemid"];
                            long tmpcoinnum = (long)obj["addnormalcoin"];
                            if (((int)tmpitemnum != -1 || (int)tmpcoinnum > 0) && PlayerPrefs.GetString(obj.ObjectId.ToString(), "false") == "false")
                            {
                                tmpchild.bonus_trg = true;
                            }
                            else tmpchild.bonus_trg = false;
                            tmpchild.movetrg = true;
                            tmpchild.parenttrg = false;

                        }
                    }
                    i++;
                }
                setuiobj.SetActive(false);
            }
        });
    }
    private void Update()
    {
        ;
    }
}
