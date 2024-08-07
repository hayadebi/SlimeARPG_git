using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteAnim : MonoBehaviour
{
    private Image img;
    private SpriteRenderer spriteren;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Image>()) img = GetComponent<Image>();
        if(GetComponent<SpriteRenderer>()) spriteren = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(img&&spriteren&&spriteren!=null&&spriteren.sprite!=img.sprite)
        {
            img.sprite = spriteren.sprite;
        }
    }
}
