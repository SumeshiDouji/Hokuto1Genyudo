using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Pokemon�����ۂɎg���Ƃ��̋Z�f�[�^

    // �Z�̃}�X�^�[�f�[�^������
    // �g���₷���悤�ɂ��邽�߂�PP������

    // Pokemon.cs���Q�Ƃ���̂�public�ɂ��Ă���
    public MoveBase Base { get; set; }
    public int PP { get; set; }

    // �����ݒ�
    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }
}