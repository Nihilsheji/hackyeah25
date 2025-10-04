using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class VoiceKeywordRecognition : MonoBehaviour
{
    [Header("Keyword Settings")]
    [Tooltip("Add the keywords you want to recognize")]
    public string[] keywords = new string[] { "jump", "fire", "menu", "pause", "start" };
    
    [Header("Recognition Settings")]
    [Tooltip("Confidence level required (Low, Medium, High)")]
    public ConfidenceLevel confidenceLevel = ConfidenceLevel.Medium;
    
    [Header("Debug")]
    public bool showDebugMessages = true;
    
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywordActions = new Dictionary<string, System.Action>();

    void Start()
    {
        // Check if keywords array is not empty
        if (keywords.Length == 0)
        {
            Debug.LogError("No keywords defined! Please add keywords in the Inspector.");
            return;
        }

        // Initialize the keyword recognizer with your keywords
        keywordRecognizer = new KeywordRecognizer(keywords, confidenceLevel);
        
        // Subscribe to the OnPhraseRecognized event
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        
        // Start listening
        keywordRecognizer.Start();
        
        if (showDebugMessages)
        {
            Debug.Log("Voice Recognition Started. Listening for: " + string.Join(", ", keywords));
        }
        
        // Register default keyword actions
        RegisterKeywordActions();
        StartRecognition();
        Debug.Log(IsRunning());
    }

    private void RegisterKeywordActions()
    {
        // Register your keyword actions here
        // Example actions - customize these for your game
        
        keywordActions["jump"] = () => {
            Debug.Log("Jump command recognized!");
            // Add your jump logic here
            // Example: GetComponent<PlayerController>()?.Jump();
        };
        
        keywordActions["fire"] = () => {
            Debug.Log("Fire command recognized!");
            // Add your fire logic here
            // Example: GetComponent<WeaponController>()?.Fire();
        };
        
        keywordActions["menu"] = () => {
            Debug.Log("Menu command recognized!");
            // Add your menu logic here
            // Example: UIManager.Instance?.ToggleMenu();
        };
        
        keywordActions["pause"] = () => {
            Debug.Log("Pause command recognized!");
            // Add your pause logic here
            // Example: GameManager.Instance?.TogglePause();
        };
        
        keywordActions["start"] = () => {
            Debug.Log("Start command recognized!");
            // Add your start logic here
            // Example: GameManager.Instance?.StartGame();
        };
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        string recognizedKeyword = args.text;
        
        if (showDebugMessages)
        {
            Debug.Log($"Voice Command Recognized: '{recognizedKeyword}' (Confidence: {args.confidence})");
        }
        
        // Execute the action associated with the keyword
        if (keywordActions.ContainsKey(recognizedKeyword))
        {
            keywordActions[recognizedKeyword]?.Invoke();
        }
    }

    // Public method to register custom keyword actions from other scripts
    public void RegisterKeyword(string keyword, System.Action action)
    {
        if (keywordActions.ContainsKey(keyword))
        {
            keywordActions[keyword] = action;
        }
        else
        {
            keywordActions.Add(keyword, action);
        }
    }

    // Public method to check if recognition is running
    public bool IsRunning()
    {
        return keywordRecognizer != null && keywordRecognizer.IsRunning;
    }

    [Button]
    public void DebugLogIsRunning()
    {
        Debug.Log(IsRunning());
    }

    // Public method to stop recognition
    public void StopRecognition()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            if (showDebugMessages)
            {
                Debug.Log("Voice Recognition Stopped");
            }
        }
    }

    // Public method to start recognition
    public void StartRecognition()
    {
        if (keywordRecognizer != null && !keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Start();
            if (showDebugMessages)
            {
                Debug.Log("Voice Recognition Started");
            }
        }
    }

    void OnApplicationQuit()
    {
        CleanUp();
    }

    void OnDestroy()
    {
        CleanUp();
    }

    private void CleanUp()
    {
        if (keywordRecognizer != null)
        {
            if (keywordRecognizer.IsRunning)
            {
                keywordRecognizer.Stop();
            }
            keywordRecognizer.OnPhraseRecognized -= OnPhraseRecognized;
            keywordRecognizer.Dispose();
        }
    }
}