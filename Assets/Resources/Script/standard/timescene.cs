using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timescene : MonoBehaviour
{
    public bool endtrg = false;
    public float maxtime;
    public string scenetxt;
    private bool pushtrg = false;
    // Start is called before the first frame update
    void Start()
    {
        
        Invoke("NextScene", maxtime);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && pushtrg == false && endtrg == false)
        {
            pushtrg = true;
            
            NextScene();
        }
    }

    void NextScene()
    {
        if (endtrg == true)
        {
            GManager.instance.walktrg = true;
            GManager.instance.over = false;
            GManager.instance.setmenu = 0;
            GManager.instance.txtget = "";
            GManager.instance.pushtrg = false;
            GManager.instance.setrg = -1;
            PlayerPrefs.DeleteAll();
        }
        SceneManager.LoadScene(scenetxt);
    }
}
