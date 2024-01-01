using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BattleState
{
    Start,
    PlayerAction, // 行動選択
    PlayerMove, // 技選択
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
    int currentAction; // 0:Fight，1:Run
    int currentMove; // 0:左上，1:右上，2:左下，3:右下　の技

    /// <summary>
    /// ・メッセージが出て、1秒後にActionSelectorを表示する
    /// ・Zボタンを押すと、MoveSelectorとMoveDetailsを表示する
    /// </summary>
    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
        StartCoroutine(dialogBox.TypeDialog("アクションを選択してください"));
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
    // PlayerActionでの行動
    void HandleActionSelection()
    {
        // 下を入力するとRun，上を入力するとFightになる
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

        // 色をつけてどちらを選択してるかわかるようにする
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
            // 技決定
            // ・技選択のUIは非表示
            dialogBox.EnableMoveSelector(false);
            // ・メッセージ復活
            dialogBox.EnableDialogText(true);

            StartCoroutine(PerformPlayerMove());
        }
    }

    // PlayerMoveの実行
    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        // 技を決定
        Move move = playerUnit.pokemon.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.pokemon.Base.Name} は{move.Base.Name}をつかった");
        playerUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(0.7f);
        enemyUnit.PlayerHitAnimation();

        // Enemyダメージ計算
        DamageDetails damageDetails = enemyUnit.pokemon.TakeDamage(move, playerUnit.pokemon);
        // HP反映
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        // 戦闘不能ならメッセージ
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.pokemon.Base.Name} は再起不能");
            enemyUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(0.7f);
            // gameController.EndBattle();
            OnBattleOver();
        }
        else
        {
            StartCoroutine(EnemyMove());// それ以外ならEnemyMove
        }
    }
    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        // 技を決定:ランダム
        Move move = enemyUnit.pokemon.GetRandomMove();
        yield return dialogBox.TypeDialog($"野生の{enemyUnit.pokemon.Base.Name} は{move.Base.Name}をつかった");
        enemyUnit.PlayerAttackAnimation();
        yield return new WaitForSeconds(0.7f);
        playerUnit.PlayerHitAnimation();

        // Playerダメージ計算
        DamageDetails damageDetails = playerUnit.pokemon.TakeDamage(move, enemyUnit.pokemon);
        // HP反映
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        // 相性/クリティカルのメッセージ

        // 戦闘不能ならメッセージ
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.pokemon.Base.Name} は再起不能");
            playerUnit.PlayerFaintAnimation();
            yield return new WaitForSeconds(0.7f);
            // gameController.EndBattle();
            OnBattleOver();
        }
        else
        {
            PlayerAction();// それ以外ならEnemyMove
        }
    }
    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if(damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog($"急所に当たった");
        }
        if(damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog($"効果はばつくんだ");
        }
        if(damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog($"効果はいまひとつ");
        }
    }

    /// <summary>
    /// ・ActionSelectorでどちらを選択しているかをわかりやすくする
    /// </summary>
    /// <returns></returns>

    IEnumerator SetupBattle()
    {
        state = BattleState.Start;
        // モンスターの生成と描画
        playerUnit.Setup();
        enemyUnit.Setup();
        // HUDの描画
        playerHud.SetData(playerUnit.pokemon);
        enemyHud.SetData(enemyUnit.pokemon);
        dialogBox.SetMoveNames(playerUnit.pokemon.Moves);
        yield return dialogBox.TypeDialog($"野生の {enemyUnit.pokemon.Base.Name} があらわれた");
        PlayerAction();
    }
    
}
