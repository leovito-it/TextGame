using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public ValuesData Values { set; get; }
    public ChapterData Chapter { set; get; }
    public Dialogues Dialogues { set; get; }

    public bool IsValuesUpdated => valuesUpdate;
    public bool IsChapterUpdated => chapterUpdate;
    bool valuesUpdate = false, chapterUpdate = false;

    void Awake()
    {
        Dialogues = FindObjectOfType<Dialogues>();
        GetValuesData();
    }

    void GetValuesData()
    {
        Values = new ValuesData(global::GameValues.VALUES_FILE_NAME);

        if (Values.IsExists() && Values.Load() != null)
        {
            Values = JsonUtility.FromJson<ValuesData>(Values.Load());
            Values.LoadPath = MyData.GetPath(global::GameValues.VALUES_FILE_NAME);
        }
        else
        {
            DefaultValuesData();
            UpdateValuesData();
        }
    }

    public void GetChapterData()
    {
        string lastChapter = GetLastChapter() + global::GameValues.TXT;
        
        Chapter = new ChapterData(lastChapter);
        Dialogues.SetTree(GetLastChapter());

        if (Chapter.IsExists())
        {
            Chapter = JsonUtility.FromJson<ChapterData>(Chapter.Load());
            Chapter.LoadPath = MyData.GetPath(lastChapter);
        }
        else
        {
            DefaultChapterData();
            UpdateChapterData();
        }
    }

    string GetLastChapter()
    {
        return Values.loadedChapter.Count > 0 ? Values.loadedChapter[Values.loadedChapter.Count - 1] : 
            Dialogues.GetAllTree()[0];
    }

    public void UpdateValuesData()
    {
        Values.Update();
        valuesUpdate = false;
    }

    public void UpdateChapterData()
    {
        Chapter.Update();
        chapterUpdate = false;
    }

    void DefaultValuesData()
    {
        Values.speed = 3.0f;
        Values.volume = 1.0f;
        Values.autoSave = true;
        Values.loadedChapter.Add(Dialogues.GetAllTree()[0]);
    }

    void DefaultChapterData()
    {
        Chapter.loadedDialogues = new List<int>();
    }

    public void AddData()
    {
        int id = Dialogues.GetID();
        string character = Dialogues.GetOwner();

        if (!Chapter.loadedDialogues.Contains(id))
        {
            Chapter.loadedDialogues.Add(id);
            chapterUpdate = true;
        }

        if (!Values.characterList.Contains(character) && character.Trim() != "")
        {
            Values.characterList.Add(character);
            valuesUpdate = true;
        }
    }

    public void Reset(string tree)
    {
        Debug.Log(tree);
        // Delete higher chapters
        for (int i = Values.loadedChapter.IndexOf(tree); i < Values.loadedChapter.Count; i++)
        {
            MyData myData = new MyData(Values.loadedChapter[i] + GameValues.TXT);
            myData.Delete();
        }

        for (int i = Values.loadedChapter.IndexOf(tree) + 1; i < Values.loadedChapter.Count; i++)
        {
            Values.loadedChapter.RemoveAt(i);
            UpdateValuesData();
        }
    }
}
