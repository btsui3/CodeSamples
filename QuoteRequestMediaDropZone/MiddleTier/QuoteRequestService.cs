using Microsoft.Practices.Unity;
using project.Data;
using project.Web.Domain;
using project.Web.Domain.Quotes;
using project.Web.Enums.QuoteRequestBidWorkflow;
using project.Web.Enums.QuoteWorkflow;
using project.Web.Hubs;
using project.Web.Enums;
using project.Web.Models.Requests;
using project.Web.Models.Requests.Quotes;
using project.Web.Models.Requests.User;
using project.Web.Models.Responses;
using project.Web.Services.Interfaces;
using project.Web.Services.S3Service;
using project.Web.Services.Workflow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;


namespace project.Web.Services
{
	public class QuoteRequestService : BaseService, IQuoteRequestService 
	{
		[Dependency]
		public IUserProfileService _UserProfileService { get; set; }

		// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


		public List<QuoteRequestsMediaDomain> GetAllMediaByQuoteRequestId(int QRID)
		{

			List<QuoteRequestsMediaDomain> quoteMediaList = null;

			DataProvider.ExecuteCmd(GetConnection, "dbo.QuoteRequestsMedia_SelectAllByQuoteRequestId"
				, inputParamMapper: delegate (SqlParameterCollection paramCollection)
				{
					// Pass in data to the Database
					paramCollection.AddWithValue("@QuoteRequestsId", QRID);

				}, map: delegate (IDataReader reader, short set)
				{
					QuoteRequestsMediaDomain SingleQuoteRequestMedia = new QuoteRequestsMediaDomain();
					int startingIndex = 0;


					SingleQuoteRequestMedia.QuoteRequestId = reader.GetSafeInt32(startingIndex++);
					SingleQuoteRequestMedia.MediaId = reader.GetSafeInt32(startingIndex++);

					if (quoteMediaList == null)
					{
						quoteMediaList = new List<QuoteRequestsMediaDomain>();
					}

					quoteMediaList.Add(SingleQuoteRequestMedia);
				});

			return quoteMediaList;
		}



		// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

		public List<MediaDomain> GetMediaByQRId(int qrId)
		{
			List<MediaDomain> mediaList = null;

			try
			{
				// Returns a list of QR-id and media-id pairs. 
				List<QuoteRequestsMediaDomain> mediaItems = GetAllMediaByQuoteRequestId(qrId);

				// We will be using the media ids to get a list of media domains.

				if(mediaItems != null)
				{
					foreach (QuoteRequestsMediaDomain qrMedia in mediaItems)
					{
						MediaService mediaService = new MediaService();

						MediaDomain media = mediaService.GetMediaById(qrMedia.MediaId);

						if (mediaList == null)
						{
							mediaList = new List<MediaDomain>();
						}

						mediaList.Add(media);
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return mediaList;

		}

		// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%



		public bool QuoteRequestMediaListInsert(QRMediaInsertListRequest model)
		{
			bool isSuccessful = false;


			foreach (int mediaIdItem in model.MediaIdList)
			{
				QRMediaInsertRequest mediaItem = new QRMediaInsertRequest();
				mediaItem.MediaId = mediaIdItem;
				mediaItem.QuoteRequestId = model.QuoteRequestId;
				//Call Service method to sent MediaItem
				InsertQuoteRequestMediaItem(mediaItem);

			}

			isSuccessful = true;

			return isSuccessful;
		}


		// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

		public void InsertQuoteRequestMediaItem(QRMediaInsertRequest mediaItem)
		{

			DataProvider.ExecuteNonQuery(GetConnection, "dbo.QuoteRequestsMedia_Insert"
				, inputParamMapper: delegate (SqlParameterCollection paramCollection)
				{
					paramCollection.AddWithValue("@QuoteRequestId", mediaItem.QuoteRequestId);
					paramCollection.AddWithValue("@MediaId", mediaItem.MediaId);

				},
				returnParameters: delegate (SqlParameterCollection param)

				{

				});

		}


		// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


		public bool DeleteQuoteRequestsMedia(int QRID)
		{
			bool success = false;

			try
			{
				DataProvider.ExecuteNonQuery(GetConnection, "dbo.QuoteReqeuestsMedia_Delete"
					   , inputParamMapper: delegate (SqlParameterCollection paramCollection)
					   {
						   paramCollection.AddWithValue("@QuoteRequestId", QRID);

						   success = true;
					   });
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return success;
		}



		/// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%





		
        

		public bool QuoteRequestMediaInsert(QRMediaInsertRequest model)
		{
			throw new NotImplementedException();
		}
	}
}
