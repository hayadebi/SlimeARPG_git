using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class slimeUI : MonoBehaviour
{
    public GameObject selectmenuUI;
    //public GameObject selectmenuUI;
    public AudioSource audioS;
    public AudioClip selectse;
    public AudioClip onse;
    public AudioClip notse;
    public Sprite nullimage;
    public Text nameText;
    public Text scriptText;
    public Text statusText;
    public Text selectText;
    [System.Serializable]
    public struct skillP
    {
        public Text skillname;
        public Text skillscript;
        public Image skillimage;
    }
    public skillP[] SkillUI;
    public Image SLimage;
    int selectnumber = 0;
    public int[] onSlime;
    int boxnumber = 0;
    int inputnumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; GManager.instance.Pstatus.Length > i;)
        {
            for (int l = 0; l == 0 && GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp > 0;)
            {
                boxnumber += 1;
                l++;
            }
            i += 1;
        }
        onSlime = new int[boxnumber];
        for (int i = 0; GManager.instance.Pstatus.Length > i;)
        {
            for (int l = 0; l == 0 && GManager.instance.Pstatus[i].getpl > 0 && GManager.instance.Pstatus[i].hp > 0;)
            {
                onSlime[inputnumber] = i;
                inputnumber += 1;
                l++;
            }
            i += 1;
        }
        SetUI();
    }

    // Update is called once per frame
    void SetUI()
    {
        if (onSlime == null)
        {
            SLimage.sprite = nullimage;
            selectText.text = "";
            nameText.text = "??????";
            scriptText.text = "????????";
            for (int i = 0; i < SkillUI.Length;)
            {
                SkillUI[i].skillimage.sprite = nullimage;
                SkillUI[i].skillname.text = "????";
                SkillUI[i].skillscript.text = "????????";
                i++;
            }
            if (GManager.instance.isEnglish == 0)
            {
                statusText.text = "MaxHP:??" +
                    "\nMaxMP:??" +
                    "\nAT:?" +
                    "\nDF:?" +
                    "\nSP:?" +
                    "\nLV:??" +
                    "\n攻撃タイプ:???" +
                    "\n弱点タイプ:???";
            }
            else if (GManager.instance.isEnglish == 1)
            {
               statusText.text = "MaxHP:??" +
                     "\nMaxMP:??" +
                     "\nAT:?" +
                     "\nDF:?" +
                     "\nSP:?" +
                     "\nLV:??" +
                     "\nAttack type:???" +
                     "\nWeakness type:???";
            }
        }
        else if (GManager.instance.Pstatus[onSlime[selectnumber]].getpl > 0)
        {
            SLimage.sprite = GManager.instance.Pstatus[onSlime[selectnumber]].pimage;
            if (GManager.instance.isEnglish == 0)
            {
                if (onSlime[selectnumber] == GManager.instance.playerselect)
                {
                    selectText.text = "-"+ GManager.instance.Pstatus[onSlime[selectnumber]].pname+ "を使用中-";
                }
                else
                {
                    selectText.text = "";
                }
                nameText.text = GManager.instance.Pstatus[onSlime[selectnumber]].pname;
                scriptText.text = GManager.instance.Pstatus[onSlime[selectnumber]].pscript;
                for (int i = 0; i < SkillUI.Length;)
                {
                    if (GManager.instance.Pstatus[onSlime[selectnumber]].inputskill[i] != -1)
                    {
                        SkillUI[i].skillimage.sprite = GManager.instance.SkillID[GManager.instance.Pstatus[onSlime[selectnumber]].inputskill[i]].skillicon;
                        SkillUI[i].skillname.text = GManager.instance.SkillID[GManager.instance.Pstatus[onSlime[selectnumber]].inputskill[i]].skillname;
                        SkillUI[i].skillscript.text = GManager.instance.SkillID[GManager.instance.Pstatus[onSlime[selectnumber]].inputskill[i]].skillscript;
                    }
                    else
                    {
                        SkillUI[i].skillimage.sprite = nullimage;
                        SkillUI[i].skillname.text = "????";
                        SkillUI[i].skillscript.text = "????????";
                    }
                    i++;
                }
                statusText.text = "MaxHP:"+ GManager.instance.Pstatus[onSlime[selectnumber]].maxHP +
                    "\nMaxMP:"+ GManager.instance.Pstatus[onSlime[selectnumber]].maxMP +
                    "\nAT:"+ GManager.instance.Pstatus[onSlime[selectnumber]].attack +
                    "\nDF:"+ GManager.instance.Pstatus[onSlime[selectnumber]].defense +
                    "\nSP:"+ GManager.instance.Pstatus[onSlime[selectnumber]].speed +
                    "\nLV:"+ GManager.instance.Pstatus[onSlime[selectnumber]].Lv +
                    "\n攻撃タイプ:"+ GManager.instance.Pstatus[onSlime[selectnumber]].attacktype  +
                    "\n弱点タイプ:"+ GManager.instance.Pstatus[onSlime[selectnumber]].badtype;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                if (onSlime[selectnumber] == GManager.instance.playerselect)
                {
                    selectText.text = "-" + GManager.instance.Pstatus[onSlime[selectnumber]].pname2 + " in use-";
                }
                else
                {
                    selectText.text = "";
                }
                nameText.text = GManager.instance.Pstatus[onSlime[selectnumber]].pname2 ;
                scriptText.text = GManager.instance.Pstatus[onSlime[selectnumber]].pscript2;
                for (int i = 0; i < SkillUI.Length;)
                {
                    if (GManager.instance.Pstatus[onSlime[selectnumber]].inputskill[i] != -1)
                    {
                        SkillUI[i].skillimage.sprite = GManager.instance.SkillID[GManager.instance.Pstatus[onSlime[selectnumber]].inputskill[i]].skillicon;
                        SkillUI[i].skillname.text = GManager.instance.SkillID[GManager.instance.Pstatus[onSlime[selectnumber]].inputskill[i]].skillname2;
                        SkillUI[i].skillscript.text = GManager.instance.SkillID[GManager.instance.Pstatus[onSlime[selectnumber]].inputskill[i]].skillscript2;
                    }
                    else
                    {
                        SkillUI[i].skillimage.sprite = nullimage;
                        SkillUI[i].skillname.text = "????";
                        SkillUI[i].skillscript.text = "????????";
                    }
                    i++;
                }
                statusText.text = "MaxHP:" + GManager.instance.Pstatus[onSlime[selectnumber]].maxHP +
                    "\nMaxMP:" + GManager.instance.Pstatus[onSlime[selectnumber]].maxMP +
                    "\nAT:" + GManager.instance.Pstatus[onSlime[selectnumber]].attack +
                    "\nDF:" + GManager.instance.Pstatus[onSlime[selectnumber]].defense +
                    "\nSP:" + GManager.instance.Pstatus[onSlime[selectnumber]].speed +
                    "\nLV:" + GManager.instance.Pstatus[onSlime[selectnumber]].Lv +
                    "\nAttack type:" + GManager.instance.Pstatus[onSlime[selectnumber]].attacktype2 +
                    "\nWeakness type:" + GManager.instance.Pstatus[onSlime[selectnumber]].badtype2;
            }
        }
        else
        {
            SLimage.sprite = nullimage;
            selectText.text = "";
            nameText.text = "??????";
            scriptText.text = "????????";
            for (int i = 0; i < SkillUI.Length;)
            {
                SkillUI[i].skillimage.sprite = nullimage;
                SkillUI[i].skillname.text = "????";
                SkillUI[i].skillscript.text = "????????";
                i++;
            }
            if (GManager.instance.isEnglish == 0)
            {
                statusText.text = "MaxHP:??" +
                    "\nMaxMP:??" +
                    "\nAT:?" +
                    "\nDF:?" +
                    "\nSP:?" +
                    "\nLV:??" +
                    "\n攻撃タイプ:???" +
                    "\n弱点タイプ:???";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                statusText.text = "MaxHP:??" +
                     "\nMaxMP:??" +
                     "\nAT:?" +
                     "\nDF:?" +
                     "\nSP:?" +
                     "\nLV:??" +
                     "\nAttack type:???" +
                     "\nWeakness type:???";
            }
        }
    }

    public void SelectMinus()
    {
        if (onSlime.Length == 0)
        {
            audioS.PlayOneShot(notse);
        }
        else if (selectnumber > 0)
        {
            audioS.PlayOneShot(selectse);
            selectnumber -= 1;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    public void SelectPlus()
    {
        if (onSlime.Length == 0)
        {
            audioS.PlayOneShot(notse);
        }
        else if (selectnumber < (onSlime.Length - 1))
        {
            audioS.PlayOneShot(selectse);
            selectnumber += 1;
            //----
            SetUI();
            //----
        }
        else
        {
            audioS.PlayOneShot(notse);
        }
    }
    //----
    public void MenuUI()
    {
        if (onSlime != null && onSlime.Length != 0)
        {
            if(onSlime[selectnumber] == GManager.instance.playerselect)
            {
                audioS.PlayOneShot(notse);
            }
            else if (GManager.instance.Pstatus[onSlime[selectnumber]].getpl != 0 && onSlime[selectnumber] != GManager.instance.playerselect)
            {
                audioS.PlayOneShot(onse);
                selectmenuUI.SetActive(true);
            }
            else
            {
                audioS.PlayOneShot(notse);
            }
        }
    }
    public void ItemSet()
    {
        GManager.instance.setrg = 1;
        GManager.instance.playerselect = onSlime[selectnumber];
        //----------------------------------------------
        SetUI();
        selectmenuUI.SetActive(false);
    }

    public void NotSet()
    {
        audioS.PlayOneShot(notse);
        selectmenuUI.SetActive(false);
    }

}