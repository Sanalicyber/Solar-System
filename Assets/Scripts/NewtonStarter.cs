using System;
using System.Collections;
using System.Collections.Generic;
using Smug.Data;
using UnityEngine;

namespace Smug
{
    public class NewtonStarter : MonoBehaviour
    {
        public List<SerializablePlanetData> PlanetDataForEarth;
        public List<SerializablePlanetData> RealPlanetData;
        public List<NewtonObject> NewtonObjects = new();
        public NewtonObject Prefab;

        public NewtonSceneConfiguration Configuration;

        public bool customPlanetCount;

        public float GeneralDivider = 0.02f;
        public float Distancedivider;

        [ContextMenu("Read Planet Data")]
        public void ReadPlanetData()
        {
            PlanetDataForEarth = Reader.ReadPlanets("Assets\\Data\\PlanetDataForEarth.csv").ToSerializable();
            RealPlanetData = Reader.ReadPlanets("Assets\\Data\\PlanetData.csv").ToSerializable();
        }

        public void Start()
        {
            IEnumerator delayStart()
            {
                Init();
                yield return new WaitForSeconds(3f);
                Newton.Start();
            }

            StartCoroutine(delayStart());
        }

        private void Init()
        {
            if (PlanetDataForEarth.Count == 0)
                ReadPlanetData();

            if (!customPlanetCount)
                Configuration.PlanetCount = PlanetDataForEarth.Count;

            for (int i = 0; i < Configuration.PlanetCount; i++)
            {
                var obj = Instantiate(Prefab);
                NewtonObjects.Add(obj);
            }

            for (int i = 0; i < RealPlanetData.Count; i++)
            {
                var planet = NewtonObjects[i];
                planet.Mass = RealPlanetData[i].Mass * GeneralDivider;
                var planetTransform = planet.transform;
                planetTransform.position = Distancedivider * RealPlanetData[i].DistanceFromSun * Vector3.forward;

                planetTransform.localScale *= RealPlanetData[i].Radius * 2 * GeneralDivider;
                planet.name = RealPlanetData[i].Identifier;
                switch (planet.name)
                {
                    case PlanetName.Sun:
                        planet.isLocked = Configuration.SunIsStatic;
                        planet.GetComponent<MeshRenderer>().enabled = Configuration.SunIsVisible;
                        break;
                    case PlanetName.Moon:
                        planet.Acceleration = GeneralDivider * RealPlanetData[i].OrbitalSpeed * Vector3.up;
                        break;
                }
            }

            // for (int i = 0; i < PlanetDataForEarth.Count; i++)
            // {
            //     var planet = NewtonObjects[i];
            //     planet.Mass *= PlanetDataForEarth[i].Mass;
            //     var planetTransform = planet.transform;
            //     var position = planetTransform.position;
            //     position = new Vector3(position.x, position.y, position.z * PlanetDataForEarth[i].DistanceFromSun);
            //     planetTransform.position = position;
            //     //planet.Acceleration = new Vector3(planet.Acceleration.x * GeneralDivider * PlanetDataForEarth[i].OrbitalSpeed, 0, 0);
            //     planetTransform.localScale *= PlanetDataForEarth[i].Radius;
            // }
        }

        public void Reset()
        {
            Newton.Reset();
        }

        private void OnDestroy()
        {
            ThreadChecker.IsRunning = false;
        }
    }

    public class PlanetName
    {
        public const string Sun = "SOL";
        public const string Mercury = "MER";
        public const string Venus = "VEN";
        public const string Earth = "HOM";
        public const string Moon = "LUN";
        public const string Mars = "MARS";
        public const string Jupiter = "JUP";
        public const string Saturn = "SAT";
        public const string Uranus = "URN";
        public const string Neptune = "NEP";
        public const string Pluto = "PLU";
    }

    [Serializable]
    public class NewtonSceneConfiguration
    {
        public int PlanetCount;
        public bool SunIsCenter;
        public bool SunIsStatic;
        public bool SunIsVisible;
    }
}