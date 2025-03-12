using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��������
//�þ߰� �ʹ� ���� �� ���Ƽ� CamPosition ��ġ ����(0.25>0.5)
//ī�޶� �̵� �ӵ� ����
//��ź �߻� ���� �� �� ���� ����(slightlyAboveCameraForward, offset ���� ���)

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition; //�߻� ��ġ(����Ƽ�󿡼� ����-.public)
    public GameObject bombFactory; //��ź ����(����Ƽ�󿡼� ��ź ������ ����)

    public int weaponPower = 5;

    public GameObject bulletEffect;
    ParticleSystem ps;

    public float throwPower = 15f;
    float offset = 0.3f;

    Animator anim; //�ִϸ�����

    bool ZoomMode = false; //�������� ���

    public Text wModeText; //���� ��� �ؽ�Ʈ

    public GameObject[] eff_Flash; //����Ʈ �迭, ����1���

    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject crosshair1;
    public GameObject crosshair2;
    public GameObject weapon1_R;
    public GameObject weapon2_R;
    public GameObject crosshair_Zoom;
    //UI����Ʈ���� ������(�����庰 �̹���/ũ�ν���� �̹��� Ȱ��ȭ/��Ȱ��ȭ)

    IEnumerator ShootEffectOn(float duration)
    {
        int num = Random.Range(0, eff_Flash.Length - 1); //���� ���� �̱�
        eff_Flash[num].SetActive(true); //����Ʈ ���(Ȱ��ȭ)

        yield return new WaitForSeconds(duration); //���� �ð���ŭ ���

        eff_Flash[num].SetActive(false); //�ٽ� ��Ȱ��ȭ
    }

    enum WeaponMode //���� ���� enum
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
        if(GameManager.gm.gState != GameManager.GameState.Run) //���ӸŴ���-���� ���°� ���� �� �� ��쿡�� ���� �����ϰ�
        {
            return;
        }

        if (Input.GetMouseButtonDown(1)) //���콺 ������ ��ư �Է�
        {
            switch (wMode)
            {
                case WeaponMode.Normal:

                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    Rigidbody rb = bomb.GetComponent<Rigidbody>(); 
                    //ObjectName.GetComponent<������Ʈ�̸�>() ���� ������
                    //AddForce�޼ҵ� (������ ���� ����-����/ũ��, ���� ���ϴ� ���)
                    //transfrom.forward ���� ���� ���Ͱ�
                    //throwPower ���� ���� ����(������ ������)
                    //ForceMode.Impulse ���� ���ϴ� ���-�������� ��
                    //rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse); //å ����
                    Vector3 slightlyAboveCameraForward = Camera.main.transform.forward + Camera.main.transform.up * offset; //��¦ ���� ��ô�ǰ� �ϰ������.��
                    rb.AddForce(slightlyAboveCameraForward * throwPower, ForceMode.Impulse);

                    break;

                case WeaponMode.Sniper:

                    //print("��������);

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

        //�߻�� �Ѿ��� �����ؼ� ó���ϱ� �����(���� ����, �ӵ��� ����(�߰������ʿ�x))->����ĳ��Ʈ ����ü�� ���
        //������-������� > �浹���� ����
        if (Input.GetMouseButtonDown(0)) //���콺 ���� ��ư-�߻�
        {
            //Debug.Log("click"); 

            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); //����(����Ƽ3D ���� ����ü: �������� �������� ����)
            //���ο� ���� ����(������:ī�޶� ��ġ, �������:ī�޶� ����)

            RaycastHit hitInfo = new RaycastHit(); //hitInfo�� ����ĳ��Ʈ�� ���� �浹 ��ġ ������ ��ü ����

            if(Physics.Raycast(ray, out hitInfo)) //�ε��� ��ü�� �ִٸ�
            {

                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) //�ε��� ��ü�� ���̶��
                {
                    //print("����������");
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>(); //���� EnemyFSM��ũ��Ʈ(������Ʈ)�� ������
                    eFSM.HitEnemy(weaponPower); //�� ��ũ��Ʈ�� HitEnemy�޼ҵ� ����
                }
                else
                {

                    bulletEffect.transform.position = hitInfo.point; //�ǰ�����Ʈ ��ġ ����
                    bulletEffect.transform.forward = hitInfo.normal; //�ǰ�����Ʈ�� forward�� �ε��� ������ �������ͷ� ����

                    ps.Play(); //�ǰ�����Ʈ ���

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
            //print("2��");
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
