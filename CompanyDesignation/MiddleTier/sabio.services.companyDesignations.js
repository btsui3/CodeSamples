project.services.companyDesignations = project.services.companyDesignations || {};

project.services.companyDesignations.getURLS = function (companyDesignations) {

    var designationCodes = {
        SmallBusiness: 1, // 00001
        VeteranOwned: 2, // 00010
        MinorityOwned: 4, // 00100
        WomenOwned: 8 // 01000
    }

    var designations = {};
    

    15 // 01111

    designations.smallBusinessURL = "/Content/Theme/img/companyType/Small-Business-48-grey.png";
    designations.veteranOwnedURL = "/Content/Theme/img/companyType/Medal-48-grey.png";
    designations.minorityOwnedURL = "/Content/Theme/img/companyType/Racism-48-grey.png";
    designations.womenOwnedURL = "/Content/Theme/img/companyType/Female-48-grey.png";

    if (companyDesignations & designationCodes.SmallBusiness) {
        designations.smallBusinessURL = "/Content/Theme/img/companyType/Small-Business-48.png";

    }

    if (companyDesignations & designationCodes.VeteranOwned) {
        designations.veteranOwnedURL = "/Content/Theme/img/companyType/Medal-48.png";

    }

    if (companyDesignations & designationCodes.MinorityOwned) {
        designations.minorityOwnedURL = "/Content/Theme/img/companyType/Racism-48.png";
    }

    if (companyDesignations & designationCodes.WomenOwned) {
        designations.womenOwnedURL = "/Content/Theme/img/companyType/Female-48.png";

    }

    return designations;
}