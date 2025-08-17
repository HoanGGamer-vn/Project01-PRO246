using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeBetweenPlayers : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    private void LateUpdate()
    {
        if (player1 != null && player2 != null)
        {
            line.SetPosition(0, player1.position);
            line.SetPosition(1, player2.position);
        }
    }
}
