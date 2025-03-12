using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{

    public float rotSpeed = 500f; //�ӵ�

    float mx = 0;
    float my = 0; //ȸ�� ���� ����
    
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y"); //���콺 �Է� ��������
        /*
        Vector3 dir = new Vector3(-mouseY, mouseX, 0); //�Է°��� ���Ͱ�����

        transform.eulerAngles += dir * rotSpeed * Time.deltaTime; //ȸ��

        Vector3 rot = transform.eulerAngles; 
        rot.x = Mathf.Clamp(rot.x, -90f, 90f);
        transform.eulerAngles = rot; */
        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime; //ȸ�� �� ������ �Է� �� ����

        my = Mathf.Clamp(my, -90f, 90f);

        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
