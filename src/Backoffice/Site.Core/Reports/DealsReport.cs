using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Site.Core.Reports.ReportsData;

namespace Site.Core.Reports
{
	public class DealsReport : BaseReportsManager<DealsReportData>
	{
		private readonly IOptions<SiteSettings> _settings;
		public DealsReport(IOptions<SiteSettings> settings)
		{
			_settings = settings;
		}
		
		public override async Task<byte[]> GetReport(DealsReportData data)
		{
			DocumentOptions options = new DocumentOptions();
			options.BaseUrl = _settings.Value.BaseUrl;
			
			string serializedData = Serialize(data);
			string xsltResource = await GetResource($"{nameof(DealsReport)}.xslt");
			string html = await base.TransformXmlToHtml(serializedData, xsltResource);
			return await base.GetPdf(html, options);
		}
	}
}