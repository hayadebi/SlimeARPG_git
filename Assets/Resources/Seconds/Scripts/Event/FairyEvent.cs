using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyEvent : MonoBehaviour
{
    [Header("プレイヤー位置")]
    public Transform playerPosition;
    private Transform targetPosition;
    [Header("フェード")]
    public GameObject fadein;
    public GameObject fadeout;
    private bool isMove = false;
    private float times = 60f;
    [Header("対象オブジェクトBからの指定距離")]
    public float boundaryRadius = 5f; // 対象オブジェクトBからの指定距離
    [Header("追尾オブジェクトAの移動速度")]
    public float moveSpeed = 2f; // 追尾オブジェクトAの移動速度
    private float cooltime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        targetPosition = null;
        times = 60f;
        isMove = true;
    }
    private IEnumerator OnEnd()
    {
        Instantiate(fadein, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        DataManager.instance.TextGet = "瓶から出した精霊が力尽きた…！";
        Instantiate(fadeout, transform.position, transform.rotation);
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            if(times>=0)times -= Time.deltaTime;
            if (!targetPosition && cooltime <= 0f && GameObject.FindGameObjectsWithTag("TargetPoint") != null && GameObject.FindGameObjectsWithTag("TargetPoint").Length > 0) targetPosition = GameObject.FindGameObjectsWithTag("TargetPoint")[0].transform;
            else if (!targetPosition && cooltime <= 0) cooltime = 1f;
            if (cooltime >= 0) cooltime -= Time.deltaTime;

            if (targetPosition)
            {
                // 追尾オブジェクトAから対象オブジェクトAへの方向を計算
                Vector3 directionToTarget = (targetPosition.position - this.gameObject.transform.position).normalized;

                // 追尾オブジェクトAが対象オブジェクトAに向かって移動
                Vector3 newPosition = this.gameObject.transform.position + directionToTarget * moveSpeed * Time.deltaTime;

                // 対象オブジェクトBからの距離を計算
                float distanceToBoundaryCenter = Vector3.Distance(playerPosition.position, newPosition);

                // 新しい位置が範囲内かどうかをチェック
                if (distanceToBoundaryCenter <= boundaryRadius)
                {
                    // 範囲内なら追尾オブジェクトAを新しい位置に移動
                    this.gameObject.transform.position = newPosition;
                }
                else
                {
                    // 範囲外なら追尾オブジェクトBからの指定距離の境界に沿って移動
                    Vector3 directionToBoundaryCenter = (newPosition - playerPosition.position).normalized;
                    this.gameObject.transform.position = playerPosition.position + directionToBoundaryCenter * boundaryRadius;
                }
            }
            if (times <= 0)
            {
                isMove = false;
                StartCoroutine(OnEnd());
            }
        }
    }
}
