#region File Info

// Solution: Excelsior.Web
// Project: Excelsior
// 
// Filename: ExcelResult.cs
// Created: 12/02/2011 [1:14 AM]
// Modified: 12/02/2011 [1:14 AM]
// By: Alven

#endregion

#region

using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace Excelsior.Web.Excelsior
{
	/// <summary>
	/// 	Excel result.
	/// </summary>
	public class ExcelResult : ActionResult
	{
		#region Private Methods

		/// <summary>
		/// 	Excel file name.
		/// </summary>
		private readonly string _fileName;

		/// <summary>
		/// 	Excel row data.
		/// </summary>
		private readonly IQueryable _rows;

		private TableItemStyle _headerStyle;

		/// <summary>
		/// 	Excel headers.
		/// </summary>
		private string[] _headers;

		private TableItemStyle _itemStyle;
		private TableStyle _tableStyle;

		/// <summary>
		/// 	Excel worksheet name.
		/// </summary>
		private string _worksheetName;

		#endregion

		#region Public Properties

		/// <summary>
		/// 	Excel file name.
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
		}

		/// <summary>
		/// 	Excel row data.
		/// </summary>
		public IQueryable Rows
		{
			get { return _rows; }
		}

		#endregion

		#region Constructor and Initialization

		/// <summary>
		/// 	Initializes a new excel result with a filename, sheetname 
		/// 	and row data.
		/// </summary>
		/// <param name = "fileName">Excel filename.</param>
		/// <param name = "sheetName">Spreadsheet filename.</param>
		/// <param name = "rows">Row data.</param>
		public ExcelResult(string fileName, string sheetName, IQueryable rows)
			: this(fileName, sheetName, rows, null)
		{
		}

		/// <summary>
		/// 	Initializes a new excel result with filename, sheetname, row data,
		/// 	and cell headers.
		/// </summary>
		/// <param name = "fileName">Excel filename.</param>
		/// <param name = "sheetName">Spreadsheet filename.</param>
		/// <param name = "rows">Row data.</param>
		/// <param name = "headers">Cell column headers.</param>
		public ExcelResult(string fileName, string sheetName, IQueryable rows, string[] headers)
		{
			_fileName = fileName;
			_worksheetName = sheetName;
			_rows = rows;
			_headers = headers;

			SetDefaultStyles();
		}

		/// <summary>
		/// 	Assigns styles to  default plain ones.
		/// </summary>
		private void SetDefaultStyles()
		{
			if (_tableStyle == null) SetTableStyle();
			if (_headerStyle == null) SetHeaderStyle();
			if (_itemStyle == null) SetItemStyle();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 	Set cell item style.
		/// </summary>
		/// <param name = "itemStyle"></param>
		public void SetItemStyle(TableItemStyle itemStyle = null)
		{
			_itemStyle = itemStyle ??
			             new TableItemStyle {BackColor = Color.White};
		}

		/// <summary>
		/// 	Set cell column header style.
		/// </summary>
		/// <param name = "headerStyle"></param>
		public void SetHeaderStyle(TableItemStyle headerStyle = null)
		{
			_headerStyle = _headerStyle ??
			               new TableItemStyle {BackColor = Color.LightGray};
		}

		/// <summary>
		/// 	Set table style.
		/// </summary>
		/// <param name = "tableStyle"></param>
		public void SetTableStyle(TableStyle tableStyle = null)
		{
			if (tableStyle != null)
				_tableStyle = tableStyle;
			else
			{
				_tableStyle = new TableStyle
				              	{
				              		BorderStyle = BorderStyle.Solid,
				              		BorderColor = Color.Black,
				              		BorderWidth = Unit.Parse("1px")
				              	};
			}
		}

		#endregion

		#region Overrides

		/// <summary>
		/// 	Execute result override.
		/// </summary>
		/// <param name = "context">Controller context.</param>
		public override void ExecuteResult(ControllerContext context)
		{
			var sw = new StringWriter();
			var tw = new HtmlTextWriter(sw);

			if (_tableStyle != null)
				_tableStyle.AddAttributesToRender(tw);
			tw.RenderBeginTag(HtmlTextWriterTag.Table);

			// If headers are null, attempt to grab from reflection.
			if (_headers == null)
			{
				_headers = _rows
					.ElementType
					.GetProperties()
					.Select(m => m.Name)
					.ToArray();
			}

			// Write table head.
			tw.RenderBeginTag(HtmlTextWriterTag.Thead);
			foreach (var header in _headers)
			{
				if (_headerStyle != null)
					_headerStyle.AddAttributesToRender(tw);
				tw.RenderBeginTag(HtmlTextWriterTag.Th);

				var tempHeader = header;

				// Check if object has DisplayName attribute.
				var attrs = _rows
					.ElementType
					.GetProperty(tempHeader)
					.GetCustomAttributes(typeof (DisplayNameAttribute), false);

				// Override if data annotations are present.
				if (attrs.Any())
					foreach (var attr in attrs.Cast<DisplayNameAttribute>())
						tempHeader = attr.DisplayName;

				tw.Write(tempHeader);
				tw.RenderEndTag();
			}
			tw.RenderEndTag();

			// Write table rows.
			tw.RenderBeginTag(HtmlTextWriterTag.Tbody);
			foreach (var row in _rows)
			{
				tw.RenderBeginTag(HtmlTextWriterTag.Tr);
				if (_headers != null)
					foreach (var header in _headers)
					{
						var value = string.Empty;

						if (row.GetType().GetProperty(header).GetValue(row, null) != null)
						{
							value = row.GetType().GetProperty(header).GetValue(row, null).ToString();
							value = ReplaceSpecialCharacters(value);
						}
						if (_itemStyle != null)
							_itemStyle.AddAttributesToRender(tw);

						tw.RenderBeginTag(HtmlTextWriterTag.Td);
						tw.Write(HttpUtility.HtmlEncode(value));
						tw.RenderEndTag();
					}
				tw.RenderEndTag();
			}
			tw.RenderEndTag();

			WriteFile(_fileName, "applications/ms-excel", sw.ToString());
			tw.Close();
			sw.Close();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 	Replace special characters. You know, the special ones.
		/// </summary>
		/// <param name = "value">Unchecked value.</param>
		/// <returns>Stripped string. Accepts dollars ;)</returns>
		private static string ReplaceSpecialCharacters(string value)
		{
			return value
				.Replace("’", "'")
				.Replace("“", "\"")
				.Replace("”", "\"")
				.Replace("–", "-")
				.Replace("…", "...");
		}

		/// <summary>
		/// 	Write our file.
		/// </summary>
		/// <param name = "fileName">Filename.</param>
		/// <param name = "contentType">Content type.</param>
		/// <param name = "content">Content.</param>
		private static void WriteFile(string fileName, string contentType, string content)
		{
			var context = HttpContext.Current;
			context.Response.Clear();
			context.Response.AddHeader(
				"content-disposition",
				string.Format("attachment;filename={0}", fileName));
			context.Response.Charset = string.Empty;
			context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			context.Response.ContentType = contentType;
			context.Response.Write(content);
			context.Response.End();
		}

		#endregion
	}
}