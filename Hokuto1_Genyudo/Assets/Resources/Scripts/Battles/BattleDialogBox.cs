using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogBox : MonoBehaviour
{
    // dialog��Text���擾���āA�ύX����
    [SerializeField] int letterPerSecond; // 1����������̎���
    [SerializeField] TextMeshProUGUI dialogText;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<TextMeshProUGUI> actionTexts;
    [SerializeField] List<TextMeshProUGUI> moveTexts;

    [SerializeField] TextMeshProUGUI ppText;
    [SerializeField] TextMeshProUGUI typeText;

    // �^�C�v�`���ŕ�����\������
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = string.Empty;
        foreach (char letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        yield return new WaitForSeconds(0.7f);
    }

    // UI�̕\��/��\��������

    // dialogText�̕\���Ǘ�
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    // actionSelector�̕\���Ǘ�
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    // moveSelector�̕\���Ǘ�
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    // �I�𒆂̃A�N�V�����̐F��ς���
    public void UpdateActionSelection(int selectAction)
    {
        // selectAction��0�̎���actionTexts[0]�̐F��ɂ���B����ȊO����
        // selectAction��1�̎���actionTexts[1]�̐F��ɂ���B����ȊO����

        for (int i = 0; i < actionTexts.Count; i++)
        {
            if(selectAction == i)
            {
                actionTexts[i].color = Color.blue;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }
        }
    } 
    public void UpdateMoveSelection(int selectMove, Move move)
    {
        for (int i = 0; i < moveTexts.Count; i++)
        {
            if(selectMove == i)
            {
                moveTexts[i].color = Color.blue;
                ppText.text = $"PP {move.PP}/{move.Base.PP}";
                typeText.text = move.Base.Type.ToString();
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
        }
    }
    // �ڕW
    // �E�Z���̔��f
    // �E�I�𒆂̋Z�̐F��ς���
    // �EPP��TYPE�̔��f

    public void SetMoveNames(List<Move> moves)
    {
        for(int i=0; i< moveTexts.Count; i++)
        {
            if(i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name;
            }
            else
            {
                moveTexts[i].text = "�|";
            }
        }
    }
}
