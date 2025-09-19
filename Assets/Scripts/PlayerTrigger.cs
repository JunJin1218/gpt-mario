using UnityEngine;
using TMPro;

public class PlayerTrigger : MonoBehaviour
{
    private Rigidbody2D marioBody;
    private PlayerMovement playerMovement; //붙어있는 cs파일 불러오기

    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;
    public CanvasSwitch gameOverCanvasManager;
    public CanvasSwitch inGameCanvasManager;

    void Start()
    {
        marioBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Collided with goomba!");
            Time.timeScale = 0.0f;
            inGameCanvasManager.switchCanvasCallback(false);
            gameOverCanvasManager.switchCanvasCallback(true);
        }
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
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
        gameOverCanvasManager.switchCanvasCallback(false);
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }

    }
}
