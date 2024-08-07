using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    public AudioClip[] se;
    private AudioSource audioSource=null;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<AudioSource>()) audioSource = GetComponent<AudioSource>();
    }

   public void AnimSound0()
    {
        if(audioSource!=null)audioSource.PlayOneShot(se[0]);
    }
}
