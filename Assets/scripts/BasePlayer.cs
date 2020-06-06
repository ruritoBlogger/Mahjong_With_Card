using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// プレイヤーのベース関数
public abstract class BasePlayer : MonoBehaviour
{
    public int zihu;
    public string name;

    private List<int> hands;
    private List<int> used;

    public void Setup(int tmp_zihu, string tmp_name)
    {
        // 自風牌の設定
        zihu = tmp_zihu;
        // プレイヤー名の設定
        name = tmp_name;
        //初期化
        hands = new List<int>();
        used = new List<int>();
    }

    public void Reset()
    {
        hands = new List<int>();
        used = new List<int>();
    }

    // 手持ちの牌
    public List<int> Hands { set; get; }
    
    // 捨てた牌
    public List<int> Used { set; get; }

    public string Name { set; get; }
    
    // プレイヤーの手牌のポジションを管理する
    public Vector3 HandsPosition { set; get; }
    
    // プレイヤーの手牌のポジションを管理する
    public Vector3 DumpedPosition { set; get; }
    
    // プレイヤーの手牌のポジションを管理する
    public Vector3 Direction { set; get; }

    
    // 山から取って来た牌を手持ちに追加
    public void AddNewPai(int newPai)
    {
        hands.Add(newPai);
        hands.Sort();
    }

    // 牌を捨てる処理
    public int DumpPai()
    {
        int choiced = ChoicePai();
        int choiced_pai = hands[choiced];
        used.Add(choiced_pai);
        hands.Remove(choiced_pai);
        /*
        string tmp = "";
        for( int i = 0; i < Hands.Count; i++ )
        {
            tmp += Hands[i].ToString();
            tmp += " ";
        }
        Debug.Log(tmp);
        */

        return choiced_pai;
    }

    // どの牌を捨てるか選ぶ部分
    public abstract int ChoicePai();

}
