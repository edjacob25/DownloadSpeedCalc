using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DownloadSpeeder
{

    class Options
    {
        [Value(0, Required = true, HelpText = "How many units are you going to download, default is GB", MetaName = "Download size")]
        public float Size { get; set; }

        [Option('s', "Download speed", Required = true)]
        public float Speed { get; set; }

        [Option('d', "Download speed units", Required = false, Default = "Mb")]
        public string SpeedUnits { get; set; }

        [Option('i', "Item size units", Required = false, Default = "GB")]
        public string SizeUnits { get; set; }

        public static Dictionary<string, long> AvailableSpeedUnits = new Dictionary<string, long>() {
            { "Kb", 1_024 },
            { "Mb", 1_000_000 },
            { "Gb", 1_073_741_824 }};

        public static Dictionary<string, long> AvailableSizeUnits = new Dictionary<string, long>() {
            { "KB", 1_024 },
            { "MB", 1_048_576 },
            { "GB", 1_073_741_824 },
            { "TB", 1_099_511_627_776 }};
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => 
                {
                    if (!Options.AvailableSpeedUnits.ContainsKey(o.SpeedUnits))
                    {
                        Console.WriteLine(Options.AvailableSpeedUnits
                            .Aggregate("Speed unit not recognized, please use one of the following: ", (acc, item) => $"{acc}, {item.Key}"));
                        return;
                    }
                    if (!Options.AvailableSizeUnits.ContainsKey(o.SizeUnits))
                    {
                        Console.WriteLine(Options.AvailableSizeUnits
                            .Aggregate("Size unit not recognized, please use one of the following: ", (acc, item) => $"{acc}, {item.Key}"));
                        return;
                    }
                    Check(o);
                });
        }

        private static void Check(Options ops)
        {
            var bytes = ops.Size * Options.AvailableSizeUnits.GetValueOrDefault(ops.SizeUnits);
            var bits = ops.Speed * Options.AvailableSpeedUnits.GetValueOrDefault(ops.SpeedUnits);

            var seconds = Math.Round(bytes / (bits / 8));
            var time = TimeSpan.FromSeconds(seconds);

            Console.WriteLine($"A file of {ops.Size}{ops.SizeUnits} at a download " +
                $"rate of {ops.Speed}{ops.SpeedUnits}/s will take {seconds} seconds, " +
                $"which is {time} ");

            
        }
    }
}
