﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Nuclex.Input;
using Nuclex.UserInterface.Controls;

namespace SkyShoot.Game.Controls
{
	public abstract class Controller
	{
		//public delegate void EventHandler(object sender, EventArgs e);

		protected GameScreen ActiveScreen
		{
			get { return ScreenManager.Instance.CurrentScreen; }
		}

		protected int Length
		{
			get { return Controls.Count; }
		}

		private Collection<Control> Controls
		{
			get
			{
				var controls = new Collection<Control>();
				foreach (Control control in ActiveScreen.Desktop.Children)
				{
					if (control is IFocusable)
					{
						controls.Add(control);
					}
				}
				return controls;
			}
		}

		protected int Index = 0;

		protected readonly InputManager InputManager;

		protected IDictionary<Control, List<EventHandler>> Listeners = new Dictionary<Control, List<EventHandler>>();   

		protected Controller(InputManager inputManager)
		{
			InputManager = inputManager;
		}

		public abstract void Update();

		public abstract Vector2? RunVector { get; }

		public abstract Vector2 SightPosition { get; }

		public abstract ButtonState ShootButton { get; }

		public virtual void RegisterListener(Control control, EventHandler eventHandler)
		{
			List<EventHandler> currentListeners;

			if (!Listeners.TryGetValue(control, out currentListeners))
			{
				currentListeners = new List<EventHandler>();
				Listeners.Add(control, currentListeners);
			}

			currentListeners.Add(eventHandler);
		}

		protected void FocusChanged()
		{
			ActiveScreen.FocusedControl = Controls[Index];
		}

		protected void NotifyListeners(Control control)
		{
			if(!Listeners.ContainsKey(control))
				return;

			foreach (var listener in Listeners[control])
			{
				listener(control, null);
			}
		}

		protected void NotifyListeners(int index)
		{
			NotifyListeners(Controls[index]);
		}
	}
}