// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;

namespace Ricimi
{
    // Lớp này quản lý việc xoay của vòng quay ví dụ trong demo.
    public class SpinWheel : MonoBehaviour
    {
        // Đường cong animation này điều khiển chuyển động của vòng quay.
        public AnimationCurve AnimationCurve;

        private bool m_spinning = false;

        public void Spin()
        {
            if (!m_spinning)
                StartCoroutine(DoSpin());
        }

        private IEnumerator DoSpin()
        {
            m_spinning = true;
            var timer = 0.0f;
            var startAngle = transform.eulerAngles.z;

            var time = 3.0f;
            var maxAngle = 270.0f;

            while (timer < time)
            {
                var angle = AnimationCurve.Evaluate(timer / time) * maxAngle;
                transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
            m_spinning = false;
        }
    }
}
