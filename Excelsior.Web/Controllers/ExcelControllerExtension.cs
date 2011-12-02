#region File Info

// Solution: Excelsior.Web
// Project: Excelsior
// 
// Filename: ExcelControllerExtension.cs
// Created: 12/02/2011 [1:14 AM]
// Modified: 12/02/2011 [1:14 AM]
// By: Alven

#endregion

#region

using System.Linq;
using System.Web.Mvc;
using Excelsior.Web.Excelsior;

#endregion

namespace Excelsior.Web.Controllers
{
	/// <summary>
	/// 	Excel controller extension.
	/// </summary>
	public static class ExcelControllerExtension
	{
		/// <summary>
		/// 	Extends a controller to return an excel file by filename, 
		/// 	worksheet name, and data rows.
		/// </summary>
		/// <param name = "controller">Controller.</param>
		/// <param name = "fileName">Excel filename.</param>
		/// <param name = "worksheetName">Worksheet name.</param>
		/// <param name = "rows">Data rows.</param>
		/// <returns></returns>
		public static ActionResult Excel(this Controller controller,
		                                 string fileName, string worksheetName, IQueryable rows)
		{
			return new ExcelResult(fileName, worksheetName, rows);
		}

		/// <summary>
		/// 	Extends a controller to return an excel file by filename, 
		/// 	worksheet name, data rows, and pre-set headers.
		/// </summary>
		/// <param name = "controller">Controller.</param>
		/// <param name = "fileName">Excel filename.</param>
		/// <param name = "worksheetName">Worksheet name.</param>
		/// <param name = "rows">Data rows.</param>
		/// <param name = "headers">Cell column headers</param>
		/// <returns></returns>
		public static ActionResult Excel(this Controller controller,
		                                 string fileName, string worksheetName, IQueryable rows,
		                                 string[] headers)
		{
			return new ExcelResult(fileName, worksheetName, rows, headers);
		}
	}
}