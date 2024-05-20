using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    // UI
    [SerializeField]
    Text numHand;
    [SerializeField]
    Slider cuttingSlider;
    [SerializeField]
    GameObject cuttingSliderObj;
    Color tempColor;
    [SerializeField]
    Image[] ingreImg;
    [SerializeField]
    Image[] preingreImg;

    //CustomerController customer;

    AudioSource cuttingAudio;
    AudioSource completeAudio;

    PlayerMovement playerMovement;

    [SerializeField]
    public GameObject nearObject; // 근처 오브젝트 판단

    bool isDown; // 버튼 눌렀는지

    public bool isCatch;// 손에 오브젝트가 있는지, catchAnim에 적용
    public GameObject equipObj; // 잡고 있는(장착) 오브젝트가 무엇인지
    public Item equipItem; // 장착 오브젝트을 item으로 저장

    // 캐릭터 손에 미리 세팅해둘 오브젝트
    public GameObject[] handIngredients; // 오브젝트-재료 Ingredients
    public GameObject[] handPrepIngredients; // 오브젝트-손질재료 PrepIngredients
    public GameObject[] handDishes; // 오브젝트-완성요리 Dish
    public GameObject handPlate;
    public GameObject handKnife;

    int numStoreHand = 0;
    int[] numStoreTable = new int[6] { 0, 0, 0, 0, 0, 0 }; // 테이블의 각 pos index

    public int score = 0;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (nearObject != null)
        {
            if (nearObject.tag != "Doma")
            {
                cuttingSliderObj.SetActive(false);
            }
        }
        else
        {
            cuttingSliderObj.SetActive(false);
        }



        isDown = playerMovement.isDown;
        if (isDown)
        {
            if (nearObject != null)
            {
                if (nearObject.tag == "Ingredients")
                {
                    playerMovement.isCut = false; // 썰기 애님 비활성화
                    handKnife.SetActive(false); // 칼 비활성화
                    CatchObject();
                }
                else if (nearObject.tag == "Table")
                {
                    Table table = nearObject.GetComponent<Table>();
                    // 테이블에 오브젝트 없을 때
                    if (table.tableItem == null)
                    {
                        playerMovement.isCut = false; // 썰기 애님 비활성화
                        handKnife.SetActive(false); // 칼 비활성화
                        PutObject();
                    }
                    // 테이블에 오브젝트 있을 때
                    else
                    {
                        playerMovement.isCut = false; // 썰기 애님 비활성화
                        handKnife.SetActive(false); // 칼 비활성화
                        CatchObject();
                    }
                }
                else if (nearObject.tag == "Doma")
                {
                    Cutting();
                }
                else if (nearObject.tag == "Pot")
                {
                    if (equipObj != null)
                    {
                        playerMovement.isCut = false; // 썰기 애님 비활성화
                        handKnife.SetActive(false); // 칼 비활성화
                        Boiling();
                    }
                    else
                    {
                        playerMovement.isCut = false; // 썰기 애님 비활성화
                        handKnife.SetActive(false); // 칼 비활성화
                        GetDishOnPot();
                    }
                }
                else if (nearObject.tag == "Trash")
                {
                    playerMovement.isCut = false; // 썰기 애님 비활성화
                    handKnife.SetActive(false); // 칼 비활성화
                    ResetHand();
                }
                else
                {
                    playerMovement.isCut = false; // 썰기 애님 비활성화
                    handKnife.SetActive(false); // 칼 비활성화
                }
            }
            else
            {
                playerMovement.isCut = false; // 썰기 애님 비활성화
                handKnife.SetActive(false); // 칼 비활성화
                Debug.Log("주변에 반응 가능한 오브젝트가 없습니다.");
            }

        }
        if (numStoreHand >= 0) {
            numHand.text = "X " + numStoreHand.ToString("0"); //UI
        }
        if(numStoreHand == 0)
        {
            ImgNull();
        }
    }

    // UI
    void ImgNull()
    {
        for (int i = 0; i < 4; i++)
        {
            tempColor = ingreImg[i].color;
            tempColor.a = 0f;
            ingreImg[i].color = tempColor;
        }

        for (int i = 0; i < 4; i++)
        {
            tempColor = preingreImg[i].color;
            tempColor.a = 0f;
            preingreImg[i].color = tempColor;
        }
    }
    void IngreColorChange(int index)
    {
        for(int i = 0; i<4; i++)
        {
            tempColor = ingreImg[i].color;
            tempColor.a = 0f;
            ingreImg[i].color = tempColor;

            tempColor = preingreImg[i].color;
            tempColor.a = 0f;
            preingreImg[i].color = tempColor;

            if (i == index)
            {
                tempColor = ingreImg[i].color;
                tempColor.a = 1f;
                ingreImg[i].color = tempColor;

                tempColor = preingreImg[i].color;
                tempColor.a = 0f;
                preingreImg[i].color = tempColor;
            }
        }
    }
    void PreIngreColorChange(int index)
    {
        for (int i = 0; i < 4; i++)
        {
            tempColor = preingreImg[i].color;
            tempColor.a = 0f;
            preingreImg[i].color = tempColor;

            tempColor = ingreImg[i].color;
            tempColor.a = 0f;
            ingreImg[i].color = tempColor;

            if (i == index)
            {
                tempColor = ingreImg[i].color;
                tempColor.a = 0f;
                ingreImg[i].color = tempColor;

                tempColor = preingreImg[i].color;
                tempColor.a = 1f;
                preingreImg[i].color = tempColor;
            }
        }
    }

    // --------------< 요리 관련 오브젝트 판단 >--------------
    // 인터렉션 가능 오브젝트가 근처에 있을 때
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ingredients" || other.tag == "PrepIngredients" ||
            other.tag == "Dish" || other.tag == "Table" || other.tag == "Trash" ||
            other.tag == "Doma" || other.tag == "Pot" || other.tag == "Outlet")
        {
            nearObject = other.gameObject;
            //Debug.Log(nearObject.name + "이 옆에 있다.");
        }
    }

    // 인터렉션 가능 오브젝트가 근처에서 벗어났을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ingredients" || other.tag == "PrepIngredients" ||
            other.tag == "Dishes" || other.tag == "Table" || other.tag == "Trash" ||
            other.tag == "Doma" || other.tag == "Pot" || other.tag == "Outlet")
        {
            nearObject = null;
            Debug.Log("벗어났다");
        }
    }

    // 오브젝트 가챠!
    void Equip(GameObject[] targetObject, int index)
    {
        isCatch = true;
        handKnife.SetActive(false);
        targetObject[index].SetActive(true); // 캐릭터 손에 미리 세팅해둔 오브젝트 중 해당하는 오브젝트 활성화
        equipObj = targetObject[index]; // 손에 장착 오브젝트 = 활성화된 오브젝트
        equipItem = equipObj.GetComponent<Item>(); // 장착 오브젝트의 Item으로써 정보 저장
        Debug.Log(equipObj.name + "을 잡았다."); // 확인용 디버깅
    }

    void Put(GameObject handObject, int index)
    {
        Item handItem = handObject.GetComponent<Item>();
        ActiveFalse(handItem);
        Debug.Log("손에 있는 오브젝트를 놓았다.");
    }


    // SetActive 비활성화
    void ActiveFalse(Item item)
    {
        int index = item.iValue;

        Debug.Log(equipObj.name + "비활성화한다.");
        isCatch = false;
        switch (item.iType)
        {
            case Item.ItemType.tIngre:
                handIngredients[index].SetActive(false);
                break;
            case Item.ItemType.tPrepIngre:
                handPrepIngredients[index].SetActive(false);
                break;
            case Item.ItemType.tDish:
                handDishes[index].SetActive(false);
                break;
            default:
                break;
        }
        equipObj = null;
        equipItem = null;
    }

    // --------------< 요리 관련 오브젝트 잡기(습득) >--------------
    void CatchObject()
    {
        // 원재료 옆일 때
        if (nearObject.tag == "Ingredients")
        {
            Item nearItem = nearObject.GetComponent<Item>();
            // 처음 잡을 때
            if (equipObj == null)
            {
                Equip(handIngredients, nearItem.iValue);
                IngreColorChange(nearItem.iValue);
                AddHand();
            }
            // 이미 손에 뭔가 있을 때
            else
            {
                // (전제: 손에 있는 건 재료) && 같은 재료
                if(equipItem.iValue == nearItem.iValue)
                {
                    IngreColorChange(nearItem.iValue);
                    AddHand();
                }
                // 다른 재료
                else
                {
                    Debug.Log("손에 이미 오브젝트가 있습니다.");
                }
            }
        }
        // 테이블 옆일 때
        else if(nearObject.tag == "Table")
        {
            Table table = nearObject.GetComponent<Table>();
            // 테이블에 오브젝트가 있을 때
            if (table.tableItem != null)
            {
                Item nearItem = table.tableItem;
                // 손에 아무것도 없을 때
                if (equipItem == null)
                {
                    switch (nearItem.iType)
                    {
                        case Item.ItemType.tIngre:
                            Equip(handIngredients, nearItem.iValue);
                            IngreColorChange(nearItem.iValue);

                            // 원재료는 10개까지 들 수 있음
                            // 테이블 위의 오브젝트 개수가 1개 이하 : 한 번 가져가면 다 없어지는 상황
                            if (numStoreTable[table.posTable] <= 1)
                            {
                                table.SetActiveFalseTableItem(equipItem, equipItem.iValue); // 테이블 위의 오브젝트 비활성화
                            }
                            // 테이블 위에 오브젝트가 1개 초과일 때
                            AddHand();
                            SubTable(table.posTable);
                            break;

                        case Item.ItemType.tPrepIngre:
                            Equip(handPrepIngredients, nearItem.iValue);
                            PreIngreColorChange(nearItem.iValue);
                            handPlate.SetActive(true);
                            table.SetActiveFalseTableItem(equipItem, equipItem.iValue); // 테이블 위의 오브젝트 비활성화
                            break;

                        case Item.ItemType.tDish:
                            Equip(handDishes, nearItem.iValue);
                            table.SetActiveFalseTableItem(equipItem, equipItem.iValue); // 테이블 위의 오브젝트 비활성화
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Debug.Log("이미 손에 오브젝트가 있습니다.");
                }
            }
            else
            {
                Debug.Log("가져올 오브젝트가 테이블에 없습니다.");
            }
        }
        else
        {
            Debug.Log("잡을 게 없습니다.");
        }
    }

    void AddHand()
    {
        if (numStoreHand >= 10)
        {
            numStoreHand = 10;
        }
        else
        {
            numStoreHand++;
        }
        Debug.Log("+손: " + numStoreHand);
    }


    void SubTable(int index)
    {
        if (numStoreTable[index] <= 0)
        {
            numStoreTable[index] = 0;
        }
        else
        {
            numStoreTable[index]--;
        }
        Debug.Log("-테이블: " + numStoreTable[index]);
    }
    void SubHandAddTable(int index)
    {
        // 한꺼번에 올려두기
        if (numStoreTable[index] >= 10)
        {
            numStoreTable[index] = 10;
        }
        else
        {
            numStoreTable[index] += numStoreHand;
        }
        numStoreHand = 0;
        Debug.Log("-손: " + numStoreHand + "+테이블: " + numStoreTable[index]);
    }

    // --------------< 요리 관련 오브젝트 놓기 >--------------
    void PutObject()
    {
        // 테이블 옆
        if (nearObject.tag == "Table")
        {
            Table table = nearObject.GetComponent<Table>();
            // 손에 오브젝트 있음
            if (equipObj != null)
            {
                // 테이블 비었을 때
                if(table.tableItem == null)
                {
                    if (equipItem.iType == Item.ItemType.tIngre)
                    {
                        Debug.Log("테이블 위치: " + table.posTable);
                        SubHandAddTable(table.posTable);
                        ImgNull();
                    }
                    else if (equipItem.iType == Item.ItemType.tPrepIngre)
                    {
                        handPlate.SetActive(false);
                        numStoreHand = 0;
                        ImgNull();
                        numStoreTable[table.posTable] = 1;
                    }
                    else {
                        numStoreHand = 0;
                        ImgNull();
                        numStoreTable[table.posTable] = 1;
                    }
                    // 손에 있는 오브젝트랑 같은 걸 테이블에 활성화
                    table.SetActiveTableItem(equipItem, equipItem.iValue);
                    ActiveFalse(equipItem); // 손에 있는 걸 비활성화
                }
                // 테이블에 오브젝트 있음 : 실행안될 예정
                else
                {
                    // 테이블 위의 오브젝트 타입 == 손에 있는 오브젝트 타입 == 재료
                    if ((table.tableItem.iType == Item.ItemType.tIngre) && (equipItem.iType == Item.ItemType.tIngre))
                    {
                        // 테이블 위의 오브젝트 == 손에 있는 오브젝트 같은 종류
                        if(equipItem.iValue == table.tableItem.iValue)
                        {
                            SubHandAddTable(table.posTable);
                        }
                        else
                        {
                            Debug.Log("테이블 위의 오브젝트는 손에 있는 오브젝트와 다른 재료입니다.");
                        }
                    }
                    else
                    {
                        Debug.Log("이미 테이블에 오브젝트가 있습니다.");
                    }
                }
            }
            else
            {
                Debug.Log("손에 내려놓을 오브젝트가 없습니다.");
            }
        }
    }

    // --------------< 도마에서 일어나는 인터렉션 >--------------

    void Cutting()
    {
        // 도구 근처
        if(nearObject.tag == "Doma")
        {
            Doma doma = nearObject.GetComponent<Doma>();
            // 처음 도마 위에 오브젝트 올리기(손에 재료를 들고 있어야 함)
            if (equipObj != null)
            {
                if (equipObj.tag == "hasIngre")
                {
                    // 도마에 올려두기
                    if (doma.domaItem == null)
                    {
                        if(numStoreHand > 1)
                        {
                            Debug.Log("손에 재료가 너무 많습니다.");
                            cuttingSliderObj.SetActive(false);

                        }
                        else
                        {
                            cuttingSliderObj.SetActive(true);
                            cuttingSlider.value = 10 - doma.cntCut;


                            numStoreHand = 0;
                            ImgNull();
                            doma.SetActiveDomaItem(equipItem, equipItem.iValue); // 도마 위의 재료 활성화
                            Put(equipObj, equipItem.iValue);
                            handKnife.SetActive(true); // 칼 활성화
                            Debug.Log("도마 위에 올려둠");
                        }
                    }
                    else
                    {
                        cuttingSliderObj.SetActive(false);

                        Debug.Log("도마 위에 이미 오브젝트가 있습니다.");
                    }
                }
                else
                {
                    cuttingSliderObj.SetActive(false);
                    Debug.Log("손질된 재료를 들고 와주세요.");
                }
            }
            // 빈 손으로 <썰기> 하러 갈 때
            else if(equipObj == null)
            {
                if(doma.domaItem == null)
                {
                    cuttingSliderObj.SetActive(false);
                    Debug.Log("재료를 올려주세요");
                }
                else{
                    if (doma.cntCut > 0) // 손질 중
                    {
                        cuttingSliderObj.SetActive(true);

                        cuttingSlider.value = 10 - doma.cntCut;

                        playerMovement.isCut = true; // 썰기 애님 활성화
                        handKnife.SetActive(true); // 칼 활성화

                        isCatch = false;
                        Debug.Log("썰기: " + doma.cntCut);
                        doma.cntCut--;

                    }
                    else // 손질 완료
                    {
                        cuttingSliderObj.SetActive(true);

                        cuttingSlider.value = 10 - doma.cntCut;

                        playerMovement.isCut = false; // 썰기 애님 비활성화

                        Debug.Log("썰기: " + doma.cntCut);
                        Debug.Log("썰기 완료");
                        doma.cntCut = 10;
                        numStoreHand = 1;

                        cuttingSlider.value = 0;
                        cuttingSliderObj.SetActive(false);

                        Equip(handPrepIngredients, doma.domaItem.iValue);
                        PreIngreColorChange(equipItem.iValue);
                        handKnife.SetActive(false); // 칼 비활성화
                        handPlate.SetActive(true); // 접시 활성화
                        doma.SetActiveFalseDomaItem(doma.domaItem, doma.domaItem.iValue); // 도마 위의 재료 비활성화
                    }
                }
            }
            // 빈손이거나 재료를 들고 있지 않은 경우
            else
            {
                cuttingSliderObj.SetActive(false);

                Debug.Log("<썰기>를 할 수 없습니다.");
                playerMovement.isCut = false; // 썰기 애님 비활성화
                handKnife.SetActive(false); // 칼 비활성화
            }
        }
        else
        {
            cuttingSliderObj.SetActive(false);

            playerMovement.isCut = false; // 썰기 애님 비활성화
            handKnife.SetActive(false); // 칼 비활성화

        }

    }

    // --------------< 솥에서 일어나는 인터렉션 >--------------

    // 끓이기
    void Boiling()
    {
        Pot pot = nearObject.GetComponent<Pot>();
        if (pot.isDishFinish == true)
        {
            Debug.Log("완성된 요리를 먼저 꺼내주세요");
        }
        else
        {
            // "손에 손질재료" 들고 있음 && 솥 근처
            if (equipObj.tag == "hasPrepIngre")
            {
                // 솥에 재료 넣기
                pot.arrInPot[equipItem.iValue]++;
                pot.fireEffect.SetActive(true);
                Debug.Log("솥에 들어간 음식: " + equipObj.name);

                // 손에 있던 오브젝트 비활성화
                numStoreHand = 0;
                Put(equipObj, equipItem.iValue);
                ImgNull();
                handPlate.SetActive(false);

                // 타이머 시작
                pot.isTimerP = true;
            }
        }
    }

    void GetDishOnPot()
    {
        Pot pot = nearObject.GetComponent<Pot>();
        // 요리가 완성되었을 때
        if (pot.isDishFinish == true)
        {
            // 빈 손일 때
            if (equipObj == null)
            {
                // 성공했을 때
                if (pot.dishIndex < 4)
                {
                    switch (pot.dishIndex)
                    {
                        case 0:
                            score += 5;
                            break;
                        case 1:
                            score += 20;
                            break;
                        case 2:
                            score += 30;
                            break;
                        case 3:
                            score += 50;
                            break;
                    }
                    GetComponent<AudioSource>().Play();

                    Debug.Log("현재 점수: " + score);
                    //Equip(handDishes, pot.dishIndex);
                    //Debug.Log("완성 요리인 " + handDishes[pot.dishIndex].name + "을 잡았다.");
                }
                // 실패했을 때
                else
                {
                    score -= 20;
                    Debug.Log("score - 실패(20)");
                }
                pot.fireEffect.SetActive(false);
                pot.isDishFinish = false;
                pot.DishesOnPot[pot.dishIndex].SetActive(false);
            }
            else
            {
                Debug.Log("손에 오브젝트를 들고 있어서 <완성 요리>를 잡지 못합니다.");
            }
        }
        else
        {
            Debug.Log("요리가 완성되지 않았습니다.");
        }
        equipItem = null;
        equipObj = null;
        isCatch = false;
    }

    // 버그 정리
    void ResetHand()
    {
        if(nearObject.tag == "Trash")
        {
            if(equipObj != null)
            {
                Put(equipObj, equipItem.iValue);
                handPlate.SetActive(false);
                numStoreHand = 0;
                ImgNull();
            }
        }
    }



}

