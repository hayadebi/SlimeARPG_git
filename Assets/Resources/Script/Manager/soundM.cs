using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundM : MonoBehaviour
{
    public AudioClip[] se;
    AudioSource audioS;
    private float cooltime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (cooltime >= 0) cooltime -= Time.deltaTime;
        if(Input.GetKeyDown (KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        if( GManager.instance.ase != null)
        {
            audioS.PlayOneShot(GManager.instance.ase);
            GManager.instance.ase = null;
        }
        else if( GManager.instance.setrg != -1 && GManager.instance.setrg != 99 && cooltime<=0)
        {
            cooltime = 0.2f;
            audioS.PlayOneShot(se[GManager.instance.setrg]);
            GManager.instance.setrg = -1;
        }
    }

}
