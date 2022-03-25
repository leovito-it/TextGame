using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    public List<Character> characters;
    public GameObject template;
    public Sprite locked;

    public RectTransform site;

    DataManager data;

    public void Start()
    {
        CreateView();
        StartCoroutine(UnlockCharacters());
    }

    void CreateView()
    {
        foreach (Character character in characters)
        {
            GameObject characterObject = Instantiate(template, site);
            characterObject.name = character.characterName;
            characterObject.transform.GetChild(0).Find("avatar").GetComponent<Image>().sprite = locked;
        }
    }

    void Unlock(Character ch)
    {
        ch.unlock = true;
        site.Find(ch.characterName.Trim()).transform.GetChild(0).Find("avatar").GetComponent<Image>().sprite = ch.avatar;
    }

    IEnumerator UnlockCharacters()
    {
        data = FindObjectOfType<GameController>().data;
        while (true)
        {
            foreach (string character in data.Values.characterList)
            {
                foreach (Character ch in characters)
                {
                    if (site == null)
                        break;

                    if (MyString.Compare(ch.characterName, character))
                        Unlock(ch);
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
