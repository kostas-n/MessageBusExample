namespace MassTransitExample.Messages
{
	public interface ISampleAdded
	{
		public long SampleId { get; set; }
		public string SampleDescription { get; set; }
	}
	public class SampleAdded: ISampleAdded
	{
		public long SampleId { get; set; }
		public string SampleDescription { get; set; }
	}
}
