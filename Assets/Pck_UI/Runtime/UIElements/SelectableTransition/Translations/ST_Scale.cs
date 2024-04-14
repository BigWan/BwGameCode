﻿namespace BW.GameCode.UI
{

    using UnityEngine;

    using static BW.GameCode.UI.SelectableAnimationController;
    using BW.GameCode.Core;

    /// <summary>
    /// 缩放
    /// </summary>
    public class ST_Scale : SelectableTransition
    {
        [SerializeField] Transform m_scalePart = default;
        [SerializeField] STValue_Float m_value;
        //[SerializeField] float m_selectScale = 1.1f;

        [SerializeField] float m_animTime = 0.25f;

        SimpleTween<float> runner = new SimpleTween<float>();

        private void Awake() {
            runner.SetCallback((x) => {
                if (m_scalePart != null) {
                    m_scalePart.localScale= Vector3.one*x;
                }
            })
            .SetDuration(m_animTime)
            .SetLerp(Mathf.Lerp);
        }
        internal override void DoStateTransition(SelectableState state, bool instant) {
           if(m_scalePart!=null && m_value != null) {
                DOScale(m_value.GetValue(state), instant);
            }
        }

        private void DOScale(float value, bool instant) {
            //m_scalePart.DOKill();
            if (instant) {
                m_scalePart.localScale = value * Vector3.one;
            } else {
                runner.SetStartAndEnd(m_scalePart.localScale.x, value).StartTween(this);
            }
        }



        private void OnValidate() {
            if (m_scalePart == null) {
                m_scalePart = this.transform;
            }
        }
    }
}