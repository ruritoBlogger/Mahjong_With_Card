using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    // 牌コントローラー
    public GameObject PaiController;

    private List<int> menber_turn_list;
    private List<BasePlayer> menber_list = new List<BasePlayer>();

    // 麻雀牌(変更不能) 
    private List<int> sorted_pai_list;

    // 麻雀牌の順番を管理
    private List<int> pai_list;

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

        // 麻雀牌を取得する
        sorted_pai_list = PaiController.GetComponent<PaiController>().GetTotalList();
    }

    void Update()
    {
        // 牌を並べる部分
        pai_list = ShuffleNumber(sorted_pai_list.Count);

        // ドラチェック
        
        for( int i = 0; i < pai_list.Count; i++ )
        {
            int menber_key = i % 4;
            // 実際に打つ部分
            int pai = sorted_pai_list[pai_list[i]-1];
            GetPlayer(menber_key).AddNewPai(pai);

            // 上がりチェック
            if( PaiController.GetComponent<PaiController>().CheckPoint(GetPlayer(menber_key).Hands,false,false,false,false) != 0 )
            {
                // 点棒処理

            }
            else
            {
                // 牌を捨てる処理
                GetPlayer(menber_key).DumpPai();
            }
        }
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

    private BasePlayer GetPlayer(int key)
    {
        return menber_list[menber_turn_list[key] - 1];
    }
}
