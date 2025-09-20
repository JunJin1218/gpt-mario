using System.Threading.Tasks;
using UnityEngine;

public class GPTConnectionTest : MonoBehaviour
{
    [SerializeField] string apiKey; // GitHub에 절대 올리지 말것
    [SerializeField] string prompt = "앞으로 두 칸 가다가 점프";

    async void Start()
    {
        /*
        Debug.Log("=== GPTTest Start ===");
        string result = await OpenAIClient.GetTimelineJsonAsync(prompt, apiKey);
        (float interval, string[] steps) = OpenAIClient.ExtractTimelineJson(result);
        Debug.Log("=== Raw Response ===");
        Debug.Log(result);
        Debug.Log("=== Extracted Values ===");
        Debug.Log(interval);
        Debug.Log(string.Join(", ", steps));
        */
    }
}