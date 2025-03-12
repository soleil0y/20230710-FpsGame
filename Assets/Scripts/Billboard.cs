using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target; //타겟과 방향 일치시키는 스크립트..타겟은 메인카메라일듯
    void Update()
    {
        transform.forward = target.forward;
    }
}
