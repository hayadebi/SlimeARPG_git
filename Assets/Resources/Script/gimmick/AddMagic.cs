using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMagic : MonoBehaviour
{
    public int Damage = 1;
    public bool nokill = false;
    public string changeType = "";
    public int magicID = 0;
    public GameObject[] obj;
    public AudioSource audioS;
    public AudioClip[] ase;
    public float destroytime = 5;
    public bool[] trg;
    public float[] time;
    public int[] indexn;
    public bool enemytrg = false;
    private GameObject P = null;
    public enemyS inputEs = null;
    public bool cmshake = false;
    private Vector3 targetP;
    public bool _childTrg = false;
    public int change_rot = 0;
    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.Find("Player");
        Destroy(gameObject, destroytime);
        Invoke("Magic" + magicID, 0.1f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void Magic0()
    {
        audioS.PlayOneShot(ase[0]);
        if(enemytrg == false)
        {
            for (int i = 0; i < 1;)
            {
                var newP = GManager.instance.mouseP;
                newP.y += 2.1f;
                Vector3 vec = newP - this.transform.position;
                vec.Normalize();
                vec = Quaternion.Euler(0, 0, 0) * vec;
                if (GManager.instance.subgameTrg == false)
                {
                    vec *= 100;
                }
                else if (GManager.instance.subgameTrg == true)
                {
                    vec *= 40;
                }
                GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation);
                t.transform.localScale = this.transform.localScale;
                t.tag = "pbullet";
                if (changeType != "")
                {
                    t.GetComponent<AddDamage>().attacktype = changeType;
                }
                t.GetComponent<AddDamage>().Damage += Damage;
                t.GetComponent<AddDamage>().nokill = nokill;
                t.GetComponent<Rigidbody>().velocity = vec;
                i++;
            }
        }
        else if(enemytrg == true)
        {
            for (int i = 0; i < 1;)
            {
                if (P != null)
                {
                    var newP = P.transform.position;
                    newP.y += 2.1f;
                    Vector3 vec = newP - this.transform.position;
                    vec.Normalize();
                    vec = Quaternion.Euler(0, 0, 0) * vec;
                    if (GManager.instance.subgameTrg == false)
                    {
                        vec *= 100;
                    }
                    else if (GManager.instance.subgameTrg == true)
                    {
                        vec *= 40;
                    }
                    GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation);
                    t.transform.localScale = this.transform.localScale;
                    t.tag = "ebullet";
                    if (changeType != "")
                    {
                        t.GetComponent<AddDamage>().attacktype = changeType;
                    }
                    t.GetComponent<AddDamage>().Damage += Damage;
                    t.GetComponent<AddDamage>().nokill = nokill;
                    t.GetComponent<Rigidbody>().velocity = vec;
                }
                i++;
            }
        }
    }
    void Magic1()
    {
        audioS.PlayOneShot(ase[0]);
        if (enemytrg == false)
        {
            for (int i = 0; i < 1;)
            {
                var newP = GManager.instance.mouseP;
                newP.y += 2f;
                Vector3 vec = newP - this.transform.position;
                vec.Normalize();
                vec = Quaternion.Euler(0, 0, 0) * vec;
                if (GManager.instance.subgameTrg == false)
                {
                    vec *= 60;
                }
                else if (GManager.instance.subgameTrg == true)
                {
                    vec *= 15;
                }
                if (!_childTrg)
                {
                    GameObject t = Instantiate(obj[1], this.transform.position, this.transform.rotation);
                    t.transform.localScale = this.transform.localScale;
                    t.tag = "pbullet";
                    if (changeType != "")
                    {
                        t.GetComponent<AddDamage>().attacktype = changeType;
                    }
                    t.GetComponent<AddDamage>().Damage += Damage;
                    t.GetComponent<AddDamage>().nokill = nokill;
                    t.GetComponent<Rigidbody>().velocity = vec;
                }
                else if (_childTrg)
                {
                    GameObject t = Instantiate(obj[1], this.transform.position, this.transform.rotation,this.transform);
                    t.transform.localScale = this.transform.localScale;
                    t.tag = "pbullet";
                    if (changeType != "")
                    {
                        t.GetComponent<AddDamage>().attacktype = changeType;
                    }
                    t.GetComponent<AddDamage>().Damage += Damage;
                    t.GetComponent<AddDamage>().nokill = nokill;
                    t.GetComponent<Rigidbody>().velocity = vec;
                }
                i++;
            }
        }
        else if (enemytrg == true)
        {
            for (int i = 0; i < 1;)
            {
                if (P != null)
                {
                    var newP = P.transform.position;
                    newP.y += 2.1f;
                    Vector3 vec = newP - this.transform.position;
                    vec.Normalize();
                    vec = Quaternion.Euler(0, 0, 0) * vec;
                    if (GManager.instance.subgameTrg == false)
                    {
                        vec *= 60;
                    }
                    else if (GManager.instance.subgameTrg == true)
                    {
                        vec *= 15;
                    }
                    GameObject t = Instantiate(obj[1], this.transform.position, this.transform.rotation);
                    t.transform.localScale = this.transform.localScale;
                    t.tag = "ebullet";
                    if (changeType != "")
                    {
                        t.GetComponent<AddDamage>().attacktype = changeType;
                    }
                    t.GetComponent<AddDamage>().Damage += Damage;
                    t.GetComponent<AddDamage>().nokill = nokill;
                    t.GetComponent<Rigidbody>().velocity = vec;
                }
                i++;
            }
        }
    }
    void Magic2()
    {
        audioS.PlayOneShot(ase[0]);
        if (enemytrg == false)
        {
            for (int i = 0; i < 3;)
            {
                var newP = GManager.instance.mouseP;
                newP.y += 3.2f;
                Vector3 vec = newP - obj[0].transform.position;
                vec.Normalize();
                vec = Quaternion.Euler(0, -15 + (15*i), 0) * vec;
                if (GManager.instance.subgameTrg == true)
                {
                    vec = Quaternion.Euler(0-(15*i), 0, 0) * vec;
                }
                if (GManager.instance.subgameTrg == false)
                {
                    vec *= 100;
                }
                else if (GManager.instance.subgameTrg == true)
                {
                    vec *= 10;
                }
                GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation);
                t.transform.localScale = this.transform.localScale;
                t.tag = "pbullet";
                if (changeType != "")
                {
                    t.GetComponent<AddDamage>().attacktype = changeType;
                }
                t.GetComponent<AddDamage>().Damage += Damage;
                t.GetComponent<AddDamage>().nokill = nokill;
                t.GetComponent<Rigidbody>().velocity = vec;
                i++;
            }
        }
        else if (enemytrg == true)
        {
            for (int i = 0; i < 3;)
            {
                if (P != null)
                {
                    var newP = P.transform.position;
                    newP.y += 3.2f;
                    Vector3 vec = newP - obj[0].transform.position;
                    vec.Normalize();
                    vec = Quaternion.Euler(0, -15 + (15 * i), 0) * vec;
                    if (GManager.instance.subgameTrg == true)
                    {
                        vec = Quaternion.Euler(0-(15*i), 0, 0) * vec;
                    }
                    if (GManager.instance.subgameTrg == false)
                    {
                        vec *= 100;
                    }
                    else if (GManager.instance.subgameTrg == true)
                    {
                        vec *= 10;
                    }
                    GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation);
                    t.transform.localScale = this.transform.localScale;
                    t.tag = "ebullet";
                    if (changeType != "")
                    {
                        t.GetComponent<AddDamage>().attacktype = changeType;
                    }
                    t.GetComponent<AddDamage>().Damage += Damage;
                    t.GetComponent<AddDamage>().nokill = nokill;
                    t.GetComponent<Rigidbody>().velocity = vec;
                }
                i++;
            }
        }
    }
    void Magic3()
    {
        audioS.PlayOneShot(ase[0]);
        if (enemytrg == false)
        {
            for (int i = 0; i < 3;)
            {
                var newP = GManager.instance.mouseP;
                newP.y += 2f;
                Vector3 vec = newP - obj[0].transform.position;
                vec.Normalize();
                vec = Quaternion.Euler(-20, -20 + (20 * i), 0) * vec;
                if (GManager.instance.subgameTrg == true)
                {
                    vec = Quaternion.Euler(0-(20*i), 0, 0) * vec;
                }
                if (GManager.instance.subgameTrg == false)
                {
                    vec *= 90;
                }
                else if (GManager.instance.subgameTrg == true)
                {
                    vec *= 10;
                }
                GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation);
                t.transform.localScale = this.transform.localScale;
                t.tag = "pbullet";
                if (changeType != "")
                {
                    t.GetComponent<AddDamage>().attacktype = changeType;
                }
                t.GetComponent<AddDamage>().Damage += Damage;
                t.GetComponent<AddDamage>().nokill = nokill;
                t.GetComponent<Rigidbody>().velocity = vec;
                i++;
            }
        }
        else if (enemytrg == true)
        {
            for (int i = 0; i < 3;)
            {
                if (P != null)
                {
                    var newP = P.transform.position;
                    newP.y += 2f;
                    Vector3 vec = newP - obj[0].transform.position;
                    vec.Normalize();
                    vec = Quaternion.Euler(-20, -20 + (20 * i), 0) * vec;
                    if (GManager.instance.subgameTrg == true)
                    {
                        vec = Quaternion.Euler(0 - (20 * i), 0, 0) * vec;
                    }
                    if (GManager.instance.subgameTrg == false)
                    {
                        vec *= 90;
                    }
                    else if (GManager.instance.subgameTrg == true)
                    {
                        vec *= 10;
                    }
                    GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation);
                    t.transform.localScale = this.transform.localScale;
                    t.tag = "ebullet";
                    if (changeType != "")
                    {
                        t.GetComponent<AddDamage>().attacktype = changeType;
                    }
                    t.GetComponent<AddDamage>().Damage += Damage;
                    t.GetComponent<AddDamage>().nokill = nokill;
                    t.GetComponent<Rigidbody>().velocity = vec;
                }
                i++;
            }
        }
    }
    void Magic4()
    {
        if (enemytrg == false)
        {
            GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation, this.transform);
            t.transform.localScale = this.transform.localScale;
            t.tag = "Player";
            if (changeType != "")
            {
                t.GetComponent<AddDamage>().attacktype = changeType;
            }
            t.GetComponent<AddDamage>().Damage += Damage;
            t.GetComponent<AddDamage>().nokill = nokill;
        }
        else if (enemytrg == true)
        {
            if (P != null)
            {
                GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation,this.transform);
                t.transform.localScale = this.transform.localScale;
                t.tag = "enemy";
                if (changeType != "")
                {
                    t.GetComponent<AddDamage>().attacktype = changeType;
                }
                t.GetComponent<AddDamage>().Damage += Damage;
                t.GetComponent<AddDamage>().nokill = nokill;
            }
        }
        Invoke("setSE", time[0]);
    }
    void Magic5()
    {
        if (time != null && time.Length != 0 && time[0] > 0)
        {
            Invoke("Lug_shot", time[0]);
        }
        else
        {
            Lug_shot();
        }
    }
    void Lug_shot()
    {
        audioS.PlayOneShot(ase[0]);
        if (cmshake)
        {
            iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("y", 1f, "x", 1f, "time", 1f));
        }
        if (enemytrg == false)
        {
            GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation, this.transform);
            t.transform.localScale = this.transform.localScale;
            t.tag = "Player";
            if (changeType != "")
            {
                t.GetComponent<AddDamage>().attacktype = changeType;
            }
            t.GetComponent<AddDamage>().Damage += Damage;
            t.GetComponent<AddDamage>().nokill = nokill;
        }
        else if (enemytrg == true)
        {
            if (P != null)
            {
                GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation, this.transform);
                t.transform.localScale = this.transform.localScale;
                t.tag = "enemy";
                if (changeType != "")
                {
                    t.GetComponent<AddDamage>().attacktype = changeType;
                }
                t.GetComponent<AddDamage>().Damage += Damage;
                t.GetComponent<AddDamage>().nokill = nokill;
            }
        }
    }
    void Magic6()
    {
        audioS.PlayOneShot(ase[0]);
        if (enemytrg == false)
        {
            GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation, this.transform);
            t.transform.localScale = this.transform.localScale;
            t.tag = "Player";
            GManager.instance.Pstatus[GManager.instance.playerselect].hp += (GManager.instance.Pstatus[GManager.instance.playerselect].maxHP / 3);
            if(GManager.instance.Pstatus[GManager.instance.playerselect].hp > GManager.instance.Pstatus[GManager.instance.playerselect].maxHP)
            {
                GManager.instance.Pstatus[GManager.instance.playerselect].hp = GManager.instance.Pstatus[GManager.instance.playerselect].maxHP;
            }
        }
        else if (enemytrg == true)
        {
            if (inputEs != null)
            {
                GameObject t = Instantiate(obj[1], obj[0].transform.position, this.transform.rotation, this.transform);
                t.transform.localScale = this.transform.localScale;
                t.tag = "enemy";
                inputEs.Estatus.health += (inputEs.Estatus.maxhp / 3);
                if(inputEs.Estatus.health > inputEs.Estatus.maxhp)
                {
                    inputEs.Estatus.health = inputEs.Estatus.maxhp;
                }
            }
        }
    }
    void setSE()
    {
        audioS.PlayOneShot(ase[0]);
    }
    void Magic7()
    {
        audioS.PlayOneShot(ase[0]);
        if (enemytrg == false )
        {
            if (obj[1] != null)
            {
                GameObject t = Instantiate(obj[1], this.transform.position, this.transform.rotation, this.transform);
                t.transform.localScale = this.transform.localScale;
                t.tag = "Player";
            }
            targetP = GManager.instance.mouseP;
        }
        else if (enemytrg == true )
        {
            if (obj[1] != null)
            {
                GameObject t = Instantiate(obj[1], this.transform.position, this.transform.rotation, this.transform);
                t.transform.localScale = this.transform.localScale;
                t.tag = "enemy";
            }
            targetP = P.transform.position;
        }
        targetP.y = this.transform.position.y;
        Invoke("meteoV", time[0]);
    }

    void meteoV()
    {
        if (enemytrg == false)
        {
            GameObject t = Instantiate(obj[2], targetP, this.transform.rotation);
            foreach (Transform childT in t.transform)
            {
                childT.tag = "Player";
            }
        }
        else if (enemytrg == true)
        {
            if (P != null)
            {
                GameObject t = Instantiate(obj[2], targetP, this.transform.rotation);
                foreach (Transform childT in t.transform)
                {
                    childT.tag = "enemy";
                }
            }
        }
    }
    void Magic8()
    {
        Invoke("twinShot", time[0]);
    }
    void twinShot()
    {
        audioS.PlayOneShot(ase[0]);
        if (enemytrg == false)
        {
            for (int i = 0; i < 2;)
            {
                var newP = GManager.instance.mouseP;
                newP.y += 2f;
                Vector3 vec = newP - obj[i].transform.position;
                vec.Normalize();
                vec = Quaternion.Euler(0, 0, 0) * vec;
                if (GManager.instance.subgameTrg == false)
                {
                    vec *= 90;
                }
                else if (GManager.instance.subgameTrg == true)
                {
                    vec *= 10;
                }
                GameObject t = Instantiate(obj[2], obj[i].transform.position, this.transform.rotation);
                t.transform.localScale = this.transform.localScale;
                t.tag = "pbullet";
                if (changeType != "")
                {
                    t.GetComponent<AddDamage>().attacktype = changeType;
                }
                t.GetComponent<AddDamage>().Damage += Damage;
                t.GetComponent<AddDamage>().nokill = nokill;
                t.GetComponent<Rigidbody>().velocity = vec;
                i++;
            }
        }
        else if (enemytrg == true)
        {
            for (int i = 0; i < 2;)
            {
                if (P != null)
                {
                    var newP = P.transform.position;
                    newP.y += 2f;
                    Vector3 vec = newP - obj[i].transform.position;
                    vec.Normalize();
                    vec = Quaternion.Euler(0, 0, 0) * vec;
                    if (GManager.instance.subgameTrg == false)
                    {
                        vec *= 90;
                    }
                    else if (GManager.instance.subgameTrg == true)
                    {
                        vec *= 10;
                    }
                    GameObject t = Instantiate(obj[2], obj[i].transform.position, this.transform.rotation);
                    t.transform.localScale = this.transform.localScale;
                    t.tag = "ebullet";
                    if (changeType != "")
                    {
                        t.GetComponent<AddDamage>().attacktype = changeType;
                    }
                    t.GetComponent<AddDamage>().Damage += Damage;
                    t.GetComponent<AddDamage>().nokill = nokill;
                    t.GetComponent<Rigidbody>().velocity = vec;
                }
                i++;
            }
        }
    }
    void Magic9()
    {
        audioS.PlayOneShot(ase[0]);
        Invoke("seV", time[0]);
    }
    void seV()
    {
        audioS.PlayOneShot(ase[1]);
        if (enemytrg == false)
        {
            if (obj[1] != null)
            {
                GameObject t = Instantiate(obj[1], this.transform.position, this.transform.rotation, this.transform);
                t.transform.localScale = this.transform.localScale;
                t.tag = "Player";
            }
            targetP = GManager.instance.mouseP;
        }
        else if (enemytrg == true)
        {
            if (obj[1] != null)
            {
                GameObject t = Instantiate(obj[1], this.transform.position, this.transform.rotation, this.transform);
                t.transform.localScale = this.transform.localScale;
                t.tag = "Player";
            }
            targetP = P.transform.position;
        }
        targetP.y = this.transform.position.y;
        Invoke("impactV", time[1]);
    }
    void impactV()
    {
        if (enemytrg == false)
        {
            GameObject t = Instantiate(obj[2], targetP, this.transform.rotation, this.transform);
            t.transform.localScale = this.transform.localScale;
            t.tag = "Player";
            if (changeType != "")
            {
                t.GetComponent<AddDamage>().attacktype = changeType;
            }
            t.GetComponent<AddDamage>().Damage += Damage;
            t.GetComponent<AddDamage>().nokill = nokill;
        }
        else if (enemytrg == true)
        {
            if (P != null)
            {
                GameObject t = Instantiate(obj[2], targetP, this.transform.rotation, this.transform);
                t.transform.localScale = this.transform.localScale;
                t.tag = "enemy";
                if (changeType != "")
                {
                    t.GetComponent<AddDamage>().attacktype = changeType;
                }
                t.GetComponent<AddDamage>().Damage += Damage;
                t.GetComponent<AddDamage>().nokill = nokill;
            }
        }
    }

    void Magic12()
    {
        audioS.PlayOneShot(ase[0]);
        if (enemytrg == false)
        {
            for (int i = 0; i < indexn[0];)
            {
                var newP = GManager.instance.mouseP;
                newP.y += 2.1f;
                Vector3 vec = newP - obj[0].transform.position;
                vec.Normalize();
                vec = Quaternion.Euler(0, -(indexn[1]/2) + (indexn[1] / indexn[0])*i, 0) * vec;
                if (GManager.instance.subgameTrg == true)
                {
                    vec = Quaternion.Euler(0 - ((indexn[1] / indexn[0]) * i), 0, 0) * vec;
                }
                if (GManager.instance.subgameTrg == false)
                {
                    vec *= 100;
                }
                else if (GManager.instance.subgameTrg == true)
                {
                    vec *= 40;
                }
                GameObject t = Instantiate(obj[1], obj[0].transform.position, transform.rotation);
                t.tag = "pbullet";
                var bulletR = t.transform.localEulerAngles;
                bulletR.y += indexn[1] / indexn[0]*i+change_rot;
                t.transform.localEulerAngles = bulletR;
                if (changeType != "")
                {
                    t.GetComponent<AddDamage>().attacktype = changeType;
                }
                t.GetComponent<AddDamage>().Damage += Damage;
                t.GetComponent<AddDamage>().nokill = nokill;
                t.GetComponent<Rigidbody>().velocity = vec;
                i++;
            }
        }
        else if (enemytrg == true)
        {
            for (int i = 0; i < indexn[0];)
            {
                if (P != null)
                {
                    var newP = P.transform.position;
                    newP.y += 2.1f;
                    Vector3 vec = newP - obj[0].transform.position;
                    vec.Normalize();
                    vec = Quaternion.Euler(0, -(indexn[1] / 2) + ((indexn[1] / indexn[0]) * i), 0) * vec;
                    if (GManager.instance.subgameTrg == true)
                    {
                        vec = Quaternion.Euler(0 - ((indexn[1] / indexn[0]) * i), 0, 0) * vec;
                    }
                    if (GManager.instance.subgameTrg == false)
                    {
                        vec *= 100;
                    }
                    else if (GManager.instance.subgameTrg == true)
                    {
                        vec *= 40;
                    }
                    GameObject t = Instantiate(obj[1], obj[0].transform.position, transform.rotation);
                    t.tag = "ebullet";
                    var bulletR = t.transform.localEulerAngles;
                    bulletR.y += indexn[1] / indexn[0] * i + change_rot;
                    t.transform.localEulerAngles = bulletR;
                    if (changeType != "")
                    {
                        t.GetComponent<AddDamage>().attacktype = changeType;
                    }
                    t.GetComponent<AddDamage>().Damage += Damage;
                    t.GetComponent<AddDamage>().nokill = nokill;
                    t.GetComponent<Rigidbody>().velocity = vec;
                }
                i++;
            }
        }
    }
}
