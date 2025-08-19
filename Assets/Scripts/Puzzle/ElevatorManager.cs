using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public bool needButton;
    public bool isRepeat;
    public bool activateElevator;
    public bool isNeed2;
    private int playerCount;
    private bool isGoing;

    public float elevatorSpeed;
    public float inputX;
    public float inputY;

    private Vector2 startPos;
    private Vector2 currentPos;
    private Vector2 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        if (needButton)
        {
            activateElevator = false;
        }
        else
        {
            isGoing = true;
        }
        playerCount = 0;
        startPos = transform.position;
        targetPos = new Vector2(startPos.x + inputX, startPos.y + inputY);
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        if (activateElevator)
        {
            if (isRepeat)
            {
                RepeatElevator();
            }
            else
            {
                GoToTarget();
            }
        }
        else
        {
            ReturnToStart();
        }
        if(isNeed2)
        {

        }
        else
        {
        }
    }

    void GoToTarget()
    {
        transform.position = Vector2.MoveTowards(currentPos, targetPos, elevatorSpeed * Time.deltaTime);
    }
    void ReturnToStart()
    {
        if (Vector2.Distance(currentPos, startPos) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(currentPos, startPos, elevatorSpeed * Time.deltaTime);
        }
    }
    void RepeatElevator()
    {
        if (Vector2.Distance(currentPos, targetPos) < 0.1f && isGoing)
        {
            isGoing = false;
        }
        if (Vector2.Distance(currentPos, startPos) < 0.1f && !isGoing)
        {
            isGoing = true;
        }

        if (isGoing)
        {
            GoToTarget();
        }
        else
        {
            ReturnToStart();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Blue") || collision.gameObject.CompareTag("Red"))
        {
            playerCount++;
            if (playerCount >= 2 && isNeed2)
            {
                activateElevator = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Blue") || collision.gameObject.CompareTag("Red"))
        {
            playerCount--;
            if (playerCount < 2 && isNeed2)
            {
                activateElevator = false;
            }
        }
    }
}
