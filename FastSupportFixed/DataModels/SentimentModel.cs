using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace FastSupportFixed.DataModels
{
    public partial class SentimentModel
    {
        /// <summary>
        /// model input class for SentimentModel.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"col0")]
            public string Col0 { get; set; }

            [ColumnName(@"col1")]
            public float Col1 { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for SentimentModel.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"col0")]
            public float[] Col0 { get; set; }

            [ColumnName(@"col1")]
            public uint Col1 { get; set; }

            [ColumnName(@"Features")]
            public float[] Features { get; set; }

            [ColumnName(@"PredictedLabel")]
            public float PredictedLabel { get; set; }

            [ColumnName(@"Score")]
            public float[] Score { get; set; }

        }

        #endregion

        private static string MLNetModelPath = Path.GetFullPath("SentimentModel.zip");

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
    }


    public partial class SentimentModel
    {
        public static ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {
            var pipeline = BuildPipeline(context);
            var model = pipeline.Fit(trainData);

            return model;
        }

        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Text.FeaturizeText(inputColumnName: @"col0", outputColumnName: @"col0")
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new[] { @"col0" }))
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: @"col1", inputColumnName: @"col1"))
                                    .Append(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"))
                                    .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(new LbfgsMaximumEntropyMulticlassTrainer.Options() { L1Regularization = 1F, L2Regularization = 1F, LabelColumnName = @"col1", FeatureColumnName = @"Features" }))
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: @"PredictedLabel", inputColumnName: @"PredictedLabel"));

            return pipeline;
        }
    }
}
