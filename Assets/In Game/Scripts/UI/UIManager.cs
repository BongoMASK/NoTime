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
}
