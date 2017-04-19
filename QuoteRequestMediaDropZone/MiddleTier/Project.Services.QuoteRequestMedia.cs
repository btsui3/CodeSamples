   project.services.profile = project.services.profile || {};

   project.services.quoteRequestMedia = project.services.quoteRequestMedia || {};

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // AJAX PUT
        //project.services.public.insertMediaId = function (id, data, onAjaxSuccess, onAjaxError) {
   project.services.quoteRequestMedia.insertMediaId = function (id, data, onAjaxSuccess, onAjaxError) {
 
        var url = "/api/quoterequest/attachmedia/" + id;

            var settings = {
                cache: false,
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                data: data,
                dataType: "json",
                success: onAjaxSuccess,
                error: onAjaxError,
                type: "PUT"
            };
            $.ajax(url, settings);
        };

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // AJAX GET 
   project.services.quoteRequestMedia.getByMediaId = function (onAjaxSuccess, onAjaxError) {
            // setting the route prefix + userId
            var url = "/api/quoterequest/attachmedia/update";
            // establish the ajax load
            var settings = {
                cache: false,
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                dataType: "json",
                success: onAjaxSuccess,
                error: onAjaxError,
                // Establish type of ajax call
                type: "GET"

            };

            // call the ajax
            $.ajax(url, settings);

        };



