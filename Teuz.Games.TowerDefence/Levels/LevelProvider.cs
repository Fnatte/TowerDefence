using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Teuz.Games.TowerDefence.GameEntities;

namespace Teuz.Games.TowerDefence.Levels
{
	class LevelProvider
	{
		private readonly Dictionary<int, Level> levels = new Dictionary<int, Level>();

		public LevelProvider()
		{

		}

		public void LoadContent()
		{
			// Open document
			XDocument doc = XDocument.Load("Content/Levels.xml");
			foreach (var node in doc.Root.Elements("Level"))
			{
				Level level = Level.FromXml(node);
				levels.Add(level.Id, level);
			}
		}

		public int GetLastLevelId()
		{
			return levels.Count;
		}

		public Level Get(int id)
		{
			return levels[id <= levels.Count ? id : levels.Count];
		}
	}
}
