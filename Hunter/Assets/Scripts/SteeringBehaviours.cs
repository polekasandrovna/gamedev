using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviours : MonoBehaviour
{
    [SerializeField] protected float maxSpeed, maxForce;
    protected Vector3 velocity, location, acceleration, startPosition;
    
    protected void Start()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        location = transform.position;
        startPosition = transform.position;
    }
    protected Vector3 AvoidEdges()
    {
        if (location.x < - FindObjectOfType<Spawner>().SpawnRangeX + FindObjectOfType<Spawner>().Bounds)
        {
            return SteerForWallAvoid(new Vector2(maxSpeed, velocity.y));
        }
        if (location.x > FindObjectOfType<Spawner>().SpawnRangeX - FindObjectOfType<Spawner>().Bounds)
        {
            return SteerForWallAvoid(new Vector2(-maxSpeed, velocity.y));
        }
        if (location.y < -FindObjectOfType<Spawner>().SpawnRangeY + FindObjectOfType<Spawner>().Bounds)
        {
            return SteerForWallAvoid(new Vector2(velocity.x, maxSpeed));
        }
        if (location.y > FindObjectOfType<Spawner>().SpawnRangeY - FindObjectOfType<Spawner>().Bounds)
        {
            return SteerForWallAvoid(new Vector2(velocity.x, -maxSpeed));
        }
        
        return Vector3.zero;
    }
    private Vector3 SteerForWallAvoid(Vector3 desired)
    {
        Vector3 steer = Vector3.ClampMagnitude(desired - velocity, maxForce);
        return steer;
    }
    protected void RotateTowardTarget()
    {
        Vector3 dirToDesiredLoc = location - transform.position;
        dirToDesiredLoc.Normalize();
        float rotZ = Mathf.Atan2(dirToDesiredLoc.y, dirToDesiredLoc.x) * Mathf.Rad2Deg;
        rotZ -= 90;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    protected Vector3 Steer(Vector3 targetPosition)
    {
        Vector3 desired = targetPosition - location;
        desired.Normalize();
        desired *= maxSpeed;
        Vector3 steer = Vector3.ClampMagnitude(desired - velocity, maxForce);
        return steer;
    }
    protected Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 desired = targetPosition - location;
        desired.Normalize();
        desired *= -maxSpeed;
        Vector3 steer = Vector3.ClampMagnitude(desired - velocity, maxForce);
        return steer;
    }
    protected void ApplyForce(Vector3 force)
    {
        acceleration += force;
    }
    protected void ApplySteeringToMotion()
    {
        velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
        location += velocity * Time.deltaTime;
        acceleration = Vector3.zero;
        RotateTowardTarget();
        transform.position = location;
    }
}
