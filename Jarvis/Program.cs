using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace Jarvis
{
    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        /// <summary>
        /// Where the magic happens
        /// </summary>
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            // Greeting
            Console.WriteLine("Greetings");
            synth.Speak("Welcome to Jarvis Version one point oh");

            // Pull CPU load
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCpuCount.NextValue();

            // Pull Memory In MBytes
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            perfMemCount.NextValue();

            // Pull Uptime (Seconds)
            PerformanceCounter perfUptimeCount = new PerformanceCounter("System", "System Up Time");
            perfUptimeCount.NextValue();

            // System Uptime
            string day = "day";
            string days = "days";
            TimeSpan uptimeSpan = TimeSpan.FromSeconds(perfUptimeCount.NextValue());
            if (uptimeSpan.TotalDays > 1) day = "days";
            string systemUpTimeMessage = string.Format("The current system up time is {0}" + day + " {1} hours {2} minutes {3} seconds",
                (int) uptimeSpan.TotalDays,
                uptimeSpan.Hours,
                uptimeSpan.Minutes,
                uptimeSpan.Seconds
                );

            // System Uptime Console
            string systemUpTimeMessageConsole = string.Format("The current system up time is {0} day(s) {1} hours {2} minutes {3} seconds",
                (int) uptimeSpan.TotalDays,
                (int) uptimeSpan.Hours,
                (int) uptimeSpan.Minutes,
                (int) uptimeSpan.Seconds
                );

            // Tell the user the system uptime
            Console.WriteLine(systemUpTimeMessageConsole);
            Speak(systemUpTimeMessage, VoiceGender.Male, 1);

            // Infinite While Loop
            while (true)
            {
                // Get current performance counter values
                int currentcpupercentage = (int) perfCpuCount.NextValue();
                int currentmemavailable = (int) perfMemCount.NextValue();

                // Every second print cpu load in percentage
                Console.WriteLine("CPU Load: {0}%", (int)currentcpupercentage);
                Console.WriteLine("Available Memory: {0}MB", (int)currentmemavailable);
                Thread.Sleep(1000);

                // Only speak if CPU Load is over 50%
                if (currentcpupercentage > 80)
                {
                    if (currentcpupercentage == 100)
                    {
                        string cpuLoadVocalMessage = string.Format("WARNING: YOUR CPU IS ABOUT TO CATCH FIRE", currentcpupercentage);
                        Speak(cpuLoadVocalMessage, VoiceGender.Male, 5);
                    }
                    else {
                        string cpuLoadVocalMessage = string.Format("The Current CPU Load is {0} percent", currentcpupercentage);
                        Speak(cpuLoadVocalMessage, VoiceGender.Male, 3);
                    }
                }
                
                // Only speak if memory available is under a gig
                if(currentmemavailable < 1024)
                {
                    string memAvailableVocalMessage = String.Format("You currently have {0} megabytes of memory available", currentmemavailable);
                    Speak(memAvailableVocalMessage, VoiceGender.Male);
                }

            } // end of infinite loop
        }

        public static void Speak(string message, VoiceGender voiceGender)
        {
            synth.SelectVoiceByHints(voiceGender);
            string memAvailableVocalMessage = message;
            synth.Speak(memAvailableVocalMessage);
        }


        // Speaks with a gender voice and rate
        public static void Speak(string message, VoiceGender voiceGender, int rate)
        {
            synth.Rate = rate;
            string memAvailableVocalMessage = message;
            synth.Speak(memAvailableVocalMessage);
        }
    }
}
