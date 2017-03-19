//&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& MAIN CONTROLLER  &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('productMarketPlaceController', ProductMarketPlaceController); // "Method Chaining" mechanism shown here. 

    ProductMarketPlaceController.$inject = ['$scope', '$baseController', '$productMarketPlaceService', '$uibModal', "$googleMapServiceTemp", '$timeout'];

    function ProductMarketPlaceController(

        $scope
        , $baseController
        , $productMarketPlaceService
        , $uibModal
        , $googleMapServiceTemp
        , $timeout
        ) {

        //  controllers with vm syntax:
        var vm = this;  //this points to a new {}

        // services with vm syntax:
        vm.$googleMapService = $googleMapServiceTemp;
        vm.$productMarketPlaceService = $productMarketPlaceService;
        vm.$scope = $scope;
        vm.$uibModal = $uibModal;


        vm.category = 60000;
        vm.radius = 0;
        vm.zipcode = null;
        vm.latitude = 0.0;
        vm.longitude = 0.0;
        vm.searchQuery = null;
   
        //  bindable members (functions) always go up top while the "meat" of the functions go below
        vm.receiveItems = _receiveItems;
        vm.refreshProducts = _refreshProducts;
        vm.renderProducts = _renderProducts;
        vm.searchProducts = _searchProducts;
        vm.getLatLngByZip = _getLatLngByZip;
        vm.redirectToLogin = _redirectToLogin;
        vm.productCategories = _productCategories();
        vm.productRadius = _productRadius();



       
        // to simulate inheritance
        $baseController.merge(vm, $baseController);

        // wrapper for our small dependency on $scope
        vm.notify = vm.$productMarketPlaceService.getNotifier($scope);


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        $scope.$watch("prodItems", function (newValue, oldValue) {
            $timeout(function () {
                //console.log('timeout is working');
            });
        });
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _renderProducts() {          
            //  defer data operations into an external service:

            vm.$productMarketPlaceService.getProducts(vm.receiveItems, vm.onError);
            
            //console.log('Render function has passed');
            return true;
           
        };
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _receiveItems(data) {
            //console.log('recieveItems has passed', data.items);
            //this receives the data and calls the special
            //notify method that will trigger ng to refresh UI
            vm.notify(function () {
                //console.log('notify has passed');
                vm.items = data.items;
                //console.log('vm.items', vm.items);


                setTimeout(function () {
                    $(window).trigger('resize');
                    //console.log('trigger resize running');
                }, 200);


            });
            

             //Wait until the ngRepeat has completed, then initiate google maps
            setTimeout(function () {
                $('.map_display_ProductMarketPlace').each(function () {
                    var currentId = $(this).attr('id');
                    var thisMap = vm.$googleMapService.init();


                    //var thisMap = sabio.map.init();
                    thisMap.init(currentId);


                    //- apply lat long info
                    var currentLat = $(this).data('latitude');
                    var currentLng = $(this).data('longitude');
                    thisMap.geocodeLatLng(currentLat, currentLng);

                   
                });
            }, 1000);


        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _refreshProducts(data) {

            //console.log('refreshResources handler has passed');
            vm.$productMarketPlaceService.getByCategoryId(vm.receiveItems, vm.onError);

        };
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _onError(data) {
            console.log('An Error Has Occurred, data:', data);
        };
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _renderSuccess(data) {
            _receiveItems(data)

            setTimeout(function () {
                $(window).trigger('resize');
                //console.log('trigger resize running');
            }, 200);

            //console.log('render page handler has passed');
        };
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _productCategories() {

            //console.log('Product Categories has passed');
            
            // Pulling product categories and ids from master format
             vm.sampleCatalog = sabio.page.masterformat;

             vm.catCatalog = vm.sampleCatalog[0];
           
        };
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _productRadius() {

            //console.log('Product Radius has passed');
            return [
                {
                    name: "Select Radius",
                    // empty object to offset ng-chosen
                },
                {
                    radius: 5,
                    name: "5 miles"
                },
                {
                    radius: 10,
                    name: "10 miles"
                },
                {
                    radius: 20,
                    name: "20 miles"
                },
                {
                    radius: 50,
                    name: "50 miles"
                },
                 {
                     radius: 100,
                     name: "100 miles"
                 }
            ];

        };
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _searchProducts() {

            var payload = {
                categoryId: vm.category,
                searchQuery: vm.searchQuery,
                latitude: vm.latitude,
                longitude: vm.longitude,
                radius: vm.radius

            };
            vm.$productMarketPlaceService.getByCategoryId(payload, _renderSuccess, vm.onError);
  
            console.log('payload filled:', payload);
            
        };
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _getLatLngByZip() {
            var payload = {
                zipcode: vm.zipcode
            };
      
            vm.$productMarketPlaceService.getByZipCode(payload, _getLatLngSuccess, _onError);

            console.log('zipcode field invoked');
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _getLatLngSuccess(data) {
            vm.latitude = data.items[0].latitude;
            vm.longitude = data.items[0].longitude;

            console.log('lat and lng,', data.items[0].latitude, data.items[0].longitude);
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        function _redirectToLogin() {
            window.location = "http://quotemule.dev/Account/Register";
            console.log('relocation function invoked');
        };
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//

      
    }

})();
