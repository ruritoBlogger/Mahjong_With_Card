using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class DefaultPlayer : BasePlayer
{
    // ランダムに切る牌を選ぶ
    public override UniTask<int> ChoicePai()
    {
        return UniTask.FromResult((int)UnityEngine.Random.Range(0f, 13f));
    }

    // ランダムに牌を切るだけなので特に処理は実行しない
    public override void ResetTurn() { }
}
