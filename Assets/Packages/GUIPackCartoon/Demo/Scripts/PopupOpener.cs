// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using UnityEngine;

namespace Ricimi
{
    // Lớp này chịu trách nhiệm tạo và mở popup của prefab đã cho và thêm
    // nó vào canvas UI của scene hiện tại.
    public class PopupOpener : MonoBehaviour
    {
        public GameObject popupPrefab;

        protected Canvas m_canvas;

        protected void Start()
        {
            m_canvas = GetComponentInParent<Canvas>();
        }

        public virtual void OpenPopup()
        {
            var popup = Instantiate(popupPrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.localScale = Vector3.zero;
            popup.transform.SetParent(m_canvas.transform, false);
            popup.GetComponent<Popup>().Open();
        }
    }
}