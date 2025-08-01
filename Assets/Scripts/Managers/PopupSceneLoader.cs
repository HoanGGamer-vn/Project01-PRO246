using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script helper để gắn vào UI Buttons trong popup
/// Có thể gọi SceneReloader hoặc reload trực tiếp nếu không có SceneReloader
/// </summary>
public class PopupSceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Tên scene muốn load (để trống = reload scene hiện tại)")]
    public string targetSceneName = "";
    
    [Tooltip("Hoặc dùng scene index (-1 = reload scene hiện tại)")]
    public int targetSceneIndex = -1;
    
    [Header("Reload Options")]
    [Tooltip("Có delay trước khi reload không (giây)")]
    public float delayBeforeReload = 0f;
    
    [Tooltip("Có đóng popup trước khi reload không")]
    public bool closePopupFirst = true;
    
    /// <summary>
    /// Method gọi từ UI Button để reload scene hiện tại
    /// </summary>
    public void ReloadCurrentScene()
    {
        if (closePopupFirst)
        {
            CloseCurrentPopup();
        }
        
        if (delayBeforeReload > 0)
        {
            Invoke(nameof(DoReloadCurrent), delayBeforeReload);
        }
        else
        {
            DoReloadCurrent();
        }
    }
    
    /// <summary>
    /// Method gọi từ UI Button để load scene đã setup
    /// </summary>
    public void LoadTargetScene()
    {
        if (closePopupFirst)
        {
            CloseCurrentPopup();
        }
        
        if (delayBeforeReload > 0)
        {
            Invoke(nameof(DoLoadTarget), delayBeforeReload);
        }
        else
        {
            DoLoadTarget();
        }
    }
    
    /// <summary>
    /// Method gọi từ UI Button để load scene cụ thể theo tên
    /// </summary>
    public void LoadSceneByName(string sceneName)
    {
        if (closePopupFirst)
        {
            CloseCurrentPopup();
        }
        
        targetSceneName = sceneName;
        
        if (delayBeforeReload > 0)
        {
            Invoke(nameof(DoLoadTarget), delayBeforeReload);
        }
        else
        {
            DoLoadTarget();
        }
    }
    
    /// <summary>
    /// Method gọi từ UI Button để load scene cụ thể theo index
    /// </summary>
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (closePopupFirst)
        {
            CloseCurrentPopup();
        }
        
        targetSceneIndex = sceneIndex;
        
        if (delayBeforeReload > 0)
        {
            Invoke(nameof(DoLoadTarget), delayBeforeReload);
        }
        else
        {
            DoLoadTarget();
        }
    }
    
    #region Private Methods
    
    private void DoReloadCurrent()
    {
        Debug.Log("Popup reload scene hiện tại");
        
        // Thử dùng SceneReloader trước
        if (SceneReloader.Instance != null)
        {
            SceneReloader.ReloadCurrentScene();
        }
        else
        {
            // Fallback: reload trực tiếp
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    private void DoLoadTarget()
    {
        // Ưu tiên tên scene trước
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log("Popup load scene: " + targetSceneName);
            
            if (SceneReloader.Instance != null)
            {
                SceneReloader.LoadScene(targetSceneName);
            }
            else
            {
                SceneManager.LoadScene(targetSceneName);
            }
        }
        // Sau đó mới dùng index
        else if (targetSceneIndex >= 0)
        {
            Debug.Log("Popup load scene index: " + targetSceneIndex);
            
            if (SceneReloader.Instance != null)
            {
                SceneReloader.LoadScene(targetSceneIndex);
            }
            else
            {
                SceneManager.LoadScene(targetSceneIndex);
            }
        }
        // Mặc định reload scene hiện tại
        else
        {
            DoReloadCurrent();
        }
    }
    
    private void CloseCurrentPopup()
    {
        // Tìm popup chứa button này
        Transform parent = transform;
        
        // Tìm lên các parent để tìm popup
        while (parent != null)
        {
            // Tìm component Popup
            var popup = parent.GetComponent<Ricimi.Popup>();
            if (popup != null)
            {
                popup.Close();
                Debug.Log("Đã đóng popup trước khi reload");
                return;
            }
            
            // Tìm GameObject có tên chứa "Popup" và có thể deactivate
            if (parent.name.Contains("Popup") || parent.name.Contains("popup"))
            {
                parent.gameObject.SetActive(false);
                Debug.Log("Đã ẩn popup GameObject: " + parent.name);
                return;
            }
            
            parent = parent.parent;
        }
        
        // Fallback: Tìm tất cả popup active trong scene
        var allPopups = FindObjectsOfType<Ricimi.Popup>();
        foreach (var popup in allPopups)
        {
            if (popup.gameObject.activeInHierarchy)
            {
                popup.Close();
                Debug.Log("Đã đóng popup fallback: " + popup.name);
                return;
            }
        }
        
        Debug.Log("Không tìm thấy popup để đóng");
    }
    
    #endregion
    
    #region Editor Helpers
    
    /// <summary>
    /// Lấy danh sách scenes trong Build Settings (cho dropdown trong Inspector)
    /// </summary>
    public void SetTargetToCurrentScene()
    {
        targetSceneName = SceneManager.GetActiveScene().name;
        targetSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    
    /// <summary>
    /// Reset về reload scene hiện tại
    /// </summary>
    public void ResetToReloadCurrent()
    {
        targetSceneName = "";
        targetSceneIndex = -1;
    }
    
    #endregion
}
