angular.module('phonecat', ['phoneFilter', 'phoneService']).
    config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/phones', {
            templateUrl: 'partials/phone-list.html', controller: phoneCtrl
        }).when('/phones/:phoneId', {
            templateUrl: 'partials/phone-detail.html', controller: phoneDetailCtrl
        }).otherwise({
            redirectTo: '/phones'
        });
    }]);