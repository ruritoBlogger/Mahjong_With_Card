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
                Debug.Log(clickedGameObject);
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
    public override void ResetTurn()
    {
        clickedGameObject = null;
    }


    public async override UniTask<int> ChoicePai()
    {
        // クリックされるまで待機
        //await new WaitWhile(() => clickedGameObject == null );
        do
        {
            clickedGameObject = await WaitClicking();
        }
        while (!Hand_Objects.ContainsKey(clickedGameObject));

        /*
        return Math.Abs((int)(HandsPosition.z -6*Direction.z 
                              - clickedGameObject.transform.position.z
                              + HandsPosition.x -6*Direction.x
                              - clickedGameObject.transform.position.x));
        */
        return Hand_Objects[clickedGameObject];
    }

    private async UniTask<GameObject> WaitClicking()
    {
        await new WaitWhile(() => clickedGameObject == null);
        return clickedGameObject;
    }
}
