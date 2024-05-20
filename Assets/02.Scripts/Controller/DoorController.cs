using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    Color tempColor;
    [SerializeField]
    Image doorOpen;
    [SerializeField]
    Image doorClose;
    [SerializeField]
    Text doorOpenTimeTxt;
    [SerializeField]
    Text doorCloseTimeTxt;
    Animator doorAnim;

    int min, sec;
    bool isOpen = true;

    float openTime = 20f; // 20초간 열려있음
    float closeTime = 40f; // 40초간 닫혀있음;

    //float intialTime = 10f;
    private void Awake()
    {
        doorAnim = GetComponent<Animator>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        doorAnim.SetBool("isOpen", isOpen == true);
        DoorTimer();

    }

    //void InitialDoor()
    //{
    //    intialTime -= Time.deltaTime;
    //    if (intialTime <= 0)
    //    {
    //        DoorTimer();
    //    }
    //}

    void DoorTimer()
    {
        if (isOpen == true)
        {
            tempColor = doorOpen.color;
            tempColor.a = 1f;
            doorOpen.color = tempColor;

            tempColor = doorClose.color;
            tempColor.a = 0f;
            doorClose.color = tempColor;

            //open 시각화
            doorOpenTimeTxt.color = new Color(255, 255, 255, 255);

            min = Mathf.FloorToInt(openTime / 60);
            sec = Mathf.FloorToInt(openTime % 60);
            doorOpenTimeTxt.text = min.ToString("00") + ":" + sec.ToString("00");


            //close 숨기기
            doorCloseTimeTxt.color = new Color(255, 255, 255, 0);


            //tempColor = doorOpenTimeTxt.color;
            //tempColor.a = 0f;
            //doorOpenTimeTxt.color = tempColor;

            openTime -= Time.deltaTime;

            if (openTime <= 0)
            {
                isOpen = false;
                openTime = 20f;
            }
        }
        else
        {
            tempColor = doorOpen.color;
            tempColor.a = 0f;
            doorOpen.color = tempColor;

            tempColor = doorClose.color;
            tempColor.a = 1f;
            doorClose.color = tempColor;

            // close 색깔 되돌리기(화이트)
            doorCloseTimeTxt.color = new Color(255, 255, 255, 255);

            min = Mathf.FloorToInt(closeTime / 60);
            sec = Mathf.FloorToInt(closeTime % 60);
            doorCloseTimeTxt.text = min.ToString("00") + ":" + sec.ToString("00");


            // open 숨기기(r, g, b, a)
            doorOpenTimeTxt.color = new Color(255, 255, 255, 0);
                //Color.Lerp(doorCloseTimeTxt.color, tempColor, 0.1f);
                //tempColor = doorOpenTimeTxt.color;
                //tempColor.a = 0f;
                //doorOpenTimeTxt.color = tempColor;

            closeTime -= Time.deltaTime;

            if (closeTime <= 0)
            {
                isOpen = true;
                closeTime = 40f;
            }
        }
    }

    void DoorUITimer()
    {
    }
}
