using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IData
{
    public void Save(string data);
    public string Load();
    public void Update(string data);
    public void Delete();
}

public class MyData : IData
{
    public string LoadPath { get; set; }

    public MyData(string fileName)
    {
        LoadPath = GetPath(fileName);
    }

    public bool IsExists()
    {
        if (Load() == null)
        {
            StreamWriter sw = new StreamWriter(LoadPath);
            sw.Write("");
            sw.Close();
            return false;
        }
        return true;
    }

    public static string GetPath(string fileName)
    {
        string local = "";

        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                local = Application.persistentDataPath;
                break;
            case RuntimePlatform.Android:
                local = Application.temporaryCachePath;
                break;
            default:
                local = Application.dataPath;
                break;
        }

        return local + "/" + fileName;
    }

    public string Load()
    {
        string data;
        if (!File.Exists(LoadPath))
            return null;

        StreamReader sr = new StreamReader(LoadPath);
        data = sr.ReadLine();
        sr.Close();

        return data;
    }

    public void Save(string data)
    {
        StreamWriter sw = new StreamWriter(LoadPath);
        sw.Write(data);
        sw.Close();
    }

    public void Update(string data)
    {
        string currentData = Load();

        if (currentData != null)
            if (currentData.Trim().Equals(data.Trim()))
                return;

        Save(data);
    }

    public void Delete()
    {
        if (File.Exists(LoadPath))
            File.Delete(LoadPath);
    }
}

[System.Serializable]
public class ValuesData : MyData
{
    public List<string> loadedChapter;
    public List<string> characterList;
    public float volume;
    public float speed;
    public bool autoSave;

    public ValuesData(string loadPath) : base(loadPath)
    {
        loadedChapter = new List<string>();
        characterList = new List<string>();
        volume = 1.0f;
        speed = 3.0f;
        autoSave = true;
    }

    public void Update()
    {
        Update(JsonUtility.ToJson(this));
    }
}

[System.Serializable]
public class ChapterData : MyData
{
    public List<int> loadedDialogues;

    public ChapterData(string loadPath) : base(loadPath)
    {
        loadedDialogues = new List<int>();
    }

    public void Update()
    {
        Update(JsonUtility.ToJson(this));
    }
}
