using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditController : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField, Min(0.1f)] private float fadeDuration = 1f;
    [Tooltip("Interval between automatic fade-ins (seconds)")]
    [SerializeField, Min(0f)] private float interval = 2f;

    [Header("UI Groups (CanvasGroup)")]
    [SerializeField] private CanvasGroup titleGroup;
    [SerializeField] private CanvasGroup row1Group;
    [SerializeField] private CanvasGroup descriptionGroup;
    [SerializeField] private CanvasGroup thanksGroup;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "LobbyScene";

    private int state = 0; // 0=auto fade, 1=waiting first Enter, 2=fading out/showing thanks, 3=waiting second Enter

    private void Start()
    {
        HideGroup(titleGroup);
        HideGroup(row1Group);
        HideGroup(descriptionGroup);
        HideGroup(thanksGroup);

        StartCoroutine(FadeAllSequence());
    }

    private void Update()
    {
        if (state == 1 && Input.GetKeyDown(KeyCode.Return))
        {
            state = 2;
            StartCoroutine(FadeOutAndShowThanks());
        }
        else if (state == 3 && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private IEnumerator FadeAllSequence()
    {
        yield return FadeInGroup(titleGroup);
        yield return new WaitForSecondsRealtime(interval);
        yield return FadeInGroup(row1Group);
        yield return new WaitForSecondsRealtime(interval);
        yield return FadeInGroup(descriptionGroup);

        // auto fade-ins complete, now wait for first Enter
        state = 1;
    }

    private IEnumerator FadeOutAndShowThanks()
    {
        // fade out row1 and description simultaneously
        float t = 0f;
        CanvasGroup[] groups = new CanvasGroup[] { row1Group, descriptionGroup };
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            foreach (var cg in groups)
                cg.alpha = a;
            yield return null;
        }
        foreach (var cg in groups)
        {
            cg.alpha = 0f;
            cg.interactable = cg.blocksRaycasts = false;
            cg.gameObject.SetActive(false);
        }

        // show thanks message
        yield return FadeInGroup(thanksGroup);

        // now wait for second Enter
        state = 3;
    }

    private IEnumerator FadeInGroup(CanvasGroup cg)
    {
        cg.gameObject.SetActive(true);
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        cg.alpha = 1f;
        cg.interactable = cg.blocksRaycasts = true;
    }

    private void HideGroup(CanvasGroup cg)
    {
        cg.gameObject.SetActive(false);
        cg.alpha = 0f;
        cg.interactable = cg.blocksRaycasts = false;
    }
}
