﻿using System;
using Common.Commands;
using Common.Communication;
using Smartphone.Driver.Models;

namespace Smartphone.Driver.Handlers
{
	public class CmdSendNotificationHandler : CommandHandler<CmdSendNotification>
	{

		private WrappedOrders orders = null;

		public CmdSendNotificationHandler(WrappedOrders orders)
		{
			this.orders = orders;
		}

		protected override void Handle (CmdSendNotification command, string connectionIdOrNull)
		{
			// Update Orders model.
			Xamarin.Forms.Device.BeginInvokeOnMainThread (new Action (() => orders.UpdateSingle (command.Order)));
		}

	}
}

