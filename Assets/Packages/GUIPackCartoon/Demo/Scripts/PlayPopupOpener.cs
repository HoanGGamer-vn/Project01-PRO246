// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;

namespace Ricimi
{
    // Phiên bản chuyên biệt của lớp PopupOpener để mở popup PlayPopup
    // và thiết lập số sao phù hợp (có thể được cấu hình từ trong editor).
    public class PlayPopupOpener : PopupOpener
    {
        public int starsObtained = 0;

        public override void OpenPopup()
        {
            var popup = Instantiate(popupPrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.localScale = Vector3.zero;
            popup.transform.SetParent(m_canvas.transform, false);

            var playPopup = popup.GetComponent<PlayPopup>();
            playPopup.Open();
            playPopup.SetAchievedStars(starsObtained);
        }
    }
}
