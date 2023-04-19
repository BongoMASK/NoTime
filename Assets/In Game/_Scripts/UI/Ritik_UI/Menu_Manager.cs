using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu_Manager : MonoBehaviour
{

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSensi = 4;
    public int mainControllerSensi = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [SerializeField] private GameObject confirmationPrompt=null;
    [Header("Levels to load")]
    public string _newGameLevel;
    private string levelToLoad; 
    [SerializeField] private GameObject nosavedPopup = null;
    [SerializeField] public GameObject new_gamepopup;
    [SerializeField] public GameObject POpup_Panel;
    [SerializeField] public GameObject loadgame_popup;
    [SerializeField] public GameObject NoFound_popup;
    [SerializeField] public GameObject Mainmenu_panel;
    [SerializeField] public GameObject Option_Panel;
    [SerializeField] private TMP_Dropdown resolutionDropDown;

    private Resolution[] resolutions;
    private List<Resolution> filterResolutions;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    public static Menu_Manager instance;

   
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }

    }


    private void Start()
    {
        resolutions = Screen.resolutions;
        filterResolutions = new List<Resolution>();
        resolutionDropDown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        Debug.Log("RefreshRate :" + currentRefreshRate);

        for (int i = 0; i < resolutions.Length; i++)
        {
            Debug.Log("CurrentResolution :" + resolutions[i]);
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filterResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        for (int i = 0; i < filterResolutions.Count; i++)
        {
            string resolutionOption = filterResolutions[i].width + "x" + filterResolutions[i].height + " " + filterResolutions[i].refreshRate + "Hz";
            options.Add(resolutionOption);
            if (filterResolutions[i].width == Screen.width && filterResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filterResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }


    public void NewGameButtonClick()
    {
        new_gamepopup.SetActive(true);
        Mainmenu_panel.SetActive(false);
       
       
    }

    public void newgame_OKpressed()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void NO_buttonpressed()
    {
        if (new_gamepopup == true)
        {
            new_gamepopup.SetActive(false);
            Mainmenu_panel.SetActive(true);
        }
        if(loadgame_popup == true)
        {
            loadgame_popup.SetActive(false);
            Mainmenu_panel.SetActive(true);
        }
    }

    
    public void LoadGAmeButtonClicked()
    {
        loadgame_popup.SetActive(true);
        Mainmenu_panel.SetActive(false);
    }
    public void LoadGameOK()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SaveLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            nosavedPopup.SetActive(true);
        }
    }

    public void OptionsButttonClicked()
    {
        Option_Panel.SetActive(true);
        Mainmenu_panel.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
        //show prompt
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSensi = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        if(invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
            // invert Y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
            //not invert
        }
        PlayerPrefs.SetFloat("masterSen", mainControllerSensi);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if(MenuType == "Gameplay")
        {
            controllerSenTextValue.text = defaultSensi.ToString("0");
            controllerSenSlider.value = defaultSensi;
            mainControllerSensi = defaultSensi;
            invertYToggle.isOn = false;
            GameplayApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
