using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


// プレイヤーのベース関数
public abstract class BasePlayer : MonoBehaviour
{
    public int zihu;
    public string name;

    public void Setup(int tmp_zihu, string tmp_name)
    {
        // 自風牌の設定
        zihu = tmp_zihu;
        // プレイヤー名の設定
        name = tmp_name;
        //初期化
        Hands = new List<int>();
        Used = new List<int>();
    }

    public void Reset()
    {
        Hands = new List<int>();
        Used = new List<int>();
    }

    // 手持ちの牌
    public List<int> Hands { get; set; }

    // GameObjectと接続している手牌
    public Dictionary<int, GameObject> Hand_Objects { get; set; }
    
    // 捨てた牌
    public List<int> Used { get; set; }

    public string Name { get; set; }
    
    // プレイヤーの手牌のポジションを管理する
    public Vector3 HandsPosition { get; set; }
    
    // プレイヤーの手牌のポジションを管理する
    public Vector3 DumpedPosition { get; set; }
    
    // プレイヤーの手牌のポジションを管理する
    public Vector3 Direction { get; set; }
    
    // 山から取って来た牌を手持ちに追加
    public void AddNewPai(int newPai)
    {
        Hands.Add(newPai);
        Hands.Sort();
    }

    public void SetHandsObject(List<GameObject> pais)
    {
        Hand_Objects = new Dictionary<int, GameObject>();
        for(int i = 0; i < pais.Count; i++ )
        {
            Hand_Objects.Add(i, pais[i]);
        }
    }

    // 牌を捨てる処理
    public async UniTask<int> DumpPai()
    {
        int choiced = await ChoicePai();
        int choiced_pai = Hands[choiced];
        Used.Add(choiced_pai);
        Hands.Remove(choiced_pai);
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

    // プレイヤーのターンが始まった際の処理
    public abstract void ResetTurn();

    // どの牌を捨てるか選ぶ部分
    public abstract UniTask<int> ChoicePai();

}
