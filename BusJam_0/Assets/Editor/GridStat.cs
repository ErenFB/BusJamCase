using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStat : MonoBehaviour
{
    public int visited = -1;
    public int x = 0;
    public int y = 0;
    // Start is called before the first frame update
    void Start()
    {
        float r = Random.Range(0, 256);
        float g = Random.Range(0, 256);
        float b = Random.Range(0, 256);
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(r/256,g / 256, b / 256);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
