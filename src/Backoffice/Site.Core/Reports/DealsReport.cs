using System.Threading.Tasks;
using Site.Core.Reports.ReportsData;

namespace Site.Core.Reports
{
	public class DealsReport : BaseReportsManager<DealsReportData>
	{
		public override async Task<byte[]> GetReport(DealsReportData data, DocumentOptions options)
		{
			string resourceName = $"{nameof(DealsReport)}.xslt";
			string serializedData = base.Serialize(data);

			string html = await base.TransformXmlToHtml(serializedData, resourceName);

			return await base.GetPdf(html, options);
		}
	}
}