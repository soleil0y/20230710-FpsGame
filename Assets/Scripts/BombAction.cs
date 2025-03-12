using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect; //��������Ʈ ������ ����

    public int attackPower = 10;
    public float explRadius = 5f;
    private void OnCollisionEnter(Collision collision)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explRadius, 1 << LayerMask.NameToLayer("Enemy"));
        //����Ʈ ����..���ʹ̷��̾� ����
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
            print("dd");
        }

        GameObject eff = Instantiate(bombEffect); //����Ʈ ������ ����
        eff.transform.position = transform.position; //����Ʈ��ġ=��ź��ġ
        //eff.transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);

        Destroy(gameObject);
    }
}
