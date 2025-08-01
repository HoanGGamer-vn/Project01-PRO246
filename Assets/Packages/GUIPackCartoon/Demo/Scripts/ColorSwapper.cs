// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
    // Lớp tiện ích để hoán đổi màu của UI Image giữa hai giá trị đã định trước.
    public class ColorSwapper : MonoBehaviour
    {
        public Color enabledColor;
        public Color disabledColor;

        private bool m_swapped = true;

        private Image m_image;

        private void Awake()
        {
            m_image = GetComponent<Image>();
        }

        public void SwapColor()
        {
            if (m_swapped)
            {
                m_swapped = false;
                m_image.color = disabledColor;
            }
            else
            {
                m_swapped = true;
                m_image.color = enabledColor;
            }
        }
    }
}
