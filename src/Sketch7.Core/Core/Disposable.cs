using System;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	public class Disposable : IDisposable
	{
		private bool _isDisposed;

		~Disposable()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!_isDisposed && disposing)
			{
				DisposeCore();
			}

			_isDisposed = true;
		}

		protected virtual void DisposeCore()
		{
		}
	}
}