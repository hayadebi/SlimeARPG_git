using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class clickfade : MonoBehaviour
{
    public GameObject fadein;
    public AudioSource audioS;
    public AudioClip se;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void clickCommand()
    {
        audioS.PlayOneShot(se);
        Instantiate(fadein, transform.position, transform.rotation);
    }

   
}
