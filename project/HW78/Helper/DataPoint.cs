using System.Runtime.Serialization;

namespace HW78.Helper
{
	[DataContract]
	public class DataPoint
	{
		public DataPoint(string label, double y)
		{
			this.Label = label;
			this.Y = y;
		}

		[DataMember(Name = "label")]
		public string Label = "";


		[DataMember(Name = "y")]
		public double? Y = null;
	}
}
