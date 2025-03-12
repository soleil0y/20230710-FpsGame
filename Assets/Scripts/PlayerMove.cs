using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{ 
    public float moveSpeed = 7; //캐릭터 속도
    public int hp = 20;
    int maxHp = 20;
    public Slider hpSlider;

    CharacterController cc; //캐릭터 컨트롤러

    float gravity = -20f; //중력
    float yVelocity = 0; //수직 속력

    public float jumpPower = 10f; //점프력 
    public bool isJumping = false; //점프상태변수

    public GameObject hitEffect;

    Animator anim;

    public void DamageAction(int damage) //피격효과
    {
        hp -= damage;
        if (hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
    }
    IEnumerator PlayHitEffect() //피격효과 코루틴
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
        float v = Input.GetAxis("Vertical"); //키보드 입력 값 받아오기

        Vector3 dir = new Vector3(h, 0, v); 
        dir = dir.normalized; //이동 방향 설정

        anim.SetFloat("MoveMotion", dir.magnitude);

        dir = Camera.main.transform.TransformDirection(dir);
        
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0; //수직 속도 0으로 초기화

        } // 점프를 했고(true), 바닥에 닿은 경우>점프가 끝났으므로 false
        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            isJumping = true;
        } // !점프, 버튼입력이 들어온 경우> 점프, true 

        yVelocity += gravity * Time.deltaTime;
        dir.y= yVelocity; //대충 벡터값 계산

        cc.Move(dir * moveSpeed * Time.deltaTime); //움직임(CharacterController사용)

    }
}
