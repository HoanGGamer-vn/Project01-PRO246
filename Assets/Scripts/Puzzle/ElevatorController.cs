using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Transform[] targets;
    public float moveSpeed = 2f;
    public bool returnWhenNotEnoughTags = true;

    private Vector3 startPosition;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void CheckTagsCondition(HashSet<Collider2D> currentObjects)
    {
        HashSet<string> tagSet = new HashSet<string>();

        foreach (var obj in currentObjects)
        {
            if (obj == null) continue;

            if (obj.CompareTag("Red")) tagSet.Add("Red");
            else if (obj.CompareTag("Blue")) tagSet.Add("Blue");
            else if (obj.CompareTag("Box")) tagSet.Add("Box");
        }

        if (tagSet.Count >= 2)
        {
            if (!isMoving)
                MoveToTarget();
        }
        else if (returnWhenNotEnoughTags)
        {
            ReturnToStart();
        }
    }

    private void MoveToTarget()
    {
        isMoving = true;
        StopAllCoroutines();
        StartCoroutine(MoveElevator(targets[0].position));
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
