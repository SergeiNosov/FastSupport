using System;
using System.Collections.Generic;
using FastSupportFixed.DataModels;

namespace FastSupportFixed.DataAnalysis
{
    internal class DataAnalyzer
    {
        internal DataAnalyzer()
        {
           
        }


        internal KeywordsAnalizeInfo GetKeyWords(string txt)
        {
            KeywordsAnalizeInfo keywordsAnalizeInfo = new KeywordsAnalizeInfo();

            using (Pullenti.Ner.Processor proc = Pullenti.Ner.ProcessorService.CreateSpecificProcessor(Pullenti.Ner.Keyword.KeywordAnalyzer.ANALYZER_NAME))
            {
               

                Pullenti.Ner.AnalysisResult ar = proc.Process(new Pullenti.Ner.SourceOfAnalysis(txt), null, null);
              
                foreach (Pullenti.Ner.Referent e in ar.Entities)
                {
                    //if (e is Pullenti.Ner.Keyword.KeywordReferent)
                    //  Console.WriteLine(e);

                    if (e is Pullenti.Ner.Keyword.KeywordReferent)
                    {
                        keywordsAnalizeInfo.keywords.Add(e.ToString());
                    }

                    if(e is Pullenti.Ner.Keyword.KeywordReferent b && b.Typ == Pullenti.Ner.Keyword.KeywordType.Annotation)
                    {
                        keywordsAnalizeInfo.shortDescription = e.ToString();
                    }
                }

                /*
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
                }*/
            }

            return keywordsAnalizeInfo;
        }

        internal void GetShortDescription(string text)
        {

        }

        internal void DetectMainCategory()
        {

        }

        internal void DetectSubCategory()
        {

        }

        internal void DetectPositiveOrNegative()
        {

        }

        internal void DrfineUrgency() //Определить срочность
        {
            
        }


        /// <summary>
        /// 0 - positive
        /// 1 - negative
        /// 3 - neutral
        /// </summary>
        /// <returns></returns>
        internal int GetEmotionMessage(string txt)
        {
            // Create single instance of sample data from first line of dataset for model input
            SentimentModel.ModelInput sampleData = new SentimentModel.ModelInput()
            {
                Col0 = txt
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = SentimentModel.Predict(sampleData);
            return Convert.ToInt32(predictionResult.PredictedLabel);
        }

    }
}


internal class KeywordsAnalizeInfo
{
    public string shortDescription = string.Empty;
    public List<string> keywords = new List<string>();
}