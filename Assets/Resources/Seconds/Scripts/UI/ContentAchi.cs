using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ContentAchi : MonoBehaviour
{
    public int achiId=-1;
    public Text[] achiName;
    public Text[] achiDoc;
    public Image[] img;
    private int oldId = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (achiId != oldId && achiId != -1)
        {
            foreach(Text _text in achiName)
            {
                _text.text = DataManager.instance.achievementsID[achiId].name;
            }
            foreach (Text _text in achiDoc)
            {
                _text.text = DataManager.instance.achievementsID[achiId].description;
            }
            foreach (Image _img in img)
            {
                _img.sprite = DataManager.instance.achievementsID[achiId].image;
            }
        }
    }
}
