using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target; //target의 위치정보

    void Update()
    {
        transform.position = target.position; //카메라의 위치를 target으로
    }
}

//근데 그냥 메인카메라 위치를 0, 0.25, 0ㅇ,로 고정해서 플레이어 자식으로 넣어버리면 안 되나?
