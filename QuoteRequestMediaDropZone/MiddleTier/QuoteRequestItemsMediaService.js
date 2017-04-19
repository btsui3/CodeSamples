(function () {
    "use strict";

    angular.module(APPNAME)
        .factory('$QRmediaService', QRMediaServiceFactory);

    QRMediaServiceFactory.$inject = ['$baseService', '$project'];

    function QRMediaServiceFactory($baseService, $project) {

        var serviceCopy = project.services.quoteRequestMedia;

        //  merge the jQuery object with the angular base service to simulate inheritance
        var newService = $baseService.merge(true, {}, serviceCopy, $baseService);


        return newService;
    }
})();


