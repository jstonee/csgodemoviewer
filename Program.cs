using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DemoInfo;

namespace csgodemoviewer
{
  class Program
  {
    static void Main(string[] args)
    {
      var watch = new System.Diagnostics.Stopwatch();
      Dictionary<Player, int> killsPerPlayer = new Dictionary<Player, int>();
      watch.Start();
      using (var file = File.OpenRead("demo3.dem"))
      {
        using (var demo = new DemoParser(file))
        {
          demo.ParseHeader();

          string map = demo.Map;

          demo.MatchStarted += (sender, e) =>
          {
            foreach (var player in demo.PlayingParticipants)
            {
              killsPerPlayer[player] = 0;
            }
          };

          demo.PlayerKilled += (object sender, PlayerKilledEventArgs e) =>
          {
            if(e.Killer != null)
            {
              if (killsPerPlayer.ContainsKey(e.Killer))
                killsPerPlayer[e.Killer]++;
            }
          };
          demo.ParseToEnd();
        }
      }
      watch.Stop();
      Console.WriteLine("Base Run time: " + watch.Elapsed);
      foreach(KeyValuePair<Player, int> x in killsPerPlayer)
      {
        Console.WriteLine("{0} : {1}", x.Key.Name, x.Value);
      }
      Console.ReadKey();
    }
  }
}
