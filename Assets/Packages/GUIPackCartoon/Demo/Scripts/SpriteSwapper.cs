// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
    // Lớp tiện ích để hoán đổi sprite của UI Image giữa hai giá trị đã định trước.
    public class SpriteSwapper : MonoBehaviour
    {
        public Sprite enabledSprite;
        public Sprite disabledSprite;

        private bool m_swapped = true;

        private Image m_image;

        public void Awake()
        {
            m_image = GetComponent<Image>();
        }

        public void SwapSprite()
        {
            if (m_swapped)
            {
                m_swapped = false;
                m_image.sprite = disabledSprite;
            }
            else
            {
                m_swapped = true;
                m_image.sprite = enabledSprite;
            }
        }
    }
}
