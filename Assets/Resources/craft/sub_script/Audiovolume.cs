using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiovolume : MonoBehaviour
{
    public bool battletrg = false;
    private bool isadd = false;
    float oldvolume;
    public bool setrg = false;
    void Start()
    {
        //アタッチされているAudioSource取得
        AudioSource audio = GetComponent<AudioSource>();
        if(GManager.instance.over)
        {
            audio.volume = GManager.instance.audioMax / 12;
            oldvolume = GManager.instance.audioMax / 12;
        }
        else if (!setrg)
        {
            audio.volume = GManager.instance.audioMax / 4;
            oldvolume = GManager.instance.audioMax / 4;
        }
        else if (setrg)
        {
            audio.volume = GManager.instance.seMax  ;
            oldvolume = GManager.instance.seMax ;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (GManager.instance.Triggers[104] == 0)
        {
            if (GManager.instance.over || !GManager.instance.walktrg)
            {
                if (oldvolume != GManager.instance.audioMax / 16 && !setrg && !GManager.instance.over)
                {
                    audio.volume = GManager.instance.audioMax / 16;
                    oldvolume = GManager.instance.audioMax / 16;
                }
                else if (oldvolume != 0 && !setrg && GManager.instance.over)
                {
                    audio.volume = 0;
                    oldvolume = 0;
                }
            }
            else if (!setrg && oldvolume != GManager.instance.audioMax / 4)
            {
                audio.volume = GManager.instance.audioMax / 4;
                oldvolume = GManager.instance.audioMax / 4;
            }
            else if (setrg && oldvolume != GManager.instance.seMax)
            {
                audio.volume = GManager.instance.seMax;
                oldvolume = GManager.instance.seMax;
            }
            ////オンオフ
            //if (!battletrg && !audio.enabled)
            //{
            //    audio.enabled = true;
            //}
            //if (!battletrg && DataManager.instance.Triggers[1] == 1&& audio.enabled)
            //{
            //    audio.enabled = false;
            //}
            //else if (!battletrg && DataManager.instance.Triggers[1] == 0 && !audio.enabled)
            //{
            //    audio.enabled = true;
            //}
        }
    }
}
