using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //자이로센서 사용
    private bool gyroEnabled;
    private Gyroscope gyroscope;

    //자이로센서 사용시 카메라 오브젝트, 각도
    private GameObject cameraContainer;
    private Quaternion rot;

    //카메라 드래그 속도
    private float dragSpeed = 0.1f;

    //카메라 고정 좌표
    private float fixY = 10f;
    private float minX = 14f;
    private float maxX = 57f;
    private float minZ = -13f;
    private float maxZ = 43f;

    //카메라 좌표 이동 계산 변수
    private Vector2 prePos, nowPos;
    private Vector3 movePos;

    //카메라 회전 값 저장 변수
    private Quaternion lastTouchRotation = Quaternion.identity;

    //카메라의 FieldOfView의 기본값
    float fieldView = 60f;
    private void Start()
    {
        //자이로센서 사용을 위해 부모 오브젝트생성
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);
        gyroEnabled = EnableGyro();

    }

    //자이로센서 사용
    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyroscope = Input.gyro;
            gyroscope.enabled = true;
            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0);
            rot = new Quaternion(0, 0, 1, 0);
            return true;
        }
        return false;
    }

    void Update()
    {
        //자이로센서 회전값과 유니티 회전값 조율
        Quaternion gyroRotation = Quaternion.Euler(90, 0, 0) * new Quaternion(gyroscope.attitude.x, gyroscope.attitude.y, -gyroscope.attitude.z, -gyroscope.attitude.w);

        //자이로센서 업데이트
        if (gyroEnabled && Input.touchCount == 0)
        {
            cameraContainer.transform.localRotation = lastTouchRotation * gyroRotation;
        }
        //터치시 수평회전
        if (Input.touchCount == 1)
        {
            gyroEnabled = false;

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position;
                float deltaX = nowPos.x - prePos.x;
                Quaternion touchDeltaRotation = Quaternion.Euler(0, -deltaX * dragSpeed, 0);
                lastTouchRotation *= touchDeltaRotation;

                cameraContainer.transform.localRotation = lastTouchRotation * gyroRotation;

                prePos = touch.position;
            }
            else if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                gyroEnabled = true;
            }
        }
        //터치시 카메라 위치 이동
        if (Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position;
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * dragSpeed;
                movePos.y = 0; // y 축에서의 이동은 없음

                transform.Translate(movePos);
                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, minX, maxX),
                    fixY,
                    Mathf.Clamp(transform.position.z, minZ, maxZ)
                );

                prePos = touch.position;
            }
        }

        // 카메라 줌인/아웃
        if (Input.touchCount == 2)
        {
            Vector2 prePos0 = Input.touches[0].position - Input.touches[0].deltaPosition;
            Vector2 prePos1 = Input.touches[1].position - Input.touches[1].deltaPosition;

            float preDis = (prePos0 - prePos1).magnitude;
            float nowDis = (Input.touches[0].position - Input.touches[1].position).magnitude;

            float resDis = preDis - nowDis;

            fieldView += (resDis * dragSpeed);

            fieldView = Mathf.Clamp(fieldView, 20f, 100f);

            Camera.main.fieldOfView = fieldView;

        }

    }
}