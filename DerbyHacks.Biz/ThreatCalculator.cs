using DerbyHacks.Model;
using DerbyHacksApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DerbyHacks.Biz
{
    public class ThreatCalculator
    {
        private Dictionary<ThreatLevel, int> threatThresholds;

        private Block block;

        public ThreatLevel ThreatLevel
        {
            get
            {
                if (threatThresholds.Count == 0)
                {
                    throw new InvalidOperationException("Please add threshold.");
                }

                int severity = calculate();

                foreach (var item in threatThresholds.OrderByDescending(t => t.Key))
                {
                    if (item.Value < severity)
                    {
                        return item.Key;
                    }
                }

                return ThreatLevel.None;
            }
        }

        public ThreatCalculator(Block _block)
        {
            block = _block;
            threatThresholds = new Dictionary<ThreatLevel, int>();
        }

        public void AddThreshold(ThreatLevel level, int value)
        {
            int currentVal;

            if (level == ThreatLevel.None)
            {
                return;
            }

            if (!threatThresholds.TryGetValue(level, out currentVal))
            {
                threatThresholds.Add(level, value);
            }
            else
            {
                threatThresholds[level] = value;
            }
        }

        public void CalculateCrimeRatios()
        {
            int count = 0;

            Dictionary<string, int> map = new Dictionary<string, int>();
            map.Add("Arson".ToUpper(), 0);
            map.Add("Assault".ToUpper(), 0);
            map.Add("Burgulary".ToUpper(), 0);
            map.Add("DisturbingThePeace".ToUpper(), 0);
            map.Add("Drugs".ToUpper(), 0);
            map.Add("Dui".ToUpper(), 0);
            map.Add("Fraud".ToUpper(), 0);
            map.Add("Homicide".ToUpper(), 0);
            map.Add("MotorVehicleTheft".ToUpper(), 0);
            map.Add("Other".ToUpper(), 0);
            map.Add("Robbery".ToUpper(), 0);
            map.Add("SexCrimes".ToUpper(), 0);
            map.Add("Theft".ToUpper(), 0);
            map.Add("Vandalism".ToUpper(), 0);
            map.Add("VehicleBreakIn".ToUpper(), 0);
            map.Add("Weapons".ToUpper(), 0);

            foreach (CrimeData indident in block.Incidents)
            {
                count++;
                int currentCount = 0;
                if (!map.TryGetValue(indident.CrimeType, out currentCount))
                {
                    map.Add(indident.CrimeType, currentCount);
                }
                else
                {
                    map[indident.CrimeType] = currentCount++;
                }
            }

            List<CrimeRatio> crimeRatios = new List<CrimeRatio>();
            foreach (KeyValuePair<string, int> entry in map)
            {
                CrimeRatio ratio = new CrimeRatio(entry.Key, entry.Value, count);
                crimeRatios.Add(ratio);
            }

            block.CrimeRatios = crimeRatios;
        }
        private int calculate()
        {
            int currentThreatLevel = 0;

            Dictionary<string, ThreatType> map = new Dictionary<string, ThreatType>();
            map.Add("ARSON", ThreatType.Arson);
            map.Add("ASSAULT", ThreatType.Assault);
            map.Add("BURGULARY", ThreatType.Burgulary);
            map.Add("DISTURBINGTHEPEACE", ThreatType.DisturbingThePeace);
            map.Add("DRUGS", ThreatType.Drugs);
            map.Add("DUI", ThreatType.Dui);
            map.Add("FRAUD", ThreatType.Fraud);
            map.Add("HOMICIDE", ThreatType.Homicide);
            map.Add("MOTOR VEHICLE THEFT", ThreatType.MotorVehicleTheft);
            map.Add("OTHER", ThreatType.Other);
            map.Add("ROBBERY", ThreatType.Robbery);
            map.Add("SEXCRIMES", ThreatType.SexCrimes);
            map.Add("THEFT", ThreatType.Theft);
            map.Add("VANDALISM", ThreatType.Vandalism);
            map.Add("VEHICLEBREAKIN", ThreatType.Vandalism);
            map.Add("WEAPONS", ThreatType.Weapons);

            foreach (CrimeData indident in block.Incidents)
            {
                ThreatType scalar;
                if (map.TryGetValue(indident.CrimeType.Replace(" ",""), out scalar))
                {
                    currentThreatLevel = currentThreatLevel + (int)scalar;
                }
            }
            return currentThreatLevel;

        }
    }

    public enum ThreatLevel
    {
        None = 0,
        Low = 1,
        Moderate = 2,
        High = 3,
        Critical = 4
    }

    public enum ThreatType
    {
        Arson = 7,
        Assault = 11,
        Burgulary = 12,
        DisturbingThePeace = 4,
        Drugs = 6,
        Dui = 10,
        Fraud = 2,
        Homicide = 15,
        MotorVehicleTheft,
        Other = 1,
        Robbery = 13,
        SexCrimes = 14,
        Theft = 9,
        Vandalism = 3,
        VehicleBreakIn = 5,
        Weapons = 8
    }
}
