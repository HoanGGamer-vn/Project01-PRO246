// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Ricimi
{
    // Lớp này dựa trên mã nguồn chính thức cho UI Button của Unity (có thể
    // tìm thấy tại: https://bitbucket.org/Unity-Technologies/ui), nhưng thêm độ trễ trước khi
    // gọi sự kiện on-clicked của button. Lý do làm điều này là vì
    // các button demo chủ yếu được sử dụng để mở popup hoặc kích hoạt chuyển đổi sang scene mới,
    // và nó mang lại cảm giác thị giác đẹp hơn khi đợi animation button được phát
    // một chút trước khi thực hiện những hành động đó (thay vì làm gián đoạn animation đó).
    public class AnimatedButton : UIBehaviour, IPointerDownHandler
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        public bool interactable = true;

        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        private Animator m_animator;

        override protected void Start()
        {
            base.Start();
            m_animator = GetComponent<Animator>();
        }

        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !interactable)
                return;

            Press();
        }

        private void Press()
        {
            if (!IsActive())
                return;

            m_animator.SetTrigger("Pressed");
            Invoke("InvokeOnClickAction", 0.1f);
        }

        private void InvokeOnClickAction()
        {
            m_OnClick.Invoke();
        }
    }
}
