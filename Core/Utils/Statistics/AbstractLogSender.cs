// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using System.Threading;
using Assets.Plugins.DeepLabs.Core.Utils.Log;
using Assets.Sources.Util.Concurrent;

namespace Assets.Sources.Util.Statistics
{
    public abstract class AbstractLogSender
    {
        private const int DefaultSleepTime = 60 * 1000;
        private readonly int sendItervalMillis;
        private readonly BlockingCollection<IAbstractJsonLog> logConveyor = new BlockingCollection<IAbstractJsonLog>();
        private bool shouldStop = true;

        public AbstractLogSender(int sendItervalMillis)
        {
            this.sendItervalMillis = sendItervalMillis;
        }

        public void Start()
        {
            if (shouldStop)
            {
                shouldStop = false;
                logConveyor.StartTakingAgainIfCompleted();
                UnityThreadHelper.CreateThread(() => ExceptionRun());
            }
        }

        public void ShouldStop()
        {
            if (!shouldStop)
            {
                shouldStop = true;
                logConveyor.CompleteTaking();
            }
        }

        private void ExceptionRun()
        {
            try
            {
                Run();
            }
            catch (Exception e)
            {
                Lc.Assertion(e);
            }
        }

        private void Run()
        {
            var lastSendMillis = TimeUtil.GetCurrentTimeMillis();
            var logsWaitingSend = new Dictionary<string, List<IAbstractJsonLog>>();
            
            foreach (var log in logConveyor.GetConsumingEnumerable())
            {
                List<IAbstractJsonLog> logs;
                if (!logsWaitingSend.TryGetValue(log.Method, out logs))
                {
                    logs = new List<IAbstractJsonLog>();
                    logsWaitingSend.Add(log.Method, logs);
                }
                logs.Add(log);

                if (TimeUtil.GetCurrentTimeMillis() - lastSendMillis >= sendItervalMillis)
                {
                    lastSendMillis = TimeUtil.GetCurrentTimeMillis();
                    SendLogsSleepOnNetworkError(logsWaitingSend);
                }
            }
            SendLogsSleepOnNetworkError(logsWaitingSend);
        }

        private void SendLogsSleepOnNetworkError(Dictionary<string, List<IAbstractJsonLog>> logsWaitingSend)
        {
            foreach (var methodToLogs in logsWaitingSend)
            {
                if (methodToLogs.Value.Count == 0)
                {
                    continue;
                }
                if (SendLogs(methodToLogs.Key, methodToLogs.Value))
                {
                    methodToLogs.Value.Clear();
                }
                else
                {
                    Thread.Sleep(DefaultSleepTime);
                }
            }
        }

        // returns false, if there was an error sending logs
        protected abstract bool SendLogs(string method, List<IAbstractJsonLog> logsConsumed);

        public void AddLog(IAbstractJsonLog log)
        {
            logConveyor.Add(log);
        }
    }
}
