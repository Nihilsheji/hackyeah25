using UnityEngine;
using UnityAtoms.BaseAtoms;

[RequireComponent(typeof(MonoBehaviour))]
public class KeywordComparator : MonoBehaviour 
{
    [SerializeField] private StringEvent keywordRecognizedEvent;
    [SerializeField] private string targetKeyword;
    [SerializeField] private MonoBehaviour reactableComponent; // Should implement IDamageable

    private IKeywordReactable keywordReactable;

    private void Awake()
    {
        keywordReactable = reactableComponent as IKeywordReactable;
        if (keywordReactable == null)
        {
            Debug.LogError($"{nameof(KeywordComparator)} requires a component implementing {nameof(IKeywordReactable)}.");
        }
    }

    private void OnEnable()
    {
        keywordRecognizedEvent?.Register(OnKeywordRecognized);
    }

    private void OnDisable()
    {
        keywordRecognizedEvent?.Unregister(OnKeywordRecognized);
    }

    public void OnKeywordRecognized(string recognizedKeyword)
    {
        if (recognizedKeyword == targetKeyword)
        {
            keywordReactable?.OnKeywordRecognized();
        }
    }
}
