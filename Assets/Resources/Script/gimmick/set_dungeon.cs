using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_dungeon : MonoBehaviour
{
    [Header("親、ダンジョンのマネージャーを格納")]
    public set_dungeon dungeon_manager;
    [Header("0=マネージャー,1=部屋,2=出入口")]
    public int manager_mode = 0;
    [Header("index 0は最初の部屋")]
    public set_dungeon[] room_prefab;
    [System.Serializable]
    public struct SetRoomID
    {
        [Header("x軸…index番目")]
        public int[] room_id;
    }
    [Header("y軸…index番目")]
    public SetRoomID[] set_roomID;

    [Header("プレイヤーが現在いる部屋[y][x]")]
    public int[] player_room;

    //現在の部屋セットを取得しやすいように格納
    private GameObject get_roomobj = null;
    public Transform start_pos;
    [Header("※各部屋オブジェクト用")]
    public bool North = false;
    public bool South = false;
    public bool East = false;
    public bool West = false;
    [Header("各部屋オブジェの子オブジェクト出入口移動量[y][x]")]
    public int[] chid_moveroom;
    [Header("各方角のTP位置(北南東西)")]
    public Transform[] tp_pos;

    private GameObject P;
    private bool move_trg = false;
    public Transform[] child_tmpP;

    private float tmpx;
    private float tmpy;
    public bool parent_use = false;
    public bool debug_trg = false;
    // Start is called before the first frame update
    void Start()
    {
        if (manager_mode == 0)
        {
            P = GameObject.Find("Player");
            if (!debug_trg)
            {
                for (int y = 0; y < set_roomID.Length;)
                {
                    for (int x = 0; x < set_roomID[y].room_id.Length;)
                    {
                        if (set_roomID[y].room_id[x] == 0)
                        {
                            player_room[0] = y;
                            player_room[1] = x;
                            get_roomobj = Instantiate(room_prefab[set_roomID[player_room[0]].room_id[player_room[1]]].gameObject, this.transform.position, this.transform.rotation, this.transform);

                            P.transform.position = start_pos.position;
                        }
                        x++;
                    }
                    y++;
                }
            }
            else if (debug_trg)
            {
                for (int y = 0; y < set_roomID.Length;)
                {
                    for (int x = 0; x < set_roomID[y].room_id.Length;)
                    {
                        if (set_roomID[y].room_id[x] != -1)
                        {
                            Vector3 tmp_pos = new Vector3(this.transform.position.x+x *(256*1.25f), this.transform.position.y, this.transform.position.z+y * (256*1.25f));
                            get_roomobj = Instantiate(room_prefab[set_roomID[y].room_id[x]].gameObject, tmp_pos, this.transform.rotation, this.transform);

                        }
                        x++;
                    }
                    y++;
                }
            }
        }
        else
        {
            dungeon_manager = GameObject.Find("dungeon_main").GetComponent<set_dungeon>();
        }
        move_trg = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void room_move(int movey = 0,int movex=0)
    {
        player_room[0] += movey;
        player_room[1] += movex;
        tmpx = movex;
        tmpy = movey;
        Destroy(get_roomobj);
        parent_use = true;
        get_roomobj = Instantiate(room_prefab[set_roomID[player_room[0]].room_id[player_room[1]]].gameObject, this.transform.position, this.transform.rotation, this.transform);
        Invoke(nameof(dungeon_move), 0.1f);
    }
    void dungeon_move()
    {
        if (child_tmpP != null && child_tmpP.Length > 0)
        {
            if (tmpy > 0)
            {
                P.transform.position = GameObject.Find("北").transform.position;
            }
            else if (tmpy < 0)
            {
                P.transform.position = GameObject.Find("南").transform.position;
            }
            else if (tmpx > 0)
            {
                P.transform.position = GameObject.Find("西").transform.position;
            }
            else if (tmpx < 0)
            {
                P.transform.position = GameObject.Find("東").transform.position;
            }
        }
        else
        {
            if (tmpy > 0)
            {
                P.transform.position = tp_pos[0].position;
            }
            else if (tmpy < 0)
            {
                P.transform.position = tp_pos[1].position;
            }
            else if (tmpx > 0)
            {
                P.transform.position = tp_pos[2].position;
            }
            else if (tmpx < 0)
            {
                P.transform.position = tp_pos[3].position;
            }
        }
        parent_use = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(!move_trg && manager_mode == 2 && GManager.instance.walktrg && !GManager.instance.over && GManager.instance.setmenu <= 0 && col.tag == "Player" && dungeon_manager != null && !dungeon_manager.parent_use)
        {
            move_trg = true;
            dungeon_manager.child_tmpP = tp_pos;
            dungeon_manager.room_move(chid_moveroom[0], chid_moveroom[1]);
            
        }
    }

}
