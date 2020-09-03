using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleTables;

namespace ApiClient
{
    class Capsule
    {
        [JsonPropertyName("capsule_serial")]
        public string CapsuleSerial { get; set; }
        [JsonPropertyName("capsule_id")]
        public string CapsuleId { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        //[JsonPropertyName("original_launch")]
        //public DateTime OriginalLaunch { get; set; }
        [JsonPropertyName("reuse_count")]
        public int ReuseCount { get; set; }
    }
    class Core
    {
        [JsonPropertyName("core_serial")]
        public string CoreSerial { get; set; }
        //[JsonPropertyName("block")]
        //public int Block { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        //[JsonPropertyName("original_launch")]
        //public DateTime OriginalLaunch { get; set; }
        [JsonPropertyName("reuse_count")]
        public int ReuseCount { get; set; }
    }
    class Program
    {
        static int CheckUserChoice(int options)
        {
            int choice = 1000;
            bool trueInt = false;

            while (trueInt == false)
            {
                trueInt = int.TryParse(Console.ReadLine(), out choice);
                if (trueInt == false || choice > options || choice <= 0)
                {
                    Console.WriteLine("Input was not recognized. Try again.");
                    trueInt = false;
                }
            }
            return choice;
        }
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var client = new HttpClient();
            var responseAsStreamCore = await client.GetStreamAsync("https://api.spacexdata.com/v3/cores?filter=core_serial,status,reuse_count");
            var responseAsStreamCapsule = await client.GetStreamAsync("https://api.spacexdata.com/v3/capsules?filter=capsule_serial,capsule_id,status,reuse_count");

            var cores = await JsonSerializer.DeserializeAsync<List<Core>>(responseAsStreamCore);
            var capsules = await JsonSerializer.DeserializeAsync<List<Capsule>>(responseAsStreamCapsule);

            var menu = new ConsoleTable("Choice", "Menu Options");
            menu.AddRow("( 1 )", "Look at Cores");
            menu.AddRow("( 2 )", "Look at Capsules");
            menu.AddRow("( 3 )", "Quit The Menu");

            var coresMenu = new ConsoleTable("Choice", "Status Options");
            coresMenu.AddRow("( 1 )", "See all Cores");
            coresMenu.AddRow("( 2 )", "See all Active Cores");
            coresMenu.AddRow("( 3 )", "See all Inactive Cores");
            coresMenu.AddRow("( 4 )", "see all Lost Cores");
            coresMenu.AddRow("( 5 )", "See all Cores W/ Unknown Status");

            var capsulesMenu = new ConsoleTable("Choice", "Status Options");
            capsulesMenu.AddRow("( 1 )", "See all Capsules");
            capsulesMenu.AddRow("( 2 )", "See all Active Capsules");
            capsulesMenu.AddRow("( 3 )", "See all Retired Capsules");
            capsulesMenu.AddRow("( 4 )", "see all Destroyed Capsules");
            capsulesMenu.AddRow("( 5 )", "See all Capsules W/ Unknown Status");


            bool running = true;

            while (running == true)
            {

                Console.Clear();
                Console.WriteLine("Welcome to the SpaceX Capsule & Core checker.\r\nWhy not include the Dragon module? Because it had waaayyyy too many complex properties to sort!");

                menu.Write();
                Console.WriteLine("please input a choice");
                int choice = CheckUserChoice(3);

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Choose An Option");
                        coresMenu.Write();
                        choice = CheckUserChoice(5);
                        Console.Clear();
                        var statusOptions = new List<string> { "", "active", "inactive", "lost", "unknown" };
                        var coresTable = new ConsoleTable("Status", "CoreSerial", "ReuseCount");
                        if (choice > 1)
                        {
                            var choicesToDisplay = cores.Where(Core => Core.Status == statusOptions[choice - 1]);
                            foreach (Core Core in choicesToDisplay)
                            {
                                coresTable.AddRow(Core.Status, Core.CoreSerial, Core.ReuseCount);
                            }
                        }
                        else
                            foreach (Core Core in cores)
                            {
                                coresTable.AddRow(Core.Status, Core.CoreSerial, Core.ReuseCount);
                            }
                        coresTable.Write();
                        Console.WriteLine("Press ENTER to go back to menu.");
                        Console.ReadLine();
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Choose An Option");
                        capsulesMenu.Write();
                        choice = CheckUserChoice(5);
                        Console.Clear();
                        var statusOptions2 = new List<string> { "", "active", "retired", "destroyed", "unknown" };
                        var capsulesTable = new ConsoleTable("Status", "CapsuleId", "CapsuleSerial", "ReuseCount");
                        if (choice > 1)
                        {
                            var choicesToDisplay = capsules.Where(Capsule => Capsule.Status == statusOptions2[choice - 1]);
                            foreach (Capsule Capsule in choicesToDisplay)
                            {
                                capsulesTable.AddRow(Capsule.Status, Capsule.CapsuleId, Capsule.CapsuleSerial, Capsule.ReuseCount);
                            }
                        }
                        else
                            foreach (Capsule Capsule in capsules)
                            {
                                capsulesTable.AddRow(Capsule.Status, Capsule.CapsuleId, Capsule.CapsuleSerial, Capsule.ReuseCount);
                            }
                        capsulesTable.Write();
                        Console.WriteLine("Press ENTER to go back to menu.");
                        Console.ReadLine();
                        break;

                    case 3:
                        running = false;
                        break;

                }
                Console.Clear();
                Console.WriteLine("goodbye");






            }





        }
    }
}
