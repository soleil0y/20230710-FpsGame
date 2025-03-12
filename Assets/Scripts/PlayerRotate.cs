using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float rotSpeed = 500f; //¼Óµµ

    float mx = 0;

    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X");
        mx += mouseX * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
