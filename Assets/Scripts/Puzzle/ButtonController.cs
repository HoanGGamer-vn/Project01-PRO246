using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer sr;
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
        if (collision.gameObject.CompareTag("Blue") || collision.gameObject.CompareTag("Red") || collision.gameObject.CompareTag("Box"))
        {
            pressedCount++;
            if (pressedCount == 1)
            {
                sr.sprite = pressedButton;
                elevatorManager.activateElevator = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Blue") || collision.gameObject.CompareTag("Red") || collision.gameObject.CompareTag("Box"))
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
