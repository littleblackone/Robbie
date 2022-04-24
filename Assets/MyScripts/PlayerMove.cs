using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D rb;
    BoxCollider2D box;

    [Header("�ƶ�����")]
    public float speed = 5;//�ƶ��ٶ�
    public float JumpForce = 5;//��Ծ����
    public float JumpHoldForce = 6;//����������
    public float HangingJumpForce = 10;//������Ծ����
    public float CrouchSpeedDivisor = 3;//����ʱ�ٶȼ������ٶȵĳ���
    public float JumpDuration = 0.1f;//����������ʱ��                                    
    public float CrouchJumpForce = 2;//������Ծ��������                      
      
    [Header("״̬�ж�")]
    public bool isCrouch = false;//�Ƿ����
    public bool isStand = false;//�Ƿ�վ��
    public bool isTouchLayer = false;//�Ƿ�Ӵ�����
    public bool isJumping = false;//�Ƿ�������Ծ
    public bool isheadBlock = false;//ͷ���Ƿ񱻵�ס
    public bool isHanging = false;//�Ƿ�����
    public bool isRelieve = false;//�Ƿ������ҡ�--------- ����������֤������bool�ж�---------------

    //����
    public float xVelocity;//�������Լ��̵�ˮƽ���ķ���  
    float Jumptime;//��ʵʱ���������ʱ��

    [Header("ͼ������")]
    public LayerMask Ground;//Platforms��ͼ������

    [Header("�����ж�")]
    public float footOffset = 0.45f;//˫�ŵ�λ��
    public float headBlock = 0.2f;//ͷ��������ײ�ľ���
    public float footBlock = 0.2f;//˫��������ײ�ľ���

    [Header("���Ҳ���")]
    public float eyesHight = 1.55f;//�۾��ĸ߶�
    public float grabDistance = 0.4f;//���ҵľ���
    public float reachOffset = 0.7f;//�ж�������û��ǽ�ڵ����߾��ɫ�ľ���
    public float playHeight = 0.05f;//��ɫ�߶�
    float boxHeight;

    //��ײ����С��λ��
    Vector2 Standoffset;
    Vector2 Standsize;
    Vector2 Crouchoffset;
    Vector2 Crouchsize;

    //��������
    bool isJumpHold = false;
    bool isJumpPress = false;
    bool CrouchHold = false;//��ʱ����԰�bool��public������Ϸ�п������仯�������ҵ��������ڡ�
    bool CrouchPress = false;

    void Awake()                
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        boxHeight = box.size.y;
        Standoffset = box.offset;
        Standsize = box.size;
        Crouchsize = new Vector2(Standsize.x, Standsize.y / 2);
        Crouchoffset = new Vector2(Standoffset.x, Standoffset.y / 2);
    }
    //�ű���һЩ��Ա��������ڴ���֮��Ĵ���������ʹ�ã������д��Awake()����. 
    //����Scenceʱ���ȶ����нű���Awake()��ִ��. ��ִ��Start()
    //�������ű�A�ڳ�ʼ��ʱ��Ҫ���õ��ű�B��ı���.
    //��A�ĵ������ Ӧ����Start()��,������Awake()
    //��B�ű�Ҫ�����õı���Ӧ��Awake()��ִ�г�ʼ��.


    void Update()//Update��д��Ҫʵʱ��Ⱦ�Ĵ�������л�ȡ�����Ĵ��룬��Button��Key��صĴ���
    {     
        xVelocity = Input.GetAxis("Horizontal");
        if (!isHanging)
        {
          PlayerFacing();
        }      
        if (Input.GetButtonDown("Jump"))//����ֱ��дJumpPress=Input.GetButtonDown("Jump")��JumpHold=Input.GetButton("Jump"),������Ծ������
        {
            isJumpPress = true;
        }
        if (Input.GetButton("Jump"))
        {
            isJumpHold = true;
        }
        CrouchHold = Input.GetButton("Crouch");//����Ķ��²������������Ծһ��д��ԭ��δ֪
        if (Input.GetButtonDown("Crouch"))
        {
            CrouchPress = true;
        }
        if (Input.GetButtonUp("Crouch"))//���������⣬û�뵽�Ӹ�GetButtonUp�����ˣ������͸�CrouchHold��true��false��һ�����ˣ��ܵ���˵��ͦˬ�ģ����гɾ͸С�
        {
            CrouchPress = false;
        }
    }
    private void FixedUpdate()//FixedUpdate��д�������ص���Ҫ����������ֵĴ��룬���ƶ�������ʵ�飬��Ծ��صĴ������FixedUpdate����Ծ�Ż�������
    {
        RayCheck();
        GroundMovement();
        AccumulatorJump();
        HangingRelieve();
    }
    void GroundMovement()//ˮƽ�ƶ�
    {    
        if (CrouchHold&&!isCrouch&&isTouchLayer)
        {
            Crouch();
        }
        else if (!CrouchHold&&isCrouch&&!isheadBlock)
        {
            Stand();
        }
        else if (!isTouchLayer&&isCrouch)
        {
            Stand();
        }     
        if (isCrouch)
        {
            xVelocity /= CrouchSpeedDivisor;
        }
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);               
    }
    void PlayerFacing()//��ҳ���
    {
        if (xVelocity < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (xVelocity > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }
    void Crouch()//����
    {
        isCrouch = true;
        box.offset = Crouchoffset;
        box.size = Crouchsize;
        isStand = false;
    }
    void Stand()//վ��
    {
        isStand = true;
        box.offset = Standoffset;
        box.size = Standsize;
        isCrouch = false;
    }     
   void AccumulatorJump()//��Ծ�����ܽ᣺��Ծ����д��FixedUpdate���Update��ð�����������if��bool�ж��Ƿ����˰������ٽ�ֵ���͵�FixedUpdate�������Ծ������������֡��Ӱ��
   {
        if (isTouchLayer && isJumpPress && !isheadBlock)//��ֹͷ��ǽ��ʱ�������
        {
            if (isCrouch)
            {
                Stand();                 
                rb.AddForce(Vector2.up * CrouchJumpForce, ForceMode2D.Impulse);
            }

            isJumping = true;
            isJumpPress = false;
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            Jumptime = Time.time + JumpDuration;
            
        }
        if (isJumping&&!isHanging)
        {
            if (isJumpHold)
            {
                isJumpHold = false;
                rb.AddForce(Vector2.up * JumpHoldForce, ForceMode2D.Impulse);           
            }       
            if (Jumptime < Time.time)
            {
                isJumping = false;
            }
        }
    }
    RaycastHit2D raycast(Vector2 offset,Vector2 rayDiraction,float Distance,LayerMask layer)//���ߺ���ģ��
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, Distance, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDiraction, color);
        return hit;
    }
    void RayCheck()//���������ж���������������������Ĳ���������ײ�������ų�����
    {  
        float dirction = transform.localScale.x;
        Vector2 grabDir = new Vector2(dirction, 0);
        RaycastHit2D headCheck = raycast(new Vector2(footOffset * dirction, playHeight+boxHeight), grabDir, grabDistance, Ground);//ͷ������ �ж������Ƿ���ǽ
        RaycastHit2D wallCheck = raycast(new Vector2(footOffset * dirction, eyesHight), grabDir, grabDistance, Ground);//�۾����� �ж�ǰ���Ƿ���ǽ
        RaycastHit2D reachCheck = raycast(new Vector2(reachOffset * dirction, playHeight+boxHeight), Vector2.down, grabDistance, Ground);//��ͷ�����»�һ�������ж��·��Ƿ���ǽ
        if (!isRelieve&& !isTouchLayer && rb.velocity.y <= 0 && !headCheck && wallCheck && reachCheck)
        {
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance-0.08f) * dirction;
            pos.y = pos.y - reachCheck.distance ;
            transform.position = pos;
            isHanging = true;
            isRelieve = true;
            rb.bodyType = RigidbodyType2D.Static;
        }
        //���ҽ�����
        RaycastHit2D leftfoot = raycast(new Vector2(-footOffset,0),Vector2.down,footBlock,Ground);
        RaycastHit2D rightfoot = raycast(new Vector2(footOffset, 0), Vector2.down, footBlock, Ground);
        if (leftfoot || rightfoot)
        {
            isTouchLayer = true;
        }
        else
            isTouchLayer = false;
        //ͷ������
        RaycastHit2D head = raycast(new Vector2(0, box.size.y), Vector2.up, headBlock, Ground); ;
        if (head)
        {
            isheadBlock = true;
        }
        else
            isheadBlock = false;
    }
    void HangingRelieve()//���ҽ��
    {
        //��������
        if (isHanging)
        {
            if (isJumpPress)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.velocity = new Vector3(HangingJumpForce*-0.8f, HangingJumpForce);
                isRelieve = false ;
                isHanging = false ;
            }
            if (CrouchPress)//�����ҵ��������ڣ��������ⲻҪ�����侲����˼���ؼ��㣬����ʵ�飬�ڴ�������һע������֤����㡣
            {
                CrouchPress = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                isRelieve = false;
                isHanging = false;
            }
        }
    }
}
