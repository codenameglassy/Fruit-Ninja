using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public List<UIPanel> uiPanels;

    public UIPanel activeUIPanel;
    public Slider soundSlider;
    public Slider musicSlider;

    public Canvas effectCanvas;

    public GameObject ComboObjPrefab;

    private GameObject comboSpwaned;

    public TMP_Text highscoreText;

    public HighScore highscore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SwitchCanvas(uiPanels[0].uiPanelType);
        soundSlider.value = soundManager.instance.SoundVolume();
        musicSlider.value = soundManager.instance.MusicVolume();
        soundSlider.onValueChanged.AddListener(delegate { OnSoundVolumeChanged(); });
        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
        if (activeUIPanel.uiPanelType == UIPanelType.mainmenu)
        {
            highscoreText.text = highscore.highscore.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCanvas(UIPanelType targetPanel)
    {


        foreach (UIPanel panel in uiPanels)
        {

            if (panel.uiPanelType == targetPanel)
            {
                activeUIPanel = panel;
            }
            else
            {
                panel.gameObject.SetActive(false);
            }
        }
        
        activeUIPanel.gameObject.SetActive(true);
    }

    public void OnMusicVolumeChanged()
    {

        soundManager.instance.MusicVolumeChanged(musicSlider.value);
    }

    public void OnSoundVolumeChanged()
    {
        soundManager.instance.SoundVolumeChanged(soundSlider.value);
    }

    public void ShowCombo(Vector3 pos,int combotext)
    {
        StopAllCoroutines();
        if(comboSpwaned==null)
            comboSpwaned = Instantiate(ComboObjPrefab, effectCanvas.transform);
        EnableCombo();
        comboSpwaned.transform.position = pos;
        comboSpwaned.GetComponent<TMP_Text>().text ="Combo\n" +combotext.ToString();
        StartCoroutine("AutoDisableCombo");
    }

    public void DisableCombo()
    {
        if (comboSpwaned != null)
            comboSpwaned.SetActive(false);
    }
    public void EnableCombo()
    {
        if (comboSpwaned != null)
            comboSpwaned.SetActive(true);
    }

    IEnumerator AutoDisableCombo()
    {
        yield return new WaitForSeconds(1);
        DisableCombo();
        // Code to execute after the delay
    }

}