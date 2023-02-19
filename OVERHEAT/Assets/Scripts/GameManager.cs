using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public CinemachineVirtualCamera startCam;

    public Animator StartAnim, PauseAnim, DefeatAnim, VictoryAnim, DashTutorial, DeflectTutorial, DischargeTutorial, OverheatTutorial;

    public bool isPaused = false;
    public bool isStarted = false;

    public bool shownDashTutorial, shownDeflectTutorial, shownDischargeTutorial, shownOverheatTutorial;
    private bool dashTutorialShown = false, deflectTutorialShown = false, dischargeTutorialShown = false, overheatTutorialShown = false;

    public bool showingTutorial = false;
    public bool ended = false; 

    public Toggle tutorialToggle;

    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    // Start is called before the first frame update
    void Start()
    {
        gm = this;
        isStarted = false;
        isPaused = false;
        Time.timeScale = 0f;
        startCam.Priority = 11;
        tutorialToggle.isOn = Settings.TutorialOn;
    }

    public void OnTutorialToggle()
    {
        Settings.TutorialOn = tutorialToggle.isOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!isPaused)
            {
                Pause();
            }
            else if(isPaused)
            {
                Unpause();
            }
        }
    }

    public void ShowTutorial(int num)
    {
        if (Settings.TutorialOn)
        {
            switch (num)
            {
                case 0: //dash
                    if (!dashTutorialShown)
                    {
                        showingTutorial = true;
                        Time.timeScale = 0f;
                        dashTutorialShown = true;
                        DashTutorial.SetTrigger("Pause");
                    }
                    break;
                case 1: //deflect
                    if (!deflectTutorialShown)
                    {
                        showingTutorial = true;
                        Time.timeScale = 0f;
                        deflectTutorialShown = true;
                        DeflectTutorial.SetTrigger("Pause");
                    }
                    break;
                case 2: //discharge
                    if (!dischargeTutorialShown)
                    {
                        showingTutorial = true;
                        Time.timeScale = 0f;
                        dischargeTutorialShown = true;
                        DischargeTutorial.SetTrigger("Pause");
                    }
                    break;
                case 3: //overheat
                    if (!overheatTutorialShown)
                    {
                        showingTutorial = true;
                        Time.timeScale = 0f;
                        overheatTutorialShown = true;
                        OverheatTutorial.SetTrigger("Pause");
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public void HideTutorial(int num)
    {
        switch (num)
        {
            case 0: //dash
                showingTutorial = false;
                Time.timeScale = 1f;
                DashTutorial.SetTrigger("Unpause");

                break;
            case 1: //deflect
                showingTutorial = false;
                Time.timeScale = 1f;
                DeflectTutorial.SetTrigger("Unpause");

                break;
            case 2: //discharge
                showingTutorial = false;
                Time.timeScale = 1f;
                DischargeTutorial.SetTrigger("Unpause");

                break;
            case 3: //overheat
                showingTutorial = false;
                Time.timeScale = 1f;
                OverheatTutorial.SetTrigger("Unpause");
                tutorialToggle.isOn = false; // if we want to make it so players can replay without having to get the tutorial over and over
                break;
            default:
                break;

        }
    }
    public void WinGame()
    {
        Time.timeScale = 0f;
        ended = true;
        VictoryAnim.SetTrigger("End");
    }
    public void LoseGame()
    {
        Time.timeScale = 0f;
        ended = true;
        DefeatAnim.SetTrigger("End");
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        isStarted = true;
        startCam.Priority = 9;
        StartAnim.SetTrigger("Start");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        PauseAnim.ResetTrigger("Unpause");
        PauseAnim.SetTrigger("Pause");
    }
    public void Unpause()
    {
        if(isStarted && !showingTutorial && !ended)
            Time.timeScale = 1f;
        isPaused = false;
        PauseAnim.SetTrigger("Unpause");
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateMixerVolume()
    {
        print("Changed");
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
    public void OnMasterSliderValueChange(float value)
    {
        masterVolume = value;
        Settings.MasterVolume = masterVolume;
        UpdateMixerVolume();
    }
    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        Settings.MusicVolume = musicVolume;
        UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        sfxVolume = value;
        Settings.SFXVolume = sfxVolume;
        UpdateMixerVolume();
    }
}
