﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_Annotation_Tool
{
    public class Calculations
    {
        public class JsonData
        {
            [JsonProperty("box")]
            public List<float> Box { get; set; }

            [JsonProperty("class")]
            public float Class { get; set; }

            [JsonProperty("track_id")]
            public float TrackId { get; set; }
        }

        public static List<float[]> CalcDiffs(string add1, string add2)
        {
            List<float[]> matchTIDiff = new List<float[]>();

            var addr1 = File.ReadAllText(add1);
            var addr2 = File.ReadAllText(add2);

            var jsonObject1 = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(addr1);
            var jsonObject2 = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(addr2);

            foreach (var entry1 in jsonObject1)
            {
                if (entry1.Value.TrackId == 0) { continue; }

                foreach (var entry2 in jsonObject2)
                {
                    if (entry2.Value.TrackId == 0) { continue; }

                    if (entry1.Value.TrackId == entry2.Value.TrackId)
                    {
                        Console.WriteLine($"Track ID: {entry1.Value.TrackId}");

                        float[] diffArray = new float[5];
                        diffArray[4] = entry1.Value.TrackId;

                        for (int i = 0; i < entry1.Value.Box.Count; i++)
                        {
                            float fark = entry2.Value.Box[i] - entry1.Value.Box[i];
                            diffArray[i] = fark;
                        }

                        for (int i = 0; i < diffArray.Length; i++)
                        {
                            Console.WriteLine($"Difference [{i + 1}]: {diffArray[i]}");
                        }
                        Console.WriteLine("-------------------------");

                        matchTIDiff.Add(diffArray);
                    }
                }
            }

            return matchTIDiff;
        }

        public static List<float[]> calcPI(List<float[]> CoorDiffs, int imgDiffs)
        {
            List<float[]> listPI = new List<float[]>();

            for (int i = 0; i < CoorDiffs.Count(); i++)
            {
                float[] pointIncr = new float[5];
                pointIncr[4] = CoorDiffs[i][4];

                for (int j = 0; j < 4; j++)
                {
                    pointIncr[j] = CoorDiffs[i][j] / imgDiffs;
                }

                listPI.Add(pointIncr);
            }

            for (int i = 0; i < listPI.Count(); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.WriteLine($"TrackID [{listPI[i][4]}], Box: {listPI[i][j]}");
                    Console.WriteLine($"TrackID [{CoorDiffs[i][4]}], Box: {CoorDiffs[i][j]}");
                    Console.WriteLine("-------------------------");
                }
            }

            return listPI;
        }

        public static void calcNewPoints(string prevAddr, string currAddr, List<float[]> calcPI)
        {
            var prevJson = File.ReadAllText(prevAddr);
            var currJson = File.ReadAllText(currAddr);

            var prevJsonObject = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(prevJson);
            var currJsonObject = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(currJson);

            for (int i = 0; i < calcPI.Count; i++)
            {
                foreach (var entryPrev in prevJsonObject)
                {
                    if (entryPrev.Value.TrackId == calcPI[i][4])
                    {
                        currJsonObject[entryPrev.Key] = new JsonData
                        {
                            Box = new List<float> { entryPrev.Value.Box[0] + calcPI[i][0], entryPrev.Value.Box[1] + calcPI[i][1], entryPrev.Value.Box[2] + calcPI[i][2], entryPrev.Value.Box[3] + calcPI[i][3] },
                            Class = entryPrev.Value.Class,
                            TrackId = entryPrev.Value.TrackId
                        };
                    }
                }
            }

            string updJson = JsonConvert.SerializeObject(currJsonObject, Formatting.Indented);
            File.WriteAllText(currAddr, updJson);
        }
    }
}