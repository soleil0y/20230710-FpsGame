using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{ 
    public float moveSpeed = 7; //ĳ���� �ӵ�
    public int hp = 20;
    int maxHp = 20;
    public Slider hpSlider;

    CharacterController cc; //ĳ���� ��Ʈ�ѷ�

    float gravity = -20f; //�߷�
    float yVelocity = 0; //���� �ӷ�

    public float jumpPower = 10f; //������ 
    public bool isJumping = false; //�������º���

    public GameObject hitEffect;

    Animator anim;

    public void DamageAction(int damage) //�ǰ�ȿ��
    {
        hp -= damage;
        if (hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
    }
    IEnumerator PlayHitEffect() //�ǰ�ȿ�� �ڷ�ƾ
    { 
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitEffect.SetActive(false);
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        hpSlider.value = (float)hp / (float)maxHp;
        //print(hpSlider.value);

        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical"); //Ű���� �Է� �� �޾ƿ���

        Vector3 dir = new Vector3(h, 0, v); 
        dir = dir.normalized; //�̵� ���� ����

        anim.SetFloat("MoveMotion", dir.magnitude);

        dir = Camera.main.transform.TransformDirection(dir);
        
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0; //���� �ӵ� 0���� �ʱ�ȭ

        } // ������ �߰�(true), �ٴڿ� ���� ���>������ �������Ƿ� false
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            isJumping = true;
        } // !����, ��ư�Է��� ���� ���> ����, true 

        yVelocity += gravity * Time.deltaTime;
        dir.y= yVelocity; //���� ���Ͱ� ���

        cc.Move(dir * moveSpeed * Time.deltaTime); //������(CharacterController���)

    }
}
