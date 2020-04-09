using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;

namespace ApiService.help
{
    public static class ConsulHelper
    {
        public static void ConsulRegist(this IConfiguration configuration, string consulUrl, string registIp, string registPort, string weight, string group, string id)
        {
            try
            {
                Console.WriteLine("url:{0}",consulUrl);
                Console.WriteLine("ip:{0}", registIp);
                Console.WriteLine("port:{0}", registPort);
                //consul启动的实例对象
                ConsulClient client = new ConsulClient(x =>
                {
                    x.Address = new Uri(consulUrl);
                    x.Datacenter = "dc1";
                });

                client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = id,//组成员名称
                    Name = group,//服务Group名称
                    Address = registIp,//服务ip
                    Port = Convert.ToInt32(registPort),//服务端口
                    Tags = new string[] { id },
                    Check = new AgentServiceCheck()
                    {
                        Interval = TimeSpan.FromSeconds(10),//10s心跳检测一次
                        HTTP = $"http://{registIp}:{registPort}/api/checkapi/check",//心跳检测的地址
                        Timeout = TimeSpan.FromSeconds(5),//检测等待时间
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(11)//失败多久后移除
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}
