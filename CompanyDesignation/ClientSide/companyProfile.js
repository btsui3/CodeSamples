// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// Company Profile Controller
(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('companyProfileController', CompanyProfileController);

    CompanyProfileController.$inject = ['$scope', '$baseController'
        , '$companyProfileService', '$uibModal', '$mediaService', '$companyDesignationsService'];

    function CompanyProfileController(
      $scope
      , $baseController
      , $companyProfileService
      , $uibModal
      , $mediaService
      , $companyDesignationsService
                   ) {

        var vm = this;

        // Injection
        vm.$scope = $scope;
        vm.$companyProfileService = $companyProfileService;
        vm.$uibModal = $uibModal;
        vm.$mediaService = $mediaService;
        vm.$companyDesignationsService = $companyDesignationsService;


        $baseController.merge(vm, $baseController);

        // Properties
        vm.companyId = project.p.companyId;
        vm.companyPhoto = project.p.companyPhotoUrl;
        vm.item = null;
        vm.designations = {
            SmallBusiness: 1,
            VeteranOwned: 2,
            MinorityOwned: 4,
            WomenOwned: 8
        }

        vm.companyRole = null;

        vm.buyerQuotes = null;

        vm.logoUrl = '/Content/Theme/Images/Industry_Logo_not_open_src.jpg';

        // Methods
        vm.renderThisCompany = _renderThisCompany;
        vm.receiveItem = _receiveItem;

        vm.openEditModal = _openEditModal;
        vm.openListOfBuyersModal = _openListOfBuyersModal;

        vm.getBuyerCompanyQuotes = _getBuyerCompanyQuotes;

        vm.notify = vm.$companyProfileService.getNotifier($scope);

        vm.payload = {
            "companyId": project.p.companyId,
            "ownerId": project.p.companyOwnerId,
            "name": "",
            "typeId": project.p.companyRole,
            "phone": '',
            "fax": '',
            "email": '',
            "mediaId": 0,
            "url": "",
            "designations": 0
        };

        // startup
        _renderThisCompany();

        

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _renderThisCompany() {
            // console.log("Rendering company.");
            //  defer data operations into an external service: https://github.com/johnpapa/angular-styleguide#style-y035
            vm.$companyProfileService.getById(vm.companyId, vm.receiveItem, vm.onEmpError);


        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _receiveItem(data) {
            console.log("Handling received data: ", data);

            vm.notify(function () {
                vm.item = data.item;

                if (data.item.typeId === 1) {
                    vm.companyRole = 'Buyer'
                }
                else if (data.item.typeId === 2) {
                    vm.companyRole = 'Supplier'
                }
                else if (data.item.typeId === 3) {
                    vm.companyRole = 'Shipper'
                }
                else {
                    vm.companyRole = 'Undefined'
                }

                // Update company logo
                _getCompanyLogo(vm.item.mediaId);

                vm.payload.name = vm.item.name;
                vm.payload.phone = vm.item.phone;
                vm.payload.fax = vm.item.fax;
                vm.payload.email = vm.item.email;
                vm.payload.url = vm.item.url;
                vm.payload.mediaId = vm.item.mediaId;
                vm.payload.designations = vm.item.designations;

                data.item.designationURLS = vm.$companyDesignationsService.getURLS(data.item.designations)


            });

            if (vm.companyRole == 1 || vm.companyRole == "Buyer") {
                _getBuyerCompanyQuotes();
            }
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _getBuyerCompanyQuotes() {
            vm.$companyProfileService.getQuotesByBuyerCompanyId(vm.companyId, _onRecieveCompanyQuotes, _onEmpError);
        }


        function _onRecieveCompanyQuotes(data) {
            vm.notify(function () {
                console.log(">>>>>>>>>>>>Buyer Company QUOTES", data);
                vm.buyerQuotes = data.items;
            });
            
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _getCompanyLogo(mediaId) {

            vm.$mediaService.getUrlByMediaId(mediaId, _onRecieveLogoUrl, _onEmpError);

        }



        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _onRecieveLogoUrl(data) {
            // console.log("logo url: ", data.item);
            vm.notify(function () {
                vm.logoUrl = data.item;
            });
        }



        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _onEmpError(jqXhr, error) {
            console.error(error);
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _openEditModal() {

            var modalInstance = vm.$uibModal.open({
                animation: true,

                // This tells it what html template to use.
                // It must exist in a script tag OR external file.
                templateUrl: '/Scripts/app/Company/Templates/editProfileModal.html',

                // This controller must exist & be registered with angular in order to work.
                controller: 'companyModalController as mc',
                size: 'md',

                // Anything in resolve can be injected into the modal controller as shown below
                resolve: {
                    companyDetails: function () {
                        //console.log('vm.payload.designations:', vm.payload.designations);

                        //return vm.payload;

                        var companyDetails = {};

                        companyDetails.payload = vm.payload;

                        companyDetails.designations = {};

                        companyDetails.designations.smallBusiness = (vm.payload.designations & vm.designations.SmallBusiness);
                        companyDetails.designations.veteranOwned = (vm.payload.designations & vm.designations.VeteranOwned);
                        companyDetails.designations.minorityOwned = (vm.payload.designations & vm.designations.MinorityOwned);
                        companyDetails.designations.womenOwned = (vm.payload.designations & vm.designations.WomenOwned);

                        return companyDetails;


                    }
                }
            });

            //  when the modal closes it returns a promise
            modalInstance.result.then(function () {

                _renderThisCompany();

            }, function () {

                //  If the user closed the modal by clicking cancel.
                //console.log('Modal dismissed at: ' + new Date());

            });
        } // End _openEditModal


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _openListOfBuyersModal() {

            var listOfBuyersModal = vm.$uibModal.open({
                animation: true,
                templateUrl: '/Scripts/App/Company/Templates/buyersModal.html',
                controller: 'listOfBuyersModalController as lbmc',
                size: 'sm',
                resolve: {
                    items: function () {
                        return vm.items;
                    }
                }
            });

            listOfBuyersModal.result.then(function () {


            }, function () {

                //console.log('Modal closed at ' + new Date());

            });
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _onDropzoneSending(file, xhr, formData) {
            console.log("DZ Sending");

            var mediaType = "companyLogo"; // <-- Set this value in the upload modal. 

            // Rest value to null before closing modal
            var userId = project.p.currentUserId; // <-- global variable for userid
            var companyId = Number(project.p.companyId);  // <-- global variable for company id

            formData.append("MediaType", mediaType);
            formData.append("UserId", userId);
            formData.append("CompanyId", companyId);

        }; // End _onDropzoneSending

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        function _onDropzoneSuccess(file, response) {
            console.log("DZ Success");

            var id = Number(project.p.companyId);

            var payload = {
                'companyId': id,
                'mediaId': response.item
            };

            var onSuccess = function (data) { console.log("update success! ", data); };
            var onError = function (data) { console.log("An error occured: ", data) };

            project.services.companyProfile.editMediaId(id, payload, onSuccess, onError);

        }; // End _onDropzoneSuccess

    } // END COMPANY PROFILE CONTROLLER
})();