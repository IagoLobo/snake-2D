using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public Image blackBackground;
    private float fadeAlphaCounter = 1f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        // Alpha needs to be zero for us to read the text, so keep changing alpha
        if(blackBackground.color.a > 0)
        {
            fadeAlphaCounter -= 0.01f;
            blackBackground.color = new Color(blackBackground.color.r, blackBackground.color.g, blackBackground.color.b, fadeAlphaCounter);

            yield return new WaitForSeconds(0.01f);

            StartCoroutine(FadeIn());
        }
        else
        {
            StartCoroutine(KeepMessageOnScreen());
        }
    }

    public IEnumerator KeepMessageOnScreen()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.01f);

        // Alpha needs to be zero for us to read the text, so keep changing alpha
        if(blackBackground.color.a < 1f)
        {
            fadeAlphaCounter += 0.01f;
            blackBackground.color = new Color(blackBackground.color.r, blackBackground.color.g, blackBackground.color.b, fadeAlphaCounter);

            StartCoroutine(FadeOut());
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
