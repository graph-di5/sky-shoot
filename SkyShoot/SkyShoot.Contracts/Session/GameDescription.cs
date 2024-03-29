﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SkyShoot.Contracts.Session
{
	[DataContract]
	public class GameDescription
	{
		public GameDescription()
		{
		}

		public GameDescription(List<string> players, int maxPlayersAllowed, GameMode gameType, int gameId, TileSet usedTileSet, int teams)
		{
			GameId = gameId;
			Players = players;
			MaximumPlayersAllowed = maxPlayersAllowed;
			GameType = gameType;
			UsedTileSet = usedTileSet;
			Teams = teams;
		}

		[DataMember]
		public int GameId { get; set; }

		[DataMember]
		public List<string> Players { get; set; }

		[DataMember]
		public int MaximumPlayersAllowed { get; set; }

		[DataMember]
		public GameMode GameType { get; set; }

		[DataMember]
		public TileSet UsedTileSet { get; set; }

		[DataMember]
		public int Teams { get; set; }

		public override string ToString()
		{
			return string.Format("[ {0} ; {1} ; {2}/{3} ; {4}]", UsedTileSet, GameType, Players.Count, MaximumPlayersAllowed, Teams);
		}
	}
}
