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
}

public class knifeController : MonoBehaviour
{
    public GameObject trail;
    public GameObject Currenttrail;
    private Vector2 initialPos;
    private bool cutting;
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
            initialPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

        if (collision.gameObject.CompareTag("Fruits"))
        {
            FruitType type = collision.gameObject.GetComponent<fruitController>().fruitType;
            //Vector2 direction = Vector2.Distance
            switch (type)
            {
                case FruitType.apple:
                    print("apple");
                    break;
                case FruitType.banana:
                    print("banana");

                    break;
                case FruitType.melon:
                    print("melon");

                    break;
                case FruitType.papaya:
                    print("papaya");

                    break;

                case FruitType.melonBluyish:
                    print("melonBluyish");

                    break;
                default:
                    print("something wrong");

                    break;
            }
        }
    }
}
