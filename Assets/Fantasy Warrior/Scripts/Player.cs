using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private float horizontalInput;
    public float movementSpeed;
    public float jumpForce;
    public GameObject enemy;
    private bool isGrounded;
    public bool isAttacking;
    public int xDirection;
    public bool isFacingRight;
    public bool isBlocking;
    public int attackCounter = 0;
    public float health = 100.0f;
    public bool isEnemyNear;
    public bool isDead;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (gameObject.CompareTag("PlayerOne"))
        {
            xDirection = 1;
            isFacingRight = true;
        }
        else
        {
            xDirection = -1;
            isFacingRight = false;
        }
    }

    
    private void Update()
    {
        CheckHealth();

        HorizontalMove();

        Jump();

        AnimationControllers();

        LookAtEnemy();

        StartAttacking();

        RangedAttack();

        Block();
    }


    private void CheckHealth()
    {
        if (health <= 0)
        {
            isDead = true;
        }
    }


    private void Block()
    {
        string blockButtonName = gameObject.CompareTag("PlayerOne") ? "BlockOne" : "BlockTwo";
        if (Input.GetButton(blockButtonName))
        {
            if (!isGrounded || isAttacking || isDead) return;
            isBlocking = true;
        }
        if (Input.GetButtonUp(blockButtonName)) { isBlocking = false; }
    }


    private void RangedAttack()
    {
        string rangedButtonName = gameObject.CompareTag("PlayerOne") ? "RangedOne" : "RangedTwo";
        if (Input.GetButtonDown(rangedButtonName))
        {
            if (!isGrounded || isBlocking || isAttacking || isDead) return;
            animator.SetTrigger("rangedAttack");
        }
    }


    public void OnRangedAttakAnim()
    {
        GameObject gameObj = Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);
        gameObj.GetComponent<Projectile>().enemy = enemy;
    }


    private void StartAttacking()
    {
        string attackButtonName = gameObject.CompareTag("PlayerOne") ? "AttackOne" : "AttackTwo";
        if (Input.GetButtonDown(attackButtonName))
        {
            if (!isGrounded || isBlocking || isDead) return;
            if (!isAttacking)
                isAttacking = true;
        }
    }


    private void LookAtEnemy()
    {
        if ((transform.position.x > enemy.transform.position.x) && isFacingRight) 
        {
            xDirection = -1;
            isFacingRight = false;
            transform.Rotate(0, 180, 0);
        }
        else if ((transform.position.x < enemy.transform.position.x) && !isFacingRight)
        {
            xDirection = 1;
            isFacingRight = true;
            transform.Rotate(0, 180, 0);
        }
    }


    private void AnimationControllers()
    {
        animator.SetInteger("xVelocity", ((int)horizontalInput) * xDirection);   
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetInteger("attackCounter", attackCounter);
        animator.SetBool("isDead", isDead);
        animator.SetBool("isBlocking", isBlocking);
    }


    private void Jump()
    {
        string jumpButtonName = gameObject.CompareTag("PlayerOne") ? "JumpOne" : "JumpTwo";
        if (Input.GetButtonDown(jumpButtonName))
        {
            if (isGrounded && !isDead)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            }
        }
    }


    private void HorizontalMove()
    {
        if (!isBlocking && !isDead)
        {
            string horizontalAxisName = gameObject.CompareTag("PlayerOne") ? "HorizontalOne" : "HorizontalTwo";

            horizontalInput = Input.GetAxisRaw(horizontalAxisName);

            rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);
        }
    }


    public void OnMeeleAttackAnimEnd()
    {
        isAttacking = false;

        if (isEnemyNear)
        {
            MeeleAttack();
        }

        attackCounter++;
        if (attackCounter > 2)
            attackCounter = 0;
    }


    private void MeeleAttack()
    {
        int damage = 0;
        switch (attackCounter)
        {
            case 0:
                damage = 5;
                break;
            case 1:
                damage = 10;
                break;
            case 2:
                damage = 15;
                break;
        }

        enemy.GetComponent<Player>().TakeDamage(damage);   
    }


    public void TakeDamage(int hitPoints)
    {
        if (isBlocking || isDead) return;
        animator.SetTrigger("isTakingHit");
        health -= hitPoints;
    }


    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
