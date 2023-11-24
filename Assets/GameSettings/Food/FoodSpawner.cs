using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public float spawnRadius = 100f;
    [Range(1, 100)]
    public int startFood = 50;

    [Range(1, 30)]
    public float spawnTime = 10;

    public int multiplier = 1;

    private float timeToSpawn = 0f;

    [Range(10, 500)]
    public int maxFoodCount = 100;

    public List<GameObject> foodList = new List<GameObject>();

    [Space]
    [Header("References")]
    public GameObject foodPrefab;
    public Transform player;
    void Start()
    {
        player = GameObject.FindAnyObjectByType<player>().transform;
        for (int i = 0; i < startFood; i++)
        {
            SpawnFood();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (timeToSpawn > spawnTime)
            for(int i = 0; i<multiplier;i++){
                SpawnFood();
            }
        else
            timeToSpawn += 1 * Time.deltaTime;


        checkUncessaryFoods();
    }
    void SpawnFood()
    {
        timeToSpawn = 0;
        if (GameObject.FindGameObjectsWithTag("Food").Length >= maxFoodCount)
        {
            // Debug.Log("Número máximo de comida atingido");
            return;
        }

        var spawnPos = changeFoodPosition();

        var spawnedFood = Instantiate(foodPrefab, spawnPos, Quaternion.identity);
        spawnedFood.transform.SetParent(this.transform);
        spawnedFood.name = "food"+foodList.Count;
        spawnedFood.tag = "Food";
        foodList.Add(spawnedFood);
        // Debug.Log("Food created in " + spawnPos);
    }

    Vector3 changeFoodPosition()
    {
        Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(player.position.x + randomPos.x, player.position.y + randomPos.y, 0f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPos, 1f);
        foreach (var collider in colliders)
        {
            if (collider.tag == "Food")
            {
                return changeFoodPosition();

            }
        }
        return spawnPos;
    }
    void checkUncessaryFoods()
    {
        foreach (var food in foodList)
        { 
            if(food.GetComponent<Entity>().wasEaten){
                foodList.Remove(food);
                Destroy(food);
                return;
            }   
            float distanceToPlayer = Vector3.Distance(player.transform.position,food.transform.position);
            if(distanceToPlayer > spawnRadius){
                food.transform.position = changeFoodPosition();
            }
           
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, spawnRadius);
    }
}
