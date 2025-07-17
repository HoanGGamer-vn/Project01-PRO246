using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;

    public float moveSpeed;
    public float jumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
            sprite.flipX = true;
            animator.SetBool("isRunning", animator.GetBool("isGrounded"));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
            sprite.flipX = false;
            animator.SetBool("isRunning", animator.GetBool("isGrounded"));
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.LeftControl) && animator.GetBool("isGrounded"))
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void OnCollisionEnter2D(Collision2D redCollision)
    {
        if (redCollision.gameObject.CompareTag("Tilemap"))
        {
            animator.SetBool("isGrounded", true);
        }
    }

    public void OnCollisionExit2D(Collision2D redCollision)
    {
        if (redCollision.gameObject.CompareTag("Tilemap"))
        {
            animator.SetBool("isGrounded", false);
        }
    }
}
