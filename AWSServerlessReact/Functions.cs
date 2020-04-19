using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

using Newtonsoft.Json;
using AWSServerlessReact.Interfaces;
using AWSServerlessReact.Models;
using AWSServerlessReact.Services;
using Newtonsoft.Json.Serialization;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.LambdaJsonSerializer))]

namespace AWSServerlessReact
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        /// 
        public Functions() : this(new LPService(), new LPSummaryService())
        {
        }

        public Functions(ILPService lPService, ILPSummaryService lPSummaryService)
        {
            _LPService = lPService;
            _LPSummaryService = lPSummaryService;
        }

        private readonly ILPService _LPService;
        private readonly ILPSummaryService _LPSummaryService;

        /// <summary>
        /// A Lambda function that returns back a page worth of LPs.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public async Task<APIGatewayProxyResponse> GetLPsAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var page = await _LPService.GetLPs();
            context.Logger.LogLine($"Found {JsonConvert.SerializeObject(page)} LPs");

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(page),
                Headers = new Dictionary<string, string> { 
                    { "Content-Type", "application/json" }, 
                    { "Access-Control-Allow-Origin", "*" }
                }
            };

            return response;
        }


        /// <summary>
        /// A Lambda function that adds a LPs.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> AddLPsAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine($"Saving lp {request?.Body}");

            var lps = JsonConvert.DeserializeObject<LPS>(request?.Body);

            if (lps == null || lps.Lp == null)
            {
                context.Logger.LogLine($"Failed to deserialize to lp {request.Body}");
                return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            context.Logger.LogLine($"Saving lp {JsonConvert.SerializeObject(lps)}");
            await _LPService.AddLPs(lps.Lp);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "",
                Headers = new Dictionary<string, string> { 
                    { "Content-Type", "*" }, 
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Headers", "*"},
                    { "Access-Control-Allow-Methods", "*"}
                }
            };
            context.Logger.LogLine($"Saving lps {JsonConvert.SerializeObject(response)}");
            return response;
        }

        /// <summary>
        /// A Lambda function that returns back a page worth of LPs.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public async Task<APIGatewayProxyResponse> GetLPSummaryAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine($"Service values {JsonConvert.SerializeObject(_LPService)} LPs");
            var page = await _LPSummaryService.GetLPSummary(context);
            context.Logger.LogLine($"Found {JsonConvert.SerializeObject(page)} LPs");

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(page),
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };

            return response;
        }

        /// <summary>
        /// A Lambda function that adds a LP summary.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> AddLPSummaryAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine($"Saving lp {request?.Body}");

            var lpSummary = JsonConvert.DeserializeObject<LPSummaries>(request?.Body);

            if (lpSummary == null)
            {
                context.Logger.LogLine($"Failed to deserialize to lp summary {request.Body}");
                return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            context.Logger.LogLine($"Saving lp {JsonConvert.SerializeObject(lpSummary.LPSummary)}");
            await _LPSummaryService.AddLPSummary(lpSummary.LPSummary);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "",
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "*" },
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Headers", "*"},
                    { "Access-Control-Allow-Methods", "*"}
                }
            };

            return response;
        }
    }
}
