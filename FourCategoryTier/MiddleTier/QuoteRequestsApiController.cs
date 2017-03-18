using Microsoft.Practices.Unity;
using Sabio.Web.Domain;
using Sabio.Web.Domain.Quotes;
using Sabio.Web.Enums;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.Quotes;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
	[RoutePrefix("api/quoterequest")]
	//[Authorize]
	public class QuotesApiController : ApiController
	{
		// +++++++++++++++++++++++++++++++++++++ DEPENDENCY INJECTION BEGIN +++++++++++++++++++++++++++++++++++++++++++++++++++


		[Dependency]
		public IQuoteRequestService _QuoteRequestService { get; set; }

		[Dependency]
		public IQuoteRequestItemService _QuoteRequestItemService { get; set; }

		[Dependency]
		public IAdminService _AdminService { get; set; }

		[Dependency]
		public IQuoteService _QuoteService { get; set; }

		[Dependency]
		public BidService _BidService { get; set; }

		[Dependency]
		public IshippingQuoteRequestService _ShippingQuoteRequestService { get; set; }

		[Dependency]
		public IAddressService _AddressService { get; set; }

		// +++++++++++++++++++++++++++++++++++++ DEPENDENCY INJECTION END +++++++++++++++++++++++++++++++++++++++++++++++++++






		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route(), HttpGet]
		public HttpResponseMessage QuoteRequestGetAll()
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			List<QuoteRequestDomain> quoteList = _QuoteRequestService.GetAllQuoteRequest();

			var response = new ItemsResponse<QuoteRequestDomain> { Items = quoteList };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route(), HttpPost]
		[Authorize]
		public HttpResponseMessage QuoteRequestInsert(QuoteRequestInsertRequest model)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			//QuoteRequestInsertRequest newQuoteRequest = new QuoteRequestInsertRequest();

			model.UserId = UserService.GetCurrentUserId();

			var userProfile = _AdminService.ProfileGetByUserId(model.UserId);

			model.CompanyId = userProfile.CompanyId;



			int quoteId = _QuoteRequestService.InsertQuoteRequest(model);

			ActivityService activityService = new ActivityService();

			ActivityRequest activity = new ActivityRequest();

			activity.UserId = model.UserId;
			activity.CompanyId = model.CompanyId;
			activity.PrimaryId = quoteId.ToString();
			activity.ActivityType = ActivityType.QuoteRequest;
			activity.ActionType = ActionType.Created;
			activity.Name = model.Name;

			activityService.ActivityInsert(activity);



			ItemResponse<int> response = new ItemResponse<int> { Item = quoteId };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route("{id:int}"), HttpGet]
		[Authorize]
		public HttpResponseMessage GetQuoteById(int id)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			QuoteRequestDomain singleQuote = _QuoteRequestService.GetQuoteRequestById(id);

			ItemResponse<QuoteRequestDomain> response = new ItemResponse<QuoteRequestDomain> { Item = singleQuote };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route("{id:int}"), HttpPut]
		[Authorize]
		public HttpResponseMessage QuoteRequestEdit(QuoteRequestUpdateRequest model)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			bool isSuccessful = _QuoteRequestService.UpdateQuoteRequest(model);

            var qrUserId = UserService.GetCurrentUserId();

            var userProfile = _AdminService.ProfileGetByUserId(qrUserId);

            var qrCompanyId = userProfile.CompanyId;

            ActivityService activityService = new ActivityService();

            ActivityRequest activity = new ActivityRequest();

            activity.UserId = qrUserId;
            activity.CompanyId = qrCompanyId;
            activity.PrimaryId = model.QrId.ToString();
            activity.ActivityType = ActivityType.QuoteRequest;
            activity.ActionType = ActionType.Updated;
            activity.Name = model.Name;

            activityService.ActivityInsert(activity);

            ItemResponse<bool> response = new ItemResponse<bool> { Item = isSuccessful };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route("{id:int}"), HttpDelete]
		[Authorize]
		public HttpResponseMessage QuoteRequestDelete(int id)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			bool isSuccessful = _QuoteRequestService.DeleteQuoteRequest(id);

			ItemResponse<bool> response = new ItemResponse<bool> { Item = isSuccessful };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route("{id:int}/items"), HttpPost]
		[Authorize]
		public HttpResponseMessage QuoteItemInsertRequest(QuoteRequestItemInsertRequest model)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			int quoteId = _QuoteRequestItemService.QuoteRequestItemsInsert(model);

			ItemResponse<int> response = new ItemResponse<int> { Item = quoteId };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route("{quoteRequestId:int}/items"), HttpGet]
		[Authorize]
		public HttpResponseMessage GetQuoteRequestItem(int quoteRequestId)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			List<QuoteRequestItemDomain> quoteList = _QuoteRequestItemService.GetAllQuoteRequestItems(quoteRequestId);

			var response = new ItemsResponse<QuoteRequestItemDomain> { Items = quoteList };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route("{quoteRequestId:int}/{id:int}"), HttpDelete]
		[Authorize]
		public HttpResponseMessage QuoteRequestItemDelete(int id)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			bool isSuccessful = _QuoteRequestItemService.DeleteQuoteRequestItem(id);

			ItemResponse<bool> response = new ItemResponse<bool> { Item = isSuccessful };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}

		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route("{quoteRequestId:int}/items/{id:int}"), HttpPut]
		[Authorize]
		public HttpResponseMessage QuoteRequestItemUpdate(QuoteRequestItemUpdateRequest model)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			bool isSuccessful = _QuoteRequestItemService.UpdateQuoteRequestItem(model);
            

            ItemResponse<bool> response = new ItemResponse<bool> { Item = isSuccessful };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


		[Route("AttachMediaList/{id:int}"), HttpPut]
		[Authorize]
		public HttpResponseMessage QRMediaListInsert(QRMediaInsertListRequest model)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}
			//create response model
			// item = true

			ItemResponse<bool> response = new ItemResponse<bool>();

			response.Item = true;

			_QuoteRequestService.QuoteRequestMediaListInsert(model);


			return Request.CreateResponse(HttpStatusCode.OK, response);


		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


		[Route("AttachMedia/{id:int}"), HttpPut]
		[Authorize]
		public HttpResponseMessage QRMediaInsert(QRMediaInsertRequest model)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}
			//create response model
			// item = true

			ItemResponse<bool> response = new ItemResponse<bool>();

			response.Item = true;

			_QuoteRequestService.InsertQuoteRequestMediaItem(model);


			return Request.CreateResponse(HttpStatusCode.OK, response);


		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



		[Route("attachmedia/update/{QRID:int}"), HttpDelete]
		[Authorize]
		public HttpResponseMessage QuoteRequestsMediaDelete(int QRID) // Action method with model as parameter
		{

			{
				if (!ModelState.IsValid)
				{
					return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
				}

				bool isSuccessful = _QuoteRequestService.DeleteQuoteRequestsMedia(QRID);

				ItemResponse<bool> response = new ItemResponse<bool> { Item = isSuccessful };

				return Request.CreateResponse(HttpStatusCode.OK, response);
			}

		}


		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route("GetMediaId/{QRID:int}"), HttpGet]
		//[Authorize]
		public HttpResponseMessage QuoteRequestsMediaIdGetbyQRID(int QRID)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			List<QuoteRequestsMediaDomain> mediaList = _QuoteRequestService.GetAllMediaByQuoteRequestId(QRID);

			ItemsResponse<QuoteRequestsMediaDomain> response = new ItemsResponse<QuoteRequestsMediaDomain>();

			response.Items = mediaList;

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		[Route("GetMedia/{QRID:int}"), HttpGet]
		//[Authorize]
		public HttpResponseMessage QuoteRequestsMediaGetbyQRID(int QRID)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			List<MediaDomain> mediaList = _QuoteRequestService.GetMediaByQRId(QRID);

			var response = new ItemsResponse<MediaDomain> { Items = mediaList };

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}



		// Create shipping quote request 
		[Route("createshippingqr"), HttpPost]
		public HttpResponseMessage ShippingQuoteRequestItemInsert(ShippingQuoteRequestInsertRequest model)
		{
			if (!ModelState.IsValid)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}

			List<QuoteItemDomain> quoteItemList = _QuoteService.QuoteItemsGetByQuoteId(model.QuoteId);

			model.Name = "New Shipping Quote Request";



			int qrid = _QuoteRequestService.InsertQuoteRequest(model);

			// create a new shipping qr
			// capture shipping qrid 

			foreach (QuoteItemDomain quoteItem in quoteItemList)
			{
				//insert each quote items into a shipping quoterequest item database
				ShippingQrItemRequest shippingQR = new ShippingQrItemRequest();
				shippingQR.Name = "New QR Request";
				shippingQR.Volume = 3;
				shippingQR.Weight = 40;
				shippingQR.Quantity = 4;
				shippingQR.Unit = "30 pounds";
				shippingQR.QrId = qrid;
				shippingQR.QuoteId = 3;

				_ShippingQuoteRequestService.ShippingQuoteRequestItemInsert(shippingQR);

				////int QRId, int userCompanyId - FROM ADDRESS
				//QRInfoDomain fromAddressObject = _BidService.InfoGetByQRId(qrid, model.CompanyId);
				//quote.SellerCompanyAddress1 = fromAddressObject.Address1;
				//quote.SellerCompanyCity = fromAddressObject.City;
				//quote.StateName = fromAddressObject.State;
				//quote.SellerCompanyZipCode = fromAddressObject.ZipCode;

				// TO ADDRESS
				List<QuoteRequestDomain> sendAddressObject = _QuoteRequestService.GetAllQuoteRequest();
				foreach (QuoteRequestDomain address in sendAddressObject)
				{
					shippingQR.AddressId = address.AddressId;


				}

				//AddressDomain toAddressObject = new AddressDomain();
				//toAddressObject = _AddressService.GetAddressById(shippingQR.AddressId);
				//quote.BuyerCompanyAddress1 = toAddressObject.Address1;
				//quote.BuyerCompanyCity = toAddressObject.City;
				//quote.BuyerCompanyState = toAddressObject.State;
				//quote.BuyerCompanyZipCode = toAddressObject.ZipCode;

										
			}



			ItemResponse<int> response = new ItemResponse<int>();
					
			return Request.CreateResponse(HttpStatusCode.OK, response);

		}


	}

}

