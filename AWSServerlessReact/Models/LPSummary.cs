using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AWSServerlessReact.Models
{
	public class LPSummary
	{
		[DataMember(Name = "fileName")]
		public string FileName { get; set; }

		[DataMember(Name = "date")]
		public string Date { get; set; }

		[DataMember(Name = "meter")]
		public string Meter { get; set; }

		[DataMember(Name = "dataType")]
		public string DataType { get; set; }

		[DataMember(Name = "min")]
		public decimal? Min { get; set; }

		[DataMember(Name = "max")]
		public decimal? Max { get; set; }

		[DataMember(Name = "median")]
		public decimal? Median { get; set; }
	}
}
