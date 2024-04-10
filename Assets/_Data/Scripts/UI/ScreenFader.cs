using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    private Image fadeOutUIImage;
    public enum FadeDirection
    {
        In,
        Out
    }
    private void Start()
    {
        fadeOutUIImage = GetComponent<Image>();
    }
    public IEnumerator Fade(FadeDirection fadeDirection)
    {
        yield return new WaitForEndOfFrame();
        float alpha = fadeDirection == FadeDirection.Out ? 1 : 0;
        float fadeEndValue = fadeDirection == FadeDirection.Out ? 0 : 1;

        if (fadeDirection == FadeDirection.Out)
        {
            while (alpha >= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
            fadeOutUIImage.enabled = false;
        }
        else
        {
            fadeOutUIImage.enabled = true;
            while (alpha <= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }

        }
    }

    public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection, string sceneToLoad)
    {
        fadeOutUIImage.enabled = true;
        yield return Fade(fadeDirection);
        SceneManager.LoadScene(sceneToLoad);
    }

    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
        alpha += Time.deltaTime * (1 / fadeTime) * (fadeDirection == FadeDirection.Out ? -1 : 1);
    }
}
