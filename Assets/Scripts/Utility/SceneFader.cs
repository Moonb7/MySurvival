using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image faderimage;

    // 자연스러운 페이드 효과
    public AnimationCurve curve;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }
    
    public void InFade()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float a = 1;

        while (a >= 0f)
        {
            a -= Time.deltaTime;
            float cValue = curve.Evaluate(a);
            faderimage.color = new Color(0, 0, 0, cValue);
            yield return 0;
        }
    }

    public void FadeTo(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        float a = 0;
        while (a <= 1f)
        {
            a += Time.deltaTime;
            float cValue = curve.Evaluate(a);
            faderimage.color = new Color(0, 0, 0, cValue);
            yield return 0;
        }
        SceneManager.LoadScene(sceneName);
        
    }
}
