#region File Info

// Solution: Excelsior.Web
// Project: Excelsior.Web
// 
// Filename: Person.cs
// Created: 12/02/2011 [1:01 AM]
// Modified: 12/02/2011 [1:09 AM]
// By: Alven

#endregion

#region

using System.ComponentModel;

#endregion

namespace Excelsior.Web.Models
{
	/// <summary>
	/// 	Person.
	/// </summary>
	public class Person
	{
		/// <summary>
		/// 	Person's ID.
		/// </summary>
		[DisplayName("ID")]
		public int Id { get; set; }

		/// <summary>
		/// 	Person's first name.
		/// </summary>
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		/// <summary>
		/// 	Person's last name.
		/// </summary>
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		/// <summary>
		/// 	Person's power level.
		/// </summary>
		[DisplayName("Power Level")]
		public int PowerLevel { get; set; }
	}
}