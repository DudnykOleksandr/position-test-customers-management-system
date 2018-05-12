angular.module('Customers').controller('UsersController', ['guidService', 'modes', 'entityActionType', '$scope', '$log',
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

        self.isPasswordRequired = function () {
            return self.currentEntity && self.currentEntity.ActionType === entityActionType.Add;
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
            if (!$scope.userForm.$valid) {
                return;
            }

            var existingEntity = customer.Users.find(item => item.UserId === self.currentEntity.UserId);
            if (!existingEntity)
                customer.Users.push(self.currentEntity)
            else
                Object.assign(existingEntity, self.currentEntity);

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