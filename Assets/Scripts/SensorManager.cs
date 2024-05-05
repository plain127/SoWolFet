using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SensorManager : MonoBehaviour
{
    private Animator anim;
    private GameObject character;
    void Start()
    {
        if (CharacterARAppearManager.placedObject)
        {
            character = CharacterARAppearManager.placedObject;
            character.transform.rotation = Quaternion.Euler(character.transform.eulerAngles.x, 180, character.transform.eulerAngles.z);
            anim = character.GetComponent<Animator>();
        }
        //가속도센서 사용
        EnableDeviceIfAvailable(Accelerometer.current);

    }

    void Update()
    {
        //가속도 센서 값을 읽음
        Vector3 acceleration = Accelerometer.current?.acceleration.ReadValue() ?? Vector3.zero;

        //사용자가 멈춰있을때
        if (acceleration.x <= 0.05 && acceleration.y >= -1 && acceleration.z >= -1)
        {
            anim.Play("Idle");
        }
        //사용자가 걷고 있을때
        else if (acceleration.x >= 1 && acceleration.y >= -0.9 && acceleration.z >= -0.5)
        {
            anim.Play("Walk");
        }
        //사용자가 뛰고 있을때
        else if (acceleration.x >= 0.05 && acceleration.y >= -1 && acceleration.z >= -0.6)
        {
            anim.Play("Run");
        }
    }


    void EnableDeviceIfAvailable(InputDevice device)
    {
        //하드웨어에 센서가 있는지 확인
        if (device != null)
        {
            //있으면 센서값 사용
            InputSystem.EnableDevice(device);
        }
    }
}