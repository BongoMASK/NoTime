using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI interactText;
    public void SetInteractText(string text)
    {
        interactText.text = text;
        interactText.gameObject.SetActive(true);
    }
    private void Awake()
    {
        interactText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        IRayCastMessage.OnPlayerViewed += OnPlayerViewed;
    }

    private void OnDisable()
    {
        IRayCastMessage.OnPlayerViewed -= OnPlayerViewed;
    }

    private void OnPlayerViewed(string viewedObjMessage)
    {
        interactText.text = viewedObjMessage;
        interactText.gameObject.SetActive(true);
    }
}
