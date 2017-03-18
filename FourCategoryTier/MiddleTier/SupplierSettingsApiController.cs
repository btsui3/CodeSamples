using Microsoft.Practices.Unity;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/supplierNotifcationSettings")]
    public class SupplierSettingsApiController : ApiController
    {

        //....// ===================== DEPENDENCY INJECTION START ==============================


        [Dependency]
        public IAdminService _AdminService { get; set; }

        [Dependency]
        public ISupplierSettingsService _SupplierSettingsService { get; set; }


        //....// ===================== DEPENDENCY INJECTION END ================================


        [Route(), HttpPost]
        [Authorize]
        public HttpResponseMessage SettingInsert(SupplierNotificationSettingRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string userId = UserService.GetCurrentUserId();

            UserProfileDomain userProfile = _AdminService.ProfileGetByUserId(userId);

            model.UserId = userId;
            model.CompanyId = userProfile.CompanyId;

            SupplierSettingsService InsertSupplierSettings = new SupplierSettingsService();

            int settingId = _SupplierSettingsService.SupplierSettingInsert(model);

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = settingId;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("companyid"), HttpGet]
        [Authorize]
        public HttpResponseMessage GetSettingsByUserId()
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string userId = UserService.GetCurrentUserId();

            List<SupplierNotificationSettingsDomain> settings = _SupplierSettingsService.GetSettingsByUserId(userId);

            ItemsResponse<SupplierNotificationSettingsDomain> response = new ItemsResponse<SupplierNotificationSettingsDomain>();

            response.Items = settings;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("{id:int}"), HttpPut]
        [Authorize]
        public HttpResponseMessage EditSetting(SupplierNotificationSettingRequest model)
        {

            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            }

            bool isSuccessfull = _SupplierSettingsService.EditSupplierNotifySetting(model);

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = isSuccessfull;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("{id:int}"), HttpDelete]
        [Authorize]
        public HttpResponseMessage DeleteSetting(int Id)
        {

            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool isSuccessfull = _SupplierSettingsService.DeleteSupplierNotifySetting(Id);

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = isSuccessfull;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

    }
}