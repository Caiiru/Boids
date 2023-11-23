
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




[RequireComponent(typeof(Collider2D))]
public class BoidAgent : Entity
{ 
    Boid boidAgent;
    public Boid AgentBoid { get { return boidAgent; } }

    [Header("Movement Settings")]
    public float boidWeigth = 1f; 
    public Transform target;


    [Space]
    [Header("Hunger Settings")]

    public float hunger;
    private float baseLookForFoodRadius = 20f;
    public float lookForFoodRadius = 20f;
    private float timeToDeathByHunger = 100f;
    private float hungerMultiplier;

    [Space]
    [Header("Status Settings")]


    public float feedUrgency = 0;

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }
     
    public override void Start()
    {
        base.Start();
        agentCollider = GetComponent<Collider2D>();
        changeWeigth(Random.Range(1, 7));
        maxHealth = Random.Range(3, 20) * boidWeigth;
        damage = Random.Range(1, 6) * boidWeigth;
        hungerMultiplier = (Random.Range(0.2f, 1.5f));
        baseLookForFoodRadius = (Random.Range(1, 10));
        lookForFoodRadius = baseLookForFoodRadius;
        creatureCategory = SelectCreatureType();
        currentAction = CreatureAction.None;
        currentHealth = maxHealth;

    }
    CreatureCategory SelectCreatureType()
    {
        var num = Random.Range(0, 100);
        if (num >= 50)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            return CreatureCategory.Agressive;
        }
        else{
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            return CreatureCategory.Passive;
        }
    }
    public void Initialize(Boid boid)
    {
        boidAgent = boid;

    }
    public void setAction(CreatureAction action)
    {
        this.currentAction = action;
    }
    public void checkAction(Vector2 moveVelocity)
    {
        switch (currentAction)
        {
            case CreatureAction.Exploring:
                Move(moveVelocity);
                break;
            case CreatureAction.Pursuiting:
                Pursuit();
                break;
            case CreatureAction.GoingToFood:
                Pursuit();
                break;
        }
    }
    public void Pursuit()
    {

        if (creatureCategory == CreatureCategory.Passive)
        {
            var context = new List<Transform>();
            Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, lookForFoodRadius);

            foreach (Collider2D c in contextColliders)
            {
                if (c != this.agentCollider && c.gameObject.CompareTag("Food"))
                {
                    context.Add(c.transform);
                }
            }
            var distance = 0f;
            foreach (var t in context)
            {
                distance = Vector3.Distance(transform.position, t.position);
                if (target != null)
                {
                    if (distance < Vector3.Distance(transform.position, target.transform.position))
                        target = t;
                    else
                        return;
                }
                else
                    target = t;

            }


        }
        else
        {
            var context = new List<Transform>();
            Collider2D[] contextColliders = Physics2D.OverlapCircleAll(transform.position, lookForFoodRadius);

            foreach (Collider2D c in contextColliders)
            {
                if (c != this.agentCollider)
                {
                    context.Add(c.transform);
                }
            }
            var distance = 0f;
            foreach (var t in context)
            {
                distance = Vector3.Distance(transform.position, t.position);
                if (target != null)
                {
                    if (distance < Vector3.Distance(transform.position, target.transform.position))
                        target = t;
                    else
                        return;
                }
                else
                    target = t;

            }
        }

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            Attack(target.GetComponent<Entity>());
        }
    }
    public void Move(Vector2 velocity)
    {

        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.fixedDeltaTime;
        // transform.up = velocity;
        // transform.position += (Vector3)velocity * Time.fixedDeltaTime;
        Debug.DrawRay(transform.position, velocity.normalized * 1f, Color.red);
        spendEnergy();
    }
    public void changeWeigth(float valor)
    {
        boidWeigth += valor;
        transform.localScale = new Vector3(boidWeigth * 0.7f / 1.2f, boidWeigth * 0.7f);
        currentSpeed = maxSpeed / boidWeigth;
    }
    public void spendEnergy()
    {
        hunger += Time.deltaTime * 1 * hungerMultiplier / timeToDeathByHunger;
        lookForFoodRadius = baseLookForFoodRadius + (hunger * 100);

    }

  

    public void checkStatus()
    {
        checkDeath();
    }

    void checkDeath()
    {
        if (hunger >= 1)
            Die();

    }
 

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            if (hunger > 0.5f)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, lookForFoodRadius);
            }
        }


    }
}
