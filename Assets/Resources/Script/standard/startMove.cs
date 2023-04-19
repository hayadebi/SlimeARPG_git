using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startMove : MonoBehaviour
{
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public float time = 1;
    // Start is called before the first frame update
    void Start()
    {
        iTween.MoveAdd(gameObject, iTween.Hash("x", x, "y", y, "z", z, "time", time));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
