using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static PathfindingData;
public enum CarColor
{
    Destroy = 0,
    Blue = 1,
    Cyan = 2,
    DarkPurple = 3,
    Green = 4,
    Orange = 5,
    Pink = 6,
    Purple = 7,
    Red = 8,
    White = 9,
    Yellow = 10,
    Empty = 11
}
public class CarScript : MonoBehaviour
{
    public static int characterCount;
    public CarColor carColor;
    private MeshRenderer renderer;
    private Transform childTransform;
    public GameObject[] seaters;
   
    public static List<GameObject> seaters2 = new List<GameObject>();
    public void UpdateColor()
    {
        childTransform = gameObject.transform.GetChild(0);
        renderer =childTransform.GetComponent<MeshRenderer>();
        if (renderer == null) return;
        Material selectedMaterial0 = GameManager.Instance.GetMaterialCar(carColor);
        renderer.material = selectedMaterial0;

    }
    private void Start()
    {
        UpdateColor();
        foreach (GameObject gm in seaters)
        {
            gm.GetComponent<SkinnedMeshRenderer>().material = renderer.material;
        }
    }
    public void canGoNow()
    {
        foreach (GameObject gm in seaters)
        {
            seaters2.Add(gm);
        }
    }
}

