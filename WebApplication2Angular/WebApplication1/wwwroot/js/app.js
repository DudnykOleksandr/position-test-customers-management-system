(function () {
    var app = angular.module('customers', []);

    app.controller('IndexController', ['$http', function ($http) {
        var self = this;

        self.customers = [];

        $http.get('Customers/GetAllCustomers')
            .then(function (response) {
                self.customers = response.data;
            }, function (error) {
                alert("Failed");
            });
    }]);
})();
