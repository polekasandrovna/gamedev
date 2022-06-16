using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class RabbitScript : SteeringBehaviours, IGiveTarget
{
   private Transform target;
   private Vector3 randomPos;
   private double randomPhi;
   private Vector3 targetPos;
   private string state = "Wandering";
   [SerializeField] private float detectRange;
   [SerializeField] private float calmRange;

   [SerializeField] private float timeBetweenStates;

   private Transform closestCreature;
   private float distanceToClosestCreature;

   [SerializeField] private float wanderSpeed;
   [SerializeField] private float fleeSpeed;
   private IGiveTarget _giveTargetImplementation;

   private void Awake()
   {
      base.Start();
      PickRandomWanderSpot();
      ResetWanderTime();
   }
   private void PickRandomWanderSpot()
   {
      Random rn = new Random();
      randomPhi += rn.NextDouble() * 1.6 - 0.8;
      randomPos = new Vector3((float)Math.Cos(randomPhi), (float) Math.Sin(randomPhi));
      targetPos = transform.GetChild(0).position + randomPos;
   }
   private void FixedUpdate()
   {
      closestCreature = GetClosestTarget();
      if (distanceToClosestCreature < (detectRange * detectRange))
      {
         state = "Chase";
         if (closestCreature != null)
         {
            target = closestCreature;
            maxSpeed = fleeSpeed;
         }
         else
         {
            state = "Wandering";
            maxSpeed = wanderSpeed;
         }
      }
      else if (distanceToClosestCreature > calmRange * calmRange)
      {
         state = "Wandering";
         maxSpeed = wanderSpeed;
      }
      else
      {
         if (closestCreature == null)
         {
            state = "Wandering";
            maxSpeed = wanderSpeed;
         }
      }

      Vector3 avoidEdges = AvoidEdges();
      Vector3 steer = Vector3.zero;
      switch (state)
      {
         case "Wandering":
            steer = Steer(targetPos);
            if (timeBetweenStates + Time.deltaTime <= Time.time)
            {
               PickRandomWanderSpot();
               ResetWanderTime();
            }
            break;
         case "Chase":
            if (target != null)
            {
               steer = Flee(target.position);
            }
            break;
            }
      ApplyForce(steer);
      ApplyForce(avoidEdges * 1.5f);
      ApplySteeringToMotion();
   }
   public Transform GetClosestTarget()
   {
      List<Transform> enemies = GameObject.FindGameObjectsWithTag("Creature").Select(go => go.transform.parent.gameObject.transform).ToList();
      enemies.Remove(gameObject.transform);
      Transform bestTarget = null;
      float closestDistanceSqr = Mathf.Infinity;
      Vector3 currentPosition = transform.position;
      foreach(Transform potentialTarget in enemies)
      {
         Vector3 directionToTarget = potentialTarget.position - currentPosition;
         float dSqrToTarget = directionToTarget.sqrMagnitude;
         if(dSqrToTarget < closestDistanceSqr)
         {
            distanceToClosestCreature = dSqrToTarget;
            closestDistanceSqr = dSqrToTarget;
            bestTarget = potentialTarget;
         }
      }
     
      return bestTarget;
   }
   private float ResetWanderTime()
   {
      return timeBetweenStates + Time.time;
   }
}
