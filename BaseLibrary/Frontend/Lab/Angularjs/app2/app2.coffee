m = angular.module('app2', [])

@ctrl = ($scope,$window) ->
	$scope.show = ->
		$window.alert $scope.phone

m.controller('ctrl', @ctrl)