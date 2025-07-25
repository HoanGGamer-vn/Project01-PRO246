using UnityEngine;
using System.Collections.Generic;

public class ElevatorTrigger : MonoBehaviour
{
    public ElevatorController elevatorController;

    // Dùng để lưu tất cả người chơi đang đứng trong trigger
    private HashSet<Collider2D> playersOnElevator = new HashSet<Collider2D>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsRelevantTag(other))
        {
            if (playersOnElevator.Add(other))
            {
                elevatorController.CheckTagsCondition(playersOnElevator);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsRelevantTag(other))
        {
            if (playersOnElevator.Remove(other))
            {
                elevatorController.CheckTagsCondition(playersOnElevator);
            }
        }
    }
    private bool IsRelevantTag(Collider2D other)
    {
        return other.CompareTag("Red") || other.CompareTag("Blue") || other.CompareTag("Box");
    }
}

