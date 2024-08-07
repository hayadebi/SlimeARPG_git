using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ContentItem : MonoBehaviour
{
    public int itemId = -1;
    public Text[] itemName;
    public Text[] itemNum;
    public Image[] img;
    private int oldId = -1;
    public bool useType = false;
    public ItemView itemView;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (itemId != oldId && itemId != -1)
        {
            foreach (Text _text in itemName)
            {
                _text.text = DataManager.instance.ItemID[itemId].itemname;
            }
            foreach (Text _text in itemNum)
            {
                _text.text = DataManager.instance.ItemID[itemId].itemnumber.ToString()+"個所持";
            }
            foreach (Image _img in img)
            {
                _img.sprite = DataManager.instance.ItemID[itemId].itemimage;
            }
            if (DataManager.instance.ItemID[itemId].isMenuUse) useType = true;
            else if (!DataManager.instance.ItemID[itemId].isMenuUse) useType = false;
        }
    }

    public void ClickContent()
    {
        itemView.ContentViewUpdate(itemId);
    }
}
