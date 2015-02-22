function phoneCtrl($scope, $http, Phone) {
    $scope.title = 'Angular.js';
    
    $scope.phones = Phone.query();
    
    $scope.orderOpt = 'name'
}

function phoneDetailCtrl($scope, $routeParams, Phone) {
    $scope.phoneId = $routeParams.phoneId;

    Phone.get({ phoneId: 'phone' + $routeParams.phoneId }, function (data) {
        $scope.phone = data;
        $scope.bigImg = data.images[0];
    });

    $scope.setImg = function (img) {
        $scope.bigImg = img;
    }
}
