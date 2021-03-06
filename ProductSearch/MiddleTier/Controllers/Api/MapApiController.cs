﻿using Microsoft.Practices.Unity;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/Home/MapUtility")]

    public class MapApiController : ApiController
    {
        //==========================================================================================

        [Dependency]
        public IMapService _MapService { get; set; }

        [Route(), HttpPost]

        public HttpResponseMessage MapRadiusBuyersCreate(MapInsertRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            List<MapDomain> buyersList = _MapService.CreateMapRadiusBuyers(model);

            var response = new ItemsResponse<MapDomain> { Items = buyersList };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
