using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManage : MonoBehaviour
{
    public Sprite ButtonOFF;
    public Sprite ButtonON;

    public GameObject Bridge;
    public Vector2 targetPosition;
    public float moveSpeed = 2f;

    private Vector2 startPosition;              // Vị trí ban đầu của cầu
    private SpriteRenderer spriteRenderer;

    private bool moveToTarget = false;
    private bool moveToStart = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ButtonOFF;

        if (Bridge != null)
        {
            startPosition = Bridge.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = ButtonON;
            moveToTarget = true;
            moveToStart = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.sprite = ButtonOFF;
            moveToStart = true;
            moveToTarget = false;
        }
    }

    void Update()
    {
        if (Bridge == null) return;

        if (moveToTarget)
        {
            Bridge.transform.position = Vector2.MoveTowards(
                Bridge.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(Bridge.transform.position, targetPosition) < 0.01f)
            {
                moveToTarget = false;
            }
        }

        if (moveToStart)
        {
            Bridge.transform.position = Vector2.MoveTowards(
                Bridge.transform.position,
                startPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(Bridge.transform.position, startPosition) < 0.01f)
            {
                moveToStart = false;
            }
        }
    }
}
