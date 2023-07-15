using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using System.IO;
using System;
using UnityEngine.UI;

public class TargetDataDestroy : MonoBehaviour
{
    public bool destroytrg = true;
    public string class_name = "GameNews";
    public string check_name = "スライムディストピア";
    private int query_limit = 199;
    public NCMBObject get_ncmbobj = null;
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
        query.OrderByDescending("gametype");
        //検索件数を設定
        query.Limit = query_limit;
        //データストアでの検索を行う
        int i = 0;

        DateTime tmpdays = GManager.instance.GetGameDay();
        NCMBObject loginobj = new NCMBObject("GameNews");
        loginobj.ObjectId = "YpY9012nWJzXaBuS";
        loginobj.FetchAsync((NCMBException e) => {
             if (e != null)
             {
                 //エラー処理
             }
             else
             {
                //成功時の処理
                long tmpyear = (long)loginobj["targetYear"];
                long tmpmonth = (long)loginobj["targetMonth"];
                long tmpday = (long)loginobj["targetDay"];
                DateTime tmptime = new DateTime((int)tmpyear, (int)tmpmonth, (int)tmpday);
                query.FindAsync((List<NCMBObject> objList, NCMBException r) =>
                {
                    if (r != null)
                    {
                        Destroy(gameObject, 0.2f);
                    }
                    else
                    {
                        //検索成功時の処理
                        foreach (NCMBObject obj in objList)
                        {
                            if (tmpdays != tmptime)
                            {
                                break;
                            }
                            else if (PlayerPrefs.GetString(obj.ObjectId.ToString(), "false") == "false" && (obj["gametype"].ToString() == check_name || obj["gametype"].ToString() == "共通"))
                            {
                                ;
                            }
                            else
                            {
                                Destroy(gameObject, 0.2f);
                                break;
                            }
                            i++;
                        }
                    }
                });
            }
         });
        
    }
    private void Update()
    {
        ;
    }
}
