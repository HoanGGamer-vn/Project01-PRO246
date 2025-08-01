// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
    // Lớp này xử lý việc cập nhật các widget UI âm thanh tùy thuộc vào lựa chọn của người chơi.
    public class SoundManager : MonoBehaviour
    {
        private Slider m_soundSlider;
        private GameObject m_soundButton;

        private void Start()
        {
            m_soundSlider = GetComponent<Slider>();
            m_soundSlider.value = PlayerPrefs.GetInt("sound_on");
            m_soundButton = GameObject.Find("SoundButton/Button");
        }

        public void SwitchSound()
        {
            AudioListener.volume = m_soundSlider.value;
            PlayerPrefs.SetInt("sound_on", (int)m_soundSlider.value);
            if (m_soundButton != null)
            {
                m_soundButton.GetComponent<SoundButton>().ToggleSprite();
            }
        }
    }
}
