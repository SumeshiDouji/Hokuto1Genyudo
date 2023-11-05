using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongGrass : MonoBehaviour, IPlayerTriggerble
{
    GameController gameController;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    public void OnPlayerTriggerd(PlayerController player)
    {
        // ランダムエンカウント
        if (Random.Range(0, 1000) < 50)
        {
            // Random.Range(0,100)：0～99までのどれかの数字が出る
            // 10より小さい数字は0～9までの10個
            // 10以上の数字は10～99までの90個
            gameController.StartBattle();
        }
    }
}
