using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

   [SerializeField] private TextMeshProUGUI interactText;
    private Pickable pickable;
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
