using UnityEngine;

namespace DefaultNamespace
{
    public interface IGiveTarget
    {
        public Transform GetClosestTarget();
    }
}