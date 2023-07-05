using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayorEvcheck : MonoBehaviour
{
    public bool ischeck_or_notset = true;
    public bool isday_or_notev = true;
    public int targetev_id=20;
    public int targetev_num = 0;//true>target_num 超えたら
    private int check_num = 0;
    private float cooltime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (ischeck_or_notset && isday_or_notev)
        {
            check_num= PlayerPrefs.GetInt("DayAds", 0);
            if (check_num >= 5) Destroy(gameObject);
        }
        else if(ischeck_or_notset && !isday_or_notev && GManager.instance.EventNumber[targetev_id] > targetev_num) Destroy(gameObject); 
        else if (!ischeck_or_notset && GManager.instance.EventNumber[targetev_id] <= targetev_num)
        {
            GManager.instance.EventNumber[targetev_id] = targetev_num + 1;
        }
        else if (!ischeck_or_notset && GManager.instance.EventNumber[targetev_id] > targetev_num)
        {
            Destroy(gameObject);
        }
    }
}
