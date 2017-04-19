(function () {
    "use strict";

    angular.module(APPNAME)
        .factory('$companyDesignationsService', CompanyDesignationsServiceFactory);

    CompanyDesignationsServiceFactory.$inject = ['$baseService', '$project'];

    function CompanyDesignationsServiceFactory($baseService, $project) {

        var serviceCopy = project.services.companyDesignations;

        //  merge the jQuery object with the angular base service to simulate inheritance
        var newService = $baseService.merge(true, {}, serviceCopy, $baseService);

        return newService;
    }
})();