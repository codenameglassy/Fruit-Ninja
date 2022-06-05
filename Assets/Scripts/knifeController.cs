using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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


    private bool cutting;
    private float lastCut;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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
            if (Currenttrail != null)
            {
                Currenttrail.transform.SetParent(null);
                Destroy(Currenttrail, 2);
            }
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
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                SceneManager.LoadScene(2);
                return;
            }
            fruitController fruit = collision.gameObject.GetComponent<fruitController>();
            fruit.CutFruit(transform.position);
            Color color=Color.black;
            float scale=1;
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
                    fruit.cut = true;
                    return;
                default:
                    break;
            }
            if ((Time.time - lastCut) < 1)
            {
                GameManager.instance.increaseCombo();
                
                UIManager.instance.ShowCombo(Camera.main.WorldToScreenPoint(collision.transform.position),(int) GameManager.instance.combo);
            }
            else
            {
                GameManager.instance.combo = 1;
                UIManager.instance.DisableCombo();
            }
            
            lastCut = Time.time;
            GameManager.instance.AddScore(10);
            soundManager.instance.PlaySound(SoundType.slashSound);
            GameManager.instance.PlaySplash(collision.transform.position, color,scale);
        }
    }
}
