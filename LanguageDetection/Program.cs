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
                    Console.WriteLine("This program can learn and detect languages.");
                    var detectOrLearn = string.Empty;
                    var FileOrString = string.Empty;
                    do
                    {
                        Console.Write("Do you  want to detect (D) or learn (L) a language?: ");
                        detectOrLearn = (Console.ReadLine() ?? string.Empty);
                    } while (!detectOrLearn.Equals("D", StringComparison.OrdinalIgnoreCase) &&
                        !detectOrLearn.Equals("L", StringComparison.OrdinalIgnoreCase));

                    if (detectOrLearn.Equals("D", StringComparison.OrdinalIgnoreCase))
                    {
                        do {
                            Console.Write("Do you want to detect the language of a string (S) or the text in a file (F)?: ");
                            FileOrString = (Console.ReadLine() ?? string.Empty);
                        } while (!FileOrString.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                        !FileOrString.Equals("F", StringComparison.OrdinalIgnoreCase));

                        if (FileOrString.Equals("S",StringComparison.OrdinalIgnoreCase))
                        {
                            Console.Write("Enter the text you want to detect: ");
                            var text = Console.ReadLine();
                            Console.WriteLine();
                            Detect(text, FileOrString, knownLanguagesFile);
                        }
                        else
                        {
                            Console.Write("Enter the path to the file with the text to detect: ");
                            var path = Console.ReadLine();
                            Detect(path, FileOrString, knownLanguagesFile);
                        }
                    }

                    else
                    {
                        Console.Write("Enter the code of the language you want the program to learn. Ex: for English a good code would be 'en': ");
                        var languageCode = Console.ReadLine();
                        Console.Write(@"Enter the path to the file with the data to learn the language. Ex: C:\Desktop\english.txt :");
                        var path = Console.ReadLine();
                        Console.WriteLine();
                        Learn(languageCode, path, knownLanguagesFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                do
                {
                    Console.WriteLine();
                    Console.Write("Would you like to run the program again?(Y/N): ");
                    decision = (Console.ReadLine() ?? string.Empty);
                } while (!decision.Equals("Y", StringComparison.OrdinalIgnoreCase) &&
                    !decision.Equals("N", StringComparison.OrdinalIgnoreCase));
                Console.WriteLine();
                continueDecision = decision.Equals("Y", StringComparison.OrdinalIgnoreCase);

            } while (continueDecision);
        }

        static void Learn(string languageCode, string newLanguageFile, string knownLanguagesFile)
        {
            var learner = new LanguageLearner();

            learner.Learn(languageCode, newLanguageFile, knownLanguagesFile);

            Console.WriteLine("The language '{0}' has been learned!", languageCode);
        }

        static void Detect(string file, string choice, string knownLanguagesFile)
        {
            var learner = new LanguageLearner();

            var knownLanguages = learner.Remember(knownLanguagesFile);

            var detector = new LanguageDetector(knownLanguages);

            //int score;

            var languageCode = detector.Detect(file, choice);

            Console.WriteLine("The language code of the detected language is: {0}", languageCode);
        }
    }
}