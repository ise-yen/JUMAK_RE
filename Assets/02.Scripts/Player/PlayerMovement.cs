using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player player;
    PlayerSkill player;

    // 조이스틱, 조이버튼
    public Joystick joystick;
    public JoyButtonController joybutton;

    //애니메이션
    Animator playerAnim;
    

    // 이동
    Rigidbody rigid;
    public float runSpeed;
    float hAxis;
    float vAxis;
    Vector3 moveVec;

    // 인터렉션 저장 변수
    [HideInInspector]
    public bool isDown;
    bool isBorder;

    public bool isCut = false;

    private void Start()
    {
        joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
        joybutton = GameObject.Find("Fixed Joybutton").GetComponent<JoyButtonController>();
        //joystick = FindObjectOfType<Joystick>();
        //joybutton = FindObjectOfType<JoyButtonController>();
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        player = GetComponent<PlayerSkill>();
        //player = GetComponent<Player>();
    }

    private void Update()
    {
        GetInput();
        Move();
        Turn();
    }

    void FixedUpdate()
    {
        FreezeRotation();
    }
    // -------< 캐릭터 기본 이동 >-------
    // 조이스틱 입력
    public void GetInput()
    {
        //hAxis = joystick.Horizontal + Input.GetAxis("Horizontal");
        //vAxis = joystick.Vertical + Input.GetAxis("Vertical");
        hAxis = joystick.Horizontal * runSpeed;
        vAxis = joystick.Vertical * runSpeed;
        isDown = joybutton.Pressed;
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; // normalized: 대각선 고려
        transform.position += moveVec * runSpeed * Time.deltaTime;
        playerAnim.SetBool("isWalk", moveVec != Vector3.zero);
        playerAnim.SetBool("isCatch", player.isCatch == true);
        playerAnim.SetBool("isCut", isCut != false);
        if(player.isCatch == true || moveVec != Vector3.zero || player.nearObject == null)
        {
            playerAnim.SetBool("isCut", false);
            player.handKnife.SetActive(false); // 칼 비활성화
        }

    }

    void Turn()
    {
        // 플레이어의 시선방향으로 회전
        transform.LookAt(transform.position + moveVec);

    }

    // 캐릭터 물리 제어
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }



}
