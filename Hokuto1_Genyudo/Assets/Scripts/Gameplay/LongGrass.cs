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
        // �����_���G���J�E���g
        if (Random.Range(0, 1000) < 50)
        {
            // Random.Range(0,100)�F0�`99�܂ł̂ǂꂩ�̐������o��
            // 10��菬����������0�`9�܂ł�10��
            // 10�ȏ�̐�����10�`99�܂ł�90��
            gameController.StartBattle();
        }
    }
}
