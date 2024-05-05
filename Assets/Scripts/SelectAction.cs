using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectAction : MonoBehaviour
{
    Selecting selecting;
    public GameObject mainCamera;
    public List<Transform> locations;

    private UnityEngine.AI.NavMeshAgent agent;
    private int currentLocationIndex = 0; // 현재 위치 인덱스를 추적하는 변수

    Animator anim;

    float time;

    void Start()
    {
        selecting = mainCamera.GetComponent<Selecting>();  // GameObject 키워드 제거
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (locations.Count > 0)
        {
            agent.destination = locations[currentLocationIndex].position;
        }

        agent.speed = 0.5f;

        StartCoroutine(InitializeAnimWhenReady());
        if (selecting != null)
        {
            selecting.OnCharacterChanged += ResetAnimationState;
        }
    }

    IEnumerator InitializeAnimWhenReady()
    {

        yield return new WaitUntil(() => transform.childCount > 0);
        anim = transform.GetComponentInChildren<Animator>();

    }

    private void OnDestroy()
    {
        if (selecting != null)
        {
            selecting.OnCharacterChanged -= ResetAnimationState;
        }
    }

    void ResetAnimationState()
    {
        Debug.Log("Resetting Animation State...");
        time = 0;
        if (anim != null)
        {
            Debug.Log("Playing Walk Animation");
            anim.Play("Walk");
        }
        else
        {
            Debug.Log("Animator is null");
        }
    }
    void Update()
    {
        if (selecting == null)
        {
            return;
        }
        if (selecting.stageCharacter != null)
        {
            if (agent.remainingDistance < 0.1f && !agent.pathPending)
            {
                MoveToNextLocation();
            }
        }

        StartCoroutine(InitializeAnimWhenReady());
        if (selecting != null)
        {
            selecting.OnCharacterChanged += ResetAnimationState;
        }
        Animation();

    }

    void MoveToNextLocation()
    {
        currentLocationIndex++;

        if (currentLocationIndex >= locations.Count)
        {
            currentLocationIndex = 0;
        }

        Vector3 dir = (locations[currentLocationIndex].position - transform.position).normalized;
        transform.forward = dir;

        agent.destination = locations[currentLocationIndex].position;
    }

    void Animation()
    {
        time += Time.deltaTime;

        if (anim != null)
        {
            if (time >= 3 && time < 3.5)
            {
                anim.Play("JumpUp");

            }
            else if (time >= 3.5 && time < 6)
            {
                anim.Play("Walk");

            }
            else if (time >= 6 && time < 7)
            {
                anim.Play("Sitting");

            }
            else if (time >= 7 && time < 9)
            {
                anim.Play("Walk");

            }
            else if (time >= 9 && time < 11)
            {
                anim.Play("Attack");

            }
            else if (time >= 11 && time < 12)
            {
                anim.Play("Walk");

            }
            else if (time >= 12)
            {
                anim.Play("Run");
                time -= 12;
            }
        }
    }
}
