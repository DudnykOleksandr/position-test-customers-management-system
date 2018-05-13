angular.module('Customers').controller('DepartmentsController', ['guidService', 'modes', 'entityActionType', '$scope', '$log',
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

        self.getManagerUserName = function (department, users) {
            var result = "No manager";
            var managerUser = users.find(u => u.IsDepartmentManager === true);
            if (managerUser)
                result = managerUser.UserName;
            return result;
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
                    alert("Failed to generate identifiers");
                }
            );
        };

        self.edit = function (entity, customer) {
            mode = modes.Edit;
            var copyOfEntity = JSON.parse(JSON.stringify(entity));
            if (copyOfEntity.ActionType !== entityActionType.Add)
                copyOfEntity.ActionType = entityActionType.Update;

            //select user manager
            var managerUser = customer.Users.filter(user => user.DepartmentId === entity.DepartmentId).find(user => user.IsDepartmentManager === true);
            if (managerUser)
                copyOfEntity.ManagerUserId = managerUser.UserId;

            self.currentEntity = copyOfEntity;
        };

        self.save = function (customer) {
            if (!$scope.departmentForm.$valid)
                return;

            var existingEntity = customer.Departments.find(item => item.DepartmentId === self.currentEntity.DepartmentId);
            if (!existingEntity)
                customer.Departments.push(self.currentEntity)
            else {
                Object.assign(existingEntity, self.currentEntity);

                //setting IsDepartmentManager flag
                var userManagerId = self.currentEntity.ManagerUserId;
                customer.Users.filter(user => user.DepartmentId === self.currentEntity.DepartmentId && user.IsDepartmentManager === true)
                    .forEach(user => {
                        if (userManagerId && user.UserId !== userManagerId)
                            user.IsDepartmentManager = false; user.ActionType = entityActionType.Update;
                    });

                if (userManagerId) {
                    var managerUser = customer.Users.find(user => user.UserId === userManagerId);
                    managerUser.IsDepartmentManager = true;
                    managerUser.ActionType = entityActionType.Update;
                }
            }

            self.currentEntity = null;
            mode = null;
        };

        self.delete = function (entity, customer) {
            if (confirm("Delete department?")) {
                if (entity.ActionType === entityActionType.Add)
                    customer.Departments = customer.Departments.filter(item => item.DepartmentId !== entity.DepartmentId);
                else
                    entity.ActionType = entityActionType.Delete;

                //clearing IsDepartmentManager flag
                customer.Users.filter(user => user.DepartmentId === entity.DepartmentId && user.IsDepartmentManager === true)
                    .forEach(user => { user.IsDepartmentManager = false; user.ActionType = entityActionType.Update; });
            }
        };

        self.discard = function () {
            mode = null;
            self.currentEntity = null;
        };
    }]);