// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;

namespace Ricimi
{
    // Lớp tiện ích để bắt buộc nhạc và hiệu ứng âm thanh được bật khi khởi chạy lần đầu.
    public class InitialPlayerPrefs : MonoBehaviour
    {
        private void Awake()
        {
            if (!PlayerPrefs.HasKey("music_on"))
                PlayerPrefs.SetInt("music_on", 1);

            if (!PlayerPrefs.HasKey("sound_on"))
                PlayerPrefs.SetInt("sound_on", 1);
        }
    }
}
