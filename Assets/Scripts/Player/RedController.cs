using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedController : MonoBehaviour
{
    public Data data;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    public Rigidbody2D blue_rb;
    private Animator animator;

    public float moveSpeed;
    public float jumpForce;

    private bool isTouchingBlue;
    private bool canJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        isTouchingBlue = false;
        data.isRedGrounded = false;
        canJump = false;
    }

    void Update()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
            sprite.flipX = true;
            animator.SetBool("isRunning", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
            sprite.flipX = false;
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if (isTouchingBlue && data.isBlueStatic && !data.isBlueGrounded)
        {
            blue_rb.velocity = new Vector2(moveInput * moveSpeed, blue_rb.velocity.y);
        }

        if (transform.position.y > blue_rb.transform.position.y && transform.position.x > blue_rb.transform.position.x - 0.5f && transform.position.x < blue_rb.transform.position.x + 0.5f)
        {
            if (isTouchingBlue)
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && canJump)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        }


        if (rb.velocity.magnitude < 0.01f)
        {
            data.isRedStatic = true;
            Debug.Log("Red is static: " + data.isRedStatic);
        }
        else
        {
            data.isRedStatic = false;
            Debug.Log("Red is not static: " + data.isRedStatic);
        }
    }

    public void OnCollisionEnter2D(Collision2D redCollision)
    {
        if (redCollision.gameObject.CompareTag("Tilemap"))
        {
            data.isRedGrounded = true;
            canJump = true;
        }
        if (redCollision.gameObject.CompareTag("Blue"))
        {
            isTouchingBlue = true;
        }
    }

    public void OnCollisionExit2D(Collision2D redCollision)
    {
        if (redCollision.gameObject.CompareTag("Tilemap"))
        {
            data.isRedGrounded = false;
            canJump = false;
        }
        if (redCollision.gameObject.CompareTag("Blue"))
        {
            isTouchingBlue = false;
        }
    }
}
