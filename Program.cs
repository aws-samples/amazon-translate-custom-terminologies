using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Translate;
using Amazon.Translate.Model;
using Microsoft.Extensions.Configuration;

namespace translate_terminologies {
	class Program {
		const string EnglishText = @"Amazon Translate is a text translation service that uses advanced machine learning technologies to provide high-quality translation on demand.";
		const string TerminologyName = "Preserve_Amazon_Translate";
		static void Main(string[] args) {
			var awsOptions = BuildAwsOptions();
			var service = new TranslateService(awsOptions.CreateServiceClient<IAmazonTranslate>());

			// simple translation
			var translateTask = service.TranslateText(EnglishText, "en", "hi");
			translateTask.Wait();
			var result = translateTask.Result;
			var translatedText = result.TranslatedText;
			Console.WriteLine("Source: {0}", EnglishText);
			Console.WriteLine("Translation: {0}", translatedText);


			// translation with custom Terminology
			// read the csv terminology file
			var memoryStream = new MemoryStream();
			var fileStream = new FileStream("custom-terminology.csv", FileMode.Open);
			fileStream.CopyTo(memoryStream);

			// set terminology
			Console.WriteLine("Setting a custom Terminology...");
			service.SetTerminolgy(TerminologyName, memoryStream).Wait();

			// query with terminology
			var terminologies = new List<string>() { TerminologyName };
			translateTask = service.TranslateText(EnglishText, "en", "hi", terminologies);
			result = translateTask.Result;
			translatedText = result.TranslatedText;
			Console.WriteLine("After applying custom terminology:");
			translateTask.Result.AppliedTerminologies.ForEach(x => {
				Console.WriteLine(x.Name);
			});
			Console.WriteLine("Translation: {0}", translatedText);
			fileStream.Close();
			fileStream.Dispose();
		}

		private static AWSOptions BuildAwsOptions() {
			var builder = new ConfigurationBuilder()
				.SetBasePath(Environment.CurrentDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();
			return builder.GetAWSOptions();
		}
	}

	public class TranslateService {
		private IAmazonTranslate translate;
		public TranslateService(IAmazonTranslate translate) {
			this.translate = translate;
		}

		public async Task<ImportTerminologyResponse> SetTerminolgy(string name, MemoryStream fileStream) {
			return await this.translate.ImportTerminologyAsync(new ImportTerminologyRequest {
				Name = name,
				MergeStrategy = MergeStrategy.OVERWRITE,
				TerminologyData = new TerminologyData {
					File = fileStream,
					Format = TerminologyDataFormat.CSV
				}
			});
		}

		public async Task<TranslateTextResponse> TranslateText(string text, string sourceLanguage, string targetLanguage) {
			return await this.TranslateText(text, sourceLanguage, targetLanguage, null);
		}

		public async Task<TranslateTextResponse> TranslateText(string text, string sourceLanguage, string targetLanguage, List<string> terminologies) {
			var request = new TranslateTextRequest {
				SourceLanguageCode = sourceLanguage,
				TargetLanguageCode = targetLanguage,
				TerminologyNames = terminologies,
				Text = text
			};

			return await this.translate.TranslateTextAsync(request);
		}
	}

}
