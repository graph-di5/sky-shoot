﻿using System;
using System.Collections.Generic;
using SkyShoot.Contracts.Session;
using SkyShoot.Contracts.Mobs;
using SkyShoot.XNA.Framework;
using SkyShoot.Contracts.Weapon.Projectiles;
using System.Timers;
using SkyShoot.Contracts;
using System.Diagnostics;
using SkyShoot.ServProgram.Session;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Bonuses;

namespace SkyShoot.Service.Session
{
	
	public class GameSession
	{
		public List<MainSkyShootService> Players { get; set; }

		private List<AGameObject> _gameObjects;
		//private ObjectPool<AGameObject> _mobs;// { get; set; }
		//private List<AProjectile> _projectiles;
		//private ObjectPool<AProjectile> _projectiles;
		private List<Mob> _mobs { get; set; }
		private List<AGameBonus> _bonuses { get; set; }
		private ObjectPool<AProjectile> _projectiles { get; set; }

		public GameDescription LocalGameDescription { get; private set; }

		public bool IsStarted { get; set; }
		public GameLevel GameLevel { get; private set; }
		private SpiderFactory _spiderFactory;
		private long _timerCounter;
		private long _intervalToSpawn = 0;

		private long _lastUpdate;
		private long _updateDelay;

		private Timer _gameTimer;
		private object _updating;

		public GameSession(TileSet tileSet, int maxPlayersAllowed, GameMode gameType, int gameID)
		{
			IsStarted = false;
			GameLevel = new GameLevel(tileSet);

			var playerNames = new List<string>();

			_gameObjects = new List<AGameObject>();
			//_mobs = new ObjectPool<AGameObject>();
			_bonuses = new List<AGameBonus>();
			Players = new List<MainSkyShootService>();
			//_projectiles = new List<AProjectile>();
			//_projectiles = new ObjectPool<AProjectile>();
			//_gameEventStack = new Stack<AGameEvent>();

			LocalGameDescription = new GameDescription(playerNames, maxPlayersAllowed, gameType, gameID, tileSet);
			_spiderFactory= new SpiderFactory(GameLevel);
		}

		//public event SomebodyMovesHandler SomebodyMoves; 
		//public event SomebodyDiesHandler SomebodyDies;
		//public event SomebodyHitHandler SomebodyHit;
		
		private void SomebodyMoved(AGameObject sender, Vector2 direction)
		{
			sender.RunVector = direction;
			PushEvent(new ObjectDirectionChanged(direction,sender.Id,_timerCounter));
		}

		private void SomebodyShot(AGameObject sender, Vector2 direction)
		{
			sender.ShootVector = direction;
			
			if (sender.Weapon != null)
			{
				if (sender.Weapon.Reload(System.DateTime.Now.Ticks / 10000))
				{
					var a = sender.Weapon.CreateBullets(sender, direction);
					foreach (var b in a)
					{
						//_projectiles.Add(b);
						//_projectiles.GetInActive().Copy(b);
						_gameObjects.Add(b);// GetInActive().Copy(b);
						PushEvent(new NewObjectEvent(b,_timerCounter));
					}
					//Trace.WriteLine("projectile added", "GameSession");
				}
			}
		}

		public void SomebodyDied(AGameObject sender)
		{
			PushEvent(new ObjectDeleted(sender.Id, _timerCounter));
		}

		public void SomebodySpawned(AGameObject sender) 
		{			
			PushEvent(new NewObjectEvent(sender, _timerCounter));
		}

		private void NewBonusDropped(AGameObject bonus)
		{
			_bonuses.Add((AGameBonus) bonus);
			PushEvent(new NewObjectEvent(bonus, _timerCounter));
		}

		public void MobDead(AGameObject mob)
		{
			//SomebodyDied(mob);
			//mob.MeMoved -= SomebodyMoved;
			NewBonusDropped(new AGameBonus(mob.Coordinates));
			if(mob.IsPlayer)
			{
				PlayerDead(mob as MainSkyShootService);
				return;
			}
			PushEvent(new ObjectDeleted(mob.Id,_timerCounter));
			_gameObjects.Remove(mob);
		}

