using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Security.Cryptography;
using Unity.AI.Navigation;
//using UnityEditor.Timeline;
using Unity.XR.CoreUtils;
//using UnityEngine.UIElements;

public class CharacterAction : MonoBehaviour
{
    //애니메이터 변수
    Animator anim;

    //캐릭터 액션 오브젝트 변수
    private Transform action;
    private List<Transform> actionList = new List<Transform>();

    //되돌아오는 시간 변수
    private float backIdleTime;

    //캐릭터 액션 상태 상수
    enum CharacterActionState
    {
        Walk,
        Idle,
        Sitting,
        LeftHand,
        RightHand,
        Lie,
        Attack,
        Jump

    };

    //캐릭터 액션 상태 변수
    CharacterActionState c_State;

    //캐릭터 자동로밍 위치 변수
    public Transform roamRoute;
    public List<Transform> locations;
    private int locationIndex;

    // NavMeshAgent 컴포넌트 저장할 필드
    private NavMeshAgent agent;

    //채팅 버튼
    public Button chatStart;

    private RaycastHit hit;
    private float rayLength = 10.0f;

    int layerMask;
    void Start()
    {
        backIdleTime = 0;
        //자식 오브젝트로부터 애니메이터 변수 받아오기
        StartCoroutine(InitializeAnimWhenReady());

        //로밍 위치 참조
        RoamRoute();

        //NavMeshAgent 컴포넌트를 찾아 반환
        agent = GetComponent<NavMeshAgent>();

        //최초의 캐릭터 상태는 방을 걸어다니는(Walk) 상태로 한다.
        c_State = CharacterActionState.Walk;

        //채팅 버튼 클릭시 애니메이션 상태 전환
        chatStart.onClick.AddListener(BtnClick);

        //자식 오브젝트 터치 콜라이더 가져오기
        StartCoroutine(InitializeActionReady());

        layerMask = 1 << LayerMask.NameToLayer("Character");
    }
    IEnumerator InitializeAnimWhenReady()
    {
        // 자식 오브젝트가 존재할 때까지 대기
        yield return new WaitUntil(() => transform.childCount > 0);
        anim = transform.GetComponentInChildren<Animator>();
    }

    IEnumerator InitializeActionReady()
    {
        yield return new WaitUntil(() => transform.childCount > 0);
        //리스트에 터치 콜라이더 오브젝트 가져오기
        action = transform.GetChild(0).Find("Action");
        if (action != null)
        {
            foreach (Transform child in action)
            {
                actionList.Add(child);
            }
        }
    }
    void Update()
    {
        //현재 상태 체크 후 상태 별로 정해진 기능 수행
        switch (c_State)
        {
            case CharacterActionState.Walk:
                Walk();
                break;
            case CharacterActionState.Idle:
                Idle();
                break;
            case CharacterActionState.Sitting:
                backIdleTime += Time.deltaTime;
                Sitting();
                break;
            case CharacterActionState.LeftHand:
                backIdleTime += Time.deltaTime;
                LeftHand();
                break;
            case CharacterActionState.RightHand:
                backIdleTime += Time.deltaTime;
                RightHand();
                break;
            case CharacterActionState.Lie:
                backIdleTime += Time.deltaTime;
                Lie();
                break;
            case CharacterActionState.Attack:
                backIdleTime += Time.deltaTime;
                Attack();
                break;
        }

        //Off Mesh Link를 지나갈때
        if (agent.isOnOffMeshLink)
        {
            Jump();
        }

        //터치가 1번 이상 발생 시
        if (Input.touchCount > 0)
        {
            Debug.Log("Screen touched");
            //레이캐스트 설정함수
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            //터치시 레이를 발사
            if (Physics.Raycast(ray, out hit, rayLength, layerMask))
            {
                Debug.Log("Raycast hit: " + hit.transform.name);
                for (int i = 0; i < actionList.Count; i++)
                {
                    if (Vector3.Distance(hit.transform.localPosition, actionList[i].localPosition) < 1.0f)
                    {
                        Vector3 distance = actionList[i].localPosition - transform.localPosition;
                        actionList[i].localPosition = transform.localPosition + distance;

                        switch (i)
                        {
                            case 0:
                                Debug.Log("Hit");
                                anim.SetTrigger("WalkToSitting");
                                c_State = CharacterActionState.Sitting;
                                break;
                            case 1:
                                Debug.Log("Hit");
                                anim.SetTrigger("WalkToLeftHand");
                                c_State = CharacterActionState.LeftHand;
                                break;
                            case 2:
                                Debug.Log("Hit");
                                anim.SetTrigger("WalkToRightHand");
                                c_State = CharacterActionState.RightHand;
                                break;
                            case 3:
                                Debug.Log("Hit");
                                anim.SetTrigger("WalkToLie");
                                c_State = CharacterActionState.Lie;
                                break;
                            case 4:
                                Debug.Log("Hit");
                                anim.SetTrigger("WalkToAttack");
                                c_State = CharacterActionState.Attack;
                                break;
                        }

                        break;
                    }
                }
            }
        }

    }

