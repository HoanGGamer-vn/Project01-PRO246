// Bản quyền (C) 2015 ricimi - Bảo lưu mọi quyền.
// Mã này chỉ có thể được sử dụng theo Thỏa thuận Cấp phép Người dùng Cuối của Unity Asset Store.
// Bản sao EULA của Asset Store có sẵn tại http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;

namespace Ricimi
{
    // Lớp tiện ích để làm mờ dần mượt mà một UI CanvasGroup.
    public class Fader : MonoBehaviour
    {
        public float duration = 0.5f;

        private CanvasGroup m_canvasGroup;

        private void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeIn()
        {
            StartCoroutine(RunFadeIn());
        }

        public void FadeOut()
        {
            StartCoroutine(RunFadeOut());
        }

        private IEnumerator RunFadeIn()
        {
            var time = 0.0f;
            var initialAlpha = m_canvasGroup.alpha;

            while (time < duration)
            {
                time += Time.deltaTime;
                m_canvasGroup.alpha = Mathf.Lerp(initialAlpha, 1.0f, time / duration);
                yield return new WaitForEndOfFrame();
            }
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
        }

        private IEnumerator RunFadeOut()
        {
            var time = 0.0f;
            var initialAlpha = m_canvasGroup.alpha;

            while (time < duration)
            {
                time += Time.deltaTime;
                m_canvasGroup.alpha = Mathf.Lerp(initialAlpha, 0.0f, time / duration);
                yield return new WaitForEndOfFrame();
            }
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
        }
    }
}
