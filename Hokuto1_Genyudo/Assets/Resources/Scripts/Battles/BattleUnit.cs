using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] SpiritBase _base; // ��킹�郂���X�^�[���Z�b�g����
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Spirit pokemon { get; set; }
    Image image;
    Color originalColor;
    Vector3 originalPos;

    // �o�g���Ŏg�������X�^�[��ێ�
    // �����X�^�[�̉摜�𔽉f����

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = transform.localPosition;
        originalColor = image.color;
    }

    public void Setup()
    {
        // _base���烌�x���ɉ����������X�^�[�𐶐�����
        // BattleSystem�Ŏg������v���p�e�B�ɓ����
        pokemon = new Spirit(_base, level);

        Image image = GetComponent<Image>();
        if (isPlayerUnit)
        {
            image.sprite = pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = pokemon.Base.FrontSprite;
        }
        PlayerEnterAnimation();
    }

    // �o��Anim
    public void PlayerEnterAnimation()
    {
        if (isPlayerUnit)
        {
            // ���[�ɔz�u
            transform.localPosition = new Vector3(-850, originalPos.y);
        }
        else
        {
            // �E�[�ɔz�u
            transform.localPosition = new Vector3(850, originalPos.y);
        }
        // �퓬���̈ʒu�܂ŃA�j���[�V����
        transform.DOLocalMoveX(originalPos.x, 1f);
    }
    // �U��Anim
    public void PlayerAttackAnimation()
    {
        // �V�[�P���X
        // �E�ɓ�������A���̈ʒu�ɖ߂�
        Sequence sequence = DOTween.Sequence();
        
        if (isPlayerUnit)
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }
        sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.2f));
    }
    // �_���[�WAnim
    public void PlayerHitAnimation()
    {
        // �F����xGLAY�ɂ��Ă���߂�
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    // �퓬�s�\Anim
    public void PlayerFaintAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0, 0.5f));
    }
}
