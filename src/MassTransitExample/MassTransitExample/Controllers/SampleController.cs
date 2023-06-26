using MassTransitExample.Dtos;
using MassTransitExample.MassTransit;
using MassTransitExample.Messages;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitExample.Controllers
{
	[ApiController]
	public class SampleController : ControllerBase
	{
		private readonly ISamplePublisher SamplePublisher;

		public SampleController(ISamplePublisher samplePublisher)
		{
			SamplePublisher = samplePublisher;
		}

		[HttpPost, Route("/samples")]
		public async Task<ActionResult> AddASample([FromBody] Sample sample)
		{
			var message = new SampleAdded() { SampleId = sample.SampleId, SampleDescription = sample.SampleDescription };
			await SamplePublisher.Publish(message);
			return Ok();
		}
	}
}
