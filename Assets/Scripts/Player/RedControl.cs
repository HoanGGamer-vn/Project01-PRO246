using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedControl : MonoBehaviour
{
    public Data data;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    public Rigidbody2D blue_rb;
    private Animator animator;

    public float moveSpeed;
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A) && data.canRedMoveLeft)
        {
            moveInput = -1f;
            sprite.flipX = true;
            animator.SetBool("isRunning", true);
        }
        else if (Input.GetKey(KeyCode.D) && data.canRedMoveRight)
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

        if (data.isBlueStandOnRed && data.isBlueStatic)
        {
            blue_rb.velocity = new Vector2(rb.velocity.x, blue_rb.velocity.y);
        }
        //-------------------------------------

        // Jump
        if (Input.GetKeyDown(KeyCode.LeftControl) && data.canRedJump)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
        }
        //-----------------------------------

        // Check static
        if (rb.velocity.magnitude < 0.01f)
        {
            data.isRedStatic = true;
        }
        else
        {
            data.isRedStatic = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D redTrigger)
    {
        if (redTrigger.gameObject.CompareTag("LowLimit"))
        {
            StartCoroutine(WaitforRespawn(2.5f));
        }
    }

    private IEnumerator WaitforRespawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        rb.position = blue_rb.position + new Vector2(0, 5f);
    }
}
