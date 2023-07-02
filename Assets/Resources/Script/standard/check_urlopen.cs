using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check_urlopen : MonoBehaviour
{
    public GameObject onoffobj;
    public string seturl;
    private AudioSource _audio=null;
    [Header("AudioSources必須")]
    public AudioClip se;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<AudioSource>()) _audio = GetComponent<AudioSource>();
        onoffobj.SetActive(false);
    }

    public void CheckOnView()
    {
        if (_audio != null) _audio.PlayOneShot(se);
        onoffobj.SetActive(true);
    }
    public void CheckNoView()
    {
        if (_audio != null) _audio.PlayOneShot(se);
        onoffobj.SetActive(false);
    }
    public void OpenPageView()
    {
        if (_audio != null) _audio.PlayOneShot(se);
        onoffobj.SetActive(false);
        Application.OpenURL(seturl);
    }
}
