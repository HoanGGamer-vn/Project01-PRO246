using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public bool needButton;
    public bool isRepeat;
    public bool activateElevator;
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
}
