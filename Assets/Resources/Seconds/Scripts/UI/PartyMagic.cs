using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PartyMagic : MonoBehaviour
{
    public int partyId = 0;
    public Text[] slimeName;
    public Image[] slimeIcon;
    public Sprite noneIconimage;
    public MagicView magicView;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        if (DataManager.instance.playerselect[partyId] != -1 && DataManager.instance.playerselect[partyId] >= 0)
        {
            foreach (Text name in slimeName)
            {
                name.text = DataManager.instance.Pstatus[DataManager.instance.playerselect[partyId]].pname;
            }
            foreach (Image icon in slimeIcon)
            {
                icon.sprite = DataManager.instance.Pstatus[DataManager.instance.playerselect[partyId]].pimage;
            }
        }
        else
        {
            foreach (Text name in slimeName)
            {
                name.text = "????????";
            }
            foreach (Image icon in slimeIcon)
            {
                icon.sprite = noneIconimage;
            }
        }
    }

    public void PartyClick()
    {
        magicView.PartyClick(partyId);
    }
}
