using UnityEngine;
using System.Collections;
using UnityEngine.UI; // ←※これを忘れずに入れる

public class slider : MonoBehaviour
{
    public string sliderType = "";
    Image _slider;
    float addnumber = 0;
    public enemyS es;
    public string bossname;
    int oldbosshp;
    GameObject boss;
    enemyS script;
    void Start()
    {
        // スライダーを取得する
        _slider = this.GetComponent<Image>();
        GManager.instance.audioMax = PlayerPrefs.GetFloat("audioMax", GManager.instance.audioMax);
        GManager.instance.seMax = PlayerPrefs.GetFloat("seMax", GManager.instance.seMax);
        GManager.instance.mode = PlayerPrefs.GetInt("mode", GManager.instance.mode);
        
        GManager.instance.kando = PlayerPrefs.GetFloat("kando", GManager.instance.kando);
        if (sliderType == "audio")
        {
            addnumber = 100 / 1;
            addnumber /= 100;
            addnumber = addnumber * 100;
            addnumber = Mathf.Floor(addnumber) / 100;
            _slider.fillAmount = addnumber * GManager.instance.audioMax;
        }
        else if (sliderType == "se")
        {
            addnumber = 100 / 1;
            addnumber /= 100;
            addnumber = addnumber * 100;
            addnumber = Mathf.Floor(addnumber) / 100;
            _slider.fillAmount = addnumber * GManager.instance.seMax;
        }
        else if (sliderType == "rotpivot")
        {
            addnumber = 1000 / 4;
            addnumber /= 1000;
            addnumber = addnumber * 1000;
            addnumber = Mathf.Floor(addnumber) / 1000;
            _slider.fillAmount = addnumber * GManager.instance.rotpivot;
        }
        else if (sliderType == "boss")
        {
            boss = GameObject.Find(bossname);
            script = boss.GetComponent<enemyS>();
            oldbosshp = script.Estatus.health;
            addnumber = 1000000 / oldbosshp;
            addnumber /= 1000000;
            addnumber = addnumber * 1000000;
            addnumber = Mathf.Floor(addnumber) / 1000000;
            _slider.fillAmount = addnumber * script.Estatus.health;
        }
        else if (sliderType == "mode")
        {
            addnumber = 100 / 2;
            addnumber /= 100;
            addnumber = addnumber * 100;
            addnumber = Mathf.Floor(addnumber) / 100;
            _slider.fillAmount = addnumber * GManager.instance.mode;
        }
        else if (sliderType == "kando")
        {
            addnumber = 100 / 5;
            addnumber /= 100;
            addnumber = addnumber * 100;
            addnumber = Mathf.Floor(addnumber) / 100;
            _slider.fillAmount = addnumber * GManager.instance.kando;
        }
        else if (sliderType == "hp")
        {
             addnumber = 10000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
            addnumber /= 10000;
            addnumber = addnumber * 10000;
            addnumber = Mathf.Floor(addnumber) / 10000;
            _slider.fillAmount = addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].hp;
        }
        else if (sliderType == "mp")
        {
            addnumber = 10000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
            addnumber /= 10000;
            addnumber = addnumber * 10000;
            addnumber = Mathf.Floor(addnumber) / 10000;
            _slider.fillAmount = addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].mp;
        }
        else if (sliderType == "ehp")
        {
            addnumber = 10000 / es.Estatus.maxhp;
            addnumber /= 10000;
            addnumber = addnumber * 10000;
            addnumber = Mathf.Floor(addnumber) / 10000;
            _slider.fillAmount = addnumber * es.Estatus.health;
        }
        else if (sliderType == "load")
        {
            addnumber = 10000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxload;
            addnumber /= 10000;
            addnumber = addnumber * 10000;
            addnumber = Mathf.Floor(addnumber) / 10000;
            _slider.fillAmount = addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].loadtime;
        }
        else if (sliderType == "lv")
        {
            addnumber = 1000000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxExp;
            addnumber /= 1000000;
            addnumber = addnumber * 1000000;
            addnumber = Mathf.Floor(addnumber) / 1000000;
            _slider.fillAmount = addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].inputExp;
        }
    }

    void Update()
    {
        if (sliderType == "audio")
        {
            if (addnumber != 100 / 1 || _slider.fillAmount != addnumber * GManager.instance.audioMax)
            {
                addnumber = 100 / 1;
                addnumber /= 100;
                addnumber = addnumber * 100;
                addnumber = Mathf.Floor(addnumber) / 100;
                _slider.fillAmount = addnumber * GManager.instance.audioMax;
            }
        }
        else if (sliderType == "se")
        {
            if (addnumber != 100 / 1 || _slider.fillAmount != addnumber * GManager.instance.seMax)
            {
                addnumber = 100 / 1;
                addnumber /= 100;
                addnumber = addnumber * 100;
                addnumber = Mathf.Floor(addnumber) / 100;
                _slider.fillAmount = addnumber * GManager.instance.seMax;
            }
        }
        else if (sliderType == "rotpivot")
        {
            if (addnumber != 1000 / 4 || _slider.fillAmount != addnumber * GManager.instance.rotpivot)
            {
                addnumber = 1000 / 4;
                addnumber /= 1000;
                addnumber = addnumber * 1000;
                addnumber = Mathf.Floor(addnumber) / 1000;
                _slider.fillAmount = addnumber * GManager.instance.rotpivot;
            }
        }
        else if (sliderType == "boss" && boss != null)
        {
            if (addnumber != 1000000 / script.Estatus.health || script.Estatus.health * addnumber != _slider.fillAmount)
            {
                //    boss = GameObject.Find(bossname);
                //    script = boss.GetComponent<enemyS>();
                addnumber = 1000000 / oldbosshp;
                addnumber /= 1000000;
                addnumber = addnumber * 1000000;
                addnumber = Mathf.Floor(addnumber) / 1000000;
                _slider.fillAmount = addnumber * script.Estatus.health;
            }
        }
        else if (sliderType == "kando")
        {
            if (addnumber != 100 / 5 || _slider.fillAmount != addnumber * GManager.instance.kando)
            {
                addnumber = 100 / 5;
                addnumber /= 100;
                addnumber = addnumber * 100;
                addnumber = Mathf.Floor(addnumber) / 100;
                _slider.fillAmount = addnumber * GManager.instance.kando;
            }
        }
        else if (sliderType == "mode")
        {
            if (addnumber != 100 / 2 || _slider.fillAmount != addnumber * GManager.instance.mode)
            {
                addnumber = 100 / 2;
                addnumber /= 100;
                addnumber = addnumber * 100;
                addnumber = Mathf.Floor(addnumber) / 100;
                _slider.fillAmount = addnumber * GManager.instance.mode;
            }
        }
        else if (sliderType == "hp")
        {
            if (addnumber != 10000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxHP || _slider.fillAmount != addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].hp)
            {
                addnumber = 10000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
                addnumber /= 10000;
                addnumber = addnumber * 10000;
                addnumber = Mathf.Floor(addnumber) / 10000;
                _slider.fillAmount = addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].hp;
            }
        }
        else if (sliderType == "ehp")
        {
            if (addnumber != 10000 / es.Estatus.maxhp || _slider.fillAmount != addnumber * es.Estatus.health)
            {
                addnumber = 10000 / es.Estatus.maxhp;
                addnumber /= 10000;
                addnumber = addnumber * 10000;
                addnumber = Mathf.Floor(addnumber) / 10000;
                _slider.fillAmount = addnumber * es.Estatus.health;
            }
        }
        else if (sliderType == "mp")
        {
            if (addnumber != 10000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxMP || _slider.fillAmount != addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].mp)
            {
                addnumber = 10000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxMP;
                addnumber /= 10000;
                addnumber = addnumber * 10000;
                addnumber = Mathf.Floor(addnumber) / 10000;
                _slider.fillAmount = addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].mp;
            }
        }
        else if (sliderType == "load")
        {
            if (addnumber != 10000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxload || _slider.fillAmount != addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].loadtime )
            {
                addnumber = 10000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxload;
                addnumber /= 10000;
                addnumber = addnumber * 10000;
                addnumber = Mathf.Floor(addnumber) / 10000;
                _slider.fillAmount = addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].loadtime;
            }
        }
        else if (sliderType == "lv" && GManager.instance.Pstatus[GManager.instance.playerselect].Lv < 100)//いずれ限界突破に対応させる
        {
            for (int l = 0; l < GManager.instance.Pstatus.Length;)
            {
                if (GManager.instance.Pstatus[l].getpl == 1 && GManager.instance.Pstatus[l].inputExp >= GManager.instance.Pstatus[l].maxExp)
                {
                    GManager.instance.Pstatus[l].inputExp -= GManager.instance.Pstatus[l].maxExp;
                    GManager.instance.Pstatus[l].maxExp = (GManager.instance.Pstatus[l].maxExp * 5)/4;
                    GManager.instance.Pstatus[l].Lv += 1;
                    GameObject p = GameObject.Find("Player");
                    if (GManager.instance.playerselect == l && p != null)
                    {
                        Instantiate(GManager.instance.effectobj[3], p.transform.position, GManager.instance.effectobj[0].transform.rotation, p.transform);
                    }
                    GManager.instance.setrg = 12;
                    GManager.instance.Pstatus[l].maxHP += 2 + GManager.instance.Pstatus[l].add_hp;
                    GManager.instance.Pstatus[l].attack += 1+ GManager.instance.Pstatus[l].add_at;
                    GManager.instance.Pstatus[l].defense += 1+ GManager.instance.Pstatus[l].add_df;
                    GManager.instance.Pstatus[l].hp += 2+ GManager.instance.Pstatus[l].add_hp;
                    GManager.instance.Pstatus[l].maxMP += 2+ GManager.instance.Pstatus[l].add_mp;
                    GManager.instance.Pstatus[l].mp += 2+ GManager.instance.Pstatus[l].add_mp;
                    if (GManager.instance.isEnglish == 0)
                    {
                        GManager.instance.txtget = GManager.instance.Pstatus[l].pname + "はレベルUPした！";
                    }
                    else if (GManager.instance.isEnglish == 1)
                    {
                        GManager.instance.txtget = "The " + GManager.instance.Pstatus[l].pname2 + " has leveled up！";
                    }
                    if (GManager.instance.Pstatus[l].getMagic != null || GManager.instance.Pstatus[l].getMagic.Length != 0)
                    {
                        for (int i = 0; i < GManager.instance.Pstatus[l].getMagic.Length;)
                        {
                            if (GManager.instance.Pstatus[l].getMagic[i].gettrg < 1 && GManager.instance.Pstatus[l].Lv > (GManager.instance.Pstatus[l].getMagic[i].inputlevel - 1) && GManager.instance.Pstatus[l].getMagic[i].inputlevel != -1)
                            {
                                GManager.instance.Pstatus[l].getMagic[i].gettrg = 1;
                                if (GManager.instance.isEnglish == 0)
                                {
                                    GManager.instance.txtget = GManager.instance.Pstatus[l].pname + "は" + GManager.instance.MagicID[GManager.instance.Pstatus[l].getMagic[i].magicid].magicname + "を習得した！";
                                }
                                else if (GManager.instance.isEnglish == 1)
                                {
                                    GManager.instance.txtget = GManager.instance.Pstatus[l].pname2 + " have mastered " + GManager.instance.MagicID[GManager.instance.Pstatus[l].getMagic[i].magicid].magicname2 + "！";
                                }

                            }
                            i++;
                        }
                    }
                }
                l++;
            }
            //----
            if (addnumber != 1000000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxExp || _slider.fillAmount != addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].inputExp)
            {
                addnumber = 1000000 / GManager.instance.Pstatus[GManager.instance.playerselect].maxExp;
                addnumber /= 1000000;
                addnumber = addnumber * 1000000;
                addnumber = Mathf.Floor(addnumber) / 1000000;
                _slider.fillAmount = addnumber * GManager.instance.Pstatus[GManager.instance.playerselect].inputExp;
            }
        }

    }
}