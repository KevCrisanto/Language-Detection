using LanguageDetection.Workers;
using System;
using System.Collections.Generic;
using System.IO;

namespace LanguageDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            const string knownLanguagesFile = @"known_languages.txt";
            bool continueDecision;
            do
            {
                var decision = string.Empty;
                try
                {
                    Console.WriteLine("The program can detect a language, learn a new language, or simply test the current supported languages.");
                    Console.WriteLine(@"- To detect a language just type the path to the file with the text. Ex: C:\text.txt");
                    Console.WriteLine(@"- To learn a language type the language code and the path to the file with the sample text. Ex: pl C:\polish.txt");
                    Console.Write("Enter your command: ");
                    string choice = (Console.ReadLine() ?? string.Empty);

                    if (choice != string.Empty)
                    {
                        var choices = choice.Split(' ');

                        if (choices.Length == 2)
                        {
                            Learn(choices[0], choices[1], knownLanguagesFile);
                        }
                        else if (choices.Length == 1)
                        {
                            Detect(choices[0], knownLanguagesFile);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid action.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                do
                {
                    Console.Write("Would you like to run the program again?(Y/N): ");
                    decision = (Console.ReadLine() ?? string.Empty);
                } while (!decision.Equals("Y", StringComparison.OrdinalIgnoreCase) &&
                    !decision.Equals("N", StringComparison.OrdinalIgnoreCase));

                continueDecision = decision.Equals("Y", StringComparison.OrdinalIgnoreCase);

            } while (continueDecision);
        }

        static void Learn(string languageCode, string newLanguageFile, string knownLanguagesFile)
        {
            var learner = new LanguageLearner();

            learner.Learn(languageCode, newLanguageFile, knownLanguagesFile);

            Console.WriteLine("The language '{0}' has been learned!", languageCode);
        }

        static void Detect(string file, string knownLanguagesFile)
        {
            var learner = new LanguageLearner();

            var knownLanguages = learner.Remember(knownLanguagesFile);

            var detector = new LanguageDetector(knownLanguages);

            int score;

            var languageCode = detector.Detect(file, out score);

            Console.WriteLine("The language code of the detected language is: {0} ({1})", languageCode, score);
        }
    }
}