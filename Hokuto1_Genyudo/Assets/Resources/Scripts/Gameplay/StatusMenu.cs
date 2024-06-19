using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusMenu : MonoBehaviour
{
    public UnityAction OnMenuClosed;
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(-0.1f, 0.1f, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("åàíËÉ{É^ÉìÇâüÇµÇ‹ÇµÇΩÅI");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnMenuClosed();
        }
    }
}
