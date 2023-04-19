using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exp : MonoBehaviour
{
    public float speed;
    public string tagname = "exp";
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.6f);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(tagname); //「Cube」タグのついたオブジェクトを全て検索して配列にいれる

        if (cubes.Length == 0) return; // 「Cube」タグがついたオブジェクトがなければ何もしない。

        foreach (GameObject cube in cubes) // 配列に入れた一つひとつのオブジェクト
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbodyがあれば、グレネードを中心とした爆発の力を加える
            {
                Rigidbody rb = cube.GetComponent<Rigidbody>();
                Vector3 velocity = (cube.transform.position - transform.position).normalized * speed;

                //風力を与える
                rb.AddForce(velocity,ForceMode.VelocityChange);
            }
        }
    }

    void Explode()
    {
        
    }
}