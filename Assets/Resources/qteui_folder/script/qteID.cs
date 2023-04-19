using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class qteID : MonoBehaviour
{
    [Header("ボタンID")]
    public int button_id;
    [Header("ボタンのテキストをセット")]
    public Text bt_text;
    [Header("ボタンのアニメーションをセット")]
    public Animator bt_anim;

    qteUI _q;
    // Start is called before the first frame update
    void Start()
    {
        _q = GameObject.Find("qteUI").GetComponent<qteUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pushClick()
    {
        _q.clickQTE();
    }
}
