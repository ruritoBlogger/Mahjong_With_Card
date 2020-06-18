using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : BasePlayer
{
    GameObject clickedGameObject;

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
        else
        {
            clickedGameObject = null;
        }
    }


    public override int ChoicePai()
    {
        while ( clickedGameObject == null )
        {
        }
        Debug.Log((int)(HandsPosition.x - clickedGameObject.transform.position.x));
        return (int)(HandsPosition.x - clickedGameObject.transform.position.x);
    }
}
