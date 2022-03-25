using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{
    public GameObject template;
    public GameObject notice;

    public Transform site;
    public List<string> titles = new List<string>();

    List<Button> buttonList = new List<Button>();
    List<string> listTree = new List<string>();

    DataManager data;

    private void Start()
    {
        data = FindObjectOfType<GameController>().data;
        listTree.AddRange(data.Dialogues.GetAllTree());
        LoadChapters();
        StartCoroutine(EnableChapters());
    }

    public void LoadChapters()
    {
        for (int i = 0; i < listTree.Count; i++)
        {
            GameObject obj = Instantiate(template, site);
            obj.name = listTree[i];
            obj.GetComponentInChildren<TextMeshProUGUI>().text = listTree[i] + ". " + titles[i];
            buttonList.Add(obj.GetComponentInChildren<Button>());
            buttonList[i].onClick.AddListener(delegate
            {
                GetAccept(obj.name);
                
            });
            buttonList[i].gameObject.SetActive(false);
        }
    }

    void GetAccept(string tree)
    {
        GameObject popup = Instantiate(notice, site.parent.parent.parent);
        popup.GetComponent<RectTransform>().localPosition = Vector3.zero;
        popup.transform.GetChild(0).Find("ok").GetComponent<Button>().onClick.AddListener(
            delegate
            {
                OnClick_OK(tree);
            });
        popup.transform.GetChild(0).Find("cancel").GetComponent<Button>().onClick.AddListener(
            delegate
            {
                OnClick_Cancel(popup);
            });
        Time.timeScale = 0;
    }

    void OnClick_OK(string tree)
    {
        data.Reset(tree);

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    void OnClick_Cancel(GameObject obj)
    {
        Destroy(obj);
        Time.timeScale = 1.0f;
    }

    IEnumerator EnableChapters()
    {
        while (true)
        {
            for (int i = 0; i < data.Values.loadedChapter.Count; i++)
            {
                if (site == null)
                    break;

                Transform chapter = site.Find( data.Values.loadedChapter[i]);

                if (chapter == null)
                    break;
                chapter.Find("Button").gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
