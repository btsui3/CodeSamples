(function () {

    "use strict",

    angular.module(APPNAME).controller("supplierSettingsController", SupplierSettingsController);

    SupplierSettingsController.inject = ["$scope", "$baseController", "$supplierService", "$uibModal"]

    function SupplierSettingsController($scope, $baseController, $supplierService, $uibModal)
    {


        var vm = this;


        vm.$scope = $scope
        vm.$supplierService = $supplierService;
        vm.$uibModal = $uibModal;


        $baseController.merge(vm, $baseController);


        vm.notify = vm.$supplierService.getNotifier($scope);

        // Properties 
        vm.categoryMasterList = sabio.page.masterformat;
        vm.categoryName;
        vm.items = null;
        vm.t1Name;
        vm.t2Name;
        vm.t3Name;
        vm.t4Name;

        var currentCompanyId = $("#PAGECOMPANY").val();
        var currentUserId = $("#PAGEUSER").val();


        // Methods
        vm.openAddAddressModal = _openAddAddressModal;
        vm.insertSetting = _insertSetting;
        vm.deleteSetting = _deleteSetting;
        vm.submitForm = _submitForm;


        
        


        //....// =====================================================
        function _onError(data)
        {
            console.log("Getting company addresses failed to complete.", data);
        };




        //....// =====================================================
        function _onSuccess(data)
        {
            console.log("Setting insert success: ", data)
            _getSettingsForUser();
        };





        //....// =====================================================
        function _getSettingsForUserSuccess(data)
        {

            // Create four new items in the data object for each item
            // Loop through the data.items
            if (data.items !== null && typeof data.items === 'object' && data.items.length > 0) {
                for (var i = 0; i < data.items.length; i++) {

                    //for each item, add four new properties
                    //t1Name
                    //t2name
                    //t3name
                    //t4Name

                    data.items[i].t1Name = (data.items[i].tierOneCategoryId) ? sabio.page.masterformat[data.items[i].tierOneCategoryId] : null;
                    data.items[i].t2Name = (data.items[i].tierTwoCategoryId) ? sabio.page.masterformat[data.items[i].tierTwoCategoryId] : null;
                    data.items[i].t3Name = (data.items[i].tierThreeCategoryId) ? sabio.page.masterformat[data.items[i].tierThreeCategoryId] : null;
                    data.items[i].t4Name = (data.items[i].tierFourCategoryId) ? sabio.page.masterformat[data.items[i].tierFourCategoryId] : null;

                }
            }
            vm.notify(function ()
            {
                vm.items = data.items
            })

            

        };

        


        //....// =====================================================
        function _getSettingsForUser()
        {

            vm.$supplierService.getSettingsByUserId(_getSettingsForUserSuccess, _onError);

        };




        //....// =====================================================
        function _getAddressBookByCompanyIdSuccess(data)
        {
            vm.addressList = data.items;
        };




        //....// =====================================================
        function _getAddressBookByCompanyId()
        {
            vm.$supplierService.getAddressListByCompanyId(currentCompanyId, _getAddressBookByCompanyIdSuccess, _onError)
        };




        //....// =====================================================
        function _insertSetting()
        {

            var tierCategoryObject = vm.$supplierService.categorizeTiers(vm.selectedKey);
            console.log('tiers object',tierCategoryObject);

            var payload =
                {
                      "UserId": currentUserId
                    , "CompanyId": currentCompanyId
                    , "AddressId": vm.selectedAddress
                    , "CategoryId": vm.selectedKey
                    , "CategoryName": sabio.page.masterformat[vm.selectedKey]
                    , "RangeNotification": vm.selectedRange
                    , "tierOneCategoryId": tierCategoryObject[1] 
                    , "tierTwoCategoryId": tierCategoryObject[2] 
                    , "tierThreeCategoryId": tierCategoryObject[3]
                    , "tierFourCategoryId": tierCategoryObject[4]

                };


          


            vm.$supplierService.insertSetting(payload, _onSuccess, _onError);

        };




        //....// =====================================================
        function _onDeleteSettingSuccess(data)
        {

            console.log("Setting delete confirmation: ", data)
            _getSettingsForUser();


        };




        //....// =====================================================
        function _deleteSetting(setting)
        {

            vm.$supplierService.deleteSetting(setting.supplierNotificationSettingsId, _onDeleteSettingSuccess, _onError)

        };



        //....// =====================================================
        function _openAddAddressModal()
        {

            var modalInstance = vm.$uibModal.open
            ({
                animation: true,
                templateUrl: "/Scripts/app/Dashboard/Template/AddAddressModalTemplate.html",
                controller: "addMaterialSiteModalController as msmc",
                resolve: {}

            });

            modalInstance.result.then(function ()
            {
                _getAddressBookByCompanyId();
                console.log();
            },
            function ()
            {
                console.log("Material site modal dismissed at: " + new Date());
            });

        };




        //....// =====================================================
        function _submitForm(isValid)
        {
            if (isValid)
            {
                vm.selectedKey = {};
                vm.selectedAddress = {};
                vm.selectedRange = {};


            }
            else
            {
                console.log("Form is invalid.")
            }


        };




        _getSettingsForUser();
        _getAddressBookByCompanyId();

   



    };


    })();