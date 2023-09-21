using AsbExample.Dtos;
using AsbExample.Messages;
using Microsoft.AspNetCore.Mvc;

namespace AsbExample.Controllers
{
	[ApiController]
	public class SampleController : ControllerBase
	{
		private IPublisher _publisher;

		public SampleController(IPublisher publisher)
		{
			_publisher = publisher;
		}

		[HttpPost, Route("/samples")]
		public async Task<ActionResult> AddASample([FromBody] Sample sample)
		{
			var message = new SampleAdded() { SampleId = sample.SampleId, SampleDescription = sample.SampleDescription };
			await _publisher.Publish(message);
			return Ok();
		}

		[HttpGet, Route("/samples")]
		public async Task<ActionResult> GetSamples()
		{
			var emptyList = new List<Sample>();
			return Ok(emptyList);
		}
	}
}
