using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEntity : Entity
{
    public override void Start()
    {
        base.Start();
        creatureCategory = CreatureCategory.Food;
        maxSpeed = 0;
        currentSpeed = 0;
        hunger = 0;
        damage = 0;
        currentHealth = maxHealth;
    }
    public override void Update()
    {
        base.Update(); 
         
    }
    
}
