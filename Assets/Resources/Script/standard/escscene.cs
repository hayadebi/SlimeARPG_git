using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class escscene : MonoBehaviour
{
    bool esctrg = false;
    public string sceneName = "title";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (esctrg == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Return))
            {
                esctrg = true;
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