    void Jump()
    {
        //바닥에서 올라갈때
        if (transform.position.y > 0 && transform.position.y < 1)
        {
            anim.SetTrigger("WalkToJumpUp");
            StartCoroutine(AnimationCallback("Walk"));
        }
        //바닥으로 내려갈때
        else if (transform.position.y > 1)
        {
            anim.SetTrigger("WalkToJumpDown");
            StartCoroutine(AnimationCallback("Walk"));
        }
    }

    private IEnumerator AnimationCallback(string animName)
    {
        yield return new WaitForSeconds(0.5f);
        anim.Play(animName);
    }
    void RoamRoute()
    {
        //로밍 위치 참조
        foreach (Transform child in roamRoute)
        {
            locations.Add(child);
        }
    }

    void Walk()
    {
        if (agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            if (locations.Count == 0)
            {
                return;
            }

            //자동으로 배경 내를 배회함
            agent.destination = locations[locationIndex].position;

            //캐릭터의 정면방향과 Navi의 방향을 일치
            Vector3 dir = (locations[locationIndex].position - transform.position).normalized;
            transform.forward = dir;

            //리스트 인덱스 값을 랜덤으로 지정
            locationIndex = Random.Range(0, locations.Count);

        }

    }

    void BtnClick()
    {
        //배회상태에서 채팅버튼 클릭시
        if (c_State == CharacterActionState.Walk)
        {
            WalkChatClick();
        }
        //채팅상태에서 채팅버튼 클릭시
        else if (c_State == CharacterActionState.Idle)
        {
            IdleChatClick();
        }
    }

    //자리에 멈추고 Idle상태로 바뀜
    void WalkChatClick()
    {
        agent.destination = transform.position;
        c_State = CharacterActionState.Idle;
        anim.SetTrigger("WalkToIdle");
    }

    //Idle상태에서 다시 배회하는 상태로 바뀜
    void IdleChatClick()
    {
        c_State = CharacterActionState.Walk;
        anim.SetTrigger("IdleToWalk");
    }

    void Idle()
    {
        //터치시 랜덤 행동
    }

    void Sitting()
    {
        if (backIdleTime > 1.5)
        {
            c_State = CharacterActionState.Walk;
            anim.SetTrigger("SittingToWalk");
            backIdleTime = 0;
        }
    }

    void LeftHand()
    {
        if (backIdleTime > 1.5)
        {
            c_State = CharacterActionState.Walk;
            anim.SetTrigger("LeftHandToWalk");
            backIdleTime = 0;
        }
    }

    void RightHand()
    {
        if (backIdleTime > 1.5)
        {
            c_State = CharacterActionState.Walk;
            anim.SetTrigger("RightHandToWalk");
            backIdleTime = 0;
        }
    }

    void Lie()
    {
        if (backIdleTime > 1.5)
        {
            c_State = CharacterActionState.Walk;
            anim.SetTrigger("LieToWalk");
            backIdleTime = 0;
        }
    }

    void Attack()
    {
        if (backIdleTime > 1.5)
        {
            c_State = CharacterActionState.Walk;
            anim.SetTrigger("AttackToWalk");
            backIdleTime = 0;
        }
    }
}