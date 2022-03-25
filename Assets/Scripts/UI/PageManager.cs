using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public RectTransform[] pages;
    public Button[] buttons;
    public Image[] images;

    public int currentPage = 1;
    public int chapterIndex = 0;
    public int characterIndex = 2;

    Vector3[] oldPos;
    private void Start()
    {
        BackupPosition();
        SetOnClick();
        pages[currentPage].localPosition = Vector3.zero;

        for (int i = 0; i < images.Length; i++)
        {
            SetActiveImages(i, false);
        }
    }

    void SetOnClick()
    {
        for ( int i = 0; i < buttons.Length; i++ )
        {
            buttons[i].onClick.AddListener(
                delegate
                {
                    ChangePage(i);
                });
        }
    }

    void SetActiveImages(int pos, bool state)
    {
        if (images[pos] == null)
            return;

        images[pos].gameObject.SetActive(state);
    }

    void BackupPosition()
    {
        oldPos = new Vector3[pages.Length];

        for(int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
                oldPos[i] = pages[i].localPosition;
        }
    }

    public void ChangePage(int index)
    {
        if (index < 0 || index >= pages.Length || index == currentPage || pages[index] == null)
            return;

        pages[index].localPosition = Vector3.zero;
        pages[currentPage].localPosition = oldPos[currentPage];
        SetActiveImages(index, false);
        currentPage = index;
        ChangingEffect();
    }

    void ChangingEffect()
    {
        Invoke("Reset", 0.5f);      
    }

    private void Reset()
    {
    }
}
