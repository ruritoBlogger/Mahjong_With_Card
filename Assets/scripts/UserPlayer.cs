using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System;

public class UserPlayer : BasePlayer
{
    GameObject clickedGameObject;

    private void Start()
    {
        clickedGameObject = null;
    }

    // クリックを検知する
    private void Update()
    {
        if( Input.GetMouseButtonDown(0))
        {
            clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if( Physics.Raycast(ray, out hit))
            {
                clickedGameObject = hit.collider.gameObject;
            }
            
        }
    }

    // リセットする際にクリックされたオブジェクト情報を初期化する

    public void Reset()
    {
        Hands = new List<int>();
        Used = new List<int>();
        ResetTurn();
    }

    // プレイヤーのターンが始まった際にオブジェクト情報を初期化する
    public void ResetTurn()
    {
        clickedGameObject = null;
    }


    public async override UniTask<int> ChoicePai()
    {
        // クリックされるまで待機
        await new WaitWhile(() => clickedGameObject == null);

        Debug.Log(Math.Abs((int)(HandsPosition.z - 6 - clickedGameObject.transform.position.z)));
        return Math.Abs((int)(HandsPosition.z -6 - clickedGameObject.transform.position.z));
    }
}
