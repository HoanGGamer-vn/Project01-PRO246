// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
    // Hành vi chuyên biệt cho popup mở trước khi chọn level để chơi trong
    // demo. Nó thể hiện cách tạo popup chuyên biệt với hành vi tùy chỉnh: trong trường hợp này,
    // một đến ba sao có thể được hiển thị tùy thuộc vào điểm số của người chơi trên level đó.
    public class PlayPopup : Popup
    {
        public Color enabledColor;
        public Color disabledColor;

        public Image leftStarImage;
        public Image middleStarImage;
        public Image rightStarImage;

        public void SetAchievedStars(int starsObtained)
        {
            if (starsObtained == 0)
            {
                leftStarImage.color = disabledColor;
                middleStarImage.color = disabledColor;
                rightStarImage.color = disabledColor;
            }
            else if (starsObtained == 1)
            {
                leftStarImage.color = enabledColor;
                middleStarImage.color = disabledColor;
                rightStarImage.color = disabledColor;
            }
            else if (starsObtained == 2)
            {
                leftStarImage.color = enabledColor;
                middleStarImage.color = enabledColor;
                rightStarImage.color = disabledColor;
            }
            else if (starsObtained == 3)
            {
                leftStarImage.color = enabledColor;
                middleStarImage.color = enabledColor;
                rightStarImage.color = enabledColor;
            }
        }
    }
}
