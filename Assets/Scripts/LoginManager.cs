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
            notify.text = "아이디가 생성되었습니다.";
        }
        else
        {
            notify.text = "이미 존재하는 아이디입니다.";
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
            notify.text = "아이디와 패스워드가 일치하지 않습니다.";
        }
    }
    bool CheckInput(string id, string pwd)
    {
        if(id == "" || pwd == "")
        {
            notify.text = "아이디와 패스워드를 입력해주세요.";
            return false;
        }
        else return true;   
    }
}
