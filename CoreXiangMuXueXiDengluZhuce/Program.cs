using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CoreXiangMuXueXiDengluZhuce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(p => 
            {
                p.AddJsonFile("Register.json",optional:true,reloadOnChange:true);
            })
            /*此处有三个疑问：
             * 1. UseSerilog中的增加json配置和ConfigureLogging中的代码写死配置是否是重复的？还是都是必须的？
             * 2. UseSerilog看官网要求有WriteTo注册，此处为何没有？
             * 3. json文件热更新，修改日志级别有效，但是修改日志输出格式无效（需重启才能生效为什么？）
             * */

            .UseSerilog((context, configuration) => 
            {
                IConfigurationRoot SerilogConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("wenSerilog.json", true, true)
                .AddEnvironmentVariables()
                .Build();

                configuration.ReadFrom.Configuration(SerilogConfigurationRoot);
            }).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
