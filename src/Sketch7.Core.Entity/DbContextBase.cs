using Microsoft.EntityFrameworkCore;

namespace Sketch7.Core.Entity
{
	/// <summary>
	/// 	A DbContextBase
	/// </summary>
	public class DbContextBase : DbContext
	{
		public DbContextBase()
			: base()
		{
			
			//Configuration.ProxyCreationEnabled = false;
		}

		public DbContextBase(DbContextOptions options)
			: base(options)
		{
			// Configuration.ProxyCreationEnabled = false;
		}
	}
}