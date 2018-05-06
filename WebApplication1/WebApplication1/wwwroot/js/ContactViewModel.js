ContactViewModel = function () {

    var self = this;
    self.contactId = ko.observable("");
    self.name = ko.observable("");

    self.toJson = function () {
        var result = {};
        result.ContactId = self.contactId();
        result.Name = self.name();
        return result;
    };
};
