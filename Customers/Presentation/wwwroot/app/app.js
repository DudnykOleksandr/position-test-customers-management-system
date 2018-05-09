(function () {
    var app = angular.module('customers', []);
    app.value('customerType', CustomerType);
    app.value('entityActionType', EntityActionType);
    app.value('userRole', UserRole);

    app.service('guidService', ['$http', function ($http) {
        this.getGuid = function (numberOfGuidToGet) {
            return $http.get('Common/GetGuid',
                {
                    params: { numberOfGuidToGet: numberOfGuidToGet },
                    headers: { 'Content-Type': 'application/json;charset=utf-8;' }
                });
        };
    }]);

    app.controller('CustomersController', ['$http', '$scope', 'customerType', 'guidService', function ($http, $scope, customerType, guidService) {
        var self = this;

        self.customers = [];
        self.currentCustomer = null;
        self.customerType = customerType;

        self.createNewCustomer = function () {
            var newCustomer = new CustomerModel()
            newCustomer.NumberOfSchools = 0;

            guidService.getGuid(2).then(
                function (response) {
                    newCustomer.Id = response.data[0];
                    newCustomer.Type = 0;
                    newCustomer.AddressId = response.data[1];
                    newCustomer.Address = {};
                    newCustomer.Address.Id = response.data[1];
                    self.currentCustomer = newCustomer;
                }, function (error) {
                    alert("Failed");
                }
            );
        }

        self.editCustomer = function (customer) {
            self.currentCustomer = customer;
        };

        self.saveCustomer = function () {
            if (!$scope.customerForm.$valid) {
                return;
            }

            $http.post('Customers/CreateFromJson',
                self.currentCustomer,
                {
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8;'
                    }
                }).then(function (response) {
                    self.currentCustomer = null;
                    self.customers = response.data;
                }, function (error) {
                    alert("Failed");
                });
        };

        self.discardCustomerChanges = function () {
            self.currentCustomer = null;
            loadCustomers();
        };

        var loadCustomers = function () {
            $http.get('Customers/GetAllCustomers')
                .then(function (response) {
                    self.customers = response.data;
                }, function (error) {
                    alert("Failed");
                });
        };

        loadCustomers();
    }]);

    app.controller('ContactsController', ['guidService', function (guidService) {
        var self = this;

        self.currentContact = null;

        self.createNewContact = function () {
            self.currentContact = new ContactModel();
        }

        self.saveContact = function (customer) {
            customer.Contacts.push(self.currentContact);
            self.currentContact = null;
        }
    }]);

})();
