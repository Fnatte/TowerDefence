using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teuz.Games.TowerDefence.GameEntities;
using Ninject;
using Teuz.Games.TowerDefence.PathFinding;
using Teuz.Games.TowerDefence.States;
using Teuz.Games.TowerDefence.Levels;
using Teuz.Games.TowerDefence.GameEntities.Attackers;

namespace Teuz.Games.TowerDefence
{
	class SpawnProvider
	{
		private readonly Queue<SpawnInfo> spawnQueue = new Queue<SpawnInfo>();
		public Queue<SpawnInfo> SpawnQueue { get { return spawnQueue; } }

		private SpawnInfo currentSpawnInfo;

		private readonly List<Spawn> spawns = new List<Spawn>();
		public List<Spawn> Spawns { get { return spawns; } }

		private readonly List<Attacker> spawnedAttackers = new List<Attacker>();

		private Random random;

		private TimeSpan timer;

		private GameStateManager gameStateManager;
		private Player player;
		private IWorld world;
		private PathProvider pathProvider;

		public SpawnProvider(Random random, GameStateManager gameStateManager, Player player, IWorld world, PathProvider pathProvider)
		{
			this.random = random;
			this.gameStateManager = gameStateManager;
			this.gameStateManager.StateChanged += gameStateManager_StateChanged;
			this.player = player;
			this.world = world;
			this.pathProvider = pathProvider;
		}

		void gameStateManager_StateChanged(object sender, EventArgs e)
		{
			if (gameStateManager.CurrentState is DefendingState)
			{
				PrepareLevel();
			}
		}

		public void Update(GameTime gameTime)
		{
			if(gameStateManager.CurrentState is DefendingState)
				UpdateDefending(gameTime);
		}

		private void PrepareLevel()
		{
			pathProvider.FindPaths();
		}

		private void UpdateDefending(GameTime gameTime)
		{
			if ((currentSpawnInfo != null && currentSpawnInfo.Count > 0) || SpawnQueue.Count > 0)
			{
				if (Spawns.Count > 0)
				{
					if (currentSpawnInfo == null || currentSpawnInfo.Count == 0)
						currentSpawnInfo = spawnQueue.Dequeue();

					timer += gameTime.Elapsed;
					if (timer >= currentSpawnInfo.Interval)
					{
						SpawnNext();
						timer = TimeSpan.Zero;
					}
				}
			}
			else
			{
				// Wait for all attackers to die.
				if (spawnedAttackers.All(x => x.IsDead()))
				{
					// Notfy defending state it was a successfull defence.
					var defendingState = gameStateManager.CurrentState as DefendingState;
					if (defendingState != null)
					{
						defendingState.EndSuccessfully();
					}

					// Go to shopping state
					// gameStateManager.PushState<ShoppingState>();
					gameStateManager.ReplaceState<ShoppingState>();

					// Reset SpawnProvider state
					ResetState();
				}
			}
		}

		public void ResetState()
		{
			spawnedAttackers.Clear();
			currentSpawnInfo = null;
			SpawnQueue.Clear();
		}

		private void SpawnNext()
		{
			currentSpawnInfo.Count--;

			Attacker attacker = (Attacker)NinjectFactory.Kernel.Get(currentSpawnInfo.Type);
			attacker.Initialize();
			attacker.LoadContent();
			Spawn spawn = Spawns.ElementAt(random.Next(Spawns.Count));

			attacker.Tile = spawn.Tile;
			attacker.Path = pathProvider.PathBySpawn(spawn);
			spawnedAttackers.Add(attacker);
		}

		public void Load(Level level)
		{
			foreach (var spawnInfo in level.GetSpawnInfos())
			{
				SpawnQueue.Enqueue((SpawnInfo)spawnInfo.Clone());
			}
		}
	}
}
