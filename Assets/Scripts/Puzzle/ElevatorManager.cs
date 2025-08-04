using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public bool activateElevator;

    public float elevatorSpeed;
    public float inputX;
    public float inputY;

    private Vector2 startPos;
    private Vector2 currentPos;
    private Vector2 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        activateElevator = false;
        startPos = transform.position;
        targetPos = new Vector2(startPos.x + inputX, startPos.y + inputY);

    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        if (activateElevator)
        {
            transform.position = Vector2.MoveTowards(currentPos, targetPos, elevatorSpeed * Time.deltaTime);
        }
        else
        {
            if(currentPos != startPos)
            {
                transform.position = Vector2.MoveTowards(currentPos, startPos, elevatorSpeed * Time.deltaTime);
            }
        }
    }
}
