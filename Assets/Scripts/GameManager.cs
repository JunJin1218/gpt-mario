using UnityEngine;
using TMPro;
using Unity.Mathematics;

public enum ScoreMode { JumpGoomba, Distance }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // === PUBLIC ===
    public Transform marioTransform;
    public ScoreMode scoreMode;
    public int Score { get; private set; } = 0;
    [System.NonSerialized] public bool playerAlive = true;


    // === PRIVATE ===
    private System.Action scoringMethod;
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

        switch (scoreMode)
        {
            case ScoreMode.Distance:
                scoringMethod = DistanceBasedScoring;
                break;
            case ScoreMode.JumpGoomba:
                scoringMethod = () => { };
                break;
            default:
                scoringMethod = () => { };
                break;
        }
    }
    void Update()
    {
        scoringMethod();
    }
    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void DistanceBasedScoring()
    {
        if (marioTransform.position.x > Score)
        {
            Score = Mathf.FloorToInt(marioTransform.position.x);
            UIManager.Instance.updateScoreTexts();
        }
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public int GetScore()
    {
        return Score;
    }

}
