using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AchiView : MonoBehaviour
{
    [Header("生成場所")]
    public Transform Content;
    [Header("オリジナルの実績オブジェクト")]
    public ContentAchi achiObject;
    [Header("空白オブジェクト")]
    public GameObject br;
    [Header("コンプリート率")]
    public Text[] completeText;

    private List<GameObject> achiList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (achiList.Count > 0)
        {
            foreach(GameObject obj in achiList)
            {
                Destroy(obj.gameObject);
            }
            achiList = null;
            achiList = new List<GameObject>();
            
        }
        int completeCount = 0;
        for (int i = 0; i < DataManager.instance.achievementsID.Count;)
        {
            if (DataManager.instance.achievementsID[i].gettrg > 0)
            {
                completeCount += 1;
                achiObject.achiId = i;
                GameObject tmpobj = Instantiate(achiObject.gameObject, Content.position, Content.rotation, Content);
                tmpobj.SetActive(true);
                achiList.Add(tmpobj);
            }
            i++;
        }
        GameObject tmpbr = Instantiate(br, Content.position, Content.rotation, Content);
        tmpbr.SetActive(true);
        achiList.Add(tmpbr);
        foreach (Text _text in completeText)
        {
            _text.text = completeCount.ToString() + "/" + DataManager.instance.achievementsID.Count.ToString();
        }
    }
}
