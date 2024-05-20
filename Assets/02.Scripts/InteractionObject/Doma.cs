using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doma : MonoBehaviour
{
    // 손에 들린 오브젝트가 hasPrepIngre (Item.itemType.tPrepIngre)일 때만 작동
    public Item domaItem = null;
    public GameObject[] ingredientsD; // 오브젝트-재료 Ingredients
    public GameObject knifeD;

    public int cntCut = 10; // 써는 횟수

    // 도마 위 아이템 비활성화
    public void SetActiveFalseDomaItem(Item itemOnHand, int domaItemIndex)
    {
        knifeD.SetActive(true); // 도마 위의 칼 활성화(원상복귀)
        ingredientsD[domaItemIndex].SetActive(false); // 도마위의 음식 비활성화
        domaItem = null;
    }

    // 도마 위에 아이템 올려두기
    public void SetActiveDomaItem(Item itemOnHand, int domaItemIndex)
    {
        knifeD.SetActive(false); // 도마 위의 칼 비활성화
        ingredientsD[domaItemIndex].SetActive(true);
        domaItem = ingredientsD[domaItemIndex].GetComponent<Item>();
    }

}
