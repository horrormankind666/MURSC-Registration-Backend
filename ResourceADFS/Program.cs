/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๘/๑๒/๒๕๖๒>
Modify date : <๑๘/๑๒/๒๕๖๒>
Description : <>
=============================================
*/

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ResourceServer {
	public class Program {
		public static void Main(string[] args) {
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
	}
}
