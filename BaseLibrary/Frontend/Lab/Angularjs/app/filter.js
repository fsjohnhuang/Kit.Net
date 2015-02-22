angular.module('phoneFilter', []).filter('checkmark', function () {
    return function (value) {
        return value ? '\u2713' : '\u2718';
    }
});