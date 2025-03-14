using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//痕井繊級
//獣醤亜 格巷 碍精 依 旭焼辞 CamPosition 是帖 繕舛(0.25>0.5)
//朝五虞 戚疑 紗亀 痕井
//賑添 降紫 唖亀 岨 希 是稽 繕舛(slightlyAboveCameraForward, offset 痕呪 紫遂)

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition; //降紫 是帖(政艦銅雌拭辞 走舛-.public)
    public GameObject bombFactory; //賑添 因舌(政艦銅雌拭辞 賑添 覗軒噸 走舛)

    public int weaponPower = 5;

    public GameObject bulletEffect;
    ParticleSystem ps;

    public float throwPower = 15f;
    float offset = 0.3f;

    Animator anim; //蕉艦五戚斗

    bool ZoomMode = false; //什蟹戚遁 乞球

    public Text wModeText; //巷奄 乞球 努什闘

    public GameObject[] eff_Flash; //戚薙闘 壕伸, 沓棋1窒径

    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject crosshair1;
    public GameObject crosshair2;
    public GameObject weapon1_R;
    public GameObject weapon2_R;
    public GameObject crosshair_Zoom;
    //UI戚薙闘淫恵 しさし(巷奄乞球紺 戚耕走/滴稽什伯嬢 戚耕走 醗失鉢/搾醗失鉢)

    IEnumerator ShootEffectOn(float duration)
    {
        int num = Random.Range(0, eff_Flash.Length - 1); //沓棋 収切 嗣奄
        eff_Flash[num].SetActive(true); //戚薙闘 窒径(醗失鉢)

        yield return new WaitForSeconds(duration); //走舛 獣娃幻鏑 企奄

        eff_Flash[num].SetActive(false); //陥獣 搾醗失鉢
    }

    enum WeaponMode //惟績 雌殿 enum
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
        if(GameManager.gm.gState != GameManager.GameState.Run) //惟績古艦煽-惟績 雌殿亜 惟績 掻 昔 井酔拭幻 繕拙 亜管馬惟
        {
            return;
        }

        if (Input.GetMouseButtonDown(1)) //原酔什 神献楕 獄動 脊径
        {
            switch (wMode)
            {
                case WeaponMode.Normal:

                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    Rigidbody rb = bomb.GetComponent<Rigidbody>(); 
                    //ObjectName.GetComponent<陳匂獲闘戚硯>() 生稽 亜閃身
                    //AddForce五社球 (旋遂拝 毘税 困斗-号狽/滴奄, 毘聖 亜馬澗 号縦)
                    //transfrom.forward 舛檎 号狽 困斗葵
                    //throwPower 毘税 室奄 痕呪(是拭辞 識情敗)
                    //ForceMode.Impulse 毘聖 亜馬澗 号縦-授娃旋昔 毘
                    //rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse); //奪 鎧遂
                    Vector3 slightlyAboveCameraForward = Camera.main.transform.forward + Camera.main.transform.up * offset; //詞側 是稽 燈担鞠惟 馬壱粛製し.し
                    rb.AddForce(slightlyAboveCameraForward * throwPower, ForceMode.Impulse);

                    break;

                case WeaponMode.Sniper:

                    //print("しししし);

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

        //降紫澗 恥硝聖 持失背辞 坦軒馬奄 嬢形崇(呪亜 弦製, 紗亀亜 匙硯(掻娃引舛琶推x))->傾戚蝶什闘 姥繕端研 紫遂
        //獣拙繊-遭楳号狽 > 中宜走繊 舛左
        if (Input.GetMouseButtonDown(0)) //原酔什 図楕 獄動-降紫
        {
            //Debug.Log("click"); 

            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); //傾戚(政艦銅3D 送識 姥繕端: 獣拙繊引 号狽生稽 姥失)
            //歯稽錘 傾戚 持失(獣拙繊:朝五虞 是帖, 遭楳号狽:朝五虞 穿檎)

            RaycastHit hitInfo = new RaycastHit(); //hitInfo拭 傾戚蝶什闘研 搭背 中宜 是帖 煽舌拝 梓端 持失

            if(Physics.Raycast(ray, out hitInfo)) //採禦微 弘端亜 赤陥檎
            {

                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) //採禦微 弘端亜 旋戚虞檎
                {
                    //print("ししししし");
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>(); //旋税 EnemyFSM什滴験闘(珍匂獲闘)研 亜閃身
                    eFSM.HitEnemy(weaponPower); //旋 什滴験闘税 HitEnemy五社球 叔楳
                }
                else
                {

                    bulletEffect.transform.position = hitInfo.point; //杷維戚薙闘 是帖 竺舛
                    bulletEffect.transform.forward = hitInfo.normal; //杷維戚薙闘税 forward研 採禦微 走繊税 狛識困斗稽 竺舛

                    ps.Play(); //杷維戚薙闘 仙持

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
            //print("2腰");
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
