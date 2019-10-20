using System;
using ThingMagic;
using System.IO;

namespace RFID_Tag_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connecting the reader and setting base parameters
            Reader r = Reader.Create("tmr:///COM3");
            r.Connect();
            r.ParamSet("/reader/region/id", Reader.Region.NA); //Region
            r.ParamSet("/reader/radio/readPower", 2000); //Read power
            r.ParamSet("/reader/gen2/t4", 3000); //3ms continuous wave

            //Defining the Select command to the tag
            Gen2.Select tempSelect = new Gen2.Select(false, Gen2.Bank.USER, 0xE0, 0, new byte[0]);

            //Storing Temperature data in reserved memory bank pointed at 0xE
            TagOp tempRead = new Gen2.ReadData(Gen2.Bank.RESERVED, 0xE, 1);

            SimpleReadPlan readPlan = new SimpleReadPlan(new int[] { 1 }, TagProtocol.GEN2, tempSelect, tempRead, 100);
            r.ParamSet("/reader/read/plan", readPlan);
            TagReadData[] tempReadResults = r.Read(75); //Read for 75ms

            foreach (TagReadData result in tempReadResults)
            {
                string EPC = result.EpcString;
                string frequency = result.Frequency.ToString();
                string tempCodeHex = ByteFormat.ToHex(result.Data, ", ");
                int tempCode = Convert.ToInt32(tempCodeHex, 16);
                if (tempCode > 1000 && tempCode < 3500)
                    Console.WriteLine("EPC: " + EPC + "Frequency(kHz): " + frequency + "Temperature Code: " + tempCode);

            }




        }


    }

}
