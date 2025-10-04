using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;

public class WindowsSpeechDebugger : MonoBehaviour
{
    [SerializeField] private StringEvent keywordRecognizedEvent;

    [Header("Test Settings")]
    [SerializeField] private string[] testKeywords = new string[] { "test", "hello", "unity", "microphone" };
    [SerializeField] private ConfidenceLevel confidenceLevel = ConfidenceLevel.Low;
    
    [Header("Debug Display")]
    [SerializeField] private bool showOnScreenDebug = true;
    
    private KeywordRecognizer keywordRecognizer;
    private DictationRecognizer dictationRecognizer;
    
    // Microphone test
    private AudioClip micClip;
    private string currentMicrophone;
    
    // Debug info
    private List<string> debugLog = new List<string>();
    private bool isTesting = false;
    private float testTimer = 0f;
    private int recognitionCount = 0;
    private float lastRecognitionTime = 0f;
    
    // GUI
    private Vector2 scrollPosition;
    private GUIStyle logStyle;
    private GUIStyle headerStyle;
    private GUIStyle buttonStyle;

    void Start()
    {
        LogMessage("=== Windows Speech Recognition Debugger ===");
        LogMessage("Unity Version: " + Application.unityVersion);
        LogMessage("Platform: " + Application.platform);
        LogMessage("");
        
        RunDiagnostics();
    }

    void RunDiagnostics()
    {
        LogMessage("--- RUNNING DIAGNOSTICS ---");
        LogMessage("");
        
        // 1. Check Platform
        CheckPlatform();
        
        // 2. Check Microphone
        CheckMicrophone();
        
        // 3. Check Windows Speech API
        CheckWindowsSpeechAPI();
        
        // 4. Test Keyword Recognizer
        TestKeywordRecognizer();
        
        LogMessage("");
        LogMessage("--- DIAGNOSTICS COMPLETE ---");
        LogMessage("Now say one of these words: " + string.Join(", ", testKeywords));
    }

    void CheckPlatform()
    {
        LogMessage("1. PLATFORM CHECK:");
        
        if (Application.platform == RuntimePlatform.WindowsPlayer || 
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            LogMessage("   ✓ Running on Windows - GOOD");
        }
        else
        {
            LogMessage("   ✗ NOT running on Windows - Windows.Speech requires Windows!");
            LogMessage("   Current platform: " + Application.platform);
        }
        
        LogMessage("");
    }

    void CheckMicrophone()
    {
        LogMessage("2. MICROPHONE CHECK:");
        
        string[] devices = Microphone.devices;
        
        if (devices.Length == 0)
        {
            LogMessage("   ✗ NO MICROPHONES DETECTED!");
            LogMessage("   - Check if microphone is plugged in");
            LogMessage("   - Check Windows Sound settings");
            LogMessage("   - Check Windows Privacy settings (Microphone access)");
        }
        else
        {
            LogMessage($"   ✓ Found {devices.Length} microphone(s):");
            for (int i = 0; i < devices.Length; i++)
            {
                LogMessage($"     [{i}] {devices[i]}");
            }
            
            // Test recording
            currentMicrophone = devices[0];
            LogMessage($"   Testing recording on: {currentMicrophone}");
            
            int minFreq, maxFreq;
            Microphone.GetDeviceCaps(currentMicrophone, out minFreq, out maxFreq);
            LogMessage($"   Frequency range: {minFreq}Hz - {maxFreq}Hz");
            
            // Try to record
            micClip = Microphone.Start(currentMicrophone, true, 1, 44100);
            if (micClip != null)
            {
                LogMessage("   ✓ Microphone recording started successfully");
                Microphone.End(currentMicrophone);
            }
            else
            {
                LogMessage("   ✗ Failed to start microphone recording");
            }
        }
        
        LogMessage("");
    }

