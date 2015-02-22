angular.module('phoneService', ['ngResource']).factory('Phone', function ($resource) {
    return $resource(':phoneId.js', {}, {
        query: {
            method: 'GET',
            params: {phoneId: 'phones'}, isArray: true
        }
    });
});