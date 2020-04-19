using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using AWSServerlessReact.Interfaces;
using AWSServerlessReact.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSServerlessReact.Services
{
	public class LPSummaryService: ILPSummaryService
	{
		// This const is the name of the environment variable that the serverless.template will use to set
		// the name of the DynamoDB table used to store LPs.
		const string TABLENAME_ENVIRONMENT_VARIABLE_LOOKUP = "LPSummary";

		public const string ID_QUERY_STRING_NAME = "FileName";
		IDynamoDBContext DDBContext { get; set; }

		/// <summary>
		/// Constructor used for testing passing in a preconfigured DynamoDB client.
		/// </summary>
		/// <param name="ddbClient"></param>

		public LPSummaryService ()
		{
			// Check to see if a table name was passed in through environment variables and if so 
			// add the table mapping.
			var tableName = System.Environment.GetEnvironmentVariable(TABLENAME_ENVIRONMENT_VARIABLE_LOOKUP);
			if (!string.IsNullOrEmpty(tableName))
			{
				AWSConfigsDynamoDB.Context.TypeMappings[typeof(LPSummary)] = new Amazon.Util.TypeMapping(typeof(LPSummary), tableName);
			}

			var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
			this.DDBContext = new DynamoDBContext(new AmazonDynamoDBClient(), config);
		}

		/// <summary>
		/// Constructor used for testing passing in a preconfigured DynamoDB client.
		/// </summary>
		/// <param name="ddbClient"></param>
		/// <param name="tableName"></param>
		public LPSummaryService(IAmazonDynamoDB ddbClient, string tableName)
		{
			if (!string.IsNullOrEmpty(tableName))
			{
				AWSConfigsDynamoDB.Context.TypeMappings[typeof(LPSummary)] = new Amazon.Util.TypeMapping(typeof(LPSummary), tableName);
			}

			var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
			this.DDBContext = new DynamoDBContext(ddbClient, config);
		}

		public async Task<IEnumerable<LPSummary>> GetLPSummary(ILambdaContext context)
		{
			var conditions = new List<ScanCondition>();
			// you can add scan conditions, or leave empty
			context.Logger.LogLine($"Found  {JsonConvert.SerializeObject(DDBContext)} LPs");
			var search = DDBContext.ScanAsync<LPSummary>(conditions);
			return await search.GetRemainingAsync();
		}

		public async Task AddLPSummary(LPSummary lps)
		{
			await DDBContext.SaveAsync<LPSummary>(lps);
		}
	}
}
