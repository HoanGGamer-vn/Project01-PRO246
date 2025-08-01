// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
    // Lớp này chịu trách nhiệm quản lý popup. Popup tuân theo hành vi truyền thống
    // tự động chặn input trên các phần tử phía sau nó và thêm texture nền.
    public class Popup : MonoBehaviour
    {
        public Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);

        private GameObject m_background;

        public void Open()
        {
            AddBackground();
        }

        public void Close()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                animator.Play("Close");

            RemoveBackground();
            StartCoroutine(RunPopupDestroy());
        }

        // Chúng ta tự động phá hủy popup sau 0.5 giây khi đóng nó.
        // Việc phá hủy được thực hiện bất đồng bộ thông qua một coroutine. Nếu bạn
        // muốn phá hủy popup đúng thời điểm animation đóng kết thúc,
        // bạn có thể sử dụng animation event thay thế.
        private IEnumerator RunPopupDestroy()
        {
            yield return new WaitForSeconds(0.5f);
            if (m_background != null)
                Destroy(m_background);
            if (gameObject != null)
                Destroy(gameObject);
        }

        private void AddBackground()
        {
            var bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, backgroundColor);
            bgTex.Apply();

            m_background = new GameObject("PopupBackground");
            var image = m_background.AddComponent<Image>();
            var rect = new Rect(0, 0, bgTex.width, bgTex.height);
            var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
            image.material.mainTexture = bgTex;
            image.sprite = sprite;
            var newColor = image.color;
            image.color = newColor;
            image.canvasRenderer.SetAlpha(0.0f);
            image.CrossFadeAlpha(1.0f, 0.4f, false);

            var canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                // Nếu không tìm thấy Canvas, tìm Canvas đầu tiên trong scene
                canvas = FindObjectOfType<Canvas>()?.gameObject;
            }
            
            if (canvas != null)
            {
                m_background.transform.localScale = new Vector3(1, 1, 1);
                m_background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
                m_background.transform.SetParent(canvas.transform, false);
                m_background.transform.SetSiblingIndex(transform.GetSiblingIndex());
            }
            else
            {
                Debug.LogError("Không tìm thấy Canvas trong scene! Popup không thể hiển thị đúng cách.");
                // Đặt background làm con của transform hiện tại làm fallback
                m_background.transform.SetParent(transform.parent, false);
            }
        }

        private void RemoveBackground()
        {
            if (m_background != null)
            {
                var image = m_background.GetComponent<Image>();
                if (image != null)
                    image.CrossFadeAlpha(0.0f, 0.2f, false);
            }
        }
    }
}
