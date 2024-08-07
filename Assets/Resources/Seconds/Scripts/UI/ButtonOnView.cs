using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnView : MonoBehaviour
{
    [SerializeField,Header("オーディオソース格納")]
    private AudioSource audioSources;
    [SerializeField, Header("クリック時の効果音格納")]
    private AudioClip se;
    [SerializeField, Header("表示するオブジェクト格納")]
    private GameObject OnObject;
    [SerializeField, Header("非表示にするオブジェクト格納")]
    private GameObject OffObject;

    // Start is called before the first frame update
    void Start()
    {
        ;
    }
    public void OnViewObject()
    {
        if(audioSources)audioSources.PlayOneShot(se);
        OnObject.SetActive(true);
        OffObject.SetActive(false);
    }
}
