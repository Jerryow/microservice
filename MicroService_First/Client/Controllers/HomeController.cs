using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Client.Models;
using System.Net.Http;
using Consul;


namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static int _index = 0;//demo暂不考虑线程安全问题,实际加锁或其他方案

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //string url = "http://192.168.1.211:5178/api/userapi/get";

            #region consul
            ConsulClient client = new ConsulClient(x=>
            {
                x.Address = new Uri("http://192.168.1.45:8500/");
                x.Datacenter = "dc1";
            });
            var response = client.Agent.Services().Result.Response;//获取所有服务dictionary

            var serviceDic = response.Where(x => x.Value.Service.Equals("ApiService", StringComparison.OrdinalIgnoreCase)).ToArray();
            #endregion


            AgentService agentService = null;//字典里单个的服务信息


            #region 轮询策略
            if ((_index+1) == serviceDic.Count() && serviceDic.Count() > 0)
            {
                agentService = serviceDic[_index].Value;
                _index = 0;
            }
            else if ((_index + 1) > serviceDic.Count() && serviceDic.Count() > 0)
            {
                _index = 0;
                agentService = serviceDic[0].Value;
            }
            else
            {
                agentService = serviceDic[_index].Value;
                _index++;
            }
            //或
            //agentService = serviceDic[_index++ % 3].Value;//这个要对index值做限定,定时清0
            #endregion

            #region 随机--平均
            //var radom = new Random(_index++).Next(0, serviceDic.Count());
            //agentService = serviceDic[radom].Value;
            #endregion

            #region 权重--需要在服务注册时提供权重比例

            //这里只是思路,tag值我没有填具体的int,可以自己选择, 只是太懒了,不想重新改- -.

            //List<KeyValuePair<string, AgentService>> pairs = new List<KeyValuePair<string, AgentService>>();
            //foreach (var pair in serviceDic)
            //{
            //    var count = 0;
            //    if (pair.Value.Tags[0] == "ApiService_One")
            //    {
            //        count = 1;
            //        for (int i = 0; i < count; i++)
            //        {
            //            pairs.Add(pair);
            //        }
            //    }
            //
            //    if (pair.Value.Tags[0] == "ApiService_Three")
            //    {
            //        count = 6;
            //        for (int i = 0; i < count; i++)
            //        {
            //            pairs.Add(pair);
            //        }
            //    }
            //
            //    if (pair.Value.Tags[0] == "ApiService_Two")
            //    {
            //        count = 3;
            //        for (int i = 0; i < count; i++)
            //        {
            //            pairs.Add(pair);
            //        }
            //    }
            //}
            //
            ////一共10个
            //var radom = new Random(_index++).Next(0, pairs.Count());
            //agentService = pairs.ToArray()[radom].Value;
            #endregion


            string url = $"http://{agentService.Address}:{agentService.Port}/api/userapi/get";

            ViewBag.ip = agentService.Address;
            ViewBag.port = agentService.Port;
            ViewBag.User = Newtonsoft.Json.JsonConvert.DeserializeObject<IService.Models.User>(InvokeApi(url));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string InvokeApi(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage msg = new HttpRequestMessage();
                msg.Method = HttpMethod.Get;
                msg.RequestUri = new Uri(url);
                var res = client.SendAsync(msg).Result;
                return res.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
