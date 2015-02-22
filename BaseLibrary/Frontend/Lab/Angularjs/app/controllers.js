function formCtrl($scope) {
    var data = $scope.data = {
        name: 'John',
        age: 18,
        contacts: [{
            type: 'email',
            value: 'john@hotmail.com'
        }]
    };

    $scope.num = /^\d{1,3}$/;

    $scope.addContact = function () {
        data.contacts.push({
            type: 'phone',
            value: ''
        });
    };

    $scope.delContact = function () {
        data.contacts.pop();
    };

    $scope.checkInput = function () {
        debugger;
    }

    //$scope.dragStyle = {
    //    width: '200px',
    //    height: '100px',
    //    border: 'solid 1px red'
    //};
}