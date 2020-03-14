using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Binance.Helpers;
using Binance.Net;
using Common;
using Contracts.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Persistence;
using Persistence.Entities;

namespace Binance
{
    public class TradingMonitor : BackgroundService
    {
        private readonly ILogger<TradingMonitor> _logger;
        private readonly ISettingsProcessor _exchangeSettingsProcessor;
        private readonly IOptions<Settings> _settings;
        private readonly IClusterClient _orleansClient;

        public TradingMonitor(
	        ILogger<TradingMonitor> logger,
	        ISettingsProcessor exchangeSettingsProcessor,
	        IOptions<Settings> settings,
	        OrleansClient orleansClient)
        {
            _logger = logger;
            _exchangeSettingsProcessor = exchangeSettingsProcessor;
            _settings = settings;
            _orleansClient = orleansClient.Client;
        }

		protected override async Task ExecuteAsync (CancellationToken stoppingToken)
		{
			IOrderProcessingGrain grain = _orleansClient.GetGrain<IOrderProcessingGrain>(0);
			Notificator notificator = new Notificator();

			INotificator obj = await _orleansClient.CreateObjectReference<INotificator>(notificator);
			await grain.Subscribe(obj);
			
			while (!stoppingToken.IsCancellationRequested)
			{
				
			}
		}
    }
}