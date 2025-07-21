using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueController : MonoBehaviour
{
    public Data data;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    public Rigidbody2D red_rb;
    private Animator animator;

    public float moveSpeed;
    public float jumpForce;

    private bool isTouchingRed;
    private bool canJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        isTouchingRed = false;
        data.isBlueGrounded = false;
        canJump = false;
    }

    void Update()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
            sprite.flipX = true;
            animator.SetBool("isRunning", true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
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
        if (isTouchingRed && data.isRedStatic && !data.isRedGrounded)
        {
            red_rb.velocity = new Vector2(moveInput * moveSpeed, red_rb.velocity.y);
        }
        if (transform.position.y > red_rb.transform.position.y && transform.position.x > red_rb.transform.position.x - 0.5f && transform.position.x < red_rb.transform.position.x + 0.5f)
        {
            if (isTouchingRed)
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && canJump)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        }

        if (rb.velocity.magnitude < 0.01f)
        {
            data.isBlueStatic = true;
        }
        else
        {
            data.isBlueStatic = false;
        }
    }
    public void OnCollisionEnter2D(Collision2D blueCollision)
    {
        if (blueCollision.gameObject.CompareTag("Tilemap"))
        {
            data.isBlueGrounded = true;
            canJump = true;
        }
        if(blueCollision.gameObject.CompareTag("Red"))
        {
            isTouchingRed = true;
        }
    }

    public void OnCollisionExit2D(Collision2D blueCollision)
    {
        if (blueCollision.gameObject.CompareTag("Tilemap"))
        {
            data.isBlueGrounded = false;
            canJump = false;
        }
        if (blueCollision.gameObject.CompareTag("Red"))
        {
            isTouchingRed = false;
        }
    }
    public void OnTriggerEnter2D(Collider2D blueTrigger)
    {
        if (blueTrigger.gameObject.CompareTag("LowLimit"))
        {
            rb.position = red_rb.position + new Vector2(0, 5f);
        }
    }
}
