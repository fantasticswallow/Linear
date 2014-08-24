using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class testData1
    {
        public string Name { get; set; }
        public int Lv { get; set; }
        public string ShipType { get; set; }


        public static IEnumerable<testData1> GetData()
        {
            var l = new List<testData1>();
            l.Add(testData1.Create("Fubuki", 65, "Destroyer"));
            l.Add(testData1.Create("Haruna", 61, "BattleShip"));
            l.Add(testData1.Create("Syouhou", 58, "Light Aircraft Carrer"));
            l.Add(testData1.Create("Hiei", 59, "BattleShip"));
            l.Add(testData1.Create("Shimakaze", 50, "Destroyer"));
            l.Add(testData1.Create("Atago", 61, "Heavy Cruiser"));
            l.Add(testData1.Create("Abukuma", 39, "Light Cruiser"));
            l.Add(testData1.Create("Syokaku", 43, "Aircraft Carrer"));
            l.Add(testData1.Create("I168", 55, "Submarine"));
            l.Add(testData1.Create("Zuikaku", 52, "Aircraft Carrer"));
            l.Add(testData1.Create("Ayanami", 63, "Destroyer"));
            l.Add(testData1.Create("Zuihou", 51, "Light Aircraft Carrer"));
            l.Add(testData1.Create("Yura", 42, "Light Cruiser"));
            l.Add(testData1.Create("Ashigara", 38, "Heavy Cruiser"));
            l.Add(testData1.Create("Kongou", 53, "BattleShip"));
            return l;
        }

        internal static List<testData1> DataCache;

        public static IEnumerable<testData1> GetData2()
        {
            return DataCache.ToArray();
        }

        internal static testData1 Create(string name,int lv,string st)
        {
            var obj = new testData1();
            obj.Name = name;
            obj.Lv = lv;
            obj.ShipType = st;
            return obj;
        }

    }

    
    
}
