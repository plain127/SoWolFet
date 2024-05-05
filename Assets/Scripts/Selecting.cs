using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Selecting : MonoBehaviour
{
    //색상이 변경될 오브젝트 리스트
    public List<Transform> backList;

    //캐릭터 리스트
    public List<Transform> selectList;

    //이전 클릭한 오브젝트 저장 변수
    private Renderer preRenderer;
    //원래 색 저장 변수
    private Color originColor;

    //클릭한 캐릭터
    public Transform stageCharacter;

    public Transform clickCharacter;

    // 캐릭터가 변경되었을 때 알리는 이벤트
    public event Action OnCharacterChanged;
    void Update()
    {
        //레이캐스트를 쏘아 캐릭터나 케이스를 터치하면 Back 오브젝트 색이 바뀜
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                for (int i = 0; i < selectList.Count; i++)
                {
                    if (hit.collider.name == selectList[i].name)
                    {
                        Renderer currentRenderer = backList[i].GetComponent<Renderer>();

                        if (preRenderer != null)
                        {
                            preRenderer.material.color = originColor;
                        }

                        if (stageCharacter != null)
                        {
                            Destroy(stageCharacter.gameObject);
                        }
                        preRenderer = currentRenderer;
                        originColor = currentRenderer.material.color;
                        currentRenderer.material.color = new Color(204 / 255f, 51 / 255f, 204 / 255f);

                        //미니어처 캐릭터 소환
                        Vector3 selectPos = new Vector3(0.08f, 3f, 1.6f);
                        Quaternion selectRot = Quaternion.Euler(18f, 180f, 0);
                        Vector3 selectScale = new Vector3(0, 0, 0);

                        if (selectList[i].name == "Kitty")
                        {
                            selectScale = new Vector3(2f, 2f, 2f);
                        }
                        else if (selectList[i].name == "Puppy")
                        {
                            selectScale = new Vector3(5f, 5f, 5f);
                        }
                        else if (selectList[i].name == "RealDog")
                        {
                            selectScale = new Vector3(3f, 3f, 3f);
                        }
                        else if (selectList[i].name == "RealCat")
                        {
                            selectScale = new Vector3(2.5f, 2.5f, 2.5f);
                        }
                        else if (selectList[i].name == "Corgi")
                        {
                            selectScale = new Vector3(0.8f, 0.8f, 0.8f);
                            selectRot = Quaternion.Euler(-72f, 180f, 0);
                        }
                        else if (selectList[i].name == "Humanoid")
                        {
                            selectScale = new Vector3(0.2f, 0.2f, 0.2f);
                        }

                        stageCharacter = Instantiate(selectList[i], selectPos, selectRot);
                        stageCharacter.localScale = selectScale;
                        stageCharacter.parent = clickCharacter;
                        OnCharacterChanged?.Invoke();
                        break;
                    }
                }
            }
        }

    }
}
