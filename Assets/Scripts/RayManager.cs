using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayManager : MonoBehaviour
{
    RaycastHit hit;
    float layDistance = 15f;
    public LayerMask layerMask;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * layDistance, Color.blue, 0.3f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, layDistance))
        {
            //레이캐스트를 쏳아 닿은 부위의 콜라이더를 활성시켜 애니메이션을 작동하게함

            //카메라와 캐릭터사이의 벽을 투명화시킴

        }
    }
}
