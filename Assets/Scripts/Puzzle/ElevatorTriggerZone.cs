using UnityEngine;
using System.Collections.Generic;

public class ElevatorTrigger : MonoBehaviour
{
    public ElevatorController elevatorController;

    // Dùng để lưu tất cả người chơi đang đứng trong trigger
    private HashSet<Collider2D> playersOnElevator = new HashSet<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Nếu chưa có trong danh sách thì thêm vào và thông báo
            if (playersOnElevator.Add(other))
            {
                elevatorController.NotifyObjectEntered(playersOnElevator.Count);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playersOnElevator.Remove(other))
            {
                elevatorController.NotifyObjectExited(playersOnElevator.Count);
            }
        }
    }
}

