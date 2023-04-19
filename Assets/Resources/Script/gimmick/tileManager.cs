using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class tileManager : MonoBehaviour
{
    public int tileEvent = 0;
    public ColEvent clev = null;
    public AudioSource audioS;
    public AudioClip se;
    [System.Serializable]
    public struct TileSet
    {
        public int tileID;
        public GameObject MainObj;
        public EffekseerEmitter myEffect;
        public GameObject summonEffect;
        public float nextTime;
        public Animator anim;
    }
    public TileSet[] tileset;
    private float inputtime = 0;
    public int tileindex = 0;
    public float trapTime = 2;
    private string tagname = "red";
    // Start is called before the first frame update
    void Start()
    {
        tagname = "red";
        if (tileEvent == 0)
        {
            tileindex = 0;
            tileset[tileindex].MainObj.SetActive(true);
            if (tileset[tileindex].myEffect != null)
            {
                tileset[tileindex].myEffect.Play();
            }
        }
        if(tileEvent == 1 && tileindex == 1)
        {
            tagname = "OnMask";
            for (int i = 0; i < tileset.Length;)
            {
                tileset[i].MainObj.tag = tagname;
                tileset[i].anim.SetInteger("Anumber", tileindex);
                i++;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(clev.ColTrigger == true)
        {
            if (tileEvent == 0)
            {
                inputtime += Time.deltaTime;
                if (inputtime >= tileset[tileindex].nextTime)
                {
                    inputtime = 0;
                    audioS.PlayOneShot(se);
                    Instantiate(GManager.instance.effectobj[14], tileset[tileindex].MainObj.transform.position, this.transform.rotation);
                    if (tileset[tileindex].myEffect != null)
                    {
                        tileset[tileindex].myEffect.Stop();
                    }
                    tileset[tileindex].MainObj.SetActive(false);
                    tileindex += 1;
                    if (tileindex >= tileset.Length)
                    {
                        tileindex = 0;
                    }
                    tileset[tileindex].MainObj.SetActive(true);
                    if (tileset[tileindex].myEffect != null)
                    {
                        tileset[tileindex].myEffect.Play();
                    }
                }
            }
            if (tileEvent == 1)
            {
                inputtime += Time.deltaTime;
                if (inputtime > trapTime) 
                {
                    inputtime = 0;
                    audioS.PlayOneShot(se);
                    tileindex += 1;
                    tagname = "OnMask";
                    if(tileindex > 1)
                    {
                        tileindex = 0;
                        tagname = "red";
                    }
                    for (int i = 0; i < tileset.Length;)
                    {
                        tileset[i].MainObj.tag = tagname;
                        tileset[i].anim.SetInteger("Anumber", tileindex);
                        i++;
                    }
                }
            }
        }
    }
}
