using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Main Script")]
    [SerializeField]
    public Conversation conversation;

    [Header("Setting Object")]
    public GameObject setting;
    public GameObject volumeSlider, speedSlider, autoSaveToggle;

    public DataManager data;

    //----------------- GAME LOGIC --------------------
    private void Start()
    {
        if (data == null)
            Debug.LogError("Data is null,gane will not work correctly");

        GetCorrectController();

        SettingOff();
        SyncGUI();
        conversation.PreLoad();
        StartCoroutine(AutoSave());
    }

    IEnumerator AutoSave()
    {
        while (data.Values.autoSave)
        {
            Save();
            yield return new WaitForSeconds(1);
        }
    }

    void SyncGUI()
    {
        volumeSlider.GetComponent<Slider>().value = data.Values.volume;
        speedSlider.GetComponent<Slider>().value = conversation.Speed = data.Values.speed;
    }

    //----------------- GAME ACTIONS --------------------
    public void ChangeVolume(BaseEventData eventData)
    {
        data.Values.volume = eventData.selectedObject.GetComponent<Slider>().value;
        if (data.Values.volume > 0)
            FindObjectOfType<AudioSource>().volume = data.Values.volume;
    }

    public void ChangeSpeed(BaseEventData eventData)
    {
        data.Values.speed = eventData.selectedObject.GetComponent<Slider>().value;
        if (data.Values.speed > 0)
            conversation.Speed = data.Values.speed;
    }

    public void ChangeAutoSave()
    {
        data.Values.autoSave = autoSaveToggle.GetComponent<Toggle>().isOn;
        if (data.Values.autoSave)
            StartCoroutine(AutoSave());
    }

    public void Save()
    {
        data.UpdateValuesData();
        data.UpdateChapterData();
    }

    public void SettingOn()
    {
        setting.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SettingOff()
    {
        setting.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Application.Quit();
    }

    GameController GetCorrectController()
    {
        GameController controller = this;

        if (controller.data != null)
        {
            foreach (GameController c in FindObjectsOfType<GameController>())
            {
                if (c != this)
                    Destroy(c.gameObject);
            }
            return this;
        }
 
        foreach (GameController c in FindObjectsOfType<GameController>())
        {
            if (c.data != null)
                controller = c;
        }
        return controller;
    }

    public void GameReset()
    {
        GameController controller = this;
        
        foreach( GameController c in FindObjectsOfType<GameController>() )
        {
            if (c.data != null)
                controller = c;
        }

        controller.data.Reset(controller.data.Dialogues.GetAllTree()[0]);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
