using project.Web.Domain;
using project.Web.Domain.Quotes;
using project.Web.Models.Requests;
using project.Web.Models.Requests.Quotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Web.Services.Interfaces
{
    public interface IQuoteRequestService
    {
        List<QuoteRequestsMediaDomain> GetAllMediaByQuoteRequestId(int QRID);

        List<MediaDomain> GetMediaByQRId(int qrId);

        void InsertQuoteRequestMediaItem(QRMediaInsertRequest mediaItem);

        bool QuoteRequestMediaListInsert(QRMediaInsertListRequest model);

        bool QuoteRequestMediaInsert(QRMediaInsertRequest model);

        bool DeleteQuoteRequestsMedia(int QRID);

        List<QuoteRequestDomain> GetAllQuoteRequest();

        List<QuoteRequestDomain> GetQuoteRequestsByCompanyId(int CompanyId);

        QuoteRequestDomain GetQuoteRequestById(int id);

        int InsertQuoteRequest(QuoteRequestInsertRequest model);

        bool UpdateQuoteRequest(QuoteRequestUpdateRequest model);

        bool DeleteQuoteRequest(int id);
  
        bool QuoteRequestSMAttemptCancel(int id);

        bool QuoteRequestSMAttemptDelete(int id);

        bool QuoteRequestSMAttemptePublish(int id);

        bool QuoteRequestSMAttemptPending(int id);

        bool QuoteRequestSMAttemptRepublish(int id);

        bool QuoteRequestSMAttemptReject(int id);

        bool QuoteRequestSMAttemptWithdraw(int id);

        bool QuoteRequestSMAttemptComplete(int id);

        QuoteRequestDomain GetQuoteRequestStatusId(int id);

        List<QuoteRequestDomain> GetQuoteRequestsByCompanyIdAndStatus(int CompanyId);






    }
}
