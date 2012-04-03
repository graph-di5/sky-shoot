﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using SkyShoot.Contracts.Session;

namespace SkyShoot.Contracts.Service
{
	[ServiceContract]
	public interface ISkyShootLogin
	{

		[OperationContract(IsInitiating = true)]
		bool Register(string username, string password);

		[OperationContract(IsInitiating = true)]
		Guid? Login(string username, string password);

		[OperationContract]
		GameDescription[] GetGameList();

		[OperationContract]
		bool RegisterForGame(GameDescription game);

		[OperationContract]
		GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet level);

	}
}
