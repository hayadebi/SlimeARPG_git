using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class qteUI : MonoBehaviour
{
    [Header("【QTEより簡単に作れるUI】\n-動かすテキストのアニメーションをセット"), SerializeField]
    Animator[] pop_anim;
    public Text upattack;
    [Header("ユーザーにタップさせるボタンをセット(prefabのqteButton)"),SerializeField]
    GameObject qte_button;
    [Header("ボタンを配置させる親オブジェクトをセット(オススメはmain)"), SerializeField]
    Transform parent_button;
    [Header("出現させたいボタンの数だけ作る(中身は触らなくてOK)"), SerializeField]
    public int qte_number;
    [System.Serializable]
    public struct square
    {
        public int[] square_x;
    }
    [Header("配列。いじらなくてOK")]
    public square[] square_y;
    private AudioSource audio_s;
    [Header("ボタンが出現する時の効果音"), SerializeField]
    AudioClip all_popse;
    [Header("今押せるボタンがアニメーションする時の効果音"), SerializeField]
    AudioClip turn_popse;
    [Header("ボタン押した時の効果音"), SerializeField]
    AudioClip push_popse;
    [Header("ここから下は触らなくて大丈夫)")]
    public GameObject[] set_qte;
    public qteID[] set_qteid;

    public int index_x;
    public int index_y;
    public int input_number;
    public int randomN = 4;
    private int _rm = 1;
    public int input_loop;
    private int selectN = 0;
    private bool start_push = false;
    private float timer = 0;
    private float atnumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        audio_s = this.GetComponent<AudioSource>();
        set_qte = new GameObject[qte_number];
        set_qteid = new qteID[qte_number];
        Invoke("summonUI", 1f);
    }
    public void clickQTE()
    {
        if(!start_push )
        {
            start_push = true;
        }

        set_qteid[selectN].bt_anim.SetInteger("Anumber", 3);
        audio_s.PlayOneShot(push_popse);
        selectN += 1;
        atnumber /= timer;
        if (selectN >= qte_number)
        {
            pop_anim[1].gameObject.SetActive(true);
            pop_anim[1].SetInteger("Anumber", 2);
            GManager.instance.ESCtrg = true;
            start_push = false;
            //upattack.text = "×" + atnumber.ToString ();
        }
        timer = 0;
        if (selectN < qte_number)
        {
            set_qteid[selectN].bt_anim.SetInteger("Anumber", 2);
        }

    }
    void summonUI()
    {
        while (input_loop < 5 || input_number < qte_number)
        {
            index_x = 0;
            index_y = 0;
            for (; 3 > index_y;)
            {
                for (; 6 > index_x;)
                {
                    if (square_y[index_y].square_x[index_x] == 0 && Random.Range(0, randomN) == 0 && input_number < qte_number)
                    {
                        _rm = Random.Range(1, 4);
                        switch (_rm )
                        {
                            case 1:
                                _rm = 385;
                                break;
                            case 2:
                                _rm = 625;
                                break;
                            case 3:
                                _rm = 865;
                                break;
                        }
                        set_qte[input_number] = Instantiate(qte_button, new Vector3(260 + (240 * index_x),_rm ,0), transform.rotation, parent_button);
                        set_qteid[input_number] = set_qte[input_number].GetComponent<qteID>();
                        input_number += 1;
                        set_qteid[input_number - 1].button_id = input_number;
                        set_qteid[input_number - 1].bt_text.text = input_number.ToString();
                        square_y[index_y].square_x[index_x] = input_number;
                    }
                    else if(input_number >= qte_number)
                    {
                        input_loop = 5;
                    }
                    index_x += 1;
                }
                index_y += 1;
            }
            
            input_loop += 1;
        }

        StartCoroutine(UIPlay());
    }
    private void Update()
    {
       if(start_push )
        {
            timer += (Time.deltaTime/5);
        }
        
    }

    IEnumerator UIPlay()
    {
        for (int i = 0; i < set_qte.Length;)
        {
            if (set_qteid[i] != null)
            {
                audio_s.PlayOneShot(all_popse);
                set_qteid[i].bt_anim.SetInteger("Anumber", 1);
            }
            i++;
            yield return new WaitForSeconds(0.15f);
        }
        audio_s.PlayOneShot(turn_popse);
        if (set_qteid[0] != null)
        {
            set_qteid[0].bt_anim.SetInteger("Anumber", 2);
        }
        if (set_qteid[0] != null)
        {
            pop_anim[0].gameObject.SetActive(true);
            pop_anim[0].SetInteger("Anumber", 2);
        }
    }

}