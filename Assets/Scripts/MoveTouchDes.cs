using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MoveTouchDes : MonoBehaviour
{
    public GameObject indicator;
    public static bool isARMove = false;
    private Vector3 des;
    public float speed = 0.5f;
    Animator anim;
    void Start()
    {
        indicator.SetActive(false);
    }

    void Update()
    {
        ClickAR();

        MoveDes();
    }

    void ClickAR()
    {
        //클릭한 위치로 캐릭터가 이동(AR)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (ARRayManager.Raycast(touch.position, out Pose hitPos))
                {
                    indicator.transform.position = hitPos.position;
                    indicator.SetActive(true);
                    des = indicator.transform.position;
                    isARMove = true;
                }
            }
            else
            {
                indicator.SetActive(false);
                isARMove = false;
            }
        }
    }
    void MoveDes()
    {
        if (isARMove)
        {
            anim = CharacterARAppearManager.placedObject.transform.GetComponentInChildren<Animator>();
            if (Vector3.Distance(des, CharacterARAppearManager.placedObject.transform.position) <= 0.1f)
            {
                indicator.SetActive(false);
                anim.Play("Idle");
                CharacterARAppearManager.placedObject.transform.rotation = Quaternion.Euler(0, 180f, 0);
                isARMove = false;
            }
            else
            {
                Vector3 dir = des - CharacterARAppearManager.placedObject.transform.position;
                CharacterARAppearManager.placedObject.transform.forward = dir;
                anim.Play("Walk");
                CharacterARAppearManager.placedObject.transform.position += dir.normalized * speed * Time.deltaTime;
            }
        }
    }
}
