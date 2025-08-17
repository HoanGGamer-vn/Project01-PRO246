using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTriggerManager : MonoBehaviour
{
    private bool isTrapActive;
    public GameObject trap;
    private ElevatorManager elevatorManager;

    // Start is called before the first frame update
    void Start()
    {
        isTrapActive = false;
        elevatorManager = trap.GetComponent<ElevatorManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrapActive)
        {
            if(collision.CompareTag("Blue") || collision.CompareTag("Red"))
            {
                isTrapActive = true;
                elevatorManager.activateElevator = true;
            }
        }
    }
}
