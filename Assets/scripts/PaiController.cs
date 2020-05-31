using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaiController : MonoBehaviour
{
    public List<int> total_list;

    void Start()
    {
        // 麻雀牌を生成する
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 10; j++)
            {
                if (i * 10 + j > 37)
                {
                    break;
                }
                total_list.Add(i * 10 + j);
                total_list.Add(i * 10 + j);
                total_list.Add(i * 10 + j);
                total_list.Add(i * 10 + j);
            }
        }
    }

    /*-------------------------------------------------------
     * 渡されたlistで上がれる時、何点かどうか判断する
     * 渡されたlistはソートされているものとする
     * 
     * list : 14牌入ってる。この14牌で何点かどうか判断
     * isMenzen: 門前かどうか
     * isReached: 立直しているかどうか
     * isNaki: 鳴いているかどうか
     * isIppatsu: 一発かどうか
     * 
     */

    public int CheckPoint(List<int> list, bool isMenzen, bool isReached, bool isNaki, bool isIppatsu)
    {
        int yaku = 0;
        // 頭が無いと成り立たない役を分ける
        //if (isHead(list))
        if (true)
        {
            yaku += Tanyao(list);

        }

        int point = 0;
        if( yaku == 0 )
        {
            // なにもなし
        }
        else if( yaku < 3 )
        {
            point = yaku * 1000;    
        }
        else if( yaku == 3 )
        {
            point = 3900;
        }
        else if( yaku < 5 )
        {
            point = 8000;
        }
        return point;
    }

    // 頭があるかどうかチェック
    private bool isHead(List<int> list)
    {
        for (int i = 0; i < list.Count - 2; i++)
        {
            if (list[i] == list[i + 1] && list[i + 1] != list[i + 2])
            {
                return true;
            }
        }
        return false;
    }

    // 門前自摸出来るかどうかチェック
    private bool isMenzen(List<int> list)
    {
        for (int i = 0; i < list.Count - 2; i++)
        {
            if (list[i] == list[i + 1] && list[i + 1] == list[i + 2])
            {
                i += 2;
                continue;
            }
            else if (list[i] < 30 && list[i + 1] - list[i] == 1 && list[i + 2] - list[i + 1] == 1)
            {
                i += 2;
                continue;
            }
            else return false;
        }
        return true;
    }

    /*----------------------------------------------
     * 
     * ここからはそれそれの役チェックを行う
     * 役がある場合は約数を返す
     * そうでない場合は0を返す
     * 
     */

    private int Tanyao(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if( i > 30 || i%10 == 1 || i%10 == 9 )
            {
                return 0;
            }
        }
        return 1;
    }

    private int Ipeiko(List<int> list)
    {
        int cnt = 0;
        for( int i = 0; i < list.Count-6; i++)
        {
            if (list[i] == list[i + 1] && list[i + 2] == list[i + 3] && list[i + 4] == list[i + 5])
            {
                if (list[i + 3] - list[i+1] == 1 && list[i + 5] - list[i+3] == 1)
                {
                    cnt++;
                    i += 5;
                }
            }
        }
        return cnt;
    }

    private int Yakuhai(List<int> list)
    {
        int cnt = 0;
        for( int i = 0; i < list.Count-2; i++ )
        {
            if (list[i] > 30)
            {
                if (list[i] == list[i + 1] && list[i + 1] == list[i + 2])
                {
                    cnt++;
                    i += 2;
                }
            }
        }
        return cnt;
    }

    private int Tsitoitsu(List<int> list)
    {
        for( int i = 0; i < list.Count-1; i++ )
        {
            if( list[i] != list[i+1] )
            {
                return 0;
            }
            else
            {
                i++;
            }
        }
        return 2;
    }

    // 数値で表されている牌を文字情報に変換する
    public string Transform(int key)
    {
        /*-------------------------------------------------
         * 萬子      索子      筒子    東西南北 　白発中
         * 1 ~ 9   11 ~ 19   21 ~ 29   31 ~ 34    35 ~ 37
         *
         * 赤ドラは現状無し
         *
         */

        List<string> name = new List<string>() { "萬", "索", "筒" };
        List<string> zihai = new List<string>() { "東", "西", "南", "北", "白", "発", "中" };

        if (key < 30)
        {
            return (key % 10).ToString() + name[key / 10].ToString();
        }
        else
        {
            return zihai[(key - 1) % 10];
        }
    }

    public List<int> GetTotalList()
    {
        return total_list;
    }
}
