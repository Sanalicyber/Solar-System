using UnityEngine;

namespace Smug
{
    public class NewtonObject : MonoBehaviour
    {
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public Vector3 Position;

        public Vector3 StartPos;

        public PlanetEnum PlanetType;

        public bool isLocked;

        public float Mass;

        private void Start()
        {
            StartPos = transform.position;
            Position = StartPos;
            Newton.AddObject(this);
        }

        public void SetInitialVelocity(float velocity)
        {
            Acceleration.x += velocity;
        }

        public void ApplyForce(Vector3 force)
        {
            Acceleration += force;
        }

        public void ApplyForce(float x, float y, float z)
        {
            ApplyForce(new Vector3(x, y, z));
        }

        public void ApplyForce(float x, float y)
        {
            ApplyForce(new Vector3(x, y, 0));
        }

        private void FixedUpdate()
        {
            if (!ThreadChecker.IsRunning) return;
            Position = transform.position;
            if (isLocked) return;
            Velocity += Acceleration;
            transform.Translate(Velocity);
            Acceleration = Vector3.zero;
        }

        public void Reset()
        {
            Velocity = Vector3.zero;
            Acceleration = Vector3.zero;
            transform.position = StartPos;
            Position = StartPos;
        }
    }
}