(function () {
    var app = angular.module('customers', []);
    app.value('customerType', CustomerType);
    app.value('entityActionType', EntityActionType);
    app.value('userRole', UserRole);
    app.value('modes', { Details: 0, Edit: 1 });

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
    }]);

    app.controller('CustomersController', ['$http', '$scope', 'customerType', 'guidService', 'entityActionType', 'modes',
        function ($http, $scope, customerType, guidService, entityActionType, modes) {
            var self = this;

            var mode = null;

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
                newCustomer.Contacts = [];
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

            self.saveCustomer = function () {
                if (!jQuery("[name='customerForm']").get(0).reportValidity()) {
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

            self.getContacts = function () {
                if (self.currentCustomer)
                    return self.currentCustomer.Contacts.filter(item => item.ActionType != entityActionType.Delete);
                else
                    return [];
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

    app.controller('ContactsController', ['guidService', 'modes', 'entityActionType', '$scope', function (guidService, modes, entityActionType, $scope) {
        var self = this;
        var mode = null;
        self.currentContact = null;

        self.isDetailEditVisible = function () {
            return self.currentContact != null;
        };
        self.isDetailsMode = function () {
            return mode === modes.Details;
        };
        self.isEditMode = function () {
            return mode === modes.Edit;
        };

        self.createNewContact = function (currentCustomer) {
            mode = modes.Edit;
            var newContact = new ContactModel();
            newContact.ActionType = entityActionType.Add;
            newContact.CustomerId = currentCustomer.CustomerId;

            guidService.getGuid(1).then(
                function (response) {
                    newContact.Id = response.data;
                    self.currentContact = newContact;
                }, function (error) {
                    alert("Failed");
                }
            );
        };

        self.editContact = function (contact) {
            mode = modes.Edit;
            var copyOfContact = JSON.parse(JSON.stringify(contact));
            if (copyOfContact.ActionType != entityActionType.Add)
                copyOfContact.ActionType = entityActionType.Update;

            self.currentContact = copyOfContact;
        };

        self.saveContact = function (customer) {
            if (!$scope.contactForm.$valid) {
                return;
            }

            var contact = customer.Contacts.find(item => item.ContactId === self.currentContact.ContactId);
            if (!contact)
                customer.Contacts.push(self.currentContact)
            else
                Object.assign(contact, self.currentContact);

            self.currentContact = null;
        };

        self.deleteContact = function (contact, customer) {
            if (confirm("Delete contact?")) {
                if (contact.ActionType === entityActionType.Add)
                    customer.Contacts = customer.Contacts.filter(item => item.ContactId != contact.ContactId);
                else
                    contact.ActionType === entityActionType.Delete;
            }
        };

        self.discardContactChanges = function () {
            mode = null;
            self.currentContact = null;
        };
    }]);

})();
