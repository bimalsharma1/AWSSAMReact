using Amazon.Lambda.Core;
using AWSServerlessReact.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSServerlessReact.Interfaces
{
	public interface ILPSummaryService
	{
		public Task<IEnumerable<LPSummary>> GetLPSummary(ILambdaContext context);
		public Task AddLPSummary(LPSummary lps);
	}
}