		public void SomebodyHitted(AGameObject mob, AProjectile projectile)
		{
			PushEvent(new ObjectHealthChanged(mob.HealthAmount, mob.Id, _timerCounter));
		}

		public void PlayerLeave(MainSkyShootService player)
		{
			LocalGameDescription.Players.Remove(player.Name);

			player.MeMoved -= SomebodyMoved;
			player.MeShot -= SomebodyShot;

			Players.Remove(player);
			Trace.WriteLine(player.Name + "leaved game");
			
		}

		private void PushEvent(AGameEvent gameEvent)
		{
			foreach (var player in Players)
			{
				player.NewEvents.Enqueue(gameEvent);
			}
		}

		public void PlayerDead(MainSkyShootService player)
		{

			//player.GameOver();

			SomebodyDied(player);			
			player.Disconnect();//временно
		}
		public void Stop()
		{
			if (_gameTimer != null)
			{
				_gameTimer.Enabled = false;
				_gameTimer.AutoReset = false;
				_gameTimer.Elapsed -= TimerElapsedListener;
			}
			IsStarted = false;
		}

		public AGameObject[] GetSynchroFrame()
		{
			var allObjects = new List<AGameObject>(_gameObjects.ToArray());
			allObjects.AddRange(Players);
			Trace.WriteLine("SynchroFrame");
			return allObjects.ToArray();

		}

		public void Start()
		{
			for(int i = 0; i < Players.Count; i++)
			{
				var player = Players[i];
				//this.SomebodyMoves += player.MobMoved;
				player.MeMoved += SomebodyMoved;
				//this.SomebodyShoots += player.MobShot;
				player.MeShot += SomebodyShot;

				//this.SomebodySpawns += player.SpawnMob;

				//this.SomebodyDies += player.MobDead;
				
				//this.SomebodyHit += player.Hit;

				player.Coordinates = new Vector2(500,500);
				player.Speed = Constants.PLAYER_DEFAULT_SPEED;
				player.Radius = Constants.PLAYER_RADIUS;
				player.Weapon = new Weapon.Pistol(Guid.NewGuid(), player);
				player.RunVector = new Vector2(0, 0);
				player.MaxHealthAmount = player.HealthAmount = 100f;
				
			}
			System.Threading.Thread.Sleep(1000);
			if (!IsStarted)
			{
				IsStarted = true;
			}
			_timerCounter = 0;
			_updating = false;

			_lastUpdate = DateTime.Now.Ticks/10000;
			_updateDelay = 0;
			_gameTimer = new Timer(Constants.FPS);
			_gameTimer.AutoReset = true;
			_gameTimer.Elapsed += TimerElapsedListener;
			_gameTimer.Start();
			Trace.WriteLine("Game Started");
			
		}

		public bool AddPlayer(MainSkyShootService player)
		{
			if (Players.Count >= LocalGameDescription.MaximumPlayersAllowed || IsStarted)
				return false;

			Players.Add(player);
			LocalGameDescription.Players.Add(player.Name);
			var names = new String[Players.Count];
			//UpdatePlayersList(player);

			//if (NewPlayerConnected != null) NewPlayerConnected(player);

			//StartGame += player.GameStart;

			if (Players.Count == LocalGameDescription.MaximumPlayersAllowed)
			{
				Trace.WriteLine("player added"+player.Name);
				var startThread = new System.Threading.Thread(new System.Threading.ThreadStart(Start));
				startThread.Start();
				
				
			}
			return true;
		}

		private void TimerElapsedListener(object sender, EventArgs e)
		{			
			Update();
					
		}

