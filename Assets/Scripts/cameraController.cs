using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public static cameraController instance;
    public Vector2 leftBound;
    public Vector2 rightBound;
    public Vector2 bottomBound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        leftBound =Camera.main.ScreenToWorldPoint(new( Camera.main.pixelRect.xMin,0,0));
        rightBound = Camera.main.ScreenToWorldPoint(new(Camera.main.pixelRect.xMax, 0, 0));
        bottomBound = Camera.main.ScreenToWorldPoint(new(0,Camera.main.pixelRect.yMin, 0));

        print((leftBound, rightBound, bottomBound));
    }

    public bool CheckBound(Vector2 pos)
    {
        if (pos.x < leftBound.x || pos.x > rightBound.x || pos.y < bottomBound.y)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