    void CheckWindowsSpeechAPI()
    {
        LogMessage("3. WINDOWS SPEECH API CHECK:");
        
        try
        {
            // Try to get PhraseRecognitionSystem status
            var status = PhraseRecognitionSystem.Status;
            LogMessage($"   ✓ Windows Speech API accessible");
            LogMessage($"   Speech System Status: {status}");
            
            if (status == SpeechSystemStatus.Stopped)
            {
                LogMessage("   Status is 'Stopped' - This is normal before starting recognition");
            }
            else if (status == SpeechSystemStatus.Running)
            {
                LogMessage("   ✓ Speech System is Running");
            }
            else if (status == SpeechSystemStatus.Failed)
            {
                LogMessage("   ✗ Speech System FAILED!");
                LogMessage("   - Windows Speech Recognition may not be set up");
                LogMessage("   - Run Windows Speech Recognition setup wizard");
            }
        }
        catch (System.Exception e)
        {
            LogMessage("   ✗ ERROR accessing Windows Speech API:");
            LogMessage($"   {e.GetType().Name}: {e.Message}");
            LogMessage("");
            LogMessage("   POSSIBLE CAUSES:");
            LogMessage("   - Not running on Windows");
            LogMessage("   - Windows Speech Recognition not installed");
            LogMessage("   - .NET version incompatibility");
        }
        
        LogMessage("");
    }

    void TestKeywordRecognizer()
    {
        LogMessage("4. KEYWORD RECOGNIZER TEST:");
        
        try
        {
            keywordRecognizer = new KeywordRecognizer(testKeywords, confidenceLevel);
            LogMessage($"   ✓ KeywordRecognizer created successfully");
            LogMessage($"   Keywords: {string.Join(", ", testKeywords)}");
            LogMessage($"   Confidence Level: {confidenceLevel}");
            
            keywordRecognizer.OnPhraseRecognized += OnKeywordRecognized;
            keywordRecognizer.Start();
            
            var status = PhraseRecognitionSystem.Status;
            LogMessage($"   ✓ Recognition started. System status: {status}");
            
            if (status == SpeechSystemStatus.Running)
            {
                LogMessage("   ✓ READY TO LISTEN!");
                isTesting = true;
            }
            else
            {
                LogMessage($"   ⚠ Status is {status} instead of Running");
            }
        }
        catch (System.Exception e)
        {
            LogMessage("   ✗ FAILED to create KeywordRecognizer:");
            LogMessage($"   {e.GetType().Name}: {e.Message}");
            LogMessage($"   Stack: {e.StackTrace}");
        }
        
        LogMessage("");
    }

