using System.Collections.Generic;

namespace InstantRiceMessenger
{
	class Util
	{
		public static IEnumerable<T> Listo<T>(T obj)
		{
			yield return obj;
		}
	}
}
