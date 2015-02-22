(function() {
  var m;

  m = angular.module('app2', []);

  this.ctrl = function($scope, $window) {
    return $scope.show = function() {
      return $window.alert($scope.phone);
    };
  };

  m.controller('ctrl', this.ctrl);

}).call(this);
