using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DefaultNamespace;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;

public class WolfScript : SteeringBehaviours, IGiveTarget
{
    [SerializeField] private Transform target;
    private Vector3 targetPos;
    private Vector3 randomPos;
    private double randomPhi = 0;
    [SerializeField] private float hungerTime = 5f;
    private bool caughtPrey = false;
    private float timerValue;
    private string state = "Wandering";
    private bool isLooping = true;
    [SerializeField] private float detectRange;
    [SerializeField] private float calmRange;
    
    private float timeBetweenStates = 0.15f;

    private Transform closestTarget;
    private float distanceToClosestTarget;
    
    private void Awake()
    {
        base.Start();
        timerValue = hungerTime;
        StartCoroutine(Hunger(hungerTime));
        PickRandomWanderSpot();
        WanderTimerReset();
    }
    private void FixedUpdate()
    {
        closestTarget = GetClosestTarget();
        if (distanceToClosestTarget < (detectRange * detectRange))
        {
            state = "Chase";
            if (closestTarget != null)
            {
                target = closestTarget; 
            }
            else
            {
                state = "Wandering";
            }
            
        } else if (distanceToClosestTarget > calmRange * calmRange)
        {
            state = "Wandering";
        }
        else
        {
            if (closestTarget == null)
            {
                state = "Wandering";
            }
        }

        Vector3 avoidWalls = AvoidEdges();
        Vector3 steer = Vector3.zero;
        switch (state)
        {
            case "Wandering":
                steer = Steer(targetPos);
                if (timeBetweenStates + Time.deltaTime <= Time.time)
                {
                    PickRandomWanderSpot();
                    WanderTimerReset();
                }
                break;
            case "Chase":
                if (target != null)
                {
                    steer = Steer(target.position);
                }
                break;
        }
        ApplyForce(steer);
        ApplyForce(avoidWalls*1.5f);
        ApplySteeringToMotion();
    }
    private float WanderTimerReset()
    {
        return Time.time + timeBetweenStates;
    }
    private void PickRandomWanderSpot()
    {
        Random rn = new Random();
        randomPhi += rn.NextDouble() * 1.6 - 0.8;
        randomPos = new Vector2((float)Math.Cos(randomPhi), (float) Math.Sin(randomPhi));
        targetPos = transform.GetChild(0).position + randomPos;
    }
    private void OnTriggerEnter2D(Collider2D biteInfo)
    {
        Debug.Log(biteInfo.name);
        switch (biteInfo.gameObject.tag)
        {
            case "Animal":
                Prey prey = biteInfo.GetComponent<Prey>();
                if (prey != null)
                {
                    biteInfo.gameObject.GetComponent<Prey>().Die();
                    caughtPrey = true;
                    state = "Wandering";
                }
                break;
            case "Player":
                HunterScript hunter = biteInfo.GetComponent<HunterScript>();
                if (hunter != null)
                {
                    hunter.Die();
                    caughtPrey = true;
                }
                break;
        }
    }
    public Transform GetClosestTarget()
    {
        // tag_1 = GameObject.FindGameObjectsWithTag("Neutral").Select(go => go.transform).ToArray();
        // tag_2 = GameObject.FindGameObjectsWithTag("Player").Select(go => go.transform).ToArray();
        Transform[] enemies = GameObject.FindGameObjectsWithTag("Prey").Select(go => go.transform.parent.gameObject.transform).ToArray();
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                distanceToClosestTarget = dSqrToTarget;
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        
        return bestTarget;
    }
    IEnumerator Hunger(float lifetime)
    {
        do
        {
            timerValue -= Time.deltaTime;
            if (!caughtPrey)
            {
                if (timerValue < 0)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                timerValue = lifetime;
                caughtPrey = false;
            }
            yield return new WaitForEndOfFrame();
        } while (isLooping);
    }
}
