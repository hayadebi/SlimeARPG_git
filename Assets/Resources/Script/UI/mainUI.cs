using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainUI : MonoBehaviour
{
    //public string bossname;
    public Text magicname;
    public Text skillname;
    public Text inputmptext;
    public Image magicicon;
    public Image skillicon;
    public string oldInt ;
    public Sprite oldmagicSprite = null;
    public Sprite oldskillSprite = null;
    public Sprite nullimage;
    public int oldEnglish = 0;
    private bool UItrg = false;
    private bool changetrg = true;
    //private int oldmagicid = -1;
    //private int oldskillid = -1;
    private int oldslime = 0;
    private int oldmode = 0;
    private int oldselectm = -1;
    private GameObject P = null;
    private player ps = null;
    private int olditem = -1;
    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.Find("Player");
        ps = P.GetComponent<player>();
        if (GManager.instance.isEnglish == 0)
        {
            if (GManager.instance.villageName == "")
            {
                GManager.instance.txtget = GManager.instance.stageName[GManager.instance.stageNumber];
            }
            else if (GManager.instance.villageName != "")
            {
                GManager.instance.txtget = GManager.instance.villageName;
            }
        }
        else if (GManager.instance.isEnglish == 1)
        {
            if (GManager.instance.villageName == "")
            {
                GManager.instance.txtget = GManager.instance.stageName2[GManager.instance.stageNumber];
            }
            else if (GManager.instance.villageName != "")
            {
                GManager.instance.txtget = GManager.instance.villageName;
            }
        }
        Invoke("startUI", 0.1f);
    }
    public void startUI()
    {
        if (P != null && ps != null)
        {
            if (GManager.instance.Pstatus[GManager.instance.playerselect].magicselect != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 0)
            {
                magicicon.sprite =
                GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicimage;
                oldmagicSprite = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicimage;
                if (GManager.instance.isEnglish == 0)
                {
                    magicname.text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicname;
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    magicname.text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicname2;
                }
            }
            else if (GManager.instance.Pstatus[GManager.instance.playerselect].selectskill != -1 && ps.onSkill != null && ps.onSkill.Length != 0 && GManager.instance.Pstatus[GManager.instance.playerselect].selectskill != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 1)
            {
                magicicon.sprite =
                    GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillicon;
                oldskillSprite = GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillicon;
                if (GManager.instance.isEnglish == 0)
                {
                    magicname.text = GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillname;
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    magicname.text = GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillname2;
                }
            }
            //----------------------
            else if (ps.onItem != null && ps.onItem.Length != 0 && GManager.instance._quickSelect != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 2 && GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemnumber > 0)
            {
                magicicon.sprite =
                    GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemimage;
                oldskillSprite = GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemimage;
                if (GManager.instance.isEnglish == 0)
                {
                    magicname.text = GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemname;
                }
                else if (GManager.instance.isEnglish == 1)
                {
                    magicname.text = GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemname2;
                }
                olditem = GManager.instance._quickSelect;
            }
            //----------------------
            else
            {
                magicicon.sprite = nullimage;
                oldmagicSprite = nullimage;
                oldskillSprite = nullimage;
                magicname.text = "????????";
            }
            if (GManager.instance.Pstatus[GManager.instance.playerselect].magicselect != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 0)
            {
                inputmptext.text = "MP：" + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].inputmagicpower;
                oldInt = "MP：" + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].inputmagicpower;
            }
            else
            {
                inputmptext.text = "MP：???";
                oldInt = "MP：???";
            }
            oldselectm = GManager.instance.Pstatus[GManager.instance.playerselect].magicselect;
            oldmode = GManager.instance.Pstatus[GManager.instance.playerselect].changemode;
            UItrg = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (P != null && ps != null)
        {
            if ((oldEnglish != GManager.instance.isEnglish)||(ps.onItem != null && ps.onItem.Length != 0 && GManager.instance._quickSelect != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 2 && ((GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemnumber <= 0 && magicname.text != "????????")|| (GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemnumber > 0 && magicname.text == "????????"))))
            {
                if (GManager.instance.Pstatus[GManager.instance.playerselect].magicselect != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 0)
                {
                    magicicon.sprite =
                    GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicimage;
                    oldmagicSprite = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicimage;
                    if (GManager.instance.isEnglish == 0)
                    {
                        magicname.text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicname;
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        magicname.text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicname2;
                    }
                }
                else if (GManager.instance.Pstatus[GManager.instance.playerselect].selectskill != -1 && ps.onSkill != null && ps.onSkill.Length != 0 && GManager.instance.Pstatus[GManager.instance.playerselect].selectskill != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 1)
                {
                    magicicon.sprite =
                        GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillicon;
                    oldskillSprite = GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillicon;
                    if (GManager.instance.isEnglish == 0)
                    {
                        magicname.text = GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillname;
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        magicname.text = GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillname2;
                    }
                }
                //-------------
                else if (ps.onItem != null && ps.onItem.Length != 0 && GManager.instance._quickSelect != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 2 && GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemnumber > 0)
                {
                    magicicon.sprite =
                        GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemimage;
                    oldskillSprite = GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemimage;
                    if (GManager.instance.isEnglish == 0)
                    {
                        magicname.text = GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemname;
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        magicname.text = GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemname2;
                    }
                    olditem = GManager.instance._quickSelect;
                }
                //-------------
                else
                {
                    magicicon.sprite = nullimage;
                    oldmagicSprite = nullimage;
                    oldskillSprite = nullimage;
                    magicname.text = "????????";
                }
                oldselectm = GManager.instance.Pstatus[GManager.instance.playerselect].magicselect;
                oldEnglish = GManager.instance.isEnglish;
                oldmode = GManager.instance.Pstatus[GManager.instance.playerselect].changemode;
            }
            if (UItrg == true && oldslime == GManager.instance.playerselect && (oldmode != GManager.instance.Pstatus[GManager.instance.playerselect].changemode || oldselectm != GManager.instance.Pstatus[GManager.instance.playerselect].magicselect || olditem != GManager.instance._quickSelect))
            {
                GManager.instance.setrg = 6;
                olditem = GManager.instance._quickSelect;
                //----------------------------------
                if (GManager.instance.Pstatus[GManager.instance.playerselect].magicselect != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 0)
                {
                    magicicon.sprite =
                    GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicimage;
                    oldmagicSprite = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicimage;
                    if (GManager.instance.isEnglish == 0)
                    {
                        magicname.text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicname;
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        magicname.text = GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].magicname2;
                    }
                }
                else if (GManager.instance.Pstatus[GManager.instance.playerselect].selectskill != -1 && ps.onSkill != null && ps.onSkill.Length != 0 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 1)
                {
                    magicicon.sprite =
                        GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillicon;
                    oldskillSprite = GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillicon;
                    if (GManager.instance.isEnglish == 0)
                    {
                        magicname.text = GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillname;
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        magicname.text = GManager.instance.SkillID[GManager.instance.Pstatus[GManager.instance.playerselect].inputskill[ps.onSkill[GManager.instance.Pstatus[GManager.instance.playerselect].selectskill]]].skillname2;
                    }
                }
                //----------
                else if (ps.onItem != null && ps.onItem.Length != 0 && GManager.instance._quickSelect != -1 && GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 2 && GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemnumber > 0)
                {
                    magicicon.sprite =
                        GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemimage;
                    oldskillSprite = GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemimage;
                    if (GManager.instance.isEnglish == 0)
                    {
                        magicname.text = GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemname;
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        magicname.text = GManager.instance.ItemID[GManager.instance.Quick_itemSet[ps.onItem[GManager.instance._quickSelect]]].itemname2;
                    }
                    olditem = GManager.instance._quickSelect;
                }
                //----------
                else
                {
                    magicicon.sprite = nullimage;
                    oldskillSprite = nullimage;
                    oldmagicSprite = nullimage;
                    magicname.text = "????????";
                }
                oldselectm = GManager.instance.Pstatus[GManager.instance.playerselect].magicselect;
                oldmode = GManager.instance.Pstatus[GManager.instance.playerselect].changemode;
            }
            //----------------------------------------------------
            if (UItrg == true && oldslime == GManager.instance.playerselect)
            {
                if (GManager.instance.Pstatus[GManager.instance.playerselect].magicselect != -1 && oldInt != "MP：" + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].inputmagicpower)
                {
                    inputmptext.text = "MP：" + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].inputmagicpower;
                    oldInt = "MP：" + GManager.instance.MagicID[GManager.instance.Pstatus[GManager.instance.playerselect].magicSet[GManager.instance.Pstatus[GManager.instance.playerselect].magicselect]].inputmagicpower;
                }
                else if (GManager.instance.Pstatus[GManager.instance.playerselect].magicselect == -1 && oldInt != "MP：???")
                {
                    inputmptext.text = "MP：???";
                    oldInt = "MP：???";
                }
                //****************************************************************************
                if (GManager.instance.Pstatus[GManager.instance.playerselect].loadtime != 0)
                {
                    if (magicicon.color != new Color(1, 1, 1, 0.3f))
                    {
                        magicicon.color = new Color(1, 1, 1, 0.3f);
                    }
                }
                else if (GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 0 && changetrg == true)
                {
                    changetrg = false;
                    magicicon.color = new Color(1, 1, 1, 1f);
                }
                else if (GManager.instance.Pstatus[GManager.instance.playerselect].changemode == 1 && changetrg == false)
                {
                    changetrg = true;
                    magicicon.color = new Color(1, 1, 1, 1f);
                }
                else if (GManager.instance.Pstatus[GManager.instance.playerselect].loadtime == 0 && magicicon.color != new Color(1, 1, 1, 1f))
                {
                    magicicon.color = new Color(1, 1, 1, 1f);
                }
            }
            if (GManager.instance.playerselect != oldslime)
            {
                UItrg = false;
                oldslime = GManager.instance.playerselect;
                startUI();
            }
        }
    }

}