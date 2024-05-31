using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToonyColorsPro;

public enum CharacterColor
{
    Destroy = 0,
    Blue = 1,
    Cyan = 2,
    DarkPurple = 3,
    Green= 4,
    Orange = 5,
    Pink = 6,
    Purple = 7,
    Red = 8,
    White = 9,
    Yellow = 10,
    Empty = 11
}

public class CharacterController : MonoBehaviour
{
    public CharacterColor characterColor;
    private SkinnedMeshRenderer skinnedMeshRenderer0,skinnedMeshRenderer1;
    private Transform childTransform0;
    private Transform childTransform1;
    public static Material lastMat;

    private void Start()
    {
        UpdateColor();
    }

    public void UpdateColor()
    {
        childTransform0 = transform.GetChild(0);
        skinnedMeshRenderer0 = childTransform0.GetComponent<SkinnedMeshRenderer>();
        childTransform1 = transform.GetChild(1);
        skinnedMeshRenderer1 = childTransform1.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer0 == null) return;
        if (skinnedMeshRenderer1 == null) return;

        // GameManager'dan materyali al
        Material selectedMaterial0 = GameManager.Instance.GetMaterial(characterColor);

        skinnedMeshRenderer0.material = selectedMaterial0;
        Material selectedMaterial1 = GameManager.Instance.GetMaterial2(selectedMaterial0);
        skinnedMeshRenderer1.material = selectedMaterial1;
        lastMat = selectedMaterial0;
    }
}

