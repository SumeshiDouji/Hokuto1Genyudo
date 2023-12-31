using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

// レベルに応じたステータスの違うモンスターを生成するクラス
// 注意：データのみ扱う：純粋C#のクラス
public class Spirit
{
    // ベースとなるデータ
    public SpiritBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }
    // 使える技
    public List<Move> Moves { get; set; }

    // コンストラクター：生成時の初期設定
    public Spirit(SpiritBase sBase, int pLevel)
    {
        Base = sBase;
        this.Level = pLevel;
        HP = MaxHP;

        Moves = new List<Move>();
        // 使える技の設定：覚える技のレベル以上なら、Movesに追加
        foreach(LearnableMove learnableMove in sBase.LearnableMoves)
        {
            if(Level >= learnableMove.Level)
            {
                // 技を覚える
                Moves.Add(new Move(learnableMove.Base1));
            }
            // 4つ以上の技は使えない
            if(Moves.Count >= 4)
            {
                break;
            }
        }
    }

    // Levelに応じたステータスを返すもの：プロパティ（+処理を加えることができる）
    // 関数バージョン
    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5;  }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5;  }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5;  }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5;  }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5;  }
    }
    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 5;  }
    }
    // 修正:３つの情報を渡す
    // ・戦闘不能
    // ・クリティカル
    // ・相性
    public DamageDetails TakeDamage(Move move, Spirit attacker)
    {
        // クリティカル
        float critical = 1f;
        // 6.25%でクリティカル
        if(Random.value * 100 <= 6.25f)
        {
            critical = 2f;
        }
        // 相性
        float type = TypeChart.GetEffectiveness(move.Base.Type,Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, Base.Type2);
        DamageDetails damageDetails = new DamageDetails
        {
            Fainted = false,
            Critical = critical,
            TypeEffectiveness = type
        };
        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);
        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }

        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}