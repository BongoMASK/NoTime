using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public string menuName;
    public bool open = true;

    /// <summary>
    /// Opens Menu
    /// </summary>
    public void Open() {
        open = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Closes Menu
    /// </summary>
    public void Close() {
        open = false;
        gameObject.SetActive(false);
    }
}
