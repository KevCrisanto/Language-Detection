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

            if (args.Length == 2)
            {
                Learn(args[0], args[1], knownLanguagesFile);

            }
            else if (args.Length == 1)
            {
                Detect(args[0], knownLanguagesFile);
            }
            else
            {
                Test();
            }

            Console.ReadKey();
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

        static void Test()
        {
            const string testFilesPath = @"lang\";

            var learner = new LanguageLearner();

            var english = learner.Learn("en", Path.Combine(testFilesPath, "en-source.txt"));

            var dutch = learner.Learn("nl", Path.Combine(testFilesPath, "nl-source.txt"));

            var spanish = learner.Learn("es", Path.Combine(testFilesPath, "es-source.txt"));

            var bulgarian = learner.Learn("bg", Path.Combine(testFilesPath, "bg-source.txt"));

            var russian = learner.Learn("ru", Path.Combine(testFilesPath, "ru-source.txt"));

            var german = learner.Learn("de", Path.Combine(testFilesPath, "de-source.txt"));

            var languages = new Dictionary<string, Dictionary<string, int>>
                {
                    { "en", english },
                    { "nl", dutch },
                    { "es", spanish },
                    { "bg", bulgarian },
                    { "ru", russian },
                    { "de", german },
                };

            var detector = new LanguageDetector(languages);

            int scoreEnglish;
            var testEnglish = detector.Detect(Path.Combine(testFilesPath, "en-sample.txt"), out scoreEnglish);

            int scoreDutch;
            var testDutch = detector.Detect(Path.Combine(testFilesPath, "nl-sample.txt"), out scoreDutch);

            int scoreSpanish;
            var testSpanish = detector.Detect(Path.Combine(testFilesPath, "es-sample.txt"), out scoreSpanish);

            int scoreBulgarian;
            var testBulgarian = detector.Detect(Path.Combine(testFilesPath, "bg-sample.txt"), out scoreBulgarian);

            int scoreRussian;
            var testRussian = detector.Detect(Path.Combine(testFilesPath, "ru-sample.txt"), out scoreRussian);

            int scoreGerman;
            var testGerman = detector.Detect(Path.Combine(testFilesPath, "de-sample.txt"), out scoreGerman);

            Console.WriteLine("Test 1: {0} ({1})", testEnglish, scoreEnglish);

            Console.WriteLine("Test 2: {0} ({1})", testDutch, scoreDutch);

            Console.WriteLine("Test 3: {0} ({1})", testSpanish, scoreSpanish);

            Console.WriteLine("Test 4: {0} ({1})", testBulgarian, scoreBulgarian);

            Console.WriteLine("Test 5: {0} ({1})", testRussian, scoreRussian);

            Console.WriteLine("Test 6: {0} ({1})", testGerman, scoreGerman);
        }
    }
}