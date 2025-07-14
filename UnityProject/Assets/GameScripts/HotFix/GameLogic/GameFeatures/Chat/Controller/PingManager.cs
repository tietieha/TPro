using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using XLua;

/**
 * add by sunliwen
 * 该类用于ping所有的服务器，最后连接网络环境最好的那个
 */
namespace UW
{
    
    public class ServerPingResult
    {
        public string Address;
        public long Ping;

        public ServerPingResult(string address, long ping)
        {
            Address = address;
            Ping = ping;
        }
    }

    [LuaCallCSharp]
    public class PingManager
    {
        public static IEnumerator PingServers(string [] serverList, Action<List<ServerPingResult>> onComplete)
        {
            List<ServerPingResult> pingResults = new List<ServerPingResult>();

            foreach (string serverAddress in serverList)
            {
                Ping ping = new Ping(serverAddress);

                while (!ping.isDone)
                {
                    yield return null;
                }

                if (ping.time >= 0)
                {
                    ServerPingResult result = new ServerPingResult(serverAddress, ping.time);
                    pingResults.Add(result);
                }
                else
                {
                    Log.Warning("Failed to ping server {0}", serverAddress);
                }
            }

            pingResults.Sort((a, b) => a.Ping.CompareTo(b.Ping));

            if (onComplete != null)
            {
                onComplete(pingResults);
            }
        }

        public static void StartPing(string[] serverList, Action<List<ServerPingResult>> onComplete)
        {
            Utility.Unity.StartCoroutine(PingServers(serverList, onComplete));
        }
    }

}