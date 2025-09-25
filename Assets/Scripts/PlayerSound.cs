using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    // ===== PUBLIC =====
    public AudioSource marioAudio;
    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }
}
