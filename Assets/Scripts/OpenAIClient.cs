using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class OpenAIClient
{
    static readonly HttpClient http = new HttpClient();

    public static async Task<string> GetTimelineJsonAsync(string userPrompt, string apiKey)
    {
        http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        var body = new
        {
            model = "gpt-4.1-mini",
            input = new object[] {
                new { role="system", content="You convert plain Korean or English commands into a strict JSON timeline for a Mario-like platformer. Use only the provided schema. When the user requires more fine-tuned actions, make the interval smaller."},
                new { role="user", content=userPrompt }
            },
            text = new
            {
                format = new
                {
                    type = "json_schema",
                    name = "mario_command_timeline",
                    strict = true,
                    schema = new
                    {
                        type = "object",
                        additionalProperties = false,
                        properties = new
                        {
                            interval = new { type = "number", minimum = 0.05, maximum = 1.0 },
                            steps = new
                            {
                                type = "array",
                                minItems = 3,
                                maxItems = 10,
                                items = new
                                {
                                    type = "string",
                                    @enum = new[] { "left", "right", "jump", "left+jump", "right+jump", "skip-for-this-step" }
                                }
                            }
                        },
                        required = new[] { "interval", "steps" }
                    }
                }
            }
        };


        var json = JsonConvert.SerializeObject(body, Formatting.None);
        // Debug.Log("Request Body: " + json);
        var req = new StringContent(json, Encoding.UTF8, "application/json");
        var res = await http.PostAsync("https://api.openai.com/v1/responses", req);
        var txt = await res.Content.ReadAsStringAsync();
        // Debug.Log("Response: " + txt);
        res.EnsureSuccessStatusCode();
        return txt;
    }

    public static (float interval, string[] steps) ExtractTimelineJson(string rawResponse)
    {
        var root = JObject.Parse(rawResponse);
        var timelineJSON = root["output"]?[0]?["content"]?[0]?["text"];

        if (timelineJSON == null || string.IsNullOrEmpty(timelineJSON.ToString()))
            throw new System.Exception("Timeline JSON not found in response");

        var timelineObj = JObject.Parse(timelineJSON.ToString());
        float interval = timelineObj["interval"].Value<float>();
        string[] steps = timelineObj["steps"].ToObject<string[]>();

        return (interval, steps);
    }
}
