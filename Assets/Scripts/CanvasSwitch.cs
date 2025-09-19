using UnityEngine;

public class CanvasSwitch : MonoBehaviour
{
    public bool activeStart = false;
    void Start()
    {
        gameObject.SetActive(activeStart);
    }

    /// <summary>
    /// on --> Activate canvas
    /// </summary>
    public void switchCanvasCallback(bool on)
    {
        gameObject.SetActive(on);
    }
}