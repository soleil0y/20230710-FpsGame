using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target; //target�� ��ġ����

    void Update()
    {
        transform.position = target.position; //ī�޶��� ��ġ�� target����
    }
}

//�ٵ� �׳� ����ī�޶� ��ġ�� 0, 0.25, 0��,�� �����ؼ� �÷��̾� �ڽ����� �־������ �� �ǳ�?
