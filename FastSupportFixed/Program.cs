using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FastSupportFixed.DataModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace FastSupportFixed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Pullenti.Sdk.InitializeAll();

            Console.Write("Initializing SDK Pullenti ver {0} ({1}) ... ", Pullenti.Sdk.Version, Pullenti.Sdk.VersionDate);
            sw.Stop();


            foreach (Pullenti.Ner.Analyzer a in Pullenti.Ner.ProcessorService.Analyzers)
            {
                Console.WriteLine("   {0} {1} \"{2}\"", (a.IsSpecific ? "Specific analyzer" : "Common analyzer"), a.Name, a.Caption);
            }


            string txt = "Жалоба на больницу в городе Сочи, не работает";

            using (Pullenti.Ner.Processor proc = Pullenti.Ner.ProcessorService.CreateSpecificProcessor(Pullenti.Ner.Keyword.KeywordAnalyzer.ANALYZER_NAME))
            {
                Pullenti.Ner.AnalysisResult ar = proc.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
                Console.WriteLine("\r\n==========================================\r\nKeywords1: ");
                foreach (Pullenti.Ner.Referent e in ar.Entities)
                {
                    if (e is Pullenti.Ner.Keyword.KeywordReferent)
                        Console.WriteLine(e);
                }
                Console.WriteLine("\r\n==========================================\r\nKeywords2: ");
                for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next)
                {
                    if (t is Pullenti.Ner.ReferentToken)
                    {
                        Pullenti.Ner.Keyword.KeywordReferent kw = t.GetReferent() as Pullenti.Ner.Keyword.KeywordReferent;
                        if (kw == null)
                            continue;
                        string kwstr = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(t as Pullenti.Ner.ReferentToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        Console.WriteLine("{0} = {1}", kwstr, kw);
                    }
                }
            }


            // Create single instance of sample data from first line of dataset for model input
            SentimentModel.ModelInput sampleData = new SentimentModel.ModelInput()
            {
                Col0 = @"Здравствуйте. Меня вчера не приняли в отделении Сочи, хочу пожаловаться на врача, который оскоробительно общался со мной!"
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = SentimentModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Col1 with predicted Col1 from sample data...\n\n");


            Console.WriteLine($"Selected: {sampleData.Col0}");


            Console.WriteLine($"\n\nPredicted Col1: {predictionResult.PredictedLabel}\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");



            Pullenti.Ner.AnalysisResult are = Pullenti.Ner.ProcessorService.EmptyProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
            Console.Write("Noun groups: ");

            // перебираем токены
            for (Pullenti.Ner.Token t = are.FirstToken; t != null; t = t.Next)
            {
                // выделяем именную группу с текущего токена
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                // не получилось
                if (npt == null)
                    continue;
                // получилось, выводим в нормализованном виде
                Console.Write("[{0}=>{1}] ", npt.GetSourceText(), npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Singular, Pullenti.Morph.MorphGender.Undefined, false));
                // указатель на последний токен именной группы
                t = npt.EndToken;
            }

            using (Pullenti.Ner.Processor proc = Pullenti.Ner.ProcessorService.CreateProcessor())
            {
                // анализируем текст
                Pullenti.Ner.AnalysisResult ar = proc.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
                // результирующие сущности
                Console.WriteLine("\r\n==========================================\r\nEntities: ");
                foreach (Pullenti.Ner.Referent e in ar.Entities)
                {
                    Console.WriteLine("{0}: {1}", e.TypeName, e.ToString());
                    foreach (Pullenti.Ner.Slot s in e.Slots)
                    {
                        Console.WriteLine("   {0}: {1}", s.TypeName, s.Value);
                    }
                }
                // пример выделения именных групп
                Console.WriteLine("\r\n==========================================\r\nNoun groups: ");
                for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next)
                {
                    // токены с сущностями игнорируем
                    if (t.GetReferent() != null)
                        continue;
                    // пробуем создать именную группу
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.AdjectiveCanBeLast, 0, null);
                    // не получилось
                    if (npt == null)
                        continue;
                    Console.WriteLine(npt);
                    // указатель перемещаем на последний токен группы
                    t = npt.EndToken;
                }
            }


            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
