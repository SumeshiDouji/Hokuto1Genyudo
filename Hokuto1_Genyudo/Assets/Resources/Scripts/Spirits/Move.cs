using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    // Pokemonが実際に使うときの技データ

    // 技のマスターデータをもつ
    // 使いやすいようにするためにPPももつ

    // Pokemon.csが参照するのでpublicにしておく
    public MoveBase Base { get; set; }
    public int PP { get; set; }

    // 初期設定
    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }
}
