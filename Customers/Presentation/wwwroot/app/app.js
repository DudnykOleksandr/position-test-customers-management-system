var app = angular.module('Customers', []);
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
