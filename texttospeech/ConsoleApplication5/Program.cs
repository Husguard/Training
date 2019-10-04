using System;
using System.Speech.Synthesis;

namespace SampleSynthesis
{
    class Program
    {
        static void Main(string[] args)
        {

            // Initialize a new instance of the SpeechSynthesizer.
            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output. 
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.
            synth.Speak("This example demonstrates a basic use of Speech Synthesizer");
            synth.Speak("Okay... Now i want to listen this response: Please be adviced, the following presentation is not intended for miners");

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}