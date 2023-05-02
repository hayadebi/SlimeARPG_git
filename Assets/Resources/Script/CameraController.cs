using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool zoomtrg = true;
    public bool enablecamera = true;
    //　キャラクターのTransform
    [SerializeField]
    private Transform charaLookAtPosition;
    //　カメラの移動スピード
    [SerializeField]
    private float cameraMoveSpeed = 2f;
    //　カメラの回転スピード
    // 障害物とするレイヤー
    [SerializeField]
    private LayerMask obstacleLayer;
    Vector3 defaultPos;
    Vector3 dfP;
    public player pl;

    [SerializeField]
    private bool hittrg = false;
    private bool stoptrg = false;
    private float stoptime = 0;
    GameObject cpos;
    GameObject raypos;
    GameObject dfpos;
    //BoxCollider bc;
    public Camera charatrg = null;
    private Camera thiscm = null;
    private void Awake()
    {
        cameraMoveSpeed = 4;
        cpos = GameObject.Find("cmpos");
        raypos = GameObject.Find("raypos");
        dfpos = GameObject.Find("dfpos");//this.transform.position - pl.oldjumpP;
        if (charatrg != null)
            thiscm = this.GetComponent<Camera>();
    }
    void LateUpdate()
    {
        if (charatrg==null && GManager.instance.walktrg && charaLookAtPosition != null && enablecamera && cpos != null && raypos != null && GManager.instance.Triggers[103] == 0)
        {
            if(!Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q))
                {
                    this.transform.eulerAngles = dfpos.transform.eulerAngles;
                    this.transform.position = dfpos.transform.position;
                }
            }
            if (stoptrg)
            {
                stoptime += Time.deltaTime;
                if (stoptime > 2f)
                {
                    stoptime = 0;
                    stoptrg = false;
                }
            }
            //　カメラの位置をキャラクターの後ろ側に移動させる
            if (!hittrg && !stoptrg)
            {
                transform.position = Vector3.Lerp(transform.position, dfpos.transform.position , cameraMoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, dfpos.transform.position, cameraMoveSpeed * Time.deltaTime);
            }

            RaycastHit hit;
            //　キャラクターとカメラの間に障害物があったら障害物の位置にカメラを移動させる
            if (Physics.Linecast(raypos.transform.position, transform.position, out hit, obstacleLayer))
            {
                if (!hittrg)
                {
                    stoptrg = true;
                    hittrg = true;
                }
            }
            else
            {
                if (hittrg && !stoptrg)
                {

                    hittrg = false;
                }
            }
        }
        else if(charatrg!=null&& charatrg.fieldOfView !=thiscm.fieldOfView)
        {
            thiscm.fieldOfView = charatrg.fieldOfView;
        }
    }

}