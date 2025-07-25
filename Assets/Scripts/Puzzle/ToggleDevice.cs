using UnityEngine;
using System.Collections;

public class ToggleDevice : MonoBehaviour
{
    [Header("Target Movement Settings")]
    public Transform targetPosition;
    public float moveSpeed = 2f;

    private Vector3 startPosition;
    private Coroutine moveCoroutine;
    private bool isOn = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void TurnOn()
    {
        if (!isOn)
        {
            isOn = true;
            StartMove(targetPosition.position);
        }
    }

    public void TurnOff()
    {
        if (isOn)
        {
            isOn = false;
            StartMove(startPosition);
        }
    }

    private void StartMove(Vector3 target)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveTo(target));
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }
}
