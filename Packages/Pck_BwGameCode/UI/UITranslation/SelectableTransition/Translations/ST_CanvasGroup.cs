﻿
using UnityEngine;
using System.Collections;
namespace BW.GameCode.UI
{
    using static BW.GameCode.UI.SelectableAnimationController;
    using BW.GameCode.Foundation;

    /// <summary>
    /// 高亮一个Cg
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class ST_CanvasGroup : SelectableTransition
    {
        [SerializeField] CanvasGroup m_canvasGroup = default;
        [SerializeField] STValue_Float m_value;
        [SerializeField] float m_animTime = 0.15f;

        SimpleTween_Float runner = new SimpleTween_Float();

        private void Awake() {
            runner.SetUpdateCall((x) => {
                if (m_canvasGroup != null) {
                    m_canvasGroup.alpha = x;
                }
            })
            
            .SetLerp(Mathf.Lerp);
        }

        internal override void DoStateTransition(SelectableState state, bool instant) {
            if (m_value != null && m_canvasGroup != null) {
                Fade(m_value.GetValue(state), instant);
            }
        }

        void Fade(float value, bool instant) {
            if (m_canvasGroup == null) {
                return;
            }

            if (instant) {
                m_canvasGroup.alpha = value;
            } else {
                runner.SetStartAndEnd(m_canvasGroup.alpha, value).SetDuration(m_animTime)
                    .StartTween(this);
            }
        }
    }
}