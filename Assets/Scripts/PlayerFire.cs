using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//변경점들
//시야가 너무 낮은 것 같아서 CamPosition 위치 조정(0.25>0.5)
//카메라 이동 속도 변경
//폭탄 발사 각도 좀 더 위로 조정(slightlyAboveCameraForward, offset 변수 사용)

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition; //발사 위치(유니티상에서 지정-.public)
    public GameObject bombFactory; //폭탄 공장(유니티상에서 폭탄 프리팹 지정)

    public int weaponPower = 5;

    public GameObject bulletEffect;
    ParticleSystem ps;

    public float throwPower = 15f;
    float offset = 0.3f;

    Animator anim; //애니메이터

    bool ZoomMode = false; //스나이퍼 모드

    public Text wModeText; //무기 모드 텍스트

    public GameObject[] eff_Flash; //이펙트 배열, 랜덤1출력

    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject crosshair1;
    public GameObject crosshair2;
    public GameObject weapon1_R;
    public GameObject weapon2_R;
    public GameObject crosshair_Zoom;
    //UI이펙트관련 ㅇㅅㅇ(무기모드별 이미지/크로스헤어 이미지 활성화/비활성화)

    IEnumerator ShootEffectOn(float duration)
    {
        int num = Random.Range(0, eff_Flash.Length - 1); //랜덤 숫자 뽑기
        eff_Flash[num].SetActive(true); //이펙트 출력(활성화)

        yield return new WaitForSeconds(duration); //지정 시간만큼 대기

        eff_Flash[num].SetActive(false); //다시 비활성화
    }

    enum WeaponMode //게임 상태 enum
    {
        Normal,
        Sniper
    } 

    WeaponMode wMode;

    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();

        anim = GetComponentInChildren<Animator>();

        wMode = WeaponMode.Normal;
    }

    void Update()
    {
        if(GameManager.gm.gState != GameManager.GameState.Run) //게임매니저-게임 상태가 게임 중 인 경우에만 조작 가능하게
        {
            return;
        }

        if (Input.GetMouseButtonDown(1)) //마우스 오른쪽 버튼 입력
        {
            switch (wMode)
            {
                case WeaponMode.Normal:

                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    Rigidbody rb = bomb.GetComponent<Rigidbody>(); 
                    //ObjectName.GetComponent<컴포넌트이름>() 으로 가져옴
                    //AddForce메소드 (적용할 힘의 벡터-방향/크기, 힘을 가하는 방식)
                    //transfrom.forward 정면 방향 벡터값
                    //throwPower 힘의 세기 변수(위에서 선언함)
                    //ForceMode.Impulse 힘을 가하는 방식-순간적인 힘
                    //rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse); //책 내용
                    Vector3 slightlyAboveCameraForward = Camera.main.transform.forward + Camera.main.transform.up * offset; //살짝 위로 투척되게 하고싶음ㅇ.ㅇ
                    rb.AddForce(slightlyAboveCameraForward * throwPower, ForceMode.Impulse);

                    break;

                case WeaponMode.Sniper:

                    //print("ㅇㅇㅇㅇ);

                    if (!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;

                        crosshair_Zoom.SetActive(true);
                        crosshair2.SetActive(false);
                    }
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;

                        crosshair_Zoom.SetActive(false);
                        crosshair2.SetActive(true);
                    }

                    break;
            }

        }

        //발사는 총알을 생성해서 처리하기 어려움(수가 많음, 속도가 빠름(중간과정필요x))->레이캐스트 구조체를 사용
        //시작점-진행방향 > 충돌지점 정보
        if (Input.GetMouseButtonDown(0)) //마우스 왼쪽 버튼-발사
        {
            //Debug.Log("click"); 

            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); //레이(유니티3D 직선 구조체: 시작점과 방향으로 구성)
            //새로운 레이 생성(시작점:카메라 위치, 진행방향:카메라 전면)

            RaycastHit hitInfo = new RaycastHit(); //hitInfo에 레이캐스트를 통해 충돌 위치 저장할 객체 생성

            if(Physics.Raycast(ray, out hitInfo)) //부딪힌 물체가 있다면
            {

                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) //부딪힌 물체가 적이라면
                {
                    //print("ㅇㅇㅇㅇㅇ");
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>(); //적의 EnemyFSM스크립트(컨포넌트)를 가져옴
                    eFSM.HitEnemy(weaponPower); //적 스크립트의 HitEnemy메소드 실행
                }
                else
                {

                    bulletEffect.transform.position = hitInfo.point; //피격이펙트 위치 설정
                    bulletEffect.transform.forward = hitInfo.normal; //피격이펙트의 forward를 부딪힌 지점의 법선벡터로 설정

                    ps.Play(); //피격이펙트 재생

                }
               //Debug.Log("Impact Point: " + hitInfo.point);
            }

            StartCoroutine(ShootEffectOn(0.05f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            crosshair_Zoom.SetActive(false);

            wMode = WeaponMode.Normal;
            Camera.main.fieldOfView = 60f;
            wModeText.text = "NORMAL MODE";

            weapon1.SetActive(true);
            weapon2.SetActive(false);
            crosshair1.SetActive(true);
            crosshair2.SetActive(false);
            weapon1_R.SetActive(true);
            weapon2_R.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //print("2번");
            wMode = WeaponMode.Sniper;
            wModeText.text = "SNIPER MODE";

            weapon1.SetActive(false);
            weapon2.SetActive(true);
            crosshair1.SetActive(false);
            crosshair2.SetActive(true);
            weapon1_R.SetActive(false);
            weapon2_R.SetActive(true);
        }

    }
}
