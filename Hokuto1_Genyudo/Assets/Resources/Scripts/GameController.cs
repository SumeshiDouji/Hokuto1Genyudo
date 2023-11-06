using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    FreeRoam,
    Battle,

}

public class GameController : MonoBehaviour
{
    // ƒQ[ƒ€‚Ìó‘Ô‚ğŠÇ—
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    // ‘ŠŒİˆË‘¶‚ğ‰ğÁ:UnityAction(ŠÖ”‚ğ“o˜^‚·‚é)
    GameState state = GameState.FreeRoam;

    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        battleSystem.StartBattle();
    }

    public void EndBattle()
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        battleSystem = GameObject.Find("EssentialObjects").transform.Find("BattleSystem").GetComponent<BattleSystem>();
        worldCamera = GameObject.Find("World Camera").GetComponent<Camera>();

        playerController.OnEncounted += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }
    // Update is called once per frame
    void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if(state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }
}
