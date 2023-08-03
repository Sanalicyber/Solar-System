using System;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Smug.Data
{
    [Serializable]
    public class PlanetData
    {
        [Index(0)]
        public string Identifier { get; set; }
        [Index(1)]
        public float Radius { get; set; }
        [Index(2)]
        public float Mass { get; set; }
        [Index(3)]
        public float OrbitalSpeed { get; set; }
        [Index(4)]
        public float AngularSpeed { get; set; }
        [Index(5)]
        public float DistanceFromSun { get; set; }

        public SerializablePlanetData ToSerializable()
        {
            return new SerializablePlanetData
            {
                Identifier = Identifier,
                Radius = Radius,
                Mass = Mass,
                OrbitalSpeed = OrbitalSpeed,
                AngularSpeed = AngularSpeed,
                DistanceFromSun = DistanceFromSun
            };
        }
    }

    [Serializable]
    public class SerializablePlanetData
    {
        public string Identifier;
        public float Radius;
        public float Mass;
        public float OrbitalSpeed;
        public float AngularSpeed;
        public float DistanceFromSun;

        public SerializablePlanetData(string identifier, float radius, float mass, float orbitalSpeed, float angularSpeed, float distanceFromSun)
        {
            Identifier = identifier;
            Radius = radius;
            Mass = mass;
            OrbitalSpeed = orbitalSpeed;
            AngularSpeed = angularSpeed;
            DistanceFromSun = distanceFromSun;
        }

        public SerializablePlanetData()
        {
            
        }
    }

    public class PlanetDataMap : ClassMap<PlanetData>
    {
        public PlanetDataMap()
        {
            Map(m => m.Identifier).Name("Planet");
            Map(m => m.Radius).Name("Radius");
            Map(m => m.Mass).Name("Mass");
            Map(m => m.OrbitalSpeed).Name("Orbital Speed");
            Map(m => m.AngularSpeed).Name("Angular Speed");
            Map(m => m.DistanceFromSun).Name("Distance From Sun");
        }
    }
}