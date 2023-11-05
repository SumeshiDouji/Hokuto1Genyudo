using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �|�P�����̃}�X�^�[�f�[�^�F�O������ύX���Ȃ��i�C���X�y�N�^�[�����ύX�\�j
[CreateAssetMenu]
public class PokemonBase : ScriptableObject
{
    // ���O�A�����A�摜�A�^�C�v�A�X�e�[�^�X
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;

    // �摜
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    // �^�C�v
    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    // �X�e�[�^�X�Fhp,at,df,sAT,sDF,sp
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    // �o����Z�ꗗ
    [SerializeField] List<LearnableMove> learnableMoves;

    // ���t�@�C������attack�̒l�̎擾�͂ł��邪�ύX�͂ł��Ȃ�
    public int Attack { get => attack; }
    public int Defense { get => defense; }
    public int SpAttack { get => spAttack; }
    public int SpDefense { get => spDefense; }
    public int Speed { get => speed; }
    public int MaxHP { get => maxHP; }
    public List<LearnableMove> LearnableMoves { get => learnableMoves; }
    public string Name { get => name; }
    public string Description { get => description; }
    public Sprite FrontSprite { get => frontSprite; }
    public Sprite BackSprite { get => backSprite; }
    public PokemonType Type1 { get => type1; }
    public PokemonType Type2 { get => type2; }
}

// �o����Z�F�ǂ̃��x���ŉ����o���邩
[Serializable]
public class LearnableMove
{
    // �q�G�����L�[�Őݒ肷��
    [SerializeField] MoveBase _base;
    [SerializeField] int level;
    
    public MoveBase Base1 { get => _base; }
    public int Level { get => level; }
}

// �X�e�[�^�X
public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Evil,
    Steel
}


// ��肽������
// �E�킴�̑����v�Z
// �E�N���e�B�J���q�b�g
// �E��̂��Ƃ����b�Z�[�W�ɕ\��
public class TypeChart
{
    static float[][] chart =
    {
    //�U��\�h��̃^�C�v��   �m�[�}���ق̂��݂� �ł� ���� �����肩���Ƃ��ǂ����߂�Ђ����G�X�p�[�ނ�����S�[�X�g�h���S�������͂���
    /*�m�[�}��*/  new float[]{1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.0f, 1.0f, 1.0f, 0.5f},
    /*�ق̂�*/    new float[]{1.0f, 0.5f, 0.5f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f, 2.0f},
    /*�݂�*/      new float[]{1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f},
    /*�ł�*/    new float[]{1.0f, 1.0f, 2.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 0.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f},
    /*����*/      new float[]{1.0f, 0.5f, 2.0f, 1.0f, 0.5f, 1.0f, 1.0f, 0.5f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f},
    /*������*/    new float[]{1.0f, 0.5f, 0.5f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f},
    /*�����Ƃ�*/  new float[]{2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 1.0f, 0.5f, 0.5f, 0.5f, 2.0f, 0.0f, 1.0f, 2.0f, 2.0f},
    /*�ǂ�*/      new float[]{1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 0.0f},
    /*���߂�*/    new float[]{1.0f, 2.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 0.0f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f},
    /*�Ђ���*/    new float[]{1.0f, 1.0f, 1.0f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 1.0f, 1.0f, 0.5f},
    /*�G�X�p�[*/  new float[]{1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.5f},
    /*�ނ�*/      new float[]{1.0f, 0.5f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 1.0f, 0.5f, 1.0f, 2.0f, 0.5f},
    /*����*/      new float[]{1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 2.0f, 0.5f, 1.0f, 0.5f, 2.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f},
    /*�S�[�X�g*/  new float[]{0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.5f},
    /*�h���S��*/  new float[]{1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f},
    /*����*/      new float[]{1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 2.0f, 1.0f, 0.5f, 0.5f},
    /*�͂���*/    new float[]{1.0f, 0.5f, 0.5f, 0.5f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 2.0f, 1.0f, 1.0f, 1.0f, 0.5f},
    };
    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if(attackType == PokemonType.None || defenseType == PokemonType.None)
        {
            return 1f;
        }
        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;
        return chart[row][col];
    }
}


