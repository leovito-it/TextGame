using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Dialogues : MonoBehaviour
{
    Window Current;

    //Enums
    public enum WindowTypes { Text, Choice, ChoiceAnswer }
    public enum NodeType { Start, Default, End }

    [HideInInspector]
    public List<WindowSet> Set = new List<WindowSet>();
    [HideInInspector]
    public int CurrentSet = 0;
    [HideInInspector]
    public List<string> TabList = new List<string>();

    [System.Serializable]
    public class WindowSet
    {
        public int CurrentId;
        public bool NewWindowOpen;
        public int FirstWindow = -562;
        public List<Window> Windows = new List<Window>();

        public WindowSet() { CurrentId = 0; NewWindowOpen = false; }

        public Window GetWindow(int ID)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                if (Windows[i].ID == ID)
                    return Windows[i];
            }
            return null;
        }
    }

    //Window class, holds all the info a Window needs
    [System.Serializable]
    public class Window
    {
        public int ID;

        public Rect Size;
        public string Text;
        public string Owner;
        public WindowTypes Type;
        public NodeType NodeType;
        public int Parent;
        public bool Trigger;
        public string TriggerText;

        public List<int> Connections = new List<int>();

        public Window(int id, int parent, Rect newSize, WindowTypes type = WindowTypes.Text, NodeType nodeType = NodeType.Default) { ID = id; Parent = parent; Size = newSize; Text = ""; Owner = ""; Type = type; NodeType = nodeType; Trigger = false; TriggerText = ""; }

        public bool IsChoice() { if (Type == WindowTypes.Choice) return true; else return false; }
    }

    /// <summary>
    /// Set the current node back to the beginning
    /// </summary>
    /// <returns></returns>
    public string Reset()
    {
        if (CurrentSet < Set.Count)
            Current = Set[CurrentSet].Windows[Set[CurrentSet].FirstWindow];
        if (Current == null)
            return "";
        return Current.Text;
    }

    /// <summary>
    /// Sets the current tree to be used
    /// </summary>
    /// <param name="TreeName"></param>
    /// <returns></returns>
    public bool SetTree(string TreeName)
    {
        for (int i = 0; i < Set.Count; i++)
        {
            if (TabList[i] == TreeName)
            {
                CurrentSet = i;
                Reset();
                return true;
            }
        }
        return false;
    }

    public string[] GetAllTree()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < Set.Count; i++)
        {
            list.Add(TabList[i]);
        }
        return list.ToArray();
    }

    public string GetCurrentTree()
    {
        return TabList[CurrentSet];
    }

    /// <summary>
    /// Returns if you're at the end of the dialogue tree
    /// </summary>
    /// <returns></returns>
    public bool End()
    {
        if (Current.Connections.Count == 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Checks if the current window has a trigger
    /// </summary>
    /// <returns></returns>
    public bool HasTrigger()
    {
        return Current.Trigger;
    }

    /// <summary>
    /// Returns any trigger text this current window might have
    /// </summary>
    /// <returns></returns>
    public string GetTrigger()
    {
        return Current.TriggerText;
    }

    /// <summary>
    /// Moves to the next item in the list.
    /// </summary>
    /// <returns># = Amount of choices it has | 0 = success | -1 = end</returns>
    public int Next()
    {
        if (Current.Type == WindowTypes.Choice)
            return Current.Connections.Count;
        else if (Current.Connections.Count == 0)
            return -1;
        else
        {
            Current = Set[CurrentSet].GetWindow(Current.Connections[0]);
            return 0;
        }
    }

    /// <summary>
    /// Returns the choices the current node has. 
    /// </summary>
    /// <returns>null if the node isn't a decision node. An array of strings otherwise</returns>
    public string[] GetTextChoices()
    {
        if (Current.Type != WindowTypes.Choice)
            return new string[] { };
        else
        {
            List<string> Choices = new List<string>();
            for (int i = 0; i < Current.Connections.Count; i++)
            {
                Choices.Add(Set[CurrentSet].GetWindow(Current.Connections[i]).Text);
            }
            return Choices.ToArray();
        }
    }

    public int[] GetIDChoices()
    {
        if (Current.Type != WindowTypes.Choice)
            return new int[] { };
        else
        {
            List<int> Choices = new List<int>();
            for (int i = 0; i < Current.Connections.Count; i++)
            {
                Choices.Add(Set[CurrentSet].GetWindow(Current.Connections[i]).ID);
            }
            return Choices.ToArray();
        }
    }

    public void SetTrigger(int id)
    {
        Set[CurrentSet].GetWindow(id).Trigger = true;
    }

    public int GetTriggerChoise()
    {
        for (int i = 0; i < Current.Connections.Count; i++)
        {
            if (Set[CurrentSet].GetWindow(Current.Connections[i]).Trigger)
                return Set[CurrentSet].GetWindow(Current.Connections[i]).ID;
        }
        return int.MaxValue;
    }

    /// <summary>
    /// Moves to a window
    /// </summary>
    /// <param name="choice"></param>
    /// <returns></returns>
    public void MoveTo(int id)
    {
        Current = Set[CurrentSet].GetWindow(id);
    }

    public string GetCurrentDialogue()
    {
        return Current.Text;
    }

    public string GetOwner()
    {
        return Current.Owner;
    }

    public int GetID()
    {
        return Current.ID;
    }

    public void ClearAllTrigger()
    {
        foreach (Window w in Set[CurrentSet].Windows)
        {
            w.Trigger = false;
        }
    }

}
