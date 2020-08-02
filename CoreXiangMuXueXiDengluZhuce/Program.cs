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
            /*�˴����������ʣ�
             * 1. UseSerilog�е�����json���ú�ConfigureLogging�еĴ���д�������Ƿ����ظ��ģ����Ƕ��Ǳ���ģ�
             * 2. UseSerilog������Ҫ����WriteToע�ᣬ�˴�Ϊ��û�У�
             * 3. json�ļ��ȸ��£��޸���־������Ч�������޸���־�����ʽ��Ч��������������ЧΪʲô����
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
