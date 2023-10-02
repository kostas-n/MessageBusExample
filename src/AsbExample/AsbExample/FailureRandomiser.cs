namespace AsbExample
{
	public class FailureRandomiser
	{
		public bool ShoudlFail() 
		{ 
			return new Random(10).Next() % 2 == 0;
		}
	}
}
