using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ricimi;

public class SceneReloader : MonoBehaviour
{
    [Header("Popup Settings (Tùy chọn)")]
    [Tooltip("Nếu muốn hiện popup xác nhận trước khi reload")]
    public PopupOpener confirmPopupOpener;
    
    [Header("Reload Settings")]
    [Tooltip("Thời gian delay trước khi reload (giây)")]
    public float reloadDelay = 0f;
    
    [Tooltip("Có fade effect khi reload không")]
    public bool useFadeEffect = false;
    
    [Tooltip("Màu fade (nếu sử dụng fade effect)")]
    public Color fadeColor = Color.black;
    
    [Tooltip("Thời gian fade (giây)")]
    public float fadeDuration = 1f;
    
    // Private variables
    private bool isReloading = false;
    private Canvas fadeCanvas;
    private UnityEngine.UI.Image fadeImage;
    
    // Static instance để có thể gọi từ bất kỳ đâu
    public static SceneReloader Instance { get; private set; }
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        // Tạo fade canvas nếu cần
        if (useFadeEffect)
        {
            CreateFadeCanvas();
        }
    }
    
    #region Public Methods - Các method để gọi từ UI Button
    
    /// <summary>
    /// Reload scene ngay lập tức (không popup xác nhận)
    /// </summary>
    public void ReloadSceneInstant()
    {
        if (isReloading) return;
        
        Debug.Log("Reload scene ngay lập tức");
        StartCoroutine(ReloadCoroutine());
    }
    
    /// <summary>
    /// Hiện popup xác nhận trước khi reload (nếu có setup popup)
    /// </summary>
    public void ShowReloadConfirmation()
    {
        if (isReloading) return;
        
        if (confirmPopupOpener != null)
        {
            Debug.Log("Hiện popup xác nhận reload");
            confirmPopupOpener.OpenPopup();
        }
        else
        {
            // Nếu không có popup thì reload luôn
            Debug.Log("Không có popup xác nhận, reload ngay");
            ReloadSceneInstant();
        }
    }
    
    /// <summary>
    /// Xác nhận reload từ popup (gọi từ button "Yes" trong popup)
    /// </summary>
    public void ConfirmReload()
    {
        Debug.Log("Xác nhận reload scene");
        StartCoroutine(ReloadCoroutine());
    }
    
    /// <summary>
    /// Hủy reload từ popup (gọi từ button "No" trong popup)
    /// </summary>
    public void CancelReload()
    {
        Debug.Log("Hủy reload scene");
        // Popup sẽ tự đóng, không cần làm gì thêm
    }
    
    /// <summary>
    /// Reload scene cụ thể theo tên
    /// </summary>
    public void ReloadSpecificScene(string sceneName)
    {
        if (isReloading) return;
        
        Debug.Log("Reload scene: " + sceneName);
        StartCoroutine(ReloadSpecificSceneCoroutine(sceneName));
    }
    
    /// <summary>
    /// Reload scene cụ thể theo build index
    /// </summary>
    public void ReloadSpecificScene(int sceneIndex)
    {
        if (isReloading) return;
        
        Debug.Log("Reload scene index: " + sceneIndex);
        StartCoroutine(ReloadSpecificSceneCoroutine(sceneIndex));
    }
    
    #endregion
    
    #region Private Methods
    
    private IEnumerator ReloadCoroutine()
    {
        if (isReloading) yield break;
        
        isReloading = true;
        
        // Fade out nếu có
        if (useFadeEffect && fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }
        
        // Delay nếu có
        if (reloadDelay > 0)
        {
            yield return new WaitForSeconds(reloadDelay);
        }
        
        // Reload scene hiện tại
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Đang reload scene: " + currentSceneName);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private IEnumerator ReloadSpecificSceneCoroutine(string sceneName)
    {
        if (isReloading) yield break;
        
        isReloading = true;
        
        // Fade out nếu có
        if (useFadeEffect && fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }
        
        // Delay nếu có
        if (reloadDelay > 0)
        {
            yield return new WaitForSeconds(reloadDelay);
        }
        
        // Load scene cụ thể
        Debug.Log("Đang load scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    
    private IEnumerator ReloadSpecificSceneCoroutine(int sceneIndex)
    {
        if (isReloading) yield break;
        
        isReloading = true;
        
        // Fade out nếu có
        if (useFadeEffect && fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }
        
        // Delay nếu có
        if (reloadDelay > 0)
        {
            yield return new WaitForSeconds(reloadDelay);
        }
        
        // Load scene cụ thể
        Debug.Log("Đang load scene index: " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
    
    private void CreateFadeCanvas()
    {
        // Tạo Canvas cho fade effect
        GameObject fadeCanvasObj = new GameObject("FadeCanvas");
        fadeCanvas = fadeCanvasObj.AddComponent<Canvas>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 9999; // Đảm bảo hiện trên cùng
        
        // Thêm CanvasScaler
        var scaler = fadeCanvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Thêm GraphicRaycaster
        fadeCanvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // Tạo Image cho fade effect
        GameObject fadeImageObj = new GameObject("FadeImage");
        fadeImageObj.transform.SetParent(fadeCanvas.transform, false);
        
        fadeImage = fadeImageObj.AddComponent<UnityEngine.UI.Image>();
        fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f); // Bắt đầu trong suốt
        
        // Đặt kích thước full screen
        RectTransform rect = fadeImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        // Ẩn canvas ban đầu
        fadeCanvas.gameObject.SetActive(false);
        
        // Đảm bảo không bị destroy khi load scene
        DontDestroyOnLoad(fadeCanvas.gameObject);
        
        Debug.Log("Đã tạo Fade Canvas cho scene reload");
    }
    
    private IEnumerator FadeOut()
    {
        if (fadeCanvas == null || fadeImage == null) yield break;
        
        // Hiện canvas
        fadeCanvas.gameObject.SetActive(true);
        
        // Fade từ trong suốt sang đục
        float elapsedTime = 0f;
        Color startColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        Color endColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Dùng unscaledDeltaTime để không bị ảnh hưởng bởi timeScale
            float progress = elapsedTime / fadeDuration;
            fadeImage.color = Color.Lerp(startColor, endColor, progress);
            yield return null;
        }
        
        fadeImage.color = endColor;
    }
    
    #endregion
    
    #region Static Methods - Gọi từ Popup Buttons
    
    /// <summary>
    /// Static method để reload scene từ popup button
    /// </summary>
    public static void ReloadCurrentScene()
    {
        if (Instance != null)
        {
            Instance.ReloadSceneInstant();
        }
        else
        {
            Debug.LogWarning("SceneReloader Instance không tồn tại! Reload bằng cách thông thường.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    /// <summary>
    /// Static method để load scene cụ thể từ popup button
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        if (Instance != null)
        {
            Instance.ReloadSpecificScene(sceneName);
        }
        else
        {
            Debug.LogWarning("SceneReloader Instance không tồn tại! Load bằng cách thông thường.");
            SceneManager.LoadScene(sceneName);
        }
    }
    
    /// <summary>
    /// Static method để load scene theo index từ popup button
    /// </summary>
    public static void LoadScene(int sceneIndex)
    {
        if (Instance != null)
        {
            Instance.ReloadSpecificScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning("SceneReloader Instance không tồn tại! Load bằng cách thông thường.");
            SceneManager.LoadScene(sceneIndex);
        }
    }
    
    #endregion
    
    #region Utility Methods
    
    /// <summary>
    /// Kiểm tra có đang trong quá trình reload không
    /// </summary>
    public bool IsReloading()
    {
        return isReloading;
    }
    
    /// <summary>
    /// Lấy tên scene hiện tại
    /// </summary>
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    
    /// <summary>
    /// Lấy build index của scene hiện tại
    /// </summary>
    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    
    #endregion
    
    void OnDestroy()
    {
        // Dọn dẹp fade canvas nếu có
        if (fadeCanvas != null && fadeCanvas.gameObject != null)
        {
            Destroy(fadeCanvas.gameObject);
        }
    }
}
