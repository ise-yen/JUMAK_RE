using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public int posTable;
    public Item tableItem = null;
    //public bool hasTableItem = false;

    public GameObject[] ingredientsT; // 오브젝트-재료 Ingredients
    public GameObject[] prepIngredientsT; // 오브젝트-손질재료 PrepIngredients
    public GameObject[] dishesT; // 오브젝트-완성요리 Dish
    public GameObject plateT;

    // 테이블 위 아이템 비활성화
    public void SetActiveFalseTableItem(Item itemOnTable, int tableItemIndex)
    {
        switch (itemOnTable.iType)
        {
            case Item.ItemType.tIngre:
                ingredientsT[tableItemIndex].SetActive(false);
                tableItem = null;
                break;
            case Item.ItemType.tPrepIngre:
                plateT.SetActive(false); ; // 접시 비활성화
                prepIngredientsT[tableItemIndex].SetActive(false);
                tableItem = null;
                break;
            case Item.ItemType.tDish:
                dishesT[tableItemIndex].SetActive(false);
                tableItem = null;
                break;
            default:
                tableItem = null;
                break;
        }
    }
    // 테이블 위에 아이템 올려두기
    public void SetActiveTableItem(Item itemOnHand, int tableItemIndex)
    {
        switch (itemOnHand.iType)
        {
            case Item.ItemType.tIngre:
                ingredientsT[tableItemIndex].SetActive(true);
                tableItem = ingredientsT[tableItemIndex].GetComponent<Item>();
                break;
            case Item.ItemType.tPrepIngre:
                plateT.SetActive(true); // 접시 활성화
                prepIngredientsT[tableItemIndex].SetActive(true);
                tableItem = prepIngredientsT[tableItemIndex].GetComponent<Item>();
                break;
            case Item.ItemType.tDish:
                dishesT[tableItemIndex].SetActive(true);
                tableItem = dishesT[tableItemIndex].GetComponent<Item>();
                break;
            default:
                break;
        }
    }
}
