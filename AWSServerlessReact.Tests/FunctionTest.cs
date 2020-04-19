using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using Newtonsoft.Json;

using Xunit;
using AWSServerlessReact.Models;
using Moq;

namespace AWSServerlessReact.Tests
{
    public class FunctionTest : IDisposable
    { 
        string TableName { get; }
        IAmazonDynamoDB DDBClient { get; }

        public FunctionTest()
        {
            this.TableName = "LPTableTest";
            this.DDBClient = new AmazonDynamoDBClient(RegionEndpoint.APSoutheast2);
        }

        [Fact]
        public async Task LPTestNoDataAsync()
        {
            TestLambdaContext context;
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();
            APIGatewayProxyResponse response;

            // Add a new LP post
            LP myLP = new LP
            {
                Id = new Guid(),
                MeterPointCode = 555555
            };

            LP myLP1 = new LP
            {
                Id = new Guid(),
                MeterPointCode = 5888
            };


            List<LP> myLpsList = new List<LP>
            {
                myLP,
                myLP1
            };

            IEnumerable<LP> myLps = myLpsList.AsEnumerable();

            var mock = new Mock<Interfaces.ILPService>();
            mock.Setup(x => x.AddLPs(myLps));
            mock.Setup(x => x.GetLPs())
                .Returns(Task.FromResult(myLps));

            var repository = mock.Object;

            Functions functions = new Functions(null, null);

            request = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(myLP)
            };
            context = new TestLambdaContext();
            response = await functions.AddLPsAsync(request, context);
            Assert.Equal(400, response.StatusCode);
        }


        [Fact]
        public async Task LPTestAsync()
        {
            TestLambdaContext context;
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();
            APIGatewayProxyResponse response;


            // Add a new LP post
            LP myLP = new LP
            {
                Id = new Guid(),
                MeterPointCode = 555555
            };

            LP myLP1 = new LP
            {
                Id = new Guid(),
                MeterPointCode = 77777
            };


            List<LP> myLpsList = new List<LP>
            {
                myLP,
                myLP1
            };

            IEnumerable<LP> myLps = myLpsList.AsEnumerable();

            var mock = new Mock<Interfaces.ILPService>();
            mock.Setup(x => x.AddLPs(myLps));
            mock.Setup(x => x.GetLPs())
                .Returns(Task.FromResult(myLps));

            var repository = mock.Object;

            Functions functions = new Functions(repository, null);

            LPS body = new LPS
            {
                Lp = new List<LP> { myLP1 }
            };

            request = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(body)
            };
            context = new TestLambdaContext();
            response = await functions.AddLPsAsync(request, context);
            Assert.Equal(200, response.StatusCode);

            var LPId = response.Body;
            context = new TestLambdaContext();
            response = await functions.GetLPsAsync(request, context);
            Assert.Equal(200, response.StatusCode);

            var readLP = JsonConvert.DeserializeObject<List<LP>>(response.Body);
            Assert.Equal(myLP.MeterPointCode, readLP[0].MeterPointCode);
            Assert.Equal(myLP1.MeterPointCode, readLP[1].MeterPointCode);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   // this.DDBClient.DeleteTableAsync(this.TableName).Wait();
                    this.DDBClient.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion


    }
}
