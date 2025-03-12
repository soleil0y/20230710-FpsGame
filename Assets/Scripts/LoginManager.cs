using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField id;
    public InputField pw;
    public Text notify;
    void Start()
    {
        notify.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveUserData()
    {
        if(!CheckInput(id.text, pw.text))
        {
            return;
        }

        if (!PlayerPrefs.HasKey(id.text))
        {
            PlayerPrefs.SetString(id.text,pw.text);
            notify.text = "���̵� �����Ǿ����ϴ�.";
        }
        else
        {
            notify.text = "�̹� �����ϴ� ���̵��Դϴ�.";
        }
        PlayerPrefs.SetString(id.text, pw.text);
    }
    public void CheckUserData()
    {
        if (!CheckInput(id.text, pw.text))
        {
            return;
        }

        string pass = PlayerPrefs.GetString(id.text);

        if (pw.text == pass)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            notify.text = "���̵�� �н����尡 ��ġ���� �ʽ��ϴ�.";
        }
    }
    bool CheckInput(string id, string pwd)
    {
        if(id == "" || pwd == "")
        {
            notify.text = "���̵�� �н����带 �Է����ּ���.";
            return false;
        }
        else return true;   
    }
}
