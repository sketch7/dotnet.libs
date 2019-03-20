using Microsoft.EntityFrameworkCore;

namespace Sketch7.Core.Entity.Repository
{
	public class DatabaseFactory<TContext> : Disposable, IDatabaseFactory<TContext>
		where TContext : DbContext, new()
	{
		private TContext _dataContext;

		public TContext Get() => _dataContext ?? (_dataContext = new TContext());

		protected override void DisposeCore() => _dataContext?.Dispose();
	}
}