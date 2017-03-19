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
using System.Web.Http;
using System.Web.Http.Routing;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/products")]
    public class ProductsApiController : ApiController
    {
        [Dependency]
        public IProductService _ProductService { get; set; }


        [Dependency]
        public IAdminService _AdminService { get; set; }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        [Route(), HttpPost]
        [Authorize]
        public HttpResponseMessage ProductInsert(ProductInsertRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string userId = UserService.GetCurrentUserId();

            UserProfileDomain userProfile = _AdminService.ProfileGetByUserId(userId);

            model.UserId = userId;
            model.CompanyId = userProfile.CompanyId;

            ProductService ProductInsert = new ProductService();

            int productId = _ProductService.ProductInsert(model);

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = productId;
            
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        [Route(), HttpGet]
        [Authorize]
        public HttpResponseMessage GetAllProducts()
        {

            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            List<ProductDomain> productArray = _ProductService.GetAllProducts();

            ItemsResponse<ProductDomain> response = new ItemsResponse<ProductDomain>(); // { Items = productArray };

            response.Items = productArray;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }



        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        [Route("{id:int}"), HttpGet]
        [Authorize]
        public HttpResponseMessage GetProductById(int id)
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ProductDomain product = _ProductService.GetProductById(id);

            ItemResponse<ProductDomain> response = new ItemResponse<ProductDomain>(); // { Item = product };

            response.Item = product;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [Route("companyid")HttpGet]
        public HttpResponseMessage GetProductsByCompanyId()
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            string userId = UserService.GetCurrentUserId();

            UserProfileDomain userProfile = _AdminService.ProfileGetByUserId(userId);
            
            List<ProductDomain> products = _ProductService.GetProductsByCompanyId(userProfile.CompanyId);

            ItemsResponse<ProductDomain> response = new ItemsResponse<ProductDomain>();

            response.Items = products;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        [Route("{id:int}")HttpPut]
        [Authorize]
        public HttpResponseMessage ProductEdit(ProductUpdateRequest model)
        {
            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool isSuccessful = _ProductService.UpdateProduct(model);

            ItemResponse<bool> response = new ItemResponse<bool>(); // { Item = isSuccessful };

            response.Item = isSuccessful;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }



        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        [Route("{id:int}"), HttpDelete]
        [Authorize]
        public HttpResponseMessage ProductDelete(int id)
        {

            if(!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }                       
            
            bool isSuccessfull = _ProductService.DeleteProduct(id);

            ItemResponse<bool> response = new ItemResponse<bool>(); // { Item = isSuccessfull };

            response.Item = isSuccessfull;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
