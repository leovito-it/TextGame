using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatboxCreator : MonoBehaviour
{
    public GameObject spCharacter_template;
    public GameObject mainCharacter_template;
    public GameObject choises_template;
    public GameObject narrator_template;
    public RectTransform site, parentOfSite;
    
    TextMeshProUGUI input;

    GameObject template;
    public float space = 100f;

    string lastName = null; // remember last person
    ChatboxType type;

    public Button Create(Character character, Sprite avatar)
    {
        if ( input == null )
            input = gameObject.AddComponent<TextMeshProUGUI>();
        input.SetText(character.speech);

        GetSuitableTemplate();

        GameObject chatbox = Instantiate(template, site);

        foreach (TextMeshProUGUI textMesh in chatbox.GetComponentsInChildren<TextMeshProUGUI>())
        {
            // Show character speech
            if (MyString.Compare(textMesh.tag, GameValues.CONTENT_TAG))
            {
                textMesh.text = character.speech;
                continue;
            }
            // Show character name
            if (MyString.Compare(textMesh.tag, GameValues.CHARACTER_NAME_TAG))
            {
                textMesh.text = character.characterName;
                continue;
            }
        }

        // Set object iamge (act as an Character's Avatar)
        Image chImg = null;

        foreach (Image img in chatbox.GetComponentsInChildren<Image>())
        {
            if (MyString.Compare(img.gameObject.name, "avatar"))
                chImg = img;
        }

        if (chImg != null)
            chImg.sprite = avatar == null ? character.avatar : avatar;

        // Delete unnecessary image object
        if (MyString.Compare(character.characterName, lastName))
        {
            GameObject image = chatbox.transform.Find("avatar").gameObject;
            if (image != null)
                Destroy(image);
        }

        lastName = character.characterName;

        if (type != ChatboxType.Narrator)
            Resize(ref chatbox);
        ReportNews();

        return type == ChatboxType.Choises ? chatbox.GetComponent<Button>() : null;
    }

    void GetSuitableTemplate()
    {
        template = type == ChatboxType.MainCharacter ? mainCharacter_template :
            type == ChatboxType.SupportCharacter ? spCharacter_template :
            type == ChatboxType.Choises ? choises_template :
            type == ChatboxType.Narrator ? narrator_template : template;
    }

    void Resize(ref GameObject output)
    {
        Vector2 textSize = Wrapping(input, 600);
        Vector2 padding = new Vector2(0, 40);

        output.GetComponent<RectTransform>().sizeDelta = new Vector2(0, textSize.y) + padding;
        foreach (RectTransform rec in output.GetComponentsInChildren<RectTransform>())
        {
            if (MyString.Compare(rec.tag, GameValues.CONTENT_SITE_TAG))
                rec.sizeDelta = new Vector2(textSize.x, 0);
        }
    }

    Vector2 Wrapping(TextMeshProUGUI input, float maxLength)
    {
        Vector2 textSize = input.GetPreferredValues();
        float charLength = textSize.x;
        float charHeight = textSize.y;

        if (charLength <= 0) charLength = 200;
        if (charHeight <= 0 || charHeight < 111) charHeight = 100;

        return charLength > maxLength ? new Vector2(maxLength, charHeight * (charLength / maxLength))
            : new Vector2(charLength, charHeight);
    }

    void ReportNews()
    {
        // report by change site position
        float new_y = site.sizeDelta.y - parentOfSite.sizeDelta.y;
        if (new_y > -100)
            site.transform.localPosition = new Vector3(0, new_y + space, 0);
    }

    public void ClearLastName()
    {
        lastName = null;
    }

    public void CleanSite()
    {
        for (int i = 0; i < site.childCount; i++)
        {
            Destroy(site.GetChild(i).gameObject);
        }
    }

    public void SetType(ChatboxType type)
    {
        this.type = type;
    }

    public enum ChatboxType { MainCharacter, SupportCharacter, Choises, Narrator }
}
