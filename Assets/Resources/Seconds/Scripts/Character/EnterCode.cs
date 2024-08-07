using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCode : MonoBehaviour
{
    public int CodeNumber = 1;
    public SecondPlayer player;
    // Start is called before the first frame update
    void Start()
    {
        ;
    }
    public void CodeInvoke()
    {
        player.CodeInvoke("Alpha" + CodeNumber.ToString());
    }
}
