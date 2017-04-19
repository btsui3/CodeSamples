using project.Web.Models.Requests.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project.Web.Models.Requests
{
    public class QRMediaInsertListRequest
    {
        public int QuoteRequestId { get; set; }

        public List<int> MediaIdList { get; set; }

    }
}