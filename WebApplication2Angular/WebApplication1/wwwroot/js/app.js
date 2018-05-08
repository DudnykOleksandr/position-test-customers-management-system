(function () {
    var app = angular.module('customers', []);

    app.controller('CustomersController', ['$http', function ($http) {
        var self = this;

        self.customers = [];
        self.currentCustomer = null;

        self.createNewCustomer = function () {
            self.currentCustomer = new CustomerModel();
        }

        self.editCustomer = function (customer) {
            self.currentCustomer = customer;
        }

        self.saveCustomer = function () {
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

        $http.get('Customers/GetAllCustomers')
            .then(function (response) {
                self.customers = response.data;
            }, function (error) {
                alert("Failed");
            });
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
