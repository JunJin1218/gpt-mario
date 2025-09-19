using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI[] scoreTexts;

    void Awake()
    {
        // Singleton << 이거 없이 코딩 못하겠다 씨발
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void updateScoreTexts()
    {
        foreach (TextMeshProUGUI scoreText in scoreTexts)
            scoreText.text = "Score: " + GameManager.Instance.GetScore().ToString();
    }

}
