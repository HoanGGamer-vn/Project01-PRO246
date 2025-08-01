using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform[] targets;
    public float moveSpeed = 2f;
    public bool returnWhenNotEnoughTags = true;
    
    [Header("Activation Settings")]
    [Tooltip("Số lượng tag khác nhau cần có để kích hoạt elevator")]
    public int requiredTagCount = 2;
    
    [Tooltip("Các tag được chấp nhận để kích hoạt")]
    public string[] acceptedTags = { "Red", "Blue", "Box" };

    private Vector3 startPosition;
    private bool isMoving = false;

    private void Start()
    {
        startPosition = transform.position;
        
        // Validation settings
        ValidateSettings();
    }
    
    /// <summary>
    /// Kiểm tra và điều chỉnh settings nếu cần
    /// </summary>
    private void ValidateSettings()
    {
        // Đảm bảo requiredTagCount hợp lệ
        if (requiredTagCount <= 0)
        {
            requiredTagCount = 1;
            Debug.LogWarning("Required Tag Count phải > 0, đã set về 1");
        }
        
        if (requiredTagCount > acceptedTags.Length)
        {
            requiredTagCount = acceptedTags.Length;
            Debug.LogWarning($"Required Tag Count không thể > số accepted tags, đã set về {acceptedTags.Length}");
        }
        
        // Đảm bảo có accepted tags
        if (acceptedTags == null || acceptedTags.Length == 0)
        {
            acceptedTags = new string[] { "Red", "Blue", "Box" };
            Debug.LogWarning("Accepted Tags trống, đã set mặc định: Red, Blue, Box");
        }
        
        // Đảm bảo có targets
        if (targets == null || targets.Length == 0)
        {
            Debug.LogWarning("Elevator không có targets để di chuyển đến!");
        }
        
        Debug.Log($"Elevator Settings - Cần {requiredTagCount} tags từ: [{string.Join(", ", acceptedTags)}]");
    }

    public void CheckTagsCondition(HashSet<Collider2D> currentObjects)
    {
        HashSet<string> tagSet = new HashSet<string>();

        foreach (var obj in currentObjects)
        {
            if (obj == null) continue;

            // Kiểm tra tag với danh sách accepted tags
            foreach (string acceptedTag in acceptedTags)
            {
                if (obj.CompareTag(acceptedTag))
                {
                    tagSet.Add(acceptedTag);
                    break; // Thoát vòng lặp khi tìm thấy tag phù hợp
                }
            }
        }

        // Debug thông tin tags hiện tại
        Debug.Log($"Tags hiện tại: {string.Join(", ", tagSet)} | Cần: {requiredTagCount} | Có: {tagSet.Count}");

        // Kiểm tra điều kiện kích hoạt
        if (tagSet.Count >= requiredTagCount)
        {
            if (!isMoving)
            {
                Debug.Log("Đủ điều kiện kích hoạt elevator!");
                MoveToTarget();
            }
        }
        else if (returnWhenNotEnoughTags)
        {
            if (isMoving || transform.position != startPosition)
            {
                Debug.Log("Không đủ điều kiện, trở về vị trí ban đầu");
                ReturnToStart();
            }
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
        Debug.Log("Elevator đã đến vị trí: " + target);
    }
    
    #region Public Utility Methods
    
    /// <summary>
    /// Kiểm tra elevator có đang di chuyển không
    /// </summary>
    public bool IsMoving()
    {
        return isMoving;
    }
    
    /// <summary>
    /// Lấy số lượng tag cần thiết
    /// </summary>
    public int GetRequiredTagCount()
    {
        return requiredTagCount;
    }
    
    /// <summary>
    /// Set số lượng tag cần thiết (runtime)
    /// </summary>
    public void SetRequiredTagCount(int count)
    {
        if (count > 0 && count <= acceptedTags.Length)
        {
            requiredTagCount = count;
            Debug.Log($"Required Tag Count đã được set về: {requiredTagCount}");
        }
        else
        {
            Debug.LogWarning($"Invalid tag count: {count}. Phải từ 1 đến {acceptedTags.Length}");
        }
    }
    
    /// <summary>
    /// Lấy danh sách accepted tags
    /// </summary>
    public string[] GetAcceptedTags()
    {
        return acceptedTags;
    }
    
    /// <summary>
    /// Kiểm tra elevator có ở vị trí start không
    /// </summary>
    public bool IsAtStartPosition()
    {
        return Vector3.Distance(transform.position, startPosition) < 0.01f;
    }
    
    /// <summary>
    /// Force elevator về start position
    /// </summary>
    public void ForceReturnToStart()
    {
        StopAllCoroutines();
        isMoving = false;
        transform.position = startPosition;
        Debug.Log("Elevator đã được force về start position");
    }
    
    #endregion
}