	#region local functions
		public void SpawnMob()
		{
			//!! debug
			var t = _gameObjects.FindAll(m => m.ObjectType == AGameObject.EnumObjectType.Mob);
			if(t != null && t.Count > 2)
				return;
			if (_intervalToSpawn == 0)
			{
				_intervalToSpawn = (long) Math.Exp(4.8 - (float)_timerCounter/40000f);
				
				var mob = _spiderFactory.CreateMob();
				Trace.WriteLine("mob spawned" + mob.Id);
				
				_gameObjects.Add(mob);
				SomebodySpawned(mob);
				//mob.MeMoved += new SomebodyMovesHandler(SomebodyMoved);
			}
			else
			{
				_intervalToSpawn--;
			}
		}

		/// <summary>
		/// здесь будут производится обработка всех действий
		/// </summary>
		private void Update() 
		{
			if (!System.Threading.Monitor.TryEnter(_updating)) return;

			Trace.WriteLine("update begin "+ _timerCounter);
			SpawnMob();
			 var now = DateTime.Now.Ticks/10000;
			_updateDelay = now - _lastUpdate;
			_lastUpdate = now;

			int shift = Players.Count;
			for (int i = 0; i < shift + _gameObjects.Count; i++)
			{
				AGameObject activeObject;
				if(i < shift)
				{
					activeObject = Players[i];
				}
				else
				{
					activeObject = _gameObjects[i - shift];
				}
				// объект не существует
				if (!activeObject.IsActive)
				{
					continue;
				}
				activeObject.Think(new List<AGameObject>(Players));
				var newCoord = activeObject.ComputeMovement(_updateDelay, GameLevel);
				var canMove = true;
				/* <b>int j = 0</b> потому что каждый с каждым, а действия не симметричны*/
				for (int j = 0 ; j < shift + _gameObjects.Count; j++)
				{
					// тот же самый объект. сам с собой он ничего не делает
					if (i == j)
					{
						continue;
					}
					AGameObject slaveObject;
					if (j < shift)
					{
						slaveObject = Players[j];
					}
					else
					{
						slaveObject = _gameObjects[j - shift];
					}
					// объект не существует
					if (!slaveObject.IsActive)
					{
						continue;
					}
					//!! rewrite sqrt!!
					// обект далеко. не рассамтриваем
					if(Vector2.Distance(newCoord, slaveObject.Coordinates) > (activeObject.Radius + slaveObject.Radius))
					{
						continue;
					}
					activeObject.Do(slaveObject);
					if (activeObject.IsBullet)
					{
						// пуля поиспользована, должна быть удалена
						activeObject.HealthAmount = -1;
						break;
					}
					if (!slaveObject.IsBullet && 
						slaveObject.ObjectType != AGameObject.EnumObjectType.Bonus)
					{
						canMove = false;
					}
				}
				if (canMove)
				{
					activeObject.Coordinates = newCoord;
				}
				if(activeObject.HealthAmount < 0)
				{
					activeObject.IsActive = false;
					MobDead(activeObject);
				}
			}

			/*
			for (int i = 0; i < _gameObjects.Count; i++)
			{
				var mob = _gameObjects[i];
				mob.Think(new List<AGameObject>(Players.ToArray()));
				mob.Coordinates = ComputeMovement(mob);
				//System.Diagnostics.Trace.WriteLine("Mob cord: " + mob.Coordinates); 

			}

			for(int i = 0; i < Players.Count; i++)
			{
				var player = Players[i];
				player.Coordinates = ComputeMovement(player);

				//Проверка на касание игрока и моба
				HitTestTouch(player);

				List<AGameBonus> bonuses2delete = new List<AGameBonus>();
				for(int j = 0; j < _bonuses.Count; j ++)
				{
					var bonus = _bonuses[j];
					if (Vector2.Distance(bonus.Coordinates, player.Coordinates) < player.Radius)
					{
						if(!bonus.IsActive)
						{
							continue;
						}
						player.bonuses.Add(bonus);
						bonus.IsActive = false;
						PushEvent(new ObjectDeleted(bonus.Id, _timerCounter));
						bonuses2delete.Add(bonus);
					} 
				}
				foreach (AGameBonus bonus in bonuses2delete)
				{
					_bonuses.Remove(bonus);
				}
			}
			//Trace.WriteLine("" + _projectiles.Size);
			//for (var pr = _projectiles.FirstActive; pr != null; pr = _projectiles.Next(pr))
			foreach (var projectile in _projectiles)
			{
				
				//if (pr == null || pr.Item == null) continue;
				//var projectile = pr.Item;
				if (projectile.LifeDistance <= 0)
				{
					projectile.IsActive = false;
					//pr.isActive = false;
					continue;
				}
				//var projectile = _projectiles[i];
				var newCord = projectile.Coordinates + projectile.RunVector * projectile.Speed * _updateDelay;

				//Проверка на касание пули и моба
				var hitedMob = hitTestProjectile(projectile, newCord);
				if (hitedMob == null)
				{
					projectile.OldCoordinates = projectile.Coordinates;
					projectile.Coordinates = newCord;
					projectile.LifeDistance -= Vector2.Distance(projectile.Coordinates, projectile.OldCoordinates);
				}
				else
				{
					projectile.Do(hitedMob);
					//hitedMob.DamageTaken(projectile);
					SomebodyHitted(hitedMob, projectile);
					if (hitedMob.HealthAmount <= 0)
					{
						MobDead(hitedMob);
					}
					projectile.LifeDistance = -1;
				}

			}
			/**/

			//_projectiles.RemoveAll(x => (x==null) || (x.LifeDistance <= 0));
			Trace.WriteLine(System.DateTime.Now.Ticks/10000 - now);
			Trace.WriteLine("update end " + _timerCounter);
			_timerCounter++;
			//_updated = false;
			System.Threading.Monitor.Exit(_updating);
		}
		
