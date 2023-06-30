using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private Slider fovSlider;
    [SerializeField] private TextMeshProUGUI fovValue;
    [SerializeField] private TMP_InputField fovInput;
    public TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    public const string MIXER_MASTER = "MasterVolume";

    [Header("Sound")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeValue;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicValue;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxValue;

    private Resolution[] resolutions;
    private List<Resolution> filteredRes = new List<Resolution>();

    private const string masterVolumeKey = "volume";
    private const string musicVolumeKey = "musicVolume";
    private const string sfxVolumeKey = "sfxVolume";
    private const string resKey = "resolution";
    private const string fullscreenKey = "fullscreen";
    private const string graphicsKey = "graphics";
    private const string fovKey = "fov";

    [Header("Values")]
    [SerializeField]
    private float defaultVol;
    [SerializeField]
    private int defaultFov;
    [SerializeField]
    private int defaultGraphics;
    private int defaultRes;

    private int graphicsIndex;
    private int resolutionIndex;
    private bool fullscreen;
    private bool settingsHaveChanged;

    private float fovDisplayVal;
    [SerializeField] private GameEvent onFovChange;

    private void OnEnable()
    {
        LoadSettings();
        settingsHaveChanged = false;
    }

    private void OnDisable()
    {
        SaveSettings();
        settingsHaveChanged = false;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        GetResolutions();
        
        graphicsIndex = defaultGraphics;
        fovInput.characterLimit = 3;
        fovInput.characterValidation = TMP_InputField.CharacterValidation.Integer;
    }

    private void GetResolutions()
    {
        resolutions = Screen.resolutions;

        defaultRes = 0;
        List<string> options = new List<string>();
        float currRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i += 1)
        {
            if (resolutions[i].refreshRate == currRefreshRate)
            {
                filteredRes.Add(resolutions[i]);
            }
        }

        for (int i = 0; i < filteredRes.Count; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (filteredRes[i].width == Screen.width && filteredRes[i].height == Screen.height)
            {
                defaultRes = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = defaultRes;
        resolutionDropdown.RefreshShownValue();
    }

    private void Start()
    {
        LoadSettings();
        settingsHaveChanged = false;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        int volPercent = (int)(volume * 100);
        volumeValue.SetText(volPercent.ToString() + '%');

        settingsHaveChanged = true;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        int volPercent = (int)(volume * 100);
        musicValue.SetText(volPercent.ToString() + '%');

        settingsHaveChanged = true;
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        int volPercent = (int)(volume * 100);
        sfxValue.SetText(volPercent.ToString() + '%');

        settingsHaveChanged = true;
    }

    public void SetVideoQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        graphicsIndex = qualityIndex;

        settingsHaveChanged = true;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        fullscreen = isFullscreen;

        settingsHaveChanged = true;
    }

    public void SetResolution(int index)
    {
        Resolution selectedRes = filteredRes[index];
        Screen.SetResolution(selectedRes.width, selectedRes.height, fullscreen);
        resolutionIndex = index;

        settingsHaveChanged = true;
    }

    public void SetFov(float value)
    {
        Camera.main.fieldOfView = value;
        fovDisplayVal = (int)value;
        fovInput.text = fovDisplayVal.ToString();
        onFovChange.CallEvent(this, value);

        settingsHaveChanged = true;
    }

    private void LoadSettings()
    {
        // audio
        // master
        float volume = PlayerPrefs.GetFloat(masterVolumeKey, defaultVol);
        SetMasterVolume(volume);
        volumeSlider.value = volume;
        // music
        float music = PlayerPrefs.GetFloat(musicVolumeKey, defaultVol);
        SetMusicVolume(music);
        musicSlider.value = music;
        // sfx
        float sfx = PlayerPrefs.GetFloat(musicVolumeKey, defaultVol);
        SetSfxVolume(sfx);
        sfxSlider.value = sfx;

        // res
        int resIndex = PlayerPrefs.GetInt(resKey, defaultRes);
        SetResolution(resIndex);
        resolutionDropdown.value = resIndex;

        // graphics
        int qualityIndex = PlayerPrefs.GetInt(graphicsKey, defaultGraphics);
        SetVideoQuality(qualityIndex);
        qualityDropdown.value = qualityIndex;

        // fullscreen
        bool isFull = PlayerPrefs.GetInt(fullscreenKey, 1) == 1;
        SetFullscreen(isFull);
        fullscreenToggle.isOn = isFull;

        // fov
        float fov = PlayerPrefs.GetFloat(fovKey, defaultFov);
        SetFov(fov);
        fovSlider.value = fov;

        Debug.Log("Settings loaded");
    }

    private void SaveSettings()
    {
        if (settingsHaveChanged)
        {
            // volume
            PlayerPrefs.SetFloat(masterVolumeKey, volumeSlider.value);
            PlayerPrefs.SetFloat(musicVolumeKey, musicSlider.value);
            PlayerPrefs.SetFloat(sfxVolumeKey, sfxSlider.value);
            // res
            PlayerPrefs.SetInt(resKey, resolutionIndex);
            // graphics
            PlayerPrefs.SetInt(graphicsKey, graphicsIndex);
            // fullscreen
            PlayerPrefs.SetInt(fullscreenKey, fullscreen ? 1 : 0);
            // fov
            PlayerPrefs.SetFloat(fovKey, fovDisplayVal);

            PlayerPrefs.Save();
        }
    }

    public void SetFovOnText(string input)
    {
        int fovValue = 70;
        if (int.TryParse(input, out fovValue))
        {
            if (fovValue < fovSlider.minValue)
            {
                fovValue = (int)fovSlider.minValue;
            }
            else if (fovValue > fovSlider.maxValue)
            {
                fovValue = (int)fovSlider.maxValue;
            }
            SetFov(fovValue);
            fovSlider.value = fovValue;
        }
        else
        {
            fovInput.text = fovDisplayVal.ToString();
        }
    }


}