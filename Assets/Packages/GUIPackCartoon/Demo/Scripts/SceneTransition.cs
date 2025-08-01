// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;

namespace Ricimi
{
    // Lớp này chịu trách nhiệm tải scene tiếp theo trong một chuyển đổi (phần cốt lõi của
    // công việc này được thực hiện trong lớp Transition).
    public class SceneTransition : MonoBehaviour
    {
        public string scene = "<Insert scene name>";
        public float duration = 1.0f;
        public Color color = Color.black;

        public void PerformTransition()
        {
            Transition.LoadLevel(scene, duration, color);
        }
    }
}
