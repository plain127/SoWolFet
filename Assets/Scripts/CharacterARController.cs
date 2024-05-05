using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterARController : MonoBehaviour
{   
    //회전속도 조절 변수
    public float rotSpeed = 0.1f;

    void Update()
    {
        //스크린 터치가 1회 이상 들어오면
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //터치 상태가 움직이면
            if(touch.phase == TouchPhase.Moved)
            {
                //카메라에서 레이를 발사해 부딪힌 대상이 8번 레이어면 터치 이동거리를 구한다.
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                RaycastHit hitInfo;

                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1<<8))
                {
                    Vector3 delataPos = touch.deltaPosition;

                    //직전 프레임에서 현재 프레임까지 X축 이동량에 비례해 로컬 Y축으로 회전
                    transform.Rotate(transform.up, delataPos.x * -1.0f);
                }
            }
        }
    }
}
