using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlimeContent : MonoBehaviour
{
    public int slimeId = -1;
    public Text[] slimeName;
    public Image[] slimeImage;
    private int oldId = -1;
    public SlimeView slimeView;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (slimeId != oldId && slimeId != -1)
        {
            foreach (Text _text in slimeName)
            {
                _text.text = DataManager.instance.Pstatus[slimeId].pname;
            }
            foreach (Image _img in slimeImage)
            {
                _img.sprite = DataManager.instance.Pstatus[slimeId].pimage;
            }
        }
    }

    public void SelectSlime()
    {
        slimeView.SelectSlime(slimeId);
    }
}
