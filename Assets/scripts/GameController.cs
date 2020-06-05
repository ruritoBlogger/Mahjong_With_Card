using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    // 牌コントローラー
    public GameObject PaiController;

    private List<int> menber_turn_list;
    private List<BasePlayer> menber_list = new List<BasePlayer>();

    // 麻雀牌のゲームオブジェクトを管理する
    private List<GameObject> pai_object_list;

    // プレイヤーの位置を管理する
    private List<Vector3> player_position;

    // プレイヤー視点で左側から右側への方角
    private List<Vector3> player_direction;

    // 麻雀牌(変更不能) 
    private List<int> sorted_pai_list;

    // 麻雀牌の順番を管理
    private List<int> pai_list;

    private List<string> player_names = new List<string>() { "AaA", "BbB", "CcC", "DdD" };

    private bool flag = true;

    void Start()
    {
        // 親と順番を決める
        menber_turn_list = ShuffleNumber(4);

        // Playerを初期化する
        for (int i = 0; i < 4; i++)
        {
            DefaultPlayer tmp_player = new DefaultPlayer();
            tmp_player.Setup(menber_turn_list[i], player_names[i]);
            menber_list.Add(tmp_player);
        }


        /*------------------------
         *  1
         *4   2
         *  3
         *  
         * プレイヤーの位置を初期化する
         */

        player_position = new List<Vector3>();
        player_direction = new List<Vector3>();
        for( int i = 0; i < 4; i++ )
        {
            int x_key = 0;
            int z_key = 0;
            if (i == 0) x_key -= 10;
            else if (i == 1) z_key += 10;
            else if (i == 2) x_key += 10;
            else z_key -= 10;
            player_position.Add(new Vector3(x_key, 0.0f, z_key));
            player_direction.Add(new Vector3(-z_key/10, 0.0f, x_key/10));
        }

        // 麻雀牌を生成する
        sorted_pai_list = new List<int>();
        for ( int i = 0; i < 136; i++ )
        {
            sorted_pai_list.Add(i);
        }

        // 麻雀牌を紐づける
        pai_object_list = new List<GameObject>( GameObject.FindGameObjectsWithTag("pai") );

        Debug.Log(pai_object_list.Count);
    }

    void Update()
    {
        // 牌を並べる部分
        pai_list = ShuffleNumber(sorted_pai_list.Count);

        for( int i = 0; i < menber_list.Count; i++ )
        {
            List<int> tmp = new List<int>();
            for( int j = 0; j < 13; j++ )
            {
                tmp.Add(sorted_pai_list[ pai_list[i*13+j]-1 ]);
            }
            GetPlayer(i).Reset();
            GetPlayer(i).Hands = tmp;
        }



        // ドラチェック

        bool isGetReward = false;
        for (int i = 13 * menber_list.Count; i < pai_list.Count; i++)
        {
            int menber_key = i % 4;
            // 実際に打つ部分
            int pai = sorted_pai_list[pai_list[i] - 1];
            GetPlayer(menber_key).AddNewPai(pai);

            // 上がりチェック
            if (PaiController.GetComponent<PaiController>().CheckPoint(GetPlayer(menber_key).Hands, false, false, false, false) != 0)
            {
                // 点棒処理
                Debug.Log("あがったよ");
                Debug.Log("-----------------------------------------------------");
                isGetReward = true;
                break;
            }
            else
            {
                // 牌を捨てる処理
                int dumped_pai = GetPlayer(menber_key).DumpPai();

                // 捨てた牌を表示する
                if(true)
                {
                    MovePai(pai_object_list[dumped_pai],
                            new Vector3( player_direction[menber_key].x * GetPlayer(menber_key).Used.Count%7,
                                         player_position[menber_key].y,
                                         player_direction[menber_key].z * GetPlayer(menber_key).Used.Count/7) );
                }
            }
            if (flag)
            {
                //MovePai(pai_object_list[pai_list[i] - 1], i - 13 * menber_list.Count + 1);
            }
            //if (i - 13 * menber_list.Count == 10 && flag )
            if ( true )
            {
                List<GameObject> hands = new List<GameObject>();
                foreach( int hand in GetPlayer(menber_key).Hands )
                {
                    hands.Add(pai_object_list[hand]);
                }
                MovePais(hands, 
                         new Vector3( player_position[menber_key].x - 6 * player_direction[menber_key].x,
                                      player_position[menber_key].y,
                                      player_position[menber_key].z - 6 * player_direction[menber_key].z),
                         player_direction[menber_key]);
                flag = false;
            }
        }
        // もし流局すれば流局処理
        if( !isGetReward )
        {
            //Debug.Log("流局したよ");
        }

        // 順位処理
    }

    // 順番をランダムに決める
    private List<int> ShuffleNumber(int key)
    {
        List<int> list = new List<int>();

        for (int i = 0; i < key; i++)
        {
            list.Add(i + 1);
        }

        for (int i = 0; i < list.Count; i++)
        {
            int tmp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = tmp;
        }

        return list;
    }

    private BasePlayer GetPlayer(int key)
    {
        return menber_list[menber_turn_list[key] - 1];
    }

    private void MovePai(GameObject pai, Vector3 key)
    {
        pai.transform.position = key;
    }

    // 基準座標から横並びさせる
    private void MovePais(List<GameObject> pais, Vector3 point, Vector3 direction)
    {
        foreach( GameObject pai in pais )
        {
            pai.transform.position = point;
            point = new Vector3(point.x+direction.x, point.y+direction.y, point.z+direction.z);
        }
    }
}
