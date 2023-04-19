using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damageEffect : MonoBehaviour
{
    [Header("-生成場所をセット")]
    [Header("※ボタンそれぞれで設定する"), SerializeField]
    Transform generation_position;
    [Header(" これの子オブジェクトにテキスト配置")]
    [Header("-生成オブジェクト(エフェクトのキャンバス)をセット"), SerializeField]
    GameObject generation_obj;
    [Header("-ボタン押した後、何秒後に生成するか"), SerializeField]
    float generation_time = 1;
    [Header("-生成後、何秒後に消すか"), SerializeField]
    float destroy_time = 2;
    [Header("-攻撃強さの選択(1=小,2=中,3=強)"), SerializeField]
    int attack_mode = 1;
    [Header("-小攻撃のダメージ量"), SerializeField]
    int minattack_damage = 10;
    [Header("-中攻撃のダメージ量"), SerializeField]
    int normalattack_damage = 50;
    [Header("-強攻撃のダメージ量"), SerializeField]
    int maxattack_damage = 100;
    [Header("※ここから必須ではない")]
    [Header("-押す音を鳴らしたいならオーディオソースセット"), SerializeField]
    AudioSource audio_s = null;
    [Header("-効果音をセット"), SerializeField]
    AudioClip set_se = null;

    private Text damage_text;
    private GameObject setobj;
    // Start is called before the first frame update
    void Start()
    {

    }
    //ここをボタンから呼び出してください。
    public void DamageClick()
    {
        if (audio_s != null && set_se != null)
        {
            audio_s.PlayOneShot(set_se);
        }
        Invoke("SummonEffect", generation_time);
    }

    //これは後から勝手に呼び出すやつ※気にするな
    public void SummonEffect()
    {
        setobj = Instantiate(generation_obj, generation_position.position, transform.rotation, generation_position);
        if (setobj != null && setobj.transform.GetChild(0).gameObject.GetComponent<Text>())
        {
            damage_text = setobj.transform.GetChild(0).gameObject.GetComponent<Text>();
            if (attack_mode == 1)
            {
                //もし表示がおかしかったら".ToString()"を消して
                damage_text.text = minattack_damage.ToString() + "ダメージ！";
            }
            else if (attack_mode == 2)
            {
                //もし表示がおかしかったら".ToString()"を消して
                damage_text.text = normalattack_damage.ToString() + "ダメージ！";
            }
            else if (attack_mode == 3)
            {
                //もし表示がおかしかったら".ToString()"を消して
                damage_text.text = maxattack_damage.ToString() + "ダメージ！";
            }
        }
        Destroy(setobj.gameObject, destroy_time);

    }
}
