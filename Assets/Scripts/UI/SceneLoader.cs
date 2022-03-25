using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    [Range(0.5f, 5f)]
    public float timeToLoading;

    public GameObject loadedObject;
    public GameObject doneObject;
    public GameObject noticeObject;

    string currentSceneName;
    AsyncOperation process;
    bool enable = false;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        doneObject.SetActive(false);
        noticeObject.SetActive(false);
        Invoke("StopLoading", timeToLoading);
        LoadScene();
    }

    private void FixedUpdate()
    {
        if (enable)
        if (Input.touchCount > 0 && Input.GetTouch(0).type == TouchType.Direct)
        {
            //Do Stuff
            process.allowSceneActivation = true;
        }
    }

    void StopLoading()
    {
        // Destroy and show UI objects
        Destroy(loadedObject);
        doneObject.SetActive(true);
        noticeObject.SetActive(true);
        enable = true;
    }

    private void LoadScene()
    {
        process = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
        process.allowSceneActivation = false;
    }
}
