using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{

    // 牌コントローラー
    public GameObject PaiController;

    // プレイヤーのprefab
    public GameObject player_prefab;

    private List<int> menber_turn_list;
    private List<BasePlayer> menber_list = new List<BasePlayer>();

    // 麻雀牌のゲームオブジェクトを管理する
    private List<GameObject> pai_object_list;

    // 麻雀牌(変更不能) 
    private List<int> sorted_pai_list;

    // 麻雀牌の順番を管理
    private List<int> pai_list;

    private List<string> player_names = new List<string>() { "AaA", "BbB", "CcC", "DdD" };

    private bool flag = true;

    // 現在のゲームモード
    private int game_mode;

    // 直前のゲームモード
    private int last_mode;

    void Start()
    {
        // 親と順番を決める
        menber_turn_list = ShuffleNumber(4);

        // Playerを初期化する
        for (int i = 0; i < 4; i++)
        {
            GameObject tmp = Instantiate(player_prefab) as GameObject;
            if (i == 1)
            {
                tmp.AddComponent(typeof(UserPlayer));
                UserPlayer tmp_player = tmp.GetComponent<UserPlayer>();
                tmp_player.Setup(menber_turn_list[i], player_names[i]);
                menber_list.Add(tmp_player);
            }
            else
            {
                tmp.AddComponent(typeof(DefaultPlayer));
                DefaultPlayer tmp_player = tmp.GetComponent<DefaultPlayer>();
                tmp_player.Setup(menber_turn_list[i], player_names[i]);
                menber_list.Add(tmp_player);
            }
            
        }


        /*------------------------
         *  1
         *4   2
         *  3
         *  
         * プレイヤーの位置を初期化する
         */

        for( int i = 0; i < 4; i++ )
        {
            int x_key = 0;
            int z_key = 0;
            if (i == 0) x_key -= 10;
            else if (i == 1) z_key += 10;
            else if (i == 2) x_key += 10;
            else z_key -= 10;
            //player_position.Add(new Vector3(x_key, 0.0f, z_key));
            //player_direction.Add(new Vector3(-z_key/10, 0.0f, x_key/10));
            GetPlayer(i).HandsPosition = new Vector3(x_key, 0.0f, z_key);
            GetPlayer(i).DumpedPosition = new Vector3(x_key / 2, 0.0f, z_key / 2);
            GetPlayer(i).Direction = new Vector3(-z_key / 10, 0.0f, x_key / 10);
        }

        // 麻雀牌を生成する
        sorted_pai_list = new List<int>();
        for ( int i = 0; i < 136; i++ )
        {
            sorted_pai_list.Add(i);
        }

        // 麻雀牌を紐づける
        pai_object_list = new List<GameObject>( GameObject.FindGameObjectsWithTag("pai") );

        // ゲームモードの設定
        game_mode = 4;
        last_mode = -1;
    }

    /*
     * ゲームモードについて
     * 
     * -1: 例外処理
     *  1: ゲーム進行
     *  2: 上がり処理
     *  3: 親の交代処理
     *  4: 牌の順番初期化
     */

    void Update()
    {
        // ゲームが進行した場合
        if (game_mode != last_mode)
        {
            last_mode = game_mode;
            if (game_mode == -1)
            {
                Debug.Log("エラーが出たよ");
            }
            else if (game_mode == 1)
            {
                ProgressGame();
            }
            else if (game_mode == 2)
            {
                Debug.Log("未実装");
                game_mode = 4;
            }
            else if (game_mode == 3)
            {
                Debug.Log("親を切り替えたよ");
                game_mode = 4;
            }
            else if (game_mode == 4)
            {
                Initialize();
            }
        }
    }

    private void Initialize()
    {
        // 牌を並べる部分
        pai_list = ShuffleNumber(sorted_pai_list.Count);

        for (int i = 0; i < menber_list.Count; i++)
        {
            List<int> tmp = new List<int>();
            for (int j = 0; j < 13; j++)
            {
                tmp.Add(sorted_pai_list[pai_list[i * 13 + j] - 1]);
            }
            GetPlayer(i).Reset();
            GetPlayer(i).Hands = tmp;
        }

        // 牌の位置を初期化する
        for( int i = 0; i < pai_object_list.Count; i++ )
        {
            MovePai(pai_object_list[i], new Vector3(0, -1, 0));
        }

        // 次のゲームモードに移行
        game_mode = 1;
    }

    private async void ProgressGame()
    {
        // ドラチェック

        bool isGetReward = false;
        for (int i = 13 * menber_list.Count; i < pai_list.Count; i++)
        {
            int menber_key = i % 4;
            BasePlayer player = GetPlayer(menber_key);
            // 実際に打つ部分
            int pai = sorted_pai_list[pai_list[i] - 1];
            player.AddNewPai(pai);

            // 上がりチェック
            if (PaiController.GetComponent<PaiController>().CheckPoint(player.Hands, false, false, false, false) != 0)
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
                int dumped_pai = player.DumpPai();
                await Task.Delay(100);

                // 捨てた牌を表示する
                if(true)
                {
                    MovePai(pai_object_list[dumped_pai],
                            new Vector3( player.DumpedPosition.x + player.Direction.x * ((player.Used.Count-1)%7-3) + (int)((player.Used.Count-1)/7 * player.Direction.z),
                                         player.DumpedPosition.y,
                                         player.DumpedPosition.z + player.Direction.z * ((player.Used.Count-1)%7-3) + (int)((player.Used.Count-1)/7 * (-player.Direction.x) )) );
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
                foreach( int hand in player.Hands )
                {
                    hands.Add(pai_object_list[hand]);
                }
                MovePais(hands, 
                         new Vector3( player.HandsPosition.x - 6 * player.Direction.x,
                                      player.HandsPosition.y,
                                      player.HandsPosition.z - 6 * player.Direction.z),
                         player.Direction);
                flag = false;
            }
        }
        // もし流局すれば流局処理
        if( !isGetReward )
        {
            //Debug.Log("流局したよ");
        }

        game_mode = 3;
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
            int randomIndex = UnityEngine.Random.Range(0, list.Count);
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
