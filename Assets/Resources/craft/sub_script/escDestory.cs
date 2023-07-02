using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escDestory : MonoBehaviour
{
    public int seTrg = -1;
    public bool mouseesctrg = false;
    [Header("1個前のUIに戻す指定変数(+1される)")] public int inputUInumber = -1;
    public bool mousetrg = false;
    public Animator ui = null;
    public string animname;
    public float destroytime = 0.1f;
    bool inputon = false;
    public bool pushesc_anim = false;
    // Start is called before the first frame update
    void Start()
    {
        if (mousetrg == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        Invoke("trgOn", 1);
    }
    void trgOn()
    {
        inputon = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(pushesc_anim )
        {
            GManager.instance.ESCtrg = true;
            pushesc_anim = false;
        }
        if (inputon == true && inputUInumber == -1)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || GManager.instance.ESCtrg)
            {
                GManager.instance.ESCtrg = false;
                PlayerPrefs.SetFloat("audioMax", GManager.instance.audioMax);
                PlayerPrefs.SetFloat("seMax", GManager.instance.seMax);
                PlayerPrefs.SetInt("mode", GManager.instance.mode);
                PlayerPrefs.SetFloat("kando", GManager.instance.kando);
                PlayerPrefs.SetInt("autoattack", GManager.instance.autoattack);
                PlayerPrefs.SetFloat("rotpivot", GManager.instance.rotpivot);
                PlayerPrefs.Save();
                GManager.instance.setmenu = 0;
                GManager.instance.walktrg = true;
                Destroy(gameObject, destroytime);
                ////試験的にアンロード
                //Resources.UnloadUnusedAssets();
                //-----------------
                if (ui != null)
                {
                    ui.Play(animname);
                    if(seTrg != -1)
                    {
                        GManager.instance.setrg = seTrg;
                    }
                }
            }
            else if (mouseesctrg == true)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || GManager.instance.ESCtrg || Input.GetKeyDown(KeyCode.Return))
                {
                    GManager.instance.ESCtrg = false;
                    PlayerPrefs.SetFloat("audioMax", GManager.instance.audioMax);
                    PlayerPrefs.SetFloat("seMax", GManager.instance.seMax);
                    PlayerPrefs.SetInt("mode", GManager.instance.mode);
                    PlayerPrefs.SetFloat("kando", GManager.instance.kando);
                    PlayerPrefs.SetInt("autoattack", GManager.instance.autoattack);
                    PlayerPrefs.SetFloat("rotpivot", GManager.instance.rotpivot);
                    PlayerPrefs.Save();
                    GManager.instance.setmenu = 0;
                    GManager.instance.walktrg = true;
                    Destroy(gameObject, destroytime);
                    //-----------------
                    if (ui != null)
                    {
                        ui.Play(animname);
                        if (seTrg != -1)
                        {
                            GManager.instance.setrg = seTrg;
                        }
                    }
                }
            }
        }
        else if(inputon == true && (inputUInumber + 1) > GManager.instance.setmenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || GManager.instance.ESCtrg == true)
            {
                PlayerPrefs.SetFloat("audioMax", GManager.instance.audioMax);
                PlayerPrefs.SetFloat("seMax", GManager.instance.seMax);
                PlayerPrefs.SetInt("mode", GManager.instance.mode);
                PlayerPrefs.SetFloat("kando", GManager.instance.kando);
                PlayerPrefs.SetInt("autoattack", GManager.instance.autoattack);
                PlayerPrefs.SetFloat("rotpivot", GManager.instance.rotpivot);
                PlayerPrefs.Save();
                GManager.instance.setmenu -= 1;
                if (GManager.instance.setmenu < 1)
                {
                    GManager.instance.setmenu = 0;
                    GManager.instance.ESCtrg = false;
                    GManager.instance.walktrg = true;
                    if (mousetrg == true)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                }
                if (ui != null)
                {
                    GManager.instance.ESCtrg = false;
                    ui.Play(animname);
                    if (seTrg != -1)
                    {
                        GManager.instance.setrg = seTrg;
                    }
                }
                Destroy(gameObject, destroytime);
                //-----------------
            }
            else if(mouseesctrg == true)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || GManager.instance.ESCtrg || Input.GetKeyDown(KeyCode.Return))
                {
                    PlayerPrefs.SetFloat("audioMax", GManager.instance.audioMax);
                    PlayerPrefs.SetFloat("seMax", GManager.instance.seMax);
                    PlayerPrefs.SetInt("mode", GManager.instance.mode);
                    PlayerPrefs.SetFloat("kando", GManager.instance.kando);
                    PlayerPrefs.SetInt("autoattack", GManager.instance.autoattack);
                    PlayerPrefs.SetFloat("rotpivot", GManager.instance.rotpivot);
                    PlayerPrefs.Save();
                    GManager.instance.setmenu -= 1;
                    if (GManager.instance.setmenu < 1)
                    {
                        GManager.instance.setmenu = 0;
                        GManager.instance.ESCtrg = false;
                        GManager.instance.walktrg = true;
                    }
                    if (ui != null)
                    {
                        ui.Play(animname);
                        if (seTrg != -1)
                        {
                            GManager.instance.setrg = seTrg;
                        }
                    }
                    Destroy(gameObject, destroytime);
                }
            }
        }
    }

    public void BtnEsc()
    {
        GManager.instance.ESCtrg = true;
        if (pushesc_anim)
        {
            GManager.instance.ESCtrg = true;
            pushesc_anim = false;
        }
        if (inputon == true && inputUInumber == -1 && GManager.instance.ESCtrg)
        {
            GManager.instance.ESCtrg = false;
            PlayerPrefs.SetFloat("audioMax", GManager.instance.audioMax);
            PlayerPrefs.SetFloat("seMax", GManager.instance.seMax);
            PlayerPrefs.SetInt("mode", GManager.instance.mode);
            PlayerPrefs.SetFloat("kando", GManager.instance.kando);
            PlayerPrefs.SetInt("autoattack", GManager.instance.autoattack);
            PlayerPrefs.SetFloat("rotpivot", GManager.instance.rotpivot);
            PlayerPrefs.Save();
            GManager.instance.setmenu = 0;
            GManager.instance.walktrg = true;
            Destroy(gameObject, destroytime);
            ////試験的にアンロード
            //Resources.UnloadUnusedAssets();
            //-----------------
            if (ui != null)
            {
                GManager.instance.ESCtrg = false;
                ui.Play(animname);
                if (seTrg != -1)
                {
                    GManager.instance.setrg = seTrg;
                }
            }
        }
        else if (inputon == true && (inputUInumber + 1) > GManager.instance.setmenu && GManager.instance.ESCtrg)
        {
            PlayerPrefs.SetFloat("audioMax", GManager.instance.audioMax);
            PlayerPrefs.SetFloat("seMax", GManager.instance.seMax);
            PlayerPrefs.SetInt("mode", GManager.instance.mode);
            PlayerPrefs.SetFloat("kando", GManager.instance.kando);
            PlayerPrefs.SetInt("autoattack", GManager.instance.autoattack);
            PlayerPrefs.SetFloat("rotpivot", GManager.instance.rotpivot);
            PlayerPrefs.Save();
            GManager.instance.setmenu -= 1;
            if (GManager.instance.setmenu < 1)
            {
                GManager.instance.setmenu = 0;
                GManager.instance.ESCtrg = false;
                GManager.instance.walktrg = true;
                if (mousetrg == true)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            if (ui != null)
            {
                GManager.instance.ESCtrg = false;
                ui.Play(animname);
                if (seTrg != -1)
                {
                    GManager.instance.setrg = seTrg;
                }
            }
            Destroy(gameObject, destroytime);
            //-----------------
        }
    }
}
