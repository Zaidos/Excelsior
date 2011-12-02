#region File Info

// Solution: Excelsior.Web
// Project: Excelsior.Web
// 
// Filename: HomeController.cs
// Created: 12/01/2011 [11:48 PM]
// Modified: 12/02/2011 [1:09 AM]
// By: Alven

#endregion

#region

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Excelsior.Web.Models;

#endregion

namespace Excelsior.Web.Controllers
{
	/// <summary>
	/// 	Home controller.
	/// </summary>
	public class HomeController : Controller
	{
		/// <summary>
		/// 	List of people. Demonstration purposes only.
		/// </summary>
		private readonly List<Person> _people;

		/// <summary>
		/// 	Constructor.
		/// </summary>
		public HomeController()
		{
			_people = new List<Person>
			          	{
			          		new Person
			          			{
			          				FirstName = "Steve",
			          				LastName = "Jobs",
			          				Id = 1,
			          				PowerLevel = 9001
			          			},
			          		new Person
			          			{
			          				FirstName = "Ray",
			          				LastName = "Lewis",
			          				Id = 35,
			          				PowerLevel = 52
			          			},
			          		new Person
			          			{
			          				FirstName = "Mark",
			          				LastName = "Zuckerberg",
			          				Id = 4,
			          				PowerLevel = 700000000
			          			}
			          	};
		}

		/// <summary>
		/// 	Home page.
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			return View("Index");
		}

		/// <summary>
		/// 	Returns an excel file random people.
		/// </summary>
		/// <returns></returns>
		public ActionResult Excelsiate()
		{
			return this.Excel("file.xls", "worksheet", _people.AsQueryable());
		}
	}
}