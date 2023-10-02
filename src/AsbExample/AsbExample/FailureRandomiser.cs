namespace AsbExample
{
	public class FailureRandomiser
	{
		public bool ShouldFail() 
		{ 
			return new Random(10).Next() % 2 == 0;
		}
	}
}
