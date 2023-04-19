using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shopMagic : MonoBehaviour
{
    public AudioSource audioS;
    public AudioClip getse;
    public AudioClip notse;
    public int inputshopID;
    public Sprite nullsprite;
    public Text magicname;
    public Image magicsprite;
    public Text magicscript;
    public Text magicprice;
    public Text magicinputget;
    public Image slimesprite;
    public Text slimename;
    bool pushtrg = false;
    float pushtime = 0;
    private bool notget = true;
    // Start is called before the first frame update
    void Start()
    {
        if (GManager.instance.shopID[inputshopID] == -1)
        {
            magicname.text = "？？？？？？";
            magicsprite.sprite = nullsprite;
            magicscript.text = "？？？？？？？？？？";
            magicprice.text = "？×";
            magicinputget.text = "？???";
            slimesprite.sprite = nullsprite;
            if (inputshopID == 0)
            {
                slimename.text = "????:????";
            }
        }
        else if (GManager.instance.shopID[inputshopID] != -1)
        {
            magicsprite.sprite = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicimage;
            slimesprite.sprite = GManager.instance.Pstatus[GManager.instance.playerselect].pimage;
            if (GManager.instance.isEnglish == 0)
            {
                magicscript.text = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicscript;
                magicname.text = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicname;
                if (inputshopID == 0)
                {
                    slimename.text = "選択中のスライム：" + GManager.instance.Pstatus[GManager.instance.playerselect].pname;

                }
            }
            else if (GManager.instance.isEnglish == 1)
            {
                magicscript.text = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicscript2;
                magicname.text = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicname2;
                if (inputshopID == 0)
                {
                    slimename.text = "Slime in selection：" + GManager.instance.Pstatus[GManager.instance.playerselect].pname2;
                }
            }
            magicprice.text = GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicprice + "×";
            for (int i = 0; i < GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length;)
            {
                notget = true;
                if (GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].magicid == GManager.instance.shopID[inputshopID] && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].gettrg < 1)
                {
                    if (GManager.instance.isEnglish == 0)
                    {
                        magicinputget.text = "覚えられる：";
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        magicinputget.text = "be able to remember：";
                    }
                    notget = false;
                    i = GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length;
                }
                i++;
            }
            if (notget == true)
            {
                if (GManager.instance.isEnglish == 0)
                {
                    magicinputget.text = "覚えられない：";
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    magicinputget.text = "I can't remember：";
                }
            }

        }
        else
        {
            magicname.text = "？？？？？？";
            magicsprite.sprite = nullsprite;
            magicscript.text = "？？？？？？？？？？";
            magicprice.text = "？×";
            magicinputget.text = "？???";
            slimesprite.sprite = nullsprite;
            if (inputshopID == 0)
            {
                slimename.text = "????:????";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (pushtrg == true)
        {
            pushtime += Time.deltaTime;
            if (pushtime > 1f)
            {
                pushtime = 0;
                pushtrg = false;
            }
        }
    }

    public void ShopClick()
    {
        if (pushtrg == false)
        {
            pushtrg = true;
            if (GManager.instance.shopID[inputshopID] == -1 || notget == true)
            {
                audioS.PlayOneShot(notse);
            }
            else if (GManager.instance.shopID[inputshopID] != -1 && notget == false)
            {
                if (GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicprice > GManager.instance.Coin)
                {
                    audioS.PlayOneShot(notse);
                }
                else if (GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicprice <= GManager.instance.Coin )
                {
                    audioS.PlayOneShot(getse);
                    GManager.instance.Coin -= GManager.instance.MagicID[GManager.instance.shopID[inputshopID]].magicprice;

                    for (int i = 0; i < GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length;)
                    {
                        if (GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].magicid == GManager.instance.shopID[inputshopID] && GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].gettrg < 1)
                        {
                            GManager.instance.Pstatus[GManager.instance.playerselect].getMagic[i].gettrg = 1;
                            i = GManager.instance.Pstatus[GManager.instance.playerselect].getMagic.Length;
                        }
                        i++;
                    }
                    notget = true;
                    if (GManager.instance.isEnglish == 0)
                    {
                        magicinputget.text = "覚えられない：";
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        magicinputget.text = "I can't remember：";
                    }
                }
            }
        }
    }
}
