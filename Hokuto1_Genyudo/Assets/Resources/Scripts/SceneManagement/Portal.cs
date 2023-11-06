using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IPlayerTriggerble
{
    [SerializeField] int sceneToLoad = -1;
    public void OnPlayerTriggerd(PlayerController player)
    {
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
