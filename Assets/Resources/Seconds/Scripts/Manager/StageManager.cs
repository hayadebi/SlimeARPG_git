using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StageManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerPos;
    public GameObject fadeIn;
    public AudioSource BGM;
    void Start()
    {
        //ロードセーブ
        DataManager.instance.AllLoadInvoke();
        DataManager.instance.AllSaveInvoke();
       // print(PlayerPrefs.GetString("AllData", ""));
        //生成
        Instantiate(DataManager.instance.stageData[DataManager.instance.stageNumber].stageObject, transform.position, transform.rotation, transform);
        //プレイヤー位置設定
        playerPos.position = DataManager.instance.playerStartPos;
        BGM.Stop();
        BGM.clip = DataManager.instance.stageData[DataManager.instance.stageNumber].defaultBGM;
        BGM.loop = true;
        BGM.Play();
    }

    public IEnumerator NextScene(int targetsceneId,Vector3 targetPosition)
    {
        DataManager.instance.stageNumber = targetsceneId;
        DataManager.instance.playerStartPos = targetPosition;
        DataManager.instance.AllSaveInvoke();
        Instantiate(fadeIn, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(DataManager.instance.stageData[DataManager.instance.stageNumber].realname);
    }
}
