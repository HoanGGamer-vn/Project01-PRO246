using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Transform[] targets;
    public float moveSpeed = 2f;
    public int requiredObjects = 2;
    public bool returnWhenEmpty = true;

    private Vector3 startPosition;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void NotifyObjectEntered(int currentCount)
    {
        if (currentCount >= requiredObjects && !isMoving)
        {
            MoveToTarget();
        }
    }

    public void NotifyObjectExited(int currentCount)
    {
        if (currentCount < requiredObjects && returnWhenEmpty)
        {
            ReturnToStart();
        }
    }

    private void MoveToTarget()
    {
        isMoving = true;
        StopAllCoroutines();
        StartCoroutine(MoveElevator(targets[0].position)); // hoặc chọn nhiều target tùy logic
    }

    private void ReturnToStart()
    {
        isMoving = true;
        StopAllCoroutines();
        StartCoroutine(MoveElevator(startPosition));
    }

    private System.Collections.IEnumerator MoveElevator(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }
}
