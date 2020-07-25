using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // rigidbody
    private Rigidbody2D rb2d;

    // boxCollider
    private BoxCollider2D bc2d;

    // animator
    private Animator animator;

    // move speed
    [SerializeField] private float moveSpeed = 0.0f;

    // gravivity
    [SerializeField] private float gravity = 0.0f;
    
    // move dir
    private Vector2 moveDir = Vector2.zero;

    // jump
    private float failForce = 0;
    [SerializeField]private bool isJumping = false;

    // failing
    private bool isFailing = false;

    // ground
    [SerializeField] private bool isGround = false;

    // inputs
    private float inputH = 0;
    private float inputV = 0;

    // facing state
    private bool isFacingRight = true;

    // attack
    private bool isAttacking1 = false;
    private bool isAttacking2 = false;
    private bool isAttackOver = true;
    // Start is called before the first frame update
    void Start()
    {
        
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Control();
        GroundDetect();
        AnimationChange();
    }
    
    private void Control()
    {
        

        if (Input.GetKeyDown(KeyCode.Z) && isGround)
        {
            isAttackOver = false;
            if (!isAttacking1 && !isAttacking2)
            {
                isAttacking1 = true;
                animator.SetBool("isAttacking", true);
                animator.SetBool("isRun", false);

            }
            else if (isAttacking1)
            {
                animator.SetBool("isAttacking2", true);                
                isAttacking2 = true;
                isAttacking1 = false;
            }
            else if (isAttacking2)
            {                
                animator.SetBool("isAttacking3", true);
            }
            

            
            
        }             
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.attackOver"))
        {
            isAttacking1 = false;
            isAttacking2 = false;
            animator.SetBool("isAttacking", false);
            animator.SetBool("isAttacking2", false);
            animator.SetBool("isAttacking3", false);

        }   
        if(isAttackOver)
        {
            Move();
        }

        if (CheckAnimatorState("attackOver"))
        {
            isAttackOver = true;
        }
        

        
        
    }

    private void Move()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        TurningFaceTo();

        if (inputV > 0 && !isJumping && isAttackOver)
        {
            isJumping = true;            
            failForce = moveSpeed * 2.5f;
            animator.SetBool("isIdle", false);

        }
        else if (isJumping)
        {
            failForce -= Time.deltaTime * moveSpeed * 3.0f;
        }
                
        moveDir = new Vector2(inputH * moveSpeed, failForce);


        if (!isGround && !isJumping)
        {
            failForce += -gravity * Time.deltaTime;
            moveDir.y = failForce;
        }
        


        if(moveDir.y < 0)
        {
            isFailing = true;
        }
        else
        {
            isFailing = false;
        }
        rb2d.velocity = moveDir;
        
    }

    private void GroundDetect()
    {
        float height = bc2d.size.y / 2 + 0.05f;
        float width = (bc2d.size.x / 2);
        float dis = height;
        Vector2 rightStartPos = new Vector2(transform.position.x + bc2d.offset.x + width, transform.position.y + bc2d.offset.y);
        Vector2 leftStartPos = new Vector2(transform.position.x + bc2d.offset.x - width, transform.position.y + bc2d.offset.y);
        Vector2 rightEndPos = new Vector2(rightStartPos.x, rightStartPos.y - height);
        Vector2 leftEndPos = new Vector2(leftStartPos.x, rightStartPos.y - height);
        Debug.DrawLine(rightStartPos, rightEndPos, Color.red);
        Debug.DrawLine(leftStartPos, leftEndPos, Color.red);
        isGround = false;
        RaycastHit2D rightHit = Physics2D.Raycast(rightStartPos, rightEndPos - rightStartPos, dis, ~(1 << 8));
        RaycastHit2D leftHit = Physics2D.Raycast(leftStartPos, leftEndPos - leftStartPos, dis, ~(1 << 8) );
        if (rightHit.collider != null || leftHit.collider != null) 
        {
            isGround = true;
            isJumping = false;
            failForce = 0;
        }

    }

    private void AnimationChange()
    {
        if (isGround)
        {
            animator.SetBool("isFailing", false);
            animator.SetBool("isJump", false);
            if (inputH != 0)
            {
                animator.SetBool("isRun", true);
                animator.SetBool("isIdle", false);
            }
            else if (inputH == 0)
            {
                animator.SetBool("isIdle", true);
                animator.SetBool("isRun", false);
            }
        }
        else
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isRun", false);
            if (isJumping && !isFailing)
            {                
                animator.SetBool("isJump", true);
                animator.SetBool("isFailing", false);

            }
            else if (isFailing)
            {
                animator.SetBool("isJump", false);
                animator.SetBool("isFailing", true);
            }
        }        
    }

    private void TurningFaceTo()
    {
        if(inputH > 0 && !isFacingRight)
        {
            isFacingRight = true;
            float tmpX = -transform.localScale.x;
            transform.localScale = new Vector3(tmpX, transform.localScale.y, transform.localScale.z);
        }
        else if(inputH < 0 && isFacingRight)
        {
            isFacingRight = false;
            float tmpX = -transform.localScale.x;
            transform.localScale = new Vector3(tmpX, transform.localScale.y, transform.localScale.z);
        }
    }

    private bool CheckAnimatorState(string str)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer." + str))
        {
            return true;
        }
        return false;
    }

}
