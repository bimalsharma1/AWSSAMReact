using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AWSServerlessReact.Models
{
	public class LPS
	{
		[DataMember(Name = "lp")]
		public List<LP> Lp { get; set; }
	}
}
