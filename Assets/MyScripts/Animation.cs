using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    PlayerMove move;
    Animator animator;
    int speedID;
    int isHangingID;
    int isCrouchingID;
    int isOnGroundID;
    int fallID;
    Rigidbody2D rb;
    void Start()
    {    
        move = GetComponentInParent<PlayerMove>();
        rb = PlayerMove.FindObjectOfType<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speedID = Animator.StringToHash("speed");
        isHangingID = Animator.StringToHash("isHanging");
        isCrouchingID = Animator.StringToHash("isCrouching");
        isOnGroundID = Animator.StringToHash("isOnGround");
        fallID = Animator.StringToHash("verticalVelocity");
    }


    void Update()
    {   
        animator.SetFloat(speedID,Mathf.Abs(move.xVelocity));
        animator.SetBool(isOnGroundID, move.isTouchLayer);
        animator.SetBool(isHangingID, move.isHanging);
        animator.SetBool(isCrouchingID, move.isCrouch);
        animator.SetFloat(fallID,rb.velocity.y);
    }
    public void StepAudio()
    {

    }

    public void CrouchStepAudio()
    {

    }
}
