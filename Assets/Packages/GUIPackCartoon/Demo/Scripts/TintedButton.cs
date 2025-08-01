// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ricimi
{
    // Lớp này dựa trên mã nguồn chính thức cho UI Button của Unity (có thể
    // tìm thấy tại: https://bitbucket.org/Unity-Technologies/ui), nhưng thêm chức năng
    // để tô màu mượt mà cho image liên kết khi được nhấn.
    public class TintedButton : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        private const float m_colorOffset = 0.2f;

        private bool m_pointerInside = false;
        private bool m_pointerPressed = false;

        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_pointerInside = true;
            if (m_pointerPressed)
                Press();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_pointerInside = false;
            if (m_pointerPressed)
                Unpress();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_pointerPressed = false;
            if (m_pointerInside)
            {
                Unpress();
                m_OnClick.Invoke();
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_pointerPressed = true;
            if (m_pointerInside)
                Press();
        }

        private void Press()
        {
            if (!IsActive())
                return;

            Darken();
        }

        private void Unpress()
        {
            if (!IsActive())
                return;

            Brighten();
        }

        private void Darken()
        {
            var images = GetComponentsInChildren<Image>();
            foreach (var image in images)
                StartCoroutine(DarkenColor(image));
        }

        private void Brighten()
        {
            var images = GetComponentsInChildren<Image>();
            foreach (var image in images)
                StartCoroutine(BrightenColor(image));
        }

        private IEnumerator DarkenColor(Image image)
        {
            for (int i = 0; i < 20; i++)
            {
                var newColor = image.color;
                newColor.r -= 0.01f;
                newColor.g -= 0.01f;
                newColor.b -= 0.01f;
                image.color = newColor;
                yield return new WaitForSeconds(0.01f);
            }
        }

        private IEnumerator BrightenColor(Image image)
        {
            for (int i = 0; i < 20; i++)
            {
                var newColor = image.color;
                newColor.r += 0.01f;
                newColor.g += 0.01f;
                newColor.b += 0.01f;
                image.color = newColor;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
