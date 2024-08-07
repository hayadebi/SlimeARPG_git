using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    public UniLang.BattleSystem battleSystem;
    public AudioClip se;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ClickBone()
    {
        battleSystem.BoneInvoke(this.gameObject, se);
    }
}
