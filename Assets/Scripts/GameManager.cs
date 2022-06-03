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
    [Tooltip("Position To spwan fruits from")]
    public SpawnPos[] spawnPos;

    [Space(10)]
    public GameObject[] fruits;

    public 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnFruits");
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
            Destroy(fruit, 5);
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
}
