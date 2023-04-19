using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class oreUI : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip[] se;
    public Animator oreanim;
    public Animator effectanim;
    
    public GameObject effectobj;
    public Image itemimage;
    public Sprite oresprite;
    public Sprite nullsprite;
    public Text getitemText;
    private bool buttonNot = false;
    public string animname = "Anumber";
    private int randomid;
    // Start is called before the first frame update
    void Start()
    {
        if (GManager.instance.isEnglish == 0)
        {
            getitemText.text = "手に入れたアイテム：\n「？？？？？？？？」";
        }
        else if (GManager.instance.isEnglish == 1)
        {
            getitemText.text = "Items we got：\n「？？？？？？？？」";
        }
        SetUI();
    }
    void SetUI()
    {
        if (GManager.instance.ItemID[10].itemnumber > 0)
        {
            itemimage.sprite = oresprite;
        }
        else
        {
            itemimage.sprite = nullsprite;
        }
    }

    public void Plan()
    {
        if (GManager.instance.Coin >= 5 && GManager.instance.ItemID[10].itemnumber > 0 && buttonNot == false)
        {
            buttonNot = true;
            audioS.PlayOneShot(se[1]);
            GManager.instance.ItemID[10].itemnumber -= 1;
            GManager.instance.Coin -= 5;
            randomid = Random.Range(0, GManager.instance.stoneID.Length);
            effectobj.SetActive(true);
            effectanim.enabled = true;
            if (GManager.instance.isEnglish == 0)
            {
                getitemText.text = "手に入れたアイテム：\n" + GManager.instance.ItemID[GManager.instance.stoneID[randomid]].itemname;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                getitemText.text = "Items we got：\n" + GManager.instance.ItemID[GManager.instance.stoneID[randomid]].itemname2;
            }
            itemimage.sprite = GManager.instance.ItemID[GManager.instance.stoneID[randomid]].itemimage;
            GManager.instance.ItemID[GManager.instance.stoneID[randomid]].itemnumber += 1;
            GManager.instance.ItemID[GManager.instance.stoneID[randomid]].gettrg = 1;
            oreanim.SetInteger(animname, 2);
            Invoke("Omove_1", 1f);
        }
        else
        {
            audioS.PlayOneShot(se[0]);
        }
    }
    void Omove_1()
    {
        effectanim.enabled = false;
        effectobj.SetActive(false);
        oreanim.SetInteger(animname, 1);
        Invoke("Omove_2", 1f);
    }
    void Omove_2()
    {
        SetUI();
        effectobj.SetActive(false);
        oreanim.SetInteger(animname, 0);
        Invoke("Omove_3", 1f);
    }
    void Omove_3()
    {
        buttonNot = false;
    }
}
