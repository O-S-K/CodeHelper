using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace OSK
{
    public class FillText : MonoBehaviour
    {
        public float timeFill = 1;
        public float delayFill = 0;

        [Space]
        public bool isScaleToFill = true;
        public float scaleTextFill = 1.25f;
        // public TweenEasings.Easings Easings;
        
        public UnityEvent onStartFill;

 
        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void FillFloatText(Text textFill, float current, float target, float delay = 0, float timeFill = 1,
            Action onCompleted = null)
        {
            this.timeFill = timeFill;
            this.delayFill = delay;
            StartCoroutine(FillTextTo(textFill,current, target, delay, onCompleted));
        }
        
        public void FillFloatText(TextMeshProUGUI textFill, float current, float target, float delay = 0, float timeFill = 1,
            Action onCompleted = null)
        {
            this.timeFill = timeFill;
            this.delayFill = delay;
            StartCoroutine(FillTextTo(textFill,current, target, delay, onCompleted));
        }


        private IEnumerator FillTextTo(TextMeshProUGUI textFill,float current, float target, float delay = 0, Action onCompleted = null)
        {
            yield return new WaitForSeconds(delay);

            onStartFill?.Invoke();

            if (isScaleToFill)
                Tweener.Instance.ScaleTo(textFill.transform, Vector3.one * scaleTextFill, timeFill, delay,
                    TweenEasings.GetEasing(TweenEasings.Easings.BounceEaseOut));

            float elapsedTime = 0f;
            while (elapsedTime < timeFill)
            {
                int value = Mathf.RoundToInt(Mathf.Lerp(current, target, elapsedTime / timeFill));

                if (value <= 0)  value = 0;
                textFill.text = value.ToString();

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (isScaleToFill)
                Tweener.Instance.ScaleTo(textFill.transform, Vector3.one, .1f, 0f,
                    TweenEasings.GetEasing(TweenEasings.Easings.BounceEaseIn));
            
            textFill.text = target.ToString();
            onCompleted?.Invoke();
        }
        

        private IEnumerator FillTextTo(Text textFill,float current, float target, float delay = 0, Action onCompleted = null)
        {
            yield return new WaitForSeconds(delay);

            onStartFill?.Invoke();

            if (isScaleToFill)
                Tweener.Instance.ScaleTo(textFill.transform, Vector3.one * scaleTextFill, timeFill, delay,
                    TweenEasings.GetEasing(TweenEasings.Easings.BounceEaseOut));

            float elapsedTime = 0f;
            while (elapsedTime < timeFill)
            {
                int value = Mathf.RoundToInt(Mathf.Lerp(current, target, elapsedTime / timeFill));
                if (value <= 0)  value = 0;

                textFill.text = value.ToString();

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (isScaleToFill)
                Tweener.Instance.ScaleTo(textFill.transform, Vector3.one, .1f, 0f,
                    TweenEasings.GetEasing(TweenEasings.Easings.BounceEaseIn));
            
            textFill.text = target.ToString();
            onCompleted?.Invoke();
        }
        
    }
}