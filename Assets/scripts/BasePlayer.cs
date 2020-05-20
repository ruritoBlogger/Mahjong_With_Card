﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// プレイヤーのベース関数
public abstract class BasePlayer : MonoBehaviour
{
    public int zihu;
    public string name;

    private List<int> hands;

    public void Setup(int tmp_zihu, string tmp_name)
    {
        // 自風牌の設定
        zihu = tmp_zihu;
        // プレイヤー名の設定
        name = tmp_name;
    }

    // 手持ちの牌
    public List<int> Hands
    {
        set { this.hands = value; }
        get { return this.hands; }
    }

    // 山から取って来た牌を手持ちに追加
    public void AddNewPai(int newPai)
    {
        hands.Add(newPai);
    }

    // 牌を捨てる処理
    public void DumpPai()
    {
        int choiced_pai = ChoicePai();
        hands.Remove(choiced_pai);
    }

    // どの牌を捨てるか選ぶ部分
    public abstract int ChoicePai();
}
