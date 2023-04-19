using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyBook : MonoBehaviour
{
    //public GameObject selectmenuUI;
    public AudioSource audioS;
    public AudioClip selectse;
    public AudioClip notse;
    public Sprite nullimage;
    public Image Eimage;
    public Text Ename;
    public Text Escript;
    public Text Ehouse;
    public Text Etype;
    int selectnumber = 0;
    public int[] onEnemy;
    int boxnumber = 0;
    int inputnumber = 0;
    bool usetrg = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; GManager.instance.enemynoteID.Length > i;)
        {
            for (int l = 0; l == 0;)
            {
                boxnumber += 1;
                l++;
            }
            i += 1;
        }
        onEnemy = new int[boxnumber];
        for (int i = 0; GManager.instance.enemynoteID.Length > i;)
        {
            for (int l = 0; l == 0;)
            {
                onEnemy[inputnumber] = i;
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
        if (onEnemy  == null)
        {
            Eimage.sprite = nullimage;
            Ename.text = "??????";
            if (GManager.instance.isEnglish == 0)
            {
                Escript.text = "解説:\n" + "????????";
                Ehouse.text = "生息地:\n" + "????????";
                Etype.text = "攻撃タイプ:" + "????" + "　" + "弱点タイプ:" + "????";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                Escript.text = "Description:\n" + "????????";
                Ehouse.text = "Habitat:\n" + "????????";
                Etype.text = "Attack Type:" + "????" + "　" + "Weakness Type:" + "????";
            }
        }
        else if (onEnemy != null && onEnemy.Length != 0 && GManager.instance.enemynoteID[onEnemy[selectnumber]].gettrg > 0)
        {
            Eimage.sprite = GManager.instance.enemynoteID[onEnemy[selectnumber]].image;
            if (GManager.instance.isEnglish == 0)
            {
                Ename.text = GManager.instance.enemynoteID[onEnemy[selectnumber]].name;
                Escript.text = "解説:\n" + GManager.instance.enemynoteID[onEnemy[selectnumber]].script;
                Ehouse.text = "生息地:\n" + GManager.instance.enemynoteID[onEnemy[selectnumber]].inputhouse;
                Etype.text = "攻撃タイプ:" + GManager.instance.enemynoteID[onEnemy[selectnumber]].inputattacktype + "　" + "弱点タイプ:" + GManager.instance.enemynoteID[onEnemy[selectnumber]].inputbadtype;
            }
            else if (GManager.instance.isEnglish == 1)
            {
                Ename.text = GManager.instance.enemynoteID[onEnemy[selectnumber]].name2;
                Escript.text = "Description:\n" + GManager.instance.enemynoteID[onEnemy[selectnumber]].script2;
                Ehouse.text = "Habitat:\n" + GManager.instance.enemynoteID[onEnemy[selectnumber]].inputhouse2;
                Etype.text = "Attack Type:" + GManager.instance.enemynoteID[onEnemy[selectnumber]].inputattacktype2 + "　" + "Weakness Type:" + GManager.instance.enemynoteID[onEnemy[selectnumber]].inputbadtype2;
            }
        }
        else
        {
            Eimage.sprite = nullimage;
            Ename.text = "??????";
            if (GManager.instance.isEnglish == 0)
            {
                Escript.text = "解説:\n" + "????????";
                Ehouse.text = "生息地:\n" + "????????";
                Etype.text = "攻撃タイプ:" + "????" + "　" + "弱点タイプ:" + "????";
            }
            else if (GManager.instance.isEnglish == 1)
            {
                Escript.text = "Description:\n" + "????????";
                Ehouse.text = "Habitat:\n" + "????????";
                Etype.text = "Attack Type:" + "????" + "　" + "Weakness Type:" + "????";
            }
        }
    }

    public void SelectMinus()
    {
        if (onEnemy == null)
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
        if (onEnemy == null)
        {
            audioS.PlayOneShot(notse);
        }
        else if (selectnumber < (onEnemy.Length - 1))
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
}