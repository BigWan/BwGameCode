﻿
using UnityEngine;
using UnityEngine.UI;

namespace BW.GameCode.UI
{
    public sealed class ToggleTranslation_GraphicColor : AbstractToggleTranslation
    {
        [SerializeField] Graphic m_targetGraphic;
        [SerializeField] Color m_colorOn;
        [SerializeField] Color m_colorOff;
        [SerializeField] float m_duration = 0.2f;

        public override void OnToggleChanged(bool isOn) {
            m_targetGraphic.CrossFadeColor(isOn ? m_colorOn : m_colorOff, m_duration, false, false);
        }
    
    }
}