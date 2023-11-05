using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    // 目標：モンスターのデータ管理
    // ・モンスターの多様化：ScriptableObject
    // Playerの1マス移動
    [SerializeField] float moveSpeed;

    bool isMoving;
    Vector2 input;

    float offsetY = 0.2f;

    Animator animator;
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask longGrassLayer;
    public UnityAction OnEncounted;

    [SerializeField] GameController gameController;
    [SerializeField] GameLayer gameLayer;
    PlayerState playerState;

    Flowchart flowchart;

    public enum PlayerState
    {
        Normal,
        Talk,
        Command,
    }

    private void Awake()
    {        
        animator = GetComponent<Animator>();
    }
    public void Start()
    {
        flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
        // flowchart.SendFungusMessage("MessageTalk");
    }
    public void HandleUpdate()
    {
        if (!isMoving)
        {
            // キーボードの入力方向に動く
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // 斜め移動はしない
            if(input.x != 0)
            {
                input.y = 0;
            }

            // 入力があったら
            if (input != Vector2.zero)
            {
                // 入力があったときに、向きを変えたい
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                Vector2 targetPos = transform.position;
                targetPos += input;
                if (IsWalkabel(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }
        animator.SetBool("isMoving", isMoving);
    }
    // コルーチンを使って徐々に目的地に近づける
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // targetPosに近づける
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed*Time.deltaTime
                );
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        OnMoveOver();
    }

    // targetPosに移動可能かを調べる関数
    bool IsWalkabel(Vector2 targetPos)
    {
        // targetPosに半径0.2fの円のRayを飛ばして、ぶつかったらtrue
        // その否定だから"!"
        bool hit = Physics2D.OverlapCircle(targetPos, 0.2f,solidObjectsLayer);
        return !hit;
    }
    private void OnMoveOver()
    {
        var colliders =Physics2D.OverlapCircleAll(transform.position - new Vector3(0,offsetY), 0.2f, gameLayer.TriggerableLayers);

        foreach(var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerble>();
            if(triggerable != null)
            {
                // animator.SetBool("isMoving", false);
                triggerable.OnPlayerTriggerd(this);
                break;
            }
        }
    }

}
