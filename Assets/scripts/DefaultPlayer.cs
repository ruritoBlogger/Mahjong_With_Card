using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayer : BasePlayer
{
    // ランダムに切る牌を選ぶ
    public override int ChoicePai()
    {
        int key = (int)Random.Range(0f, 13f);
        StartCoroutine(WaitChoicingPai());
        return key;
    }

    IEnumerator WaitChoicingPai()
    {
        yield return new WaitForSeconds(3);
    }
}
