using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

// ���x���ɉ������X�e�[�^�X�̈Ⴄ�����X�^�[�𐶐�����N���X
// ���ӁF�f�[�^�݈̂����F����C#�̃N���X
public class Pokemon : MonoBehaviour
{
    // �x�[�X�ƂȂ�f�[�^
    public PokemonBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }
    // �g����Z
    public List<Move> Moves { get; set; }

    // �R���X�g���N�^�[�F�������̏����ݒ�
    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        this.Level = pLevel;
        HP = MaxHP;

        Moves = new List<Move>();
        // �g����Z�̐ݒ�F�o����Z�̃��x���ȏ�Ȃ�AMoves�ɒǉ�
        foreach(LearnableMove learnableMove in pBase.LearnableMoves)
        {
            if(Level >= learnableMove.Level)
            {
                // �Z���o����
                Moves.Add(new Move(learnableMove.Base1));
            }
            // 4����̋Z�͎g���Ȃ�
            if(Moves.Count >= 4)
            {
                break;
            }
        }
    }

    // Level�ɉ������X�e�[�^�X��Ԃ����́F�v���p�e�B�i+�����������邱�Ƃ��ł���j
    // �֐��o�[�W����
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
    // �C��:�R�̏���n��
    // �E�퓬�s�\
    // �E�N���e�B�J��
    // �E����
    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        // �N���e�B�J��
        float critical = 1f;
        // 6.25%�ŃN���e�B�J��
        if(Random.value * 100 <= 6.25f)
        {
            critical = 2f;
        }
        // ����
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