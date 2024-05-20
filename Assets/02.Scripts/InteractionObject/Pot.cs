using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pot : MonoBehaviour
{
    public GameObject player;
    PlayerSkill playerskill;
    public int posPot; // 솥 위치
    public int[] arrInPot = new int[4] { 0, 0, 0, 0 };
    public int scoreP = 0;
    public bool isTimerP = false;
    public float potTimeP = 20;

    public GameObject[] DishesOnPot;
    public int dishIndex;
    public bool isDishFinish = false;

    public GameObject fireEffect;

    void Start()
    {
        fireEffect.SetActive(false);
    }
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // playerSkill 찾기 위해
        playerskill = player.GetComponent<PlayerSkill>();
    }

    void Update()
    {
        StartCoroutine(BoilingTimer());
    }

    public void RecipeinPot()
    {
        //쌀밥 = 쌀+쌀
        if (arrInPot[0] == 2
            && arrInPot[1] == 0
            && arrInPot[2] == 0
            && arrInPot[3] == 0)
        {
            dishIndex = 0;
            //scoreP += 5;
            //Debug.Log("+ 쌀(5): " + scoreP);
        }
        //치킨 = 닭 + 채소
        else if (arrInPot[0] == 0
            && arrInPot[1] == 1
            && arrInPot[2] == 0
            && arrInPot[3] == 1)
        {
            dishIndex = 1;
            //scoreP += 20;
            //Debug.Log("+ 백숙(20): " + scoreP);
        }
        //삼겹살 = 고기+채소
        else if (arrInPot[0] == 0
            && arrInPot[1] == 0
            && arrInPot[2] == 1
            && arrInPot[3] == 1)
        {
            dishIndex = 2;
            //scoreP += 30;
            //Debug.Log("+ 삼겹살(30): " + scoreP);
        }
        //국밥 = 쌀+고기+채소
        else if (arrInPot[0] == 1
            && arrInPot[1] == 0
            && arrInPot[2] == 1
            && arrInPot[3] == 1)
        {
            dishIndex = 3;
            //scoreP += 50;
            //Debug.Log("+ 국밥(50): " + scoreP);
        }
        // 실패작
        else
        {
            dishIndex = 4;
            //scoreP -= 20;
            //Debug.Log("- 실패(20) : " + scoreP);
        }
        DishesOnPot[dishIndex].SetActive(true);
        isDishFinish = true;
    }

    IEnumerator BoilingTimer()
    {
        if (isTimerP)
        {
            potTimeP -= Time.deltaTime;
            Debug.Log("요리 남은 시간: " + (int)potTimeP);

            if (potTimeP <= 0)
            {
                isTimerP = false;
                RecipeinPot();

                //playerskill.scoreInPot[posPot] = scoreP;
                for (int i = 0; i < 4; i++)
                {
                    arrInPot[i] = 0;
                }
                potTimeP = 20f;
            }
            
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
