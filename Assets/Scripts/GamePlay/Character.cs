using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Character", menuName = "Character/Create")]
public class Character : ScriptableObject
{
    public string characterName;
    public Sprite avatar;
    public string speech;
    public bool unlock;

    public Character(string charactorName, Sprite avatar)
    {
        this.characterName = charactorName;
        this.avatar = avatar;
        this.unlock = false;
    }
}
