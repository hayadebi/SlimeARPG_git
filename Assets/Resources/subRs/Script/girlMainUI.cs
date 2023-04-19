using UnityEngine;
using System.Collections;
using UnityEngine.UI; // ←※これを忘れずに入れる

public class girlMainUI : MonoBehaviour
{
    public Image hp_slider;
    public Image mp_slider;
    public Text hp_text;
    public Text mp_text;
    private float hp_addnumber = 0;
    private float mp_addnumber;
    private GameObject plobj;
    private girlPlayer glpl = null;
    public Image slglImage;
    [System.Serializable]
   public struct ATUI
    {
        public Image atImage;
        public Text atRemoveText;
    }
    public ATUI[] atSet;
    private int old_hp;
    private int old_mp;
    private int old_pl;
    public Sprite nullSprite;
    public Color dfColor;
    public Color notColor;
    private bool loadtrg = false;

    void Start()
    {
        //取得する
        plobj = GameObject.Find("Player");
        glpl = plobj.GetComponent<girlPlayer>();
        if (plobj != null && glpl != null)
        {
            //HP
            hp_addnumber = 10000 / glpl.MaxHP;
            hp_addnumber /= 10000;
            hp_addnumber = hp_addnumber * 10000;
            hp_addnumber = Mathf.Floor(hp_addnumber) / 10000;
            hp_slider.fillAmount = hp_addnumber * glpl.HP;
            hp_text.text = glpl.HP.ToString();
            old_hp = glpl.HP;
            //MP
            mp_addnumber = 10000/ glpl.MaxMP;
            mp_addnumber /= 10000;
            mp_addnumber = mp_addnumber * 10000;
            mp_addnumber = Mathf.Floor(mp_addnumber) / 10000;
            mp_slider.fillAmount = mp_addnumber * glpl.MP;
            mp_text.text = glpl.MP.ToString();
            old_mp = glpl.MP;
            //PL
            slglImage.sprite = glpl.slsp;
            for(int i = 0;i < glpl.motionManager.Length;)
            {
                if (glpl.motionManager[i].activeTrg > 0)
                {
                    atSet[i].atImage.sprite = glpl.motionManager[i].atsp;
                    if (GManager.instance.isEnglish == 0)
                    {
                        atSet[i].atRemoveText.fontSize = 24;
                        atSet[i].atRemoveText.text = "消費空腹度：" + glpl.motionManager[i].removemp;
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        atSet[i].atRemoveText.fontSize = 21;
                        atSet[i].atRemoveText.text = "Remove satiety：" + glpl.motionManager[i].removemp;
                    }
                }
                else if(glpl.motionManager[i].activeTrg < 1)
                {
                    atSet[i].atImage.sprite = nullSprite;
                    if (GManager.instance.isEnglish == 0)
                    {
                        atSet[i].atRemoveText.fontSize = 24;
                        atSet[i].atRemoveText.text = "消費空腹度：??";
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        atSet[i].atRemoveText.fontSize = 21;
                        atSet[i].atRemoveText.text = "Remove satiety：??";
                    }
                }
                i++;
            }
            old_pl = glpl.charachange;
        }
    }

    void Update()
    {
        //UI更新
        if (plobj != null && glpl != null)
        {
            //HP
            if (old_hp != glpl.HP)
            {
                hp_slider.fillAmount = hp_addnumber * glpl.HP;
                hp_text.text = glpl.HP.ToString();
                old_hp = glpl.HP;
            }
            //MP
            if (old_mp != glpl.MP)
            {
                mp_slider.fillAmount = mp_addnumber * glpl.MP;
                mp_text.text = glpl.MP.ToString();
                old_mp = glpl.MP;
            }
            //PL
            if (old_pl != GManager.instance.subcharaTrg)
            {
                slglImage.sprite = glpl.slsp;
                hp_addnumber = 10000 / glpl.MaxHP;
                mp_addnumber = 10000 / glpl.MaxMP;
                hp_addnumber /= 10000;
                hp_addnumber = hp_addnumber * 10000;
                hp_addnumber = Mathf.Floor(hp_addnumber) / 10000;
                mp_addnumber /= 10000;
                mp_addnumber = mp_addnumber * 10000;
                mp_addnumber = Mathf.Floor(mp_addnumber) / 10000;
                    hp_slider.fillAmount = hp_addnumber * glpl.HP;
                    hp_text.text = glpl.HP.ToString();
                    old_hp = glpl.HP;
                    mp_slider.fillAmount = mp_addnumber * glpl.MP;
                    mp_text.text = glpl.MP.ToString();
                    old_mp = glpl.MP;
                for (int i = 0; i < glpl.motionManager.Length;)
                {
                    if (glpl.motionManager[i].activeTrg > 0)
                    {
                        atSet[i].atImage.sprite = glpl.motionManager[i].atsp;
                        if (GManager.instance.isEnglish == 0)
                        {
                            atSet[i].atRemoveText.fontSize = 24;
                            atSet[i].atRemoveText.text = "消費空腹度：" + glpl.motionManager[i].removemp;
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            atSet[i].atRemoveText.fontSize = 21;
                            atSet[i].atRemoveText.text = "Remove satiety：" + glpl.motionManager[i].removemp;
                        }
                    }
                    else if(glpl.motionManager[i].activeTrg < 1)
                    {
                        atSet[i].atImage.sprite = nullSprite;
                        if (GManager.instance.isEnglish == 0)
                        {
                            atSet[i].atRemoveText.fontSize = 24;
                            atSet[i].atRemoveText.text = "消費空腹度：??";
                        }
                        else if (GManager.instance.isEnglish == 1)
                        {
                            atSet[i].atRemoveText.fontSize = 21;
                            atSet[i].atRemoveText.text = "Remove satiety：??";
                        }
                    }
                    i++;
                }
                old_pl = glpl.charachange;
            }
        }
        //ロードUI
        if(glpl.loadtime != 0 && loadtrg == false)
        {
            loadtrg = true;
            for(int i = 0; i < atSet.Length;)
            {
                if(atSet[i].atImage.color != notColor)
                {
                    atSet[i].atImage.color = notColor;
                }
                i++;
            }
        }
        else if(glpl.loadtime == 0 && loadtrg == true)
        {
            loadtrg = false;
            for (int i = 0; i < atSet.Length;)
            {
                if (atSet[i].atImage.color != dfColor)
                {
                    atSet[i].atImage.color = dfColor;
                }
                i++;
            }
        }
    }
}