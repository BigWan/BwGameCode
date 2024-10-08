﻿using System;
using System.Collections;

using UnityEngine;

namespace BW.GameCode.Foundation
{
    public delegate T EaseFunc<T>(T a, T b, float t);

    public class SimpleTween_Float : SimpleTween<float>
    {
        public SimpleTween_Float() : base(Mathf.Lerp) {
        }
    }

    public class SimpleTween_V2 : SimpleTween<Vector2>
    {
        public SimpleTween_V2() : base(Vector2.Lerp) {
        }
    }

    public class SimpleTween_V3 : SimpleTween<Vector3>
    {
        public SimpleTween_V3() : base(Vector3.Lerp) {
        }
    }

    public class SimpleTween<T> where T : struct
    {
        IEnumerator tweenInstance;

         Action<T> OnValueUpdated;

        public MonoBehaviour Host { get; private set; }
        public float Duration { get; private set; } = 1f;
        public bool IgnoreTimeScale { get; private set; } = false;
        public T StartValue { get; set; } = default;
        public T EndValue { get; set; } = default(T);

        EaseFunc<T> mLerpFunc;
        Action OnDone;
        public SimpleTween(EaseFunc<T> lerp) {
            this.mLerpFunc = lerp;
        }

        public SimpleTween<T> SetLerp(EaseFunc<T> lerp) {
            mLerpFunc = lerp;
            return this;
        }

        public SimpleTween<T> SetDuration(float duration) {
            Duration = duration;
            return this;
        }

        public SimpleTween<T> SetStartAndEnd(T start, T end) {
            return SetStart(start).SetEnd(end);
        }

        public SimpleTween<T> SetStart(T value) {
            StartValue = value;
            return this;
        }
        public SimpleTween<T> SetEnd(T value) {
            EndValue = value;
            return this;
        }

        public SimpleTween<T> SetIgnoreTimeScale(bool value) {
            IgnoreTimeScale = value;
            return this;
        }

        public SimpleTween<T> SetUpdateCall(Action<T> callback) {
            OnValueUpdated = callback;
            return this;
        }

        public SimpleTween<T> SetDoneCall(Action callback) {
            OnDone = callback;
            return this;
        }

        public Coroutine StartTween(MonoBehaviour host) {
            this.Host = host;
            if (Host == null) {
                return null;
            }
            StopTween();
            tweenInstance = SpawnIEnumerator();
            return Host.StartCoroutine(tweenInstance);
        }

        public void StopTween() {
            if (tweenInstance != null && Host!=null) {
                Host.StopCoroutine(tweenInstance);
                tweenInstance = null;
            }
        }

        void TweenValueAndRaiseEvent(float progress) {
            Debug.Assert(mLerpFunc != null, this.Host.transform);
            var value = mLerpFunc(StartValue, EndValue, progress);
            OnValueUpdated?.Invoke(value);
        }

        IEnumerator SpawnIEnumerator() {
            float elapsedTime = 0f;
            while (elapsedTime < Duration) {
                elapsedTime += (IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
                float floatPercentage = Mathf.Clamp01(elapsedTime / Duration);
                TweenValueAndRaiseEvent(floatPercentage);
                yield return null;
            }

            TweenValueAndRaiseEvent(1f);
            OnDone?.Invoke();
        }
    }
}