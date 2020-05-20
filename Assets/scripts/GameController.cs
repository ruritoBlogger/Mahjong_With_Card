using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private List<int> menber_turn_list;
    private List<BasePlayer> menber_list;

    private List<string> player_names = new List<string>() { "AaA", "BbB", "CcC", "DdD" };

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
        Main();
    }

    // メイン関数
    void Main()
    {
        // 牌を並べる部分


        // ドラチェック


        // 実際に打つ部分


        // 上がりチェック


        // もし流局すれば流局処理


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
}
