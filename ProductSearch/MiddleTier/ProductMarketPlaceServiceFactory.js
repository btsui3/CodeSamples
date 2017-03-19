
(function () {
    "use strict";

    //  APPNAME is a type of variable called a "constant" because it never changes.
    angular.module(APPNAME)
        .factory('$productMarketPlaceService', productMarketPlaceServiceFactory);

    //  manually identify dependencies for injection
    //  $sabio is a reference to sabio.page object. sabio.page is created in sabio.js
    productMarketPlaceServiceFactory.$inject = ['$baseService', '$sabio'];

    function productMarketPlaceServiceFactory($baseService, $sabio) {
        //  sabio.page has been injected as $sabio so we can reference anything that is attached to sabio.page here
        var aSabioServiceObject = sabio.services.productMarketPlace;

        //  merge the jQuery object with the angular base service to simulate inheritance
        var newService = $baseService.merge(true, {}, aSabioServiceObject, $baseService);
        //console.log('Service has passed');
        return newService;

    }
})();
