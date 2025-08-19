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
    private bool isAlive;
    private Vector2 spawnPoint;
    private bool checkSpawnPointOnBlue;

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
        checkSpawnPointOnBlue = true;
        spawnPoint = blue_rb.position + new Vector2(0, 5f);

        isAlive = true;
        animator.SetBool("isAlive", true);
    }

    void Update()
    {
        float moveAxis = moveAction.action.ReadValue<float>();
        float moveInput = 0;
        if (moveAxis < 0 && data.canRedMoveLeft)
        {
            moveInput = -1;
            sprite.flipX = true;
            animator.SetBool("isRunning", true);
        }
        else if (moveAxis > 0 && data.canRedMoveRight)
        {
            moveInput = 1;
            sprite.flipX = false;
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        if (isAlive)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            animator.SetBool("isAlive", false);
            StartCoroutine(WaitForDead());
        }

        if (data.isBlueStandOnRed && data.isBlueStatic)
        {
            blue_rb.velocity = new Vector2(rb.velocity.x, blue_rb.velocity.y);
        }

        if (jumpAction.action.WasPressedThisFrame() && data.canRedJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        data.isRedStatic = rb.velocity.magnitude < 0.01f;

        SetSpawnPointOnBlue();
    }

    public void OnTriggerEnter2D(Collider2D redTrigger)
    {
        if (redTrigger.gameObject.CompareTag("LowLimit"))
        {
            StartCoroutine(WaitforRespawn(2.5f));
        }
        if (redTrigger.gameObject.CompareTag("Trap"))
        {
            if (isAlive)
            {
                StartCoroutine(WaitforRespawn(2f));
                isAlive = false;
            }
        }
        if (redTrigger.gameObject.CompareTag("SpawnPoint"))
        {
            checkSpawnPointOnBlue = false;
            spawnPoint = transform.position;
        }
    }

    private IEnumerator WaitforRespawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        rb.position = spawnPoint;
        isAlive = true;
        animator.SetBool("isAlive", true);
    }

    private IEnumerator WaitForDead()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
    }
    private void SetSpawnPointOnBlue()
    {
        if (checkSpawnPointOnBlue && spawnPoint != blue_rb.position + new Vector2(0, 5f))
        {
            spawnPoint = blue_rb.position + new Vector2(0, 5f);
        }
    }
}
