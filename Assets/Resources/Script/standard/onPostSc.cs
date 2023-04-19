using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class onPostSc : MonoBehaviour
{
    //public bool depthTrg = false;
    //public float depthdistance = 4.5f;
    public PostProcessVolume ppv;
    private DepthOfField _depthOFfield;
    public bool depth_walk = true;
    public bool depthTrg = true;
    // Start is called before the first frame update
    void Start()
    {
        if(GManager.instance.reduction == 1)
        {
            ppv.enabled = false;
        }
        else if (GManager.instance.reduction == 0)
        {
            ppv.enabled = true;
        }
        
        foreach (PostProcessEffectSettings item in ppv.profile.settings)
        {
            if (item as DepthOfField)
            {
                _depthOFfield = item as DepthOfField;
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (depthTrg)
        {
            if (depth_walk && _depthOFfield.active && !GManager.instance.walktrg)
            {
                _depthOFfield.active = false;
            }
            else if (depth_walk && !_depthOFfield.active && GManager.instance.walktrg)
            {
                _depthOFfield.active = true;
            }
            //if (depthTrg == true && _depthOFfield && depthdistance != _depthOFfield.focusDistance.value)
            //{
            //    _depthOFfield.focusDistance.value = depthdistance;
            //}
            if (GManager.instance.reduction == 1 && ppv.enabled == true)
            {
                ppv.enabled = false;
            }
            else if (GManager.instance.reduction == 0 && ppv.enabled == false)
            {
                ppv.enabled = true;
            }
        }
        else if(!depthTrg && _depthOFfield.active)
        {
            _depthOFfield.active = false;
        }
    }
}
