using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startUIset : MonoBehaviour
{
    public bool colorTrg = false;
    public Color clset;
    public Image im;
    public Text fonText;
    public Font fon;
    
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        if (GManager.instance.subgameTrg == true && im != null && sprite != null)
        {
            im.sprite = sprite;
        }
        if(GManager.instance.subgameTrg == false && fonText != null && fon != null)
        {
            fonText.font = fon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (colorTrg == true && GManager.instance.subgameTrg == false && fonText != null && fonText.color != clset)
        {
            fonText.color = clset;
        }
        if (colorTrg == true && GManager.instance.subgameTrg == false && im != null && im.color != clset)
        {
            im.color = clset;
        }
    }
}
