using UnityEngine;

public class CoinDynamics : MonoBehaviour
{
    // ==== PUBLIC =====
    public Rigidbody2D coinBody;
    public Animator coinAnimator;
    public AudioSource audioSource;
    public AudioClip coinSound;
    public float coinSpeed = 5;
    void jumpCoin()
    {
        coinBody.bodyType = RigidbodyType2D.Dynamic;
        coinBody.AddForce(Vector2.up * coinSpeed, ForceMode2D.Impulse);
    }

    public void resetCoin()
    {
        coinBody.transform.localPosition = Vector3.zero;
        coinBody.bodyType = RigidbodyType2D.Static;
        coinAnimator.SetTrigger("reset");

    }
    public void PlayCoinSound()
    {
        if (audioSource != null && coinSound != null)
            audioSource.PlayOneShot(coinSound);
    }
}
