using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HPBar hpBar;

    Spirit _pokemon;

    public void SetData(Spirit pokemon)
    {
        _pokemon = pokemon;
        nameText.text = pokemon.Base.name;
        levelText.text = "LV" + pokemon.Level;
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHP);
    }
}
