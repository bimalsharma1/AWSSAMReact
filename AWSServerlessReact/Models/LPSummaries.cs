using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AWSServerlessReact.Models
{
	public class LPSummaries
	{
		[DataMember(Name = "lpSummary")]
		public LPSummary LPSummary { get; set; }
	}
}
