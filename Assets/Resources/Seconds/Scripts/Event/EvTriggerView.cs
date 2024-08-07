using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvTriggerView : MonoBehaviour
{
    public GameObject ViewObject;
    // Start is called before the first frame update
    void Start()
    {
        ;
    }

    public void OnView()
    {
        ViewObject.SetActive(true);
    }
    public void OffView()
    {
        ViewObject.SetActive(false);
    }

    private void OnDisable()
    {
        ViewObject.SetActive(false);
    }
}
