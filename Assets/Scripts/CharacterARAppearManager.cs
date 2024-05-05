using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CharacterARAppearManager : MonoBehaviour
{
    public List<GameObject> characterList;
    GameObject character;
    [SerializeField]
    private Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    public static GameObject placedObject;
    float relocationDistance = 1f;
    Vector3 desVel = Vector3.zero;
    bool movePoint = MoveTouchDes.isARMove;
    //애니메이션은 임시
    Animator anim;
    float time;
    void Start()
    {
        CheckingCharacter();
        StartCoroutine("Appear");
    }

    void Update()
    {
        StartCoroutine("Appear");

        Ani();
    }

    //Select씬에서 선택한 캐릭터 => DB에 저장된 유저 계정의 캐릭터로 가져오기
    bool CheckingCharacter()
    {
        foreach (GameObject selectcharacter in characterList)
        {
            string name = selectcharacter.name;
            if (PlayerPrefs.HasKey(name))
            {
                int characterIndex = PlayerPrefs.GetInt(name);
                character = characterList[characterIndex];
                break;
            }
        }
        return true;
    }

    //코루틴으로 선택한 캐릭터 정보를 받고 
    //평면 인식이 완료된 곳에 캐릭터 생성
    IEnumerator Appear()
    {
        yield return new WaitUntil(() => CheckingCharacter() == true);

        if (ARRayManager.Raycast(screenSize, out Pose hitPose))
        {
            hitPose.rotation = Quaternion.Euler(0, 180f, 0);
            if (placedObject == null)
            {
                placedObject = Instantiate(character, hitPose.position, hitPose.rotation);
                placedObject.transform.localScale = new Vector3(1f, 1f, 1f);
                anim = placedObject.transform.GetComponentInChildren<Animator>();
            }
            else
            {
                if (movePoint == false)
                {
                    if (Vector3.Distance(placedObject.transform.position, hitPose.position) > relocationDistance)
                    {
                        placedObject.transform.position = Vector3.SmoothDamp(transform.position, hitPose.position, ref desVel, 1f);
                    }
                }
            }
        }

    }

    void Ani()
    {
        time += Time.deltaTime;

        if (anim != null)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                if (time < 10)
                {
                    anim.Play("Idle");
                }
                else if (time >= 10 && time < 12)
                {
                    anim.Play("Sitting_start");
                }
                else if (time >= 12 && time < 18)
                {
                    anim.Play("Sitting");
                }
                else if (time >= 18 && time < 20)
                {
                    anim.Play("Sitting_end");
                }
                else if (time >= 20 && time < 27)
                {
                    anim.Play("Idle_6");

                }
                else if (time >= 27 && time < 30)
                {
                    anim.Play("RightHand");
                }
                else if (time >= 30 && time < 33)
                {
                    anim.Play("Attack");
                }
                else if (time >= 33 && time < 35)
                {
                    anim.Play("Lie_start");
                }
                else if (time >= 35 && time < 42)
                {
                    anim.Play("Lie");
                }
                else if (time >= 42 && time < 44)
                {
                    anim.Play("Lie_end");
                }
                else if (time >= 44 && time < 46)
                {
                    anim.Play("Digging_start");
                }
                else if (time >= 46 && time < 53)
                {
                    anim.Play("Digging");
                }
                else if (time >= 53 && time < 55)
                {
                    anim.Play("Digging_end");
                    time -= 55;
                }
            }

        }
    }
}
