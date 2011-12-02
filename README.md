# Excelsior
### Create excel files directly in ASP.NET MVC

## Setup

- Add ExcelResult.cs and ExcelControllerExtension.cs to your project.
- Change the namespace of ExcelControllerExtension to match your controller's namespace.

## Usage

- Make the proper call to 'Excel'
- Include a filename (with proper extension), worksheet name, and your data rows as IQueryable

```c#
return this.excel("fileName.xls", "worksheetName", data.asQueryable());
```

## Try it!

Run the project in Visual Studio and click 'Excelsiate' on the main screen.
