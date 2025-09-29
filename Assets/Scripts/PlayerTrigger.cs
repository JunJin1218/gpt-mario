using UnityEngine;
using TMPro;

public class PlayerTrigger : MonoBehaviour
{
    private Rigidbody2D marioBody;
    private PlayerMovement playerMovement; //붙어있는 cs파일 불러오기

    // ===== PUBLIC =====
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public GameObject questionBoxes;
    public JumpOverGoomba jumpOverGoomba;
    public CanvasSwitch gameOverCanvasManager;
    public CanvasSwitch inGameCanvasManager;
    public CanvasSwitch GPTCanvasManager;
    public AudioSource marioAudio;
    public Animator marioAnimator;

    public AudioClip marioDeath;
    public float deathImpulse = 15;

    // ===== NONSERIALIZED =====



    void Start()
    {
        marioBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && GameManager.Instance.playerAlive)
        {
            // Debug.Log("Collided with goomba!");

            GameOverAnimation();
        }
    }

    void GameOverAnimation()
    {
        marioAnimator.Play("mario-die");
        marioAudio.PlayOneShot(marioDeath);
        GameManager.Instance.playerAlive = false;
    }

    void GameOver()
    {
        Time.timeScale = 0.0f;

        // Later below functions should be moved to GameManager
        inGameCanvasManager.switchCanvasCallback(false);
        GPTCanvasManager.switchCanvasCallback(false);
        gameOverCanvasManager.switchCanvasCallback(true);

    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
        GameManager.Instance.playerAlive = true;
        marioAnimator.SetTrigger("gameRestart");
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(0f, 4f, 0.0f);
        // reset sprite direction
        playerMovement.setMarioFace(true);
        // reset score
        GameManager.Instance.ResetScore();
        scoreText.text = "Score: 0";
        // reset Canvas
        inGameCanvasManager.switchCanvasCallback(true);
        GPTCanvasManager.switchCanvasCallback(true);
        gameOverCanvasManager.switchCanvasCallback(false);
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
        // reset QuestionBox
        var boxes = questionBoxes.GetComponentsInChildren<QuestionBoxDynamics>();
        foreach (var box in boxes)
        {
            box.resetQuestionBox();
        }
        var coins = questionBoxes.GetComponentsInChildren<CoinDynamics>();
        foreach (var coin in coins)
        {
            coin.resetCoin();
        }
    }

    void PlayDeathImpulse()
    {
        marioBody.linearVelocity = Vector2.zero;
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

}
