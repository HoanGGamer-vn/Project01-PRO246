using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RedControl : MonoBehaviour
{
    public Data data;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    public Rigidbody2D blue_rb;
    private Animator animator;

    public float moveSpeed;
    public float jumpForce;

    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveInput = moveAction.action.ReadValue<float>();

        if (moveInput < 0 && data.canRedMoveLeft)
        {
            sprite.flipX = true;
            animator.SetBool("isRunning", true);
        }
        else if (moveInput > 0 && data.canRedMoveRight)
        {
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

        if (jumpAction.action.WasPressedThisFrame() && data.canRedJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        data.isRedStatic = rb.velocity.magnitude < 0.01f;
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
