//- Dropzone Modal Controller
(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('qRItemDropZoneModalController', QRItemDropZoneModalController);

    //  $uibModalInstance is coming from the UI Bootstrap library and is a reference to the modal window itself so we can work with it
    //  items is the array passed in from the main controller above through the resolve property
    QRItemDropZoneModalController.$inject = ['$scope', '$baseController', '$quoterequestItemService', '$uibModalInstance', 'QrDropZoneItem', '$QRmediaService', '$alertService']

    function QRItemDropZoneModalController(
        $scope
        , $baseController
        , $quoterequestItemService
        , $uibModalInstance
        , QrDropZoneItem
        , $QRmediaService
        , $alertService

        ) {

        var vm = this;
 
        $baseController.merge(vm, $baseController);

        vm.$scope = $scope;



        vm.$uibModalInstance = $uibModalInstance;
        vm.$quoterequestItemService = $quoterequestItemService;
        vm.$QRmediaService = $QRmediaService;
        vm.$alertService = $alertService;
        vm.QrDropZoneItem = QrDropZoneItem;
        vm.onQRDropzoneSending = _onQRDropzoneSending;
        vm.onQRDropzoneSuccess = _onQRDropzoneSuccess;

        vm.onQRMediaSuccess = _onQRMediaSuccess;

        vm.quoteRequestItemId = null;

        vm.userId = $("#PAGEUSER").val();
        vm.quoteRequestId = $('#quote_request_id').val();
        //vm.selectedQuoteRequestId = project.p.selectedQuoteRequestId;
        
        
        

        vm.notify = vm.$quoterequestItemService.getNotifier($scope);

        vm.ok = function () {
            vm.$uibModalInstance.close(vm.QrDropZoneItem);
        };

        vm.cancel = function () {
            vm.$uibModalInstance.dismiss('cancel');
        };


        
        // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&   DROP ZONE &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&




     


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        function _onQRDropzoneSending(file, xhr, formData) {
            console.log("DZ Sending");

           

            var mediaType = 'QRMediaItem' ; // <-- Set this value in the upload modal. 

            // Rest value to null before closing modal
           

           

           
           // <-- global variable for userid
             // <-- global variable for company id

            formData.append("MediaType", mediaType);
            formData.append("UserId", vm.userId);
           
       

        }; 
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        function _onQRDropzoneSuccess(file, response) {
            console.log("DZ Success");



             //capture mediaid into a shared array in this . grab media id each time. 
            var payload = {

                // put mediaId in payload
      
                'QuoteRequestId': vm.quoteRequestId,
                 'Mediaid': response.item
            
            };

            console.log('payload,', payload);

            var onSuccess = function (data) { console.log("update success! ", data); };
            var onError = function (data) { console.log("An error occured: ", data) };

            vm.$QRmediaService.insertMediaId(vm.quoteRequestId, payload, _onQRMediaSuccess, _onError);
            //project.services.quoteRequestMedia.insertMediaId(mediaId, payload, _onQRMediaSuccess, _onError);

        }; 
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _onQRMediaSuccess(data) {
            console.log("QR Media Item uploaded: ", data.item);
            vm.notify(function () {

                vm.logoUrl = data.item;

            });

            vm.$alertService.success('File Successfully Attached to your Quote Request');
        }



        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _onError(jqXhr, error) {
            console.error(error);

            vm.$alertService.error('Media File was not Attached');

        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++






    }
})();