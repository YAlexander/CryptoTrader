using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.StyledXmlParser.Css.Media;

namespace Site.Core.Reports
{
	public abstract class BaseReportsManager<T> where T : IReport
	{
		public abstract Task<byte[]> GetReport(T data, DocumentOptions options);
		
		protected virtual async Task<string> TransformXmlToHtml(string data, string templateName)
		{
			string xsltResource = await GetResource(templateName);
					
			XslCompiledTransform transform = new XslCompiledTransform();
					
			using (StringReader sr = new StringReader(xsltResource))
			{ 
				using (XmlReader xr = XmlReader.Create(sr))
				{ 
					transform.Load(xr);
				}
			}
			
			StringWriter results = new StringWriter();
			
			using(XmlReader dataReader = XmlReader.Create(new StringReader(data))) 
			{
				transform.Transform(dataReader, null, results);
			}
			
			return results.ToString();
		}
	
		
		private Task<string> GetResource(string resourceName)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(resourceName));

			using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					return reader.ReadToEndAsync();
				}
			}
		}
		
		protected async Task<byte[]> GetPdf(string html, DocumentOptions options)
		{
			await using var ms = new MemoryStream();
			
			ConverterProperties converterProperties = new ConverterProperties();
			converterProperties.SetBaseUri(options.ImagesLocation);
			converterProperties.SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.PRINT));
				
			PdfDocument pdfDocument = new PdfDocument(new PdfWriter(ms));
			pdfDocument.SetDefaultPageSize(MapPageSie(options.PageSize));
				
			HtmlConverter.ConvertToPdf(html, pdfDocument, converterProperties);
			
			return ms.ToArray();
		}
		
		protected string Serialize(T value)
		{
			if (value == null)
			{
				throw new NullReferenceException($"Serialization error: {nameof(T)} can't be null");
			}
			
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				StringWriter stringWriter = new StringWriter();
				using (XmlWriter writer = XmlWriter.Create(stringWriter))
				{
					xmlSerializer.Serialize(writer, value);
					return stringWriter.ToString();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred", ex);
			}
		}

		private PageSize MapPageSie(string size)
		{
			return size.ToUpper() switch
			{
				nameof(PageSize.A0) => PageSize.A0,
				nameof(PageSize.A1) => PageSize.A1,
				nameof(PageSize.A2) => PageSize.A2,
				nameof(PageSize.A3) => PageSize.A3,
				nameof(PageSize.A4) => PageSize.A4,
				nameof(PageSize.A5) => PageSize.A5,
				nameof(PageSize.A6) => PageSize.A6,
				nameof(PageSize.A7) => PageSize.A7,
				nameof(PageSize.A8) => PageSize.A8,
				nameof(PageSize.A9) => PageSize.A9,
				nameof(PageSize.A10) => PageSize.A10,
				nameof(PageSize.B0) => PageSize.B0,
				nameof(PageSize.B1) => PageSize.B1,
				nameof(PageSize.B2) => PageSize.B2,
				nameof(PageSize.B3) => PageSize.B3,
				nameof(PageSize.B4) => PageSize.B4,
				nameof(PageSize.B5) => PageSize.B5,
				nameof(PageSize.B6) => PageSize.B6,
				nameof(PageSize.B7) => PageSize.B7,
				nameof(PageSize.B8) => PageSize.B8,
				nameof(PageSize.B9) => PageSize.B9,
				nameof(PageSize.B10) => PageSize.B10,
				nameof(PageSize.LETTER) => PageSize.LETTER,
				nameof(PageSize.LEGAL) => PageSize.LEGAL,
				nameof(PageSize.TABLOID) => PageSize.TABLOID,
				nameof(PageSize.LEDGER) => PageSize.LEDGER,
				nameof(PageSize.EXECUTIVE) => PageSize.EXECUTIVE,
				_ => throw new Exception($"Unknown format {size}")
			};
		}
	}
}
