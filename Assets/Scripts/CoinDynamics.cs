using UnityEngine;

public class CoinDynamics : MonoBehaviour
{
    // ==== PUBLIC =====
    public Rigidbody2D coinBody;
    public Animator coinAnimator;
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
}
