using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace Launchie
{
	[Serializable]
	public class HashesContainer 
	{
		public Dictionary<string, byte[]> Hashes;

		public HashesContainer(Dictionary<string, byte[]> hashes) 
		{
			Hashes = hashes;
		}

		public void Serialize(Stream stream)
		{
			new BinaryFormatter().Serialize (stream, this);
		}

		public static Dictionary<string, byte[]> Deserialize(Stream stream)
		{
			return (new BinaryFormatter().Deserialize (stream) as HashesContainer).Hashes;
		}
	}
}

