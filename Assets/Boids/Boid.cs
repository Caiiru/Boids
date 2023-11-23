using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public BoidAgent agentPrefab;
    List<BoidAgent> agents = new List<BoidAgent>();
    public MovementBoidBehavior movementBehavior;
    public EntityBehavior entityBehavior;

    [Range(10, 400)] public int startingCount = 200;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)] public float driveFactor = 10f;
    [Range(1f, 100f)] public float maxSpeed = 5f;
    [Range(1f, 10f)] public float neighborRadius = 1.5f;
    [Range(0f, 1f)] public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed,
    squareNeighborRadius,
    squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    private Transform player;


    void Start()
    {
        player = GameObject.FindAnyObjectByType<player>().transform;
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            BoidAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity * 10,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360)),
                transform
            );

            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BoidAgent agent in agents)
        {
            agent.transform.position = returnBoids(agent.transform.position);
            List<Transform> context = GetNearbyObjects(agent);

            //ONLY EDITOR
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white,Color.red, context.Count/6f);

            Vector2 move = movementBehavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = (move.normalized * agent.currentSpeed) * maxSpeed;
            }
            agent.setAction(entityBehavior.ChooseAction(agent,context,this,agent.creatureCategory));

            agent.checkAction(move);
            agent.checkStatus();

        }
    }

    List<Transform> GetNearbyObjects(BoidAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);

        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
                //Debug.DrawLine(agent.transform.position,c.transform.position, Color.blue, 0.5f, false);
            }
        }
        return context;
    }

    Vector3 returnBoids(Vector3 boid)
    {
        var distanceToPlayer = Vector3.Distance(player.position, boid);
        if (distanceToPlayer > 300)
        {
            Vector2 randomPos = Random.insideUnitCircle * 300;
            Vector3 spawnPos =
                new Vector3((player.position.x + 40) + randomPos.x, (player.position.y + 40) + randomPos.y, 0f);

            return spawnPos;
        }
        return boid;

    }


    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.position , 300);

        }
    }

}
