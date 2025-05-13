using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SettingsMenuManager : MonoBehaviour
{
    [Header("패널 및 UI 요소")]
    public GameObject settingsPanel;
    public TMP_Dropdown resolutionDropdown;
    public GameObject resolutionDropdownContainer;
    public TextMeshProUGUI screenModeText;
    public Button leftArrowButton;
    public Button rightArrowButton;
    public Button applyButton;
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;

    private Resolution[] resolutions;
    private int selectedResolutionIndex;
    private int selectedScreenModeIndex; // 0: 창모드, 1: 전체화면

    private float selectedMasterVolume;
    private float selectedBGMVolume;
    private float selectedSFXVolume;

    private readonly string[] screenModes = { "창모드", "전체화면" };

    private void Start()
    {
        settingsPanel.SetActive(false);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
        selectedResolutionIndex = resolutions.Length - 1;
        resolutionDropdown.value = selectedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        selectedScreenModeIndex = 1;
        UpdateScreenModeText();

        leftArrowButton.onClick.AddListener(() => ChangeScreenMode(-1));
        rightArrowButton.onClick.AddListener(() => ChangeScreenMode(1));
        applyButton.onClick.AddListener(ApplySettings);

        masterVolumeSlider.onValueChanged.AddListener(value => selectedMasterVolume = value);
        bgmVolumeSlider.onValueChanged.AddListener(value => selectedBGMVolume = value);
        sfxVolumeSlider.onValueChanged.AddListener(value => selectedSFXVolume = value);

        selectedMasterVolume = masterVolumeSlider.value;
        selectedBGMVolume = bgmVolumeSlider.value;
        selectedSFXVolume = sfxVolumeSlider.value;
    }

    private void ChangeScreenMode(int direction)
    {
        selectedScreenModeIndex = Mathf.Clamp(selectedScreenModeIndex + direction, 0, screenModes.Length - 1);
        UpdateScreenModeText();
        ApplyResolutionUIVisibility();
    }

    private void UpdateScreenModeText()
    {
        screenModeText.text = screenModes[selectedScreenModeIndex];
    }

    private void ApplyResolutionUIVisibility()
    {
        bool isWindowed = (selectedScreenModeIndex == 0);
        resolutionDropdown.interactable = isWindowed;
        if (resolutionDropdownContainer != null)
            resolutionDropdownContainer.SetActive(isWindowed);
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
        ApplyResolutionUIVisibility();
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void ApplySettings()
    {
        StartCoroutine(ApplySettingsWithDelay());
    }

    private IEnumerator ApplySettingsWithDelay()
    {
        Resolution res = resolutions[resolutionDropdown.value];

        if (selectedScreenModeIndex == 1)
        {
            // 전체화면: 창모드로 전환 후 전체화면으로 설정
            Screen.SetResolution(res.width, res.height, FullScreenMode.Windowed);
            yield return null;
            yield return new WaitForSeconds(0.05f);
            Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(res.width, res.height, FullScreenMode.Windowed);
        }

         SoundManager.Instance.SetMasterVolume(selectedMasterVolume);
    SoundManager.Instance.SetBGMVolume(selectedBGMVolume);
    SoundManager.Instance.SetSFXVolume(selectedSFXVolume);

    Debug.Log("설정 적용 완료: 해상도, 화면모드, 볼륨");

       

    }


}
