using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType
{
    apple,
    banana,
    melon,
    papaya,
    melonBluyish,
    bomb,
}

public class knifeController : MonoBehaviour
{
    public GameObject trail;
    public GameObject Currenttrail;
    private Vector2 initialPos;
    private bool cutting;
    private float clickTime;

    private float lastCut;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cutting = true;
            Currenttrail = GameObject.Instantiate(trail, transform);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cutting = false;
            Currenttrail.transform.SetParent(null);
            Destroy(Currenttrail, 2);
        }

        if (cutting)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!cutting)
            return;
        ////Swiping speed is not fast enough;
        //print(Vector2.Distance(transform.position, initialPos) / Time.deltaTime);
        //if (Vector2.Distance(transform.position, initialPos) /Time.deltaTime < 4)
        //    return;

        if (collision.gameObject.CompareTag("Fruits"))
        {
            fruitController fruit = collision.gameObject.GetComponent<fruitController>();
            fruit.CutFruit();
            Color color = Color.white;
            
            float scale = 1;
            switch (fruit.fruitType)
            {
                case FruitType.apple:
                    color = Color.red;
                    scale = 0.7f;
                    break;
                case FruitType.banana:
                    color = Color.yellow;
                    scale = 0.7f;
                    break;
                case FruitType.melon:
                    color = Color.red;
                    break;
                case FruitType.papaya:
                    color = new Color(1,165/255,0);
                    break;
                case FruitType.melonBluyish:
                    color = Color.blue;
                    break;
                case FruitType.bomb:
                    break;
                default:
                    break;
            }
            if ((Time.time - lastCut) < 1)
            {
                GameManager.instance.combo += 1;
            }
            else
            {
                GameManager.instance.combo = 1;
            }
            lastCut = Time.time;
            GameManager.instance.AddScore(10);
            GameManager.instance.PlaySplash(collision.transform.position, color,scale);
        }
    }
}
