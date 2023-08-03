using System.Collections.Generic;
using UnityEngine;

namespace Smug
{
    public static class Newton
    {
        public static List<NewtonObject> Objects = new ();

        private static float G_Raw = 667408F;
        private static float MultiplierG = 1E-3F;
        public static float G => G_Raw * MultiplierG;

        public static NewtonTask.TaskDelegate Cycle()
        {
            NewtonTask.TaskDelegate task = () =>
            {
                foreach (NewtonObject obj in Objects)
                {
                    foreach (NewtonObject obj1 in Objects)
                    {
                        if (obj.Equals(obj1)) continue;
                        float differenceX = obj1.Position.x - obj.Position.x;
                        float differenceY = obj1.Position.y - obj.Position.y;
                        float differenceZ = obj1.Position.z - obj.Position.z;
                        float distance = Vector3.Distance(obj.Position, obj1.Position);
                        float force = obj.Mass * obj1.Mass / Mathf.Pow(distance, 2);

                        obj.ApplyForce(
                            force * differenceX / distance,
                            force * differenceY / distance,
                            force * differenceZ / distance);
                    }
                }
            };

            return task;
        }

        public static bool AddObject(NewtonObject obj)
        {
            if (Objects.Contains(obj))
                return false;

            Objects.Add(obj);

            return true;
        }

        public static bool Start()
        {
            if (Objects.Count == 0) return false;
            ThreadChecker.IsRunning = true;
            ThreadChecker.StartThread();

            foreach (var newtonObject in Objects)
            {
                foreach (var other in Objects)
                {
                    if (newtonObject.Equals(other)) continue;
                    var distance = Vector3.Distance(newtonObject.Position, other.Position);
                    var mass = other.Mass;
                    var velocity = InitialOrbitalVelocity(mass, distance);
                    newtonObject.SetInitialVelocity(velocity);
                }
            }

            return true;
        }

        public static void Reset()
        {
            ThreadChecker.Reset();
            foreach (var newtonObject in Objects)
            {
                newtonObject.Reset();
            }
        }

        public static float InitialOrbitalVelocity(float m1, float distance)
        {
            var velocitySquare =  m1 / distance; /*(m1 + m2) / (2 / distance - 1 / (distance * 1.5f));*/
            return Mathf.Sqrt(velocitySquare);
        }
    }
}