    void OnKeywordRecognized(PhraseRecognizedEventArgs args)
    {
        recognitionCount++;
        keywordRecognizedEvent.Raise(args.text);
        lastRecognitionTime = Time.time;
        
        LogMessage($">>> RECOGNITION SUCCESS! <<<");
        LogMessage($"   Keyword: '{args.text}'");
        LogMessage($"   Confidence: {args.confidence}");
        LogMessage($"   Timestamp: {args.phraseStartTime}");
        LogMessage($"   Total recognitions: {recognitionCount}");
        LogMessage("");
        
        // Play a beep to confirm
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
        }
    }

    void Update()
    {
        if (isTesting)
        {
            testTimer += Time.deltaTime;
            
            // Show periodic status updates
            if (Mathf.FloorToInt(testTimer) % 5 == 0 && testTimer % 1f < Time.deltaTime)
            {
                LogMessage($"[{Mathf.FloorToInt(testTimer)}s] Still listening... (Recognitions: {recognitionCount})");
            }
        }
    }

    void LogMessage(string message)
    {
        debugLog.Add(message);
        Debug.Log(message);
        
        // Keep only last 100 messages
        if (debugLog.Count > 100)
        {
            debugLog.RemoveAt(0);
        }
    }

    void OnGUI()
    {
        if (!showOnScreenDebug) return;
        
        InitGUIStyles();
        
        float width = Screen.width * 0.95f;
        float height = Screen.height * 0.9f;
        float x = Screen.width * 0.025f;
        float y = Screen.height * 0.05f;
        
        GUI.Box(new Rect(x, y, width, height), "");
        
        GUILayout.BeginArea(new Rect(x + 10, y + 10, width - 20, height - 20));
        
        // Header
        GUILayout.Label("Windows Speech Recognition Debugger", headerStyle);
        GUILayout.Space(10);
        
        // Status
        if (isTesting)
        {
            GUI.color = Color.green;
            GUILayout.Label($"● LISTENING - Say: {string.Join(", ", testKeywords)}", headerStyle);
            GUILayout.Label($"Recognitions: {recognitionCount} | Time: {Mathf.FloorToInt(testTimer)}s", logStyle);
            GUI.color = Color.white;
        }
        else
        {
            GUI.color = Color.yellow;
            GUILayout.Label("○ NOT LISTENING", headerStyle);
            GUI.color = Color.white;
        }
        
        GUILayout.Space(10);
        
        // Buttons
        GUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Restart Test", buttonStyle, GUILayout.Height(40)))
        {
            RestartTest();
        }
        
        if (GUILayout.Button("Test Dictation", buttonStyle, GUILayout.Height(40)))
        {
            TestDictation();
        }
        
        if (GUILayout.Button("Clear Log", buttonStyle, GUILayout.Height(40)))
        {
            debugLog.Clear();
        }
        
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        
        // Log
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(height - 200));
        
        foreach (string log in debugLog)
        {
            if (log.Contains("✓"))
            {
                GUI.color = Color.green;
            }
            else if (log.Contains("✗"))
            {
                GUI.color = Color.red;
            }
            else if (log.Contains(">>>"))
            {
                GUI.color = Color.yellow;
            }
            else
            {
                GUI.color = Color.white;
            }
            
            GUILayout.Label(log, logStyle);
        }
        
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    void InitGUIStyles()
    {
        if (logStyle == null)
        {
            logStyle = new GUIStyle(GUI.skin.label);
            logStyle.fontSize = 14;
            logStyle.wordWrap = true;
            logStyle.padding = new RectOffset(5, 5, 2, 2);
        }
        
        if (headerStyle == null)
        {
            headerStyle = new GUIStyle(GUI.skin.label);
            headerStyle.fontSize = 18;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.alignment = TextAnchor.MiddleCenter;
        }
        
        if (buttonStyle == null)
        {
            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 16;
            buttonStyle.fontStyle = FontStyle.Bold;
        }
    }

    void RestartTest()
    {
        debugLog.Clear();
        recognitionCount = 0;
        testTimer = 0f;
        
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
        
        RunDiagnostics();
    }

    void TestDictation()
    {
        LogMessage("");
        LogMessage("--- TESTING DICTATION RECOGNIZER ---");
        
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            LogMessage("Stopped KeywordRecognizer");
        }
        
        try
        {
            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.DictationResult += OnDictationResult;
            dictationRecognizer.DictationHypothesis += OnDictationHypothesis;
            dictationRecognizer.DictationComplete += OnDictationComplete;
            dictationRecognizer.DictationError += OnDictationError;
            
            dictationRecognizer.Start();
            LogMessage("✓ Dictation started - Speak freely for 10 seconds...");
            
            Invoke(nameof(StopDictation), 10f);
        }
        catch (System.Exception e)
        {
            LogMessage($"✗ Dictation failed: {e.Message}");
        }
    }

    void OnDictationResult(string text, ConfidenceLevel confidence)
    {
        LogMessage($">>> DICTATION: '{text}' (Confidence: {confidence})");
    }

    void OnDictationHypothesis(string text)
    {
        LogMessage($"Hypothesis: {text}");
    }

    void OnDictationComplete(DictationCompletionCause cause)
    {
        LogMessage($"Dictation completed: {cause}");
    }

    void OnDictationError(string error, int hresult)
    {
        LogMessage($"✗ Dictation error: {error} (Code: {hresult})");
    }

    void StopDictation()
    {
        if (dictationRecognizer != null)
        {
            dictationRecognizer.Stop();
            dictationRecognizer.Dispose();
            LogMessage("Dictation test complete");
        }
    }

    void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            if (keywordRecognizer.IsRunning)
            {
                keywordRecognizer.Stop();
            }
            keywordRecognizer.Dispose();
        }
        
        if (dictationRecognizer != null)
        {
            dictationRecognizer.Dispose();
        }
    }
}