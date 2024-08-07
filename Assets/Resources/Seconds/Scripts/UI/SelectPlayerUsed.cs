using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectPlayerUsed : MonoBehaviour
{
    public int playerIndex = 1;
    public ItemView itemView;
    public Image playerIcon;
    public Sprite noneIconImage;
    public Text hpmpText;
    public Text deathText;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        if(DataManager.instance.playerselect[playerIndex]!=-1&& DataManager.instance.playerselect[playerIndex] >= 0)
        {
            playerIcon.sprite = DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].pimage;
            int maxhp = (int)((DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].maxHP + DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].add_hp) * (1 + (DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].Lv / 1.5f)));
            int maxmp= (int)((DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].maxMP + DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].add_mp) * (1 + (DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].Lv / 1.5f)));
            hpmpText.text = "HP:"+ DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].hp.ToString()+"/"+ maxhp.ToString() + "\nMP:" + DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].mp.ToString() + "/" + maxmp.ToString();
            if (DataManager.instance.Pstatus[DataManager.instance.playerselect[playerIndex]].hp <= 0) deathText.gameObject.SetActive(true);
            else deathText.gameObject.SetActive(false);
        }
        else
        {
            playerIcon.sprite = noneIconImage;
            hpmpText.text = "HP:0/0\nMP:0/0";
            deathText.gameObject.SetActive(false);
        }
    }
    public void ClickSelect()
    {
        itemView.ClickSelectedPlayer(playerIndex);
    }
}
