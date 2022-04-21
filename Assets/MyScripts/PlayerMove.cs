using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D box;

    [Header("移动参数")]
    public float speed = 5;//移动速度
    public float JumpForce = 5;//跳跃力度
    public float JumpHoldForce = 6;//蓄力跳力度
    public float HangingJumpForce = 10;//悬挂跳跃力度
    public float CrouchSpeedDivisor = 3;//蹲下时速度减慢，速度的除数
    public float JumpDuration = 0.1f;//蓄力跳持续时间                                    
    
    public float CrouchJumpForce = 2;//蹲下跳跃增加力度                      
      
    [Header("状态判断")]
    public bool isCrouch = false;//是否蹲下
    public bool isStand = false;//是否站起
    public bool isTouchLayer = false;//是否接触地面
    public bool isJumping = false;//是否正在跳跃
    public bool isheadBlock = false;//头顶是否被挡住
    public bool isHanging = false;//是否悬挂

    //杂项
    float xVelocity;//接受来自键盘的水平力的方向  
    float Jumptime;//现实时间加蓄力跳时间

    [Header("图层名称")]
    public LayerMask Ground;//Platforms的图层名称

    [Header("射线判断")]
    public float footOffset = 0.45f;//双脚的位置
    public float headBlock = 0.2f;//头顶射线碰撞的距离
    public float footBlock = 0.2f;//双脚射线碰撞的距离

    [Header("悬挂参数")]
    public float eyesHight = 1.55f;//眼睛的高度
    public float grabDistance = 0.4f;//悬挂的距离
    public float reachOffset = 0.7f;//判断下面有没有墙壁的射线距角色的距离
    public float playHeight = 0.05f;//角色高度


    //碰撞器大小和位置
    Vector2 Standoffset;
    Vector2 Standsize;
    Vector2 Crouchoffset;
    Vector2 Crouchsize;

    //按键参数
    bool isJumpHold = false;
    bool isJumpPress = false;
    bool CrouchHold = false;
    bool CrouchPress = false;

    void Awake()                
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        Standoffset = box.offset;
        Standsize = box.size;
        playHeight += box.size.y;
        Crouchsize = new Vector2(Standsize.x, Standsize.y / 2);
        Crouchoffset = new Vector2(Standoffset.x, Standoffset.y / 2);
    }
    //脚本的一些成员，如果想在创建之后的代码中立即使用，则必须写在Awake()里面. 
    //加载Scence时会先对所有脚本的Awake()先执行. 再执行Start()
    //因此如果脚本A在初始化时需要调用到脚本B里的变量.
    //那A的调用语句 应放在Start()中,而不是Awake()
    //而B脚本要被调用的变量应在Awake()中执行初始化.


    void Update()//Update里写需要实时渲染的代码和所有获取按键的代码，如Button，Key相关的代码
    {     
        xVelocity = Input.GetAxis("Horizontal");
        if (!isHanging)
        {
          PlayerFacing();
        }      
        if (Input.GetButtonDown("Jump"))//不能直接写JumpPress=Input.GetButtonDown("Jump")和JumpHold=Input.GetButton("Jump"),会让跳跃不流畅
        {
            isJumpPress = true;
        }
        if (Input.GetButton("Jump"))
        {
            isJumpHold = true;
        }
        CrouchHold = Input.GetButton("Crouch");//这里的蹲下不能与上面的跳跃一样写，原因未知
        if (Input.GetButtonDown("Crouch"))
        {
            CrouchPress = true;
        }     
    }
    private void FixedUpdate()//FixedUpdate里写与刚体相关的需要连续物理呈现的代码，如移动。经过实验，跳跃相关的代码放在FixedUpdate里跳跃才会流畅。
    {
        RayCheck();
        GroundMovement();
        AccumulatorJump();
    }
    void GroundMovement()//水平移动
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
    void PlayerFacing()//玩家朝向
    {
        if (xVelocity < 0)
            transform.localScale = new Vector2(-1, 1);
        else if (xVelocity > 0)
            transform.localScale = new Vector2(1, 1);
    }
    void Crouch()//蹲下
    {
        isCrouch = true;
        box.offset = Crouchoffset;
        box.size = Crouchsize;
        isStand = false;
    }
    void Stand()//站起
    {
        isStand = true;
        box.offset = Standoffset;
        box.size = Standsize;
        isCrouch = false;
    }     
   void AccumulatorJump()//跳跃代码总结：跳跃代码写在FixedUpdate里，在Update获得按键参数，用if和bool判断是否按下了按键，再将值传送到FixedUpdate里，这样跳跃才能流畅不受帧数影响
   {
        if (isTouchLayer && isJumpPress && !isheadBlock&&!isHanging)//防止头有墙的时候可以跳
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
    RaycastHit2D raycast(Vector2 offset,Vector2 rayDiraction,float Distance,LayerMask layer)//射线函数模板
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, Distance, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDiraction, color);
        return hit;
    }
    void RayCheck()//利用射线判断物理情况（射线起点以外的部分碰到碰撞体条件才成立）
    {
        //悬挂射线
        if (isHanging)
        {
            if (isJumpPress)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;            
                rb.AddForce(Vector2.up * HangingJumpForce, ForceMode2D.Impulse);                                 
                isHanging = false;             
            }         
            if (CrouchPress)
            {
                CrouchPress = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }
        float dirction = transform.localScale.x;
        Vector2 grabDir = new Vector2(dirction, 0);
        RaycastHit2D headCheck = raycast(new Vector2(footOffset * dirction, playHeight), grabDir, grabDistance, Ground);//头顶射线 判断上面是否有墙
        RaycastHit2D wallCheck = raycast(new Vector2(footOffset * dirction, eyesHight), grabDir, grabDistance, Ground);//眼睛射线 判断前方是否有墙
        RaycastHit2D reachCheck = raycast(new Vector2(reachOffset * dirction, playHeight), Vector2.down, grabDistance, Ground);//从头上往下画一条射线判断下方是否有墙
        if (!isTouchLayer && rb.velocity.y <= 0 && !headCheck && wallCheck && reachCheck)
        {
            Vector3 pos = transform.position;
            pos.x += (wallCheck.distance-0.08f) * dirction;
            pos.y = pos.y - reachCheck.distance ;
            transform.position = pos;
            isHanging = true;
            rb.bodyType = RigidbodyType2D.Static;
        }
        
      //左右脚射线
      RaycastHit2D leftfoot = raycast(new Vector2(-footOffset,0),Vector2.down,footBlock,Ground);
        RaycastHit2D rightfoot = raycast(new Vector2(footOffset, 0), Vector2.down, footBlock, Ground);
        if (leftfoot || rightfoot)
        {
            isTouchLayer = true;
        }
        else
            isTouchLayer = false;
        //头顶射线
        RaycastHit2D head = raycast(new Vector2(0, box.size.y), Vector2.up, headBlock, Ground); ;
        if (head)
        {
            isheadBlock = true;
        }
        else
            isheadBlock = false;
    }
}
