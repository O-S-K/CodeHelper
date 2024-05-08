using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FillText : MonoBehaviour
{
    public Text textFill;
    public float timeFill = 1;
    public float delayFill = 0;
    
    // [Space]
    // public bool animFillScale = true;
    // public float timeAnimFillScale = 1;
    // public TweenEasings.Easings Easings;

    private float target;

    private void OnEnable()
    {
        textFill = gameObject.GetOrAddComponent<Text>();
       StartCoroutine(FillTextTo(0, 1000, delayFill, () => { Debug.Log("Fill Text Completed!"); }) );
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        textFill.text = target.ToString();
    }

    public void FillFloatText(float current, float target, float delay = 0, float timeFill = 1,
        Action onCompleted = null)
    {
        this.timeFill = timeFill;
        this.delayFill = delay;
        StartCoroutine(FillTextTo(current, target, delay, onCompleted));
    }

    private IEnumerator FillTextTo(float current, float target, float delay = 0, Action onCompleted = null)
    {
        this.target = target;
        yield return new WaitForSeconds(delay);

        // if (animFillScale)
        //     Tweener.Instance.ScaleTo(textFill.transform, Vector3.one * 1.2f , timeAnimFillScale, 0f, TweenEasings.GetEasing(Easings));
        
        float elapsedTime = 0f;
        while (elapsedTime < timeFill)
        {
            int value = Mathf.RoundToInt(Mathf.Lerp(current, target, elapsedTime / timeFill));
            textFill.text = value.ToString();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textFill.text = target.ToString();
        onCompleted?.Invoke();
    }
}