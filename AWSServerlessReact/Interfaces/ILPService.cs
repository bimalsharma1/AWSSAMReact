using Amazon.Lambda.Core;
using AWSServerlessReact.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AWSServerlessReact.Interfaces
{
	public interface ILPService
	{
		public Task<IEnumerable<LP>> GetLPs();
		public Task AddLPs(IEnumerable<LP> lps);
	}
}
