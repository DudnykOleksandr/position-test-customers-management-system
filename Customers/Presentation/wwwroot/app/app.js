(function () {
    var app = angular.module('customers', []);
    app.value('customerType', CustomerType);
    app.value('entityActionType', EntityActionType);
    app.value('userRole', UserRole);

    app.controller('CustomersController', ['$http', '$scope', 'customerType', function ($http, $scope, customerType) {
        var self = this;

        self.customers = [];
        self.currentCustomer = null;
        self.customerType = customerType;

        self.createNewCustomer = function () {
            self.currentCustomer = new CustomerModel();
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

    app.controller('ContactsController', function () {
        var self = this;

        self.currentContact = null;

        self.createNewContact = function () {
            self.currentContact = new ContactModel();
        }

        self.saveContact = function (customer) {
            customer.Contacts.push(self.currentContact);
            self.currentContact = null;
        }
    });

})();
