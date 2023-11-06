using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    // �ڕW�F�����X�^�[�̃f�[�^�Ǘ�
    // �E�����X�^�[�̑��l���FScriptableObject
    // Player��1�}�X�ړ�
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
            // �L�[�{�[�h�̓��͕����ɓ���
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // �΂߈ړ��͂��Ȃ�
            if(input.x != 0)
            {
                input.y = 0;
            }

            // ���͂���������
            if (input != Vector2.zero)
            {
                // ���͂��������Ƃ��ɁA������ς�����
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
    // �R���[�`�����g���ď��X�ɖړI�n�ɋ߂Â���
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // targetPos�ɋ߂Â���
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

    // targetPos�Ɉړ��\���𒲂ׂ�֐�
    bool IsWalkabel(Vector2 targetPos)
    {
        // targetPos�ɔ��a0.2f�̉~��Ray���΂��āA�Ԃ�������true
        // ���̔ے肾����"!"
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
