using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) //�}�E�X���N���b�N�A�X�y�[�X�L�[�AA�{�^���A�W�����v�{�^�����������ꍇ
        {
            SceneManager.LoadScene("HomeTown");//some_sensei�V�[�������[�h����
        }
    }
}