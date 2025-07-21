using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraTargetController : MonoBehaviour
{
    public Rigidbody2D blue;
    public Rigidbody2D red;
    void LateUpdate()
    {
        if (blue != null && red != null)
        {
            Vector2 centerPos = (blue.position + red.position) / 2f;
            transform.position = new Vector3(centerPos.x, centerPos.y, transform.position.z);
        }
    }
}
