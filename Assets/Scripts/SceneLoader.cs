using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad;

    [Header("Loading UI")]
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public TMP_Text loadingText;

    [Header("Progress Smoothing")]
    [Range(0.1f, 5f)]
    public float smoothSpeed = 1.5f;

    [Header("Spawn Settings")]
    public string exitPoint;

    float displayedProgress = 0f;

    void Start()
    {
        if (!string.IsNullOrEmpty(exitPoint))
            GameManager.Instance.SetExit(exitPoint);

        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            displayedProgress = Mathf.Lerp(
                displayedProgress,
                targetProgress,
                Time.deltaTime * smoothSpeed
            );

            loadingSlider.value = displayedProgress;
            loadingText.text = Mathf.RoundToInt(displayedProgress * 100f) + "%";

            if (displayedProgress >= 0.99f && operation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingText.text = "100%";

                // LOGIC: Save progress to Firebase right before entering the new map
                if (FirebaseManager.Instance != null)
                {
                    FirebaseManager.Instance.SaveCurrentProgress();
                }

                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}