using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] SpiritBase _base; // 戦わせるモンスターをセットする
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Spirit pokemon { get; set; }
    Image image;
    Color originalColor;
    Vector3 originalPos;

    // バトルで使うモンスターを保持
    // モンスターの画像を反映する

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = transform.localPosition;
        originalColor = image.color;
    }

    public void Setup()
    {
        // _baseからレベルに応じたモンスターを生成する
        // BattleSystemで使うからプロパティに入れる
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

    // 登場Anim
    public void PlayerEnterAnimation()
    {
        if (isPlayerUnit)
        {
            // 左端に配置
            transform.localPosition = new Vector3(-850, originalPos.y);
        }
        else
        {
            // 右端に配置
            transform.localPosition = new Vector3(850, originalPos.y);
        }
        // 戦闘時の位置までアニメーション
        transform.DOLocalMoveX(originalPos.x, 1f);
    }
    // 攻撃Anim
    public void PlayerAttackAnimation()
    {
        // シーケンス
        // 右に動いた後、元の位置に戻る
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
    // ダメージAnim
    public void PlayerHitAnimation()
    {
        // 色を一度GLAYにしてから戻す
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    // 戦闘不能Anim
    public void PlayerFaintAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0, 0.5f));
    }
}
