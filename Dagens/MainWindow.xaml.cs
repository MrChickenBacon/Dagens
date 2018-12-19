using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Speech.Synthesis;
using System.Threading;

namespace Dagens
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Random Random = new Random();
        private static readonly SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        public MainWindow()
        {
            InitializeComponent();
            synthesizer.SelectVoice("Microsoft Jon");
        }

        private void Speak(object sender, RoutedEventArgs e)
        {
            
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = 0;     // -10...10

            // Asynchronous
            //synthesizer.SpeakAsync("Text");
        }

        public  void Button_Click(object sender, RoutedEventArgs e)
        {
            var list = ReturnArray();
            list = FilterListByLength(list);
            var getRandom = Chosenword(list);

            Output.Content = (getRandom);
            // Text to speach under
            Speak(sender, e);
            synthesizer.SpeakAsync($"Dagens ord er {getRandom}");
            Thread.Sleep(500);
            synthesizer.SpeakAsync($"Jeg vet ikke hva det betyr. Spør heller Emil.");
            Console.WriteLine(getRandom);
        }

        private static string getThreeRandom(string getRandom)
        {
            char[] chararray = new char[3];
            chararray[0] = getRandom[getRandom.Length - 3];
            chararray[1] = getRandom[getRandom.Length - 2];
            chararray[2] = getRandom[getRandom.Length - 1];
            return new string(chararray);
        }

        public static string Chosenword(string[] list)
        {
            int i = Random.Next(0, list.Length + 1);
            return list[i];
        }

        public static string[] FilterListByLength(string[] list)
        {
            var filteredList = new List<string>();
            foreach (var word in list)
            {
                if (word.Contains('-')) continue;
                if (word.Length < 7 || word.Length > 10) continue;
                filteredList.Add(word);
            }
            return filteredList.ToArray();
        }

        public static string[] ReturnArray()
        {
            var list = new List<string>();
            string lastword = null;
            foreach (var line in File.ReadLines("ordbok.txt"))
            {
                var words = line.Split('\t');
                var word = words[1];

                if (lastword != word)
                {
                    lastword = word;
                    list.Add(word);
                }

            }
            return list.ToArray();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            synthesizer.SpeakAsyncCancelAll();
        }
    }
}

