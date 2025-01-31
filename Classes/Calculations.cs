using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Image_Annotation_Tool
{
    public class Calculations
    {
        public class Item
        {
            public string Text { get; set; }
            public int ID { get; set; }
            public override string ToString() => Text;
        }

        //Defination of Json Data Format
        public class JsonData
        {
            [JsonProperty("box")]
            public List<float> Box { get; set; }

            [JsonProperty("class")]
            public float Class { get; set; }

            [JsonProperty("track_id")]
            public float TrackId { get; set; }
        }

        //Calculation of 2 Different Json Files' Box Value Differences
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

                        float[] diffArray = new float[5];
                        diffArray[4] = entry1.Value.TrackId;

                        for (int i = 0; i < entry1.Value.Box.Count; i++)
                        {
                            float fark = entry2.Value.Box[i] - entry1.Value.Box[i];
                            diffArray[i] = fark;
                        }

                        matchTIDiff.Add(diffArray);
                    }
                }
            }

            return matchTIDiff;
        }

        //Calculation of Incrementation Values
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

            return listPI;
        }

        //Calculation of New Points
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
                        currJsonObject[avaBoxId(currJsonObject, Convert.ToInt32(entryPrev.Value.TrackId)).ToString()] = new JsonData
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

        public static void calcSepBox(string prevAddr, string currAddr, List<float[]> calcPI, int selectedBoxTrack)
        {

            var prevJson = File.ReadAllText(prevAddr);
            var currJson = File.ReadAllText(currAddr);

            var prevJsonObject = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(prevJson);
            var currJsonObject = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(currJson);

            for (int i = 0; i < calcPI.Count; i++)
            {
                foreach (var entryPrev in prevJsonObject)
                {
                    if (entryPrev.Value.TrackId == calcPI[i][4] && selectedBoxTrack == entryPrev.Value.TrackId)
                    {
                        currJsonObject[avaBoxId(currJsonObject, Convert.ToInt32(entryPrev.Value.TrackId)).ToString()] = new JsonData
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

        public static int avaBoxId(Dictionary<string, JsonData> jsonObject, int prevId)
        {
            List<int> boxIds = new List<int>();
            foreach (var entry in jsonObject)
            {
                if(entry.Value.TrackId == prevId) { return Convert.ToInt32(entry.Key); }
                boxIds.Add(Convert.ToInt32(entry.Key));
            }

            if (boxIds.Count == 0)
            {
                return 0;
            }
            else { return boxIds.Max() + 1; }
        }
    }
}