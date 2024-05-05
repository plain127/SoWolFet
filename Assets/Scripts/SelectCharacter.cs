using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    //캐릭터 리스트
    public List<GameObject> selectCharacterList;

    //캐릭터 선택 버튼 
    public List<Button> selectButtonList;

    void Start()
    {
        for (int i = 0; i < selectButtonList.Count; i++)
        {
            AddListenerToButton(selectButtonList[i], i);
        }
    }

    void AddListenerToButton(Button button, int index)
    {
        button.onClick.AddListener(() => Select(index));
    }

    void Select(int characterIndex)
    {

        string name = selectCharacterList[characterIndex].name;
        //선택한 캐릭터 저장
        PlayerPrefs.SetInt(name, characterIndex);
        SceneManager.LoadScene("Main");
    }
}
