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

    app.controller('CustomersController', ['$http', '$scope', 'customerType', 'guidService', 'entityActionType', 'modes', '$log',
        function ($http, $scope, customerType, guidService, entityActionType, modes, $log) {
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

                var newCustomer = {}
                newCustomer.Type = 0;
                newCustomer.Address = {};
                newCustomer.Contacts = [];
                newCustomer.Departments = [];
                newCustomer.NumberOfSchools = 0;
                newCustomer.ActionType = entityActionType.Add;

                guidService.getGuid(2).then(
                    function (response) {
                        newCustomer.CustomerId = response.data[0];
                        newCustomer.AddressId = response.data[1];
                        newCustomer.Address.AddressId = response.data[1];
                        self.currentEntity = newCustomer;
                    }, function (error) {
                        $log.error(error);
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
                        mode = null;

                        self.customers = response.data;
                    }, function (error) {
                        $log.error(error);
                        alert("Failed");
                    });
            };

            self.delete = function (customer) {
                if (confirm("Delete customer?")) {
                    $http.post('Customers/Delete',
                        customer,
                        {
                            headers: {
                                'Content-Type': 'application/json;charset=utf-8;'
                            }
                        })
                        .then(function (response) {
                            self.customers = response.data;
                        }, function (error) {
                            $log.error(error);
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

            self.getDepartments = function () {
                if (self.currentEntity)
                    return self.currentEntity.Departments.filter(item => item.ActionType !== entityActionType.Delete);
                else
                    return [];
            };

            var loadCustomers = function () {
                $http.get('Customers/GetAll')
                    .then(function (response) {
                        self.customers = response.data;
                    }, function (error) {
                        $log.error(error);
                        alert("Failed");
                    });
            };

            loadCustomers();
        }]);

    app.controller('ContactsController', ['guidService', 'modes', 'entityActionType', '$scope', '$log',
        function (guidService, modes, entityActionType, $scope, $log) {
            var self = this;
            var mode = null;
            self.currentEntity = null;

            self.isDetailEditVisible = function () {
                return self.currentEntity !== null;
            };
            self.isEditMode = function () {
                return mode === modes.Edit;
            };

            self.create = function (customer) {
                mode = modes.Edit;
                var newEntity = {};
                newEntity.ActionType = entityActionType.Add;
                newEntity.CustomerId = customer.CustomerId;

                guidService.getGuid(1).then(
                    function (response) {
                        newEntity.ContactId = response.data[0];

                        self.currentEntity = newEntity;
                    }, function (error) {
                        $log.error(error);
                        alert("Failed");
                    }
                );
            };

            self.edit = function (entity) {
                mode = modes.Edit;
                var copyOfEntity = JSON.parse(JSON.stringify(entity));
                if (copyOfEntity.ActionType !== entityActionType.Add)
                    copyOfEntity.ActionType = entityActionType.Update;

                self.currentEntity = copyOfEntity;
            };

            self.save = function (customer) {
                if (!$scope.contactForm.$valid) {
                    return;
                }

                var existingEntity = customer.Contacts.find(item => item.ContactId === self.currentEntity.ContactId);
                if (!existingEntity)
                    customer.Contacts.push(self.currentEntity)
                else
                    Object.assign(existingEntity, self.currentEntity);

                self.currentEntity = null;
                mode = null;
            };

            self.delete = function (contact, customer) {
                if (confirm("Delete contact?")) {
                    if (contact.ActionType === entityActionType.Add)
                        customer.Contacts = customer.Contacts.filter(item => item.ContactId !== contact.ContactId);
                    else
                        contact.ActionType = entityActionType.Delete;
                }
            };

            self.discard = function () {
                mode = null;
                self.currentEntity = null;
            };
        }]);

    app.controller('DepartmentsController', ['guidService', 'modes', 'entityActionType', '$scope', '$log',
        function (guidService, modes, entityActionType, $scope, $log) {
            var self = this;
            var mode = null;
            self.currentEntity = null;

            self.isDetailEditVisible = function () {
                return self.currentEntity !== null;
            };
            self.isEditMode = function () {
                return mode === modes.Edit;
            };

            self.create = function (customer) {
                mode = modes.Edit;
                var newEntity = {};
                newEntity.ActionType = entityActionType.Add;
                newEntity.CustomerId = customer.CustomerId;
                newEntity.Address = {};

                guidService.getGuid(2).then(
                    function (response) {
                        newEntity.DepartmentId = response.data[0];
                        newEntity.AddressId = response.data[1];
                        newEntity.Address.AddressId = response.data[1];

                        self.currentEntity = newEntity;
                    }, function (error) {
                        $log.error(error);
                        alert("Failed");
                    }
                );
            };

            self.edit = function (entity) {
                mode = modes.Edit;
                var copyOfEntity = JSON.parse(JSON.stringify(entity));
                if (copyOfEntity.ActionType !== entityActionType.Add)
                    copyOfEntity.ActionType = entityActionType.Update;

                self.currentEntity = copyOfEntity;
            };

            self.save = function (customer) {
                if (!$scope.departmentForm.$valid) {
                    return;
                }

                var existingEntity = customer.Departments.find(item => item.DepartmentId === self.currentEntity.DepartmentId);
                if (!existingEntity)
                    customer.Departments.push(self.currentEntity)
                else
                    Object.assign(existingEntity, self.currentEntity);

                self.currentEntity = null;
                mode = null;
            };

            self.delete = function (entity, customer) {
                if (confirm("Delete department?")) {
                    if (entity.ActionType === entityActionType.Add)
                        customer.Departments = customer.Departments.filter(item => item.DepartmentId !== entity.DepartmentId);
                    else
                        entity.ActionType = entityActionType.Delete;
                }
            };

            self.discard = function () {
                mode = null;
                self.currentEntity = null;
            };
        }]);

})();
