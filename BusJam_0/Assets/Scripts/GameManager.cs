using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public static bool can = false;
    public static GameManager Instance;
    public Material defaultMaterial;
    public Material[] colorMaterials;
    public Transform carrrsss;


    //public static List<float> values = new List<float> { 25, 35, 45, 55, 65, 75, 85 };
    public static Dictionary<int, (GameObject gameObject, float value, bool empty)> values = new Dictionary<int, (GameObject gameObject, float value, bool empty)>
    {
        { 0, (null, 25, true) },
        { 1, (null, 35, true) },
        { 2, (null, 45, true) },
        { 3, (null, 55, true) },
        { 4, (null, 65, true) },
        { 5, (null, 75, true) },
        { 6, (null, 85, true) }
    };
    private Vector3 reverseTransform;
    [SerializeField] private GameObject[] carList;
    [SerializeField] private CarColor currentColor;
    private int carIndex;
    public static int characterCount = 0;
    void Start()
    {
        CarCurrentColor();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Material GetMaterial(CharacterColor color)
    {
        string colorName = color.ToString() + "Mat";
        foreach (Material mat in colorMaterials)
        {
            if (mat.name.Equals(colorName, System.StringComparison.OrdinalIgnoreCase))
            {
                return mat;
            }
        }
        return defaultMaterial;
    }
    public Material GetMaterial2(Material material)
    {
        string colorName = "O_" + material.name.ToString(); 
                                                           
        foreach (Material mat in colorMaterials)
        {
           
            if (mat.name.Equals(colorName, System.StringComparison.OrdinalIgnoreCase))
            {
                return mat;
            }
        }
        return material;
    }
    public Material GetMaterialCar(CarColor color)
    {
        string colorName = color.ToString() + "Mat";
        foreach (Material mat in colorMaterials)
        {
            if (mat.name.Equals(colorName, System.StringComparison.OrdinalIgnoreCase))
            {
                return mat;
            }
        }
        return defaultMaterial;
    }
    public void CarFullMove()
    {
        carList[carIndex].gameObject.SetActive(false); //no time move anim:c
        carIndex++; 
        carList[carIndex].gameObject.SetActive(true);ResetSeats(); carList[carIndex].gameObject.GetComponent<CarScript>().canGoNow();
        CarCurrentColor();
    }
    public CarColor CarCurrentColor()
    {
        carList[carIndex].GetComponent<CarScript>().canGoNow();
        currentColor = carList[carIndex].GetComponent<CarScript>().carColor;
        return currentColor;
    }
    public void CheckCarFull()
    {

            CarFullMove();
            characterCount = 0;
            carrrsss.position = new Vector3(-24, 2, 130);
    }
    public void AddReserveCharacter(GameObject character)
    {
 
        int index = FindSmallestIndexWithEmpty();
        character.transform.position = reverseTransform;
        SetCanMove(index, character, false);

    }
    
    public GameObject transformPos(GameObject gm)
    {
        return gm;
    }
    public void GoToCar(GameObject character)
    {
        characterCount++;

        character.SetActive(false);
        if (characterCount == 1) {
                CarScript.seaters2[0].SetActive(true);
        }
        if (characterCount == 2)
        {
            CarScript.seaters2[0].SetActive(true);
            CarScript.seaters2[1].SetActive(true);
        }
        if (characterCount == 3)
        {
            CarScript.seaters2[0].SetActive(true);
            CarScript.seaters2[1].SetActive(true);
            CarScript.seaters2[2].SetActive(true); 
            CheckCarFull();
        }
       
    }
    public void ResetSeats()
    {
       
            CarScript.seaters2[0].SetActive(false);
            CarScript.seaters2[1].SetActive(false);
            CarScript.seaters2[2].SetActive(false); 
        

    }
    public void CheckReserveAndGo()
    {
        int i = 0;
        foreach (var key in values.Keys)
        {
            if (values[key].empty == false)
            {
                
                if (values[key].gameObject.GetComponent<CharacterController>().characterColor.ToString() == currentColor.ToString())
                {
                    GoToCar(values[key].gameObject);
                    SetCanMove(key, null, true);

                }
            }
      
        }
    }
    void SetCanMove(int index,GameObject gameObject, bool empty)
    {
        if (values.ContainsKey(index))
        {
            var value = values[index].value;
            values[index] = (gameObject, value, empty);
        }
     
    }
    int FindSmallestIndexWithEmpty()
    {
        for (int i = 0; i < values.Count; i++)
        {;
            if (values[i].empty == true)
            {
                reverseTransform = new Vector3(values[i].value, 0, 105);
                return i;
            }
        }
        return -1;
    }
}
