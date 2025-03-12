using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect; //폭발이펙트 프리팹 변수

    public int attackPower = 10;
    public float explRadius = 5f;
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explRadius, 1 << LayerMask.NameToLayer("Enemy"));
        //시프트 연산..에너미레이어 지정
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
            print("dd");
        }

        GameObject eff = Instantiate(bombEffect); //이펙트 프리팹 생성
        eff.transform.position = transform.position; //이펙트위치=폭탄위치
        //eff.transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);

        Destroy(gameObject);
    }
}
