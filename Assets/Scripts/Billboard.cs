using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target; //Ÿ�ٰ� ���� ��ġ��Ű�� ��ũ��Ʈ..Ÿ���� ����ī�޶��ϵ�
    void Update()
    {
        transform.forward = target.forward;
    }
}
