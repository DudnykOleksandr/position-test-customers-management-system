angular.module('Customers').controller('TabsController', ['$scope', function ($scope) {
    var self = this;
    self.tab = 1;

    self.setTab = function (newValue) {
        self.tab = newValue;
    };

    self.isSet = function (tabName) {
        return self.tab === tabName;
    };
}]);