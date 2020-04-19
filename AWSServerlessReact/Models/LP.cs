using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AWSServerlessReact.Models
{
	public class LP
	{
		[DataMember(Name = "id")]
		public Guid Id { get; set; }

		[DataMember(Name = "meterPointCode")]
		public int? MeterPointCode { get; set; }

		[DataMember(Name = "serialNumber")]
		public int? SerialNumber { get; set; }

		[DataMember(Name = "plantCode")]
		public string PlantCode { get; set; }

		[DataMember(Name = "dateTime")]
		public string DateTime { get; set; }

		[DataMember(Name = "dataType")]
		public string DataType { get; set; }

		[DataMember(Name = "dataValue")]
		public decimal? DataValue { get; set; }

		[DataMember(Name = "units")]
		public string Units { get; set; }

		[DataMember(Name = "status")]
		public string Status { get; set; }
	}
}
