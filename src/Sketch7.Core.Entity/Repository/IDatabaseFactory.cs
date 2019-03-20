using Microsoft.EntityFrameworkCore;
using System;

namespace Sketch7.Core.Entity.Repository
{
	public interface IDatabaseFactory<out TContext> : IDisposable where TContext : DbContext //Base
	{
		TContext Get();
	}
}