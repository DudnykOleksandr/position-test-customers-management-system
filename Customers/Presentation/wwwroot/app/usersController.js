angular.module('Customers').controller('UsersController', ['guidService', 'modes', 'entityActionType', '$scope', '$log', '$http',
    function (guidService, modes, entityActionType, $scope, $log, $http) {
        var self = this;
        var mode = null;
        self.currentEntity = null;

        self.isDetailEditVisible = function () {
            return self.currentEntity !== null;
        };
        self.isEditMode = function () {
            return mode === modes.Edit;
        };

        self.isPasswordRequired = function () {
            return self.currentEntity && self.currentEntity.ActionType === entityActionType.Add;
        };

        self.checkUniqueness = function (customer) {
            if (self.currentEntity && self.currentEntity.UserName) {
                var duplicateUserNameUser = customer.Users.find(u => u.UserName === self.currentEntity.UserName)
                if (duplicateUserNameUser)
                    $scope.userForm.userName.$setValidity('duplicate', false);
                else {
                    $http.get('Account/IsUserNameUnique',
                        {
                            params: { userName: self.currentEntity.UserName },
                            headers: {
                                'Content-Type': 'application/json;charset=utf-8;'
                            }
                        })
                        .then(function (response) {
                            $scope.userForm.userName.$setValidity('duplicate', !response.data);
                        }, function (error) {
                            $log.error(error);
                            alert("Failed");
                        });
                }
            }

        };

        self.create = function (customer) {
            mode = modes.Edit;
            var newEntity = {};
            newEntity.ActionType = entityActionType.Add;
            newEntity.CustomerId = customer.CustomerId;

            guidService.getGuid(1).then(
                function (response) {
                    newEntity.UserId = response.data[0];

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
            if (!$scope.userForm.$valid)
                return;

            var existingEntity = customer.Users.find(item => item.UserId === self.currentEntity.UserId);
            if (!existingEntity)
                customer.Users.push(self.currentEntity)
            else {
                if (existingEntity.DepartmentId !== self.currentEntity.DepartmentId)
                    self.currentEntity.IsDepartmentManager = false;
                Object.assign(existingEntity, self.currentEntity);
            }

            self.currentEntity = null;
            mode = null;
        };

        self.delete = function (entity, customer) {
            if (confirm("Delete user?")) {
                if (entity.ActionType === entityActionType.Add)
                    customer.Users = customer.Users.filter(item => item.UserId !== entity.UserId);
                else
                    entity.ActionType = entityActionType.Delete;
            }
        };

        self.discard = function () {
            mode = null;
            self.currentEntity = null;
        };
    }]);