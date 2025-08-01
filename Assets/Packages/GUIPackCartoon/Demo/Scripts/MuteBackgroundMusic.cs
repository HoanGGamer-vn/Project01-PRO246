// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;

namespace Ricimi
{
    // Lớp này chịu trách nhiệm làm mờ dần nhạc nền đang phát hiện tại.
    public class MuteBackgroundMusic : MonoBehaviour
    {
        private BackgroundMusic m_bgMusic;

        private void Awake()
        {
            var backgroundMusic = GameObject.Find("BackgroundMusic");
            if (backgroundMusic != null)
            {
                m_bgMusic = backgroundMusic.GetComponent<BackgroundMusic>();
                if (m_bgMusic != null)
                    m_bgMusic.FadeOut();
            }
        }

        private void OnDestroy()
        {
            if (m_bgMusic != null)
                m_bgMusic.FadeIn();
        }
    }
}
