using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        [TextArea(2, 4)]
        public string questionText;
        public string[] choices = new string[4];
        public int correctAnswerIndex;
    }

    [Header("Questions")]
    public Question[] questions;

    [Header("Tutorial Questions")]
    public Question[] tutorialQuestions;

    [Header("UI")]
    public GameObject quizPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;

    [Header("Guide UI")]
    public GameObject guidePanel;
    private bool guideShown;

    [Header("Tutorial UI")]
    public GameObject tutorialIntroPanel;
    public TMP_Text tutorialIntroText;
    public GameObject tutorialEndPanel;
    public TMP_Text tutorialEndText;
    public TMP_Text tutorialFeedbackText;
    public GameObject mouseGuide;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    [Header("Exit Message UI")]
    public GameObject exitMessagePanel;
    private CanvasGroup exitMessageCanvasGroup;

    private int currentQuestionIndex;
    private bool isTutorial = false;
    private bool showMouseGuide = false;

    void Start()
    {
        guideShown = PlayerPrefs.GetInt("GuideShown", 0) == 1;

        if (quizPanel != null) quizPanel.SetActive(false);
        if (guidePanel != null) guidePanel.SetActive(false);
        if (tutorialIntroPanel != null) tutorialIntroPanel.SetActive(false);
        if (tutorialEndPanel != null) tutorialEndPanel.SetActive(false);
        if (mouseGuide != null) mouseGuide.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (exitMessagePanel != null)
        {
            exitMessageCanvasGroup = exitMessagePanel.GetComponent<CanvasGroup>();
            if (exitMessageCanvasGroup != null)
                exitMessageCanvasGroup.alpha = 0f;
            exitMessagePanel.SetActive(false);
        }

        // SAFETY: reset NanayQuizDone flag if needed
        if (PlayerPrefs.GetInt("QuestProgress", 0) > 2)
        {
            PlayerPrefs.SetInt("QuestProgress", 0);
            PlayerPrefs.SetInt("NanayQuizDone", 0);
            PlayerPrefs.Save();
        }
    }

    // ==============================
    // Normal Quiz
    // ==============================
    public void TriggerQuizWithGuide()
    {
        Debug.Log("TriggerQuizWithGuide CALLED");

        if (!guideShown && guidePanel != null) guidePanel.SetActive(true);
        else StartQuiz();
    }

    public void CloseGuide()
    {
        guideShown = true;
        PlayerPrefs.SetInt("GuideShown", 1);
        PlayerPrefs.Save();
        if (guidePanel != null) guidePanel.SetActive(false);
        StartQuiz();
    }

    public void StartQuiz()
    {
        if (!guideShown && guidePanel != null)
        {
            guidePanel.SetActive(true);
            return;
        }
        quizPanel.SetActive(true);
        currentQuestionIndex = 0;
        isTutorial = false;
        ShowQuestion();
    }

    // ==============================
    // Tutorial Quiz (Tatay)
    // ==============================
    public void TriggerTutorialQuiz()
    {
        isTutorial = true;
        currentQuestionIndex = 0;
        if (quizPanel != null) quizPanel.SetActive(true);
        if (tutorialIntroPanel != null) tutorialIntroPanel.SetActive(true);
    }

    public void StartTutorial()
    {
        if (tutorialIntroPanel != null) tutorialIntroPanel.SetActive(false);
        showMouseGuide = true;
        ShowQuestion();
    }

    public void CloseTutorial()
    {
        if (tutorialEndPanel != null) tutorialEndPanel.SetActive(false);
        if (tutorialFeedbackText != null) tutorialFeedbackText.text = "";
        isTutorial = false;

        PlayerPrefs.SetInt("QuestProgress", 1); // Tutorial done
        PlayerPrefs.Save();
    }

    void ShowQuestion()
    {
        Question q = isTutorial ? tutorialQuestions[currentQuestionIndex] : questions[currentQuestionIndex];

        questionText.text = q.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = q.choices[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => SelectAnswer(index));

            if (isTutorial && showMouseGuide && i == q.correctAnswerIndex && mouseGuide != null)
            {
                mouseGuide.SetActive(true);
                mouseGuide.transform.position = answerButtons[i].transform.position;
            }
        }
    }

    public void RetryGame()
    {
        // 1️⃣ Reset PlayerPrefs
        PlayerPrefs.SetInt("QuestProgress", 0);
        PlayerPrefs.SetInt("NanayQuizDone", 0);
        PlayerPrefs.SetInt("GuideShown", 0);
        PlayerPrefs.Save();

        // 2️⃣ Reset PlayerHealth
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.currentHearts = PlayerHealth.Instance.maxHearts;
            PlayerHealth.Instance.CurrentHealth = PlayerHealth.Instance.maxHearts;
            PlayerHealth.Instance.UpdateHeartsUI();
        }

        // 3️⃣ Close GameOverPanel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // 4️⃣ Load first scene (labas ng bahay)
        SceneManager.LoadScene("LabasNgBahay");
    }

    void SelectAnswer(int index)
    {
        Question currentQ = isTutorial ? tutorialQuestions[currentQuestionIndex] : questions[currentQuestionIndex];
        bool isCorrect = index == currentQ.correctAnswerIndex;

        if (isTutorial)
        {
            if (tutorialFeedbackText != null)
            {
                StopAllCoroutines();
                StartCoroutine(ShowFeedback(
                    isCorrect ? "Tama ang iyong sagot! Magaling!" : "Mali ang iyong sagot! Galingan sa susunod",
                    isCorrect ? Color.green : Color.red
                ));
            }

            showMouseGuide = false;
            if (mouseGuide != null) mouseGuide.SetActive(false);
        }
        else
        {
            if (isCorrect && CoinManager.Instance != null)
            {
                CoinManager.Instance.AddCoin(1);
            }
            else if (!isCorrect && PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.TakeDamage(1);

                // 🔴 GAME OVER CHECK (IDINAGDAG LANG ITO)
                if (PlayerHealth.Instance.CurrentHealth <= 0 && gameOverPanel != null)
                {
                    quizPanel.SetActive(false);
                    gameOverPanel.SetActive(true);
                    return; // stop quiz completely
                }
            }
        }

        currentQuestionIndex++;

        Question[] activeSet = isTutorial ? tutorialQuestions : questions;

        if (currentQuestionIndex < activeSet.Length) ShowQuestion();
        else
        {
            if (isTutorial)
            {
                if (quizPanel != null) quizPanel.SetActive(false);
                if (tutorialEndPanel != null) tutorialEndPanel.SetActive(true);
                isTutorial = false;
            }
            else
            {
                quizPanel.SetActive(false);
                PlayerPrefs.SetInt("NanayQuizDone", 1); // mark as finished
                PlayerPrefs.SetInt("QuestProgress", 2);
                PlayerPrefs.Save();
            }
        }

        IEnumerator ShowFeedback(string message, Color color)
        {
            tutorialFeedbackText.text = message;
            tutorialFeedbackText.color = color;
            tutorialFeedbackText.alpha = 0;
            tutorialFeedbackText.gameObject.SetActive(true);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * 3;
                tutorialFeedbackText.alpha = Mathf.Lerp(0, 1, t);
                yield return null;
            }

            yield return new WaitForSeconds(1.5f);

            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * 3;
                tutorialFeedbackText.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }

            tutorialFeedbackText.gameObject.SetActive(false);
        }
    }

    // ==============================
    // Exit Message
    // ==============================
    private IEnumerator ShowExitMessage()
    {
        if (exitMessagePanel == null || exitMessageCanvasGroup == null) yield break;

        exitMessageCanvasGroup.alpha = 0f;
        exitMessagePanel.SetActive(true);
        exitMessageCanvasGroup.interactable = true;
        exitMessageCanvasGroup.blocksRaycasts = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            exitMessageCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            exitMessageCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        exitMessagePanel.SetActive(false);
    }
}
