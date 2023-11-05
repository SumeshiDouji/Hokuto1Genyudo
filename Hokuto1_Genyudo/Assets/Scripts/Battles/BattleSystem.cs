using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BattleState
{
    Start,
    PlayerAction, // �s���I��
    PlayerMove, // �Z�I��
    EnemyMove,
    Busy,
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    // [SerializeField] GameController gameController;
    public UnityAction OnBattleOver;

    BattleState state;
    int currentAction; // 0:Fight�C1:Run
    int currentMove; // 0:����C1:�E��C2:�����C3:�E���@�̋Z

    /// <summary>
    /// �E���b�Z�[�W���o�āA1�b���ActionSelector��\������
    /// �EZ�{�^���������ƁAMoveSelector��MoveDetails��\������
    /// </summary>
    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
        StartCoroutine(dialogBox.TypeDialog("�A�N�V������I�����Ă�������"));
    }
    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableDialogText(false);
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(true);
    }
    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if(state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }
    // PlayerAction�ł̍s��
    void HandleActionSelection()
    {
        // ������͂����Run�C�����͂����Fight�ɂȂ�
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
            {
                currentAction++;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
            {
                currentAction--;
            }
        }

        // �F�����Ăǂ����I�����Ă邩�킩��悤�ɂ���
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                PlayerMove();
            }
        }
    }
    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerUnit.pokemon.Moves.Count - 1)
            {
                currentMove++;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
            {
                currentMove--;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.pokemon.Moves.Count - 2)
            {
                currentMove += 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
            {
                currentMove -= 2;
            }
        }
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // �Z����
            // �E�Z�I����UI�͔�\��
            dialogBox.EnableMoveSelector(false);
            // �E���b�Z�[�W����
            dialogBox.EnableDialogText(true);

            StartCoroutine(PerformPlayerMove());
        }
    }

    // PlayerMove�̎��s
    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        // �Z������
        Move move = playerUnit.pokemon.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.pokemon.Base.Name} ��{move.Base.Name}��������");
        playerUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(0.7f);
        enemyUnit.PlayerHitAnimation();

        // Enemy�_���[�W�v�Z
        DamageDetails damageDetails = enemyUnit.pokemon.TakeDamage(move, playerUnit.pokemon);
        // HP���f
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        // �퓬�s�\�Ȃ烁�b�Z�[�W
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.pokemon.Base.Name} �͍ċN�s�\");
            enemyUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(0.7f);
            // gameController.EndBattle();
            OnBattleOver();
        }
        else
        {
            StartCoroutine(EnemyMove());// ����ȊO�Ȃ�EnemyMove
        }
    }
    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        // �Z������:�����_��
        Move move = enemyUnit.pokemon.GetRandomMove();
        yield return dialogBox.TypeDialog($"�쐶��{enemyUnit.pokemon.Base.Name} ��{move.Base.Name}��������");
        enemyUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(0.7f);
        playerUnit.PlayerHitAnimation();

        // Player�_���[�W�v�Z
        DamageDetails damageDetails = playerUnit.pokemon.TakeDamage(move, enemyUnit.pokemon);
        // HP���f
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        // ����/�N���e�B�J���̃��b�Z�[�W

        // �퓬�s�\�Ȃ烁�b�Z�[�W
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.pokemon.Base.Name} �͍ċN�s�\");
            playerUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(0.7f);
            // gameController.EndBattle();
            OnBattleOver();
        }
        else
        {
            PlayerAction();// ����ȊO�Ȃ�EnemyMove
        }
    }
    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog($"�}���ɓ�������");
        }
        if(damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog($"���ʂ͂΂���");
        }
        if(damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog($"���ʂ͂��܂ЂƂ�");
        }
    }

    /// <summary>
    /// �EActionSelector�łǂ����I�����Ă��邩���킩��₷������
    /// </summary>
    /// <returns></returns>

    IEnumerator SetupBattle()
    {
        state = BattleState.Start;
        // �����X�^�[�̐����ƕ`��
        playerUnit.Setup();
        enemyUnit.Setup();
        // HUD�̕`��
        playerHud.SetData(playerUnit.pokemon);
        enemyHud.SetData(enemyUnit.pokemon);
        dialogBox.SetMoveNames(playerUnit.pokemon.Moves);
        yield return dialogBox.TypeDialog($"�쐶�� {enemyUnit.pokemon.Base.Name} �������ꂽ");
        PlayerAction();
    }
    
}
