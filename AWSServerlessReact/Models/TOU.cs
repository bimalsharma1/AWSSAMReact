using System;
using System.Collections.Generic;
using System.Text;

namespace AWSServerlessReact.Models
{
	public class TOU
	{
		public Guid Id { get; set; }

		public string MeterCode { get; set; }

		public string Serial { get; set; }

		public string PlantCode { get; set; }

		public string DateTime { get; set; }

		public string Quality { get; set; }

		public string Stream { get; set; }

		public decimal Energy { get; set; }

		public string Units { get; set; }

		public string Status { get; set; }
	}
}
