using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public enum GPTState   // Yes. FSM. I gave up with using so many bools(hits)
{
    Idle, // 채팅 입력 가능 & gpt 호출 가능
    Waiting, // (채팅 초기화 한 후 & )채팅 입력 가능 & gpt 호출 불가능 & 안내 메세지
    Playing // 채팅 입력 가능 & gpt 호출 불가능 --> 다시 입력 대기로..
}
public class GPTManager : MonoBehaviour
{
    public static GPTManager Instance { get; private set; }

    [Header("Refs")]
    public TMP_InputField chatInput;
    public Button sendButton;
    public TextMeshProUGUI statusText;
    public PlayerMovement player;
    public string openAIApiKey; // DONT EVER UPLOAD IT TO GITHUB

    [Header("Play Options")]
    float safetyMaxDuration = 30f; // 최대 steps 실행 시간

    public GPTState State { get; private set; } = GPTState.Idle;
    public bool useGPTControl = true;
    [NonSerialized] public int dir = 0;
    [NonSerialized] public bool doJump = false;

    [NonSerialized] public Coroutine playRoutine;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // if (sendButton) sendButton.onClick.AddListener(OnSendClicked);
        if (chatInput) chatInput.onSubmit.AddListener(_ => { if (State == GPTState.Idle) OnSendClicked(); });
        UpdateUI();
    }

    void SetState(GPTState s)
    {
        State = s;
        UpdateUI();
    }

    void Update()
    {

        if (!GameManager.Instance.playerAlive && playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;

            ApplyStep(0, "");        // 입력 해제
            SetState(GPTState.Idle); // 상태 리셋
            SetStatus("Cancelled."); // 메시지 표시
        }

    }

    void UpdateUI()
    {
        switch (State)
        {
            case GPTState.Idle:
                SetStatus("Type a command and press Send.");
                SetInteractable(true);
                break;
            case GPTState.Waiting:
                SetStatus("Contacting GPT…");
                SetInteractable(false);
                break;
            case GPTState.Playing:
                SetStatus("Playing timeline…");
                SetInteractable(false);
                break;
        }
    }

    void SetStatus(string msg) { if (statusText) statusText.text = msg; }
    void SetInteractable(bool canSend)
    {
        if (sendButton) sendButton.interactable = canSend;
        if (chatInput) chatInput.interactable = State != GPTState.Waiting; // 입력은 Waiting/Playing 중에도 허용하려면 바꿔도 됨
    }

    public async void OnSendClicked()
    {
        if (State != GPTState.Idle) return;
        var prompt = chatInput ? chatInput.text : "";
        if (string.IsNullOrWhiteSpace(prompt)) return;

        SetState(GPTState.Waiting);

        try
        {
            string raw = await OpenAIClient.GetTimelineJsonAsync(prompt, openAIApiKey);
            Debug.Log("=== Raw ===");
            Debug.Log(raw);
            var (interval, steps) = OpenAIClient.ExtractTimelineJson(raw);
            Debug.Log("=== Extracted Steps ===");
            Debug.Log(string.Join(",", steps));

            // 입력창 초기화(원하면)
            chatInput.text = "";

            // 재생 시작
            StopAllCoroutines();
            playRoutine = StartCoroutine(PlayRoutine(interval, steps));
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            SetStatus("Error: " + ex.Message);
            SetState(GPTState.Idle);
        }
    }

    IEnumerator PlayRoutine(float interval, string[] steps)
    {
        SetState(GPTState.Playing);

        float elapsed = 0f;
        foreach (var step in steps)
        {
            ApplyStep(interval, step);
            yield return new WaitForSeconds(interval);
            elapsed += interval;
            if (elapsed > safetyMaxDuration) break;
        }

        // 모든 입력 해제
        ApplyStep(0, "");
        SetState(GPTState.Idle);
        SetStatus("Done.");
    }

    void ApplyStep(float interval, string step)
    {
        // step 예: "left", "right", "jump", "left+jump", "right+jump", ""
        // interval: 0.1 ~ 1

        if (!string.IsNullOrEmpty(step))
        {
            // '+'로 동시 입력 처리
            var tokens = step.Split('+');
            dir = 0;
            doJump = false;
            foreach (var t in tokens)
            {
                if (t == "left") dir = -1;
                else if (t == "right") dir = 1;
                else if (t == "jump") doJump = true;
            }
        }
        Debug.Log(interval + " " + step);
        // player.SetHorizontal(dir);
        // if (doJump) player.QueueJumpOnce();
    }
}
