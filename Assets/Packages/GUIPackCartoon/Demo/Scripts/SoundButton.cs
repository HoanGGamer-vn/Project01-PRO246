// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;

namespace Ricimi
{
    // Lớp này đại diện cho button âm thanh được sử dụng ở nhiều nơi trong demo.
    // Nó xử lý logic để bật và tắt âm thanh của demo và lưu trữ lựa chọn
    // của người chơi vào PlayerPrefs.
    public class SoundButton : MonoBehaviour
    {
        private SpriteSwapper m_spriteSwapper;
        private bool m_on;

        private void Start()
        {
            m_spriteSwapper = GetComponent<SpriteSwapper>();
            m_on = PlayerPrefs.GetInt("sound_on") == 1;
            if (!m_on)
                m_spriteSwapper.SwapSprite();
        }

        public void Toggle()
        {
            m_on = !m_on;
            AudioListener.volume = m_on ? 1 : 0;
            PlayerPrefs.SetInt("sound_on", m_on ? 1 : 0);
        }

        public void ToggleSprite()
        {
            m_on = !m_on;
            m_spriteSwapper.SwapSprite();
        }
    }
}
