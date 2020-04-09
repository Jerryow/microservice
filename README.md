# microservice

环境：.net core 3.1 + vmware(全部本地部署也行,最好多台server) + consul + ocelot

1.此实例做的是一个.core webapi + ocelot(网关) + consul 的一个demo
2.需要自己做的内容：
   1)core的集群部署,本地/虚拟server都行
   2)网关涵盖各种不同的配置方式,从单个集群转发到搭配consul配置可以自行尝试
   3)网关的缓存以及consul的缓存可以自定义实现
   


下一步:
服务治理
1.超时
2.熔断
3.限流
4.降级