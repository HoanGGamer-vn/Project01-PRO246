using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer sr;
    public bool isOneTimeButton;
    public Sprite button;
    public Sprite pressedButton;
    public GameObject elevator;
    private ElevatorManager elevatorManager;

    private int pressedCount;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = button;
        pressedCount = 0;
        elevatorManager = elevator.GetComponent<ElevatorManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOneTimeButton)
        {
            if (collision.gameObject.CompareTag("Blue") || collision.gameObject.CompareTag("Red") || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Ball"))
            {
                sr.sprite = pressedButton;
                elevatorManager.activateElevator = true;
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Blue") || collision.gameObject.CompareTag("Red") || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Ball"))
            {
                pressedCount++;
                if (pressedCount == 1)
                {
                    sr.sprite = pressedButton;
                    elevatorManager.activateElevator = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isOneTimeButton)
        {
            return;
        }
        else
        {
            if (collision.gameObject.CompareTag("Blue") || collision.gameObject.CompareTag("Red") || collision.gameObject.CompareTag("Box") || collision.gameObject.CompareTag("Ball"))
            {
                pressedCount--;
                if (pressedCount <= 0)
                {
                    sr.sprite = button;
                    elevatorManager.activateElevator = false;
                }
            }
        }
    }
}
