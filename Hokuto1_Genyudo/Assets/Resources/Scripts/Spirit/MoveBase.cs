using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MoveBase : ScriptableObject
{
    // �Z�̃}�X�^�[�f�[�^

    // ���O�C�ڍׁC�^�C�v�C�З́C���m���CPP�i�Z���g�����ɏ����|�C���g�j

    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;
    [SerializeField] int power;
    [SerializeField] int accuracy; // ���m��
    [SerializeField] int pp;
    // ������ϐ����擾����ꍇ�́A������ϐ����Q�Ƃ���
    public string Name { get => name;}
    public string Description { get => description;}
    public PokemonType Type { get => type;}
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }



}
