using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // 아이템 종류
    public enum ItemType { tIngre, tPrepIngre, tDish, tDoma, tPot };
    //public enum DishName { none, bab, baeksook, samgyeobsal, gugbab };
    public enum ToolName { none, doma, sot};

    public ItemType iType;

    public ToolName iToolName;

    public int iValue;
    // tIngre: rice, chicken, meat, vege
    // tPrepIngre: preprice, prepchicken, prepmeat, prepvegetable
    // tDish: bab, tongdalg, samgyeobsal, gugbab
    // tTool: Doma, Pot, Table

}
