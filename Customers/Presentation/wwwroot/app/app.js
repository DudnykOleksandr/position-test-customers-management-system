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
            self.currentEntity = null;
            self.customerType = customerType;

            self.isDetailEditVisible = function () {
                return self.currentEntity !== null;
            };
            self.isEditMode = function () {
                return mode === modes.Edit;
            };

            self.create = function () {
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
                        self.currentEntity = newCustomer;
                    }, function (error) {
                        alert("Failed");
                    }
                );
            };

            self.showDetails = function (customer) {
                mode = modes.Details;
                self.currentEntity = customer;
            };

            self.edit = function (customer) {
                mode = modes.Edit;
                customer.ActionType = entityActionType.Update;
                customer.Address.ActionType = entityActionType.Update;

                self.currentEntity = customer;
            };

            self.save = function () {
                if (!jQuery("[name='customerForm']").get(0).reportValidity()) {
                    return;
                }

                $http.post('Customers/Save',
                    self.currentEntity,
                    {
                        headers: {
                            'Content-Type': 'application/json;charset=utf-8;'
                        }
                    }).then(function (response) {
                        self.currentEntity = null;
                        self.customers = response.data;
                    }, function (error) {
                        alert("Failed");
                    });
            };

            self.delete = function (customer) {
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

            self.discard = function () {
                mode = null;
                self.currentEntity = null;
                loadCustomers();
            };

            self.getContacts = function () {
                if (self.currentEntity)
                    return self.currentEntity.Contacts.filter(item => item.ActionType !== entityActionType.Delete);
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
        self.currentEntity = null;

        self.isDetailEditVisible = function () {
            return self.currentEntity !== null;
        };
        self.isEditMode = function () {
            return mode === modes.Edit;
        };

        self.create = function (currentEntity) {
            mode = modes.Edit;
            var newContact = new ContactModel();
            newContact.ActionType = entityActionType.Add;
            newContact.CustomerId = currentEntity.CustomerId;

            guidService.getGuid(1).then(
                function (response) {
                    newContact.ContactId = response.data[0];
                    self.currentEntity = newContact;
                }, function (error) {
                    alert("Failed");
                }
            );
        };

        self.edit = function (contact) {
            mode = modes.Edit;
            var copyOfContact = JSON.parse(JSON.stringify(contact));
            if (copyOfContact.ActionType !== entityActionType.Add)
                copyOfContact.ActionType = entityActionType.Update;

            self.currentEntity = copyOfContact;
        };

        self.save = function (customer) {
            if (!$scope.contactForm.$valid) {
                return;
            }

            var contact = customer.Contacts.find(item => item.ContactId === self.currentEntity.ContactId);
            if (!contact)
                customer.Contacts.push(self.currentEntity)
            else
                Object.assign(contact, self.currentEntity);

            self.currentEntity = null;
        };

        self.delete = function (contact, customer) {
            if (confirm("Delete contact?")) {
                if (contact.ActionType === entityActionType.Add)
                    customer.Contacts = customer.Contacts.filter(item => item.ContactId !== contact.ContactId);
                else {
                    contact.ActionType = entityActionType.Delete;
                    customer.Contacts.push({});
                    customer.Contacts.pop();
                }
            }
        };

        self.discard = function () {
            mode = null;
            self.currentEntity = null;
        };
    }]);

})();
