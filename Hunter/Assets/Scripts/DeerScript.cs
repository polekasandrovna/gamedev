using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;

public class DeerScript : SteeringBehaviours, IGiveTarget
{
    private Transform target;
    private Vector3 randomPos;
    private double randomPhi = 0;
    private Vector3 targetPos;
    private string state = "Wandering";
    [SerializeField] float distForNeighbor = 10;
    [SerializeField] private float detectRange;
    [SerializeField] private float calmRange;
    
    [SerializeField] private float timeBetweenStates;

    private List<GameObject> flock;
    private Transform closestDanger;
    private float distanceToClosestDanger;

    private void Awake()
    {
        base.Start();
        PickRandomWanderSpot();
        ResetWanderTime();
    }

    private float ResetWanderTime()
    {
        return timeBetweenStates + Time.time;
    }

    private void PickRandomWanderSpot()
    {
        Random rn = new Random();
        randomPhi += rn.NextDouble() * 1.6 - 0.8;
        randomPos = new Vector2((float)Math.Cos(randomPhi), (float) Math.Sin(randomPhi));
        targetPos = transform.GetChild(0).position + randomPos;
    }
    
    void FixedUpdate()
    {
        closestDanger = GetClosestTarget();
        Vector3 avoidwalls = AvoidEdges();
        flock = GameObject.FindGameObjectsWithTag("Deer").Select(go => go.transform.parent.gameObject).ToList();
        Vector3 separate = Separate(flock);
        Vector3 align = Align(flock);
        Vector3 cohesion = Cohesion(flock);
        Vector3 steer = Vector3.zero;
        if (distanceToClosestDanger < (detectRange * detectRange))
        {
            state = "Chase";
            if (closestDanger != null)
            {
                target = closestDanger; 
            }
            else
            {
                state = "Wandering";
            }
            
        } else if (distanceToClosestDanger > calmRange * calmRange)
        {
            state = "Wandering";
        }
        else
        {
            if (closestDanger == null)
            {
                state = "Wandering";
            }
        }
        
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
        
        ApplyForce(steer * 1.5f);
        ApplyForce(avoidwalls * 3f);
        ApplyForce(separate * 0.5f);
        ApplyForce(align);
        ApplyForce(cohesion * 0.5f);
        ApplySteeringToMotion();
        
    }

    public Vector3 Align(List<GameObject> flock)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (var deer in flock)
        {
            float dist = Vector3.Distance(transform.position, deer.transform.position);
            if ((dist > 0) && (dist < distForNeighbor))
            {
                sum += deer.GetComponent<DeerScript>().velocity;
                count++;
            }
        }

        if (count > 0)
        {
            sum = sum / count;
            sum.Normalize();
            sum *= maxSpeed;
            Vector3 steer = Vector3.ClampMagnitude(sum - velocity, maxForce);
            return steer;
        }

        return Vector3.zero;

    }
    
    public Vector3 Cohesion(List<GameObject> flock)
    {
        float distForNeighbor = 10;
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (var deer in flock)
        {
            float dist = Vector3.Distance(transform.position, deer.transform.position);
            if ((dist > 0) && (dist < distForNeighbor))
            {
                sum += deer.GetComponent<DeerScript>().velocity;
                count++;
            }
        }

        if (count > 0)
        {
            sum = sum / count;
            return Steer(sum);
        }

        return Vector3.zero;

    }
    
    public Vector3 Separate(List<GameObject> flock)
    {
        Vector3 sum = Vector3.zero;
        int count = 0;
        float desiredSep = 1.5f;
        foreach (var deer in flock)
        {
            float dist = Vector3.Distance(transform.position, deer.transform.position);
            if ((dist > 0) && (dist < desiredSep))
            {
                Vector3 diff = transform.position - deer.transform.position;
                diff.Normalize();
                sum += diff;
                count++;
            }
        }

        if (count > 0)
        {
            sum = sum / count;
            sum.Normalize();
            sum *= maxSpeed;
            Vector3 steer = Vector3.ClampMagnitude(sum - velocity, maxForce);
            return steer;
        }
        
        return Vector3.zero;
    }

    public Transform GetClosestTarget()
    {
        List<Transform> enemies = GameObject.FindGameObjectsWithTag("Danger").Select(go => go.transform.parent.gameObject.transform).ToList();
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
                distanceToClosestDanger = dSqrToTarget;
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }
}
