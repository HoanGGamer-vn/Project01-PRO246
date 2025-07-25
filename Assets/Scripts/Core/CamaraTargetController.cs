using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraTargetController : MonoBehaviour
{
    public Data data;
    public Rigidbody2D blue;
    public Rigidbody2D red;
    private float distance;
    void Start()
    {
        data.canRedMoveRight = true;
        data.canRedMoveLeft = true;
        data.canBlueMoveRight = true;
        data.canBlueMoveLeft = true;
    }
    void LateUpdate()
    {
        if (blue != null && red != null)
        {
            Vector2 centerPos = (blue.position + red.position) / 2f;
            transform.position = new Vector3(centerPos.x, centerPos.y, transform.position.z);
        }

        distance = Vector2.Distance(blue.position, red.position);
        if(distance > 16)
        {
            if(blue.position.x > red.position.x)
            {
                data.canRedMoveRight = true;
                data.canRedMoveLeft = false;
                data.canBlueMoveRight = false;
                data.canBlueMoveLeft = true;
            }
            else
            {
                data.canRedMoveRight = false;
                data.canRedMoveLeft = true;
                data.canBlueMoveRight = true;
                data.canBlueMoveLeft = false;
            }
        }
        else
        {
            data.canRedMoveRight = true;
            data.canRedMoveLeft = true;
            data.canBlueMoveRight = true;
            data.canBlueMoveLeft = true;
        }
    }
}
