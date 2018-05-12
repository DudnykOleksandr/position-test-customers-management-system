angular.module('Customers').controller('ContactsController', ['guidService', 'modes', 'entityActionType', '$scope', '$log',
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
            if (!$scope.contactForm.$valid)
                return;

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