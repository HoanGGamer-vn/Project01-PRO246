using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 13f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool jumpQueued;
    private bool isGrounded;
    private int groundContacts = 0;

    private Animator animator;
    private SpriteRenderer spriteRenderer;


    [Header("Ground Check")]
    public LayerMask groundLayers; // Layer dùng cho kiểm tra mặt đất

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Xử lý input di chuyển
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<float>();
    }

    // Xử lý input nhảy
    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            jumpQueued = true;
        }
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        // Di chuyển theo trục X mà vẫn giữ nguyên lực vật lý
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Nhảy nếu đang chạm đất
        if (jumpQueued)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f); // reset Y để nhảy đều
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpQueued = false;
        }

        if (moveInput > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < -0.01f)
        {
            spriteRenderer.flipX = true;
        }

    }

    // Khi chạm vào vùng ground có layer nằm trong groundLayers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsInGroundLayer(other.gameObject.layer))
        {
            groundContacts++;
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsInGroundLayer(other.gameObject.layer))
        {
            groundContacts = Mathf.Max(0, groundContacts - 1);
            isGrounded = groundContacts > 0;
        }
    }

    // Kiểm tra layer có nằm trong groundLayers không
    private bool IsInGroundLayer(int layer)
    {
        return (groundLayers.value & (1 << layer)) != 0;
    }
}
