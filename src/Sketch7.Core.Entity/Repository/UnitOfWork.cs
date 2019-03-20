using Microsoft.EntityFrameworkCore;

namespace Sketch7.Core.Entity.Repository
{
	public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext //Base
	{
		private readonly IDatabaseFactory<TContext> _databaseFactory;
		private TContext _dataContext;

		public UnitOfWork(IDatabaseFactory<TContext> databaseFactory)
		{
			_databaseFactory = databaseFactory;
		}

		protected TContext DataContext => _dataContext ?? (_dataContext = _databaseFactory.Get());

		public void Commit() => DataContext.SaveChanges();
	}
}