using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ChatboxCreator))]
[RequireComponent(typeof(CharacterManager))]
public class Conversation : MonoBehaviour
{
    public CharacterManager characterManager;
    public ChatboxCreator chatbox;
    public float Speed { get; set; }

    private string[] choises = new string[2];
    private Button[] buttons = new Button[2];

    DataManager data;

    private bool needDecided = false;

    public void PreLoad()
    {
        data = FindObjectOfType<GameController>().data;
        data.GetChapterData();

        for (int i = 0; i < data.Chapter.loadedDialogues.Count; i++)
        {
            data.Dialogues.MoveTo(data.Chapter.loadedDialogues[i]);
            CreateChatbox();
        }
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        Debug.Log(Speed.ToString());
        while (!data.Dialogues.End())
        {
            int next = data.Dialogues.Next();

            if (next == 0)
            {
                CreateChatbox();
                data.AddData();
            }
            else
            {
                if (!needDecided)
                {
                    needDecided = true;
                    CreateChoices();
                }
            }
            yield return new WaitForSeconds(Speed);
        }

        if (data.Dialogues.End())
            EndChapterAction();
    }

    void CreateChatbox()
    {
        string dialog = data.Dialogues.GetCurrentDialogue();

        Character owner = GetCharacter(data.Dialogues.GetOwner(), dialog);
        chatbox.SetType(GetType(owner));
        chatbox.Create(owner, null);
    }

    void EndChapterAction()
    {
        Debug.LogWarning("Chapter end");
        chatbox.CleanSite();

        string nextChapter = data.Dialogues.GetCurrentDialogue();

        if (nextChapter != "")
        {
            data.Values.loadedChapter.Add(nextChapter);
            ValuesReset();
            PreLoad();
        }
        else
        {
            DontDestroyOnLoad(FindObjectOfType<GameController>());
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

    void CreateChoices()
    {
        choises = data.Dialogues.GetTextChoices();
        StartCoroutine(Draw());
    }

    IEnumerator Draw()
    {
        for (int index = 0; index < data.Dialogues.GetTextChoices().Length; index++)
        {
            if (index >= 2)
                break;

            chatbox.SetType(ChatboxCreator.ChatboxType.Choises);
            buttons[index] = chatbox.Create(GetCharacter(GameValues.MAIN_CHARACTER_NAME, choises[index]), null);
            SetOnClick(index);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    void SetOnClick(int index)
    {
        buttons[index].onClick.AddListener(delegate
        {
            needDecided = false;
            foreach (Button b in buttons)
            {
                if (b != null)
                    Destroy(b.gameObject);
            }

            int id = data.Dialogues.GetIDChoices()[index];
            data.Dialogues.MoveTo(id);
            ValuesReset();
            CreateChatbox();
        });
    }

    Character GetCharacter(string name, string speech)
    {
        Character character = new Character("", null);

        foreach (Character ch in characterManager.characters)
        {
            if (MyString.Compare(name, ch.characterName))
                character = ch;
        }

        character.speech = speech;
        return character;
    }

    ChatboxCreator.ChatboxType GetType(Character ch)
    {
        // if is not choises
        if (!needDecided)
        {
            // narrator if non-character
            if (MyString.Compare(ch.characterName, ""))
                return ChatboxCreator.ChatboxType.Narrator;
            // main character if equals
            if (MyString.Compare(ch.characterName, GameValues.MAIN_CHARACTER_NAME))
                return ChatboxCreator.ChatboxType.MainCharacter;
            else
                return ChatboxCreator.ChatboxType.SupportCharacter;
        }
        else
        {
            return ChatboxCreator.ChatboxType.Choises;
        }
    }

    void ValuesReset()
    {
        choises = new string[2];
        buttons = new Button[2];
        chatbox.ClearLastName();
    }
}
