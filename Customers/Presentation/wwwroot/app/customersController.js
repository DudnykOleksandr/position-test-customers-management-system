angular.module('Customers').controller('CustomersController', ['$http', '$scope', 'customerType', 'guidService', 'entityActionType', 'modes', '$log',
    function ($http, $scope, customerType, guidService, entityActionType, modes, $log) {
        var self = this;

        var mode = null;

        var emptyUser = { UserId: '', UserName: '' };
        var emptyDepartment = { DepartmentId: '', Name: '' };

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

        self.getDepartments = function (includeEmpty) {
            var result = [];
            if (self.currentEntity) {
                result = self.currentEntity.Departments.filter(item => item.ActionType !== entityActionType.Delete);
                if (includeEmpty && includeEmpty === 'true')
                    result.unshift(emptyDepartment);
            }
            return result;
        };

        self.getUsers = function (department, includeEmpty) {
            var result = [];
            if (self.currentEntity) {
                result = self.currentEntity.Users.filter(item => item.ActionType !== entityActionType.Delete);
                if (department)
                    result = result.filter(user => user.DepartmentId === department.DepartmentId);
                if (includeEmpty && includeEmpty === 'true')
                    result.unshift(emptyUser);
            }
            return result;
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