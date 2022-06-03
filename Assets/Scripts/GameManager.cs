using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SpawnPos
{
    public Transform spawnPosition;

    [Range(-90,90)]
    public int forceAngleLeft;
    [Range(-90, 90)]
    public int forceAngleRight;
    [Range(0,1000)]
    public float force;
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Tooltip("Position To spwan fruits from")]
    public SpawnPos[] spawnPos;

    [Space(10)]
    public GameObject[] fruits;

    [Space(10)]
    public GameObject[] splashEffects;

    public float score;
    public float combo=1;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnFruits");
        Time.timeScale = 0.75f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnFruits()
    {
        while (true)
        {
            int fruitindex = Random.Range(0, fruits.Length);
            int spawnIndex = Random.Range(0, spawnPos.Length);
            float a =Random.Range( spawnPos[spawnIndex].forceAngleLeft * Mathf.Deg2Rad, spawnPos[spawnIndex].forceAngleRight * Mathf.Deg2Rad);
            Vector3 dir = (transform.up * Mathf.Cos(a) + transform.right * Mathf.Sin(a)).normalized;
            GameObject fruit =Instantiate(fruits[fruitindex],spawnPos[spawnIndex].spawnPosition.position,Quaternion.identity);
            fruit.GetComponent<fruitController>().AddForce(dir, spawnPos[spawnIndex].force);
            Destroy(fruit, 10);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (SpawnPos pos in spawnPos)
        {
            float a = pos.forceAngleLeft * Mathf.Deg2Rad;
            Vector3 dir = (transform.up * Mathf.Cos(a) + transform.right *Mathf.Sin(a)).normalized;
            Gizmos.DrawRay(pos.spawnPosition.position, dir*pos.force);
            float b = pos.forceAngleRight * Mathf.Deg2Rad;
            Vector3 dirb = (transform.up * Mathf.Cos(b) + transform.right * Mathf.Sin(b)).normalized;
            Gizmos.DrawRay(pos.spawnPosition.position, dirb*pos.force);
        }
    }

    public void PlaySplash(Vector2 position,Color splashColor,float scale)
    {
        int splashEffectIndex = Random.Range(0, splashEffects.Length);
        GameObject splashObject = Instantiate(splashEffects[splashEffectIndex], position, Quaternion.identity);
        splashObject.GetComponent<SpriteRenderer>().color = splashColor;
        splashObject.transform.localScale = splashObject.transform.localScale*scale;
        Destroy(splashObject, 5);
    }

    public void AddScore(float amount)
    {
        score += amount*combo;
    }
}
