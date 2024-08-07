using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlimeList : MonoBehaviour
{
    [Header("生成場所")]
    public Transform Content;
    [Header("オリジナルの実績オブジェクト")]
    public SlimeContent slimeObject;
    [Header("空白オブジェクト")]
    public GameObject br;

    private List<GameObject> slimeList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        if (slimeList.Count > 0)
        {
            foreach (GameObject obj in slimeList)
            {
                Destroy(obj.gameObject);
            }
            slimeList = null;
            slimeList = new List<GameObject>();

        }
        for (int i = 0; i < DataManager.instance.Pstatus.Count;)
        {
            if (DataManager.instance.Pstatus[i].getpl > 0&&DataManager.instance.playerselect[0]!=i&& DataManager.instance.playerselect[1] != i&& DataManager.instance.playerselect[2] != i)
            {
                slimeObject.slimeId = i;
                GameObject tmpobj = Instantiate(slimeObject.gameObject, Content.position, Content.rotation, Content);
                tmpobj.SetActive(true);
                slimeList.Add(tmpobj);
            }
            i++;
        }
    }
}
