using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{

    public float rotSpeed = 500f; //속도

    float mx = 0;
    float my = 0; //회전 각도 제한
    
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y"); //마우스 입력 가져오기
        /*
        Vector3 dir = new Vector3(-mouseY, mouseX, 0); //입력값을 벡터값으로

        transform.eulerAngles += dir * rotSpeed * Time.deltaTime; //회전

        Vector3 rot = transform.eulerAngles; 
        rot.x = Mathf.Clamp(rot.x, -90f, 90f);
        transform.eulerAngles = rot; */
        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime; //회전 값 변수에 입력 값 누적

        my = Mathf.Clamp(my, -90f, 90f);

        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
