using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayer : BasePlayer
{
    // ランダムに切る牌を選ぶ
    public override int ChoicePai()
    {
        int key = (int)UnityEngine.Random.Range(0f, 13f);
        StartCoroutine(DelayMethod(1.0f, () =>
        {
            Debug.Log("あああああああああああああああああああああああああああああああ");
        }));
        Debug.Log("test2");
        return key;
    }

    IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
