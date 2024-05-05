using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    public List<GameObject> characterList;

    void Start()
    {
        foreach (GameObject character in characterList)
        {
            string name = character.name;
            if (PlayerPrefs.HasKey(name))
            {
                int characterIndex = PlayerPrefs.GetInt(name);
                Instantiate(characterList[characterIndex], transform.position, transform.rotation, transform);
                //PlayerPrefs.DeleteAll();
                break;
            }
        }
    }
}
