//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Data.Entity.ModelConfiguration.Conventions;
//using System.Linq;

//namespace Sketch7.Core.Infrastructure.EF.Conventions
//{
//	public class DateConvention : Convention
//	{
//		public DateConvention()
//		{
//			this.Properties<DateTime>()
//				.Where(x => x.GetCustomAttributes(false).OfType<DataTypeAttribute>().Any(a => a.DataType == DataType.Date))
//				.Configure(c => c.HasColumnType("date"));
//		}
//	}
//}