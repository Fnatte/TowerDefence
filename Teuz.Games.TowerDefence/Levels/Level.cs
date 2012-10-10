using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Teuz.Games.TowerDefence.GameEntities;
using Ninject;
using Teuz.Games.TowerDefence.GameEntities.Attackers;

namespace Teuz.Games.TowerDefence.Levels
{
	class Level
	{
		private readonly List<SpawnInfo> spawnInfos = new List<SpawnInfo>();

		public int Id { get; private set; }

		private Level() { }

		public static Level FromXml(XElement levelNode)
		{
			Level level = new Level();

			// Id
			var idAttribute = levelNode.Attribute("id");
			if (idAttribute == null) throw new Exception("Level did not have a ID");
			int id;
			if (!int.TryParse(idAttribute.Value, out id)) throw new Exception("Failed to parse ID as integer in Level node");
			level.Id = id;

			// Order
			var orderAttribute = levelNode.Attribute("order");
			string order = orderAttribute != null ? orderAttribute.Value : "InOrder";

			// Read attackers
			foreach (var node in levelNode.Elements())
			{
				if (node.Name == "AttackerPack")
				{
					// Get the type node
					var typeNode = node.Attribute("type");
					if (typeNode == null) throw new Exception("Type not found in AttackerPack.");

					// Get the count node
					var countNode = node.Attribute("count");
					if (countNode == null) throw new Exception("Count not found in AttackerPack.");

					// Count to integer
					int count;
					if (!int.TryParse(countNode.Value, out count))
						throw new Exception("Count was not a vaild integer in AttackerPack.");

					// Type to actuall assembly attacker type
					Type type;
					try { type = Type.GetType("Teuz.Games.TowerDefence.GameEntities.Attackers." + typeNode.Value); }
					catch (Exception e) { throw new Exception("Failed to parse type in AttackerPack to actuall System.Type.", e); }
					if (type == null) throw new Exception("Could not find type specified in AttackerPack.");

					// Make sure type is a attacker
					if (!type.IsSubclassOf(typeof(Attacker)))
						throw new Exception("The type specified is not a vaild Attacker.");

					// Get interval there is any
					var intervalAttribute = node.Attribute("interval");
					double interval = 0;
					if (intervalAttribute != null)
					{
						if (!double.TryParse(intervalAttribute.Value, out interval))
							throw new Exception("Failed to parse interval of AttackerPack as System.Double.");
					}

					if (order == "InOrder")
					{
						SpawnInfo spawnInfo = new SpawnInfo();
						spawnInfo.Type = type;
						spawnInfo.Count = count;
						if (interval > 0) spawnInfo.Interval = TimeSpan.FromMilliseconds(interval);

						level.spawnInfos.Add(spawnInfo);
					}
				}
			}

			return level;
		}

		public IEnumerable<SpawnInfo> GetSpawnInfos()
		{
			foreach (var spawnInfo in spawnInfos)
			{
				yield return spawnInfo;
			}
		}
	}
}
