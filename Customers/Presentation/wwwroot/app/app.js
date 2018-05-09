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

    app.controller('TabController', ['$scope', function ($scope) {
        var self = this;
        self.tab = 1;

        self.setTab = function (newValue) {
            self.tab = newValue;
        };

        self.isSet = function (tabName) {
            return self.tab === tabName;
        };

        self.getCustomersForm = function () {
            return $scope.customerForm;
        };
    }]);

    app.controller('CustomersController', ['$http', '$scope', 'customerType', 'guidService', 'entityActionType',
        function ($http, $scope, customerType, guidService, entityActionType) {
            var self = this;

            var mode = null;
            var modes = { Details: 0, Edit: 1 };

            self.customers = [];
            self.currentCustomer = null;
            self.customerType = customerType;

            self.isDetailEditVisible = function () {
                return self.currentCustomer != null;
            };
            self.isDetailsMode = function () {
                return mode === modes.Details;
            };
            self.isEditMode = function () {
                return mode === modes.Edit;
            };

            self.createNewCustomer = function () {
                mode = modes.Edit;

                var newCustomer = new CustomerModel()
                newCustomer.Type = 0;
                newCustomer.Address = {};
                newCustomer.NumberOfSchools = 0;
                newCustomer.ActionType = entityActionType.Add;

                    guidService.getGuid(2).then(
                    function (response) {
                        newCustomer.Id = response.data[0];
                        newCustomer.AddressId = response.data[1];
                        newCustomer.Address.Id = response.data[1];
                        self.currentCustomer = newCustomer;
                    }, function (error) {
                        alert("Failed");
                    }
                    );
            };

            self.showCustomerDetails = function (customer) {
                mode = modes.Details;
                self.currentCustomer = customer;
            };

            self.editCustomer = function (customer) {
                mode = modes.Edit;
                customer.ActionType = entityActionType.Update;
                customer.Address.ActionType = entityActionType.Update;

                self.currentCustomer = customer;
            };

            self.saveCustomer = function (customersForm) {
                if (!customersForm.$valid) {
                    return;
                }

                $http.post('Customers/Save',
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

            self.deleteCustomer = function (customer) {
                if (confirm("Delete customer?")) {
                    $http.get('Customers/Delete',
                        {
                            params: { customerId: customer.CustomerId },
                            headers: { 'Content-Type': 'application/json;charset=utf-8;' }
                        })
                        .then(function (response) {
                            self.customers = response.data;
                        }, function (error) {
                            alert("Failed");
                        });
                }
            };

            self.discardCustomerChanges = function () {
                mode = null;
                self.currentCustomer = null;
                loadCustomers();
            };

            var loadCustomers = function () {
                $http.get('Customers/GetAll')
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
