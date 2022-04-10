using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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


            string txt = "Всем врачам привет. Мы идём на стройку, что бы строить дом в городе Сочи под открытым небом, где летают птицы";

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