		private Mob HitTestProjectile(AProjectile projectile, Vector2 newCord)
		{
			var prX = newCord.X - projectile.Coordinates.X;
			var prY = newCord.Y - projectile.Coordinates.Y;

			Mob hitedTarget = null;
			var minDist = double.MaxValue;

			for (int i = 0; i < _gameObjects.Count; i++)
			{
				var mob = _gameObjects[i];
				var mX = mob.Coordinates.X - projectile.Coordinates.X;
				var mY = mob.Coordinates.Y - projectile.Coordinates.Y;
				var mR = mob.Radius;
				var mDist = Math.Sqrt(mX * mX + mY * mY);

				if (mDist <= mR && mDist < minDist)
				{
					//!! hitedTarget = mob;
					minDist = mDist;
					continue;
				}

				if (prX == 0 && prY == 0)
				{
					continue;
				}

				var h = Math.Abs(prX * mY - prY * mX) / Math.Sqrt(prX * prX * + prY * prY);

				//@TODO Проверка углов. Над ней еще надо будет подумать.
				var cos1 = mX * prX + mY * prY;
				var cos2 = -1 * (prX * (mX - prX) + prY * (mY - prY));

				if (h <= mR && Math.Sign(cos1) == Math.Sign(cos2) && mDist < minDist)
				{
					//!! hitedTarget = mob;
					minDist = mDist;
				}

			}

			return hitedTarget;
		}

		private void HitTestTouch(MainSkyShootService player)
		{
			for (int i = 0; i < _gameObjects.Count;i++ )
			{
				var mob = _gameObjects[i];
				if ((mob.Coordinates - player.Coordinates).Length() <= mob.Radius + player.Radius)
				{
					if (mob.Weapon.Reload(DateTime.Now.Ticks / 10000))
					{
						mob.Do(player);
						//player.HealthAmount -= mob.Damage;
						SomebodyHitted(player, null);
					}

					if (player.HealthAmount <= 0)
					{
						PlayerDead(player);
					}
				}
			}
		} 


	#endregion
	}
}